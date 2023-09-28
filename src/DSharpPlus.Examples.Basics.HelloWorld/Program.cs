using System;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Examples.Basics.HelloWorld
{
    public static class Program
    {
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

            // Register our event handlers
            discord.MessageCreated += PongAsync;

            // Connect to the Discord gateway
            await discord.ConnectAsync();

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }

        private static async Task PongAsync(DiscordClient client, MessageCreateEventArgs eventArgs)
        {
            // TODO: Ignore messages from bots
            if (eventArgs.Message.Content.StartsWith("!ping", StringComparison.OrdinalIgnoreCase))
            {
                await eventArgs.Message.RespondAsync("pong!");
            }
        }
    }
}
