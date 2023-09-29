using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.Examples.CommandsNext.CustomCommandArguments.Commands
{
    public class CreateUlidCommand : BaseCommandModule
    {
        [Command("create_ulid"), Description("Creates a new Ulid.")]
        public async Task ExecuteAsync(CommandContext context) => await context.RespondAsync(Formatter.InlineCode(Ulid.NewUlid().ToString()));
    }
}
