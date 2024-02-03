using System.Threading.Tasks;
using DSharpPlus.Commands.Processors.TextCommands.Attributes;
using DSharpPlus.Commands.Trees;
using DSharpPlus.Commands.Trees.Attributes;

namespace DSharpPlus.Examples.Commands.Basics
{
    public sealed class PingCommand
    {
        [Command("ping"), TextAlias("pong")]
        public static ValueTask ExecuteAsync(CommandContext context) => context.RespondAsync($"Pong! Latency is {context.Client.Ping}ms.");
    }
}
