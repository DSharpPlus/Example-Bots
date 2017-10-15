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
' This is a commands example. It shows how to properly utilize 
' CommandsNext, as well as use its advanced functionality.

Option Strict On
Imports System.Text
Imports DSharpPlus
Imports DSharpPlus.CommandsNext
Imports DSharpPlus.CommandsNext.Converters
Imports DSharpPlus.CommandsNext.Entities

' help formatters can alter the look of default help command,
' this particular one replaces the embed with a simple text message.
Public Class SimpleHelpFormatter : Implements IHelpFormatter
    Private Property MessageBuilder As StringBuilder

    Public Sub New()
        Me.MessageBuilder = New StringBuilder()
    End Sub

    ' this method is called first, it sets the current command's name
    ' if no command is currently being processed, it won't be called
    Public Function WithCommandName(ByVal name As String) As IHelpFormatter Implements IHelpFormatter.WithCommandName
        Me.MessageBuilder.Append("Command: ") _
            .AppendLine(Formatter.Bold(name)) _
            .AppendLine()

        Return Me
    End Function

    ' this method is called second, it sets the current command's 
    ' description. if no command is currently being processed, it 
    ' won't be called
    Public Function WithDescription(ByVal description As String) As IHelpFormatter Implements IHelpFormatter.WithDescription
        Me.MessageBuilder.Append("Description: ") _
            .AppendLine(description) _
            .AppendLine()

        Return Me
    End Function

    ' this method is called third, it is used when currently 
    ' processed group can be executed as a standalone command, 
    ' otherwise not called
    Public Function WithGroupExecutable() As IHelpFormatter Implements IHelpFormatter.WithGroupExecutable
        Me.MessageBuilder.AppendLine("This group is a standalone command.") _
            .AppendLine()

        Return Me
    End Function

    ' this method is called fourth, it sets the current command's 
    ' aliases. if no command is currently being processed, it won't
    ' be called
    Public Function WithAliases(ByVal aliases As IEnumerable(Of String)) As IHelpFormatter Implements IHelpFormatter.WithAliases
        Me.MessageBuilder.AppendLine("Aliases: ") _
            .AppendLine(String.Join(", ", aliases)) _
            .AppendLine()

        Return Me
    End Function

    ' this method is called fifth, it sets the current command's 
    ' arguments. if no command is currently being processed, it won't 
    ' be called
    Public Function WithArguments(ByVal arguments As IEnumerable(Of CommandArgument)) As IHelpFormatter Implements IHelpFormatter.WithArguments
        Me.MessageBuilder.AppendLine("Arguments: ") _
            .AppendLine(String.Join(", ", arguments.Select(Function(ByVal xarg As CommandArgument) $"{xarg.Name} ({xarg.Type.ToUserFriendlyName()})"))) _
            .AppendLine()

        Return Me
    End Function

    ' this method is called sixth, it sets the current group's subcommands
    ' if no group is being processed or current command is not a group, it 
    ' won't be called
    Public Function WithSubcommands(ByVal subcommands As IEnumerable(Of Command)) As IHelpFormatter Implements IHelpFormatter.WithSubcommands
        Me.MessageBuilder.AppendLine("Subcommands: ") _
            .AppendLine(String.Join(", ", subcommands.Select(Function(xc) xc.Name))) _
            .AppendLine()

        Return Me
    End Function

    ' this is called as the last method, this should produce the final 
    ' message, and return it
    Public Function Build() As CommandHelpMessage Implements IHelpFormatter.Build
        Return New CommandHelpMessage(content:=Me.MessageBuilder.ToString().Replace("\r\n", "\n"))
    End Function
End Class
