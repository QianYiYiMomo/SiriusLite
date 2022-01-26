using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang.Plugin
{
    public class PluginDelegates
    {
        public delegate void Init(string key, string runPath); //初始化插件
        public delegate IntPtr AppInfo(); //初始化插件 -> 返回插件信息
        public delegate void Menu();// 插件菜单
        public delegate int OnLoad();// 加载插件
        public delegate void OnUnLoad();// 卸载插件
        public delegate void OnGuildMessage(string jsonData); // 收到消息
        public delegate int OnEnable();
        public delegate void OnDisable();
        public delegate int OnCommand(string command);
        public delegate void GuildEvent(string jsonData,GuildEventType eventType);
        public delegate void ChannelEvent(string jsonData,ChannelEventType eventType);
        public delegate void MemberEvent(string jsonData,MemberEventType eventType);
    }
    public enum GuildEventType
    {
        /// <summary>
        /// 机器人被加入到某个频道的时候
        /// </summary>
        GUILD_CREATE = 20001,
        /// <summary>
        /// 频道信息变更
        /// </summary>
        GUILD_UPDATE = 20002,
        /// <summary>
        /// 频道被解散或机器人被移除
        /// </summary>
        GUILD_DELETE = 20003
    }
    public enum ChannelEventType
    {
        /// <summary>
        /// 子频道被创建
        /// </summary>
        CHANNEL_CREATE = 30001,
        /// <summary>
        /// 子频道信息变更
        /// </summary>
        CHANNEL_UPDATE = 30002,
        /// <summary>
        /// 子频道被删除
        /// </summary>
        CHANNEL_DELETE = 30003
    }
    public enum MemberEventType
    {
        /// <summary>
        /// 新用户加入频道
        /// </summary>
        GUILD_MEMBER_ADD = 40001,
        /// <summary>
        /// 暂无
        /// </summary>
        MEMBER_UPDATE = 40002,
        /// <summary>
        /// 用户离开频道
        /// </summary>
        MEMBER_REMOVE = 40003
    }
}
