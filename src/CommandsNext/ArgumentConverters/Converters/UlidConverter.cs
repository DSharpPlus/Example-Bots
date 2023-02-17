using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace DSharpPlus.ExampleBots.CommandsNext.ArgumentConverters.Converters
{
    // Inherit from IArgumentConverter<T>, where T is the type you want to convert to.
    public sealed class UlidConverter : IArgumentConverter<Ulid>
    {
        [SuppressMessage("Style", "IDE0046", Justification = "Readability.")]
        public Task<Optional<Ulid>> ConvertAsync(string value, CommandContext context)
        {
            // Attempt to parse the value as an Ulid.
            if (Ulid.TryParse(value, out Ulid ulid))
            {
                // Because our logic is synchronous, we can use Task.FromResult.
                return Task.FromResult(Optional.FromValue(ulid));
            }
            else
            {
                // We use Optional.FromNoValue to indicate that the conversion failed.
                return Task.FromResult(Optional.FromNoValue<Ulid>());
            }
        }
    }
}
