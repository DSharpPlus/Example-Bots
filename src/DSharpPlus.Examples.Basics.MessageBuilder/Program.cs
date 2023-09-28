using System;
using System.IO;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Examples.Basics.MessageBuilder
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
            discord.MessageCreated += CommandInvokerAsync;

            // Connect to the Discord gateway
            await discord.ConnectAsync();

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }

        // TODO: Checkout our command library, CommandsNext. It makes this a lot easier!
        private static async Task CommandInvokerAsync(DiscordClient client, MessageCreateEventArgs eventArgs)
        {
            DiscordMessageBuilder messageBuilder = new();
            if (eventArgs.Message.Content.Equals("!ping", StringComparison.OrdinalIgnoreCase))
            {
                messageBuilder.Content = "Pong!";
            }
            else if (eventArgs.Message.Content.Equals("!embed", StringComparison.OrdinalIgnoreCase))
            {
                // We can assign properties manually
                DiscordEmbedBuilder embedBuilder = new()
                {
                    Color = new DiscordColor("#FF0000"),
                    Title = "This is an embed!",
                    Description = "Hello World!"
                };

                // Or we can chain them via Add and With methods
                embedBuilder.WithAuthor(eventArgs.Author.Username, eventArgs.Author.AvatarUrl, eventArgs.Author.AvatarUrl);
                embedBuilder.AddField("Field Title", "Field Content");

                // Don't forget to add the embed to the message
                messageBuilder.WithEmbed(embedBuilder);
            }
            else if (eventArgs.Message.Content.Equals("!file", StringComparison.OrdinalIgnoreCase))
            {
                // Send the license file
                messageBuilder.AddFile("LICENSE.txt", File.OpenRead("LICENSE"));
            }
            else
            {
                // No command was found so we don't want to send a message
                return;
            }

            // Send the message
            await eventArgs.Message.RespondAsync(messageBuilder);
        }
    }
}
