using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;

namespace Lang.Client
{
    /// <summary>
    /// 这里只是封装了一个简单的发送与读取
    /// </summary>
    public class MyWebSocketBase
    {
        /// <summary>
        /// 向WebSocket服务器发送信息
        /// </summary>
        /// <param name="webSocket">客户端</param>
        /// <param name="_msg">要发送的内容</param>
        public static void Send(WebSocket webSocket, string _msg)
        {
            ArraySegment<byte> array = new ArraySegment<byte>(Encoding.UTF8.GetBytes(_msg)); // 将String转换成 ArraySegment
            var task = webSocket.SendAsync(array, WebSocketMessageType.Text, true, CancellationToken.None); // 发送数据
            task.Wait();
        }
        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="_ws">客户端</param>
        /// <returns>从服务器取到的数据(没有为null)</returns>
        public static string Receive(WebSocket _ws)
        {
            try
            {
                byte[] buffer = new byte[1024 * 5];
                ArraySegment<byte> _buffer = new ArraySegment<byte>(buffer);
                var task = _ws.ReceiveAsync(_buffer, CancellationToken.None);
                task.Wait();
                byte[] msgBytes = _buffer.Take(task.Result.Count).ToArray(); // 读取 task.Result.Count 长度的消息
                return Encoding.UTF8.GetString(msgBytes);
            }
            catch (Exception ex)
            {
                Log.Log.LogErr("[WebSocket] " + ex);
                return "";
            }
        }
    }
}
