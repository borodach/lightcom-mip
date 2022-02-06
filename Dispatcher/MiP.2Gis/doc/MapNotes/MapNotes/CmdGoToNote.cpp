#include "StdAfx.h"
#include ".\cmdgotonote.h"
#include "CmdGoToNoteBase.inl"
#include "MapNoteShell.h"
#include "ContextInfo.h"

GrymCore::ICommandActionPtr CmdGoToNote::CreateInstance(const PluginDataPtr &my_data, const MapNoteShellPtr &note)
{
	return inherited::CreateInstance<CmdGoToNote>(my_data, note);
}

STDMETHODIMP CmdGoToNote::get_Available(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
{
	try
	{
		if (!pVal)
		{
			return E_POINTER;
		}
		*pVal = VARIANT_FALSE;
		if (!note_->IsClosedByUser())
		{
			if (PluginDataPtr my_data = my_data_weak_.lock())
			{
				if (IMapNoteContextInfoPtr info = ContextInfo::GetInfo(my_data, pContext))
				{
					if (info->GetPosNote() != note_)
					{
						*pVal = VARIANT_TRUE;
					}
				}
			}
		}
		return S_OK;
	} catch(...){}
	return E_FAIL;
}
