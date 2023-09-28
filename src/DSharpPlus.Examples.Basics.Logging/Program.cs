using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace DSharpPlus.Examples.Basics.Logging
{
    public class Program
    {
        public static async Task Main()
        {
            // Check for token
            // TODO: Load the token up from IConfiguration
            string? token = Environment.GetEnvironmentVariable("DISCORD_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Please set the environment variable DISCORD_TOKEN.");
                return;
            }

            IServiceCollection services = new ServiceCollection();

            // TODO: Add custom output format
            services.AddLogging(builder => builder.AddSerilog(new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/DSharpPlus.Examples.Basics.Logging..log", rollingInterval: RollingInterval.Day)
                .MinimumLevel.Override("DSharpPlus", LogEventLevel.Information) // DSharpPlus is very verbose, so we set it to only log Information and above
                .MinimumLevel.Debug() // The rest of our application is less verbose, so we set it to Debug
                .CreateLogger()));

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Create the client
            DiscordClient discord = new(new DiscordConfiguration
            {
                Token = token,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
                // Our own logger factory
                LoggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>()
                // To disable logging:
                //LoggerFactory = NullLoggerFactory.Instance
            });

            // Create an activity to display on Discord
            DiscordActivity activity = new("some logs fall...", ActivityType.Watching);

            // Connect to the Discord gateway
            await discord.ConnectAsync(activity, UserStatus.Idle);

            ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Discord bot started!");

            // Wait infinitely so the bot stays connected; DiscordClient.ConnectAsync is not a blocking call
            await Task.Delay(-1);
        }
    }
}
