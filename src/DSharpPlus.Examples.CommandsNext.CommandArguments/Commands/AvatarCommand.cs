using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;

namespace DSharpPlus.Examples.CommandsNext.CommandArguments.Commands
{
    public class AvatarCommand : BaseCommandModule
    {
        // We can just request for a DiscordMember directly here. The user will either ping the member, send the member's ID, or send the member's username.
        // Regardless of input, CommandsNext will do it's best to parse it into a DiscordMember.
        [Command("avatar"), Description("Sends the requested user's profile picture in it's own message for easy viewing.")]
        public async Task ExecuteAsync(CommandContext context, DiscordMember member) => await context.RespondAsync(member.AvatarUrl);

        // Oh, but what if the DiscordMember isn't in the server? Well, CommandsNext will fallback to the next overload (this command!)
        // This time though, we'll need a user mention or user id. Usernames won't work here.
        [Command("avatar")]
        public async Task ExecuteAsync(CommandContext context, DiscordUser user) => await context.RespondAsync(user.AvatarUrl);

        // What if no arguments are provided? CommandsNext will intelligently execute this overload first, since it has no arguments.
        // For now we'll just send the user's avatar.
        [Command("avatar")]
        public async Task ExecuteAsync(CommandContext context) => await context.RespondAsync(context.User.AvatarUrl);
    }
}
