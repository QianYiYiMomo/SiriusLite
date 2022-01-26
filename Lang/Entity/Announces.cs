using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 公告对象
    /// </summary>
    public struct Announces
    {
        /// <summary>
        /// 频道 id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; set; }
        /// <summary>
        /// 子频道 id
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string Channeld { get; set; }
        /// <summary>
        /// 消息 id
        /// </summary>
        [JsonPropertyName("message_id")]
        public string MessageId { get; set; }
    }
}
