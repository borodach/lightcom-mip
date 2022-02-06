#include "StdAfx.h"
#include "resource.h"
#include ".\cmdgotonotebase.h"
#include "MapNoteShell.h"

static const _bstr_t cmd_goto_note_group_name__(OLESTR("L200GoToNote"));
static const _bstr_t cmd_goto_note_image_name__(OLESTR("Plugin.MapNotes.Cmd.GoToNote"));

CmdGoToNoteBase::CmdGoToNoteBase(void) :
	inherited(cmd_goto_note_group_name__, 0, 0, cmd_goto_note_image_name__, IDS_TITLE_CMD_GOTONOTE)
{
}

CmdGoToNoteBase::~CmdGoToNoteBase(void)
{
}

STDMETHODIMP CmdGoToNoteBase::get_Position(long *pVal)
{
	position_ = note_->GetPosInfo();
	return inherited::get_Position(pVal);
}

STDMETHODIMP CmdGoToNoteBase::get_Title(BSTR *pVal)
{
	try
	{
		if (!pVal)
		{
			return E_POINTER;
		}
		*pVal = 0;
		
		tstd::tstring text = GPCommon::HTMLUnescape(note_->GetText());
		size_t nlpos = text.find(_T('\n'));
		bool ellipses = false;
		if (nlpos != tstd::tstring::npos)
		{
			text.resize(nlpos);
			ellipses = true;
		}
		if (text.length() > 60)
		{
			text.resize(60);
			ellipses = true;
		}
		tstd::tstring title = GPCommon::LoadResourceString(IDS_TITLE_CMD_GOTONOTE) + text;
		if (ellipses)
		{
			title += _T("...");
		}
		*pVal = _bstr_t(title.c_str()).Detach();
		return S_OK;
	} catch(...) {}
	return E_FAIL;
}

STDMETHODIMP CmdGoToNoteBase::raw_OnCommand(GrymCore::IContextBase *pContext)
{
	try
	{
		ATLASSERT(note_.get());
		if (PluginDataPtr my_data = my_data_weak_.lock())
		{
			if (GrymCore::IMapPtr map = my_data->get_base_view()->GetMap())
			{
				my_data->RestoreMapNotes();
				my_data->bring_to_front(note_, true);
				map->SetMapVisibleRect(note_->GetMapBounds());
				if (GrymCore::IDevicePtr map_device = map)
				{
					if (GrymCore::IDevPointPtr dev_pos = map_device->MapToDevice(note_->GetAnchor()))
					{
						POINT pt;
						pt.x = dev_pos->GetX();
						pt.y = dev_pos->GetY();
						if (::ClientToScreen(HWND(map->GetHWindow()), &pt))
						{
							::SetCursorPos(pt.x, pt.y);
							return S_OK;
						}
					}
				}
			}
		}
		return S_FALSE;
	} catch(...){}
	return E_FAIL;
}
