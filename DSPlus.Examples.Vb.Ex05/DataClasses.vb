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
Imports DSharpPlus.Entities

Public Structure BotGuild
    Public ReadOnly Property Guild As DiscordGuild

    Public ReadOnly Property Id As ULong
        Get
            Return Me.Guild.Id
        End Get
    End Property

    Public Sub New(ByVal gld As DiscordGuild)
        Me.Guild = gld
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Guild.Name
    End Function
End Structure

Public Structure BotChannel
    Public ReadOnly Property Channel As DiscordChannel

    Public ReadOnly Property Id As ULong
        Get
            Return Me.Channel.Id
        End Get
    End Property

    Public Sub New(ByVal chn As DiscordChannel)
        Me.Channel = chn
    End Sub

    Public Overrides Function ToString() As String
        Return $"#{Me.Channel.Name}"
    End Function
End Structure

Public Structure BotMessage
    Public ReadOnly Property Message As DiscordMessage

    Public ReadOnly Property Id As ULong
        Get
            Return Me.Message.Id
        End Get
    End Property

    Public Sub New(ByVal msg As DiscordMessage)
        Me.Message = msg
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Message.Content
    End Function
End Structure