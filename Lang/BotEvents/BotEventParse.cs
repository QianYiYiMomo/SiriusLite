using Lang.Entity.Event;
using System.Text.Json;
using Lang.Entity;

namespace Lang.BotEvents
{
    /// <summary>
    /// 提供JSON原始消息
    /// 可以直接解析成实体类使用
    /// </summary>
    public class BotEventParse
    {
        public static Message ParseMessage(string jsonStr)
        {
            var jsonData = JsonDocument.Parse(jsonStr).RootElement.GetProperty("d");
            Message message = JsonSerializer.Deserialize<Message>(jsonData)!;
            return message;
        }
        public static ReadyEvent ReadyParse(string jsonStr)
        {
            var jsonData = JsonDocument.Parse(jsonStr).RootElement.GetProperty("d");
            ReadyEvent readyEvent = JsonSerializer.Deserialize<ReadyEvent>(jsonData)!;
            return readyEvent;
        }
    }
}
