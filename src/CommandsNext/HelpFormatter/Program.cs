using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.ExampleBots.CommandsNext.HelpFormatter.Formatters;
using Microsoft.Extensions.Logging;

namespace DSharpPlus.ExampleBots.CommandsNext.HelpFormatter
{
    public class Program
    {
         // C# 7.1 and above supports Async Main; the Main entrypoint can return a Task and be marked as async.
         // You no longer need a Main() which passes execution to a MainAsync() method to use the await keyword.
        public static async Task Main()
        {
            // Retrieve your Discord token from an environment variable.
            // Before running this example, be sure to open the .env file in the root directory and paste in your Discord bot token.
            string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Please specify a token in the DISCORD_TOKEN environment variable.");
                Environment.Exit(1);
                
                return; // Early return for the compiler's nullability; unreachable code.
            }

            // Configure and instantiate a DiscordClient
            DiscordConfiguration config = new()
            {
                Token = token,
                MinimumLogLevel = LogLevel.Debug,
                
                // HEADS UP: the Message Contents intent is a 'privileged' intent which must be enabled
                // for your app on the Discord Developer Portal: https://discord.com/developers/applications
                // This example will not function properly if it is not enabled.
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };
            
            DiscordClient client = new(config);
            
            // Enable the CommandsNext extension on your DiscordClient instance
            CommandsNextConfiguration commandsConfig = new() { StringPrefixes = new[] { "!" } };
            CommandsNextExtension commandsNext = client.UseCommandsNext(commandsConfig);

            // Set a custom help formatter for your CommandsNext instance.
            commandsNext.SetHelpFormatter<CustomHelpFormatter>();

            // Register your commands with your CommandsNext instance. The method below has many overloads, one of which accepts an Assembly.
            // By providing the Assembly for this app, CommandsNext will automatically register any class that inherits from BaseCommandModule.
            commandsNext.RegisterCommands(typeof(Program).Assembly);

            // Connect to Discord
            await client.ConnectAsync();

            // Once your program reaches the end of the Main entrypoint method, it will terminate execution and close.
            // Prevent premature termination, we'll infinitely delay the execution of this method to allow background threads and Tasks to run. 
            await Task.Delay(-1);
        }
    }
}

