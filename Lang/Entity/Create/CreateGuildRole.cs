using System.Text.Json.Serialization;

namespace Lang.Entity.Create
{
    /// <summary>
    /// 创建频道身份组
    /// </summary>
    public class CreateGuildRole
    {
        /// <summary>
        /// 标识需要设置哪些字段
        /// </summary>
        [JsonPropertyName("filter")]
        public Filter Filter { get; set; }
        /// <summary>
        /// 携带需要设置的字段内容
        /// </summary>
        [JsonPropertyName("info")]
        public Info Info { get; set; }
    }
    public class GetGuildRoleInfo
    {
        /// <summary>
        /// 频道ID
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string Guild_Id { get; set; }
        /// <summary>
        /// 一组频道身份组对象
        /// </summary>
        [JsonPropertyName("roles")]
        public Role[] Roles { get; set; }
        /// <summary>
        /// 默认分组上限
        /// </summary>
        [JsonPropertyName("role_num_limit")]
        public string RoleNum_Limit { get; set; }
    }
    public class Filter
    {
        public Filter(int name, int color, int hoist)
        {
            Name = name;
            Color = color;
            Hoist = hoist;
        }
        /// <summary>
        /// 是否设置名称: 0-否, 1-是
        /// </summary>
        [JsonPropertyName("name")]
        public int Name { get; set; }
        /// <summary>
        /// 是否设置颜色: 0-否, 1-是
        /// </summary>
        [JsonPropertyName("color")]
        public int Color { get; set; }
        /// <summary>
        /// 是否设置在成员列表中单独展示: 0-否, 1-是
        /// </summary>
        [JsonPropertyName("hoist")]
        public int Hoist { get; set; }
    }
    public class Info
    {
        public Info (string name, uint color, int hoist)
        {
            Name = name;
            Color = color;
            Hoist = hoist;
        }
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
        /// 在成员列表中单独展示: 0-否, 1-是
        /// </summary>
        [JsonPropertyName("hoist")]
        public int Hoist { get; set; }
    }
    /// <summary>
    /// 创建频道身份组返回的信息
    /// </summary>
    public class CreateGuildRoleResult
    {
        /// <summary>
        /// 身份组ID
        /// </summary>
        [JsonPropertyName("role_id")]
        public string Role_Id { get; set; }
        /// <summary>
        /// 所创建的频道身份组对象
        /// </summary>
        [JsonPropertyName("role")]
        public Role Role { get; set; }
    }
}