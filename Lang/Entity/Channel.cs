using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 子频道对象
    /// </summary>
    public struct Channel
    {
        /// <summary>
        /// 子频道id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 频道id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string GuildId { set; get; }
        /// <summary>
        /// 	子频道名
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { set; get; }
        /// <summary>
        /// 子频道类型 请使用Lang.Entity.ChannelType
        /// </summary>
        [JsonPropertyName("type")]
        public int ChannelType { set; get; }
        /// <summary>
        /// 子频道子类型 请使用Lang.Entity.ChannelSubType
        /// </summary>
        [JsonPropertyName("sub_type")]
        public int ChannelSubType { set; get; }
        /// <summary>
        /// 排序，必填，而且不能够和其他子频道的值重复
        /// </summary>
        [JsonPropertyName("position")]
        public int Position { set; get; }
        /// <summary>
        /// 分组 id
        /// </summary>
        [JsonPropertyName("parent_id")]
        public string ParentId { set; get; }
        /// <summary>
        /// 创建人 id
        /// </summary>
        [JsonPropertyName("owner_id")]
        public string OwnerId { set; get; }
        /// <summary>
        /// 子频道私密类型
        /// </summary>
        [JsonPropertyName("private_type")]
        public int PrivateType { set; get; }
    }
    public enum ChannelType
    {
        /// <summary>
        /// 文字子频道
        /// </summary>
        TextSubChannel = 0,
        // 1 = 保留
        /// <summary>
        /// 语音子频道
        /// </summary>
        VoiceSubChannel = 2,
        // 3 = 保留
        /// <summary>
        /// 子频道分组
        /// </summary>
        SubChannelGrouping = 4,
        /// <summary>
        /// 直播子频道
        /// </summary>
        LiveSubChannel = 10005,
        /// <summary>
        /// 应用子频道
        /// </summary>
        AppSubChannel = 10006,
        /// <summary>
        /// 论坛子频道
        /// </summary>
        ForumSubChannel = 10007,
    };

    public enum ChannelSubType
    {
        /// <summary>
        /// 闲聊
        /// </summary>
        Chat = 0,
        /// <summary>
        /// 公告
        /// </summary>
        Announce = 1,
        /// <summary>
        /// 攻略
        /// </summary>
        Strategy  = 2,
        /// <summary>
        /// 开黑
        /// </summary>
        Gang = 3,
    }
    public enum ChannelPrivateType
    {
        ChannelPublic = 1,//公开频道
        ChannelAdmin = 2,//群主管理员可见
        ChannelAdminAnd = 3, //群主管理员+指定成员
    }
}
