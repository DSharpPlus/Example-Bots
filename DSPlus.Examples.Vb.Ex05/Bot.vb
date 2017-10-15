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
Imports DSharpPlus
Imports DSharpPlus.EventArgs

' this class holds the bot itself
' this is simply used as a sort of container to keep the code organized
' and partially separated from the form
Public Class Bot
    ' the client instance, this is initialized with the class
    Public Property Client As DiscordClient

    ' this instantiates the container class and the client
    Public Sub New(ByVal token As String)
        ' create config from the supplied token
        Dim Cfg As New DiscordConfiguration
        With Cfg
            .Token = token                    ' use the supplied token
            .TokenType = TokenType.Bot        ' log in as a bot

            .AutoReconnect = True             ' reconnect automatically
            .LogLevel = LogLevel.Debug        ' log everything
            .UseInternalLogHandler = False    ' we don't want the internal output logger
        End With

        ' initialize the client
        Client = New DiscordClient(Cfg)

        ' attach our own debug logger
        AddHandler Client.DebugLogger.LogMessageReceived, AddressOf Me.DebugLogger_LogMessageReceived
    End Sub

    ' this method logs in and starts the client
    Public Function StartAsync() As Task
        Return Me.Client.ConnectAsync()
    End Function

    ' this method logs out and stops the client
    Public Function StopAsync() As Task
        Return Me.Client.DisconnectAsync()
    End Function

    ' this method writes all of bot's log messages to debug output
    Public Sub DebugLogger_LogMessageReceived(ByVal sender As Object, ByVal e As DebugLogMessageEventArgs)
        Debug.WriteLine($"[{e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")}] [{e.Application}] [{e.Level}] {e.Message}")
    End Sub
End Class
