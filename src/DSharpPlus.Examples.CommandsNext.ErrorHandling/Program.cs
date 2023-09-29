using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace DSharpPlus.Examples.CommandsNext.ErrorHandling
{
    public static class Program
    {
        /// <summary>
        /// The prefixes used to trigger Discord text commands through CommandsNext.
        /// </summary>
        private static readonly string[] _prefixes = new[] { "!" };

        public static async Task Main()
        {
            // Check for token
            // TODO: Load the token up from IConfiguration
            string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Please set the environment variable DISCORD_TOKEN.");
                return;
            }

            // Create the client
            DiscordClient discord = new(new DiscordConfiguration
            {
                Token = token,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            });

            CommandsNextExtension commandsNext = discord.UseCommandsNext(new CommandsNextConfiguration()
            {
                // For brevity, we're going to use the string prefixes property.
                // However, if you want to do something complicated, such as per-guild prefixes,
                // you can pass a prefix resolver delegate instead.
                StringPrefixes = _prefixes
            });

            // If we pass our assembly to CommandsNext, it will automatically
            // search our program for any Command classes and register them on its own.
            // This is very handy so you don't need to manually update a list when you add a new command.
            commandsNext.RegisterCommands(typeof(Program).Assembly);

            // Register our error handler
            commandsNext.CommandErrored += CommandErroredEventHandlerAsync;

            // Connect to the Discord gateway
            await discord.ConnectAsync();

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }

        private static async Task CommandErroredEventHandlerAsync(CommandsNextExtension extension, CommandErrorEventArgs eventArgs)
        {
            DiscordEmbedBuilder embedBuilder = new();
            embedBuilder.WithTitle($"An error occurred while executing a command: {eventArgs.Exception.GetType().Name}");
            embedBuilder.WithColor(DiscordColor.DarkRed);
            embedBuilder.WithDescription(eventArgs.Exception.Message);
            embedBuilder.AddField("Stack Trace", string.IsNullOrWhiteSpace(eventArgs.Exception.StackTrace)
                ? "No stack trace available."
                : Formatter.BlockCode(eventArgs.Exception.StackTrace.Trim()[3..], "cs")); // The [3..] removes the first three characters (at\n) in the stack trace.

            await eventArgs.Context.RespondAsync(embedBuilder.Build());
        }
    }
}
