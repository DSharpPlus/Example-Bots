# DSharpPlus Core Example: Components!
This example sends a button with the `!button` command and responds with a random number when the user clicks the button. Each button is set to expire after 5 minutes.

# Sending the button
```cs
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
```

# Handling the button
```cs
// Test if the button id is valid.
if (!Guid.TryParse(eventArgs.Id, out Guid buttonId))
{
    return;
}

// Create a response
await eventArgs.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new()
{
    Content = $"Your random number is {Random.Shared.Next(1, 10)}",
    IsEphemeral = true
});
```

# Events used:
- `DiscordClient.MessageCreated`
- `DiscordClient.ComponentInteractionCreated`