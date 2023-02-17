using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.ArgumentConverters.Commands
{
    [Group("ulid")]
    public sealed class UlidCommand : BaseCommandModule
    {
        [Command("generate"), Description("Generates a new Ulid from the current timestamp."), Aliases("gen")]
        [SuppressMessage("Style", "IDE0022", Justification = "Paragraph.")]
        public async Task GenerateAsync(CommandContext context)
        {
            // Return the Ulid in Base64 format, inlined as a code block.
            await context.RespondAsync(Formatter.InlineCode(Ulid.NewUlid().ToBase64()));
        }

        [Command("parse"), Description("Returns the timestamp that the Ulid was generated at.")]
        [SuppressMessage("Style", "IDE0022", Justification = "Paragraph.")]
        public async Task ParseAsync(CommandContext context, Ulid ulid)
        {
            // Return the timestamp that the Ulid was generated at.
            await context.RespondAsync(Formatter.Timestamp(ulid.Time, TimestampFormat.RelativeTime));
        }
    }
}
