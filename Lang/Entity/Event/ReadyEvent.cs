using System.Text.Json.Serialization;

namespace Lang.Entity.Event
{
    /// <summary>
    /// 当成功鉴权连接到服务器会返回以下消息~
    /// </summary>
    public class ReadyEvent
    {
        /// <summary>
        /// 不清楚版本是什么意思
        /// </summary>
        [JsonPropertyName("version")]
        public int Version { get; set; }
        /// <summary>
        /// 这个ID可用于重连机器人
        /// </summary>
        [JsonPropertyName("session_id")]
        public string Session_Id { get; set; }
        /// <summary>
        /// 机器人的数据
        /// </summary>
        [JsonPropertyName("user")]
        public User User { get; set; }
        /// <summary>
        /// 消息分片,没啥用.png qwq
        /// </summary>
        [JsonPropertyName("shard")]
        public List<int> Shard { get; set; }
    }
}
