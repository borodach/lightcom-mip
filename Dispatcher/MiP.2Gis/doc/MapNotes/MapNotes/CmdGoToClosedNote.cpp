#include "StdAfx.h"
#include ".\cmdgotoclosednote.h"
#include "CmdGoToNoteBase.inl"
#include "MapNoteShell.h"

GrymCore::ICommandActionPtr CmdGoToClosedNote::CreateInstance(const PluginDataPtr &my_data, const MapNoteShellPtr &note)
{
	return inherited::CreateInstance<CmdGoToClosedNote>(my_data, note);
}

STDMETHODIMP CmdGoToClosedNote::get_Available(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
{
	try
	{
		if (!pVal)
		{
			return E_POINTER;
		}
		*pVal = note_->IsClosedByUser() ? VARIANT_TRUE : VARIANT_FALSE;
		return S_OK;
	} catch(...){}
	return E_FAIL;
}

STDMETHODIMP CmdGoToClosedNote::raw_OnCommand(GrymCore::IContextBase *pContext)
{
	try
	{
		note_->Show();
		return inherited::raw_OnCommand(pContext);
	}catch(...){}
	return E_FAIL;
}
