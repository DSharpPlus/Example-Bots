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
Imports DSharpPlus
Imports DSharpPlus.CommandsNext
Imports DSharpPlus.CommandsNext.Attributes
Imports DSharpPlus.Entities

Public Class ExampleUngrouppedCommands
    <Command("ping")> ' let's define this method as a command
    <Description("Example ping command")> ' this will be displayed to tell users what this command does when they invoke help
    <Aliases("pong")> ' alternative names for the command
    Public Async Function Ping(ByVal ctx As CommandContext) As Task
        ' let's trigger a typing indicator to let
        ' users know we're working
        Await ctx.TriggerTypingAsync()

        ' let's make the message a bit more colourful
        Dim emoji As DiscordEmoji = DiscordEmoji.FromName(ctx.Client, ":ping_pong:")

        ' respond with ping
        Await ctx.RespondAsync($"{emoji} Pong! Ping: {ctx.Client.Ping}ms")
    End Function

    <Command("greet"), Description("Says hi to specified user."), Aliases("sayhi", "say_hi")>
    Public Async Function Greet(ByVal ctx As CommandContext, <Description("The user to say hi to.")> ByVal member As DiscordMember) As Task ' this command takes a member as an argument; you can pass one by username, nickname, id, or mention
        ' note the [Description] attribute on the argument.
        ' this will appear when people invoke help for the
        ' command.

        ' let's trigger a typing indicator to let
        ' users know we're working
        Await ctx.TriggerTypingAsync()

        ' let's make the message a bit more colourful
        Dim emoji As DiscordEmoji = DiscordEmoji.FromName(ctx.Client, ":wave:")

        ' and finally, let's respond and greet the user.
        Await ctx.RespondAsync($"{emoji} Hello, {member.Mention}!")
    End Function

    <Command("sum"), Description("Sums all given numbers and returns said sum.")>
    Public Async Function SumOfNumbers(ByVal ctx As CommandContext, <Description("Integers to sum.")> ByVal ParamArray args() As Integer) As Task
        ' note the ParamArray on the argument. It will indicate
        ' that the command will capture all the remaining arguments
        ' into a single array

        ' let's trigger a typing indicator to let
        ' users know we're working
        Await ctx.TriggerTypingAsync()

        ' calculate the sum
        Dim Sum As Integer = args.Sum()

        ' and send it to the user
        Await ctx.RespondAsync($"The sum of these numbers is {Sum.ToString("#,##0")}")
    End Function

    ' this command will use our custom type, for which have 
    ' registered a converter during initialization
    <Command("math"), Description("Does basic math.")>
    Public Async Function Math(ByVal ctx As CommandContext, <Description("The operation to perform on the operands")> ByVal operation As MathOperation, <Description("First operand.")> ByVal num1 As Double, <Description("Second operand.")> ByVal num2 As Double) As Task
        Dim Result As Double = 0.0
        Select Case operation
            Case MathOperation.Add
                Result = num1 + num2

            Case MathOperation.Subtract
                Result = num1 - num2

            Case MathOperation.Multiply
                Result = num1 * num2

            Case MathOperation.Divide
                Result = num1 / num2

            Case MathOperation.Modulo
                Result = num1 Mod num2
        End Select

        Dim emoji As DiscordEmoji = DiscordEmoji.FromName(ctx.Client, ":1234:")
        Await ctx.RespondAsync($"{emoji} The result is {Result.ToString("#,##0.00")}")
    End Function
End Class

<Group("admin")> ' let's mark this class as a command group
<Description("Administrative commands.")> ' give it a description for help purposes
<Hidden> ' let's hide this from the eyes of curious users
<RequirePermissions(Permissions.ManageGuild)> ' and restrict this to users who have appropriate permissions
Public Class ExampleGrouppedCommands
    ' all the commands will need to be executed as <prefix>admin <command> <arguments>

    <Command("sudo"), Description("Executes a command as another user."), Hidden, RequireOwner>
    Public Async Function Sudo(ByVal ctx As CommandContext, <Description("Member to execute as.")> ByVal member As DiscordMember, <RemainingText, Description("Command text to execute.")> ByVal command As String) As Task
        ' note the <RemainingText> attribute on the argument.
        ' it will capture all the text passed to the command

        ' let's trigger a typing indicator to let
        ' users know we're working
        Await ctx.TriggerTypingAsync()

        ' get the command service, we need this for
        ' sudo purposes
        Dim cmds As CommandsNextModule = ctx.CommandsNext

        ' and perform the sudo
        Await cmds.SudoAsync(member, ctx.Channel, command)
    End Function

    <Command("nick"), Description("Gives someone a new nickname."), RequirePermissions(Permissions.ManageNicknames)>
    Public Async Function ChangeNickname(ByVal ctx As CommandContext, <Description("Member to change the nickname for.")> ByVal member As DiscordMember, <RemainingText, Description("The nickname to give to that user.")> ByVal new_nickname As String) As Task
        ' let's trigger a typing indicator to let
        ' users know we're working
        Await ctx.TriggerTypingAsync()

        Dim success = False
        Try
            ' let's change the nickname, and tell the 
            ' audit logs who did it.
            Await member.ModifyAsync(nickname:=new_nickname, reason:=$"Changed by {ctx.User.Username} ({ctx.User.Id}).")
        Catch Ex As Exception
            ' oh no, something failed, let the invoker now
            success = False
        End Try

        ' let's make a simple response.
        Dim emoji As DiscordEmoji
        If success Then
            emoji = DiscordEmoji.FromName(ctx.Client, ":-1:")
        Else
            emoji = DiscordEmoji.FromName(ctx.Client, ":-1:")
        End If

        ' and respond with it.
        Await ctx.RespondAsync(emoji)
    End Function
End Class

<Group("memes", CanInvokeWithoutSubcommand:=True)> ' this makes the class a group, but with a twist; the class now needs an ExecuteGroupAsync method
<Description("Contains some memes. When invoked without subcommand, returns a random one.")>
<Aliases("copypasta")>
Public Class ExampleExecutableGroup
    ' commands in this group need to be executed as 
    ' <prefix>memes [command] Or <prefix>copypasta [command]

    ' this is the group's command; unlike with other commands, 
    ' any attributes on this one are ignored, but like other
    ' commands, it can take arguments
    Public Async Function ExecuteGroupAsync(ByVal ctx As CommandContext) As Task
        ' let's give them a random meme
        Dim rnd As New Random()
        Dim nxt = rnd.Next(0, 2)

        Select Case nxt
            Case 0
                Await Pepe(ctx)

            Case 1
                Await NavySeal(ctx)

            Case 2
                Await Kekistani(ctx)
        End Select
    End Function

    <Command("pepe"), Aliases("feelsbadman"), Description("Feels bad, man.")>
    Public Async Function Pepe(ByVal ctx As CommandContext) As Task
        Await ctx.TriggerTypingAsync()

        ' wrap it into an embed
        Dim embed As New DiscordEmbedBuilder()
        With embed
            .Title = "Pepe"
            .ImageUrl = "http://i.imgur.com/44SoSqS.jpg"
        End With
        Await ctx.RespondAsync(embed:=embed)
    End Function

    <Command("navyseal"), Aliases("gorillawarfare"), Description("What the fuck did you just say to me?")>
    Public Async Function NavySeal(ByVal ctx As CommandContext) As Task
        Await ctx.TriggerTypingAsync()
        Await ctx.RespondAsync("What the fuck did you just fucking say about me, you little bitch? I’ll have you know I graduated top of my class in the Navy Seals, and I’ve been involved in numerous secret raids on Al-Quaeda, and I have over 300 confirmed kills. I am trained in gorilla warfare and I’m the top sniper in the entire US armed forces. You are nothing to me but just another target. I will wipe you the fuck out with precision the likes of which has never been seen before on this Earth, mark my fucking words. You think you can get away with saying that shit to me over the Internet? Think again, fucker. As we speak I am contacting my secret network of spies across the USA and your IP is being traced right now so you better prepare for the storm, maggot. The storm that wipes out the pathetic little thing you call your life. You’re fucking dead, kid. I can be anywhere, anytime, and I can kill you in over seven hundred ways, and that’s just with my bare hands. Not only am I extensively trained in unarmed combat, but I have access to the entire arsenal of the United States Marine Corps and I will use it to its full extent to wipe your miserable ass off the face of the continent, you little shit. If only you could have known what unholy retribution your little "“clever"” comment was about to bring down upon you, maybe you would have held your fucking tongue. But you couldn’t, you didn’t, and now you’re paying the price, you goddamn idiot. I will shit fury all over you and you will drown in it. You’re fucking dead, kiddo.")
    End Function

    <Command("kekistani"), Aliases("kek", "normies"), Description("I'm a proud ethnic Kekistani.")>
    Public Async Function Kekistani(ByVal ctx As CommandContext) As Task
        Await ctx.TriggerTypingAsync()
        Await ctx.RespondAsync("I'm a proud ethnic Kekistani. For centuries my people bled under Normie oppression. But no more. We have suffered enough under your Social Media Tyranny. It is time to strike back. I hereby declare a meme jihad on all Normies. Normies, GET OUT! RRRÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆÆ﻿")
    End Function

    ' this is a subgroup; you can nest groups as much 
    ' as you like
    <Group("mememan", CanInvokeWithoutSubcommand:=True), Hidden>
    Public Class MemeMan
        Public Async Function ExecuteGroupAsync(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "Meme man"
                .ImageUrl = "http://i.imgur.com/tEmKtNt.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("ukip"), Description("The UKIP pledge.")>
        Public Async Function UkipPledge(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "UKIP pledge"
                .ImageUrl = "http://i.imgur.com/ql76fCQ.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("lineofsight"), Description("Line of sight.")>
        Public Async Function LOS(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "Line of sight"
                .ImageUrl = "http://i.imgur.com/ZuCUnEb.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("art"), Description("Art.")>
        Public Async Function Art(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "Art"
                .ImageUrl = "http://i.imgur.com/VkmmmQd.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("seeameme"), Description("When you see a meme.")>
        Public Async Function SeeMeme(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "When you see a meme"
                .ImageUrl = "http://i.imgur.com/8GD0hbZ.jpg"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("thisis"), Description("This is meme man.")>
        Public Async Function ThisIs(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "This is meme man"
                .ImageUrl = "http://i.imgur.com/57vDOe6.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("deepdream"), Description("Deepdream'd meme man.")>
        Public Async Function DeepDream(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "Deep dream"
                .ImageUrl = "http://i.imgur.com/U666J6x.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("sword"), Description("Meme with a sword?")>
        Public Async Function Sword(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "Meme with a sword?"
                .ImageUrl = "http://i.imgur.com/T3FMXdu.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function

        <Command("christmas"), Description("Beneath the christmas spike...")>
        Public Async Function ChristmasSpike(ByVal ctx As CommandContext) As Task
            Await ctx.TriggerTypingAsync()

            ' wrap it into an embed
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Title = "Christmas spike"
                .ImageUrl = "http://i.imgur.com/uXIqUS7.png"
            End With
            Await ctx.RespondAsync(embed:=embed)
        End Function
    End Class
End Class