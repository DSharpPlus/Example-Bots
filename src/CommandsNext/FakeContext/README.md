# DSharpPlus CommandsNext Example: Fake Context!
This example shows how to use the `CommandsNextExtension` to create a fake context which is useful when you want to execute a command from within your code.

# Creating the context:
```cs
public async Task SudoAsync(CommandContext context, DiscordMember member, [RemainingText] string arguments)
{
    // First we try to find the command. This is a required argument for the fake context.
    Command? command = context.CommandsNext.FindCommand(arguments, out string? rawArguments);
    if (command is null)
    {
        await context.RespondAsync("Command not found.");
        return;
    }

    // Then we create the fake context
    CommandContext fakeContext = context.CommandsNext.CreateFakeContext(member, context.Channel, arguments, context.Prefix, command, rawArguments);

    // Then we execute the command
    await context.CommandsNext.ExecuteCommandAsync(fakeContext);
}
```