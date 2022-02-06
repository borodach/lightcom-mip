#include "StdAfx.h"
#include ".\cmdrestorenotes.h"
#include "resource.h"

GrymCore::ICommandActionPtr CmdRestoreNotes::CreateInstance(const PluginDataPtr &my_data)
{
	ATL::CComObject<CmdRestoreNotes> *obj;
	ATLVERIFY(S_OK == ATL::CComObject<CmdRestoreNotes>::CreateInstance(&obj));
	GrymCore::ICommandActionPtr rv = obj;
	ATLASSERT(NULL != rv);
	
	obj->my_data_weak_ = my_data;
	
	return rv;
}

static const _bstr_t cmd_restore_notes_group_name__(OLESTR("L025RestoreNotes"));
static const _bstr_t cmd_restore_notes_image_name__(OLESTR("Plugin.MapNotes.Cmd.RestoreNotes"));

CmdRestoreNotes::CmdRestoreNotes(void) :
	GPCommon::VisibleStateCommandBase(cmd_restore_notes_group_name__, 1000, 50, cmd_restore_notes_image_name__, IDS_TITLE_CMD_RESTORENOTES)
{
}

CmdRestoreNotes::~CmdRestoreNotes(void)
{
}

STDMETHODIMP CmdRestoreNotes::get_Available(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
{
	try
	{
		if (!pVal)
		{
			return E_POINTER;
		}
		*pVal = VARIANT_FALSE;
		if (PluginDataPtr my_data = my_data_weak_.lock())
		{
			if (my_data->HaveHiddenNotes())
			{
				*pVal = VARIANT_TRUE;
			}
		}
		return S_OK;
	}catch(...){}
	return E_FAIL;
}

STDMETHODIMP CmdRestoreNotes::raw_OnCommand(GrymCore::IContextBase *pContext)
{
	try
	{
		if (PluginDataPtr my_data = my_data_weak_.lock())
		{
			my_data->RestoreMapNotes();
		}
		return S_OK;
	} catch(...){}
	return E_FAIL;
}
