using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace DSharpPlus.ExampleBots.Core.ComponentHandling
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
            DiscordConfiguration config = new()
            {
                Token = token,

                // We're asking for unprivileged intents, which means we won't receive any member or presence updates.
                // Privileged intents must be enabled in the Discord Developer Portal.

                // TODO: Enable the message content intent in the Discord Developer Portal.
                // The !ping command will not work without it.
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
            DiscordClient client = new(config);

            // Register our event handlers. We can add them like this because the method signatures match.
            client.MessageCreated += EventHandlers.SendButtonAsync;
            client.ComponentInteractionCreated += EventHandlers.HandleButtonAsync;

            // We can specify a status for our bot. Let's set it to "online" and set the activity to "with fire".
            DiscordActivity status = new("with fire", ActivityType.Playing);

            // Now we connect and log in.
            await client.ConnectAsync(status, UserStatus.Online);

            // And now we wait infinitely so that our bot actually stays connected.
            await Task.Delay(-1);
        }
    }
}
