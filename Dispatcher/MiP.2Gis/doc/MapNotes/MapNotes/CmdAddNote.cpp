#include "StdAfx.h"
#include ".\cmdaddnote.h"
#include "resource.h"
#include "CommentEdit.h"
#include "MapNoteShell.h"
#include "ContextInfo.h"

GrymCore::ICommandActionPtr CmdAddNote::CreateInstance(const PluginDataPtr &my_data)
{
	ATL::CComObject<CmdAddNote> *obj;
	ATLVERIFY(S_OK == ATL::CComObject<CmdAddNote>::CreateInstance(&obj));
	GrymCore::ICommandActionPtr rv = obj;
	ATLASSERT(NULL != rv);
	
	obj->my_data_weak_ = my_data;
	
	return rv;
}

static const _bstr_t cmd_add_note_group_name__(OLESTR("L100AddNote"));
static const _bstr_t cmd_add_note_image_name__(OLESTR("Plugin.MapNotes.Cmd.AddNote"));

CmdAddNote::CmdAddNote(void) :
	GPCommon::VisibleStateCommandBase(cmd_add_note_group_name__, 1000, 50, cmd_add_note_image_name__, IDS_TITLE_CMD_ADDNOTE)
{
}

CmdAddNote::~CmdAddNote(void)
{
}

STDMETHODIMP CmdAddNote::get_Available(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
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
			if (IMapNoteContextInfoPtr info = ContextInfo::GetInfo(my_data, pContext))
			{
				if (!info->GetPosNote())
				{
					*pVal = VARIANT_TRUE;
				}
			}
		}
		return S_OK;
	}catch(...){}
	return E_FAIL;
}

STDMETHODIMP CmdAddNote::raw_OnCommand(GrymCore::IContextBase *pContext)
{
	try
	{
		if (GrymCore::IPopupMenuMapViewContextPtr pm_context = pContext)
		{
			if (PluginDataPtr my_data = my_data_weak_.lock())
			{
				tstd::tstring text = GPCommon::HTMLMakeEscaped(CommentEdit::Execute(GPCommon::LoadResourceString(IDS_TITLE_DLG_ADDNOTE), tstd::tstring()));
				if (!text.empty())
				{
					my_data->RestoreMapNotes();
					
					MapNoteShellPtr note(new MapNoteShell(my_data, my_data->get_base_view()->GetMap()->GetMapVisibleRect(), pm_context->GetMapPos(), text));
					my_data->note_add(my_data, note);
					note->Show();
				}
			}
		}
		return S_OK;
	} catch(...){}
	return E_FAIL;
}
