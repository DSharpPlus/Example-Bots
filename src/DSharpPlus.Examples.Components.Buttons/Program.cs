using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.Examples.Components.Buttons
{
    public static class Program
    {
        /// <summary>
        /// A dictionary which holds any active buttons. The values determine the behavior of the button when it's clicked.
        /// </summary>
        private static readonly Dictionary<Guid, ButtonAction> _buttonActions = new();

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
            discord.MessageCreated += SendButtonAsync;
            discord.ComponentInteractionCreated += RespondToButtonAsync;

            // Connect to the Discord gateway
            await discord.ConnectAsync();

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }

        private static async Task SendButtonAsync(DiscordClient client, MessageCreateEventArgs eventArgs)
        {
            if (!eventArgs.Message.Content.StartsWith("!ping", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Create buttons
            // Each button id is unique to the message it's sent with.
            // This means that you can have multiple buttons with the same id, as long as they're in different messages.
            // For the sake of simplicity, we'll just generate a new id for each button.
            Guid pingButtonId = Guid.NewGuid();
            DiscordButtonComponent pingButton = new(ButtonStyle.Primary, pingButtonId.ToString(), "Ping");
            _buttonActions.Add(pingButtonId, ButtonAction.Ping);

            Guid ephemeralButtonId = Guid.NewGuid();
            DiscordButtonComponent ephemeralButton = new(ButtonStyle.Secondary, ephemeralButtonId.ToString(), "Secret Ping");
            _buttonActions.Add(ephemeralButtonId, ButtonAction.Ephemeral);

            // Let's add a fun little button that'll scare your friends.
            // It won't do anything, of course. But it'll be funny!
            DiscordButtonComponent disabledButton = new(ButtonStyle.Danger, "disabled", "Ban Everyone!", true, new DiscordComponentEmoji(DiscordEmoji.FromName(client, ":lock:", false)));

            // Create the message
            DiscordMessageBuilder messageBuilder = new();
            messageBuilder.WithContent("Pong!");
            messageBuilder.AddComponents(pingButton, ephemeralButton, disabledButton);
            await eventArgs.Message.RespondAsync(messageBuilder);
        }

        private static async Task RespondToButtonAsync(DiscordClient client, ComponentInteractionCreateEventArgs eventArgs)
        {
            // Ensure that the button id was intended for this application.
            // Sometimes there can be multiple Discord bots using the same Discord token.
            // It doesn't hurt to do some extra validation.
            if (!Guid.TryParse(eventArgs.Interaction.Data.CustomId, out Guid buttonId) || !_buttonActions.TryGetValue(buttonId, out ButtonAction buttonAction))
            {
                return;
            }

            DiscordInteractionResponseBuilder responseBuilder = new();
            if (buttonAction == ButtonAction.Ping)
            {
                responseBuilder.Content = "Pong!";
            }
            else if (buttonAction == ButtonAction.Ephemeral)
            {
                responseBuilder.Content = "Pong! There's nothing secret to see here :)";
                responseBuilder.IsEphemeral = true;
            }

            // Respond to the clicked button within the first 3 seconds.
            await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);

            // Make each button only usable once. This is to make sure we don't leak memory by never removing the buttons.
            DiscordMessageBuilder originalMessage = new(eventArgs.Message);

            // Here's a little knowledge bomb for you: Discord stores buttons into a type called a "action row."
            // An action row is quite simply, a row of buttons. You can have multiple rows of buttons, but each row can only have 5 buttons.
            // But wait! You never created a row of buttons! That's because DSharpPlus does it for you within the DiscordMessageBuilder.AddComponents method.
            // So, to disable the button, we need to get the action row that the button is in, and then disable the button.
            DiscordActionRowComponent actionRow = originalMessage.Components[0];
            foreach (DiscordComponent component in actionRow.Components)
            {
                if (component is DiscordButtonComponent button && button.CustomId == buttonId.ToString())
                {
                    button.Disable();
                    break;
                }
            }

            // Update the message with the now disabled button
            await eventArgs.Message.ModifyAsync(originalMessage);

            // Remove the button from the dictionary so we don't respond to it again.
            // Not that we can respond to a disabled button, since the users can't click it.
            _buttonActions.Remove(buttonId);
        }

        private enum ButtonAction
        {
            Ping,
            Ephemeral
        }
    }
}
