#include <iostream>
#include "pch.h"

struct Announces //公告对象
{
	const char* Guild_Id; //频道ID
	const char* Channe_ld; // 子频道ID
	const char* Message_Id;// 消息ID
};

struct AudioContro //音频控制对象
{
	const char* Audio_Url; //语音URL
	const char* Text; // 状态文本
	int Audio_Status;// 状态
};

struct AudioAction //音频控制对象
{
	const char* Guild_Id; //频道ID
	const char* Channel_Id; //子频道ID
	const char* Audio_Url; //语音URL
	const char* Text; // 状态文本
};

struct Channel //子频道对象
{
	const char* Channel_Id; //子频道ID
	const char* Guild_Id; //频道ID
	const char* Name; //子频道名
	int Channel_Type; // 子频道类型
	int Channel_SubType;// 子频道子类型
	int Position;// 排序
	const char* Parent_Id; //分组ID
	const char* Owner_Id; //创建人ID
	int private_type; //子频道私密类型
};

struct ChannelPermissions //子频道权限对象
{
	const char* Channel_Id; //子频道ID
	const char* User_Id; //用户名
	const char* Permissions;// 用户拥有的子频道权限
};

struct Emoji //表情
{
	const char* Id; //表情ID
	int Type; //类型 0官方 1Emoji
};

struct Guild// 频道对象
{
     const char* Id;// 频道ID
     const char* Name;// 频道名称
     const char* Icon;// 频道头像地址
     const char* OwnerId;// 创建人用户ID
     bool owner;// 当前人是否是创建人
     int MemberCount;// 成员数
     int MaxMembers; // 最大成员数
     const char* Description; // 描述
     const char* JoinedAt;// 加入时间
};

struct Role // 频道身份组对象
{
	const char* Id;// 身份组ID
	const char* Name;// 名称
	long Color;// ARGB的HEX十六进制颜色值转换后的十进制数值
	int Hoist;// 是否在成员列表中单独展示: 0-否, 1-是
	int Number;// 人数
	int MemberLimit;// 成员上限
};

struct Schedule// 日程对象
{
     const char* Id;// 身份组ID
     const char* Name;// 名称
     long Color;// ARGB的HEX十六进制颜色值转换后的十进制数值
     int Hoist;// 是否在成员列表中单独展示: 0-否, 1-是
     int Number;// 人数
     int MemberLimit;// 成员上限
};

struct User//用户对象 (我没有对象
{
	const char* Id;//用户 id
	const char* UserName;//用户头像地址
	const char* AvatarUrl;//用户头像地址
	bool Bot;//是否是机器人
	const char* UnionPpenId;//特殊关联应用的 openid，需要特殊申请并配置后才会返回。如需申请，请联系平台运营人员。
	const char* UnionUserAccount;//机器人关联的互联应用的用户信息，与union_openid关联的应用是同一个。如需申请，请联系平台运营人员。
};