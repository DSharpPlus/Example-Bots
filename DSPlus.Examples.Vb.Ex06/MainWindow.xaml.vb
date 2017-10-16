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
' This is a WPF example. It shows how to use WPF without deadlocks.

'Option Strict On
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Threading
Imports DSharpPlus
Imports DSharpPlus.EventArgs

Class MainWindow : Inherits Window : Implements INotifyPropertyChanged
    ' this property controls the title of this window
    Public Property WindowTitle As String
        Get
            Return Me._window_title
        End Get
        Set(value As String)
            Me._window_title = value
            Me.OnPropertyChanged(NameOf(Me.WindowTitle))
        End Set
    End Property
    Private _window_title As String

    ' this property will hold the text on the bot start/stop button
    Public Property ControlButtonText As String
        Get
            Return Me._ctl_btn_text
        End Get
        Set(value As String)
            Me._ctl_btn_text = value
            Me.OnPropertyChanged(NameOf(Me.ControlButtonText))
        End Set
    End Property
    Private _ctl_btn_text As String

    ' this property will hold the next message the user intends to send
    Public Property NextMessage As String
        Get
            Return Me._next_message
        End Get
        Set(value As String)
            Me._next_message = value
            Me.OnPropertyChanged(NameOf(Me.NextMessage))
        End Set
    End Property
    Private _next_message As String

    ' this property will enable or disable certain UI elements
    Public Property EnableUI As Boolean
        Get
            Return Me._enable_ui
        End Get
        Set(value As Boolean)
            Me._enable_ui = value
            Me.OnPropertyChanged(NameOf(Me.EnableUI))
        End Set
    End Property
    Private _enable_ui As Boolean

    ' this will hold the thread on which the bot will run
    Private Property BotThread As Task

    ' this will hold the bot itself
    Private Property Bot As Bot

    ' this will hold a token required to make the bot quit cleanly
    Private Property TokenSource As CancellationTokenSource

    ' these are for UI state
    Public Property SelectedGuild As BotGuild
        Get
            Return Me._selected_guild
        End Get
        Set(value As BotGuild)
            Me._selected_guild = value
            Me._selected_channel = Nothing
            Me._selected_message = Nothing
            Me.Channels.Clear()
            Me.Banter.Clear()

            If Me._selected_guild.Guild IsNot Nothing Then
                Dim chns = Me._selected_guild.Guild.Channels _
                    .Where(Function(xc) xc.Type = ChannelType.Text) _
                    .OrderBy(Function(xc) xc.Position) _
                    .Select(Function(xc) New BotChannel(xc))
                For Each xbc In chns
                    Me.Channels.Add(xbc)
                Next
            End If

            Me.OnPropertyChanged(NameOf(Me.SelectedGuild), NameOf(Me.SelectedChannel), NameOf(Me.SelectedMessage))
        End Set
    End Property
    Private _selected_guild As BotGuild

    Public Property SelectedChannel As BotChannel
        Get
            Return Me._selected_channel
        End Get
        Set(value As BotChannel)
            Me._selected_channel = value
            Me._selected_message = Nothing
            Me.Banter.Clear()
            Me.OnPropertyChanged(NameOf(Me.SelectedChannel), NameOf(Me.SelectedMessage))
        End Set
    End Property
    Private _selected_channel As BotChannel

    Public Property SelectedMessage As BotMessage
        Get
            Return Me._selected_message
        End Get
        Set(value As BotMessage)
            Me._selected_message = value
            Me.OnPropertyChanged(NameOf(Me.SelectedMessage))
        End Set
    End Property
    Private _selected_message As BotMessage

    ' these will hold the respective collections
    ' they're observable, so any changes made to them will be automatically
    ' reflected in the UI
    Public ReadOnly Property Guilds As ObservableCollection(Of BotGuild)
    Public ReadOnly Property Channels As ObservableCollection(Of BotChannel)
    Public ReadOnly Property Banter As ObservableCollection(Of BotMessage)

    Public Sub New()
        Me._window_title = "Example WPF Bot" ' set the initial title
        Me._ctl_btn_text = "Start the bot"   ' set the initial button text
        Me._next_message = ""                ' set the initial message
        Me._enable_ui = True                 ' enable the UI

        Me.Guilds = New ObservableCollection(Of BotGuild)()
        Me.Channels = New ObservableCollection(Of BotChannel)()
        Me.Banter = New ObservableCollection(Of BotMessage)()

        Me.SelectedGuild = Nothing
        Me.SelectedChannel = Nothing
        Me.SelectedMessage = Nothing

        Me.InitializeComponent()
    End Sub

    ' this occurs when user presses the send message button
    Private Sub Button_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.SendMessage()
    End Sub

    ' this occurs when user presses a button inside the message
    ' text box, we use that to handle enter key press
    Private Sub TextBox_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        ' check if the key pressed was enter
        If e.Key = Key.Enter Then
            ' if yes, mark the event as handled, and send
            ' the message
            e.Handled = True
            Me.SendMessage()
        End If
    End Sub

    ' this occurs when user presses the start/stop button
    Private Sub Button_Click_1(ByVal sender As Object, ByVal e As RoutedEventArgs)
        ' lock the controls until they can be used again
        Me.EnableUI = False

        ' check if a bot thread is running
        If Me.BotThread Is Nothing Then
            ' start the bot

            ' change the button's text to indicate it now 
            ' stops the bot instead
            Me.ControlButtonText = "Stop the bot"

            ' create the bot container
            Me.Bot = New Bot(Me.pbToken.Password)

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
            Me.ControlButtonText = "Start the bot"

            'request cancelling the task preventing the 
            ' bot from stopping
            ' this will effectively stop the bot
            Me.TokenSource.Cancel()
        End If

        ' clear the token text box, we don't need it anymore
        Me.pbToken.Password = ""
    End Sub

    Private Sub SendMessage()
        ' check if we have a channel selected, if not, do 
        ' nothing
        If Me.SelectedChannel.Channel Is Nothing Then
            Return
        End If

        Dim txt = Me.NextMessage
        Me.NextMessage = ""

        ' check if a message was typed in at all, if not,
        ' do nothing
        If String.IsNullOrWhiteSpace(txt) Then
            Return
        End If

        ' start an asynchronous task which will send the 
        ' message, and once it's done, set the message 
        ' textbox's text to empty using the UI thread
        Dim __ = Task.Run(Function() Me.BotSendMessageCallback(txt, Me.SelectedChannel))
    End Sub

    ' this method will be ran on the bot's thread
    ' it will take care of the initialization logic, as 
    ' well as actually handling the bot
    Public Async Function BotThreadCallback() As Task
        ' this will start the bot
        Await Me.Bot.StartAsync().ConfigureAwait(False)

        ' once the bot is started, we can enable the UI
        ' elements again
        Me.SetProperty(Function(x) x.EnableUI, True)

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
        Me.SetProperty(Function(x) x.EnableUI, True)
        Me.SetProperty(Function(x) x.WindowTitle, "Example WPF Bot")

        ' and reset the UI state
        Me.SetProperty(Function(x) x.SelectedGuild, Nothing)
        Me.SetProperty(Function(x) x.SelectedChannel, Nothing)
        Me.InvokeAction(New Action(AddressOf Me.Guilds.Clear))

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
        Me.SetProperty(Function(x) x.WindowTitle, "Example WPF Bot (connected)")
        Return Task.CompletedTask
    End Function

    ' called when any of the bot's guilds becomes available
    Private Function Bot_GuildAvailable(ByVal e As GuildCreateEventArgs) As Task
        ' add the guild to the bot's guild collection
        Me.InvokeAction(New Action(Of BotGuild)(AddressOf Me.AddGuild), New BotGuild(e.Guild))
        Return Task.CompletedTask
    End Function

    ' called when any of the bot joins a guild
    Private Function Bot_GuildCreated(ByVal e As GuildCreateEventArgs) As Task
        ' add the guild to the bot's guild collection
        Me.InvokeAction(New Action(Of BotGuild)(AddressOf Me.AddGuild), New BotGuild(e.Guild))
        Return Task.CompletedTask
    End Function

    ' called when any of the bot's guilds becomes unavailable
    Private Function Bot_GuildUnavailable(ByVal e As GuildDeleteEventArgs) As Task
        ' remove the guild from the bot's guild collection
        Me.InvokeAction(New Action(Of ULong)(AddressOf Me.RemoveGuild), e.Guild.Id)
        Return Task.CompletedTask
    End Function

    ' called when any of the bot leaves a guild
    Private Function Bot_GuildDeleted(ByVal e As GuildDeleteEventArgs) As Task
        ' remove the guild from the bot's guild collection
        Me.InvokeAction(New Action(Of ULong)(AddressOf Me.RemoveGuild), e.Guild.Id)
        Return Task.CompletedTask
    End Function

    ' called when the bot receives a message
    Private Function Bot_MessageCreated(ByVal e As MessageCreateEventArgs) As Task
        ' if this message is not meant for the currently 
        ' selected channel, ignore it
        If Me.SelectedChannel.Channel Is Nothing OrElse Me.SelectedChannel.Channel.Id <> e.Channel.Id Then
            Return Task.CompletedTask
        End If

        ' if it is, add it to the banter box
        Me.InvokeAction(New Action(Of BotMessage)(AddressOf Me.AddMessage), New BotMessage(e.Message))
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
        Me.Guilds.Add(gld)
    End Sub

    ' this is called when a guild is no longer available
    Private Sub RemoveGuild(ByVal id As ULong)
        Dim gld = Me.Guilds.FirstOrDefault(Function(x) x.Id = id)
        Me.Guilds.Remove(gld)
    End Sub

    ' this is called to add a message to the banter box
    Private Sub AddMessage(ByVal msg As BotMessage)
        Me.Banter.Add(msg)
        Me.SelectedMessage = msg
        Me.lbBanter.ScrollIntoView(msg)
    End Sub

    ' this is to call the PropertyChanged event
    Private Sub OnPropertyChanged(ByVal ParamArray props As String())
        For Each prop In props
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(prop))
        Next
    End Sub

    ' this will notify the UI about changes
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
