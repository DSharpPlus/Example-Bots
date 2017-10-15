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
' This is an interactivity example. It shows how to properly utilize 
' Interactivity module.

Option Strict On
Imports System.Security.Cryptography
Imports DSharpPlus.CommandsNext
Imports DSharpPlus.CommandsNext.Attributes
Imports DSharpPlus.Entities
Imports DSharpPlus.Interactivity

' note that in here we explicitly ask for duration. This is optional,
' since we set the defaults.
Public Class ExampleInteractiveCommands
    <Command("poll"), Description("Run a poll with reactions.")>
    Public Async Function Poll(ByVal ctx As CommandContext, <Description("How long should the poll last.")> ByVal duration As TimeSpan, <Description("What options should people have?")> ByVal ParamArray options() As DiscordEmoji) As Task
        ' first retrieve the interactivity module from the client
        Dim interactivity = ctx.Client.GetInteractivityModule()
        Dim poll_options = options.Select(Function(xe) xe.ToString())

        ' then let's present the poll
        Dim embed As New DiscordEmbedBuilder()
        With embed
            .Title = "Poll time!"
            .Description = String.Join(" ", poll_options)
        End With
        Dim msg = Await ctx.RespondAsync(embed:=embed)

        ' add the options as reactions
        For Each emoji As DiscordEmoji In options
            Await msg.CreateReactionAsync(emoji)
        Next

        ' collect and filter responses
        Dim poll_result = Await interactivity.CollectReactionsAsync(msg, duration)
        Dim results = poll_result.Reactions.Where(Function(xkvp) options.Contains(xkvp.Key)) _
            .Select(Function(xkvp) $"{xkvp.Key}: {xkvp.Value}")

        ' and finally post the results
        Await ctx.RespondAsync(String.Join(ChrW(10), results))
    End Function

    <Command("waitforcode"), Description("Waits for a response containing a generated code.")>
    Public Async Function WaitForCode(ByVal ctx As CommandContext) As Task
        ' first retrieve the interactivity module from the client
        Dim interactivity = ctx.Client.GetInteractivityModule()

        ' generate a code
        Dim codebytes(7) As Byte
        Using rng = RandomNumberGenerator.Create()
            rng.GetBytes(codebytes)
        End Using

        Dim code = BitConverter.ToString(codebytes).ToLower().Replace("-", "")

        ' announce the code
        Await ctx.RespondAsync($"The first one to type the following code gets a reward: `{code}`")

        ' wait for anyone who types it
        Dim msg = Await interactivity.WaitForMessageAsync(Function(xm) xm.Content.Contains(code), TimeSpan.FromMinutes(1))
        If msg IsNot Nothing Then
            ' announce the winner
            Await ctx.RespondAsync($"And the winner is: {msg.Message.Author.Mention}")
        Else
            Await ctx.RespondAsync("Nobody? Really?")
        End If
    End Function

    <Command("waitforreact"), Description("Waits for a reaction.")>
    Public Async Function WaitForReaction(ByVal ctx As CommandContext) As Task
        ' first retrieve the interactivity module from the client
        Dim interactivity = ctx.Client.GetInteractivityModule()

        ' specify the emoji
        Dim emoji = DiscordEmoji.FromName(ctx.Client, ":point_up:")

        ' announce
        Await ctx.RespondAsync($"React with {emoji} to quote a message!")

        ' wait for reaction
        Dim em = Await interactivity.WaitForReactionAsync(Function(xe) xe = emoji, ctx.User, TimeSpan.FromMinutes(1))
        If em IsNot Nothing Then
            ' quote
            Dim embed As New DiscordEmbedBuilder()
            With embed
                .Color = If(TypeOf em.Message.Author Is DiscordMember, TryCast(em.Message.Author, DiscordMember).Color, New DiscordColor(&HFF00FF))
                .Description = em.Message.Content
                .Author = New DiscordEmbedBuilder.EmbedAuthor()
                With .Author
                    .Name = If(TypeOf em.Message.Author Is DiscordMember, TryCast(em.Message.Author, DiscordMember).DisplayName, em.Message.Author.Username)
                    .IconUrl = em.Message.Author.AvatarUrl
                End With
            End With
            Await ctx.RespondAsync(embed:=embed)
        Else
            Await ctx.RespondAsync("Seriously?")
        End If
    End Function

    <Command("waitfortyping"), Description("Waits for a typing indicator.")>
    Public Async Function WaitForTyping(ByVal ctx As CommandContext) As Task
        ' first retrieve the interactivity module from the client
        Dim interactivity = ctx.Client.GetInteractivityModule()

        ' then wait for author's typing
        Dim chn = Await interactivity.WaitForTypingChannelAsync(ctx.User, TimeSpan.FromMinutes(1))
        If chn IsNot Nothing Then
            ' got 'em
            Await ctx.RespondAsync($"{ctx.User.Mention}, you typed in {chn.Channel.Mention}!")
        Else
            Await ctx.RespondAsync("*yawn*")
        End If
    End Function

    <Command("sendpaginated"), Description("Sends a paginated message.")>
    Public Async Function SendPaginated(ByVal ctx As CommandContext) As Task
        ' first retrieve the interactivity module from the client
        Dim interactivity = ctx.Client.GetInteractivityModule()

        ' generate pages.
        Dim lipsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris vitae velit eget nunc iaculis laoreet vitae eu risus. Nullam sit amet cursus purus. Duis enim elit, malesuada consequat aliquam sit amet, interdum vel orci. Donec vehicula ut lacus consequat cursus. Aliquam pellentesque eleifend lectus vitae sollicitudin. Vestibulum sit amet risus rhoncus, hendrerit felis eget, tincidunt odio. Nulla sed urna ante. Mauris consectetur accumsan purus, ac dignissim ligula condimentum eu. Phasellus ullamcorper, arcu sed scelerisque tristique, ante elit tincidunt sapien, eu laoreet ipsum mauris eu justo. Curabitur mattis cursus urna, eu ornare lacus pulvinar in. Vivamus cursus gravida nunc. Sed dolor nisi, congue non hendrerit at, rutrum sed mi. Duis est metus, consectetur sed libero quis, dignissim gravida lacus. Mauris suscipit diam dolor, semper placerat justo sodales vel. Curabitur sed fringilla odio." & ChrW(10) & ChrW(10) & "Morbi pretium placerat nulla sit amet condimentum. Duis placerat, felis ornare vehicula auctor, augue odio consectetur eros, sit amet tristique dolor risus nec leo. Aenean vulputate ipsum sagittis augue malesuada, id viverra odio gravida. Curabitur aliquet elementum feugiat. Phasellus eu faucibus nibh, eget finibus nibh. Proin ac fermentum enim, non consequat orci. Nam quis elit vulputate, mollis eros ut, maximus lacus. Vivamus et lobortis odio. Suspendisse potenti. Fusce nec magna in eros tempor tincidunt non vel mi. Pellentesque auctor eros tellus, vel ultrices mi ultricies eu. Nam pharetra sed tortor id elementum. Donec sit amet mi eleifend, iaculis purus sit amet, interdum turpis." & ChrW(10) & ChrW(10) & "Aliquam at consectetur lectus. Ut et ultrices augue. Etiam feugiat, tortor nec dictum pharetra, nulla mauris convallis magna, quis auctor libero ipsum vitae mi. Mauris posuere feugiat feugiat. Phasellus molestie purus sit amet ipsum sodales, eget pretium lorem pharetra. Quisque in porttitor quam, nec hendrerit ligula. Fusce tempus, diam ut malesuada semper, leo tortor vulputate erat, non porttitor nisi elit eget turpis. Nam vitae arcu felis. Aliquam molestie neque orci, vel consectetur velit mattis vel. Fusce eget tempus leo. Morbi sit amet bibendum mauris. Aliquam erat volutpat. Phasellus nunc lectus, vulputate vitae turpis vel, tristique vulputate nulla. Aenean sit amet augue at mauris laoreet convallis. Nam quis finibus dui, at lobortis lectus." & ChrW(10) & ChrW(10) & "Suspendisse potenti. Pellentesque massa enim, dapibus at tortor eu, posuere ultricies augue. Nunc condimentum enim id ex sagittis, ut dignissim neque tempor. Nulla cursus interdum turpis. Aenean auctor tempor justo, sed rhoncus lorem sollicitudin quis. Fusce non quam a ante suscipit laoreet eget at ligula. Aenean condimentum consectetur nunc, sit amet facilisis eros lacinia sit amet. Integer quis urna finibus, tristique justo ut, pretium lectus. Proin consectetur enim sed risus rutrum, eu vehicula augue pretium. Vivamus ultricies justo enim, id imperdiet lectus molestie at. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas." & ChrW(10) & ChrW(10) & "Nullam tincidunt dictum nibh, dignissim laoreet libero eleifend ut. Vestibulum eget maximus nulla. Suspendisse a auctor elit, ac facilisis tellus. Sed iaculis turpis ac purus tempor, ut pretium ante ultrices. Aenean commodo tempus vestibulum. Morbi vulputate pharetra molestie. Ut rhoncus quam felis, id mollis quam dapibus id. Curabitur faucibus id justo in ornare. Praesent facilisis dolor lorem, non vulputate velit finibus ut. Praesent vestibulum nunc ac nibh iaculis porttitor." & ChrW(10) & ChrW(10) & "Fusce mattis leo sed ligula laoreet accumsan. Pellentesque tortor magna, ornare vitae tellus eget, mollis placerat est. Suspendisse potenti. Ut sit amet lacus sed nibh pulvinar mattis in bibendum dui. Mauris vitae turpis tempor, malesuada velit in, sodales lacus. Sed vehicula eros in magna condimentum vestibulum. Aenean semper finibus lectus, vel hendrerit lorem euismod a. Sed tempor ante quis magna sollicitudin, eu bibendum risus congue. Donec lectus sem, accumsan ut mollis et, accumsan sed lacus. Nam non dui non tellus pretium mattis. Mauris ultrices et felis ut imperdiet. Nam erat risus, consequat eu eros ac, convallis viverra sapien. Etiam maximus nunc et felis ultrices aliquam." & ChrW(10) & ChrW(10) & "Ut tincidunt at magna at interdum. Sed fringilla in sem non lobortis. In dictum magna justo, nec lacinia eros porta at. Maecenas laoreet mattis vulputate. Sed efficitur tempor euismod. Integer volutpat a odio eu sagittis. Aliquam congue tristique nisi, quis aliquet nunc tristique vitae. Vivamus ac iaculis nunc, et faucibus diam. Donec vitae auctor ipsum, quis posuere est. Proin finibus, dolor ac euismod consequat, urna sem ultrices lectus, in iaculis sem nulla et odio. Integer et vulputate metus. Phasellus finibus et lorem eget lacinia. Maecenas velit est, luctus quis fermentum nec, fringilla eu lorem." & ChrW(10) & ChrW(10) & "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Mauris faucibus neque eu consectetur egestas. Mauris aliquet nibh pellentesque mollis facilisis. Duis egestas lectus sed justo sagittis ultrices. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Curabitur hendrerit quis arcu id dictum. Praesent in massa eget lectus pulvinar consectetur. Aliquam eget ipsum et velit congue porta vitae ut eros. Quisque convallis lacus et venenatis sagittis. Phasellus sit amet eros ac nibh facilisis laoreet vel eget nisi. In ante libero, volutpat in risus vel, tristique blandit leo. Morbi posuere bibendum libero, non efficitur mi sagittis vel. Cras viverra pulvinar pellentesque. Mauris auctor et lacus ut pellentesque. Nunc pretium luctus nisi eu convallis." & ChrW(10) & ChrW(10) & "Sed nec ultricies arcu. Aliquam eu tincidunt diam, nec luctus ligula. Ut laoreet dignissim est, eu fermentum massa fermentum eget. Nullam non viverra justo, sed congue felis. Phasellus id convallis mauris. Aliquam elementum euismod ex, vitae dignissim nunc consectetur vitae. Donec ut odio quis ex placerat elementum sit amet eget lectus. Suspendisse potenti. Nam non massa id mi suscipit euismod. Nullam varius tincidunt diam congue congue. Proin pharetra vestibulum eros, vel imperdiet sem rutrum at. Cras eget gravida ligula, quis facilisis ex." & ChrW(10) & ChrW(10) & "Etiam consectetur elit mauris, euismod porta urna auctor a. Nulla facilisi. Praesent massa ipsum, iaculis non odio at, varius lobortis nisi. Aliquam viverra erat a dapibus porta. Pellentesque imperdiet maximus mattis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Donec luctus elit sit amet feugiat convallis. Phasellus varius, sem ut volutpat vestibulum, magna arcu porttitor libero, in dapibus metus dolor nec dolor. Fusce at eleifend magna. Mauris cursus pellentesque sagittis. Nullam nec laoreet ante, in sodales arcu."
        Dim lipsum_pages = interactivity.GeneratePagesInEmbeds(lipsum)

        ' send the paginator
        Await interactivity.SendPaginatedMessage(ctx.Channel, ctx.User, lipsum_pages, TimeSpan.FromMinutes(5), TimeoutBehaviour.Delete)
    End Function
End Class
