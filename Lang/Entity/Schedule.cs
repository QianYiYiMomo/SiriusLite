using System.Text.Json.Serialization;

namespace Lang.Entity
{
    /// <summary>
    /// 日程对象
    /// </summary>
    public struct Schedule
    {
        /// <summary>
        /// 日程 id
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }
        /// <summary>
        /// 日程名称
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// 日程描述
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// 日程开始时间戳(ms)
        /// </summary>
        [JsonPropertyName("start_timestamp")]
        public string StartTimestamp { get; set; }
        /// <summary>
        /// 日程结束时间戳(ms)
        /// </summary>
        [JsonPropertyName("end_timestamp")]
        public string EndTimestamp { get; set; }
        /// <summary>
        /// 创建者
        /// </summary>
        [JsonPropertyName("creator")]
        public Member Creator { get; set; }
        /// <summary>
        /// 	日程开始时跳转到的子频道 id
        /// </summary>
        [JsonPropertyName("jump_channel_id")]
        public string JumpChannel_id { get; set; }
        /// <summary>
        /// 日程提醒类型，取值参考RemindType
        /// 0	不提醒
        /// 1	开始时提醒
        /// 2	开始前5分钟提醒
        /// 3	开始前15分钟提醒
        /// 4	开始前30分钟提醒
        /// 5	开始前60分钟提醒
        /// </summary>
        [JsonPropertyName("remind_type")]
        public string RemindType { get; set; }
    }

    public enum RemindType
    {
        /// <summary>
        /// 不提醒
        /// </summary>
        Noreminders = 0,
        /// <summary>
        /// 开始时提醒
        /// </summary>
        Start = 1,
        /// <summary>
        /// 开始前5分钟提醒
        /// </summary>
        Start5 = 2,
        /// <summary>
        /// 开始前15分钟提醒
        /// </summary>
        Start15 = 3,
        /// <summary>
        /// 开始前30分钟提醒
        /// </summary>
        Start30 = 4,
        /// <summary>
        /// 开始前60分钟提醒
        /// </summary>
        Start60 = 5,
    }
}
