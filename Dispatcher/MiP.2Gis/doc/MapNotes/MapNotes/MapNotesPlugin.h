// MapNotesPlugin.h : Declaration of the CMapNotesPlugin

#pragma once
#include "resource.h"       // main symbols
#include "PluginData.h"

// CMapNotesPlugin

class ATL_NO_VTABLE CMapNotesPlugin : 
	public ATL::CComObjectRootEx<ATL::CComSingleThreadModel>,
	public ATL::CComCoClass<CMapNotesPlugin, &MapNotesLib::CLSID_MapNotesPlugin>,
	public GrymCore::IGrymPlugin,
  public GrymCore::IGrymPluginInfo
{
public:
	static GrymCore::IGrymPluginPtr CreateInstance();
	DECLARE_REGISTRY_RESOURCEID(IDR_MAPNOTESPLUGIN)
protected:
	CMapNotesPlugin();

	BEGIN_COM_MAP(CMapNotesPlugin)
		COM_INTERFACE_ENTRY(GrymCore::IGrymPlugin)
		COM_INTERFACE_ENTRY(GrymCore::IGrymPluginInfo)
	END_COM_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()
	HRESULT FinalConstruct();
	void FinalRelease();
public: // GrymCore::IGrymPlugin
	STDMETHOD(raw_Initialize)(GrymCore::IGrym *pRoot, GrymCore::IBaseViewThread *pBaseView);
	STDMETHOD(raw_Terminate)();
public: // GrymCore::IGrymPluginInfo
  STDMETHOD(get_Name)(BSTR *pName);
	STDMETHOD(get_Description)(BSTR *pDesc);
  STDMETHOD(get_Copyright)(BSTR *pVal);
  STDMETHOD(get_Icon)(OLE_HANDLE *pVal);
private:
	PluginDataPtr data_;
};

OBJECT_ENTRY_AUTO(__uuidof(MapNotesLib::MapNotesPlugin), CMapNotesPlugin)
