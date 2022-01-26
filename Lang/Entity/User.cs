using System.Text.Json.Serialization;

namespace Lang.Entity
{
    public struct User
    {
        /// <summary>
        /// 用户 id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        /// <summary>
        /// 用户头像地址
        /// </summary>
        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; }
        /// <summary>
        /// 是否是机器人
        /// </summary>
        [JsonPropertyName("bot")]
        public bool Bot { get; set; }
        /// <summary>
        /// 特殊关联应用的 openid，需要特殊申请并配置后才会返回。如需申请，请联系平台运营人员。
        /// </summary>
        [JsonPropertyName("union_openid")]
        public string UnionPpenId { get; set; }
        /// <summary>
        /// 	机器人关联的互联应用的用户信息，与union_openid关联的应用是同一个。如需申请，请联系平台运营人员。
        /// </summary>
        [JsonPropertyName("union_user_account")]
        public string UnionUserAccount { get; set; }
    }
}
