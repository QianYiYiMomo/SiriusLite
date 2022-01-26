#include "pch.h"
#include "SiriusLib.h"
#include <windows.h>
#include <stdio.h>
#include <intrin.h>
#include <tchar.h>
#include <ctime>
#define _CRT_SECURE_NO_WARNINGS
struct InitData
{
	CallBack EventCallBack;//消息回调
	GetPluginNameFun GetPName;//获取插件名
	CheckPluginFun CheckPlugin;//检查是否存在此插件
	AuthPluginFun AuthPlugin;//检查权限
	SendMsg SendMsg_Handle;//发消息
	SendMsg_Image SendMsg_Image_Handle;//发消息+图片
	GetMemberInfo_JSON GetMemberInfo_JSON_Handle;//取成员信息
	DeleteGuildRole DeleteGuildRole_Handle;//删除身份组
	GetGuildInfo GetGuildInfo_Handle;//获取频道信息
	CreateChannelAnnounce CreateChannelAnnounce_Handle;//创建子频道公告
	GetChannelInfo GetChannelInfo_Handle;//获取频道信息
	SetChannelPermissions SetChannelPermissions_Handle;//取用户权限
	AudioControl AudioControl_Handle;//控制音频
	GetBotInfo GetBotInfo_Handle;//取机器人信息
	GetChannelScheduleInfo GetChannelScheduleInfo_Handle;//取子频道日程
	CreateChannelSchedule CreateChannelSchedule_Handle;
	SetChannelSchedule SetChannelSchedule_Handle;
	DeleteChannelSchedule DeleteChannelSchedule_Handle;
	LogOut LogOut_Handle;
	LogOutWar LogOutWar_Handle;
	LogOutErr LogOutErr_Handle;
	LogOutPlugin LogOutPlugin_Handle;
	CreateGuildRole CreateGuildRole_Handle;
	SetGuildRole SetGuildRole_Handle;
	GetChannelPermissions GetChannelPermissions_Handle;// 取用户身份
	AddGuildRoleA AddGuildRoleA_Handle;// 用户身份组
	GetGuildRoleList GetGuildRoleList_Handle;// 取身份组列表
	DeleteGuildRoleA DeleteGuildRoleA_Handle;// 移出身份组
	DeleteChannelAnnounce DeleteChannelAnnounce_Handle;//删除频道公告
	GetGuildChannelListA GetGuildChannelListA_Handle;// 取频道列表
	MuteGuild MuteGuild_Handle;
	MuteGuildUser MuteGuildUser_Handle;
};
InitData SharpApi;

VOID __stdcall Init(InitData data) {
	SharpApi = data;
}//设置一个用于回调消息的指针地址

char* addChar(char* charA, char* charB) {
	char buffer[1024];
	memset(buffer, 0, sizeof(buffer));
	strcat_s(buffer, charA);
	strcat_s(buffer, charB);
	return buffer;
}

HMODULE _stdcall LoadPlugin(char* path) {
	return LoadLibraryA(path);
}//加载插件

BOOL _stdcall UnLoadPlugin(HMODULE handle) {
	if (NULL == handle)
		return FALSE;
	return FreeLibrary(handle);
}//卸载插件 (释放)

INT _stdcall GetEventCallBack() {
	return (int)SharpApi.EventCallBack;
}//获取用于回调消息的指针地址

VOID __stdcall CallEvent(char* data) {
	if (NULL != SharpApi.EventCallBack)
		SharpApi.EventCallBack(data);
}//调用事件
// 输出日志
VOID __stdcall Api_LogOut(char* key, char* msg)
{
	if (NULL != SharpApi.LogOut_Handle, SharpApi.AuthPlugin(key, "输出日志")) {
		SharpApi.LogOut_Handle(msg, key);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"输出日志 - 无权限", key);
	}
}
// 输出日志
VOID __stdcall Api_LogOutWar(char* key, char* msg) {
	if (NULL != SharpApi.LogOutWar_Handle, SharpApi.AuthPlugin(key, "输出日志")) {
		SharpApi.LogOutWar_Handle(msg, key);
	}
	else {
		SharpApi.LogOutErr_Handle("输出日志 - 无权限", key);
	}
}
// 输出日志
VOID __stdcall Api_LogOutErr(char* key, char* msg) {
	if (NULL != SharpApi.LogOutErr_Handle, SharpApi.AuthPlugin(key, "输出日志")) {
		SharpApi.LogOutErr_Handle(msg, key);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"输出日志 - 无权限", key);
	}
}
// 发送普通消息
CHAR* __stdcall Api_SendMsg(char* key, char* channel_Id, char* msg, char* msgId) {
	if (msgId == NULL) {
		if (NULL != SharpApi.SendMsg_Handle && SharpApi.AuthPlugin(key, "发送频道主动消息")) {
			SharpApi.LogOutPlugin_Handle(addChar((char*)"发送频道主动消息 -> ", msg), key);
			return  SharpApi.SendMsg_Handle(channel_Id, msg, msgId);
		}
		else {
			SharpApi.LogOutPlugin_Handle((char*)"发送频道主动消息 - 无权限", key);
			return (char*)"";
		}
	}
	else {
		if (NULL != SharpApi.SendMsg_Handle && SharpApi.AuthPlugin(key, "发送频道消息"))
		{
			SharpApi.LogOutPlugin_Handle(addChar((char*)"发送频道消息 -> ", msg), key);
			return  SharpApi.SendMsg_Handle(channel_Id, msg, msgId);
		}
		else {
			SharpApi.LogOutErr_Handle((char*)"发送频道消息 - 无权限", key);
			return (char*)"";
		}
	}
}
// 发送带图片消息
CHAR* __stdcall Api_SendMsg_Image(char* key, char* channel_Id, char* msg, char* msgId, char* imageUrl) {
	if (msgId == NULL) {
		if (NULL != SharpApi.SendMsg_Image_Handle && SharpApi.AuthPlugin(key, "发送频道主动消息")) {
			SharpApi.LogOutPlugin_Handle(addChar((char*)"发送频道主动消息 -> ", msg), key);
			return  SharpApi.SendMsg_Image_Handle(channel_Id, msg, msgId, imageUrl);
		}
		else {
			SharpApi.LogOutErr_Handle((char*)"发送频道主动消息 - 无权限", key);
			return (char*)"";
		}
	}
	else {
		if (NULL != SharpApi.SendMsg_Image_Handle && SharpApi.AuthPlugin(key, "发送频道消息")) {
			SharpApi.LogOutPlugin_Handle(addChar((char*)"发送频道消息 -> ", msg), key);
			return  SharpApi.SendMsg_Image_Handle(channel_Id, msg, msgId, imageUrl);
		}
		else {
			SharpApi.LogOutErr_Handle((char*)"发送频道消息 - 无权限", key);
			return (char*)"";
		}
	}
}
// 获取成员信息(Json版)
CHAR* __stdcall Api_GetMemberInfo_Json(char* key, char* guild_Id, char* userId) {
	if (NULL != SharpApi.GetMemberInfo_JSON_Handle && SharpApi.AuthPlugin(key, "获取成员信息")) {
		SharpApi.LogOutPlugin_Handle((char*)"获取成员信息", key);
		return SharpApi.GetMemberInfo_JSON_Handle(guild_Id, userId);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"获取成员信息 - 无权限", key);
		return (char*)"无权限";
	}
}
// 获取频道信息
CHAR* __stdcall Api_GetGuildInfo(char* key, char* guild_Id) {
	if (SharpApi.AuthPlugin(key, "获取频道信息")) {
		SharpApi.LogOutPlugin_Handle((char*)"获取频道信息", key);
		return SharpApi.GetGuildInfo_Handle(guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"获取频道信息 - 无权限", key);
		return (char*)"无权限";
	}
} //还没做
//创建身份组
CHAR* __stdcall Api_CreateGuildRole(char* key, int name, int color, int hoist, char* Name, long Color, int Hoist, char* guild_Id) {
	if (NULL != SharpApi.CreateGuildRole_Handle && SharpApi.AuthPlugin(key, "创建身份组")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"创建身份组 -> ", Name), key);
		return SharpApi.CreateGuildRole_Handle(name, color, hoist, Name, Color, Hoist, guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"创建身份组 - 无权限", key);
		return (char*)"无权限";
	}
}
//修改身份组
CHAR* __stdcall Api_SetGuildRole(char* key, int name, int color, int hoist, char* Name, long Color, int Hoist, char* guild_Id, char* role_Id) {
	if (NULL != SharpApi.SetGuildRole_Handle && SharpApi.AuthPlugin(key, "修改身份组")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"修改身份组 -> ", Name), key);
		return SharpApi.SetGuildRole_Handle(name, color, hoist, Name, Color, Hoist, guild_Id, role_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"修改身份组 - 无权限", key);
		return (char*)"无权限";
	}
}
//删除身份组
BOOL __stdcall Api_DeleteGuildRole(char* key, char* guild_Id, char* role_Id) {
	if (NULL != SharpApi.SetGuildRole_Handle && SharpApi.AuthPlugin(key, "删除身份组")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"删除身份组 -> ", role_Id), key);
		return SharpApi.DeleteGuildRole_Handle(guild_Id, role_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"删除身份组 - 无权限", key);
		return false;
	}
}
//置消息为公告
CHAR* __stdcall Api_CreateChannelAnnounce(char* key, char* channel_Id, char* msg_Id) {
	if (NULL != SharpApi.CreateChannelAnnounce_Handle && SharpApi.AuthPlugin(key, "置子频道公告")) {
		SharpApi.LogOutPlugin_Handle((char*)"置消息为公告 ", key);
		return SharpApi.CreateChannelAnnounce_Handle(channel_Id, msg_Id);
	}
	else
	{
		SharpApi.LogOutErr_Handle((char*)"置子频道公告 - 无权限", key);
		return (char*)"";
	}
}
//取子频道信息
CHAR* __stdcall Api_GetChannelInfo(char* key, char* channel_Id) {
	if (NULL != SharpApi.GetChannelInfo_Handle && SharpApi.AuthPlugin(key, "取子频道信息")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"取子频道信息 -> ", channel_Id), key);
		return SharpApi.GetChannelInfo_Handle(channel_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"取子频道信息 - 无权限", key);
		return (char*)"无权限";
	}
}
//取频道身份组列表
CHAR* __stdcall Api_GetGuildRoleList(char* key, char* guild_Id) {
	if (NULL != SharpApi.GetGuildRoleList_Handle && SharpApi.AuthPlugin(key, "取频道身份组列表")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"取频道身份组列表 -> ", guild_Id), key);
		return SharpApi.GetGuildRoleList_Handle(guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"取频道身份组列表 - 无权限", key);
		return (char*)"无权限";
	}
}
//加入身份组
BOOL __stdcall Api_AddGuildRole(char* key, char* channel_Id, char* guild_Id, char* role_Id, char* user_Id) {
	if (NULL != SharpApi.AddGuildRoleA_Handle && SharpApi.AuthPlugin(key, "加入身份组")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"加入身份组 -> ", channel_Id), key);
		return SharpApi.AddGuildRoleA_Handle(channel_Id, guild_Id, role_Id, user_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"加入身份组 - 无权限", key);
		return FALSE;
	}
}
//移出身份组用户
BOOL __stdcall Api_DeleteGuildRoleA(char* key, char* channel_Id, char* guild_Id, char* role_Id, char* user_Id) {
	if (NULL != SharpApi.DeleteGuildRoleA_Handle && SharpApi.AuthPlugin(key, "移出身份组用户")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"移出身份组用户 -> ", channel_Id), key);
		return SharpApi.DeleteGuildRoleA_Handle(channel_Id, guild_Id, role_Id, user_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"移出身份组用户 - 无权限", key);
		return FALSE;
	}
}
//取消消息公告
BOOL __stdcall Api_DeleteChannelAnnounce(char* key, char* channel_Id, char* msg_Id) {
	if (NULL != SharpApi.DeleteChannelAnnounce_Handle && SharpApi.AuthPlugin(key, "取消子频道公告")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"取消子频道公告 -> ", msg_Id), key);
		return SharpApi.DeleteChannelAnnounce_Handle(channel_Id, msg_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"取消子频道公告 - 无权限", key);
		return FALSE;
	}
}
//取频道子频道列表
CHAR* __stdcall Api_GetGuildChannelList(char* key, char* guild_Id) {
	if (NULL != SharpApi.GetGuildChannelListA_Handle && SharpApi.AuthPlugin(key, "取频道子频道列表")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"取频道子频道列表 -> ", guild_Id), key);
		return SharpApi.GetGuildChannelListA_Handle(guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"取频道子频道列表 - 无权限", key);
		return (char*)"无权限";
	}
}

//取用户子频道权限
CHAR* __stdcall Api_GetChannelPermissions(char* key, char* channel_Id, char* user_Id) {
	if (NULL != SharpApi.GetChannelPermissions_Handle && SharpApi.AuthPlugin(key, "取用户子频道权限")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"取用户子频道权限 -> ", channel_Id), key);
		return SharpApi.GetChannelPermissions_Handle(channel_Id, user_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"取用户子频道权限 - 无权限", key);
		return (char*)"无权限";
	}
}
//置用户子频道权限
BOOL __stdcall Api_SetChannelPermissions(char* key, char* channel_Id, char* user_Id, int add, int remove) {
	if (NULL != SharpApi.SetChannelPermissions_Handle && SharpApi.AuthPlugin(key, "置用户子频道权限")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"置用户子频道权限 -> ", channel_Id), key);
		return SharpApi.SetChannelPermissions_Handle(channel_Id, user_Id, add, remove);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"置用户子频道权限 - 无权限", key);
		return FALSE;
	}
}

//置全体禁言
BOOL __stdcall Api_MuteGuild(char* key, char* guild_Id,char* muteEndTimestamp, char* muteSeconds) {
	if (NULL != SharpApi.MuteGuild_Handle && SharpApi.AuthPlugin(key, "置全体禁言")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"置全体禁言 -> ", guild_Id), key);
		return SharpApi.MuteGuild_Handle(guild_Id, muteEndTimestamp, muteSeconds);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"置全体禁言 - 无权限", key);
		return FALSE;
	}
}
//置频道禁言
BOOL __stdcall Api_MuteGuildUser(char* key, char* guild_Id, char* user_Id, char* muteEndTimestamp, char* muteSeconds) {
	if (NULL != SharpApi.MuteGuildUser_Handle && SharpApi.AuthPlugin(key, "置频道禁言")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"置频道禁言 -> ", guild_Id), key);
		return SharpApi.MuteGuildUser_Handle(guild_Id, user_Id,muteEndTimestamp, muteSeconds);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"置频道禁言 - 无权限", key);
		return FALSE;
	}
}
//取机器人信息
CHAR* __stdcall Api_GetBotInfo(char* key) {
	if (NULL != SharpApi.GetBotInfo_Handle && SharpApi.AuthPlugin(key, "取机器人信息")) {
		SharpApi.LogOutPlugin_Handle((char*)"取机器人信息", key);
		return SharpApi.GetBotInfo_Handle();
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"取机器人信息 - 无权限", key);
		return FALSE;
	}
}