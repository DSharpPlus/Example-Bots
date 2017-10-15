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
' This is a voice example. It shows how to properly utilize VoiceNext.

Option Strict On
Imports System.IO
Imports DSharpPlus.CommandsNext
Imports DSharpPlus.CommandsNext.Attributes
Imports DSharpPlus.Entities
Imports DSharpPlus.VoiceNext

Public Class ExampleVoiceCommands
    <Command("join"), Description("Joins a voice channel.")>
    Public Async Function Join(ByVal ctx As CommandContext, Optional ByVal chn As DiscordChannel = Nothing) As Task
        ' check whether VNext is enabled
        Dim vnext = ctx.Client.GetVoiceNextClient()
        If vnext Is Nothing Then
            ' not enabled
            Await ctx.RespondAsync("VNext is not enabled or configured.")
            Return
        End If

        ' check whether we aren't already connected
        Dim vnc = vnext.GetConnection(ctx.Guild)
        If vnc IsNot Nothing Then
            ' already connected
            Await ctx.RespondAsync("Already connected in this guild.")
            Return
        End If

        ' get member's voice state
        Dim vstat = ctx.Member?.VoiceState
        If vstat?.Channel Is Nothing AndAlso chn Is Nothing Then
            ' they did not specify a channel and are not in one
            Await ctx.RespondAsync("You are not in a voice channel.")
            Return
        End If

        ' channel not specified, use user's
        If chn Is Nothing Then
            chn = vstat.Channel
        End If

        ' connect
        vnc = Await vnext.ConnectAsync(chn)
        Await ctx.RespondAsync($"Connected to `{chn.Name}`")
    End Function

    <Command("leave"), Description("Leaves a voice channel.")>
    Public Async Function Leave(ByVal ctx As CommandContext) As Task
        ' check whether VNext is enabled
        Dim vnext = ctx.Client.GetVoiceNextClient()
        If vnext Is Nothing Then
            ' not enabled
            Await ctx.RespondAsync("VNext is not enabled or configured.")
            Return
        End If

        ' check whether we are connected
        Dim vnc = vnext.GetConnection(ctx.Guild)
        If vnc Is Nothing Then
            ' not connected
            Await ctx.RespondAsync("Not connected in this guild.")
            Return
        End If

        ' disconnect
        vnc.Disconnect()
        Await ctx.RespondAsync("Disconnected")
    End Function

    <Command("play"), Description("Plays an audio file.")>
    Public Async Function Play(ByVal ctx As CommandContext, <RemainingText, Description("Full path to the file to play.")> ByVal filename As String) As Task
        ' check whether VNext is enabled
        Dim vnext = ctx.Client.GetVoiceNextClient()
        If vnext Is Nothing Then
            ' not enabled
            Await ctx.RespondAsync("VNext is not enabled or configured.")
            Return
        End If

        ' check whether we are connected
        Dim vnc = vnext.GetConnection(ctx.Guild)
        If vnc Is Nothing Then
            ' not connected
            Await ctx.RespondAsync("Not connected in this guild.")
            Return
        End If

        ' check if file exists
        If Not File.Exists(filename) Then
            ' file does not exist
            Await ctx.RespondAsync($"File `{filename}` does not exist.")
            Return
        End If

        ' wait for current playback to finish
        While vnc.IsPlaying
            Await vnc.WaitForPlaybackFinishAsync()
        End While

        ' play
        Dim exc As Exception = Nothing
        Await ctx.Message.RespondAsync($"Playing `{filename}`")
        Await vnc.SendSpeakingAsync(True)
        Try
            ' borrowed from
            ' https://github.com/RogueException/Discord.Net/blob/5ade1e387bb8ea808a9d858328e2d3db23fe0663/docs/guides/voice/samples/audio_create_ffmpeg.cs
            ' translated to VB.NET by Emzi0767

            Dim ffmpeg_inf As New ProcessStartInfo()
            With ffmpeg_inf
                .FileName = "ffmpeg"
                .Arguments = $"-i ""{filename}"" -ac 2 -f s16le -ar 48000 pipe:1"
                .UseShellExecute = False
                .RedirectStandardOutput = True
                .RedirectStandardError = True
            End With
            Dim ffmpeg = Process.Start(ffmpeg_inf)
            Dim ffout = ffmpeg.StandardOutput.BaseStream

            ' let's buffer ffmpeg output
            Using ms As New MemoryStream()
                Await ffout.CopyToAsync(ms)
                ms.Position = 0

                Dim buff(3839) As Byte ' buffer to hold the PCM data
                Dim br = 1
                While True
                    br = ms.Read(buff, 0, buff.Length)
                    If br = 0 Then
                        Exit While
                    End If

                    If br < buff.Length Then ' it's possible we got less than expected, let's null the remaining part of the buffer
                        For i = br To buff.Length - 1
                            buff(i) = 0
                        Next
                    End If

                    Await vnc.SendAsync(buff, 20) ' we're sending 20ms of data
                End While
            End Using
        Catch ex As Exception
            exc = ex
        End Try

        Await vnc.SendSpeakingAsync(False)
        If exc IsNot Nothing Then
            Await ctx.RespondAsync($"An exception occured during playback: `{exc.GetType()}: {exc.Message}`")
        End If
    End Function
End Class
