using Lang.Client;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Text.Json;
using static Lang.Config;
using WebSocketSharp.Net;
using System.Text;
using Lang.Api;
using static Lang.Log.Log;

namespace Lang.HttpApi
{
    public class HttpApiWebSocketServer
    {
        private HttpServer httpServer;
        public static Dictionary<string, HttpApiPlugin> plugins = new Dictionary<string, HttpApiPlugin>();
        private ConfigData configData;
        public class MessageAcquisition : WebSocketBehavior
        {
            protected override void OnOpen()
            {
                Log.Log.LogOut($"[WebSocketServer] [+] [{ID}] 客户端加入");
            }
            protected override void OnClose(CloseEventArgs e)
            {
                Log.Log.LogWar($"[WebSocketServer] [-] [{ID}] 客户端离开");
                plugins.Remove(ID);
            }
            protected override void OnError(WebSocketSharp.ErrorEventArgs e)
            {
            }
            protected override void OnMessage(MessageEventArgs e)
            {
                try
                {
                    Entity.AuthPlugin authPlugin = JsonSerializer.Deserialize<Entity.AuthPlugin>(e.Data)!;
                    HttpApiPlugin plugin = new(authPlugin);
                    bool nameExists = false;
                    foreach (var data in plugins)
                    {
                        if (data.Value.authPlugin.Name == authPlugin.Name) nameExists = true;
                    }


                    if (nameExists)
                    {
                        Log.Log.LogErr($"[WebSocketServer] [-] [{ID}] 已存在名字完全相同的插件 [{authPlugin.Lang}]{authPlugin.Name}");
                        Sessions.SendTo(JsonSerializer.Serialize(new
                        {
                            op = -1,
                            t = "PLUGIN_ALREADYEXISTS",
                            d = new
                            {
                                msg = "已存在名字完全相同的插件!"
                            }
                        }), ID);
                        Sessions.CloseSession(ID);
                    }
                    else
                    {
                        plugins.Add(ID, plugin);
                        Log.Log.LogOk($"[WebSocketServer] [*] [{ID}] -> [{authPlugin.Lang}]{authPlugin.Name}({authPlugin.Version})-{authPlugin.Author},{authPlugin.Description}");
                        Sessions.SendTo(JsonSerializer.Serialize(new
                        {
                            op = -1,
                            t = "PLUGIN_LOADED",
                            d = new
                            {
                                plugin_token = plugin.pluginToken
                            }
                        }), ID);
                    }
                }
                catch
                {
                    plugins.Remove(ID);
                    Sessions.CloseSession(ID);
                    Log.Log.LogErr($"[WebSocketServer] [-] [{ID}] 发送了无效数据!");
                }
            }
        }
        public void Start(int port, ConfigData configData)
        {
            this.configData = configData;
            httpServer = new HttpServer(port);
            httpServer.Start();
            httpServer.AddWebSocketService<MessageAcquisition>($"/{configData.Key}");
            httpServer.OnGet += (sender, e) => OnPluginGet(sender, e);
            httpServer.OnPost += (sender, e) => OnPluginPost(sender, e);
        }
        public void UnLoad(string id)
        {

        }
        public void Send(string msg)
        {
            httpServer.WebSocketServices[$"/{configData.Key}"].Sessions.Broadcast(msg);
        }
        public string PluginTokenGetName(string token)
        {
            foreach (var plugin in plugins)
            {
                if (plugin.Value.pluginToken == token) return plugin.Value.authPlugin.Name;
            }
            return "";
        }
        private void OnPluginGet(object sneder, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;
            string getUrl = req.Url.AbsolutePath.Replace("/", "");
            //Log.Log.LogMsg($"[HttpApi] [Get] {typeString}");


            switch (getUrl)
            {
                default:
                    res.StatusCode = 404;
                    break;
            }

            res.ContentEncoding = Encoding.UTF8;
            res.ContentType = "text";
            res.SetHeader("Server", "SiriusLite-LoveYou!");
            byte[] data = Encoding.UTF8.GetBytes("hello world");
            res.ContentLength64 = data.LongLength;
            res.Close(data, true);
        }
        private void OnPluginPost(object sneder, HttpRequestEventArgs e)
        {
            var req = e.Request;
            var res = e.Response;
            res.ContentType = "application/json";
            string getUrl = req.Url.AbsolutePath.Replace("/", "");
            //Log.Log.LogMsg($"[HttpApi] [Get] {typeString}");
            getUrl = getUrl.ToLower();
            string responseString = "";
            string token = req.QueryString.Get("token")!;
            string pluginName = PluginTokenGetName(token);
            Log.Log.LogDebug($"token:{token} => {req.Url.ToString()}");
#if RELEASE
            if(token == "")
            {
                Die(res, JsonSerializer.Serialize(new
                {
                    code = "401",
                    message = "请附加Token参数并且为全小写(api地址?token=xxxxxxxxxxxxx)"
                }), 401);
                return;
            }
            if(PluginTokenGetName(token) == "")
            {
                Die(res, JsonSerializer.Serialize(new
                {
                    code = "401",
                    message = "token不正确 未找到插件!"
                }), 401);
                return;
            }
#endif

            switch (getUrl)
            {
                case "logout": // 日志
                    string logOutMsg = req.QueryString.Get("msg")!;
                    if (logOutMsg == "")
                    {
                        res.StatusCode = 400;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 400,
                            message = "无法输出空文本"
                        });
                    }
                    else
                    {
                        Log.Log.LogOut($"[HttpApi] [日志] [{PluginTokenGetName(token)}] {logOutMsg}");
                        res.StatusCode = 200;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 200,
                            message = "OK"
                        });
                    }
                    break;
                case "logout_err":// 错误
                    string logOutErrMsg = req.QueryString.Get("msg")!;
                    if (logOutErrMsg == "")
                    {
                        res.StatusCode = 400;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 400,
                            message = "无法输出空文本"
                        });
                    }
                    else
                    {
                        Log.Log.LogErr($"[HttpApi] [错误] [{PluginTokenGetName(token)}] {logOutErrMsg}");
                        res.StatusCode = 200;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 200,
                            message = "OK"
                        });
                    }
                    break;
                case "logout_war":// 警告
                    string logOutWar = req.QueryString.Get("msg")!;
                    if (logOutWar == "")
                    {
                        res.StatusCode = 400;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 400,
                            message = "无法输出空文本"
                        });
                    }
                    else
                    {
                        Log.Log.LogWar($"[HttpApi] [警告] [{pluginName}] {logOutWar}");
                        res.StatusCode = 200;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 200,
                            message = "OK"
                        });
                    }
                    break;
                case "sendmsg":// 发消息
                    string sendMessage = req.QueryString.Get("msg")!;
                    string sendMessageImageUrl = req.QueryString.Get("img")!;
                    string snedMessageChannelId = req.QueryString.Get("channel_id")!;
                    string snedMessageId = req.QueryString.Get("msg_id")!;
                    LogOutHttpPlugin_Plugin($"在频道{snedMessageChannelId} 发送消息:{sendMessage}",pluginName);
                    string sendMessageReId = "";

                    if(sendMessageImageUrl != "")
                    {
                        sendMessageReId = OpenApi.SendMsg_Image(snedMessageChannelId, sendMessage, snedMessageId, sendMessageImageUrl);
                    }
                    else
                    {
                        sendMessageReId = OpenApi.SendMsg(snedMessageChannelId, sendMessage, snedMessageId);
                    }

                    if(sendMessageReId != "")
                    {
                        res.StatusCode = 200;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 200,
                            message = "发送成功",
                            message_id = sendMessageReId
                        });
                    }
                    else
                    {
                        res.StatusCode = 500;
                        responseString = JsonSerializer.Serialize(new
                        {
                            code = 500,
                            message = "发送消息失败,请查看框架日志输出!"
                        });
                    }

                    break;
                default:
                    res.StatusCode = 404;
                    responseString = JsonSerializer.Serialize(new
                    {
                        code = 404,
                        message = "API地址无效"
                    });
                    break;
            }

            Die(res, responseString, res.StatusCode);
        }
        private void Die(HttpListenerResponse response, string responseString, int statusCode)
        {
            response.StatusCode = statusCode;
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "application/json";
            response.SetHeader("Server", "SiriusLite-LoveYou!");
            byte[] data = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = data.LongLength;
            response.Close(data, true);
        }
    }
}
