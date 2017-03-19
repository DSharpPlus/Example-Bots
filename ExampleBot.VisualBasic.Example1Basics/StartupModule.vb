Imports System.IO
Imports Newtonsoft.Json

Module StartupModule

    Sub Main()

        If Not File.Exists("config.json") Then
            Throw New FileNotFoundException("Bot's configuration file (config.json) was not found.")
        End If

        Dim Json As String = String.Empty
        Using Fs = File.OpenRead("config.json")
            Using Sr As New StreamReader(Fs)
                Json = Sr.ReadToEnd()
            End Using
        End Using

        Dim Cfg As BotConfig = JsonConvert.DeserializeObject(Of BotConfig)(Json)

        Dim Bot As New BotProgram(Cfg)
        Bot.StartAsync().GetAwaiter().GetResult()

    End Sub

End Module
