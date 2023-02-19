# DSharpPlus CommandsNext Example: Hello World!
This example registers commands using our command framework, which makes command handling a lot easier. All the other CommandsNext examples will be loosely based off of this one. Ensure that you have the [message intent enabled](https://discord.com/developers/docs/change-log#message-content-is-a-privileged-intent).

This example also introduces DI, which injects your services/classes into your commands. This is useful for things like database contexts, logging or other services.

# Creating a command
Commands are created by creating a method with the `Command` attribute. The method must be inside of a class that inherits `BaseCommandModule` and must return a `Task`. The method must also have a `CommandContext` as its first parameter.

```cs
// Inherit from BaseCommandModule so that CommandsNext can recognize this class as a command.
public sealed class PingCommand : BaseCommandModule
{
    // Register the method as a command, specifying the name and description.
    // Unfortunately, CommandsNext does not support static methods.
    [Command("ping"), Description("Pings the bot and returns the gateway latency."), Aliases("pong")]
    public async Task PingAsync(CommandContext context)
    {
        // The CommandContext provides access to the DiscordClient, the message, the channel, etc.
        // If the CommandContext is not provided as a parameter, CommandsNext will ignore the method.
        // Additionally, without the CommandContext, it would be impossible to respond to the user.
        await context.RespondAsync($"Pong! The gateway latency is {context.Client.Ping}ms.");
    }
}
```

# Registering the command
```cs
// Register CommandsNext
CommandsNextConfiguration commandsConfig = new()
{
    // Add the service provider which will allow CommandsNext to inject the Random instance.
    Services = serviceCollection.BuildServiceProvider(),
    StringPrefixes = new[] { "!" }
};
CommandsNextExtension commandsNext = client.UseCommandsNext(commandsConfig);

// Register commands
// CommandsNext will search the assembly for any classes that inherit from BaseCommandModule and register them as commands.
commandsNext.RegisterCommands(typeof(Program).Assembly);
```