using System.Text.Json.Serialization;

namespace Lang.Entity.ClientEntity
{
    public class EventBase
    {
        [JsonPropertyName("op")]
        public Data.OpCode Type;
        [JsonPropertyName("d")]
        public object Data;
    }
}
