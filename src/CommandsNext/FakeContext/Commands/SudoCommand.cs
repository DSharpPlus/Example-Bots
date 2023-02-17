using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DSharpPlus.ExampleBots.CommandsNext.FakeContext.Commands
{
    public sealed class SudoCommand : BaseCommandModule
    {
        [Command("sudo"), Description("Runs the command as another user.")]
        [RequireUserPermissions(Permissions.Administrator), RequireGuild]
        public async Task PingAsync(CommandContext context, DiscordMember member, [RemainingText] string arguments)
        {
            // First we try to find the command
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
    }
}
