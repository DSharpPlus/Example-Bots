# DSharpPlus Example Bots
These are example bots for DSharpPlus library.

They are basic bots which are meant to introduce you to various aspects of the DSharpPlus library.

## Example directory
* **Example 1**: Introduces very basic bot skeleton. The bot simply connects to guilds, and prints some information to console.
* **Example 2**: Introduces CommandsNext. This bot also responds to several commands, as well as presents several CNext concepts and examples of use.
* **Example 3**: Introduces Interactivity. This bot has several commands that interact with the user in different ways. Presented are several examples of use.
* **Example 4**: Introduces VoiceNext. This bot has the ability to connect to voice channels and play music in them. Presented is a simple bot that broadcasts audio from specified file in a queued manner.

## Requirements
These projects were made using Visual Studio 2017, and they target .NET Core 2.0. You will need one of the following to open and build the projects:

* [**Visual Studio 2017 Community**](https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=Community&rel=15) with **.NET Core cross-platform development** installed.
* [**Visual Studio Code**](https://code.visualstudio.com/download) with **C#** extension.

You will also need to install [.NET Core 2.0 SDK](https://www.microsoft.com/net/download/core) in both cases.

## How to run the bot?
In order to run the bots, you must first create an application for them, build the bots, then configure them.

### Create the application
1. Go to [my applications](https://discordapp.com/developers/applications/me) page on Discord Developer portal.
2. Press the [**new app** button](http://i.imgur.com/IVsPyNw.png).
3. [**New app** page](http://i.imgur.com/3mrEG9x.png) will open. Enter your bot's name in the **app name** field (1), and its description in the **description** field (2).
   * You can optionally give it an avatar by pressing on the **avatar** button (3).
4. When you're done, press the [**create app** button](http://i.imgur.com/ur3HFng.png).
5. When the app is created, press the [**create bot user** button](http://i.imgur.com/b69CHy7.png).
6. Once this is done, you will need to copy the **bot's token**. Under **app bot user**, there's a **token** field, press [**click to reveal**](http://i.imgur.com/00b4Nt8.png) and copy [**the resulting value**](http://i.imgur.com/Lt2uhcN.png).

### Building the project and configuring the bot
1. Open the project, select the startup bot, and compile it.
2. Go to `<project path>`, and copy the `config.json` file there.
3. Now go to `DSPlus.Examples.<language>.Ex0n` (where `n` is the selected example), paste that file here.
4. Open it with Notepad. Replace the placeholder values (do not remove the quotes) with your actual token and command prefix. Save it.
5. Our bot is now configured, but we need to add it to our server first.

### Adding the bot to your server
1. Go back to your app page, and copy your bot's [**client ID**](http://i.imgur.com/NuAPpoY.png).
2. Go to `https://discordapp.com/oauth2/authorize?client_id=your_app_id_here&scope=bot&permissions=0`.
3. On the [page](http://i.imgur.com/QeH0o5S.png), select **your server** (1), and press **authorize** (2).
4. [Done](http://i.imgur.com/LF1gpm2.png)! You can now run the bot!

### Running the bots
1. Go back to the IDE and hit **Debug**.
2. Congratulations! [Your bot is running](http://i.imgur.com/VXpCt1P.png)!

### Note on WinForms and WPF examples
Windows Forms (WinForms), Windows Presentation Foundation (WPF), and Universal Windows Platform (UWP) are ***not officially supported*** ways of running your bot. These examples exist for educational purposes only.

We will not provide support for running bots through WinForms, WPF, UWP, or any other UI framework, especially non-asynchronous one.

If you require a non-console or non-Discord interface for your bot, consider an ASP.NET or ASP.NET Core application, as these at least offer support for asynchronous code.

## Where to now?
You might want to play around with the examples, perhaps modify them a bit.

You can find the documentation for DSharpPlus [over here](https://dsharpplus.emzi0767.com/).

If you want to get more help, join the unofficial [Discord API server](https://discord.gg/discord-api), and look for **#dotnet_dsharpplus** channel.

## Also check out
* [Naamloos' example bot](https://github.com/NaamloosDT/DSharpPlus-example)
* [DSharpPlus source on GitHub](https://github.com/NaamloosDT/DSharpPlus)
* [DSharpPlus Discord server](https://discord.gg/0oZpaYcAjfvkDuE4)
* [#dotnet_dsharpplus on Discord API server](https://discord.gg/N4WeVd)
