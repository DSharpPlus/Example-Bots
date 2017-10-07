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
' This is a basic example. It shows how to set up a project and connect to 
' Discord, as well as perform some simple tasks.

Option Strict On
Imports System.IO
Imports System.Text
Imports DSharpPlus
Imports DSharpPlus.EventArgs
Imports Newtonsoft.Json

Module Program
    Sub Main(args As String())
        ' since we cannot make the entry method asynchronous,
        ' let's pass the execution to asynchronous code
        Dim Prog As New Bot()
        Prog.MainAsync(args).GetAwaiter().GetResult()
    End Sub
End Module

Class Bot
    Public WithEvents Client As DiscordClient

    Public Async Function MainAsync(args As String()) As Task
        ' first, let's load our configuration file
        Dim Json As String = ""
        Using fs As FileStream = File.OpenRead("config.json")
            Using sr As New StreamReader(fs, New UTF8Encoding(False))
                Json = Await sr.ReadToEndAsync()
            End Using
        End Using

        ' next let's load the values from that file
        ' to our client's configuration
        Dim CfgJson As ConfigJson = JsonConvert.DeserializeObject(Of ConfigJson)(Json)
        Dim Cfg As New DiscordConfiguration()
        With Cfg
            .Token = CfgJson.Token
            .TokenType = TokenType.Bot

            .AutoReconnect = True
            .LogLevel = LogLevel.Debug
            .UseInternalLogHandler = True
        End With

        ' then we want to instantiate our client
        Me.Client = New DiscordClient(Cfg)

        ' If you are on Windows 7 and using .NETFX, install 
        ' DSharpPlus.WebSocket.WebSocket4Net from NuGet,
        ' add appropriate usings, and uncomment the following
        ' line
        'Me.Client.SetWebSocketClient(Of WebSocket4NetClient)()

        ' If you are on Windows 7 and using .NET Core, install 
        ' DSharpPlus.WebSocket.WebSocket4NetCore from NuGet,
        ' add appropriate usings, and uncomment the following
        ' line
        'Me.Client.SetWebSocketClient(Of WebSocket4NetCoreClient>()

        ' If you are using Mono, install 
        ' DSharpPlus.WebSocket.WebSocketSharp from NuGet,
        ' add appropriate usings, and uncomment the following
        ' line
        'Me.Client.SetWebSocketClient(Of WebSocketSharpClient)()

        ' if using any alternate socket client implementations, 
        ' remember to add the following to the top of this file:
        'Imports DSharpPlus.Net.WebSocket

        ' finally, let's connect and log in
        Await Me.Client.ConnectAsync()

        ' and this is to prevent premature quitting
        Await Task.Delay(-1)
    End Function

    ' Let's hook up some events so we know what's going on

    Private Function Client_Ready(ByVal e As ReadyEventArgs) As Task Handles Client.Ready
        ' let's log the fact that this event occured
        e.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", "Client is ready to process events.", DateTime.Now)

        ' since this method is not async, let's return 
        ' a completed task, so that no additional work 
        ' is done
        Return Task.CompletedTask
    End Function

    Private Function Client_GuildAvailable(ByVal e As GuildCreateEventArgs) As Task Handles Client.GuildAvailable
        ' let's log the name of the guild that was just
        ' sent to our client
        e.Client.DebugLogger.LogMessage(LogLevel.Info, "ExampleBot", $"Guild available: {e.Guild.Name}", DateTime.Now)

        ' since this method is not async, let's return 
        ' a completed task, so that no additional work 
        ' is done
        Return Task.CompletedTask
    End Function

    Private Function Client_ClientError(ByVal e As ClientErrorEventArgs) As Task Handles Client.ClientErrored
        ' let's log the details of the error that just 
        ' occured in our client
        e.Client.DebugLogger.LogMessage(LogLevel.Error, "ExampleBot", $"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}", DateTime.Now)

        ' since this method is not async, let's return 
        ' a completed task, so that no additional work 
        ' is done
        Return Task.CompletedTask
    End Function
End Class

Structure ConfigJson
    <JsonProperty("token")>
    Public Property Token() As String
        Get
            Return Me._token
        End Get
        Private Set(value As String)
            Me._token = value
        End Set
    End Property

    <JsonIgnore>
    Private _token As String

    <JsonProperty("prefix")>
    Public Property CommandPrefix() As String
        Get
            Return Me._prefix
        End Get
        Private Set(value As String)
            Me._prefix = value
        End Set
    End Property

    <JsonIgnore>
    Private _prefix As String
End Structure
