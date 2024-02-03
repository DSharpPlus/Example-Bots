using System.Threading.Tasks;
using System.Transactions;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.Examples.CommandsNext.ErrorHandling.Commands
{
    public class ErrorCommand : BaseCommandModule
    {
        // I bet you didn't know this exception existed.
        // Unless you did. In which case, my sincerest condolences.
        [Command("error"), Description("Throws an exception.")]
        public Task ExecuteAsync(CommandContext context) => throw new TransactionManagerCommunicationException("This command is supposed to fail.");
    }
}
