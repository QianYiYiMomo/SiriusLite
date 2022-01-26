using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 获取频道信息会返回的Guild对象
    /// </summary>
    public struct Guild
    {
        /// <summary>
        /// 频道ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 频道名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// 频道头像地址
        /// </summary>
        [JsonPropertyName("icon")]
        public string Icon { get; set; }
        /// <summary>
        /// 创建人用户ID
        /// </summary>
        [JsonPropertyName("owner_id")]
        public string OwnerId { get; set; }
        /// <summary>
        /// 当前人是否是创建人
        /// </summary>
        [JsonPropertyName("owner")]
        public bool Owner { get; set; }
        /// <summary>
        /// 	成员数
        /// </summary>
        [JsonPropertyName("member_count")]
        public int MemberCount { get; set; }
        /// <summary>
        /// 	最大成员数
        /// </summary>
        [JsonPropertyName("max_members")]
        public int MaxMembers { set; get; }
        /// <summary>
        /// 	描述
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { set; get; }
        /// <summary>
        /// 加入时间
        /// </summary>
        [JsonPropertyName("joined_at")]
        public string JoinedAt { get; set; }

    }
}
