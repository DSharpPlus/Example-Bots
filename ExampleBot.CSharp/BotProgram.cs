using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using Newtonsoft.Json;

namespace ExampleBot.CSharp
{
    public sealed class BotProgram
    {
        private BotConfig Configuration { get; set; }
        private DiscordClient Client { get; set; }

        public static void Main(string[] args)
        {
            // First we check whether our configuration exists
            // If it doesn't, let's throw an exception
            if (!File.Exists("config.json"))
                throw new FileNotFoundException("Bot's configuration file (config.json) was not found.");

            // So the config exists, let's load it
            var json = string.Empty;
            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs))
                json = sr.ReadToEnd();

            // Here we convert the JSON file to a bot 
            // configuration file
            var cfg = JsonConvert.DeserializeObject<BotConfig>(json);

            // Let's create and start the bot
            var bot = new BotProgram(cfg);
            bot.StartAsync().GetAwaiter().GetResult();
        }

        public BotProgram(BotConfig config)
        {
            // Let's store the given config so we can use it later
            this.Configuration = config;
        }

        public async Task StartAsync()
        {
            // In order to make our bot tick, we need to create 
            // a new Discord client, using previously-stored
            // configuration
            this.Client = new DiscordClient(new DiscordConfig
            {
                Token = this.Configuration.Token,
                TokenType = TokenType.Bot,
                LogLevel = LogLevel.Debug,
                AutoReconnect = true
            });

            // Let's make sure our client writes any output
            // to our console window
            this.Client.DebugLogger.LogMessageReceived += (o, e) =>
            {
                Console.WriteLine($"[{e.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")}] [{e.Level}] {e.Message}");
            };

            // Now let's add something basic to our bot
            // For instance, let's make our bot respond 
            // pong to every message that says ping
            this.Client.MessageCreated += async e =>
            {
                if (e.Message.Content.ToLower() == "ping")
                    await e.Channel.SendMessage("pong");
            };

            // Finally, let's connect
            await this.Client.Connect();

            // This is so the bot does not exit right after 
            // connecting, to terminate it, press Ctrl+C or 
            // just close the console window
            await Task.Delay(-1);
        }

        private void DebugLogger_LogMessageReceived(object sender, DebugLogMessageEventArgs e)
        {
            Console.WriteLine($"[{e.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss")}] [{e.Level}] {e.Message}");
        }
    }
}
