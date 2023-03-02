# DSharpPlus Example Bots
This repository is aimed to showcase the DSharpPlus .NET Discord library and reintroduce you to the .NET ecosystem. While we do attempt to make our code a good learning resource, it is not intended to teach you C#.

This repository contains example bots for the [DSharpPlus](https://github.com/DSharpPlus/DSharpPlus) .NET Discord library. In the [`./src`](./src) directory, you'll find each extension listed. Inside of the extension directories will you find examples specific to said extension. Each example contains a basic bot which is meant to introduce you to various aspects of the library.

The code contained is aimed for readability, not efficiency. While we attempt to follow best practices, we do not guarantee that the code is perfect. If you find any problems with the bot's functionality, please feel free to open an issue or a pull request.

# Requirements
Each project is targetted for .NET 7. You may find the download [here](https://dotnet.microsoft.com/download/dotnet/7.0).

# How to run the bots?
Each bot is a standalone project. You can run them by navigating to the project directory and executing `dotnet run`. You will need to provide a bot token via the `DISCORD_TOKEN` environment variable: `DISCORD_TOKEN=<token> dotnet run`. You can find more information on how to obtain a bot token [here](https://discord.com/developers/docs/intro).

# Where should I start?
If you're completely new to the library, I recommend checking out the [./src/DSharpPlus.Core/HelloWorld/](./src/DSharpPlus.Core/HelloWorld/) project. Once you feel you've understood how the library starts the bot, you can start using our extensions (such as CommandsNext or SlashCommands) to add further functionality to your bot.