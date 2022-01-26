using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 频道身份组对象
    /// </summary>
    public struct Role
    {
        /// <summary>
        /// 身份组ID
        /// DefaultRoleIDs(系统默认生成下列身份组ID)
        /// 身份组ID默认值
        /// 1	全体成员
        /// 2	管理员
        /// 4	群主/创建者
        /// 5	子频道管理员
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// ARGB的HEX十六进制颜色值转换后的十进制数值
        /// </summary>
        [JsonPropertyName("color")]
        public uint Color { get; set; }
        /// <summary>
        /// 是否在成员列表中单独展示: 0-否, 1-是
        /// </summary>
        [JsonPropertyName("hoist")]
        public int Hoist { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        [JsonPropertyName("number")]
        public int Number { get; set; }
        /// <summary>
        /// 成员上限
        /// </summary>
        [JsonPropertyName("member_limit")]
        public int MemberLimit { get; set; }
    }
}
