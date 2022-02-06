// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#define NOMINMAX

#ifndef STRICT
#define STRICT
#endif

// Modify the following defines if you have to target a platform prior to the ones specified below.
// Refer to MSDN for the latest info on corresponding values for different platforms.
#ifndef WINVER				// Allow use of features specific to Windows 95 and Windows NT 4 or later.
#define WINVER 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif

#ifndef _WIN32_WINNT		// Allow use of features specific to Windows NT 4 or later.
#define _WIN32_WINNT 0x0400	// Change this to the appropriate value to target Windows 2000 or later.
#endif						

#ifndef _WIN32_WINDOWS		// Allow use of features specific to Windows 98 or later.
#define _WIN32_WINDOWS 0x0410 // Change this to the appropriate value to target Windows Me or later.
#endif

#ifndef _WIN32_IE			// Allow use of features specific to IE 4.0 or later.
#define _WIN32_IE 0x0400	// Change this to the appropriate value to target IE 5.0 or later.
#endif

#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE
#define _WTL_NO_AUTOMATIC_NAMESPACE
#define _WTL_NO_CSTRING

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

// turns off ATL's hiding of some common and often safely ignored warning messages
#define _ATL_ALL_WARNINGS

#define BOOST_USE_WINDOWS_H

#include <Windows.h>

#include <atlbase.h>
#include <atlcom.h>
#include <atlstr.h>
#include <atlwin.h>

#include <algorithm>
#include <deque>
#include <vector>
using std::min;
using std::max;

#include <boost/smart_ptr.hpp>
#include <boost/bind.hpp>

// Grym.exe
#import "libid:7AA02C95-0B4A-43aa-92D8-BA40511A7F3F" 

#include "GrymPluginCommon/CommandBase.h"
#include "GrymPluginCommon/Encoding.h"
#include "GrymPluginCommon/PropertySet.h"
#include "GrymPluginCommon/tstd.h"
#include "GrymPluginCommon/Util.h"

namespace MapNotesLib
{
	#include "MapNotes.h"
}
