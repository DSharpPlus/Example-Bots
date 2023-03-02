using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Serilog;

namespace DSharpPlus.ExampleBots.Core.Logging
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

            // By default DSharpPlus provides it's own logger however we'll be overriding it with our own.
            ILogger logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            // Next, we instantiate our client.
            DiscordConfiguration config = new()
            {
                Token = token,

                // We create a new LoggerFactory (fully namespaced due to naming conflicts) and add our logger with Serilog.
                LoggerFactory = new Microsoft.Extensions.Logging.LoggerFactory().AddSerilog(logger),

                // We're asking for unprivileged intents, which means we won't receive any member or presence updates.
                // Privileged intents must be enabled in the Discord Developer Portal.

                // TODO: Enable the message content intent in the Discord Developer Portal.
                // The !ping command will not work without it.
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
            DiscordClient client = new(config);

            // Register a simple ping command
            client.MessageCreated += async (client, eventArgs) =>
            {
                // We use StringComparison.OrdinalIgnoreCase for case-insensitive matching.
                if (eventArgs.Message.Content.Equals("!ping", StringComparison.OrdinalIgnoreCase))
                {
                    // Respond to the message with "Pong!"
                    await eventArgs.Message.RespondAsync($"Pong! The gateway latency is {client.Ping}ms.");
                }
            };

            // We can specify a status for our bot. Let's set it to "online" and set the activity to "with fire".
            DiscordActivity status = new("with fire", ActivityType.Playing);

            // Now we connect and log in.
            await client.ConnectAsync(status, UserStatus.Online);

            // Let us know that we're ready to go.
            logger.ForContext<Program>().Information("Bot is ready!");

            // And now we wait infinitely so that our bot actually stays connected.
            await Task.Delay(-1);
        }
    }
}
