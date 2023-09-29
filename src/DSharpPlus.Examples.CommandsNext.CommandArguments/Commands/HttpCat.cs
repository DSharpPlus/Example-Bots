using System;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace DSharpPlus.Examples.CommandsNext.CommandArguments.Commands
{
    public class HttpCatCommand : BaseCommandModule
    {
        // Sometimes, when my strings get really long, I like to put them in constants.
        private const string HTTP_COMMAND = "httpcat";
        private const string HTTP_COMMAND_DESCRIPTION = "Returns a http.cat image for the provided HTTP status code.";
        private const string HTTP_COMMAND_CODE_DESCRIPTION = "An optional parameter to specify which http code to send. If no argument is provided, a random http code is chosen.";

        private static readonly HttpStatusCode[] _validHttpCodes = Enum.GetValues<HttpStatusCode>();

        // Because the code parameter has a default value (null), this makes the parameter optional.
        // Parameters do NOT become optional when they're nullable.
        // Parameters DO become optional when they have a default value.
        [Command(HTTP_COMMAND), Description(HTTP_COMMAND_DESCRIPTION)]
        public async Task ExecuteAsync(CommandContext context, [Description(HTTP_COMMAND_CODE_DESCRIPTION)] int? code = null)
        {
            // If no code is provided, we'll just pick a random one.
            code ??= (int)_validHttpCodes[Random.Shared.Next(_validHttpCodes.Length)];

            // Cast to an int so we can use it in the URL.
            await context.RespondAsync($"https://http.cat/{code}");
        }
    }
}
