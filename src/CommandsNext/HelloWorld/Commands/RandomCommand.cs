using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.HelloWorld.Commands
{
    public sealed class RandomCommand : BaseCommandModule
    {
        // Because we inserted the Random as a singleton within the DI container
        // CommandsNext will inject the instance into this property.
        // This is NOT standard dependency injection, but a feature of CommandsNext.
        // This works with all scopes, including transient.
        public Random Random { get; set; } = new();

        [Command("random"), Description("Returns a random number within the specified range. Defaults between 1 and 10.")]
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
            await context.RespondAsync($"Your random number is {Random.Next(min, max)}.");
        }
    }
}
