Imports DSharpPlus

Public Class BotProgram

    Private Property Configuration As BotConfig
    Private WithEvents Client As DiscordClient
    Private WithEvents ClientLogger As DebugLogger

    Public Sub New(ByVal config As BotConfig)

        Me.Configuration = config

    End Sub

    Public Async Function StartAsync() As Task

        Dim dcfg As New DiscordConfig
        With dcfg
            .Token = Me.Configuration.Token
            .TokenType = TokenType.Bot
            .LogLevel = LogLevel.Debug
            .AutoReconnect = True
        End With

        Me.Client = New DiscordClient(dcfg)
        Me.ClientLogger = Me.Client.DebugLogger

        Await Me.Client.Connect()
        Await Task.Delay(-1)

    End Function

    Private Sub LogReceived(ByVal sender As Object, ByVal e As DebugLogMessageEventArgs) Handles ClientLogger.LogMessageReceived

        Console.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] [{1}] {2}", e.TimeStamp, e.Level, e.Message)

    End Sub

    Private Async Function OnMessage(ByVal e As MessageCreateEventArgs) As Task Handles Client.MessageCreated

        If e.Message.Content.ToLower() = "ping" Then
            Await e.Channel.SendMessage("pong")
        End If

    End Function

End Class
