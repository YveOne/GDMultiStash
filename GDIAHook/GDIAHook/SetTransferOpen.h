#pragma once
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

class SetTransferOpen : public BaseMethodHook {
public:
	SetTransferOpen();
	SetTransferOpen(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook() override;
	void DisableHook() override;

private:
	typedef void* (__thiscall *OriginalMethodPtr)(void* This, bool isOpen);
	OriginalMethodPtr originalMethod;

	static SetTransferOpen* g_self;
	static void* __fastcall HookedMethod(void* This, bool isOpen);

	static void DisplayMessage(std::wstring, std::wstring);
	static ULONGLONG m_lastNotificationTickTime;
	static DataQueue* _m_dataQueue;
	static HANDLE _m_hEvent;

};