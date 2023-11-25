#pragma once
#include <sstream>
#include <windows.h>
#include "DataQueue.h"
#include "BaseMethodHook.h"


class SetModName : public BaseMethodHook {
public:
	SetModName();
	SetModName(DataQueue* dataQueue, HANDLE hEvent);
	void EnableHook() override;
	void DisableHook() override;

private:
	typedef void* (__thiscall* OriginalMethodPtr)(void* This, std::basic_string<char, struct std::char_traits<char>, class std::allocator<char> > const& modName);
	OriginalMethodPtr originalMethod;

	static SetModName* g_self;
	static void* __fastcall HookedMethod(void* This, std::basic_string<char, struct std::char_traits<char>, class std::allocator<char> > const& modName);
};