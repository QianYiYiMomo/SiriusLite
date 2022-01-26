#pragma once
#include "Entity.cpp"
typedef void(__stdcall* CallBack)  (char* data);
typedef char* (__stdcall* GetPluginNameFun)  (char* pluginKey);
typedef bool(__stdcall* CheckPluginFun)  (char* pluginKey);
typedef bool(__stdcall* AuthPluginFun)  (char* pluginKey, const char* auth);

typedef char* (__stdcall* SendMsg)(char* channel_Id, char* msg, char* msgId);
typedef char* (__stdcall* SendMsg_Image)(char* channel_Id, char* msg, char* msgId, char* imageUrl);
//typedef char* (_stdcall* SendMsgEx)(char* channel_Id, char* msg, char* msgId, char* imageUrl, MessageArk msgArk, MessageEmbed msgEmbed);
typedef char* (_stdcall* GetMemberInfo_JSON)(char* guild_Id, char* userId);
typedef char* (_stdcall* GetGuildInfo)(char* guild_id);
typedef char* (_stdcall* CreateGuildRole)(int name, int color, int hoist, char* Name, int Color, int Hoist, char* guild_Id);
typedef char* (_stdcall* SetGuildRole)(int name, int color, int hoist, char* Name, int Color, int Hoist, char* guild_Id, char* role_Id);
typedef bool(_stdcall* DeleteGuildRole)(char* guild_Id, char* role_Id);
typedef char* (_stdcall* CreateChannelAnnounce)(char* channel_Id, char* message_Id);
typedef char* (_stdcall* GetChannelInfo)(char* channel_Id);
typedef char* (_stdcall* GetGuildRoleList)(char* guild_Id);
//typedef Channel[](_stdcall* GetGuildChannelList)(char* guild_Id);
typedef char*(_stdcall* GetChannelPermissions)(char* channel_Id, char* user_Id);
typedef bool(_stdcall* SetChannelPermissions)(char* channel_Id, char* user_Id, int addPermissions, int removePermissions);
typedef bool(_stdcall* AudioControl)(char* channel_Id, AudioContro audioContro);
typedef char*(_stdcall* GetBotInfo)();
//typedef Guild[](_stdcall* GetGuildList)(char* before, char* after, int limit);
//typedef Schedule[](_stdcall* GetChannelScheduleList)(char* channel_Id, int since);
typedef Schedule(_stdcall* GetChannelScheduleInfo)(char* channel_Id, char* schedule_Id);
typedef Schedule(_stdcall* CreateChannelSchedule)(char* channel_Id, Schedule schedule);
typedef Schedule(_stdcall* SetChannelSchedule)(char* channel_Id, char* schedule_Id, Schedule schedule);
typedef bool(_stdcall* DeleteChannelSchedule)(char* channel_Id, char* schedule_Id);
typedef void(_stdcall* LogOut)(char* msg, char* key);
typedef void(_stdcall* LogOutWar)(char* msg, char* key);
typedef void(_stdcall* LogOutErr)(const char* msg, const char* key);
typedef void(_stdcall* LogOutPlugin)(char* msg, char* key);
typedef bool(_stdcall* AddGuildRoleA)(char* channel_Id, char* guild_Id, char* role_Id, char* user_Id);
typedef bool(_stdcall* DeleteGuildRoleA)(char* channel_Id, char* guild_Id, char* role_Id, char* user_Id);
typedef bool(_stdcall* DeleteChannelAnnounce)(char* channel_Id, char* message_Id);
typedef char* (_stdcall* GetGuildChannelListA)(char* guild_Id);
typedef bool (_stdcall* MuteGuild)(char* guild_Id, char* muteEndTimestamp, char* muteSeconds);
typedef bool(_stdcall* MuteGuildUser)(char* guild_Id, char* user_Id, char* muteEndTimestamp, char* muteSeconds);

__declspec(dllexport) HMODULE __stdcall LoadPlugin(char* path);
__declspec(dllexport) BOOL __stdcall UnLoadPlugin(HMODULE handle);
__declspec(dllexport) INT __stdcall GetEventCallBack();
__declspec(dllexport) VOID __stdcall Init(CallBack eventCallBack);
__declspec(dllexport) VOID __stdcall CallEvent(char* data);