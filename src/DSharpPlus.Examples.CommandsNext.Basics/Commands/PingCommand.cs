using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.Examples.CommandsNext.Basics.Commands
{
    public class PingCommand : BaseCommandModule
    {
        [Command("ping"), Description("Checks the bot's latency to the Discord API.")]
        public async Task ExecuteAsync(CommandContext context)
        {
            StringBuilder messageBuilder = new();
            messageBuilder.AppendLine($"Pong! Latency is {context.Client.Ping}ms.");
            messageBuilder.AppendLine($"Additionally, try running {Formatter.InlineCode($"{context.Prefix}help")} to see a list of commands.");

            await context.RespondAsync(messageBuilder.ToString());
        }
    }
}
