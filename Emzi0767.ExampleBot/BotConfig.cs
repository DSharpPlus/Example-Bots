using Newtonsoft.Json;

namespace Emzi0767.ExampleBot
{
    public sealed class BotConfig
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
    }
}
