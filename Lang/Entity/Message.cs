using System.Text.Json.Serialization;

namespace Lang.Entity
{
    public struct Message
    {
        /// <summary>
        /// 消息 id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 子频道 id
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string Channel_Id { get; set; }
        /// <summary>
        /// 频道 id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string Guild_Id { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [JsonPropertyName("content")]
        public string Content { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; }
        /// <summary>
        /// 消息编辑时间
        /// </summary>
        [JsonPropertyName("edited_timestamp")]
        public string EditedTimestamp { get; set; }
        /// <summary>
        /// 是否是@全员消息
        /// </summary>
        [JsonPropertyName("mention_everyone")]
        public bool MentionEveryone { get; set; }
        /// <summary>
        /// 消息创建者
        /// </summary>
        [JsonPropertyName("author")]
        public User Author { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        [JsonPropertyName("attachments")]
        public MessageAttachment[] Attachments { get; set; }
        /// <summary>
        /// embed
        /// </summary>
        [JsonPropertyName("embeds")]
        public MessageEmbed[] Embeds { get; set; }
        /// <summary>
        /// 消息中@的人
        /// </summary>
        [JsonPropertyName("mentions")]
        public User[] Mentions { get; set; }
        /// <summary>
        /// 消息创建者的member信息
        /// </summary>
        [JsonPropertyName("member")]
        public Member Member { get; set; }
        /// <summary>
        /// ark消息
        /// </summary>
        [JsonPropertyName("ark")]
        public MessageArk MsgArk { get; set; }
    }
    /// <summary>
    /// embeds
    /// </summary>
    public class MessageEmbed
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }
        /// <summary>
        /// 消息弹窗内容
        /// </summary>
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        [JsonPropertyName("thumbnail")]
        public MessageEmbedThumbnail Thumbnail { get; set; }
        /// <summary>
        /// 消息创建时间
        /// </summary>
        [JsonPropertyName("fields")]
        public MessageEmbedField Fields { get; set; }
    }
    /// <summary>
    /// 信息嵌入字段 图片
    /// </summary>
    public class MessageEmbedThumbnail
    {
        /// <summary>
        /// 图片地址
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class MessageEmbedField
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
    /// <summary>
    /// 信息附件
    /// </summary>
    public class MessageAttachment
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string url { get; set; }
    }
    public class MessageArk
    {
        /// <summary>
        /// ark模板id（需要先申请）
        /// </summary>
        [JsonPropertyName("template_id")]
        public int Template_Id { get; set; }
        /// <summary>
        /// kv值列表
        /// </summary>
        [JsonPropertyName("kv")]
        public MessageArkKv[] MsgArkKV { get; set; }
    }
    public class MessageArkKv
    {
        [JsonPropertyName("key")]
        public string key { get; set; }
        [JsonPropertyName("value")]
        public string value { get; set; }
        [JsonPropertyName("obj")]
        public MessageArkObj[] ArkObj { get; set; }
    }
    public class MessageArkObj
    {
        [JsonPropertyName("obj_kv")]
        public MessageArkObjKv[] MsgObjKv { get; set; }
    }
    public class MessageArkObjKv
    {
        public string key { get; set; }
        public string value { set; get; }
    }
}
