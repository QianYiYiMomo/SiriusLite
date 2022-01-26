using CSharpClient;
using Lang.Api;
using Lang.Entity;
using System.Runtime.InteropServices;
using static Lang.Plugin.SiriusApiDelegates;
using static Lang.Log.Log;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lang.Plugin
{
    public static class SiriusEvent
    {
        public static void SiriusOnEvent(string data)
        {
            LogOut(data);
        }
    }
    public static class PluginManager
    {
        public static readonly Mutex eventMutex = new Mutex(false);
        public static Mutex EventMutex { get => eventMutex; }
        public static List<Plugin> plugins = new List<Plugin>();
        private static SiriusApi.Init init;
        private static SiriusApi.InitData SiriusApiData;
        private static IntPtr siriusApiHandle;
        public static SiriusApi.GetEventCallBack getEventCallBack;
        public static SiriusApi.CallEvent callEvent;
        public static bool LoadPlugin(string path)
        {
            try
            {
                var pluginPath = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Plugin\\{Path.GetFileName(path)}";
                if (!ExistencePlugin(path))
                {
                    File.Copy(path, pluginPath);
                    Plugin plugin = new Plugin(pluginPath);
                    if (!plugin.Refuse)
                    {
                        plugins.Add(plugin);
                        if (null != plugin.PluginInfo)
                        {
                            LogOk($"{plugin.PluginInfo.pluginName} 加载完毕," +
                            $"作者:{plugin.PluginInfo.pluginAuthor} " +
                            $"版本:{plugin.PluginInfo.pluginVersion}," +
                            $"描述:{plugin.PluginInfo.pluginDis}," +
                            $"SDK版本:{plugin.PluginInfo.pluginSDKVer}");
                        }
                    }
                    else
                    {
                        if (null != plugin.PluginInfo)
                        {
                            LogErr($"{plugin.PluginInfo.pluginName}加载失败,拒绝被加载!");
                        }
                        else
                        {
                            LogErr(Path.GetFileName($"{path}未公开AppInfo,加载失败!"));
                            LogErr($"{Path.GetFileName(path)}加载失败,可能不是天狼星插件?");
                        }
                        plugin.Dispose();
                        File.Delete(pluginPath);
                    }
                    return (plugin.Refuse);
                }
                else
                {
                    LogErr($"{Path.GetFileName(path)} 相同名称的插件已存在");
                    return false;
                }
            }
            catch (IOException e)
            {
                LogErr("加载插件失败(可能插件文件已经存在):" + e);
                return false;
            }
        }
        public static void OnMessageEvent(string jsonData)
        {

            try
            {
                for (int i = 0; i < plugins.Count; i++)
                {
                    plugins[i].OnMessage(jsonData);
                }
            }
            catch (Exception e)
            {
                LogErr(e);
            }

        }
        public static void OnGuildEvent(string jsonData, GuildEventType type)
        {
            try
            {
                for (int i = 0; i < plugins.Count; i++)
                {
                    plugins[i].OnGuildEvent(jsonData, type);
                }
            }
            catch (Exception e)
            {
                LogErr(e);
            }
        }
        public static void OnChannelEvent(string jsonData, ChannelEventType type)
        {
            try
            {
                for (int i = 0; i < plugins.Count; i++)
                {
                    plugins[i].OnChannelEvent(jsonData, type);
                }
            }
            catch (Exception e)
            {
                LogErr(e);
            }
        }
        public static void OnMemberEvent(string jsonData, MemberEventType type)
        {
            try
            {
                for (int i = 0; i < plugins.Count; i++)
                {
                    plugins[i].OnMemberEvent(jsonData, type);
                }
            }
            catch (Exception e)
            {
                LogErr(e);
            }
        }

        /// <summary>
        /// 检查插件是否重复
        /// </summary>
        /// <param name="filename">文件名(xxx.dll) 不是路径哦</param>
        /// <returns></returns>
        public static bool ExistencePlugin(string filename)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (Path.GetFileName(plugins[i].pluginFilePath) == filename)
                    return true;
            }
            return false;
        }
        public enum GuildType
        {
            OnCreateGuild = 0,
            OnUpdateGuild = 1,
            OnDeleteGuild = 2,
        }
        public static bool InitSiriusAPI()
        {
            try
            {
                var path = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}SiriusAPI.dll";
                Log.Log.LogDebug(path);
                siriusApiHandle = kernel32.LoadLibraryA(path);
                Log.Log.LogDebug(siriusApiHandle);
                if (0 == (int)siriusApiHandle)
                    return false;
                init = Marshal.GetDelegateForFunctionPointer<SiriusApi.Init>(kernel32.GetProcAddress(siriusApiHandle, "Init"));
                SiriusApiData.EventCallBack = SiriusEvent.SiriusOnEvent;
                SiriusApiData.SendMsg_Handle = OpenApi.SendMsg;
                SiriusApiData.SendMsg_Image_Handle = OpenApi.SendMsg_Image;
                SiriusApiData.GetMemberInfo_JSON_Handle = OpenApi.GetMemberInfo_JSON;
                SiriusApiData.GetGuildInfo_Handle = OpenApi.GetGuildInfoA;
                SiriusApiData.DeleteChannelSchedule_Handle = OpenApi.DeleteGuildRole;
                SiriusApiData.CreateChannelAnnounce_Handle = OpenApi.CreateChannelAnnounce;
                SiriusApiData.GetChannelInfo_Handle = OpenApi.GetChannelInfoA;
                SiriusApiData.SetChannelPermissions_Handle = OpenApi.SetChannelPermissions;
                SiriusApiData.AudioControl_Handle = OpenApi.AudioControl;
                SiriusApiData.GetBotInfo_Handle = OpenApi.GetBotInfoA;
                SiriusApiData.GetChannelScheduleInfo_Handle = OpenApi.GetChannelScheduleInfo;
                SiriusApiData.CreateChannelSchedule_Handle = OpenApi.CreateChannelSchedule;
                SiriusApiData.SetChannelSchedule_Handle = OpenApi.SetChannelSchedule;
                SiriusApiData.DeleteChannelSchedule_Handle = OpenApi.DeleteChannelSchedule;
                SiriusApiData.LogOut_Handle = LogOut_Plugin;
                SiriusApiData.LogOutWar_Handle = LogOutWar_Plugin;
                SiriusApiData.LogOutErr_Handle = LogOutErr_Plugin;
                SiriusApiData.GetPName = PluginKeyGetName;
                SiriusApiData.CheckPlugin = CheckPluginKey;
                SiriusApiData.AuthPlugin = AuthPluginKey;
                SiriusApiData.LogOutPlugin_Handle = LogOutPlugin_Plugin;
                SiriusApiData.CreateGuildRole_Handle = OpenApi.CreateGuildRoleA;
                SiriusApiData.SetGuildRole_Handle = OpenApi.SetGuildRoleA;
                SiriusApiData.GetChannelPermissions_Handle = OpenApi.GetChannelPermissionsA;
                SiriusApiData.GetGuildRoleA_Handle = OpenApi.AddGuildRoleA;
                SiriusApiData.GetGuildRoleList_Handle = OpenApi.GetGuildRoleListA;
                SiriusApiData.DeleteGuildRoleA_Handle = OpenApi.DeleteGuildRoleA;
                SiriusApiData.DeleteChannelAnnounce_Handle = OpenApi.DeleteChannelAnnounce;
                SiriusApiData.GetGuildChannelListA_Handle = OpenApi.GetGuildChannelListA;
                SiriusApiData.MuteGuild_Handle = OpenApi.MuteGuild;
                SiriusApiData.MuteGuildUser_Handle = OpenApi.MuteGuildUser;
                init.Invoke(SiriusApiData);
                getEventCallBack = Marshal.GetDelegateForFunctionPointer<SiriusApi.GetEventCallBack>(kernel32.GetProcAddress(siriusApiHandle, "GetEventCallBack"));
                callEvent = Marshal.GetDelegateForFunctionPointer<SiriusApi.CallEvent>(kernel32.GetProcAddress(siriusApiHandle, "CallEvent"));
                callEvent("[Sirius/EventCallBack] 载入成功");
                return (0 != (int)siriusApiHandle);
            }
            catch (Exception e)
            {
                Log.Log.LogErr(e);
                return false;
            }
            //Log.Log.LogErr($"[API加载] API加载失败,可能是无法找到目录下的SiriusApi.dll,请确认该文件存在于以下目录:{Directory.GetCurrentDirectory}\\SiriusApi.dll");
        }

        //这是微软的代码
        private static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is
            // equal to "file2byte" at this point only if the files are
            // the same.
            return ((file1byte - file2byte) == 0);
        }

        private static bool FilesCompare(FileInfo target, List<FileInfo> files)
        {
            return false;
            bool compare = false;
            foreach (FileInfo f in files)
            {
                if (FileCompare(target.FullName, f.FullName))
                {
                    compare = true;
                    break;
                }
            }
            return compare;
        }
        public static void LoadPlugin()
        {
            var path = $"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Plugin";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles();//文件

            List<FileInfo> pluginFiles = new List<FileInfo>();

            foreach (FileInfo file in files)
            {
                if (!FilesCompare(file, pluginFiles))
                {
                    pluginFiles.Add(file);
                }
                else
                {
                    LogWar($"存在完全相同的文件:{file.FullName},已跳过");
                }
            }

            List<Plugin> disposePlugins = new();

            foreach (FileInfo pluginFile in pluginFiles)
            {

                if (Path.GetExtension(pluginFile.FullName).ToLower() == ".dll")
                {
                    Plugin plugin = new(pluginFile.FullName);
                    if (null == plugin.PluginInfo)
                    {
                        LogErr(Path.GetFileName($"{pluginFile.Name}未公开AppInfo,加载失败!"));
                        disposePlugins.Add(plugin);
                    }
                    else

                    {
                        if (plugin.Refuse)
                        {
                            LogErr(Path.GetFileName($"{plugin.PluginInfo.pluginName}插件主动拒绝被加载."));
                            disposePlugins.Add(plugin);
                        }
                        else
                        {
                            LogOk($"{plugin.PluginInfo.pluginName} 加载完毕," +
                            $"作者:{plugin.PluginInfo.pluginAuthor} " +
                            $"版本:{plugin.PluginInfo.pluginVersion}," +
                            $"描述:{plugin.PluginInfo.pluginDis}," +
                            $"SDK版本:{plugin.PluginInfo.pluginSDKVer}");

                            plugins.Add(plugin);
                        }
                    }

                }

            }

            foreach (Plugin plugin in disposePlugins)
            {
                plugin.Dispose();
            }

        }
        public static bool CheckPluginKey(string pluginKey)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (plugins[i].pluginKey == pluginKey)
                    return true;
            }
            return false;
        }
        public static string PluginKeyGetName(string pluginKey)
        {
            for (int i = 0; i < plugins.Count; i++)
            {
                if (plugins[i].pluginKey == pluginKey)
                    return plugins[i].PluginInfo.pluginName;
            }
            return "";
        }
        public static bool AuthPluginKey(string pluginKey, string auth)
        {
            return true;
        }
        public static void RemovePlugin(Plugin plugin)
        {
            plugin.Dispose();
            plugins.Remove(plugin);
        }
        public static void RemovePlugin(int index)
        {
            try
            {
                EventMutex.WaitOne();
                var plugin = plugins[index];
                plugin.Dispose();
                File.Delete(plugin.pluginFilePath);
                LogOk(plugin.PluginInfo.pluginName + "->卸载成功!");
                plugins.Remove(plugin);
                EventMutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                LogErr("卸载插件失败:" + e);
            }
        }
    }
    public static class SiriusApi
    {
        public delegate void Init
        (
            InitData data
        );// 初始化事件
        public struct InitData
        {
            public EventCallBack EventCallBack;
            public GetPluginName GetPName;
            public CheckPlugin CheckPlugin;
            public AuthPlugin AuthPlugin;
            public SendMsg SendMsg_Handle;
            public SendMsg_Image SendMsg_Image_Handle;
            public GetMemberInfo_JSON GetMemberInfo_JSON_Handle;
            public DeleteGuildRole DeleteGuildRole_Handle;
            public GetGuildInfo GetGuildInfo_Handle;
            public CreateChannelAnnounce CreateChannelAnnounce_Handle;
            public GetChannelInfo GetChannelInfo_Handle;
            public SetChannelPermissions SetChannelPermissions_Handle;
            public AudioControl AudioControl_Handle;
            public GetBotInfo GetBotInfo_Handle;
            public GetChannelScheduleInfo GetChannelScheduleInfo_Handle;
            public CreateChannelSchedule CreateChannelSchedule_Handle;
            public SetChannelSchedule SetChannelSchedule_Handle;
            public DeleteChannelSchedule DeleteChannelSchedule_Handle;
            public LogOut LogOut_Handle;
            public LogOutWar LogOutWar_Handle;
            public LogOutErr LogOutErr_Handle;
            public LogOutPlugin LogOutPlugin_Handle;
            public CreateGuildRole CreateGuildRole_Handle;
            public SetGuildRole SetGuildRole_Handle;
            public GetChannelPermissionsA GetChannelPermissions_Handle;
            public AddGuildRoleA GetGuildRoleA_Handle;
            public GetGuildRoleList GetGuildRoleList_Handle;
            public DeleteGuildRoleA DeleteGuildRoleA_Handle;
            public DeleteChannelAnnounce DeleteChannelAnnounce_Handle;
            public GetGuildChannelListA GetGuildChannelListA_Handle;
            public MuteGuild MuteGuild_Handle;
            public MuteGuildUser MuteGuildUser_Handle;
        };

        public delegate IntPtr LoadPlugin(string pluginPath); // 加载插件
        public delegate void UnLoadPlugin(int pluginHandle);// 卸载插件
        public delegate int CallEvent(string dat);// 取Event地址
        public delegate int GetEventCallBack();// 取Event地址
    }
    public static class SiriusApiDelegates
    {
        public delegate void EventCallBack(string data);
        public delegate string SendMsg(string channel_Id, string msg, string msgId);
        public delegate string SendMsg_Image(string channel_Id, string msg, string msgId, string imageUrl);
        //public delegate  string (_stdcall* SendMsgEx)(string channel_Id, string msg, string msgId, string imageUrl, MessageArk msgArk, MessageEmbed msgEmbed);
        //public delegate  Member(__stdcall* GetMemberInfo)(string guild_Id, string userId);
        public delegate string GetMemberInfo_JSON(string guild_Id, string userId);
        public delegate string GetGuildInfo(string guild_id);
        public delegate string CreateGuildRole(int name, int color, int hoist, string Name, int Color, int Hoist, string guild_Id);
        public delegate string SetGuildRole(int name, int color, int hoist, string Name, int Color, int Hoist, string guild_Id, string role_Id);
        public delegate bool DeleteGuildRole(string guild_Id, string role_Id);
        public delegate string CreateChannelAnnounce(string channel_Id, string message_Id);
        public delegate string GetChannelInfo(string channel_Id);
        //public delegate  Channel[](_stdcall* GetGuildChannelList)(string guild_Id);
        public delegate string GetChannelPermissionsA(string channel_Id, string user_Id);
        public delegate bool SetChannelPermissions(string channel_Id, string user_Id, int addPermissions, int removePermissions);
        public delegate bool AudioControl(string channel_Id, AudioContro audioContro);
        public delegate string GetBotInfo();
        //public delegate  Guild[](_stdcall* GetGuildList)(string before, string after, int limit);
        //public delegate  Schedule[](_stdcall* GetChannelScheduleList)(string channel_Id, int since);
        public delegate Schedule GetChannelScheduleInfo(string channel_Id, string schedule_Id);
        public delegate Schedule CreateChannelSchedule(string channel_Id, Schedule schedule);
        public delegate Schedule SetChannelSchedule(string channel_Id, string schedule_Id, Schedule schedule);
        public delegate bool DeleteChannelSchedule(string channel_Id, string schedule_Id);
        public delegate void LogOut(string data, string key);
        public delegate void LogOutWar(string data, string key);
        public delegate void LogOutErr(string data, string key);
        public delegate string GetPluginName(string key);
        public delegate bool CheckPlugin(string key);
        public delegate bool AuthPlugin(string key, string auth);
        public delegate void LogOutPlugin(string data, string key);
        public delegate string GetGuildRoleList(string guild_Id);
        public delegate bool AddGuildRoleA(string channel_Id, string guild_Id, string role_Id, string user_Id);
        public delegate bool DeleteGuildRoleA(string channel_Id, string guild_Id, string role_Id, string user_Id);
        public delegate bool DeleteChannelAnnounce(string channel_Id, string message_Id);
        public delegate string GetGuildChannelListA(string guild_Id);
        public delegate bool MuteGuild(string guild_Id, string muteEndTimestamp, string muteSeconds);
        public delegate bool MuteGuildUser(string guild_Id, string user_Id, string muteEndTimestamp, string muteSeconds);
    }
    public static class SiriusTools
    {
        public static string GetRandomKey()
        {
            var random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 32)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
    /// <summary>
    /// 用户自定义插件信息
    /// 适用于x86的
    /// 易语言 C++ 等...
    /// </summary>
    public class PluginInfo
    {
        [JsonPropertyName("name")]
        public string pluginName { get; set; }
        [JsonPropertyName("author")]
        public string pluginAuthor { get; set; }
        [JsonPropertyName("dis")]
        public string pluginDis { get; set; }
        [JsonPropertyName("ver")]
        public string pluginVersion { get; set; }
        [JsonPropertyName("sdk")]
        public string pluginSDKVer { get; set; }
    }
}
