using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.ExampleBots.CommandsNext.HelloWorld.Commands
{
    [Group("timestamp"), Description("Handles timestamp manipulation.")]
    public sealed class TimestampCommand : BaseCommandModule
    {
        // Discord snowflakes use 2015 (the launch date) as it's epoch for timestamps.
        private static readonly DateTimeOffset DiscordEpoch = new(2015, 1, 1, 0, 0, 0, TimeSpan.Zero);

        [Command("parse"), Description("Displays when a snowflake was made.")]
        public async Task ParseAsync(CommandContext context, [Description("The snowflake to grab the creation time from.")] ulong snowflake)
        {
            // Grab the timestamp from the snowflake.
            // If you wish to do this manually: https://discord.com/developers/docs/reference#snowflakes
            DateTimeOffset timestamp = snowflake.GetSnowflakeTime();
            await context.RespondAsync($"The snowflake was created at {Formatter.Timestamp(timestamp, TimestampFormat.RelativeTime)} (hover for exact measurement).");
        }

        [Command("now"), Description("Displays the current snowflake.")]
        public async Task NowAsync(CommandContext context)
        {
            // Grab the current snowflake.
            // If you wish to do this manually: https://discord.com/developers/docs/reference#snowflakes
            long unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            await context.RespondAsync($"The current snowflake is {Formatter.Timestamp(DiscordEpoch.AddMilliseconds(unixTimestamp >> 22), TimestampFormat.LongDateTime)}.");
        }
    }
}
