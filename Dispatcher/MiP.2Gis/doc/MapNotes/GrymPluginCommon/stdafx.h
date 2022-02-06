// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once


#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#define NOMINMAX
#define BOOST_USE_WINDOWS_H

#include <Windows.h>
#include <shlobj.h>

#include <comdef.h>

#include <atldef.h>
#include <atlstr.h>

#include <exception>
#include <string>
#include <sstream>
#include <vector>

#define UNI_VERIFY ATLVERIFY
#define UNI_ASSERT ATLASSERT

// Grym.exe
#import "libid:7AA02C95-0B4A-43aa-92D8-BA40511A7F3F" 
