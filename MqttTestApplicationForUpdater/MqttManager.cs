using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO;
using MQTTnet;
using MQTTnet.Client;

namespace MqttTestApplicationForUpdater
{
    public static class MqttManager
    {
        private static IMqttClient _mqttClient;
        private static MqttClientOptions _options;
        private static readonly string _brokerHost;
        private static readonly int _brokerPort;
        private static readonly string _clientId;
        private static readonly string _username;
        private static readonly string _password;
        private static readonly Dictionary<string, Action<string, string>> _messageHandlers;
        private static bool _isReconnecting = false;
        private static int _reconnectAttempts = 0;
        private static readonly int[] _reconnectDelays = { 1000, 2000, 5000, 10000, 15000, 30000, 60000 }; // Progressive delays up to 60 seconds
        private static bool _shouldKeepReconnecting = true; // Flag to control continuous reconnection

        static MqttManager()
        {
            var config = ReadAppConfig();
            
            _brokerHost = config.TryGetValue("MQTT_BrokerHost", out string brokerHost) ? brokerHost : ConstantMessage.DefaultBrokerHost;
            
            if (!config.TryGetValue("MQTT_BrokerPort", out string portString) || !int.TryParse(portString, out _brokerPort))
            {
                _brokerPort = ConstantMessage.DefaultBrokerPort;
            }
            
            _username = config.TryGetValue("MQTT_Username", out string username) ? username : ConstantMessage.DefaultUsername;
            _password = config.TryGetValue("MQTT_Password", out string password) ? password : ConstantMessage.DefaultPassword;
            
            _clientId = $"UpdaterService_{Environment.MachineName}_{Guid.NewGuid()}";
            _messageHandlers = new Dictionary<string, Action<string, string>>();
        }

        private static Dictionary<string, string> ReadAppConfig()
        {
            var settings = new Dictionary<string, string>();
            
            try
            {
                string configPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.config");
                if (!File.Exists(configPath))
                {
                    // Try the executable config file name
                    configPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                }

                if (File.Exists(configPath))
                {
                    var doc = new XmlDocument();
                    doc.Load(configPath);

                    var appSettingsNode = doc.SelectSingleNode("//appSettings");
                    if (appSettingsNode != null)
                    {
                        foreach (XmlNode node in appSettingsNode.ChildNodes)
                        {
                            if (node.NodeType == XmlNodeType.Element && node.Name == "add")
                            {
                                var keyAttr = node.Attributes["key"];
                                var valueAttr = node.Attributes["value"];
                                if (keyAttr != null && valueAttr != null)
                                {
                                    settings[keyAttr.Value] = valueAttr.Value;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading config file: {ex.Message}");
            }

            return settings;
        }

        public static async Task<bool> ConnectAsync()
        {
            try
            {
                if (_mqttClient != null)
                {
                    try
                    {
                        _mqttClient.ApplicationMessageReceivedAsync -= OnMessageReceived;
                        _mqttClient.DisconnectedAsync -= OnConnectionClosed;
                    }
                    catch { /* Ignore cleanup errors */ }
                    _mqttClient = null;
                }

                var factory = new MqttFactory();
                _mqttClient = factory.CreateMqttClient();

                _options = new MqttClientOptionsBuilder()
                    .WithClientId(_clientId)
                    .WithTcpServer(_brokerHost, _brokerPort)
                    .WithCredentials(_username, _password)
                    .WithCleanSession()
                    .Build();

                _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;
                _mqttClient.DisconnectedAsync += OnConnectionClosed;

                var connectResult = await _mqttClient.ConnectAsync(_options);

                if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine("MQTT connection successful");
                    _reconnectAttempts = 0; // Reset reconnect attempts on successful connection
                    ConstantMessage.MqttSubscription = true;

                    // Resubscribe to all previously subscribed topics after reconnection
                    if (_isReconnecting && _messageHandlers.Count > 0)
                    {
                        await ResubscribeToAllTopicsAsync();
                    }

                    _isReconnecting = false;
                    return true;
                }
                else
                {
                    Console.WriteLine($"MQTT connection error: {connectResult.ResultCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT connection failed: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> SubscribeAsync(string topic, Action<string, string> messageHandler)
        {
            try
            {
                if (_mqttClient?.IsConnected == true)
                {
                    _messageHandlers[topic] = messageHandler;

                    var subscribeOptions = new MqttTopicFilterBuilder()
                        .WithTopic(topic)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                    await _mqttClient.SubscribeAsync(subscribeOptions);
                    Console.WriteLine($"Subscribed to topic: {topic}");
                    return true;
                }
                else
                {
                    _messageHandlers[topic] = messageHandler;
                    Console.WriteLine($"Cannot subscribe to {topic}: MQTT client is not connected. Handler stored for future reconnection.");
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT subscription error: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> UnsubscribeAsync(string topic)
        {
            try
            {
                if (_mqttClient?.IsConnected == true)
                {
                    _messageHandlers.Remove(topic);
                    await _mqttClient.UnsubscribeAsync(topic);
                    Console.WriteLine($"Unsubscribed from topic: {topic}");
                    return true;
                }
                else
                {
                    _messageHandlers.Remove(topic);
                    Console.WriteLine($"Handler removed for topic: {topic}");
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT unsubscription error: {ex.Message}");
                return false;
            }
        }

        private static async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string topic = e.ApplicationMessage.Topic;
                string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                Action<string, string> handler = null;
                _messageHandlers.TryGetValue(topic, out handler);

                if (handler != null)
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            handler(topic, message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error in message handler for topic {topic}: {ex.Message}");
                        }
                    });
                }
                else
                {
                    Console.WriteLine($"No handler found for topic: {topic}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT message processing error: {ex.Message}");
            }
        }

        private static async Task OnConnectionClosed(MqttClientDisconnectedEventArgs e)
        {
            ConstantMessage.MqttSubscription = false;

            if (_isReconnecting || !_shouldKeepReconnecting)
                return; // Avoid multiple reconnection attempts or if reconnection is disabled

            _isReconnecting = true;

            Console.WriteLine($"MQTT connection closed. Reason: {e.Reason}");

            // Start continuous reconnection attempts
            _ = Task.Run(async () => await AttemptReconnectionAsync());
        }

        private static async Task AttemptReconnectionAsync()
        {
            while (_isReconnecting && _shouldKeepReconnecting)
            {
                _reconnectAttempts++;

                // Use progressive delays, but cap at the maximum delay
                int delayIndex = Math.Min(_reconnectAttempts - 1, _reconnectDelays.Length - 1);
                int delay = _reconnectDelays[delayIndex];

                Console.WriteLine($"Reconnection attempt {_reconnectAttempts} in {delay / 1000} seconds...");

                await Task.Delay(delay);

                // Check if we should still keep trying
                if (!_shouldKeepReconnecting)
                {
                    Console.WriteLine("Reconnection attempts stopped by user request.");
                    _isReconnecting = false;
                    return;
                }

                try
                {
                    bool connected = await ConnectAsync();
                    if (connected)
                    {
                        Console.WriteLine($"Reconnection successful after {_reconnectAttempts} attempts!");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Reconnection attempt {_reconnectAttempts} failed: {ex.Message}");
                }

                // Log progress every 10 attempts to avoid spam
                if (_reconnectAttempts % 10 == 0)
                {
                    Console.WriteLine($"Still attempting to reconnect... ({_reconnectAttempts} attempts so far)");
                }
            }

            // This should never be reached unless _shouldKeepReconnecting is set to false
            Console.WriteLine("Reconnection attempts ended.");
            _isReconnecting = false;
        }

        private static async Task ResubscribeToAllTopicsAsync()
        {
            var topics = GetSubscribedTopics();
            if (topics.Length == 0)
                return;

            Console.WriteLine($"Resubscribing to {topics.Length} topics...");

            foreach (string topic in topics)
            {
                try
                {
                    var subscribeOptions = new MqttTopicFilterBuilder()
                        .WithTopic(topic)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                    await _mqttClient.SubscribeAsync(subscribeOptions);
                    Console.WriteLine($"Resubscribed to topic: {topic}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to resubscribe to topic {topic}: {ex.Message}");
                }
            }

            Console.WriteLine("Resubscription process completed.");
        }

        public static async Task<bool> PublishAsync(string topic, string message)
        {
            try
            {
                if (_mqttClient?.IsConnected == true)
                {
                    var applicationMessage = new MqttApplicationMessageBuilder()
                        .WithTopic(topic)
                        .WithPayload(Encoding.UTF8.GetBytes(message))
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .WithRetainFlag(false)
                        .Build();

                    await _mqttClient.PublishAsync(applicationMessage);
                    return true;
                }
                Console.WriteLine($"Cannot publish to {topic}: MQTT client is not connected");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT publish error: {ex.Message}");
                return false;
            }
        }

        public static void Disconnect()
        {
            try
            {
                _shouldKeepReconnecting = false; // Stop any ongoing reconnection attempts
                _isReconnecting = false;

                if (_mqttClient != null)
                {
                    _mqttClient.ApplicationMessageReceivedAsync -= OnMessageReceived;
                    _mqttClient.DisconnectedAsync -= OnConnectionClosed;
                    _mqttClient.DisconnectAsync();
                    _mqttClient = null;
                }
                _messageHandlers.Clear();
                Console.WriteLine("MQTT disconnected");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MQTT disconnect error: {ex.Message}");
            }
        }

        public static void StopReconnecting()
        {
            _shouldKeepReconnecting = false;
            Console.WriteLine("Reconnection attempts will be stopped.");
        }

        public static void StartReconnecting()
        {
            _shouldKeepReconnecting = true;
            if (!_isReconnecting && !IsConnected)
            {
                Console.WriteLine("Starting reconnection attempts...");
                _ = Task.Run(async () =>
                {
                    _isReconnecting = true;
                    await AttemptReconnectionAsync();
                });
            }
        }

        public static bool IsConnected
        {
            get
            {
                return _mqttClient?.IsConnected == true;
            }
        }

        public static bool IsReconnecting
        {
            get
            {
                return _isReconnecting;
            }
        }

        public static bool ShouldKeepReconnecting
        {
            get
            {
                return _shouldKeepReconnecting;
            }
        }

        public static string GetConnectionInfo()
        {
            return $"Host: {_brokerHost}, Port: {_brokerPort}, ClientId: {_clientId}, Connected: {IsConnected}, Reconnecting: {IsReconnecting}, Attempts: {_reconnectAttempts}, KeepReconnecting: {ShouldKeepReconnecting}";
        }

        public static string[] GetSubscribedTopics()
        {
            var topics = new string[_messageHandlers.Count];
            _messageHandlers.Keys.CopyTo(topics, 0);
            return topics;
        }

        public static async Task<bool> ForceReconnectAsync()
        {
            if (_isReconnecting)
                return false;

            Console.WriteLine("Manual reconnection triggered...");

            _shouldKeepReconnecting = true;
            _isReconnecting = true;
            _reconnectAttempts = 0;

            return await ConnectAsync();
        }

        public static int GetReconnectAttempts()
        {
            return _reconnectAttempts;
        }

        public static void ResetReconnectAttempts()
        {
            _reconnectAttempts = 0;
        }
    }
}