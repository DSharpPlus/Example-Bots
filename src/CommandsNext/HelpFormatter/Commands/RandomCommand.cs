using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.HelpFormatter.Commands
{
    public sealed class RandomCommand : BaseCommandModule
    {
        [Command("random")]
        [Aliases("r", "rand")]
        [Description("Returns a random number within the specified range. Defaults between 1 and 10.")]
        public async Task RandomAsync(CommandContext context, int min = 0, int max = 10)
        {
            // Ensure that the minimum value is less than or equal to the maximum value.
            if (min > max)
            {
                // Inform the user that they messed up.
                await context.RespondAsync("The minimum value must be less than or equal to the maximum value.");
                return;
            }

            // Respond with the random number.
            await context.RespondAsync($"Your random number is {Random.Shared.Next(min, max)}.");
        }
    }
}
