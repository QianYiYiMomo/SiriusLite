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
	CallBack EventCallBack;//��Ϣ�ص�
	GetPluginNameFun GetPName;//��ȡ�����
	CheckPluginFun CheckPlugin;//����Ƿ���ڴ˲��
	AuthPluginFun AuthPlugin;//���Ȩ��
	SendMsg SendMsg_Handle;//����Ϣ
	SendMsg_Image SendMsg_Image_Handle;//����Ϣ+ͼƬ
	GetMemberInfo_JSON GetMemberInfo_JSON_Handle;//ȡ��Ա��Ϣ
	DeleteGuildRole DeleteGuildRole_Handle;//ɾ�������
	GetGuildInfo GetGuildInfo_Handle;//��ȡƵ����Ϣ
	CreateChannelAnnounce CreateChannelAnnounce_Handle;//������Ƶ������
	GetChannelInfo GetChannelInfo_Handle;//��ȡƵ����Ϣ
	SetChannelPermissions SetChannelPermissions_Handle;//ȡ�û�Ȩ��
	AudioControl AudioControl_Handle;//������Ƶ
	GetBotInfo GetBotInfo_Handle;//ȡ��������Ϣ
	GetChannelScheduleInfo GetChannelScheduleInfo_Handle;//ȡ��Ƶ���ճ�
	CreateChannelSchedule CreateChannelSchedule_Handle;
	SetChannelSchedule SetChannelSchedule_Handle;
	DeleteChannelSchedule DeleteChannelSchedule_Handle;
	LogOut LogOut_Handle;
	LogOutWar LogOutWar_Handle;
	LogOutErr LogOutErr_Handle;
	LogOutPlugin LogOutPlugin_Handle;
	CreateGuildRole CreateGuildRole_Handle;
	SetGuildRole SetGuildRole_Handle;
	GetChannelPermissions GetChannelPermissions_Handle;// ȡ�û����
	AddGuildRoleA AddGuildRoleA_Handle;// �û������
	GetGuildRoleList GetGuildRoleList_Handle;// ȡ������б�
	DeleteGuildRoleA DeleteGuildRoleA_Handle;// �Ƴ������
	DeleteChannelAnnounce DeleteChannelAnnounce_Handle;//ɾ��Ƶ������
	GetGuildChannelListA GetGuildChannelListA_Handle;// ȡƵ���б�
	MuteGuild MuteGuild_Handle;
	MuteGuildUser MuteGuildUser_Handle;
};
InitData SharpApi;

VOID __stdcall Init(InitData data) {
	SharpApi = data;
}//����һ�����ڻص���Ϣ��ָ���ַ

char* addChar(char* charA, char* charB) {
	char buffer[1024];
	memset(buffer, 0, sizeof(buffer));
	strcat_s(buffer, charA);
	strcat_s(buffer, charB);
	return buffer;
}

HMODULE _stdcall LoadPlugin(char* path) {
	return LoadLibraryA(path);
}//���ز��

BOOL _stdcall UnLoadPlugin(HMODULE handle) {
	if (NULL == handle)
		return FALSE;
	return FreeLibrary(handle);
}//ж�ز�� (�ͷ�)

INT _stdcall GetEventCallBack() {
	return (int)SharpApi.EventCallBack;
}//��ȡ���ڻص���Ϣ��ָ���ַ

VOID __stdcall CallEvent(char* data) {
	if (NULL != SharpApi.EventCallBack)
		SharpApi.EventCallBack(data);
}//�����¼�
// �����־
VOID __stdcall Api_LogOut(char* key, char* msg)
{
	if (NULL != SharpApi.LogOut_Handle, SharpApi.AuthPlugin(key, "�����־")) {
		SharpApi.LogOut_Handle(msg, key);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"�����־ - ��Ȩ��", key);
	}
}
// �����־
VOID __stdcall Api_LogOutWar(char* key, char* msg) {
	if (NULL != SharpApi.LogOutWar_Handle, SharpApi.AuthPlugin(key, "�����־")) {
		SharpApi.LogOutWar_Handle(msg, key);
	}
	else {
		SharpApi.LogOutErr_Handle("�����־ - ��Ȩ��", key);
	}
}
// �����־
VOID __stdcall Api_LogOutErr(char* key, char* msg) {
	if (NULL != SharpApi.LogOutErr_Handle, SharpApi.AuthPlugin(key, "�����־")) {
		SharpApi.LogOutErr_Handle(msg, key);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"�����־ - ��Ȩ��", key);
	}
}
// ������ͨ��Ϣ
CHAR* __stdcall Api_SendMsg(char* key, char* channel_Id, char* msg, char* msgId) {
	if (msgId == NULL) {
		if (NULL != SharpApi.SendMsg_Handle && SharpApi.AuthPlugin(key, "����Ƶ��������Ϣ")) {
			SharpApi.LogOutPlugin_Handle(addChar((char*)"����Ƶ��������Ϣ -> ", msg), key);
			return  SharpApi.SendMsg_Handle(channel_Id, msg, msgId);
		}
		else {
			SharpApi.LogOutPlugin_Handle((char*)"����Ƶ��������Ϣ - ��Ȩ��", key);
			return (char*)"";
		}
	}
	else {
		if (NULL != SharpApi.SendMsg_Handle && SharpApi.AuthPlugin(key, "����Ƶ����Ϣ"))
		{
			SharpApi.LogOutPlugin_Handle(addChar((char*)"����Ƶ����Ϣ -> ", msg), key);
			return  SharpApi.SendMsg_Handle(channel_Id, msg, msgId);
		}
		else {
			SharpApi.LogOutErr_Handle((char*)"����Ƶ����Ϣ - ��Ȩ��", key);
			return (char*)"";
		}
	}
}
// ���ʹ�ͼƬ��Ϣ
CHAR* __stdcall Api_SendMsg_Image(char* key, char* channel_Id, char* msg, char* msgId, char* imageUrl) {
	if (msgId == NULL) {
		if (NULL != SharpApi.SendMsg_Image_Handle && SharpApi.AuthPlugin(key, "����Ƶ��������Ϣ")) {
			SharpApi.LogOutPlugin_Handle(addChar((char*)"����Ƶ��������Ϣ -> ", msg), key);
			return  SharpApi.SendMsg_Image_Handle(channel_Id, msg, msgId, imageUrl);
		}
		else {
			SharpApi.LogOutErr_Handle((char*)"����Ƶ��������Ϣ - ��Ȩ��", key);
			return (char*)"";
		}
	}
	else {
		if (NULL != SharpApi.SendMsg_Image_Handle && SharpApi.AuthPlugin(key, "����Ƶ����Ϣ")) {
			SharpApi.LogOutPlugin_Handle(addChar((char*)"����Ƶ����Ϣ -> ", msg), key);
			return  SharpApi.SendMsg_Image_Handle(channel_Id, msg, msgId, imageUrl);
		}
		else {
			SharpApi.LogOutErr_Handle((char*)"����Ƶ����Ϣ - ��Ȩ��", key);
			return (char*)"";
		}
	}
}
// ��ȡ��Ա��Ϣ(Json��)
CHAR* __stdcall Api_GetMemberInfo_Json(char* key, char* guild_Id, char* userId) {
	if (NULL != SharpApi.GetMemberInfo_JSON_Handle && SharpApi.AuthPlugin(key, "��ȡ��Ա��Ϣ")) {
		SharpApi.LogOutPlugin_Handle((char*)"��ȡ��Ա��Ϣ", key);
		return SharpApi.GetMemberInfo_JSON_Handle(guild_Id, userId);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"��ȡ��Ա��Ϣ - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}
// ��ȡƵ����Ϣ
CHAR* __stdcall Api_GetGuildInfo(char* key, char* guild_Id) {
	if (SharpApi.AuthPlugin(key, "��ȡƵ����Ϣ")) {
		SharpApi.LogOutPlugin_Handle((char*)"��ȡƵ����Ϣ", key);
		return SharpApi.GetGuildInfo_Handle(guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"��ȡƵ����Ϣ - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
} //��û��
//���������
CHAR* __stdcall Api_CreateGuildRole(char* key, int name, int color, int hoist, char* Name, long Color, int Hoist, char* guild_Id) {
	if (NULL != SharpApi.CreateGuildRole_Handle && SharpApi.AuthPlugin(key, "���������")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"��������� -> ", Name), key);
		return SharpApi.CreateGuildRole_Handle(name, color, hoist, Name, Color, Hoist, guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"��������� - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}
//�޸������
CHAR* __stdcall Api_SetGuildRole(char* key, int name, int color, int hoist, char* Name, long Color, int Hoist, char* guild_Id, char* role_Id) {
	if (NULL != SharpApi.SetGuildRole_Handle && SharpApi.AuthPlugin(key, "�޸������")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"�޸������ -> ", Name), key);
		return SharpApi.SetGuildRole_Handle(name, color, hoist, Name, Color, Hoist, guild_Id, role_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"�޸������ - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}
//ɾ�������
BOOL __stdcall Api_DeleteGuildRole(char* key, char* guild_Id, char* role_Id) {
	if (NULL != SharpApi.SetGuildRole_Handle && SharpApi.AuthPlugin(key, "ɾ�������")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"ɾ������� -> ", role_Id), key);
		return SharpApi.DeleteGuildRole_Handle(guild_Id, role_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ɾ������� - ��Ȩ��", key);
		return false;
	}
}
//����ϢΪ����
CHAR* __stdcall Api_CreateChannelAnnounce(char* key, char* channel_Id, char* msg_Id) {
	if (NULL != SharpApi.CreateChannelAnnounce_Handle && SharpApi.AuthPlugin(key, "����Ƶ������")) {
		SharpApi.LogOutPlugin_Handle((char*)"����ϢΪ���� ", key);
		return SharpApi.CreateChannelAnnounce_Handle(channel_Id, msg_Id);
	}
	else
	{
		SharpApi.LogOutErr_Handle((char*)"����Ƶ������ - ��Ȩ��", key);
		return (char*)"";
	}
}
//ȡ��Ƶ����Ϣ
CHAR* __stdcall Api_GetChannelInfo(char* key, char* channel_Id) {
	if (NULL != SharpApi.GetChannelInfo_Handle && SharpApi.AuthPlugin(key, "ȡ��Ƶ����Ϣ")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"ȡ��Ƶ����Ϣ -> ", channel_Id), key);
		return SharpApi.GetChannelInfo_Handle(channel_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ȡ��Ƶ����Ϣ - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}
//ȡƵ��������б�
CHAR* __stdcall Api_GetGuildRoleList(char* key, char* guild_Id) {
	if (NULL != SharpApi.GetGuildRoleList_Handle && SharpApi.AuthPlugin(key, "ȡƵ��������б�")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"ȡƵ��������б� -> ", guild_Id), key);
		return SharpApi.GetGuildRoleList_Handle(guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ȡƵ��������б� - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}
//���������
BOOL __stdcall Api_AddGuildRole(char* key, char* channel_Id, char* guild_Id, char* role_Id, char* user_Id) {
	if (NULL != SharpApi.AddGuildRoleA_Handle && SharpApi.AuthPlugin(key, "���������")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"��������� -> ", channel_Id), key);
		return SharpApi.AddGuildRoleA_Handle(channel_Id, guild_Id, role_Id, user_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"��������� - ��Ȩ��", key);
		return FALSE;
	}
}
//�Ƴ�������û�
BOOL __stdcall Api_DeleteGuildRoleA(char* key, char* channel_Id, char* guild_Id, char* role_Id, char* user_Id) {
	if (NULL != SharpApi.DeleteGuildRoleA_Handle && SharpApi.AuthPlugin(key, "�Ƴ�������û�")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"�Ƴ�������û� -> ", channel_Id), key);
		return SharpApi.DeleteGuildRoleA_Handle(channel_Id, guild_Id, role_Id, user_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"�Ƴ�������û� - ��Ȩ��", key);
		return FALSE;
	}
}
//ȡ����Ϣ����
BOOL __stdcall Api_DeleteChannelAnnounce(char* key, char* channel_Id, char* msg_Id) {
	if (NULL != SharpApi.DeleteChannelAnnounce_Handle && SharpApi.AuthPlugin(key, "ȡ����Ƶ������")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"ȡ����Ƶ������ -> ", msg_Id), key);
		return SharpApi.DeleteChannelAnnounce_Handle(channel_Id, msg_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ȡ����Ƶ������ - ��Ȩ��", key);
		return FALSE;
	}
}
//ȡƵ����Ƶ���б�
CHAR* __stdcall Api_GetGuildChannelList(char* key, char* guild_Id) {
	if (NULL != SharpApi.GetGuildChannelListA_Handle && SharpApi.AuthPlugin(key, "ȡƵ����Ƶ���б�")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"ȡƵ����Ƶ���б� -> ", guild_Id), key);
		return SharpApi.GetGuildChannelListA_Handle(guild_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ȡƵ����Ƶ���б� - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}

//ȡ�û���Ƶ��Ȩ��
CHAR* __stdcall Api_GetChannelPermissions(char* key, char* channel_Id, char* user_Id) {
	if (NULL != SharpApi.GetChannelPermissions_Handle && SharpApi.AuthPlugin(key, "ȡ�û���Ƶ��Ȩ��")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"ȡ�û���Ƶ��Ȩ�� -> ", channel_Id), key);
		return SharpApi.GetChannelPermissions_Handle(channel_Id, user_Id);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ȡ�û���Ƶ��Ȩ�� - ��Ȩ��", key);
		return (char*)"��Ȩ��";
	}
}
//���û���Ƶ��Ȩ��
BOOL __stdcall Api_SetChannelPermissions(char* key, char* channel_Id, char* user_Id, int add, int remove) {
	if (NULL != SharpApi.SetChannelPermissions_Handle && SharpApi.AuthPlugin(key, "���û���Ƶ��Ȩ��")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"���û���Ƶ��Ȩ�� -> ", channel_Id), key);
		return SharpApi.SetChannelPermissions_Handle(channel_Id, user_Id, add, remove);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"���û���Ƶ��Ȩ�� - ��Ȩ��", key);
		return FALSE;
	}
}

//��ȫ�����
BOOL __stdcall Api_MuteGuild(char* key, char* guild_Id,char* muteEndTimestamp, char* muteSeconds) {
	if (NULL != SharpApi.MuteGuild_Handle && SharpApi.AuthPlugin(key, "��ȫ�����")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"��ȫ����� -> ", guild_Id), key);
		return SharpApi.MuteGuild_Handle(guild_Id, muteEndTimestamp, muteSeconds);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"��ȫ����� - ��Ȩ��", key);
		return FALSE;
	}
}
//��Ƶ������
BOOL __stdcall Api_MuteGuildUser(char* key, char* guild_Id, char* user_Id, char* muteEndTimestamp, char* muteSeconds) {
	if (NULL != SharpApi.MuteGuildUser_Handle && SharpApi.AuthPlugin(key, "��Ƶ������")) {
		SharpApi.LogOutPlugin_Handle(addChar((char*)"��Ƶ������ -> ", guild_Id), key);
		return SharpApi.MuteGuildUser_Handle(guild_Id, user_Id,muteEndTimestamp, muteSeconds);
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"��Ƶ������ - ��Ȩ��", key);
		return FALSE;
	}
}
//ȡ��������Ϣ
CHAR* __stdcall Api_GetBotInfo(char* key) {
	if (NULL != SharpApi.GetBotInfo_Handle && SharpApi.AuthPlugin(key, "ȡ��������Ϣ")) {
		SharpApi.LogOutPlugin_Handle((char*)"ȡ��������Ϣ", key);
		return SharpApi.GetBotInfo_Handle();
	}
	else {
		SharpApi.LogOutErr_Handle((char*)"ȡ��������Ϣ - ��Ȩ��", key);
		return FALSE;
	}
}