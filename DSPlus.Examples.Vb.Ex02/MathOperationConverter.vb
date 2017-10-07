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
Imports DSharpPlus.CommandsNext
Imports DSharpPlus.CommandsNext.Converters

Public Class MathOperationConverter : Implements IArgumentConverter(Of MathOperation)
    Public Function TryConvert(ByVal value As String, ByVal ctx As CommandContext, ByRef result As MathOperation) As Boolean Implements IArgumentConverter(Of MathOperation).TryConvert
        Select Case value
            Case "+"
                result = MathOperation.Add
                Return True

            Case "-"
                result = MathOperation.Subtract
                Return True

            Case "*"
                result = MathOperation.Multiply
                Return True

            Case "/"
                result = MathOperation.Divide
                Return True

            Case "%"
                result = MathOperation.Modulo
                Return True
        End Select

        result = MathOperation.Add
        Return False
    End Function
End Class
