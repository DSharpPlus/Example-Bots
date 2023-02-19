# DSharpPlus Core Example: Hello World!
This example connects to the Discord gateway and responds to `!ping`. All the other examples will be loosely based off of this one. Ensure that you have the [message intent enabled](https://discord.com/developers/docs/change-log#message-content-is-a-privileged-intent).

# Creating a client
The client is the main entry point for the library. It is used to connect to the Discord gateway and send/receive Discord payloads.

```cs
// We instantiate our client.
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

// We can specify a status for our bot. Let's set it to "online" and set the activity to "with fire".
DiscordActivity status = new("with fire", ActivityType.Playing);

// Now we connect and log in.
await client.ConnectAsync(status, UserStatus.Online);

// And now we wait infinitely so that our bot actually stays connected.
await Task.Delay(-1);
```