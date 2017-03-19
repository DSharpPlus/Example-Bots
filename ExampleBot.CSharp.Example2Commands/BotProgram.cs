using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Commands;
using Newtonsoft.Json;

// Note that to use commands you will need to 
// build the commands module from git first.
// Then you need to reference it.

namespace ExampleBot.CSharp
{
    public sealed class BotProgram
    {
        private BotConfig Configuration { get; set; }
        private DiscordClient Client { get; set; }
        private CommandModule Commands { get; set; }

        public static void Main(string[] args)
        {
            if (!File.Exists("config.json"))
                throw new FileNotFoundException("Bot's configuration file (config.json) was not found.");

            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs))
                json = sr.ReadToEnd();

            var cfg = JsonConvert.DeserializeObject<BotConfig>(json);

            var bot = new BotProgram(cfg);
            bot.StartAsync().GetAwaiter().GetResult();
        }

        public BotProgram(BotConfig config)
        {
            this.Configuration = config;
        }

        public async Task StartAsync()
        {
            this.Client = new DiscordClient(new DiscordConfig
            {
                Token = this.Configuration.Token,
                TokenType = TokenType.Bot,
                LogLevel = LogLevel.Debug,
                AutoReconnect = true
            });

            this.Client.DebugLogger.LogMessageReceived += (o, e) =>
            {
                Console.WriteLine($"[{e.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")}] [{e.Level}] {e.Message}");
            };

            // Let's enable the command service and
            // configure it with # prefix.
            this.Commands = this.Client.UseCommands(new CommandConfig
            {
                Prefix = "#",
                SelfBot = false
            });

            // Let's add a ping command, that responds 
            // with pong
            this.Commands.AddCommand("ping", async e =>
            {
                await e.Message.Respond("pong");
            });

            // Now a hello command, that responds with 
            // invoker's mention.
            this.Commands.AddCommand("hello", async e =>
            {
                await e.Message.Respond($"Hello, {e.Author.Mention}!");
            });

            // Now if you run the bot, try invoking 
            // #ping or #hello.

            await this.Client.Connect();

            await Task.Delay(-1);
        }

        private void DebugLogger_LogMessageReceived(object sender, DebugLogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")}] [{e.Level}] {e.Message}");
        }
    }
}
