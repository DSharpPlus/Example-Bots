// THIS FILE IS A PART OF EMZI0767'S BOT EXAMPLES
//
// --------
// 
// Copyright 2017 Emzi0767
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// --------
//
// This is a WinForms example. It shows how to use WinForms without deadlocks.

namespace DSPlus.Examples
{
    partial class FormBot
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
            this.gbGuilds = new System.Windows.Forms.GroupBox();
            this.lbGuilds = new System.Windows.Forms.ListBox();
            this.lbChannels = new System.Windows.Forms.ListBox();
            this.gbChannels = new System.Windows.Forms.GroupBox();
            this.gbBanter = new System.Windows.Forms.GroupBox();
            this.btBotctl = new System.Windows.Forms.Button();
            this.tbToken = new System.Windows.Forms.TextBox();
            this.lbToken = new System.Windows.Forms.Label();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.btMsgSend = new System.Windows.Forms.Button();
            this.lbBanter = new System.Windows.Forms.ListBox();
            this.gbGuilds.SuspendLayout();
            this.gbChannels.SuspendLayout();
            this.gbBanter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbGuilds
            // 
            this.gbGuilds.Controls.Add(this.lbGuilds);
            this.gbGuilds.Location = new System.Drawing.Point(12, 39);
            this.gbGuilds.Name = "gbGuilds";
            this.gbGuilds.Size = new System.Drawing.Size(175, 290);
            this.gbGuilds.TabIndex = 0;
            this.gbGuilds.TabStop = false;
            this.gbGuilds.Text = "Available guilds";
            // 
            // lbGuilds
            // 
            this.lbGuilds.FormattingEnabled = true;
            this.lbGuilds.Location = new System.Drawing.Point(6, 19);
            this.lbGuilds.Name = "lbGuilds";
            this.lbGuilds.Size = new System.Drawing.Size(163, 264);
            this.lbGuilds.TabIndex = 0;
            this.lbGuilds.SelectedValueChanged += new System.EventHandler(this.lbGuilds_SelectedValueChanged);
            // 
            // lbChannels
            // 
            this.lbChannels.FormattingEnabled = true;
            this.lbChannels.Location = new System.Drawing.Point(6, 19);
            this.lbChannels.Name = "lbChannels";
            this.lbChannels.Size = new System.Drawing.Size(163, 264);
            this.lbChannels.TabIndex = 0;
            this.lbChannels.SelectedValueChanged += new System.EventHandler(this.lbChannels_SelectedValueChanged);
            // 
            // gbChannels
            // 
            this.gbChannels.Controls.Add(this.lbChannels);
            this.gbChannels.Location = new System.Drawing.Point(193, 39);
            this.gbChannels.Name = "gbChannels";
            this.gbChannels.Size = new System.Drawing.Size(175, 290);
            this.gbChannels.TabIndex = 1;
            this.gbChannels.TabStop = false;
            this.gbChannels.Text = "Available channels";
            // 
            // gbBanter
            // 
            this.gbBanter.Controls.Add(this.lbBanter);
            this.gbBanter.Controls.Add(this.btMsgSend);
            this.gbBanter.Controls.Add(this.tbMsg);
            this.gbBanter.Location = new System.Drawing.Point(374, 39);
            this.gbBanter.Name = "gbBanter";
            this.gbBanter.Size = new System.Drawing.Size(492, 290);
            this.gbBanter.TabIndex = 2;
            this.gbBanter.TabStop = false;
            this.gbBanter.Text = "🅱anter box";
            // 
            // btBotctl
            // 
            this.btBotctl.Location = new System.Drawing.Point(791, 12);
            this.btBotctl.Name = "btBotctl";
            this.btBotctl.Size = new System.Drawing.Size(75, 23);
            this.btBotctl.TabIndex = 3;
            this.btBotctl.Text = "Start the bot";
            this.btBotctl.UseVisualStyleBackColor = true;
            this.btBotctl.Click += new System.EventHandler(this.btBotctl_Click);
            // 
            // tbToken
            // 
            this.tbToken.Location = new System.Drawing.Point(62, 14);
            this.tbToken.Name = "tbToken";
            this.tbToken.Size = new System.Drawing.Size(723, 20);
            this.tbToken.TabIndex = 4;
            this.tbToken.UseSystemPasswordChar = true;
            // 
            // lbToken
            // 
            this.lbToken.AutoSize = true;
            this.lbToken.Location = new System.Drawing.Point(15, 17);
            this.lbToken.Name = "lbToken";
            this.lbToken.Size = new System.Drawing.Size(41, 13);
            this.lbToken.TabIndex = 5;
            this.lbToken.Text = "Token:";
            // 
            // tbMsg
            // 
            this.tbMsg.Enabled = false;
            this.tbMsg.Location = new System.Drawing.Point(6, 263);
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(399, 20);
            this.tbMsg.TabIndex = 0;
            this.tbMsg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMsg_KeyPress);
            // 
            // btMsgSend
            // 
            this.btMsgSend.Enabled = false;
            this.btMsgSend.Location = new System.Drawing.Point(411, 261);
            this.btMsgSend.Name = "btMsgSend";
            this.btMsgSend.Size = new System.Drawing.Size(75, 23);
            this.btMsgSend.TabIndex = 1;
            this.btMsgSend.Text = "Send";
            this.btMsgSend.UseVisualStyleBackColor = true;
            this.btMsgSend.Click += new System.EventHandler(this.btMsgSend_Click);
            // 
            // lbBanter
            // 
            this.lbBanter.FormattingEnabled = true;
            this.lbBanter.Location = new System.Drawing.Point(6, 19);
            this.lbBanter.Name = "lbBanter";
            this.lbBanter.Size = new System.Drawing.Size(480, 238);
            this.lbBanter.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 341);
            this.Controls.Add(this.lbToken);
            this.Controls.Add(this.tbToken);
            this.Controls.Add(this.btBotctl);
            this.Controls.Add(this.gbBanter);
            this.Controls.Add(this.gbChannels);
            this.Controls.Add(this.gbGuilds);
            this.Name = "Form1";
            this.Text = "Example WinForms Bot";
            this.gbGuilds.ResumeLayout(false);
            this.gbChannels.ResumeLayout(false);
            this.gbBanter.ResumeLayout(false);
            this.gbBanter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbGuilds;
        private System.Windows.Forms.ListBox lbGuilds;
        private System.Windows.Forms.ListBox lbChannels;
        private System.Windows.Forms.GroupBox gbChannels;
        private System.Windows.Forms.GroupBox gbBanter;
        private System.Windows.Forms.ListBox lbBanter;
        private System.Windows.Forms.Button btMsgSend;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btBotctl;
        private System.Windows.Forms.TextBox tbToken;
        private System.Windows.Forms.Label lbToken;
    }
}

