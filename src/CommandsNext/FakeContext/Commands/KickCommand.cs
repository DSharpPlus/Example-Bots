using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;

namespace DSharpPlus.ExampleBots.CommandsNext.FakeContext.Commands
{
    public sealed class KickCommand : BaseCommandModule
    {
        // We use the RequirePermissions attribute to ensure that the user invoking the command has the required permissions.
        [Command("kick"), Description("Removes a member from the guild.")]
        [RequirePermissions(Permissions.KickMembers), RequireGuild]
        public async Task KickAsync(CommandContext context, DiscordMember member, [RemainingText] string reason = "No reason provided.")
        {
            // The RemainingText attribute will capture all text after the argument.

            // Try to dm the member notifying them that they've been kicked.
            try
            {
                await member.SendMessageAsync($"You've been kicked from {context.Guild.Name}: {reason}");
            }
            catch (DiscordException)
            {
                // Sometimes the member has blocked the bot or their DM's are off.
                // Methods that execute rest requests will throw their HTTP related exceptions, such as
                // NotFoundException, BadRequestException, UnauthorizedException, etc.
            }

            // Ensure that the bot can kick the member. Hierarchy is the user's highest role's position.
            // An example is the admin role position, which is usually at the top of the member list.
            if (member.Hierarchy >= context.Guild.CurrentMember.Hierarchy)
            {
                await context.RespondAsync("You cannot kick this member.");
                return;
            }

            // Because of the RequirePermissions attribute and the hierarchy check, we can safely assume that we can remove this member.
            await member.RemoveAsync(reason);

            // We're going to use a message builder to ensure that the bot doesn't ping anyone.
            DiscordMessageBuilder messageBuilder = new() { Content = $"{member.Mention} has been kicked from the guild: {reason}" };
            messageBuilder.WithAllowedMentions(Mentions.None);
            await context.RespondAsync(messageBuilder);
        }
    }
}
