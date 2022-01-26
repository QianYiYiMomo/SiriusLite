using static Lang.BotEvents.BotEventParse;
using static Lang.Log.Log;
using System.Text.Json;
using Lang.Entity;
using Lang.Client;
using Lang.Data;
using Lang.Api;
using Lang.Entity.Event;
using Lang.Plugin;
using Lang.HttpApi;
using Lang;

System.Timers.Timer heartbeatTimer = new System.Timers.Timer(); // 心跳定时器

#if DEBUG
LogCat(); // 输出一直小猫猫,还有彩蛋哦~
#else  //也就是Release (或者其他更多的编译模式)
LogMsg($"欢迎使用天狼星{FrameworkInfo.Version} 作者是 蜜蜜子");
LogMsg($"欢迎更新新版SDK:https://www.lanzoul.com/b020vglub");
LogMsg($"请将插件放入运行目录下的Plugin目录中~");
LogMsg($"※※※※※※直接将插件拖入框架即可载入插件 输入Un即可卸载插件!※※※※※※");
LogMsg($"※※※※※※直接将插件拖入框架即可载入插件 输入Un即可卸载插件!※※※※※※");
LogMsg($"※※※※※※直接将插件拖入框架即可载入插件 输入Un即可卸载插件!※※※※※※");
LogMsg($"此版本为测试版,如有BUG请加讨论群:376957298提出您宝贵的建议~");
LogWar($"禁止使用本框架制作违法违规插件");
#endif //DEBUG

int latestIndex = 0; // 要求心跳包传输最新一条消息的"D"值 此变量用于记录
int disconnectNum = 0;// 此处为断开连接次数
int reconnectNum = 0; // 此处为服务端通知重连此处
ReadyEvent botInfo = new ReadyEvent();
MyWebSocketClient currentClient;// 目前的客户端
HttpApiWebSocketServer httpApi;

/// <summary>
/// 设置控制台标题
/// </summary>
void SetConsoleTitle(string statu)
{
    Console.Title = $"天狼星{FrameworkInfo.Version}\\{statu}\\收{latestIndex} 重{disconnectNum}";
}
void OnConnect(object? client, string _)
{
    if (null != client)
    {
        /*MyWebSocketClient webSocketClient = (MyWebSocketClient)client;
        if (botInfo.Session_Id == "" || botInfo.Session_Id == null)
        {r
            webSocketClient.SendMsg(PackManager.CreateIdentify(BotData.BotAppID.ToString(), BotData.BotToken, 0 | 1 << 30 | 1 << 0 | 1 << 1));
            LogOut("连接服务器成功");
        }
        else
        {
            webSocketClient.SendMsg(PackManager.CreateResume(latestIndex, BotData.BotAppID.ToString(), BotData.BotToken, botInfo.Session_Id));
            LogDebug("重连发送");
        }*/
    }
}
async void OnMessage(object? client, string msg)
{
    httpApi.Send(msg);
    if (msg != "{\"op\":11}")
        LogDebug(msg);
    if (null != client && msg != "")
    {
        try
        {
            MyWebSocketClient clientWebSocket = (MyWebSocketClient)client;
            var data = JsonDocument.Parse(msg).RootElement;
            switch ((OpCode)data.GetProperty("op").GetInt32())
            {
                case OpCode.Dispatch: // 服务器正常推送消息
                    switch (data.GetProperty("t").GetString())
                    {
                        case "READY": // 鉴权包发送成功以后会返回READY表示已经连接到这个机器人了
                            botInfo = ReadyParse(msg);
                            break;

                        ///频道事件
                        case "GUILD_CREATE": // 机器人被加入到某个频道的时候
                            PluginManager.OnGuildEvent(JsonSerializer.Serialize(data.GetProperty("d")), GuildEventType.GUILD_UPDATE);
                            break;
                        case "GUILD_UPDATE":// 频道信息变更
                            PluginManager.OnGuildEvent(JsonSerializer.Serialize(data.GetProperty("d")), GuildEventType.GUILD_UPDATE);
                            break;
                        case "GUILD_DELETE":// 频道被解散或机器人被移出频道
                            PluginManager.OnGuildEvent(JsonSerializer.Serialize(data.GetProperty("d")), GuildEventType.GUILD_DELETE);
                            break;

                        ///子频道事件
                        case "CHANNEL_CREATE":// 子频道被创建
                            PluginManager.OnChannelEvent(JsonSerializer.Serialize(data.GetProperty("d")), ChannelEventType.CHANNEL_CREATE);
                            break;
                        case "CHANNEL_UPDATE":// 子频道信息变更
                            PluginManager.OnChannelEvent(JsonSerializer.Serialize(data.GetProperty("d")), ChannelEventType.CHANNEL_UPDATE);
                            break;
                        case "CHANNEL_DELETE":// 子频道被删除
                            PluginManager.OnChannelEvent(JsonSerializer.Serialize(data.GetProperty("d")), ChannelEventType.CHANNEL_DELETE);
                            break;

                        ///频道成员事件
                        case "GUILD_MEMBER_ADD":// 新用户加入频道
                            PluginManager.OnMemberEvent(JsonSerializer.Serialize(data.GetProperty("d")), MemberEventType.GUILD_MEMBER_ADD);
                            break;
                        case "GUILD_MEMBER_UPDATE":// 暂无
                            PluginManager.OnMemberEvent(JsonSerializer.Serialize(data.GetProperty("d")), MemberEventType.MEMBER_UPDATE);
                            break;
                        case "GUILD_MEMBER_REMOVE":// 用户离开频道
                            PluginManager.OnMemberEvent(JsonSerializer.Serialize(data.GetProperty("d")), MemberEventType.MEMBER_REMOVE);
                            break;

                        /// 消息事件
                        case "AT_MESSAGE_CREATE": // 用户发送消息，并且@当前机器人
                            Thread thread = new Thread(() =>
                            {
                                Message message = ParseMessage(msg);
                                Guild info = OpenApi.GetGuildInfo(message.Guild_Id)!;
                                LogMsg($"{message.Author.UserName} 在频道 {info.Name} 对 {message.Mentions[0].UserName} 说 {message.Content}");
                                PluginManager.OnMessageEvent(JsonSerializer.Serialize(data.GetProperty("d")));
                            });
                            thread.Start();
                            break;

                        /// 表情表态事件
                        case "MESSAGE_REACTION_ADD": // 用户对消息进行表情表态时
                            break;
                        case "MESSAGE_REACTION_REMOVE":// 用户对消息进行取消表情表态时
                            break;

                        /// 音频事件
                        case "AUDIO_START":// 音频开始播放时
                            break;
                        case "AUDIO_FINISH":// 音频开始结束时
                            break;
                        case "AUDIO_ON_MIC":// 机器人上麦时
                            break;
                        case "AUDIO_OFF_MIC":// 机器人下麦时
                            break;

                        /// 重连
                        case "RESUMED":
                            if (botInfo.User.UserName != "")
                            {
                                SetConsoleTitle(botInfo.User.UserName);
                            }
                            else
                            {
                                disconnectNum = 0;
                                LogErr("重连失败.. 正在尝试重新鉴权!");
                                await clientWebSocket.clientWebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, null, CancellationToken.None);
                            }
                            break;
                        default:
                            LogErr("收到服务器发送数据包，但我们无法解析，请尝试将框架升级至最新版本！");//可能是版本太旧以后更新了其他的API
                            break;
                    }
                    break;
                case OpCode.Heartbeat:
                    break;
                case OpCode.Identify:
                    break;
                case OpCode.Resume:
                    break;
                case OpCode.Reconnect:
                    botInfo.Session_Id = ""; // 如果将Reconnect时候的SessionId改成null 下次连接时则不会发送重连包 将自动重连
                    reconnectNum++;
                    break;
                case OpCode.InvalidSession:
                    heartbeatTimer.Stop();
                    if (botInfo.Session_Id == "" || botInfo.Session_Id == null)
                    {
                        // 这里直接就是连接失败了 
                        LogErr("鉴权连接失败,请确认您的AppId，Token和intents设置是否正确 按下回车重连...");
                        Console.ReadLine();
                    }
                    else
                    {
                        // 对于不能够进行重连的session，需要清空 session id 与 seq
                        botInfo.Session_Id = ""; // 重置后就不会重连了,会使用鉴权连接
                        latestIndex = 0;
                        LogErr("重连失败,可能会话ID已过期或AppId,Token被更改！");
                    }
                    break;
                case OpCode.hello: // 收到服务器的招呼了 鉴权 重连                    
                    LogDebug("收到Hello包");
                    if (botInfo.Session_Id == "" || botInfo.Session_Id == null)
                    {
                        clientWebSocket.SendMsg(PackManager.CreateIdentify(BotData.BotAppID.ToString(), BotData.BotToken, 0 | 1 << 30 | 1 << 0 | 1 << 1));
                        LogOut("连接服务器成功");
                    }
                    else
                    {
                        clientWebSocket.SendMsg(PackManager.CreateResume(latestIndex, BotData.BotAppID.ToString(), BotData.BotToken, botInfo.Session_Id));
                        //clientWebSocket.SendMsg(PackManager.CreateHeartbeat(latestIndex));
                        LogDebug("重连发送");
                    }
                    LogDebug("规定心跳时间:" + data.GetProperty("d").GetProperty("heartbeat_interval").GetInt32() / 1000 + "秒");
                    InitHeartbeatTimer(data.GetProperty("d").GetProperty("heartbeat_interval").GetInt32(), clientWebSocket);
                    break;
                case OpCode.HeartbeatACK:
                    break;
                default:
                    LogErr($"[版本过低] 未知op:{msg}");//可能是版本太旧以后更新了其他的API
                    break;
            }
            if (data.TryGetProperty("s", out JsonElement _data)) // 尝试获取消息中的 "s" 如果获取就设置心跳中的 d
            {
                latestIndex = _data.GetInt32(); // 心跳包内的D需要保持最新消息的“s”值

                if (botInfo.User.UserName != "")
                    SetConsoleTitle(botInfo.User.UserName);
            }
        }
        catch (Exception e)
        {
            LogErr("解析消息时出错:" + e);
        }
    }
}//      当客户端收到消息
void OnDisconnect(object? client, string _)
{
    heartbeatTimer.Stop();
    SetConsoleTitle("重连中");
    LogDebug("连接已断开");
}//>_< 当客户端断开连接
void InitHeartbeatTimer(int deartBeatTime, MyWebSocketClient webSocketClient)
{
    currentClient = webSocketClient;
    int interval = deartBeatTime; // 计时器 单位毫秒
    heartbeatTimer = new System.Timers.Timer(interval);
    heartbeatTimer.AutoReset = true;
    heartbeatTimer.Enabled = true;
    heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(HeartBeatTimerUP!);
}// 初始化心跳，同时currentClient也会在此初始化
void HeartBeatTimerUP(object _, System.Timers.ElapsedEventArgs elapsedEventArgs)
{
    Task.Run(() => currentClient.SendMsg(PackManager.CreateHeartbeat(latestIndex)));
}// 心跳
LogOut("[API加载] 正在加载SiriusAPI.dll");
if (!PluginManager.InitSiriusAPI())
    LogErr($"[API加载] [×] API加载失败,可能是无法找到目录下的SiriusAPI.dll,目录:{AppDomain.CurrentDomain.BaseDirectory}SiriusAPI.dll 如果你确定此文件确实存在但是还无法加载请尝试下载修复文件：https://www.lanzoul.com/b020vglub");
LogOut("[插件管理] -> 正在加载插件");
PluginManager.LoadPlugin();
LogOut($"[插件管理] -> 插件加载完毕,加载了{PluginManager.plugins.Count}个插件");
#region 配置文件读取与加载
LogOut("[配置] 读取配置文件中");
Config config = new Config();
config.InitConfig();
var cfg = config.GetConfig();
if (null != cfg)
{
    LogOk("[配置] 加载成功 如果你想重置你的账号和配置文件 请输入:reset");
    BotData.BotAppID = cfg.BotID;
    BotData.BotToken = cfg.Token;
}
else
{
    var ok = false;
    while (!ok)
    {
        Console.Write("[请输入您的BotId] -> ");
        var BotId = Console.ReadLine();
        Console.Write("[请输入您的BotToken] -> ");
        var Token = Console.ReadLine();

        if (OpenApi.CheckBot(BotId, Token))
        {
            config.SaveConfig(SiriusTools.GetRandomKey(), BotId, Token, 0);
            BotData.BotAppID = BotId;
            BotData.BotToken = Token;
            ok = true;
            Console.Clear();
            LogOk("欢迎使用天狼星!");
        }
        else
        {
            LogErr("登录失败，请检查您的BotId和Token...");
        }
    }
}
#endregion
while (!OpenApi.CheckBot(cfg.BotID, cfg.Token))
{
    Console.Write("[请输入您的BotId] -> ");
    var BotId = Console.ReadLine();
    Console.Write("[请输入您的BotToken] -> ");
    var Token = Console.ReadLine();

    if (OpenApi.CheckBot(BotId, Token))
    {
        config.SaveConfig(cfg.Key, BotId, Token, cfg.Port);
        BotData.BotAppID = BotId;
        BotData.BotToken = Token;
        Console.Clear();
        LogOk("欢迎使用天狼星!");
    }
    else
    {
        LogErr("登录失败，请检查您的BotId和Token...");
    }
} //防止Token过期检查
#region HttpApi配置读取和启动
cfg = config.GetConfig();
if (cfg.Key == "")
{
    config.SaveConfig(SiriusTools.GetRandomKey(), cfg.BotID, cfg.Token, cfg.Port);
    cfg = config.GetConfig();
    LogMsg($"HttpApiToken:{cfg.Key}");
}
if (cfg.Port == 0)
{
    config.SaveConfig(cfg.Key, cfg.BotID, cfg.Token, 18595);
    cfg = config.GetConfig();
    LogMsg($"HttpApi端口:{cfg.Port}");
}
int httpApiPort = cfg.Port;
if (!(cfg.Port >= 0 && cfg.Port <= 65535))
{
    LogErr("此端口不合法(应在0-65535)");
    config.SetPort(ref cfg);
}

LogOut("[API加载] 正在初始化HTTPAPI");
httpApi = new();
httpApi.Start(cfg.Port, cfg);
LogOut($"[API加载] HTTPAPI -> 端口 -> {cfg.Port}");
#endregion

#if !DEBUG
Task.Run(() =>
{
    LogOut("正在检查更新中");
    Update.UpdateCheck();
    LogOut("检测更新成功");
});
#endif //DEBUG

Thread mainThread = new Thread(() =>
{
    // 这里是连接服务器的地方
    // 这里直接写死循环 await会等待连接结束 结束以后在连接就好了
    try
    {
        while (true)
        {
            //latestIndex = 0;
            MyWebSocketClient webSocketClient = new MyWebSocketClient();
            #region 输出文本

            LogOut($"正在以{(BotData.sandBox ? "沙箱" : "正式")}模式连接服务器"); // 沙箱环境只会收到测试频道的事件，且调用openapi仅能操作测试频道
            if (BotData.sandBox)
                LogWar("沙箱环境只会收到测试频道的事件，且调用openapi仅能操作测试频道");

            #endregion
            SetConsoleTitle("连接中");
            LogDebug("连接中");
            var task = webSocketClient.Connect(BotData.BotAppID, BotData.BotToken, BotData.sandBox); // 连接服务器
            LogDebug("正在等待Hello包");
            SetConsoleTitle("等待中");

            webSocketClient.OnMessageEvent += OnMessage;
            webSocketClient.OnDisconnectEvent += OnDisconnect;
            webSocketClient.OnConnectEvent += OnConnect;

            OpenApi.intApi(BotData.BotAppID, BotData.BotToken, BotData.sandBox); // 初始化OpenAPI
            task.Wait(); // 等待client.connect执行完毕

            heartbeatTimer.Stop();
        }
    }
    catch (Exception e)
    {
        LogErr(e);
    }
});
mainThread.IsBackground = true;
mainThread.Start();

while (true)
{
    string cmd = Console.ReadLine()!;
    LogDebug($"用户输入:{cmd}");
    switch (cmd.ToLower())
    {
        case "help":
            LogOut(">help 帮助");
            LogOut(">exit  退出");
            LogOut(">update 检查更新");
            LogOut(">update_ok 开始更新");
            LogOut(">reset 重置配置文件");
            LogOut(">plugins 查看插件列表");
            LogOut(">un 卸载插件");
            LogOut(">token 取HttpApiToken");
            LogOut(">httpapi 取httpApi连接地址");
            LogOut(">直接将插件拖入此窗口即可载入插件!");
            LogOut($">目前版本:{ FrameworkInfo.VersionName} - 天狼星作者:MiuxuE");
            break;
        case "token":
            Console.WriteLine("你的Token:" + cfg.Key);
            break;
        case "httpapi":
            Console.WriteLine($"你的服务器连接地址:ws://127.0.0.1:{cfg.Port}/{cfg.Key}/");
            Console.WriteLine($"ws://127.0.0.1:{cfg.Port}/");
            break;
        case "plugins":
            if (PluginManager.plugins.Count == 0)
            {
                LogWar("你还没有插件呢,将插件拖入本窗口即可载入插件!");
            }
            for (int i = 0; i < PluginManager.plugins.Count; i++)
            {
                if (null != PluginManager.plugins[i].PluginInfo)
                {
                    LogOk($">{PluginManager.plugins[i].PluginInfo.pluginName} (ID:{i})\n作者:{PluginManager.plugins[i].PluginInfo.pluginAuthor}" +
                    $"版本:{PluginManager.plugins[i].PluginInfo.pluginVersion}-\n" +
                    $"描述:{PluginManager.plugins[i].PluginInfo.pluginDis}\n" +
                    $"SDK版本:{PluginManager.plugins[i].PluginInfo.pluginSDKVer}");
                }
            }
            break;
        case "exit":
            LogOut("正在关闭...");
            PluginManager.plugins.ForEach(plugin =>
            {
                plugin.Dispose();
            });
            Environment.Exit(Environment.ExitCode);
            return;
        case "update":
            LogOut("正在检查更新中");
            Update.UpdateCheck();
            LogOk("检测更新成功");
            break;
        case "update_ok":
            LogOut("正在更新中!");
            Update.StartUpdate();
            break;
        case "un":
            for (int i = 0; i < PluginManager.plugins.Count; i++)
            {
                LogWar($"<ID:{i}> {PluginManager.plugins[i].PluginInfo.pluginName}");
            }
            try
            {
                Console.Write("请输入要卸载的插件ID -> ");
                var id = Console.ReadLine();
                PluginManager.RemovePlugin(Convert.ToInt32(id));
            }
            catch (Exception)
            {
                LogErr("输入错误...");
            }
            break;
        case "reset":
            LogOut("重置配置文件中...");
            config.ResetConfig();
            LogOk("配置文件重置成功!");
            break;
        default:
            cmd = cmd.Replace("\"", "");
            if (File.Exists(cmd))
            {
                if (Path.GetExtension(cmd).ToLower() == ".dll")
                {
                    LogOut($"正在尝试载入:{Path.GetFileName(cmd)}");
                    PluginManager.LoadPlugin(cmd);
                }
                else
                {
                    LogErr($"错误的文件,如果你想载入插件应传入xxx.dll,而不是{Path.GetExtension(cmd).ToLower()}");
                }
            }
            else
            {
                var oks = 0;
                for (int i = 0; i < PluginManager.plugins.Count; i++)
                {
                    if (PluginManager.plugins[i].OnCommand(cmd))
                        oks++;
                }
                if (oks == 0)
                    LogOut("未知指令,输入help查看帮助");
            }
            break;
    }
}

/// <summary>
/// Bot信息 用于存放BotID和BotToken (测试时用,以后做读配置文件)
/// </summary>
public static class BotData
{
    public static string BotAppID = "";
    public static string BotToken = "";
    public static bool sandBox = false;// 沙箱环境只会收到测试频道的事件，且调用openapi仅能操作测试频道
}
/// <summary>
/// 框架信息
/// </summary>
public static class FrameworkInfo
{
    public static int VersionID { get { return 7; } }
    public static string VersionName { get { return "0.1.0.51"; } }
    public static string Version { get { return "轻量测试版 0.1.0.51"; } }
}