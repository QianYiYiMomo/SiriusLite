using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text.Json;
using static Lang.Log.Log;

/// <summary>
/// 这段代码是MIUXUE写的!!
/// 所以MIUXUE会跟你说这个代码是什么，是干什么的
/// 
/// 从Program调用Connect，然后绑定_RecMsg(收到消息) OnDisconnectEvent(关闭事件)，所以收到消息什么的都在Program执行啦
/// 
/// 如有任何疑问请阅读腾讯开发手册
/// https://bot.q.qq.com/wiki/develop/api/
/// 2021年12月15日 MiuxuE
/// </summary>
namespace Lang.Client
{
    public class MyWebSocketClient : MyWebSocketBase
    {
        public ClientWebSocket clientWebSocket = new ClientWebSocket();
        private HttpClient httpClient = new();

        public event EventHandler<string>? OnMessageEvent;
        public event EventHandler<string>? OnDisconnectEvent;
        public event EventHandler<string>? OnConnectEvent;

        #region 工具
        /// <summary>
        /// 获取与腾讯WebSocket服务器连接用的地址
        /// </summary>
        /// <returns>返回地址</returns>
        public async Task<string> GetWssUrlWithShared(string url)// 获取与腾讯WebSocket服务器连接用的地址
        {
            var res = (await httpClient.GetFromJsonAsync<JsonElement>($"{url}gateway/bot"));
            var tmp_data = res.GetProperty("url").GetString();
            return tmp_data!;
        }
        #endregion
        #region WebSocket
        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <param name="_url">服务器的地址</param>
        public async Task Connect(string appId, string token, bool sandBox)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Authorization
                    = new System.Net.Http.Headers.AuthenticationHeaderValue("Bot", $"{appId}.{token}"); // 照官方文档所说，需要在访问头里加入此消息用于验证BOT身份 否则返回401
                // 连接腾讯的服务器 , 也就是 https://api.sgroup.qq.com/ 来获取可以连接的服务器 如果_sandbox是true则是腾讯则是https://sandbox.api.sgroup.qq.com
                // sandbox环境只会收到测试频道的事件，且调用openapi仅能操作测试频道
                var wssoption = GetWssUrlWithShared(sandBox ? "https://sandbox.api.sgroup.qq.com/" : "https://api.sgroup.qq.com/").Result;

                // 尝试连接 wssoption
                if (Uri.TryCreate(wssoption, UriKind.Absolute, out Uri? websocketURL))
                {
                    // 如果连接到了会返回用于 连接腾讯WebSocket服务器的地址
                    if (null != websocketURL)
                    {
                        await clientWebSocket.ConnectAsync(websocketURL, CancellationToken.None);
                        if (null != OnConnectEvent)
                            OnConnectEvent(this, "");
                    }
                }
                else // 如果跳到此处就说明 wssoption 中的这两个链接挂了或者其他原因，总之就是有问题嘛！
                {
                    LogErr("无法连接到腾讯服务器,请检查您的网络后重试!");
                    return;
                }

                while (true)
                {
                    if (clientWebSocket.State == WebSocketState.Open)
                    {
                        string data_buffer = Receive(clientWebSocket); // 接受消息,继承IWebSocket的Receive
                        if (null != data_buffer)
                        {
                            if (OnMessageEvent != null)
                                OnMessageEvent(this, data_buffer);
                        }
                    }
                    else // 如果状态不为 Open 视为关闭
                    {
                        if (OnDisconnectEvent != null)
                            OnDisconnectEvent(this, "");
                        switch ((int)clientWebSocket.CloseStatus!)
                        {
                            case 4001:
                                LogErr("连接出现错误 : 无效的 opcode");
                                break;
                            case 4002:
                                LogErr("连接出现错误 : 无效的 payload");
                                break;
                            case 4007:
                                LogErr("连接出现错误 : 无效的 	seq 错误");
                                break;
                            case 4008:
                                LogErr("[框架错误] 发送 payload 过快，请重新连接，并遵守连接后返回的频控信息");
                                break;
                            case 4009:
                                LogDebug("连接出现错误 : 连接过期"); // 正常现象!
                                break;
                            case 4010:
                                LogErr("连接出现错误 : 无效的 shard");
                                break;
                            case 4011:
                                LogErr("连接出现错误 : 连接需要处理的 guild 过多，请进行合理的分片");
                                break;
                            case 4012:
                                LogErr("连接出现错误 : 无效的 version");
                                break;
                            case 4013:
                                LogErr("连接出现错误 : 无效的 intent");
                                break;
                            case 4014:
                                LogErr("连接出现错误 : intent 无权限");
                                break;
                            case 4914:
                                LogErr("连接出现错误 : 机器人已下架,只允许连接沙箱环境,请断开连接,检验当前连接环境");
                                break;
                            case 4015:
                                LogErr("连接出现错误 : 机器人已封禁,不允许连接,请断开连接,申请解封后再连接");
                                break;
                        }
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                LogErr(e.Message); // 一般情况下为WebSocket错误，反正到这里了以后会自动重连的
            }

        }

        public void SendMsg(string msg) // 封装一下 调用父类的send
        {
            Send(clientWebSocket, msg);
        }
        #endregion
    }
}