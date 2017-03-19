using Newtonsoft.Json;

namespace ExampleBot.CSharp
{
    public sealed class BotConfig
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
    }
}
