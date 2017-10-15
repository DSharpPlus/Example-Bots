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

Option Strict On
Imports System
Imports System.Threading
Imports DSharpPlus
Imports DSharpPlus.EventArgs

' the class holding the form logic
Public Class FormBot
    ' this will hold the thread on which the bot will run
    Private Property BotThread As Task

    ' this will hold the bot itself
    Private Property Bot As Bot

    ' this will hold a token required to make the bot quit cleanly
    Private Property TokenSource As CancellationTokenSource

    ' these are for UI state
    Private Property SelectedGuild As BotGuild
    Private Property SelectedChannel As BotChannel

    ' this occurs when user selects a guild from the guild box
    Private Sub lbGuilds_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbGuilds.SelectedValueChanged
        ' check if the item is a guild, if not, don't do anything
        If TypeOf lbGuilds.SelectedItem IsNot BotGuild Then
            Return
        End If
        Dim bg = DirectCast(lbGuilds.SelectedItem, BotGuild)

        ' set the UI state
        Me.SelectedGuild = bg
        Me.SelectedChannel = Nothing

        ' clear the channel and message lists
        Me.lbChannels.ClearSelected()
        Me.lbChannels.Items.Clear()
        Me.lbBanter.ClearSelected()
        Me.lbBanter.Items.Clear()

        ' populate the channel list
        Dim chns = bg.Guild.Channels _
            .Where(Function(xc) xc.Type = ChannelType.Text) _
            .OrderBy(Function(xc) xc.Position) _
            .Select(Function(xc) New BotChannel(xc))
        Me.lbChannels.Items.AddRange(chns.Cast(Of Object)().ToArray())
    End Sub

    ' this occurs when user selects a channel from the channel box
    Private Sub lbChannels_SelectedValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lbChannels.SelectedValueChanged
        ' check if the item is a channel, if not, don't do anything
        If TypeOf lbChannels.SelectedItem IsNot BotChannel Then
            Return
        End If
        Dim bc = DirectCast(lbChannels.SelectedItem, BotChannel)

        ' set the UI state
        Me.SelectedChannel = bc

        ' clear the message list
        Me.lbBanter.ClearSelected()
        Me.lbBanter.Items.Clear()
    End Sub

    ' this occurs when user presses the send message button
    Private Sub btMsgSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btMsgSend.Click
        SendMessage()
    End Sub

    ' this occurs when user presses a button inside the message
    ' text box, we use that to handle enter key press
    Private Sub tbMsg_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles tbMsg.KeyPress
        ' check if the key pressed was enter
        If e.KeyChar = ChrW(Keys.Return) Then
            ' if yes, mark the event as handled, and send
            ' the message
            e.Handled = True
            Me.SendMessage()
        End If
    End Sub

    ' this occurs when user presses the start/stop button
    Private Sub btBotctl_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btBotctl.Click
        ' lock the controls until they can be used again
        Me.btBotctl.Enabled = False
        Me.tbMsg.Enabled = False
        Me.btMsgSend.Enabled = False

        ' check if a bot thread is running
        If Me.BotThread Is Nothing Then
            ' start the bot

            ' change the button's text to indicate it now 
            ' stops the bot instead
            Me.btBotctl.Text = "Stop the bot"

            ' create the bot container
            Me.Bot = New Bot(Me.tbToken.Text)

            ' hook all the bot events
            AddHandler Me.Bot.Client.Ready, AddressOf Me.Bot_Ready
            AddHandler Me.Bot.Client.GuildAvailable, AddressOf Me.Bot_GuildAvailable
            AddHandler Me.Bot.Client.GuildCreated, AddressOf Me.Bot_GuildCreated
            AddHandler Me.Bot.Client.GuildUnavailable, AddressOf Me.Bot_GuildUnavailable
            AddHandler Me.Bot.Client.GuildDeleted, AddressOf Me.Bot_GuildDeleted
            AddHandler Me.Bot.Client.MessageCreated, AddressOf Me.Bot_MessageCreated
            AddHandler Me.Bot.Client.ClientErrored, AddressOf Me.Bot_ClientErrored

            ' create a cancellation token, this will be used 
            ' to cancel the infinite delay task
            Me.TokenSource = New CancellationTokenSource()

            ' finally, start the thread with the bot
            Me.BotThread = Task.Run(AddressOf Me.BotThreadCallback)
        Else
            ' stop the bot

            ' change the button's text to indicate it now 
            ' starts the bot instead
            Me.btBotctl.Text = "Start the bot"

            'request cancelling the task preventing the 
            ' bot from stopping
            ' this will effectively stop the bot
            Me.TokenSource.Cancel()
        End If

        ' clear the token text box, we don't need it anymore
        Me.tbToken.Text = ""
    End Sub

    ' this is called by the send button and message textbox 
    ' key press handler
    Private Sub SendMessage()
        ' check if we have a channel selected, if not, do 
        ' nothing
        If Me.SelectedChannel.Channel Is Nothing Then
            Return
        End If

        ' check if a message was typed in at all, if not,
        ' do nothing
        If String.IsNullOrWhiteSpace(Me.tbMsg.Text) Then
            Return
        End If

        ' start an asynchronous task which will send the 
        ' message, and once it's done, set the message 
        ' textbox's text to empty using the UI thread
        Dim __ = Task.Run(Function() Me.BotSendMessageCallback(Me.tbMsg.Text, Me.SelectedChannel)) _
            .ContinueWith(Sub(t) Me.tbMsg.SetProperty(Function(x) x.Text, ""))
    End Sub

    ' this method will be ran on the bot's thread
    ' it will take care of the initialization logic, as 
    ' well as actually handling the bot
    Public Async Function BotThreadCallback() As Task
        ' this will start the bot
        Await Me.Bot.StartAsync().ConfigureAwait(False)

        ' once the bot is started, we can enable the UI
        ' elements again
        Me.btBotctl.SetProperty(Function(x) x.Enabled, True)
        Me.btMsgSend.SetProperty(Function(x) x.Enabled, True)
        Me.tbMsg.SetProperty(Function(x) x.Enabled, True)

        ' here we wait indefinitely, or until the wait is
        ' cancelled
        Try
            Await Task.Delay(-1, Me.TokenSource.Token).ConfigureAwait(False)
        Catch ex As Exception
            ' ignore the exception; it's expected
        End Try

        ' this will stop the bot
        Await Me.Bot.StopAsync().ConfigureAwait(False)

        ' once the bot is stopped, we can enable the UI 
        ' elements again
        Me.btBotctl.SetProperty(Function(x) x.Enabled, True)
        Me.btMsgSend.SetProperty(Function(x) x.Enabled, True)
        Me.tbMsg.SetProperty(Function(x) x.Enabled, True)
        Me.SetProperty(Function(x) x.Text, "Example WinForms Bot")

        ' furthermore, we need to clear the listboxes
        Me.lbGuilds.InvokeAction(New Action(AddressOf Me.lbGuilds.Items.Clear))
        Me.lbChannels.InvokeAction(New Action(AddressOf Me.lbChannels.Items.Clear))
        Me.lbBanter.InvokeAction(New Action(AddressOf Me.lbBanter.Items.Clear))

        ' and reset the UI state
        Me.SelectedGuild = Nothing
        Me.SelectedChannel = Nothing

        ' and finally, dispose of our bot stuff
        Me.Bot = Nothing
        Me.TokenSource = Nothing
        Me.BotThread = Nothing
    End Function

    ' this is used by the send message method, to 
    ' asynchronously send the message
    Private Function BotSendMessageCallback(ByVal text As String, ByVal chn As BotChannel) As Task
        Return chn.Channel.SendMessageAsync(text)
    End Function

    ' this handles the bot's ready event
    Private Function Bot_Ready(ByVal e As ReadyEventArgs) As Task
        Me.SetProperty(Function(x) x.Text, "Example WinForms Bot (connected)")
        Return Task.CompletedTask
    End Function

    ' called when any of the bot's guilds becomes available
    Private Function Bot_GuildAvailable(ByVal e As GuildCreateEventArgs) As Task
        ' add the guild to the bot's guild collection
        Me.lbGuilds.InvokeAction(New Action(Of BotGuild)(AddressOf Me.AddGuild), New BotGuild(e.Guild))
        Return Task.CompletedTask
    End Function

    ' called when any of the bot joins a guild
    Private Function Bot_GuildCreated(ByVal e As GuildCreateEventArgs) As Task
        ' add the guild to the bot's guild collection
        Me.lbGuilds.InvokeAction(New Action(Of BotGuild)(AddressOf Me.AddGuild), New BotGuild(e.Guild))
        Return Task.CompletedTask
    End Function

    ' called when any of the bot's guilds becomes unavailable
    Private Function Bot_GuildUnavailable(ByVal e As GuildDeleteEventArgs) As Task
        ' remove the guild from the bot's guild collection
        Me.lbGuilds.InvokeAction(New Action(Of ULong)(AddressOf Me.RemoveGuild), e.Guild.Id)
        Return Task.CompletedTask
    End Function

    ' called when any of the bot leaves a guild
    Private Function Bot_GuildDeleted(ByVal e As GuildDeleteEventArgs) As Task
        ' remove the guild from the bot's guild collection
        Me.lbGuilds.InvokeAction(New Action(Of ULong)(AddressOf Me.RemoveGuild), e.Guild.Id)
        Return Task.CompletedTask
    End Function

    ' called when the bot receives a message
    Private Function Bot_MessageCreated(ByVal e As MessageCreateEventArgs) As Task
        ' if this message is not meant for the currently 
        ' selected channel, ignore it
        If Me.SelectedChannel.Channel?.Id <> e.Channel.Id Then
            Return Task.CompletedTask
        End If

        ' if it is, add it to the banter box
        Me.lbBanter.InvokeAction(New Action(Of BotMessage)(AddressOf Me.AddMessage), New BotMessage(e.Message))
        Return Task.CompletedTask
    End Function

    ' called when an unhandled exception occurs in any of the 
    ' event handlers
    Private Function Bot_ClientErrored(ByVal e As ClientErrorEventArgs) As Task
        ' show a message box by dispatching it to the UI thread
        Me.InvokeAction(New Action(Sub() MsgBox($"Exception in {e.EventName}: {e.Exception.ToString()}", MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Unhandled exception in the bot")))
        Return Task.CompletedTask
    End Function

    ' this is called when a new guild becomes available
    Private Sub AddGuild(ByVal gld As BotGuild)
        Me.lbGuilds.Items.Add(gld)
    End Sub

    ' this is called when a guild is no longer available
    Private Sub RemoveGuild(ByVal id As ULong)
        Dim gld = Me.lbGuilds.Items.OfType(Of BotGuild)().FirstOrDefault(Function(x) x.Id = id)
        Me.lbGuilds.Items.Remove(gld)
    End Sub

    ' this is called to add a message to the banter box
    Private Sub AddMessage(ByVal msg As BotMessage)
        Me.lbBanter.Items.Add(msg)
        Me.lbBanter.SelectedItem = msg
    End Sub
End Class
