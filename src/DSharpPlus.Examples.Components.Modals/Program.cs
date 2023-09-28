using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
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
            discord.InteractionCreated += HandleAnonCommandAsync;
            discord.ModalSubmitted += SendAnonMessageAsync;

            // Connect to the Discord gateway
            await discord.ConnectAsync();

            // Update our slash commands with a singular command with no parameters.
            // For the love of everything, *please*  use our slash command library instead of this.
            await discord.BulkOverwriteGlobalApplicationCommandsAsync(new[]
            {
                new DiscordApplicationCommand("anon", "Anonymously answer a survey")
            });

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }

        private static async Task HandleAnonCommandAsync(DiscordClient client, InteractionCreateEventArgs eventArgs)
        {
            // Check if the interaction is a command and check if this is the command we registered
            if (eventArgs.Interaction.Type != InteractionType.ApplicationCommand || eventArgs.Interaction.Data.Name != "anon")
            {
                return;
            }

            // Start creating our modal
            DiscordInteractionResponseBuilder builder = new();
            builder
                .WithTitle("Anonymous Message")
                .WithCustomId("modal")
                .AddComponents(new TextInputComponent("Message", "message", "Enter your message here.", "", true, TextInputStyle.Paragraph));

            // Reply with a modal prompt
            await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.Modal, builder);
        }

        private static async Task SendAnonMessageAsync(DiscordClient client, ModalSubmitEventArgs eventArgs)
        {
            // Ensure this is the modal we created
            if (eventArgs.Interaction.Data.CustomId != "modal")
            {
                return;
            }

            // Grab the message field by it's id.
            string message = eventArgs.Values["message"];

            // Create our embed
            DiscordEmbedBuilder builder = new()
            {
                Title = "Anonymous Message",
                Description = message,
                Color = DiscordColor.Blurple
            };

            // Send the embed
            await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().AddEmbed(builder));
        }
    }
}
