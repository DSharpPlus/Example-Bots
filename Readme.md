# DSharpPlus Example Bot

This is an example bot for the DSharpPlus library.

This is a very basic bot, but it should give you a good idea of where to start.

## How to run the bot?

In order to run the bot, you must first create an application for it, build the bot, then configure it.

### Create the application

1. Go to [my applications](https://discordapp.com/developers/applications/me) page on Discord Developer portal.
2. Press the [**new app** button](http://i.imgur.com/IVsPyNw.png).
3. [**New app** page](http://i.imgur.com/3mrEG9x.png) will open. Enter your bot's name in the **app name** field (1), and its description in the **description** field (2).
   * You can optionally give it an avatar by pressing on the **avatar** button (3).
4. When you're done, press the [**create app** button](http://i.imgur.com/ur3HFng.png).
5. When the app is created, press the [**create bot user** button](http://i.imgur.com/b69CHy7.png).
6. Once this is done, you will need to copy the **bot's token**. Under **app bot user**, there's a **token** field, press [**click to reveal**](http://i.imgur.com/00b4Nt8.png) and copy [**the resulting value**](http://i.imgur.com/Lt2uhcN.png).

### Building the project and configuring the bot

1. Open the project, and compile it in **Release** mode.
2. Go to `<project path>`, and copy the `config.json` file there.
3. Now go to `ExampleBot.<language>/bin/Release`, paste that file here.
4. Open it with Notepad. Replace `Put your bot token here` with your token (do not remove the quotes). Save it.
5. Our bot is now configured, but we need to add it to our server first.

### Adding the bot to your server

1. Go back to your app page, and copy your bot's [**client ID**](http://i.imgur.com/NuAPpoY.png).
2. Go to `https://discordapp.com/oauth2/authorize?client_id=your_app_id_here&scope=bot&permissions=0`.
3. On the [page](http://i.imgur.com/QeH0o5S.png), select **your server** (1), and press **authorize** (2).
4. [Done](http://i.imgur.com/LF1gpm2.png)! You can now run the bot!

### Running the bot

Now start `ExampleBot.<language>.exe`. Congratulations! [Your bot is running](http://i.imgur.com/VXpCt1P.png)! Type `ping` in your chat, and [it should respond with `pong`](http://i.imgur.com/ymnecfp.png)! If it does, it's working.

## Where to now?

You might want to play around with the example, perhaps modify it a bit.

You can find the documentation for DSharpPlus [over here](http://dsharpplus.readthedocs.io/).

If you want to get more help, join the unofficial [Discord API server](https://discord.gg/discord-api), and look for **#dotnet_dsharpplus** channel.

## Also check out

* [DSharpPlus source on GitHub](https://github.com/NaamloosDT/DSharpPlus)
* [DSharpPlus Discord server](https://discord.gg/0oZpaYcAjfvkDuE4)
