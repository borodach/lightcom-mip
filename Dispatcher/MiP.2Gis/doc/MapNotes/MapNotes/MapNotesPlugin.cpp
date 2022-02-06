// MapNotesPlugin.cpp : Implementation of CMapNotesPlugin

#include "stdafx.h"
#include "MapNotesPlugin.h"
#include "PluginData.h"

// CMapNotesPlugin

GrymCore::IGrymPluginPtr CMapNotesPlugin::CreateInstance()
{
	ATL::CComObject<CMapNotesPlugin> *obj;
	ATLVERIFY(S_OK == ATL::CComObject<CMapNotesPlugin>::CreateInstance(&obj));
	return obj;
}

CMapNotesPlugin::CMapNotesPlugin()
{
}

HRESULT CMapNotesPlugin::FinalConstruct()
{
	return S_OK;
}

void CMapNotesPlugin::FinalRelease()
{ // даже если уже все закрыто, исключение не вылетит, только код возврата
	raw_Terminate();
}

STDMETHODIMP CMapNotesPlugin::raw_Initialize(GrymCore::IGrym *pRoot, GrymCore::IBaseViewThread *pBaseView)
{
	try
	{
		if (!pBaseView)
		{
			return E_INVALIDARG;
		}
		if (data_.get())
		{
			return E_FAIL;
		}
		data_.reset(new PluginData(pBaseView));
		HRESULT rv = data_->Init(data_);
		if (S_OK != rv)
		{
			data_.reset();
		}
		return rv;
	}
	catch(...)
	{
		return E_FAIL;
	}
}

STDMETHODIMP CMapNotesPlugin::raw_Terminate()
{
	try
	{
		if (!data_.get())
		{
			return E_FAIL;
		}
		data_->Terminate();
		data_.reset();
	} catch(...){}
	return S_OK;
}

STDMETHODIMP CMapNotesPlugin::get_Name(BSTR *pName)
{
	try
	{
		if (!pName)
		{
			return E_POINTER;
		}
		*pName = 0;
		ATL::CStringW val;
		val.LoadString(IDS_PLUGIN_NAME);
		*pName = val.AllocSysString();
		return S_OK;
	}catch(...){}
	return E_FAIL;
}

STDMETHODIMP CMapNotesPlugin::get_Description(BSTR *pDesc)
{
	try
	{
		if (!pDesc)
		{
			return E_POINTER;
		}
		*pDesc = 0;
		ATL::CStringW val;
		val.LoadString(IDS_PLUGIN_DESC);
		*pDesc = val.AllocSysString();
		return S_OK;
	}catch(...){}
	return E_FAIL;
}

STDMETHODIMP CMapNotesPlugin::get_Copyright(BSTR *pVal)
{
	try
	{
		if (!pVal)
		{
			return E_POINTER;
		}
		*pVal = 0;
		ATL::CStringW val;
		val.LoadString(IDS_PLUGIN_COPY);
		*pVal = val.AllocSysString();
		return S_OK;
	}catch(...){}
	return E_FAIL;
}

STDMETHODIMP CMapNotesPlugin::get_Icon(OLE_HANDLE *pVal)
{
	try
	{
		if (!pVal)
		{
			return E_POINTER;
		}
		*pVal = 0;
		static HICON icon = ::LoadIcon(ATL::_AtlBaseModule.GetModuleInstance(),MAKEINTRESOURCE(IDI_PLUGIN));
		*pVal = *reinterpret_cast<OLE_HANDLE *>(&icon);
		return (*pVal) ? S_OK : S_FALSE;
	}catch(...){}
	return E_FAIL;
}
