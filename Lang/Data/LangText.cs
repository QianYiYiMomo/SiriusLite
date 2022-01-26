using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang.Data
{
    /// <summary>
    /// 文本代码
    /// </summary>
    public class LangText
    {
        /// <summary>
        /// @指定人
        /// </summary>
        /// <param name="id">msg.author.id</param>
        /// <returns></returns>
        public static  string At(string id)
        {
            return $"<@!{id}>";
        }
        /// <summary>
        /// 获取机器人收到的消息内容
        /// 如原消息为 <@!13133042892743931489> 你好
        /// 则返回 "你好"
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string getMessage(string message,string botId)
        {
            string msg = message.Replace($"<@!{botId}>", "");
            msg = msg.TrimStart();
            msg = msg.TrimEnd();
            return (msg);
        }
    }
}
