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
    public partial class MainForm : Form
    {
        private ToolTip connectionToolTip;

        public MainForm()
        {
            InitializeComponent();
            // Initialize the tooltip component
            connectionToolTip = new ToolTip();
            connectionToolTip.ShowAlways = true;
        }

        private async void Form1_Load(object sender, EventArgs e)
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
            textBox12.Text = "[\"appName1\", \"appName2\"]";
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

            var token = textBox1.Text.ToString();
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

            var uuid = textBox2.Text.ToString();
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
            await MqttManager.SubscribeAsync("meldCX/updater/command/response", OnProgressChanged);
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

        public void OnProgressChanged(string topic, string message)
        {
            var json = JObject.Parse(message);
            if (textBox3.InvokeRequired)
            {
                textBox3.Invoke(new Action(() => textBox3.Text = json.ToString()));
            }
            else
            {
                textBox3.Text = json.ToString();
            }

            ResetProgressBoxColor();

            var status = json["data"]?["status"]?.ToString() ?? "unknown";
            UpdateTextBoxWithCodeAndColor(textBox9, status);

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

        public void NeedUserConfirmation(string topic, string message)
        {
            var json = JObject.Parse(message);
            var uuid = json["data"]?["uuid"]?.ToString() ?? null;

            if (uuid != null)
            {
                if (textBox2.InvokeRequired)
                {
                    textBox2.Invoke(new Action(() => textBox2.Text = uuid));
                }
                else
                {
                    textBox2.Text = uuid;
                }
            }
        }

        private async void getUpdaterStatusByUuidButton_Click(object sender, EventArgs e)
        {
            this.textBox3.Text = "";
            if (ConstantMessage.MqttSubscription == false)
            {
                await SubscribeResponse();
            }

            var uuid = textBox10.Text.ToString();
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

        private async void button2_Click(object sender, EventArgs e)
        {
            if (ConstantMessage.MqttSubscription == false)
            {
                await SubscribeResponse();
            }

            var json = new JObject
            {
                ["messageCode"] = "updater.package.list",
                ["data"] = new JObject
                {
                    ["packages"] = JArray.Parse(textBox12.Text.ToString())
                }
            };
            await MqttManager.PublishAsync("meldCX/updater/command", json.ToString());
        }

        public void OnLogReceived(string topic, string message)
        {
            var json = JObject.Parse(message);
            var previousText = textBox11.Text;
            var newText = previousText + Environment.NewLine + json.ToString();
            if (textBox11.InvokeRequired)
            {
                textBox11.Invoke(new Action(() => textBox11.Text = newText));
            }
            else
            {
                textBox11.Text = newText;
            }
        }
    }
}
