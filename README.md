[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=Kiritsu/DSharpPlus-Examples-4.0)](https://dependabot.com)

# DSharpPlus Example Bots
These are example bots for DSharpPlus library.

They are basic bots which are meant to introduce you to various aspects of the DSharpPlus library.

## Example directory
* **Example 1**: Introduces very basic bot skeleton. The bot simply connects to guilds, and prints some information to console.
* **Example 2**: Introduces CommandsNext. This bot also responds to several commands, as well as presents several CNext concepts and examples of use.
* **Example 3**: Introduces Interactivity. This bot has several commands that interact with the user in different ways. Presented are several examples of use.
* **Example 4**: Introduces VoiceNext. This bot has the ability to connect to voice channels and play music in them. Presented is a simple bot that broadcasts audio from specified file in a queued manner.
* **Example 5**: Demonstrates a bot with a Windows Forms UI. Presented is a way to run a bot without deadlocking the entire application, as well as properly dispatching the events to the UI.
* **Example 6**: Demonstrates a bot with a Windows Presentation Foundation (WPF) UI. Presented is a way to run a bot without deadlocking the entire application, as well as properly dispatching the events to the UI.

## Requirements
These projects were made using Visual Studio 2017, and they target .NET Core 3.1. You will need one of the following to open and build the projects:

* [**Visual Studio 2019 Community**](https://www.visualstudio.com/thank-you-downloading-visual-studio/?sku=Community) with **.NET Core cross-platform development** installed.
* [**Visual Studio Code**](https://code.visualstudio.com/download) with **C#** extension.

You will also need to install [.NET Core 3.1 SDK](https://www.microsoft.com/net/download/core) in both cases.

## How to run the bot?
In order to run the bots, you must first create an application for them, build the bots, then configure them.

### Create the application
1. Go to [Applications](https://discord.com/developers/applications) page on Discord Developer portal.
2. Press the [**New Application** button](https://i.alnmrc.com/5c4b195c240d46c7a53a53d32e04683f.png).
3. [**New Application** page](https://i.alnmrc.com/a5b6ad1b12bf436b98539436c0acb424.png) will open. Enter your bot's name in the **name** field.
4. When you're done, press the [**create** button](https://i.alnmrc.com/bfd2b7171f134afdb484c86010024070.png).
5. When the app is created, jump to the **bot** section and press the [**add bot** button](https://i.alnmrc.com/9af7d8e04d654c9b8ae0390784d91c6c.png).
6. Once this is done, you will need to copy the **bot's token**. Under **app bot user**, there's a **token** field, press copy [**the resulting value**](https://i.alnmrc.com/22f3d32b3b754a78b798207a659d2b71.png).

### Building the project and configuring the bot
1. Open the project, select the startup bot, and compile it.
2. Go to `<project path>`, and copy the `config.json` file there.
3. Now go to `DSPlus.Examples.<language>.Ex0n` (where `n` is the selected example), paste that file here.
4. Open it with Notepad. Replace the placeholder values (do not remove the quotes) with your actual token and command prefix. Save it.
5. Our bot is now configured, but we need to add it to our server first.

### Adding the bot to your server
1. Go back to your app page, and copy your bot's [**client ID**](https://i.alnmrc.com/9a311097679c42349ef4d31dc1b9bca9.png).
2. Go to `https://discordapp.com/oauth2/authorize?client_id=your_app_id_here&scope=bot&permissions=0`.
3. On the [page](https://i.alnmrc.com/0a0a619272c041ae9874ae32fcb22f23.png), select **your server** (1), and press **authorize** (2).
4. Verify that you are not a robot and you're [done](https://i.alnmrc.com/d30ce9b2f92541ecabdbf2c339572106.png)! You can now run the bot!

### Running the bots
1. Go back to the IDE and hit **Debug**.
2. Congratulations! [Your bot is running](https://i.alnmrc.com/c5f4c7a38fcc453196e923ff83043d6e.png)!

### Note on WinForms and WPF examples
Windows Forms (WinForms), Windows Presentation Foundation (WPF), and Universal Windows Platform (UWP) are ***not officially supported*** ways of running your bot. These examples exist for educational purposes only.

We will not provide support for running bots through WinForms, WPF, UWP, or any other UI framework, especially non-asynchronous one.

If you require a non-console or non-Discord interface for your bot, consider an ASP.NET or ASP.NET Core application, as these at least offer support for asynchronous code.

## Where to now?
You might want to play around with the examples, perhaps modify them a bit.

You can find the documentation for DSharpPlus [over here](https://dsharpplus.github.io/).

If you want to get more help, join the unofficial [Discord API server](https://discord.gg/discord-api), and look for **#dotnet_dsharpplus** channel.

## Also check out
* [DSharpPlus source on GitHub](https://github.com/DSharpPlus/DSharpPlus)
* [DSharpPlus Discord server](https://discord.gg/TtjsTFB)
* [#dotnet_dsharpplus on Discord API server](https://discord.gg/discord-api)
