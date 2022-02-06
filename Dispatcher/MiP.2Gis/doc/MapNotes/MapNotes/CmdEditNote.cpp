#include "StdAfx.h"
#include ".\cmdeditnote.h"
#include "resource.h"
#include "CommentEdit.h"
#include "MapNoteShell.h"
#include "ContextInfo.h"

GrymCore::ICommandActionPtr CmdEditNote::CreateInstance(const PluginDataPtr &my_data)
{
	ATL::CComObject<CmdEditNote> *obj;
	ATLVERIFY(S_OK == ATL::CComObject<CmdEditNote>::CreateInstance(&obj));
	GrymCore::ICommandActionPtr rv = obj;
	ATLASSERT(NULL != rv);
	
	obj->my_data_weak_ = my_data;
	
	return rv;
}

static const _bstr_t cmd_edit_note_group_name__(OLESTR("L100EditNote"));
static const _bstr_t cmd_edit_note_image_name__(OLESTR("Plugin.MapNotes.Cmd.EditNote"));

CmdEditNote::CmdEditNote(void) :
	GPCommon::VisibleStateCommandBase(cmd_edit_note_group_name__, 1000, 50, cmd_edit_note_image_name__, IDS_TITLE_CMD_EDITNOTE)
{
}

CmdEditNote::~CmdEditNote(void)
{
}

STDMETHODIMP CmdEditNote::get_Available(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
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
				if (info->GetPosNote())
				{
					*pVal = VARIANT_TRUE;
				}
			}
		}
		return S_OK;
	}catch(...){}
	return E_FAIL;
}

STDMETHODIMP CmdEditNote::raw_OnCommand(GrymCore::IContextBase *pContext)
{
	try
	{
		if (PluginDataPtr my_data = my_data_weak_.lock())
		{
			if (IMapNoteContextInfoPtr info = ContextInfo::GetInfo(my_data, pContext))
			{
				if (MapNoteShellPtr note = info->GetPosNote())
				{
					tstd::tstring orig = note->GetText();

					tstd::tstring result = GPCommon::HTMLMakeEscaped(
						CommentEdit::Execute(GPCommon::LoadResourceString(IDS_TITLE_DLG_EDITNOTE),
						GPCommon::HTMLUnescape(orig)));
					if (result != orig)
					{
						if (result.empty())
						{
							my_data->note_remove(note);
						}
						else
						{
							note->UpdateText(result);
						}
					}
				}
			}
		}
		return S_OK;
	} catch(...){}
	return E_FAIL;
}
