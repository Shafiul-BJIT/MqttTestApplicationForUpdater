using System;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
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
        //private static readonly object _lockObject = new object();

        static MqttManager()
        {
            _brokerHost = "192.168.54.241";
            //_brokerHost = "localhost";
            _brokerPort =  8883;
            _username = "sdkmeldcx";
            _password = "SDKmeldCX";
            _clientId = $"UpdaterService_{Environment.MachineName}_{Guid.NewGuid()}";
            _messageHandlers = new Dictionary<string, Action<string, string>>();
        }

        public static async Task<bool> ConnectAsync()
        {
            try
            {
                //lock (_lockObject)
                {
                    // Dispose existing connection if any
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
                }

                // Logger.LogInfo($"broker: {_brokerHost}, port: {_brokerPort}, username: {_username}, pass: {(_password != null ? "[CONFIGURED]" : "[NOT CONFIGURED]")}");

                var connectResult = await _mqttClient.ConnectAsync(_options);

                if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
                {
                    Console.WriteLine("successful");
                    // Logger.LogInfo("MQTT connected successfully");
                    return true;
                }
                else
                {
                    Console.WriteLine("error");
                    // Logger.LogError($"MQTT connection failed with code: {connectResult.ResultCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection failed!");
                // Logger.LogError($"MQTT connection error: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> SubscribeAsync(string topic, Action<string, string> messageHandler)
        {
            try
            {
                if (_mqttClient?.IsConnected == true)
                {
                    //lock (_lockObject)
                    {
                        _messageHandlers[topic] = messageHandler;
                    }

                    var subscribeOptions = new MqttTopicFilterBuilder()
                        .WithTopic(topic)
                        .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                        .Build();

                    await _mqttClient.SubscribeAsync(subscribeOptions);
                    // Logger.LogInfo($"Subscribed to topic: {topic}");
                    return true;
                }
                // Logger.LogWarning("Cannot subscribe: MQTT client is not connected");
                return false;
            }
            catch (Exception ex)
            {
                // Logger.LogError($"MQTT subscription error: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> UnsubscribeAsync(string topic)
        {
            try
            {
                if (_mqttClient?.IsConnected == true)
                {
                    //lock (_lockObject)
                    {
                        _messageHandlers.Remove(topic);
                    }
                    await _mqttClient.UnsubscribeAsync(topic);
                    // Logger.LogInfo($"Unsubscribed from topic: {topic}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                // Logger.LogError($"MQTT unsubscription error: {ex.Message}");
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
                //lock (_lockObject)
                {
                    _messageHandlers.TryGetValue(topic, out handler);
                }

                if (handler != null)
                {
                    // Execute handler in a separate task to avoid blocking the MQTT thread
                    await Task.Run(() =>
                    {
                        try
                        {
                            handler(topic, message);
                        }
                        catch (Exception ex)
                        {
                            // Logger.LogError($"Error in message handler for topic {topic}: {ex.Message}");
                        }
                    });
                }
                else
                {
                    // Logger.LogInfo($"No handler found for topic: {topic}");
                }
            }
            catch (Exception ex)
            {
                // Logger.LogError($"MQTT message processing error: {ex.Message}");
            }
        }

        private static async Task OnConnectionClosed(MqttClientDisconnectedEventArgs e)
        {
            // Logger.LogWarning("MQTT connection closed, attempting reconnect...");
            await Task.Delay(5000); // Wait 5 seconds before reconnect
            await ConnectAsync();
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
                    // Logger.LogInfo($"Published message to topic: {topic}");
                    return true;
                }
                // Logger.LogWarning($"Cannot publish to {topic}: MQTT client is not connected");
                return false;
            }
            catch (Exception ex)
            {
                // Logger.LogError($"MQTT publish error: {ex.Message}");
                return false;
            }
        }

        public static void Disconnect()
        {
            try
            {
                //lock (_lockObject)
                {
                    if (_mqttClient != null)
                    {
                        _mqttClient.ApplicationMessageReceivedAsync -= OnMessageReceived;
                        _mqttClient.DisconnectedAsync -= OnConnectionClosed;
                        _mqttClient.DisconnectAsync();
                        _mqttClient = null;
                    }
                    _messageHandlers.Clear();
                }
                // Logger.LogInfo("MQTT disconnected");
            }
            catch (Exception ex)
            {
                // Logger.LogError($"MQTT disconnect error: {ex.Message}");
            }
        }

        public static bool IsConnected
        {
            get
            {
                return _mqttClient?.IsConnected == true;
            }
        }

        public static string GetConnectionInfo()
        {
            return $"Host: {_brokerHost}, Port: {_brokerPort}, ClientId: {_clientId}, Connected: {IsConnected}";
        }

        // Method to get all subscribed topics (for debugging/monitoring)
        public static string[] GetSubscribedTopics()
        {
            //lock (_lockObject)
            {
                var topics = new string[_messageHandlers.Count];
                _messageHandlers.Keys.CopyTo(topics, 0);
                return topics;
            }
        }
    }
}