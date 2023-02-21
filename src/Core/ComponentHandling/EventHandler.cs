using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DSharpPlus.ExampleBots.Core.ComponentHandling
{
    // We're sealing it because nothing will be inheriting this class
    public sealed class EventHandlers
    {
        // A private dictionary containing the currently registered event handlers.
        // Because we do not have a PeriodicTimer running, the buttons will only be
        // removed when the user clicks the button after they expire.
        // This is a memory leak!
        private static readonly Dictionary<Guid, DateTime> ActiveButtons = new();

        // Send a button when the user sends !button, case insensitive.
        public static async Task SendButtonAsync(DiscordClient client, MessageCreateEventArgs eventArgs)
        {
            if (!eventArgs.Message.Content.Equals("!button", StringComparison.OrdinalIgnoreCase))
            {
                // If the user did not send the !button message.
                return;
            }

            // Generate the button id.
            Guid buttonId = Guid.NewGuid();

            // Add the button id to the ActiveButtons list
            ActiveButtons.Add(buttonId, DateTime.UtcNow);

            // Have the button use our id.
            // Each button id is unique to it's attached message, not globally!
            DiscordButtonComponent randomButton = new(ButtonStyle.Primary, buttonId.ToString(), "Random Button!");
            DiscordMessageBuilder messageBuilder = new() { Content = "Here's a button that sends random numbers! It'll expire in 5 minutes." };
            messageBuilder.AddComponents(randomButton);

            // Send the message with the button.
            await eventArgs.Message.RespondAsync(messageBuilder);
        }

        // Invoked when someone clicks a component
        public static async Task HandleButtonAsync(DiscordClient client, ComponentInteractionCreateEventArgs eventArgs)
        {
            // Test if the button id is valid.
            if (!Guid.TryParse(eventArgs.Id, out Guid buttonId))
            {
                return;
            }
            // Check if the Guid is related to our application.
            else if (!ActiveButtons.TryGetValue(buttonId, out DateTime expireTime))
            {
                return;
            }
            // If the button has expired, then:
            // - Respond by editing the message, disabling the button
            // - Create a follow up message giving the user their final random number.
            else if (DateTime.UtcNow > expireTime)
            {
                // Recreate our message, with the button disabled. There is a round-about way to copy the contents of the message directly, though tedious.
                DiscordButtonComponent randomButton = new(ButtonStyle.Primary, buttonId.ToString(), "Random Button!", true);
                DiscordInteractionResponseBuilder disabledMessageBuilder = new() { Content = $"Your button that sends random numbers has expired. Use {Formatter.InlineCode("!button")} to get another one!" };
                disabledMessageBuilder.AddComponents(randomButton);

                // Because the button has expired, remove the button from the ActiveButtons list
                ActiveButtons.Remove(buttonId);

                // Edit the original message, disabling the button.
                await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, disabledMessageBuilder);

                // Give the user their final random number through a followup message.
                await eventArgs.Interaction.CreateFollowupMessageAsync(new()
                {
                    Content = $"Your random number is {Random.Shared.Next(1, 10)}",
                    // Ephemeral means that only the user can see the message.
                    IsEphemeral = true
                });
            }
            // If the button has not expired, then give the user their random number.
            else
            {
                await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
                {
                    Content = $"Your random number is {Random.Shared.Next(1, 10)}",
                    IsEphemeral = true
                });
            }
        }
    }
}
