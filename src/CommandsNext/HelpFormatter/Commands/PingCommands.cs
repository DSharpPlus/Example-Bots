using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.HelpFormatter.Commands
{
    public sealed class PingCommand : BaseCommandModule
    {
        [Command("ping"), Description("Pings the bot and returns the gateway latency."), Aliases("pong")]
        public async Task PingAsync(CommandContext context)
        {
            await context.RespondAsync($"Pong! The gateway latency is {context.Client.Ping}ms.");
        }
    }
}
