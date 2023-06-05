using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.HelpFormatter.Commands
{
    public sealed class SumCommand : BaseCommandModule
    {
        [Command("sum"), Description("Sums all given numbers and returns the total.")]
        public async Task SumAsync(CommandContext context, params int[] numbers)
        {
            // CommandsNext allows us to accept an array of parameters only because we specified params.
            await context.RespondAsync($"The sum of all numbers is {numbers.Sum():N0}.");
        }
    }
}
