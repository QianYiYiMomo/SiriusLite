using System.Text.Json.Serialization;

namespace Lang.HttpApi.Entity
{
    public class AuthPlugin
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("author")]
        public string Author { get; set; }
        [JsonPropertyName("ver")]
        public string Version { get; set; }
        [JsonPropertyName("dis")]
        public string Description { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("lang")]
        public string Lang { get; set; }
    }
}
