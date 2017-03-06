#pragma once

#include "threads.h"

class Thread
{
	size_t idx;
	THREAD thread;
	Mutex mutex;
	Condition condition;
	void * data;
	thread_fnc work_fnc;
	THREAD_HANDLE h;

public:
	Thread(int id) : idx(id), work_fnc(0), h(0) { };
	Thread(int id, thread_fnc tf) : idx(id), work_fnc(tf), h(0) { };
	Thread(int id, thread_fnc tf, void * dta) : idx(id), work_fnc(tf), data(dta), h(0) { };
	~Thread() { if (work_fnc) { work_fnc = NULL; } };
	
	void start(void * dat) { h = thread_create(thread, work_fnc, dat); }
	void start() { h = thread_create(thread, work_fnc, data); }
	//void join() { if (h) thread_join(&h); }
	int id() { return idx; }
};



