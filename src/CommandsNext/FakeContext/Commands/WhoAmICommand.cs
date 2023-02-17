using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.FakeContext.Commands
{
    public sealed class WhoAmICommand : BaseCommandModule
    {
        [Command("whoami"), Description("Says who you are."), RequireGuild]
        public async Task WhoAmIAsync(CommandContext context) => await context.RespondAsync($"You are {context.Member!.Mention}.");
    }
}
