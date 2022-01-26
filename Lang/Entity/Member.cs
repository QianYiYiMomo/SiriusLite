using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 成员对象
    /// </summary>
    public class Member
    {
        /// <summary>
        /// 用户基础信息，来自QQ资料，只有成员相关接口中会填充此信息
        /// </summary>
        [JsonPropertyName("user")]
        public User User { set; get; }
        /// <summary>
        /// 用户在频道内的昵称
        /// </summary>
        [JsonPropertyName("nick")]
        public string Nick { set; get; }
        /// <summary>
        /// 用户在频道内的身份组ID, 默认值可参考DefaultRoles
        /// </summary>
        [JsonPropertyName("roles")]
        public string[] Roles { set; get; }
        /// <summary>
        /// 用户加入频道的时间
        /// </summary>
        [JsonPropertyName("joined_at")]
        public string JoinedAt { set; get; }
        
    }
}
