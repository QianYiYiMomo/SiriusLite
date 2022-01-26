#include <iostream>
#include "pch.h"

struct Announces //�������
{
	const char* Guild_Id; //Ƶ��ID
	const char* Channe_ld; // ��Ƶ��ID
	const char* Message_Id;// ��ϢID
};

struct AudioContro //��Ƶ���ƶ���
{
	const char* Audio_Url; //����URL
	const char* Text; // ״̬�ı�
	int Audio_Status;// ״̬
};

struct AudioAction //��Ƶ���ƶ���
{
	const char* Guild_Id; //Ƶ��ID
	const char* Channel_Id; //��Ƶ��ID
	const char* Audio_Url; //����URL
	const char* Text; // ״̬�ı�
};

struct Channel //��Ƶ������
{
	const char* Channel_Id; //��Ƶ��ID
	const char* Guild_Id; //Ƶ��ID
	const char* Name; //��Ƶ����
	int Channel_Type; // ��Ƶ������
	int Channel_SubType;// ��Ƶ��������
	int Position;// ����
	const char* Parent_Id; //����ID
	const char* Owner_Id; //������ID
	int private_type; //��Ƶ��˽������
};

struct ChannelPermissions //��Ƶ��Ȩ�޶���
{
	const char* Channel_Id; //��Ƶ��ID
	const char* User_Id; //�û���
	const char* Permissions;// �û�ӵ�е���Ƶ��Ȩ��
};

struct Emoji //����
{
	const char* Id; //����ID
	int Type; //���� 0�ٷ� 1Emoji
};

struct Guild// Ƶ������
{
     const char* Id;// Ƶ��ID
     const char* Name;// Ƶ������
     const char* Icon;// Ƶ��ͷ���ַ
     const char* OwnerId;// �������û�ID
     bool owner;// ��ǰ���Ƿ��Ǵ�����
     int MemberCount;// ��Ա��
     int MaxMembers; // ����Ա��
     const char* Description; // ����
     const char* JoinedAt;// ����ʱ��
};

struct Role // Ƶ����������
{
	const char* Id;// �����ID
	const char* Name;// ����
	long Color;// ARGB��HEXʮ��������ɫֵת�����ʮ������ֵ
	int Hoist;// �Ƿ��ڳ�Ա�б��е���չʾ: 0-��, 1-��
	int Number;// ����
	int MemberLimit;// ��Ա����
};

struct Schedule// �ճ̶���
{
     const char* Id;// �����ID
     const char* Name;// ����
     long Color;// ARGB��HEXʮ��������ɫֵת�����ʮ������ֵ
     int Hoist;// �Ƿ��ڳ�Ա�б��е���չʾ: 0-��, 1-��
     int Number;// ����
     int MemberLimit;// ��Ա����
};

struct User//�û����� (��û�ж���
{
	const char* Id;//�û� id
	const char* UserName;//�û�ͷ���ַ
	const char* AvatarUrl;//�û�ͷ���ַ
	bool Bot;//�Ƿ��ǻ�����
	const char* UnionPpenId;//�������Ӧ�õ� openid����Ҫ�������벢���ú�Ż᷵�ء��������룬����ϵƽ̨��Ӫ��Ա��
	const char* UnionUserAccount;//�����˹����Ļ���Ӧ�õ��û���Ϣ����union_openid������Ӧ����ͬһ�����������룬����ϵƽ̨��Ӫ��Ա��
};