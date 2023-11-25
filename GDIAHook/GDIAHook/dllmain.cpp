#include "stdafx.h"
#include <chrono>
#include <codecvt>
#include <windows.h>
#include <stdlib.h>
#include "DataQueue.h"
#include "MessageType.h"
#include "SaveTransferStash.h"
#include "Exports.h"
#include "HookLog.h"
#include "SetTransferOpen.h"
#include "SetHardcore.h"
#include "SetModName.h"
HookLog g_log;

#include "GrimTypes.h"

#pragma region Variables
// Switches hook logging on/off
#if 1
#define LOG(streamdef) \
{ \
    std::wstring msg = (((std::wostringstream&)(std::wostringstream().flush() << streamdef)).str()); \
	g_log.out(logStartupTime() + msg); \
    msg += _T("\n"); \
    OutputDebugString(msg.c_str()); \
}
#else
#define LOG(streamdef) \
    __noop;
#endif




DWORD g_lastThreadTick = 0;
HANDLE g_hEvent;
HANDLE g_thread;

DataQueue g_dataQueue;

HWND g_targetWnd = NULL;

#pragma endregion

#pragma region CORE


std::wstring logStartupTime() {
	__time64_t rawtime;
	struct tm timeinfo;
	wchar_t buffer[80];

	_time64(&rawtime);
	localtime_s(&timeinfo, &rawtime);

	wcsftime(buffer, sizeof(buffer), L"%Y-%m-%d %H:%M:%S ", &timeinfo);
	std::wstring str(buffer);

	return str;
}

void LogToFile(const wchar_t* message) {
	g_log.out(logStartupTime() + message);
}
void LogToFile(std::wstring message) {
	g_log.out(logStartupTime() + message);
}
void LogToFile(std::wstringstream message) {
	g_log.out(logStartupTime() + message.str());
}


/// Thread function that dispatches queued message blocks to the IA application.
void WorkerThreadMethod() {
    while ((g_hEvent != NULL) && (WaitForSingleObject(g_hEvent,INFINITE) == WAIT_OBJECT_0)) {
        if (g_hEvent == NULL) {
            break;
        }

        DWORD tick = GetTickCount();
        if (tick < g_lastThreadTick) {
            // Overflow
            g_lastThreadTick = tick;
        }

        if ((tick - g_lastThreadTick > 1000) || (g_targetWnd == NULL)) {
            // We either don't have a valid window target OR it has been more than 1 sec since we last update the target.
            g_targetWnd = FindWindow( L"GDMSWindowClass", NULL);
            g_lastThreadTick = GetTickCount();
            LOG(L"FindWindow returned: " << g_targetWnd);
        }

        while (!g_dataQueue.empty()) {
            DataItemPtr item = g_dataQueue.pop();

            if (g_targetWnd == NULL) {
                // We have data, but no target window, so just delete the message
                continue;
            }

            COPYDATASTRUCT data;
            data.dwData = item->type();
            data.lpData = item->data();
            data.cbData = item->size();

            // To avoid blocking the main thread, we should not have a lock on the queue while we process the message.
			SendMessage( g_targetWnd, WM_COPYDATA, 0, ( LPARAM ) &data );
            LOG(L"After SendMessage error code is " << GetLastError());
        }
    }
}

unsigned __stdcall WorkerThreadMethodWrap(void* argss) {
	WorkerThreadMethod();
	return 0;
}

void StartWorkerThread() {
	LOG(L"Starting worker thread..");
	unsigned int pid;
	g_thread = (HANDLE)_beginthreadex(NULL, 0, &WorkerThreadMethodWrap, NULL, 0, &pid);
	

	DataItemPtr item(new DataItem(TYPE_REPORT_WORKER_THREAD_LAUNCHED, 0, NULL));
	g_dataQueue.push(item);
	SetEvent(g_hEvent);


	GAME::GameInfo* gameInfo = fnGetGameInfo(fnGetEngine());
	if (gameInfo != nullptr) {

		std::wstring modName;
		fnGetModNameArg(gameInfo, &modName);
		DataItemPtr item4(new DataItem(TYPE_GameInfo_ModName, modName.size() * sizeof(wchar_t), (char*)modName.c_str()));
		g_dataQueue.push(item4);
		SetEvent(g_hEvent);

		bool isHardcore = fnGetHardcore(gameInfo);
		DataItemPtr item(new DataItem(TYPE_GameInfo_IsHardcore, sizeof(isHardcore), (char*)&isHardcore));
		g_dataQueue.push(item);
		SetEvent(g_hEvent);

	}


	LOG(L"Started worker thread..");
}


void EndWorkerThread() {
	LOG(L"Ending worker thread..");
	if (g_hEvent != NULL) {
		SetEvent(g_hEvent);
		HANDLE h = g_hEvent;
		g_hEvent = NULL;
		CloseHandle(h);

		//WaitForSingleObject(g_thread, INFINITE);
		CloseHandle(g_thread);
	}
}

#pragma endregion



static void ConfigureStashDetectionHooks(std::vector<BaseMethodHook*>& hooks) {
	// Stash detection hooks
	LogToFile(L"Configuring stash detection hooks..");
	hooks.push_back(new SaveTransferStash(&g_dataQueue, g_hEvent));
	hooks.push_back(new SetTransferOpen(&g_dataQueue, g_hEvent));

	LogToFile(L"Configuring hc detection hook..");
	hooks.push_back(new SetHardcore(&g_dataQueue, g_hEvent));

	hooks.push_back(new SetModName(&g_dataQueue, g_hEvent));
}


std::vector<BaseMethodHook*> hooks;
int ProcessAttach(HINSTANCE _hModule) {
	LogToFile(L"Attatching to process..");
	g_hEvent = CreateEvent(NULL,FALSE,FALSE, L"IA_Worker");

	LogToFile(L"Preparing hooks..");
	ConfigureStashDetectionHooks(hooks);

	LogToFile(L"Starting hook enabling.. " + std::to_wstring(hooks.size()) + L" hooks.");
	for (unsigned int i = 0; i < hooks.size(); i++) {
		LOG(L"Enabling hook..");
		hooks[i]->EnableHook();
	}
	LogToFile(L"Hooking complete..");

	
    StartWorkerThread();
	LogToFile(L"Initialization complete..");

	g_log.setInitialized(true);
    return TRUE;
}


#pragma region Attach_Detatch
int ProcessDetach( HINSTANCE _hModule ) {
	// Signal that we are shutting down
	// This message is not at all guaranteed to get sent.

	LOG(L"Detatching DLL..");
	OutputDebugString(L"ProcessDetach");


	for (unsigned int i = 0; i < hooks.size(); i++) {
		hooks[i]->DisableHook();
		delete hooks[i];
	}
	hooks.clear();

    EndWorkerThread();

	LOG(L"DLL detatched..");
    return TRUE;
}

void Dump_ItemStats();
BOOL APIENTRY DllMain(HINSTANCE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
	case DLL_PROCESS_ATTACH:
        return ProcessAttach( hModule );

	case DLL_PROCESS_DETACH:
        return ProcessDetach( hModule );
	}
    return TRUE;
}
#pragma endregion


