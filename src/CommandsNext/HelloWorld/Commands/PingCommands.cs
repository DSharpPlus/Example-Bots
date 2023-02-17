using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.HelloWorld.Commands
{
    // Inherit from BaseCommandModule so that CommandsNext can recognize this class as a command.
    public sealed class PingCommand : BaseCommandModule
    {
        // Register the method as a command, specifying the name and description.
        // Unfortunately, CommandsNext does not support static methods.
        [Command("ping"), Description("Pings the bot and returns the gateway latency."), Aliases("pong")]
        [SuppressMessage("Style", "IDE0022", Justification = "Paragraph.")]
        public async Task PingAsync(CommandContext context)
        {
            // The CommandContext provides access to the DiscordClient, the message, the channel, etc.
            // If the CommandContext is not provided as a parameter, CommandsNext will ignore the method.
            // Additionally, without the CommandContext, it would be impossible to respond to the user.
            await context.RespondAsync($"Pong! The gateway latency is {context.Client.Ping}ms.");
        }
    }
}
