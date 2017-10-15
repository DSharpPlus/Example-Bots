' THIS FILE IS A PART OF EMZI0767'S BOT EXAMPLES
'
' --------
' 
' Copyright 2017 Emzi0767
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
' 
'  http://www.apache.org/licenses/LICENSE-2.0
' 
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
'
' --------
'
' This is a WinForms example. It shows how to use WinForms without deadlocks.

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormBot
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lbToken = New System.Windows.Forms.Label()
        Me.gbGuilds = New System.Windows.Forms.GroupBox()
        Me.lbGuilds = New System.Windows.Forms.ListBox()
        Me.lbChannels = New System.Windows.Forms.ListBox()
        Me.gbBanter = New System.Windows.Forms.GroupBox()
        Me.lbBanter = New System.Windows.Forms.ListBox()
        Me.btMsgSend = New System.Windows.Forms.Button()
        Me.tbMsg = New System.Windows.Forms.TextBox()
        Me.gbChannels = New System.Windows.Forms.GroupBox()
        Me.tbToken = New System.Windows.Forms.TextBox()
        Me.btBotctl = New System.Windows.Forms.Button()
        Me.gbGuilds.SuspendLayout()
        Me.gbBanter.SuspendLayout()
        Me.gbChannels.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbToken
        '
        Me.lbToken.AutoSize = True
        Me.lbToken.Location = New System.Drawing.Point(15, 17)
        Me.lbToken.Name = "lbToken"
        Me.lbToken.Size = New System.Drawing.Size(41, 13)
        Me.lbToken.TabIndex = 11
        Me.lbToken.Text = "Token:"
        '
        'gbGuilds
        '
        Me.gbGuilds.Controls.Add(Me.lbGuilds)
        Me.gbGuilds.Location = New System.Drawing.Point(12, 39)
        Me.gbGuilds.Name = "gbGuilds"
        Me.gbGuilds.Size = New System.Drawing.Size(175, 290)
        Me.gbGuilds.TabIndex = 6
        Me.gbGuilds.TabStop = False
        Me.gbGuilds.Text = "Available guilds"
        '
        'lbGuilds
        '
        Me.lbGuilds.FormattingEnabled = True
        Me.lbGuilds.Location = New System.Drawing.Point(6, 19)
        Me.lbGuilds.Name = "lbGuilds"
        Me.lbGuilds.Size = New System.Drawing.Size(163, 264)
        Me.lbGuilds.TabIndex = 0
        '
        'lbChannels
        '
        Me.lbChannels.FormattingEnabled = True
        Me.lbChannels.Location = New System.Drawing.Point(6, 19)
        Me.lbChannels.Name = "lbChannels"
        Me.lbChannels.Size = New System.Drawing.Size(163, 264)
        Me.lbChannels.TabIndex = 0
        '
        'gbBanter
        '
        Me.gbBanter.Controls.Add(Me.lbBanter)
        Me.gbBanter.Controls.Add(Me.btMsgSend)
        Me.gbBanter.Controls.Add(Me.tbMsg)
        Me.gbBanter.Location = New System.Drawing.Point(374, 39)
        Me.gbBanter.Name = "gbBanter"
        Me.gbBanter.Size = New System.Drawing.Size(492, 290)
        Me.gbBanter.TabIndex = 8
        Me.gbBanter.TabStop = False
        Me.gbBanter.Text = "🅱anter box"
        '
        'lbBanter
        '
        Me.lbBanter.FormattingEnabled = True
        Me.lbBanter.Location = New System.Drawing.Point(6, 19)
        Me.lbBanter.Name = "lbBanter"
        Me.lbBanter.Size = New System.Drawing.Size(480, 238)
        Me.lbBanter.TabIndex = 2
        '
        'btMsgSend
        '
        Me.btMsgSend.Enabled = False
        Me.btMsgSend.Location = New System.Drawing.Point(411, 261)
        Me.btMsgSend.Name = "btMsgSend"
        Me.btMsgSend.Size = New System.Drawing.Size(75, 23)
        Me.btMsgSend.TabIndex = 1
        Me.btMsgSend.Text = "Send"
        Me.btMsgSend.UseVisualStyleBackColor = True
        '
        'tbMsg
        '
        Me.tbMsg.Enabled = False
        Me.tbMsg.Location = New System.Drawing.Point(6, 263)
        Me.tbMsg.Name = "tbMsg"
        Me.tbMsg.Size = New System.Drawing.Size(399, 20)
        Me.tbMsg.TabIndex = 0
        '
        'gbChannels
        '
        Me.gbChannels.Controls.Add(Me.lbChannels)
        Me.gbChannels.Location = New System.Drawing.Point(193, 39)
        Me.gbChannels.Name = "gbChannels"
        Me.gbChannels.Size = New System.Drawing.Size(175, 290)
        Me.gbChannels.TabIndex = 7
        Me.gbChannels.TabStop = False
        Me.gbChannels.Text = "Available channels"
        '
        'tbToken
        '
        Me.tbToken.Location = New System.Drawing.Point(62, 14)
        Me.tbToken.Name = "tbToken"
        Me.tbToken.Size = New System.Drawing.Size(723, 20)
        Me.tbToken.TabIndex = 10
        Me.tbToken.UseSystemPasswordChar = True
        '
        'btBotctl
        '
        Me.btBotctl.Location = New System.Drawing.Point(791, 12)
        Me.btBotctl.Name = "btBotctl"
        Me.btBotctl.Size = New System.Drawing.Size(75, 23)
        Me.btBotctl.TabIndex = 9
        Me.btBotctl.Text = "Start the bot"
        Me.btBotctl.UseVisualStyleBackColor = True
        '
        'FormBot
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(878, 341)
        Me.Controls.Add(Me.lbToken)
        Me.Controls.Add(Me.gbGuilds)
        Me.Controls.Add(Me.gbBanter)
        Me.Controls.Add(Me.gbChannels)
        Me.Controls.Add(Me.tbToken)
        Me.Controls.Add(Me.btBotctl)
        Me.Name = "FormBot"
        Me.Text = "Example WinForms Bot"
        Me.gbGuilds.ResumeLayout(False)
        Me.gbBanter.ResumeLayout(False)
        Me.gbBanter.PerformLayout()
        Me.gbChannels.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents lbToken As Label
    Private WithEvents gbGuilds As GroupBox
    Private WithEvents lbGuilds As ListBox
    Private WithEvents lbChannels As ListBox
    Private WithEvents gbBanter As GroupBox
    Private WithEvents lbBanter As ListBox
    Private WithEvents btMsgSend As Button
    Private WithEvents tbMsg As TextBox
    Private WithEvents gbChannels As GroupBox
    Private WithEvents tbToken As TextBox
    Private WithEvents btBotctl As Button
End Class
