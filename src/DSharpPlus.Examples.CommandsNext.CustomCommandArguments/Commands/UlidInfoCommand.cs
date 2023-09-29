using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DSharpPlus.Examples.CommandsNext.CustomCommandArguments.Commands
{
    public class UlidInfoCommand : BaseCommandModule
    {
        [Command("ulid_info"), Description("Returns information about a Ulid.")]
        public async Task ExecuteAsync(CommandContext context, Ulid ulid)
        {
            DiscordEmbedBuilder embedBuilder = new();
            embedBuilder.WithTitle("Ulid Information");
            embedBuilder.WithColor(DiscordColor.Blurple);
            embedBuilder.WithDescription($"Ulid: {Formatter.InlineCode(ulid.ToString())}");
            embedBuilder.AddField("Base64", Formatter.InlineCode(ulid.ToBase64()));
            embedBuilder.AddField("Timestamp", Formatter.Timestamp(ulid.Time, TimestampFormat.LongDateTime));
            embedBuilder.AddField("Randomness", Formatter.InlineCode(Convert.ToBase64String(ulid.Random)));

            await context.RespondAsync(embedBuilder.Build());
        }
    }
}
