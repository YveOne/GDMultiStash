#pragma once
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"

/************************************************************************
 Hook for the in-game "GetPrivateStash" method
 Called when opening the stash

 This is spammed non stop when the private stash is open (not transfer)
/************************************************************************/
class GetPrivateStash : public BaseMethodHook {
public:
	GetPrivateStash();
	GetPrivateStash(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook() override;
	void DisableHook() override;

	void* GetPrivateStashInventorySack();

private:
	typedef int* (__thiscall *OriginalMethodPtr)(void*);
	static HANDLE m_hEvent;
	static OriginalMethodPtr originalMethod;
	static DataQueue* m_dataQueue;
	static void* privateStashSack;

	static void* __stdcall HookedMethod64(void* This);
};