using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Examples.Components.Selects
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
            discord.MessageCreated += SendMessageAsync;
            discord.ComponentInteractionCreated += GuessAsync;

            // Connect to the Discord gateway
            await discord.ConnectAsync();

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }

        private static async Task SendMessageAsync(DiscordClient client, MessageCreateEventArgs eventArgs)
        {
            // TODO: Ignore messages from bots
            if (!eventArgs.Message.Content.StartsWith("!ping", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            DiscordMessageBuilder messageBuilder = new();
            messageBuilder.WithContent("Guess my favorite number!");

            // Let's do a random guessing game. We'll use a select menu to let the user pick a number.
            List<DiscordSelectComponentOption> selectValues = new()
            {
                new DiscordSelectComponentOption("One", "one", "One could be my favorite number."),
                new DiscordSelectComponentOption("Two", "two", "Two could be my favorite number."),
                new DiscordSelectComponentOption("Three", "three", "Three could be my favorite number.")
            };

            // Create the select menu
            DiscordSelectComponent selectMenu = new("guess", "Guess a number", selectValues);

            // Add the select menu to the message builder
            messageBuilder.AddComponents(selectMenu);

            // Send the message
            await messageBuilder.SendAsync(eventArgs.Channel);
        }

        private static async Task GuessAsync(DiscordClient client, ComponentInteractionCreateEventArgs eventArgs)
        {
            if (eventArgs.Id != "guess")
            {
                return;
            }

            // Grab the selected value that the user sent us
            string selectedValue = eventArgs.Values[0];

            // Select a random number between 1 and 3
            int favoriteNumber = Random.Shared.Next(1, 4);

            // Match the select id to the number
            bool isCorrect = selectedValue switch
            {
                // option id => favoriteNumber == number
                "one" => favoriteNumber == 1,
                "two" => favoriteNumber == 2,
                "three" => favoriteNumber == 3,
                _ => false
            };

            // Create the message builder
            DiscordInteractionResponseBuilder messageBuilder = new();

            // If the user guessed correctly, let them know!
            if (isCorrect)
            {
                messageBuilder.WithContent("You guessed correctly!");
            }
            else
            {
                messageBuilder.WithContent("You guessed incorrectly! I've changed my mind to a different number.");
            }

            // Make sure we respond ephemerally so only the user can see the response.
            // We don't want to spam the channel for other users.
            messageBuilder.AsEphemeral(true);

            // Respond to the interaction
            await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, messageBuilder);
        }
    }
}
