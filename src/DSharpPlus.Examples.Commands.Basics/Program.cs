using System;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.Commands;
using DSharpPlus.Commands.Processors.SlashCommands;
using DSharpPlus.Commands.Processors.TextCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DSharpPlus.Examples.Commands.Basics
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new();
            configurationBuilder.AddEnvironmentVariables("");
            configurationBuilder.AddCommandLine(args);

            IConfiguration configuration = configurationBuilder.Build();
            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(configuration);
            Assembly currentAssembly = typeof(Program).Assembly;


            serviceCollection.AddSingleton(serviceProvider =>
            {
                DiscordClient client = new(new DiscordConfiguration()
                {
                    Token = configuration.GetValue<string>("discord:token") ??
                            throw new InvalidOperationException("Missing Discord token."),
                    Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
                });

                CommandsExtension extension = client.UseCommands(new()
                {
                    DebugGuildId = configuration.GetValue<ulong?>("discord:debug_guild_id", null),
                    ServiceProvider = serviceProvider,
                });

                extension.AddProcessorAsync(new TextCommandProcessor());
                extension.AddProcessorAsync(new SlashCommandProcessor());
                extension.AddCommands(currentAssembly);

                return client;
            });
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            DiscordClient client = serviceProvider.GetRequiredService<DiscordClient>();
            await client.ConnectAsync();

            await Task.Delay(-1);
        }
    }
}
