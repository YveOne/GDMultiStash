#include "stdafx.h"
#include <set>
#include <stdio.h>
#include <stdlib.h>
#include "MessageType.h"
#include "SetTransferOpen.h"
#include "Exports.h"

#include "GrimTypes.h"
#include "SetHardcore.h"

SetTransferOpen* SetTransferOpen::g_self;


ULONGLONG SetTransferOpen::m_lastNotificationTickTime;
DataQueue* SetTransferOpen::_m_dataQueue;
HANDLE SetTransferOpen::_m_hEvent;

void SetTransferOpen::EnableHook() {
	originalMethod = (OriginalMethodPtr)HookGame(
		SET_TRANSFER_OPEN,
		HookedMethod,
		m_dataQueue,
		m_hEvent,
		TYPE_OPEN_CLOSE_TRANSFER_STASH
	);
}

SetTransferOpen::SetTransferOpen(DataQueue* dataQueue, HANDLE hEvent) {
	g_self = this;
	m_dataQueue = dataQueue;
	m_hEvent = hEvent;
	SetTransferOpen::_m_dataQueue = dataQueue;
	SetTransferOpen::_m_hEvent = hEvent;
}

SetTransferOpen::SetTransferOpen() {
	m_hEvent = nullptr;
}

void SetTransferOpen::DisableHook() {
	Unhook((PVOID*)&originalMethod, HookedMethod);
}


void* __fastcall SetTransferOpen::HookedMethod(void* This, bool isOpen) {

	GAME::Engine* engine = fnGetEngine();
	GAME::GameInfo* gameInfo = fnGetGameInfo(engine);
	if (gameInfo != nullptr) {

			int expansionId = fnGetLoadedExpansionId(engine);
			DataItemPtr item1(new DataItem(TYPE_GameInfo_Expansion, sizeof(expansionId), (char*)&expansionId));
			_m_dataQueue->push(item1);
			SetEvent(_m_hEvent);

			bool isHardcore = fnGetHardcore(gameInfo);
			DataItemPtr item2(new DataItem(TYPE_GameInfo_IsHardcore, sizeof(isHardcore), (char*)&isHardcore));
			_m_dataQueue->push(item2);
			SetEvent(_m_hEvent);

			std::wstring modName;
			//int modIndex = fnGetGameInfoMode(gameInfo);

			fnGetModNameArg(gameInfo, &modName);
			DataItemPtr item3(new DataItem(TYPE_GameInfo_ModName, modName.size() * sizeof(wchar_t), (char*)modName.c_str()));
			_m_dataQueue->push(item3);
			SetEvent(_m_hEvent);

			modName = L"LOL";
			DataItemPtr item4(new DataItem(TYPE_GameInfo_ModName, modName.size() * sizeof(wchar_t), (char*)modName.c_str()));
			_m_dataQueue->push(item4);
			SetEvent(_m_hEvent);


			//if (modIndex == 0 || modName == L"survivalmode") {
				char b[1];
				b[0] = (isOpen ? 1 : 0);
				g_self->TransferData(1, (char*)b);
			//}

	}


	void* v = g_self->originalMethod(This, isOpen);
	return v;
}

void SetTransferOpen::DisplayMessage(std::wstring text, std::wstring body) {
	const ULONGLONG now = GetTickCount64();

	// Limit notifications to 1 per 3s, roughly the fade time.
	if (now - m_lastNotificationTickTime > 3000) {
		GAME::Color color;
		color.r = 1;
		color.g = 1;
		color.b = 1;
		color.a = 1;

		// TODO: How can translation support be added?

		GAME::Engine* engine = fnGetEngine();
		fnShowCinematicText(engine, &text, &body, 5, &color);
		m_lastNotificationTickTime = now;
	}
}
