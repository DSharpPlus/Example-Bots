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
Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Delegate Sub SetPropertyDelegate(Of TCtl As Control, TProp)(ByVal control As TCtl, ByVal propexpr As Expression(Of Func(Of TCtl, TProp)), ByVal value As TProp)
Public Delegate Function GetPropertyDelegate(Of TCtl As Control, TProp)(ByVal control As TCtl, ByVal propexpr As Expression(Of Func(Of TCtl, TProp))) As TProp
Public Delegate Sub InvokeActionDelegate(Of TCtl As Control)(ByVal control As TCtl, ByVal dlg As [Delegate], ByVal args As Object())
Public Delegate Function InvokeFuncDelegate(Of TCtl As Control, TResult)(ByVal control As TCtl, ByVal dlg As [Delegate], ByVal args As Object()) As TResult

Module Extensions
    ' this method modifies specified property, assigning it the given value
    ' usage is control.SetProperty(Function(x) x.Property, value)
    <Extension>
    Public Sub SetProperty(Of TCtl As Control, TProp)(ByVal control As TCtl, ByVal propexpr As Expression(Of Func(Of TCtl, TProp)), ByVal value As TProp)
        ' check for nulls
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If propexpr Is Nothing Then
            Throw New ArgumentNullException(NameOf(propexpr))
        End If

        ' check if cross-thread invocation is required
        If control.InvokeRequired Then
            ' if it is, perform it
            ' this will invoke this method from (hopefully) the control's thread
            control.Invoke(New SetPropertyDelegate(Of TCtl, TProp)(AddressOf SetProperty), control, propexpr, value)
            Return
        End If

        ' get the body of the expression, check if it's an expression that 
        ' results in a class member being passed
        Dim propexprm = TryCast(propexpr.Body, MemberExpression)
        If propexprm Is Nothing Then
            Throw New ArgumentException("Invalid member expression.", NameOf(propexpr))
        End If

        ' get the member from the expression body, and check if it's a property
        Dim prop = TryCast(propexprm.Member, PropertyInfo)
        If prop Is Nothing Then
            Throw New ArgumentException("Invalid property supplied.", NameOf(propexpr))
        End If

        ' finally, set the value of the property to the supplied one
        prop.SetValue(control, value)
    End Sub

    ' this method reads the value of specified property, and returns it
    ' usage is control.GetProperty(Function(x) x.Property)
    <Extension>
    Public Function GetProperty(Of TCtl As Control, TProp)(ByVal control As TCtl, ByVal propexpr As Expression(Of Func(Of TCtl, TProp))) As TProp
        ' check for nulls
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If propexpr Is Nothing Then
            Throw New ArgumentNullException(NameOf(propexpr))
        End If

        ' check if cross-thread invocation is required
        If control.InvokeRequired Then
            ' if it is, perform it
            ' this will invoke this method from (hopefully) the control's thread
            Return DirectCast(control.Invoke(New GetPropertyDelegate(Of TCtl, TProp)(AddressOf GetProperty), control, propexpr), TProp)
        End If

        ' get the body of the expression, check if it's an expression that 
        ' results in a class member being passed
        Dim propexprm = TryCast(propexpr.Body, MemberExpression)
        If propexprm Is Nothing Then
            Throw New ArgumentException("Invalid member expression.", NameOf(propexpr))
        End If

        ' get the member from the expression body, and check if it's a property
        Dim prop = TryCast(propexprm.Member, PropertyInfo)
        If prop Is Nothing Then
            Throw New ArgumentException("Invalid property supplied.", NameOf(propexpr))
        End If

        ' finally, set the value of the property to the supplied one
        Return DirectCast(prop.GetValue(control), TProp)
    End Function

    ' this method invokes a return-less method for given control
    ' usage is control.InvokeAction(new Action(Of T1, T2, ...)(method), arg1, arg2, ...)
    <Extension>
    Public Sub InvokeAction(Of TCtl As Control)(ByVal control As TCtl, ByVal dlg As [Delegate], ByVal ParamArray args As Object())
        ' check for nulls
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If dlg Is Nothing Then
            Throw New ArgumentNullException(NameOf(dlg))
        End If

        ' check if cross-thread invocation is required
        If control.InvokeRequired Then
            ' if it is, perform it
            ' this will invoke this method from (hopefully) the control's thread
            control.Invoke(New InvokeActionDelegate(Of TCtl)(AddressOf InvokeAction), control, dlg, args)
            Return
        End If

        ' finally, call the passed delegate, with supplied arguments
        dlg.DynamicInvoke(args)
    End Sub

    ' this method invokes a method which returns for given control, the returned value is returned to the caller
    ' usage is control.InvokeAction(Of TReturn)(new Func(Of T1, T2, ..., TReturn)(method), arg1, arg2, ...)
    <Extension>
    Public Function InvokeFunc(Of TCtl As Control, TResult)(ByVal control As TCtl, ByVal dlg As [Delegate], ByVal ParamArray args As Object()) As TResult
        ' check for nulls
        If control Is Nothing Then
            Throw New ArgumentNullException(NameOf(control))
        End If

        If dlg Is Nothing Then
            Throw New ArgumentNullException(NameOf(dlg))
        End If

        ' check if cross-thread invocation is required
        If control.InvokeRequired Then
            ' if it is, perform it
            ' this will invoke this method from (hopefully) the control's thread
            Return DirectCast(control.Invoke(New InvokeFuncDelegate(Of TCtl, TResult)(AddressOf InvokeFunc), control, dlg, args), TResult)
        End If

        ' finally, call the passed delegate, with supplied arguments and return 
        ' the result
        Return DirectCast(dlg.DynamicInvoke(args), TResult)
    End Function
End Module
