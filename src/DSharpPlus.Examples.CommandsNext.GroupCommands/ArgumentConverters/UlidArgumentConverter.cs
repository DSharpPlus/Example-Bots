using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;

namespace DSharpPlus.Examples.CommandsNext.GroupCommands.ArgumentConverters
{
    public sealed class UlidArgumentConverter : IArgumentConverter<Ulid>
    {
        public Task<Optional<Ulid>> ConvertAsync(string value, CommandContext ctx)
        {
            if (Ulid.TryParse(value, out Ulid ulid))
            {
                return Task.FromResult(Optional.FromValue(ulid));
            }

            return Task.FromResult(Optional.FromNoValue<Ulid>());
        }
    }
}
