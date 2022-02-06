#include "StdAfx.h"
#include ".\contextinfo.h"
#include "PluginData.h"

IMapNoteContextInfoPtr ContextInfo::GetInfo(const PluginDataPtr &my_data, GrymCore::IContextBase *pContext)
{
	static const _bstr_t context_key__(OLESTR("0EA8CD261D354557BA17591F85E032E2"));

	ATLASSERT(my_data.get());
	if (pContext)
	{
		IMapNoteContextInfoPtr rv;
		try
		{
			rv = pContext->GetCustomInfo(context_key__);
		}catch(...){}
	
		if (NULL == rv)
		{
			if (GrymCore::IPopupMenuMapViewContextPtr pm_context = pContext)
			{
				ATL::CComObject<ContextInfo> *obj;
				ATLVERIFY(S_OK == ATL::CComObject<ContextInfo>::CreateInstance(&obj));
				rv = obj;
				ATLASSERT(NULL != rv);
				
				obj->note_in_pos_ = my_data->note_by_position(pm_context->GetMapPos());
				
				pContext->PutCustomInfo(context_key__, _variant_t(rv, true));
			}
		}
		return rv;
	}
	return IMapNoteContextInfoPtr();
}

ContextInfo::ContextInfo(void)
{
}

ContextInfo::~ContextInfo(void)
{
}

MapNoteShellPtr ContextInfo::GetPosNote()
{
	return note_in_pos_;
}
