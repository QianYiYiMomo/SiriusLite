#include  "pch.h"

struct COpenAPI {
public:

	const char* OpenApiPostMessage(const char* Ƶ��ID, const char* msg_json) {
		typedef const char* (*Method)(const char*, const char*);


		return reinterpret_cast<Method>(this->post_message_func)(Ƶ��ID, msg_json);

	}

	void OpenApiFunc2() {
		typedef void (*Method)();
		reinterpret_cast<Method>(this->func_2)();
	}


	COpenAPI(COpenAPI* api) {
		this->post_message_func = api->post_message_func;
		this->func_2 = api->func_2;
	}

private:

	uintptr_t post_message_func = 0;
	uintptr_t func_2 = 0;

};
COpenAPI* api;
void init(COpenAPI* c_open_api) {
	api = new COpenAPI(c_open_api);
}
void a() {

	api->OpenApiPostMessage("Ƶ��ID", "��Ϣ����");

}