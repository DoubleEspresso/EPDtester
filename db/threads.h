#pragma once

#ifndef DB_THREADS_H
#define DB_THREADS_H

#ifdef _WIN32
#include <io.h>
#else
#include <unistd.h>
#endif

#ifdef __APPLE__
#include <pthread.h>   
typedef pthread_mutex_t MUTEX;
typedef pthread_t THREAD;
typedef pthread_cond_t CONDITION;
typedef int THREAD_HANDLE;
typedef void*(*thread_fnc)(void*);

#define thread_lock(x)   pthread_mutex_lock(&(x))
#define thread_unlock(x) pthread_mutex_unlock(&(x))
#define thread_create(x, f, t) pthread_create(&(x), NULL, (thread_fnc)f, t)
#define thread_join(x)  pthread_join(x,NULL)
#define thread_signal(x) pthread_cond_signal(&(x))
#define thread_broadcast(x)  pthread_cond_broadcast(&(x))
#define thread_wait(x) pthread_cond_wait(&(x))
#define thread_sleep(x) sleep(x)

#elif __linux
extern "C" {
#include <pthread.h>
#include <unistd.h>
}
typedef pthread_mutex_t MUTEX;
typedef pthread_t THREAD;
typedef int THREAD_HANDLE;
typedef pthread_cond_t CONDITION;
typedef void*(*thread_fnc)(void*);

#define mutex_init(x)   pthread_mutex_init(&(x), NULL);
#define mutex_lock(x)   pthread_mutex_lock(&(x))
#define mutex_unlock(x) pthread_mutex_unlock(&(x))
#define thread_create(x, f, t) pthread_create(&(x), NULL, (thread_fnc)f, t)
#define thread_join(x)  pthread_join(x,NULL)
#define thread_signal(x) pthread_cond_signal(&(x))
#define thread_cond_init(x) pthread_cond_init(&(x),NULL)
#define thread_broadcast(x)  pthread_cond_broadcast(&(x))
#define thread_wait(x,y) pthread_cond_wait(&(x),&(y))
#define thread_timed_wait(x,y,z) pthread_cond_timedwait(&(x),&(y),&(z))
#define thread_sleep(x) usleep(x)
#define cond_destroy(x) pthread_cond_destroy(&(x))

#elif __unix 
#elif __posix
#elif _WIN32

#include <windows.h>   
typedef CRITICAL_SECTION MUTEX;
typedef HANDLE CONDITION;
typedef HANDLE THREAD_HANDLE;
typedef DWORD THREAD;

#define mutex_init(x)   InitializeCriticalSection(&(x));
#define mutex_lock(x)   EnterCriticalSection(&(x))
#define mutex_unlock(x) LeaveCriticalSection(&(x))
#define thread_cond_init(x) create_event(x);
#define thread_wait(x,y) cond_wait (&(x),&(y))
#define thread_timed_wait(x,y,z) timed_wait(&(x),&(y),z)
#define thread_signal(x) SetEvent(x)
#define cond_destroy(x) CloseHandle(x)
#define thread_create(x, f, arg) CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)f, arg, 0, &(x))
#define thread_sleep(x) Sleep(x)
typedef void*(*thread_fnc)(void*);

namespace
{
	void create_event(HANDLE& h) { h = CreateEvent(NULL, FALSE, FALSE, NULL); }
	void cond_wait(HANDLE * h, CRITICAL_SECTION * external_mutex)
	{
		// Release the <external_mutex> here and wait for either event
		// to become signaled, due to <pthread_cond_signal> being
		// called or <pthread_cond_broadcast> being called.
		LeaveCriticalSection(external_mutex);
		WaitForMultipleObjects(1, h, FALSE, INFINITE);

		// Reacquire the mutex before returning.
		EnterCriticalSection(external_mutex);
	}
	void timed_wait(HANDLE *h, CRITICAL_SECTION * external_mutex, int time_ms)
	{
		LeaveCriticalSection(external_mutex);
		WaitForMultipleObjects(1, h, FALSE, (DWORD)time_ms);
		EnterCriticalSection(external_mutex);
	}
	void wait_thread_finish(HANDLE *h)
	{
		WaitForSingleObject(h, INFINITE);
		CloseHandle(h);
	}
	THREAD_HANDLE start_thread(thread_fnc f, void * data)
	{
		unsigned long threadid = 0;
		return thread_create(threadid, f, data);
	}
	void wait_threads_finish(THREAD_HANDLE * handles, int nb)
	{
		WaitForMultipleObjects(nb, handles, true, INFINITE);
		for (int j = 0; j < nb; ++j)
		{
			CloseHandle(handles[j]);
		}
	}
}


#endif

class Mutex
{
	friend struct Condition;
	MUTEX m;
public:
	Mutex() { mutex_init(m); };
	~Mutex() {};

	void lock() { mutex_lock(m); }
	void unlock() { mutex_unlock(m); }
	MUTEX mutex() { return m; }
};


struct Condition
{
	CONDITION c;
public:
	Condition() { thread_cond_init(c); }
	~Condition() { cond_destroy(c); }

	void wait(Mutex& m) { thread_wait(c, m.mutex()); }
#ifdef __linux
	void wait_for(NativeMutex& m, timespec& d) { thread_timed_wait(c, m.mutex, d); }
#else
	void wait_for(Mutex& m, int ms) { thread_timed_wait(c, m.mutex(), ms); }
#endif
	void notify_one() { thread_signal(c); }
};



#endif


