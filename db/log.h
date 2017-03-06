#pragma once
#ifndef UTILS_LOG_H
#define UTILS_LOG_H

#include <iostream>
#include <fstream>
#include <stdio.h>

#ifdef _WIN32
	#include "Shlobj.h"
#endif

#include "threads.h"
#include "types.h"
#include "string_utils.h"

namespace Log
{
	bool write(const char * msg, ...);
	void init(TCHAR * app_folder);
	void close();
};

#endif