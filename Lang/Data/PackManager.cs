using System.Text.Json.Serialization;
using System.Text.Json;


namespace Lang.Data
{
    /// <summary>
    /// 获取数据包
    /// </summary>
    public static class PackManager
    {
        /// <summary>
        /// 创建鉴权包
        /// </summary>
        /// <param name="botId">BotAppID 原为整数 用String保险些</param>
        /// <param name="token">BotToken</param>
        /// <returns></returns>
        public static string CreateIdentify(string botId, string token, int intent)
        {
            return JsonSerializer.Serialize(new IdentifyPack(token, botId, intent, null));
        }
        /// <summary>
        /// 创建心跳包
        /// </summary>
        /// <param name="latestIndex"></param>
        /// <returns></returns>
        public static string CreateHeartbeat(int latestIndex)
        {
            return JsonSerializer.Serialize(new HeartbeatPack(latestIndex));
        }
        /// <summary>
        /// 创建重连包
        /// </summary>
        /// <param name="latestIndex">最后一条消息</param>
        /// <param name="appId">BotAppId</param>
        /// <param name="token">BotToken</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns></returns>
        public static string CreateResume(int latestIndex, string appId, string token, string sessionId)
        {
            return JsonSerializer.Serialize(new ResumePack(latestIndex, appId, token, sessionId));
        }
    }

    /// <summary>
    /// 所有包的父类
    /// </summary>
    public class DataPackBase
    {
        public DataPackBase(OpCode opCode)
        {
            this.OpCode = (int)opCode;
        }
        /// <summary>
        /// 使用枚举类型 Data.OpCode
        /// </summary>
        [JsonPropertyName("op")]
        public int OpCode { get; set; }
        [JsonPropertyName("d")]
        public object Data { get; set; }
    }
    /// <summary>
    /// 鉴权
    /// </summary>
    public class IdentifyPack : DataPackBase
    {
        /// <summary>
        /// 鉴权包
        /// https://bot.q.qq.com/wiki/develop/api/gateway/reference.html 2.鉴权连接
        /// </summary>
        /// <param name="token">BotToken</param>
        /// <param name="appId">BotAppID</param>
        /// <param name="intents">需要的权限,请注意，如果填错会直接报错哦</param>
        /// <param name="properties">没啥用处(暂时)</param>
        public IdentifyPack(string token, string appId, long intents, object? properties) : base(Lang.Data.OpCode.Identify)
        {
            data.Token = $"{appId}.{token}";
            data.Intents = intents;
            data.Properties = properties;
            Data = data;
        }
        [JsonPropertyName("d")]
        public D data = new();
        public class D
        {
            /// <summary>
            /// 这里为 BotAppId.Token 
            /// 栗 APPID = 111 BotToken = "哈哈哈"
            /// 这里就是 111.哈哈哈
            /// </summary>
            [JsonPropertyName("token")]
            public string Token { get; set; }
            /// <summary>
            /// 需要的权限 申请错误直接报错哦
            /// </summary>
            [JsonPropertyName("intents")]
            public long Intents { get; set; }
            /// <summary>
            /// 消息分片
            /// </summary>
            [JsonPropertyName("shard")]
            public int[] Shard = new[] { 0, 1 };
            /// <summary>a
            /// 暂时没啥用
            /// </summary>
            [JsonPropertyName("properties")]
            public object? Properties { get; set; }
        }
    }
    /// <summary>
    /// 心跳
    /// </summary>
    public class HeartbeatPack : DataPackBase
    {
        public HeartbeatPack(int latestIndex) : base(Lang.Data.OpCode.Heartbeat)
        {
            Data = latestIndex;
        }
    }
    /// <summary>
    /// 重连
    /// </summary>
    public class ResumePack : DataPackBase
    {
        /// <summary>
        /// 重连包，需要传会话ID
        /// </summary>
        /// <param name="latestIndex">最后一条消息的Req 也就是JSON里面的s</param>
        /// <param name="appId">BotAppId</param>
        /// <param name="token">BotToken</param>
        /// <param name="sessionId">会话ID</param>
        public ResumePack(int latestIndex, string appId, string token, string sessionId) : base(Lang.Data.OpCode.Resume)
        {
            ResumeData data = new();
            data.LatestIndex = latestIndex;
            data.Token = $"{appId}.{token}";
            data.SessionId = sessionId;
            Data = data;
        }
        public class ResumeData
        {
            [JsonPropertyName("token")]
            public string Token { set; get; }
            [JsonPropertyName("session_id")]
            public string SessionId { get; set; }
            [JsonPropertyName("seq")]
            public int LatestIndex { get; set; }
        }
    }
}
