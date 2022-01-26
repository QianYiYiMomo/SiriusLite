using System.Text.Json.Serialization;

namespace Lang.Entity
{
    public class MemberWithGuildID
    {
        /// <summary>
        /// 频道id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { get; set; }
        /// <summary>
        /// 用户基础信息
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; set; }
        /// <summary>
        /// 用户在频道内的昵称
        /// </summary>
        [JsonPropertyName("nick")]
        public string Nick { get; set; }
        /// <summary>
        /// 用户在频道内的身份
        /// </summary>
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; }
        /// <summary>
        /// 用户加入频道的时间
        /// </summary>
        [JsonPropertyName("joined_at")]
        public DateTime JoinedAt { get; set; }
    }
}
