using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 表情对象
    /// </summary>
    public struct Emoji
    {
        /// <summary>
        /// 表情ID，系统表情使用数字为ID，emoji使用emoji本身为id，参考 EmojiType 列表
        /// 具体ID https://bot.q.qq.com/wiki/develop/api/openapi/emoji/model.html
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 表情类型
        /// </summary>
        [JsonPropertyName("type")]
        public int EmojiType { get; set; }
    }
    public enum EmojiType
    {
        /// <summary>
        /// QQ官方的表情
        /// 像 /汪汪 /ww 这种
        /// </summary>
        QQEmoji = 0,

        /// <summary>
        /// 普通表情 像是流汗黄豆
        /// (差不多了；) awa
        /// </summary>
        Emoji = 1,
    }
}
