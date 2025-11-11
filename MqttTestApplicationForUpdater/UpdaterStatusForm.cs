using MQTTnet.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MqttTestApplicationForUpdater
{
    public partial class UpdaterStatusForm : Form
    {
        private ToolTip connectionToolTip;
        private Timer statusUpdateTimer;

        public UpdaterStatusForm()
        {
            InitializeComponent();
            // Initialize the tooltip component
            connectionToolTip = new ToolTip();
            connectionToolTip.ShowAlways = true;

            // Initialize and start the status update timer
            statusUpdateTimer = new Timer();
            statusUpdateTimer.Interval = 1000; // Update every second
            statusUpdateTimer.Tick += statusUpdateTimer_Tick;
            statusUpdateTimer.Start();
        }

        private async void NewForm_Load(object sender, EventArgs e)
        {
            // Load icon from file path (replace "your-icon.ico" with your actual filename)
            try
            {
                string iconPath = System.IO.Path.Combine(Application.StartupPath, "send-icon.ico");
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
            }
            catch (Exception ex)
            {
                // Fallback to default icon if loading fails
                this.Icon = SystemIcons.Application;
            }

            // Initialize the status before connecting
            UpdateMqttStatusDisplay();

            await MqttManager.ConnectAsync();
            await SubscribeResponse();
            textBoxPackages.Text = "[\"appName1\", \"appName2\"]";
        }

        private void statusUpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateMqttStatusDisplay();
        }

        private void UpdateMqttStatusDisplay()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpdateMqttStatusDisplay));
                return;
            }

            bool isConnected = MqttManager.IsConnected;
            bool isReconnecting = MqttManager.IsReconnecting;

            if (isReconnecting)
            {
                mqttStatusLabel.Text = "Reconnecting...";
                mqttStatusLabel.ForeColor = Color.Orange;
                mqttStatusLabel.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            }
            else if (isConnected)
            {
                mqttStatusLabel.Text = "Connected";
                mqttStatusLabel.ForeColor = Color.Green;
                mqttStatusLabel.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            }
            else
            {
                mqttStatusLabel.Text = "Disconnected";
                mqttStatusLabel.ForeColor = Color.Red;
                mqttStatusLabel.Font = new Font("Arial", 11.25F, FontStyle.Bold);
            }

            // Update tooltip with detailed connection info
            string connectionInfo = MqttManager.GetConnectionInfo();
            if (mqttStatusLabel.Tag == null || !mqttStatusLabel.Tag.Equals(connectionInfo))
            {
                mqttStatusLabel.Tag = connectionInfo;
                connectionToolTip.SetToolTip(mqttStatusLabel, connectionInfo);
            }
        }

        private async void tokenSetButton_Click(object sender, EventArgs e)
        {
            if (ConstantMessage.MqttSubscription == false)
            {
                await SubscribeResponse();
            }

            var token = textBox_Token.Text.ToString();
            JObject json = new JObject
            {
                ["replyTo"] = "security.auth",
                ["data"] = new JObject
                {
                    ["token"] = token,
                    ["header"] = "Bearer <token>",
                    ["scopes"] = new JArray
                    {
                        "updater"
                    }
                }
            };
            await MqttManager.PublishAsync("meldCX/auth/token/response", json.ToString());
        }

        private async void confirmButton_Click(object sender, EventArgs e)
        {
            if (ConstantMessage.MqttSubscription == false)
            {
                await SubscribeResponse();
            }

            var uuid = textBox_UUID.Text.ToString();
            JObject json = new JObject
            {
                ["replyTo"] = "updater.update.confirm",
                ["data"] = new JArray
                {
                    new JObject
                    {
                        ["uuid"] = uuid,
                        ["confirmed"] = true
                    }
                }
            };
            await MqttManager.PublishAsync("meldCX/updater/confirm/response", json.ToString());
        }

        private async Task SubscribeResponse()
        {
            await MqttManager.SubscribeAsync("meldCX/updater/progress", OnProgressChanged);
            await MqttManager.SubscribeAsync("meldCX/updater/confirm", NeedUserConfirmation);
            await MqttManager.SubscribeAsync("meldCX/updater/command/response", OnCommandResponseFromMultipleSource);
            await MqttManager.SubscribeAsync("meldCX/updater/log", OnLogReceived);

            ConstantMessage.MqttSubscription = true;
        }

        private Color GetBackgroundColorForCode(string code)
        {
            switch (code?.ToLower())
            {
                case "scheduled":
                    return Color.Gray;
                case "started":
                    return Color.Orange;
                case "pending":
                    return Color.Yellow;
                case "downloading":
                    return Color.LightBlue;
                case "installing":
                case "copying":
                    return Color.LightSeaGreen;
                case "download-completed":
                    return Color.Cyan;
                case "successful":
                    return Color.Lime;
                case "cancelled":
                    return Color.MediumPurple;
                case "":
                    return Color.White;
                default:
                    return Color.Red;
            }
        }

        private void UpdateTextBoxWithCodeAndColor(TextBox textBox, string data)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new Action(() =>
                {
                    textBox.Text = data;
                    textBox.BackColor = GetBackgroundColorForCode(data);
                }));
            }
            else
            {
                textBox.Text = data;
                textBox.BackColor = GetBackgroundColorForCode(data);
            }
        }

        private void ResetProgressBoxColor()
        {
            string reset = "";
            UpdateTextBoxWithCodeAndColor(textBox4, reset);
            UpdateTextBoxWithCodeAndColor(textBox5, reset);
            UpdateTextBoxWithCodeAndColor(textBox6, reset);
            UpdateTextBoxWithCodeAndColor(textBox7, reset);
            UpdateTextBoxWithCodeAndColor(textBox8, reset);
        }

        private void ResetCommonActivity(string message)
        {
            var json = JObject.Parse(message);

            var status = json["data"]?["status"]?.ToString() ?? "unknown";
            UpdateTextBoxWithCodeAndColor(textBoxOverallStatus, status);

            int appCount = json["data"]?["packageStatus"]?.Count() ?? 0;
            if (appCount >= 1)
            {
                var data = json["data"]?["packageStatus"]?[0]?["code"]?.ToString() ?? "unknown";
                UpdateTextBoxWithCodeAndColor(textBox4, data);
            }
            if (appCount >= 2)
            {
                var data = json["data"]?["packageStatus"]?[1]?["code"]?.ToString() ?? "unknown";
                UpdateTextBoxWithCodeAndColor(textBox5, data);
            }
            if (appCount >= 3)
            {
                var data = json["data"]?["packageStatus"]?[2]?["code"]?.ToString() ?? "unknown";
                UpdateTextBoxWithCodeAndColor(textBox6, data);
            }
            if (appCount >= 4)
            {
                var data = json["data"]?["packageStatus"]?[3]?["code"]?.ToString() ?? "unknown";
                UpdateTextBoxWithCodeAndColor(textBox7, data);
            }
            if (appCount >= 5)
            {
                var data = json["data"]?["packageStatus"]?[4]?["code"]?.ToString() ?? "unknown";
                UpdateTextBoxWithCodeAndColor(textBox8, data);
            }
        }

        public void OnCommandResponseFromMultipleSource(string topic, string message)
        {
            var json = JObject.Parse(message);

            if (json.ContainsKey("replyTo"))
            {
                // The replyTo key exists
                string replyTo = json["replyTo"]?.ToString();
                // Process the replyTo value

                if (replyTo == "updater.update.status")
                {
                    ReplyForUpdateStatus(message);
                }
                else if (replyTo == "updater.package.list")
                {
                    ReplyForPackageList(message);
                }
            }
        }

        private void ReplyForUpdateStatus(string message)
        {
            var json = JObject.Parse(message);

            if (textBoxStatusbyUUID.InvokeRequired)
            {
                textBoxStatusbyUUID.Invoke(new Action(() => textBoxStatusbyUUID.Text = json.ToString()));
            }
            else
            {
                textBoxStatusbyUUID.Text = json.ToString();
            }

            ResetProgressBoxColor();

            ResetCommonActivity(message);

        }

        private void ReplyForPackageList(string message)
        {
            var json = JObject.Parse(message);

            if (textBoxPackageResponse.InvokeRequired)
            {
                textBoxPackageResponse.Invoke(new Action(() => textBoxPackageResponse.Text = json.ToString()));
            }
            else
            {
                textBoxPackageResponse.Text = json.ToString();
            }

            ResetProgressBoxColor();

            ResetCommonActivity(message);

        }

        public void OnProgressChanged(string topic, string message)
        {
            var json = JObject.Parse(message);

            if (textBoxRealTimeProgress.InvokeRequired)
            {
                textBoxRealTimeProgress.Invoke(new Action(() => textBoxRealTimeProgress.Text = json.ToString()));
            }
            else
            {
                textBoxRealTimeProgress.Text = json.ToString();
            }

            ResetProgressBoxColor();

            ResetCommonActivity(message);
        }

        public void NeedUserConfirmation(string topic, string message)
        {
            var json = JObject.Parse(message);

            var uuid = json["data"]?["uuid"]?.ToString() ?? null;

            if (uuid != null)
            {
                if (textBox_UUID.InvokeRequired)
                {
                    textBox_UUID.Invoke(new Action(() => textBox_UUID.Text = uuid));
                }
                else
                {
                    textBox_UUID.Text = uuid;
                }
            }
        }

        private async void getStatusbyUUIDButton_Click(object sender, EventArgs e)
        {
            this.textBoxStatusbyUUID.Text = "";
            if (ConstantMessage.MqttSubscription == false)
            {
                await SubscribeResponse();
            }

            var uuid = textBox_UUIDForStatus.Text.ToString();
            JObject json = new JObject
            {
                ["messageCode"] = "updater.update.status",
                ["data"] = new JObject
                {
                    ["uuid"] = uuid
                }
            };
            await MqttManager.PublishAsync("meldCX/updater/command", json.ToString());
        }

        private async void getInstalledPackageListButton_Click(object sender, EventArgs e)
        {
            this.textBoxPackageResponse.Text = "";

            if (ConstantMessage.MqttSubscription == false)
            {
                await SubscribeResponse();
            }

            var json = new JObject
            {
                ["messageCode"] = "updater.package.list",
                ["data"] = new JObject
                {
                    ["packages"] = JArray.Parse(textBoxPackages.Text.ToString()),
                }
            };
            await MqttManager.PublishAsync("meldCX/updater/command", json.ToString());
        }

        public void OnLogReceived(string topic, string message)
        {
            var json = JObject.Parse(message);
            var previousText = textBoxGeneralLog.Text;
            var newText = previousText + Environment.NewLine + json.ToString();
            if (textBoxGeneralLog.InvokeRequired)
            {
                textBoxGeneralLog.Invoke(new Action(() => textBoxGeneralLog.Text = newText));
            }
            else
            {
                textBoxGeneralLog.Text = newText;
            }
        }
    }
}
