namespace MqttTestApplicationForUpdater
{
    partial class UpdaterStatusForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBoxGeneralLog = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBoxRealTimeProgress = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.textBoxStatusbyUUID = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.textBoxPackageResponse = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mqttStatusLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.getInstalledPackageListButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBoxPackages = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.getStatusbyUUIDButton = new System.Windows.Forms.Button();
            this.textBox_UUIDForStatus = new System.Windows.Forms.TextBox();
            this.textBoxOverallStatus = new System.Windows.Forms.TextBox();
            this.confirmButton = new System.Windows.Forms.Button();
            this.tokenSetButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_UUID = new System.Windows.Forms.TextBox();
            this.textBox_Token = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(-4, 13);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(510, 807);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBoxGeneralLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(502, 781);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General Log";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBoxGeneralLog
            // 
            this.textBoxGeneralLog.Location = new System.Drawing.Point(2, 2);
            this.textBoxGeneralLog.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxGeneralLog.Multiline = true;
            this.textBoxGeneralLog.Name = "textBoxGeneralLog";
            this.textBoxGeneralLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxGeneralLog.Size = new System.Drawing.Size(498, 777);
            this.textBoxGeneralLog.TabIndex = 24;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBoxRealTimeProgress);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(502, 781);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Current Update Status";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBoxRealTimeProgress
            // 
            this.textBoxRealTimeProgress.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRealTimeProgress.Location = new System.Drawing.Point(0, 0);
            this.textBoxRealTimeProgress.Multiline = true;
            this.textBoxRealTimeProgress.Name = "textBoxRealTimeProgress";
            this.textBoxRealTimeProgress.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxRealTimeProgress.Size = new System.Drawing.Size(499, 778);
            this.textBoxRealTimeProgress.TabIndex = 7;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.textBoxStatusbyUUID);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(502, 781);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Status by UUID";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // textBoxStatusbyUUID
            // 
            this.textBoxStatusbyUUID.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatusbyUUID.Location = new System.Drawing.Point(0, 0);
            this.textBoxStatusbyUUID.Multiline = true;
            this.textBoxStatusbyUUID.Name = "textBoxStatusbyUUID";
            this.textBoxStatusbyUUID.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxStatusbyUUID.Size = new System.Drawing.Size(502, 778);
            this.textBoxStatusbyUUID.TabIndex = 26;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.textBoxPackageResponse);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(502, 781);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Installed Package Version";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // textBoxPackageResponse
            // 
            this.textBoxPackageResponse.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPackageResponse.Location = new System.Drawing.Point(0, 2);
            this.textBoxPackageResponse.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPackageResponse.Multiline = true;
            this.textBoxPackageResponse.Name = "textBoxPackageResponse";
            this.textBoxPackageResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPackageResponse.Size = new System.Drawing.Size(500, 779);
            this.textBoxPackageResponse.TabIndex = 44;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(521, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 43;
            this.label5.Text = "MQTT Status:";
            // 
            // mqttStatusLabel
            // 
            this.mqttStatusLabel.AutoSize = true;
            this.mqttStatusLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mqttStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.mqttStatusLabel.Location = new System.Drawing.Point(615, 9);
            this.mqttStatusLabel.Name = "mqttStatusLabel";
            this.mqttStatusLabel.Size = new System.Drawing.Size(106, 18);
            this.mqttStatusLabel.TabIndex = 42;
            this.mqttStatusLabel.Text = "Disconnected";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(516, 269);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "Get Installed Package Version";
            // 
            // getInstalledPackageListButton
            // 
            this.getInstalledPackageListButton.Location = new System.Drawing.Point(841, 303);
            this.getInstalledPackageListButton.Name = "getInstalledPackageListButton";
            this.getInstalledPackageListButton.Size = new System.Drawing.Size(121, 40);
            this.getInstalledPackageListButton.TabIndex = 40;
            this.getInstalledPackageListButton.Text = "Get Installed Package Version";
            this.getInstalledPackageListButton.UseVisualStyleBackColor = true;
            this.getInstalledPackageListButton.Click += new System.EventHandler(this.getInstalledPackageListButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox8);
            this.panel1.Controls.Add(this.textBox7);
            this.panel1.Controls.Add(this.textBox6);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.textBox4);
            this.panel1.Location = new System.Drawing.Point(510, 551);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 205);
            this.panel1.TabIndex = 39;
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(8, 176);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(371, 20);
            this.textBox8.TabIndex = 11;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(8, 136);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(371, 20);
            this.textBox7.TabIndex = 10;
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(8, 94);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(371, 20);
            this.textBox6.TabIndex = 9;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(8, 54);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(371, 20);
            this.textBox5.TabIndex = 8;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(8, 11);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(371, 20);
            this.textBox4.TabIndex = 7;
            // 
            // textBoxPackages
            // 
            this.textBoxPackages.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPackages.Location = new System.Drawing.Point(517, 287);
            this.textBoxPackages.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPackages.Multiline = true;
            this.textBoxPackages.Name = "textBoxPackages";
            this.textBoxPackages.Size = new System.Drawing.Size(302, 68);
            this.textBoxPackages.TabIndex = 38;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(515, 183);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 37;
            this.label3.Text = "Updater Status by UUID";
            // 
            // getStatusbyUUIDButton
            // 
            this.getStatusbyUUIDButton.Location = new System.Drawing.Point(841, 202);
            this.getStatusbyUUIDButton.Margin = new System.Windows.Forms.Padding(2);
            this.getStatusbyUUIDButton.Name = "getStatusbyUUIDButton";
            this.getStatusbyUUIDButton.Size = new System.Drawing.Size(121, 40);
            this.getStatusbyUUIDButton.TabIndex = 36;
            this.getStatusbyUUIDButton.Text = "Get Status By UUID";
            this.getStatusbyUUIDButton.UseVisualStyleBackColor = true;
            this.getStatusbyUUIDButton.Click += new System.EventHandler(this.getStatusbyUUIDButton_Click);
            // 
            // textBox_UUIDForStatus
            // 
            this.textBox_UUIDForStatus.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_UUIDForStatus.Location = new System.Drawing.Point(517, 202);
            this.textBox_UUIDForStatus.Margin = new System.Windows.Forms.Padding(2);
            this.textBox_UUIDForStatus.Multiline = true;
            this.textBox_UUIDForStatus.Name = "textBox_UUIDForStatus";
            this.textBox_UUIDForStatus.Size = new System.Drawing.Size(302, 41);
            this.textBox_UUIDForStatus.TabIndex = 35;
            // 
            // textBoxOverallStatus
            // 
            this.textBoxOverallStatus.Location = new System.Drawing.Point(518, 499);
            this.textBoxOverallStatus.Multiline = true;
            this.textBoxOverallStatus.Name = "textBoxOverallStatus";
            this.textBoxOverallStatus.Size = new System.Drawing.Size(371, 38);
            this.textBoxOverallStatus.TabIndex = 34;
            // 
            // confirmButton
            // 
            this.confirmButton.Location = new System.Drawing.Point(841, 133);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(121, 40);
            this.confirmButton.TabIndex = 33;
            this.confirmButton.Text = "ConfirmBeforeInstall";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // tokenSetButton
            // 
            this.tokenSetButton.Location = new System.Drawing.Point(841, 65);
            this.tokenSetButton.Name = "tokenSetButton";
            this.tokenSetButton.Size = new System.Drawing.Size(121, 40);
            this.tokenSetButton.TabIndex = 32;
            this.tokenSetButton.Text = "Set AuthToken";
            this.tokenSetButton.UseVisualStyleBackColor = true;
            this.tokenSetButton.Click += new System.EventHandler(this.tokenSetButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(515, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "UUID for Confirmation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(520, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Auth Token";
            // 
            // textBox_UUID
            // 
            this.textBox_UUID.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_UUID.Location = new System.Drawing.Point(517, 133);
            this.textBox_UUID.Multiline = true;
            this.textBox_UUID.Name = "textBox_UUID";
            this.textBox_UUID.Size = new System.Drawing.Size(302, 39);
            this.textBox_UUID.TabIndex = 29;
            // 
            // textBox_Token
            // 
            this.textBox_Token.Font = new System.Drawing.Font("Arial", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Token.Location = new System.Drawing.Point(517, 65);
            this.textBox_Token.Multiline = true;
            this.textBox_Token.Name = "textBox_Token";
            this.textBox_Token.Size = new System.Drawing.Size(302, 40);
            this.textBox_Token.TabIndex = 28;
            // 
            // UpdaterStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1130, 814);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mqttStatusLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.getInstalledPackageListButton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBoxPackages);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.getStatusbyUUIDButton);
            this.Controls.Add(this.textBox_UUIDForStatus);
            this.Controls.Add(this.textBoxOverallStatus);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.tokenSetButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_UUID);
            this.Controls.Add(this.textBox_Token);
            this.Controls.Add(this.tabControl1);
            this.Name = "UpdaterStatusForm";
            this.Text = "UpdaterStatusForm";
            this.Load += new System.EventHandler(this.NewForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxGeneralLog;
        private System.Windows.Forms.TextBox textBoxRealTimeProgress;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox textBoxStatusbyUUID;
        private System.Windows.Forms.TextBox textBoxPackageResponse;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label mqttStatusLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button getInstalledPackageListButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBoxPackages;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button getStatusbyUUIDButton;
        private System.Windows.Forms.TextBox textBox_UUIDForStatus;
        private System.Windows.Forms.TextBox textBoxOverallStatus;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button tokenSetButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_UUID;
        private System.Windows.Forms.TextBox textBox_Token;
        private System.Windows.Forms.TabPage tabPage4;
    }
}