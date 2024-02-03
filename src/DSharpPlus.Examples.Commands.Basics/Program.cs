using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using DSharpPlus.Commands.Processors.TextCommands.Parsing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DSharpPlus.Examples.Commands.Basics
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new();
            // you can also use environment vars, just uncomment this
            // configurationBuilder.AddEnvironmentVariables("");
            configurationBuilder.AddJsonFile("config.json", true, true);
            configurationBuilder.AddCommandLine(args);

            IConfiguration configuration = configurationBuilder.Build();
            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(configuration);
            Assembly currentAssembly = typeof(Program).Assembly;

            serviceCollection.AddSingleton(async serviceProvider =>
            {
                DiscordClient client = new(new DiscordConfiguration()
                {
                    Token = configuration.GetValue<string>("example_bot:token") ??
                            throw new InvalidOperationException("Missing Discord token."),
                    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents | TextCommandProcessor.RequiredIntents
                });

                CommandsExtension extension = client.UseCommands(new()
                {
                    DebugGuildId = configuration.GetValue<ulong?>("example_bot:debug_guild_id", null),
                    ServiceProvider = serviceProvider,
                    
                });
                await extension.AddProcessorAsync(new TextCommandProcessor());
                await extension.AddProcessorAsync(new SlashCommandProcessor());
                extension.AddCommands(currentAssembly);

                return client;
            });
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            DiscordClient client = await serviceProvider.GetRequiredService<Task<DiscordClient>>();
            await client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
