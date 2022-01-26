using CSharpClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace Lang.Plugin
{
    public class PluginFunc
    {
        private readonly PluginDelegates.Init initEvent;
        private readonly PluginDelegates.AppInfo appInfo;
        private readonly PluginDelegates.Menu menu;
        private readonly PluginDelegates.OnLoad onLoad;
        private readonly PluginDelegates.OnUnLoad onUnLoad;
        private readonly PluginDelegates.OnGuildMessage onGuildMessage;
        private readonly PluginDelegates.OnEnable onEnable;
        private readonly PluginDelegates.OnDisable onDisable;
        private readonly PluginDelegates.GuildEvent guildEvent;
        private readonly PluginDelegates.ChannelEvent channelEvent;
        private readonly PluginDelegates.MemberEvent memberEvent;
        private readonly PluginDelegates.OnCommand onCommand;
        private PluginFunc(IntPtr handle)
        {
            var badPointer = IntPtr.Zero;

            var info = kernel32.GetProcAddress(handle, "AppInfo");
            if (info != badPointer)
                appInfo = Marshal.GetDelegateForFunctionPointer<PluginDelegates.AppInfo>(info);

            var init = kernel32.GetProcAddress(handle, "AppInit");
            if (init != badPointer)
                initEvent = Marshal.GetDelegateForFunctionPointer<PluginDelegates.Init>(init);

            var load = kernel32.GetProcAddress(handle, "OnLoad");
            if (load != badPointer)
                onLoad = Marshal.GetDelegateForFunctionPointer<PluginDelegates.OnLoad>(load);

            var unLoad = kernel32.GetProcAddress(handle, "OnUnLoad");
            if (unLoad != badPointer)
                onUnLoad = Marshal.GetDelegateForFunctionPointer<PluginDelegates.OnUnLoad>(unLoad);

            var enable = kernel32.GetProcAddress(handle, "OnEnable");
            if (enable != badPointer)
                onEnable = Marshal.GetDelegateForFunctionPointer<PluginDelegates.OnEnable>(enable);

            var disable = kernel32.GetProcAddress(handle, "OnDisable");
            if (disable != badPointer)
                onDisable = Marshal.GetDelegateForFunctionPointer<PluginDelegates.OnDisable>(disable);

            var guildMessage = kernel32.GetProcAddress(handle, "OnGuildMessage");
            if (guildMessage != badPointer)
                onGuildMessage = Marshal.GetDelegateForFunctionPointer<PluginDelegates.OnGuildMessage>(guildMessage);

            var command = kernel32.GetProcAddress(handle, "OnCommand");
            if (command != badPointer)
                onCommand = Marshal.GetDelegateForFunctionPointer<PluginDelegates.OnCommand>(command);

            var onGuildEvent = kernel32.GetProcAddress(handle, "GuildEvent");
            if (onGuildEvent != badPointer)
                guildEvent = Marshal.GetDelegateForFunctionPointer<PluginDelegates.GuildEvent>(onGuildEvent);

            var onChannelEvent = kernel32.GetProcAddress(handle, "ChannelEvent");
            if (onChannelEvent != badPointer)
                channelEvent = Marshal.GetDelegateForFunctionPointer<PluginDelegates.ChannelEvent>(onChannelEvent);

            var onMemberEvent = kernel32.GetProcAddress(handle, "MemberEvent");
            if (onGuildEvent != badPointer)
                memberEvent = Marshal.GetDelegateForFunctionPointer<PluginDelegates.MemberEvent>(onMemberEvent);
        }
        public static PluginFunc CreatePluginFunction(IntPtr handle)
        {
            return new(handle);
        }

        public void OnInit(string pluginKey, string runPath)
        {
            if (null != initEvent)
                initEvent(pluginKey, runPath);
        }
        public PluginInfo? OnInfo()
        {
            if (null == appInfo)
                return null;
            var data = appInfo();
            return JsonSerializer.Deserialize<PluginInfo>(Marshal.PtrToStringAnsi(data))!;
        }
        public int OnLoad()
        {
            return onLoad();
        }
        public void OnMsg(string jsonData)
        {
            Task.Run(() =>
            {
                if (null != onGuildMessage)
                    onGuildMessage(jsonData);
            });
        }
        public bool OnCommand(string cmd)
        {
            if (null == onCommand)
                return false;
            return onCommand(cmd) == 1;
        }
        public void GuildEvent(string jsonData, GuildEventType type)
        {
            if (null != guildEvent)
                guildEvent(jsonData, type);
        }
        public void ChannelEvent(string jsonData, ChannelEventType type)
        {
            if (null != channelEvent)
                channelEvent(jsonData, type);
        }
        public void MemberEvent(string jsonData, MemberEventType type)
        {
            if (null != memberEvent)
                memberEvent(jsonData, type);
        }
    }
}
