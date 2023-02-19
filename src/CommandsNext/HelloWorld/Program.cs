﻿using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DSharpPlus.ExampleBots.CommandsNext.HelloWorld
{
    // We're sealing it because nothing will be inheriting this class
    public sealed class Program
    {
        // Remember to make your main method async! You no longer need to have both a Main and MainAsync method in the same class.
        public static async Task Main()
        {
            // For the sake of examples, we're going to load our Discord token from an environment variable.
            string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Please specify a token in the DISCORD_TOKEN environment variable.");
                Environment.Exit(1);

                // For the compiler's nullability, unreachable code.
                return;
            }

            // Next, we instantiate our client.
            DiscordConfiguration discordConfig = new()
            {
                Token = token,

                // We're asking for unprivileged intents, which means we won't receive any member or presence updates.
                // Privileged intents must be enabled in the Discord Developer Portal.

                // TODO: Enable the message content intent in the Discord Developer Portal.
                // The bot will not function otherwise.
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
            DiscordClient client = new(discordConfig);

            // We can specify a status for our bot. Let's set it to "online" and set the activity to "with fire".
            DiscordActivity status = new("with fire", ActivityType.Playing);

            // Register Random as a singleton. This will be used by the random command.
            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(Random.Shared); // We're using the shared instance of Random for simplicity.

            // Register CommandsNext
            CommandsNextConfiguration commandsConfig = new()
            {
                // Add the service provider which will allow CommandsNext to inject the Random instance.
                Services = serviceCollection.BuildServiceProvider(),
                StringPrefixes = new[] { "!" }
            };
            CommandsNextExtension commandsNext = client.UseCommandsNext(commandsConfig);

            // Register commands
            // CommandsNext will search the assembly for any classes that inherit from BaseCommandModule and register them as commands.
            commandsNext.RegisterCommands(typeof(Program).Assembly);

            // Now we connect and log in.
            await client.ConnectAsync(status, UserStatus.Online);

            // And now we wait infinitely so that our bot actually stays connected.
            await Task.Delay(-1);
        }
    }
}
