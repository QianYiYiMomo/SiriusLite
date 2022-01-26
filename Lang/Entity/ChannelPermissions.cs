using System.Text.Json.Serialization;
namespace Lang.Entity
{
    /// <summary>
    /// 子频道权限对象
    /// </summary>
    public struct ChannelPermissions
    {
        /// <summary>
        /// 子频道 id
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string ChannelId { get; set; }
        /// <summary>
        /// 用户 id
        /// </summary>
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
        /// <summary>
        /// 用户拥有的子频道权限
        /// 权限使用位图表示，传递时序列化为十进制数值字符串。如权限值为0x6FFF，会被序列化为十进制"28671"。
        /// 
        /// 可查看子频道(用户) =1	目前仅支持指定成员可见类型，不支持身份组可见类型
        /// 可管理子频道(管理) =2	创建者、管理员、子频道管理员都具有此权限
        /// </summary>
        [JsonPropertyName("permissions")]
        public string Permissions { get; set; }
    }

    public enum Permissions
    {
        /// <summary>
        /// 可查看子频道
        /// </summary>
        User = 1,
        /// <summary>
        /// 可管理子频道
        /// </summary>
        Admin = 2,
    }
}
