// MapNotes.cpp : Implementation of DLL Exports.

#include "stdafx.h"
#include "resource.h"
#include "MapNotesPlugin.h"

using ATL::_ATL_REGMAP_ENTRY;

class CMapNotesModule : public ATL::CAtlDllModuleT< CMapNotesModule >
{
public :
	DECLARE_LIBID(MapNotesLib::LIBID_MapNotesLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_MAPNOTES, "{E5C09B96-69E5-495F-88D9-0BC6D37386C4}")
};

CMapNotesModule _AtlModule;


// DLL Entry Point
extern "C" BOOL WINAPI DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	hInstance;
    return _AtlModule.DllMain(dwReason, lpReserved); 
}


// Used to determine whether the DLL can be unloaded by OLE
STDAPI DllCanUnloadNow(void)
{
    return _AtlModule.DllCanUnloadNow();
}


// Returns a class factory to create an object of the requested type
STDAPI DllGetClassObject(REFCLSID rclsid, REFIID riid, LPVOID* ppv)
{
    return _AtlModule.DllGetClassObject(rclsid, riid, ppv);
}


// DllRegisterServer - Adds entries to the system registry
STDAPI DllRegisterServer(void)
{
    // registers object, typelib and all interfaces in typelib
    HRESULT hr = _AtlModule.DllRegisterServer();
	return hr;
}


// DllUnregisterServer - Removes entries from the system registry
STDAPI DllUnregisterServer(void)
{
	HRESULT hr = _AtlModule.DllUnregisterServer();
	return hr;
}

// главная точка входа в плагин в случае если не используем ригистрацию в категории
extern "C" int __stdcall CreateGrymPlugin( IUnknown **pPlugin )
{
	GrymCore::IGrymPluginPtr plugin = CMapNotesPlugin::CreateInstance();
	if ( plugin == 0 )
	{
		return 1;
	}
	*pPlugin = IUnknownPtr(plugin).Detach();
	return 0;
}
