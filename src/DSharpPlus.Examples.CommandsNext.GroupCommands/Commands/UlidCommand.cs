using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DSharpPlus.Examples.CommandsNext.GroupCommands.Commands
{
    // Group is how a command can have subcommands.
    [Group("ulid"), Description("Ulid utility commands.")]
    public class UlidCommand : BaseCommandModule
    {
        // The group command is executed when no subcommands are specified.
        // It's allowed to have arguments and such, just like a normal command.
        [GroupCommand]
        public async Task ExecuteAsync(CommandContext context) => await context.RespondAsync($"Ulid utility commands. Use `help ulid` for subcommands. The current Ulid is {Formatter.InlineCode(Ulid.NewUlid().ToString())}");

        // This subcommand is executed as `!ulid create`
        [Command("create"), Description("Generate a new ULID.")]
        public async Task CreateAsync(CommandContext context) => await context.RespondAsync(Formatter.InlineCode(Ulid.NewUlid().ToString()));

        // The user would run this subcommand as `!ulid info 01HBF5ZGDHXAJ85HKMBGCJTT99`, where `01HBF5ZGDHXAJ85HKMBGCJTT99` is any valid Ulid.
        [Command("info"), Description("Returns information about a Ulid.")]
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
