using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CSharpClient;
using Lang.Plugin.Interface;

namespace Lang.Plugin
{
    public class Plugin : IPlugin, IDisposable
    {
        private readonly IntPtr handle;
        private readonly PluginFunc pluginFunc;
        public bool Refuse { get; set; }
        public PluginInfo PluginInfo { get; set; }
        public string pluginFilePath { get; set; }
        public string pluginConfigPath { get; set; }
        public string pluginKey { get; set; }
        public Plugin(string path)
        {
            try
            {
                handle = kernel32.LoadLibraryA(path);
                pluginFunc = PluginFunc.CreatePluginFunction(handle);
                pluginKey = SiriusTools.GetRandomKey();
                PluginInfo = pluginFunc.OnInfo()!;
                if (null != PluginInfo)
                {
                    pluginFunc.OnInit(pluginKey, PluginData.CreatePluginData(PluginInfo.pluginName));
                    Refuse = pluginFunc.OnLoad() == 1;
                    pluginFilePath = path;
                }
                else
                {
                    Refuse = true;
                }
            }
            catch (Exception ex)
            {
                Log.Log.LogErr($"载入插件时出错! {ex}");
                Refuse = true;
            }
        }
        //public void OnGuildEvent(string msg, PluginManager.GuildType type)
        //{
        //    try
        //    {
        //        switch (type)
        //        {
        //            case PluginManager.GuildType.OnCreateGuild:
        //                Thread thread = new Thread(() =>
        //                {
        //                    if (null != pluginFunc.onCreateGuild)
        //                        pluginFunc.onCreateGuild.Invoke(msg);
        //                });
        //                thread.Start();
        //                break;
        //            case PluginManager.GuildType.OnUpdateGuild:
        //                Thread thread2 = new Thread(() =>
        //                {
        //                    if (null != pluginFunc.onCreateGuild)
        //                        pluginFunc.onCreateGuild.Invoke(msg);
        //                });
        //                thread2.Start();
        //                break;
        //            case PluginManager.GuildType.OnDeleteGuild:
        //                Thread thread3 = new Thread(() =>
        //                {
        //                    if (null != pluginFunc.onCreateGuild)
        //                        pluginFunc.onCreateGuild.Invoke(msg);
        //                });
        //                thread3.Start();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch
        //    {

        //    }
        //}
        public void Dispose()
        {
            PluginManager.eventMutex.WaitOne();
            GC.SuppressFinalize(this);
            OnStop();
            kernel32.FreeLibrary(handle);
            PluginManager.eventMutex.ReleaseMutex();
        }
        public void OnStart()
        {
        }
        public void OnStop()
        {
        }
        public void OnMessage(string jsonData)
        {
            pluginFunc.OnMsg(jsonData);
        }
        public bool OnCommand(string command)
        {
            return pluginFunc.OnCommand(command);
        }
        public void OnGuildEvent(string jsonData, GuildEventType type) => pluginFunc.GuildEvent(jsonData, type);
        public void OnChannelEvent(string jsonData, ChannelEventType type) => pluginFunc.ChannelEvent(jsonData, type);
        public void OnMemberEvent(string jsonData, MemberEventType type) => pluginFunc.MemberEvent(jsonData, type);

    }
}