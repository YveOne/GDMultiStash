#include "stdafx.h"
#include <set>
#include <stdio.h>
#include <iostream>
#include <string>
#include <stdlib.h>
#include "MessageType.h"
#include "SetModName.h"
#include "Exports.h"
#include "GrimTypes.h"

SetModName* SetModName::g_self;

void SetModName::EnableHook() {
	originalMethod = (OriginalMethodPtr)HookEngine(
		SET_MOD_NAME,
		HookedMethod,
		m_dataQueue,
		m_hEvent,
		TYPE_GameInfo_ModName
	);
}

SetModName::SetModName(DataQueue* dataQueue, HANDLE hEvent) {
	g_self = this;
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
}

SetModName::SetModName() {
	m_hEvent = nullptr;
}

void SetModName::DisableHook() {
	Unhook((PVOID*)&originalMethod, HookedMethod);
}

typedef std::basic_string<char, std::char_traits<char>, std::allocator<char> > const& Fancystring;

void* __fastcall SetModName::HookedMethod(void* This, std::basic_string<char, struct std::char_traits<char>, class std::allocator<char> > const& modName) {

	// is this correct? well .. its working :) yayyy
	std::wstringstream stream;
	stream << modName.c_str();
	std::wstring mn = stream.str();
	g_self->TransferData(mn.size() * sizeof(wchar_t), (char*)mn.c_str());

	void* v = g_self->originalMethod(This, modName);
	return v;
}