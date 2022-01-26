using System.Text.Json.Serialization;

namespace Lang.Entity
{
    public class AudioContro
    { 
        /// <summary>
        /// 音频数据的url AudioStatus为Start时传
        /// </summary>
        [JsonPropertyName("audio_url")]
        public string AudioUrl { get; set; }
        /// <summary>
        /// 状态文本（比如：简单爱-周杰伦），可选，status为0时传，其他操作不传
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }
        /// <summary>
        /// 播放状态，参考 AudioStatus
        /// </summary>
        [JsonPropertyName("status")]
        public AudioStatus AudioStatus { get; set; }

    }
    public class AudioAction
    {
        public AudioAction(string guild_Id,string channel_Id,string audio_Url,string text)
        {
            this.Guild_Id = guild_Id;
            this.Channel_Id = channel_Id;
            this.Audio_Url = audio_Url;
            this.Text = text;
        }
        /// <summary>
        /// 频道id
        /// </summary>
        [JsonPropertyName("guild_id")]
        public string Guild_Id { get; set; }
        /// <summary>
        /// 子频道id
        /// </summary>
        [JsonPropertyName("channel_id")]
        public string Channel_Id { get; set; }
        /// <summary>
        /// 音频数据的url status为0时传
        /// </summary>
        [JsonPropertyName("audio_url")]
        public string Audio_Url { get; set; }
        /// <summary>
        /// 状态文本（比如：简单爱-周杰伦），可选，status为0时传，其他操作不传
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
    /// <summary>
    /// 音频播放状态
    /// </summary>
    public enum AudioStatus
    {
        /// <summary>
        /// 开始播放
        /// </summary>
        Start = 0,
        /// <summary>
        /// 暂停播放
        /// </summary>
        Pause = 1,
        /// <summary>
        /// 继续播放
        /// </summary>
        Resume = 2,
        /// <summary>
        /// 停止播放
        /// </summary>
        Stop = 3,
    }
}
