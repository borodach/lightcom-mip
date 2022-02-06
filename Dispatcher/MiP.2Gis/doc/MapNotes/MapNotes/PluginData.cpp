#include "StdAfx.h"
#include ".\plugindata.h"
#include "resource.h"
#include "CmdAddNote.h"
#include "CmdEditNote.h"
#include "CmdRestoreNotes.h"
#include "CmdGoToNote.h"
#include "CmdGoToClosedNote.h"
#include "MapNoteShell.h"

PluginData::PluginData(GrymCore::IBaseViewThread *base_view) :
	base_view_(base_view)
{
	ATLASSERT(NULL != base_view_);
}

PluginData::~PluginData(void)
{
}

HRESULT PluginData::Init(const PluginDataPtr &this_ptr)
{
	HRESULT rv = add_mapcontextmenu_commands(this_ptr);
	if (S_OK == rv)
	{
		load(this_ptr);
	}
	return rv;
}

void PluginData::Terminate()
{
	save();
	my_submenu_ = NULL;
	bmp_save_ = NULL;
	base_view_ = NULL;
}

GrymCore::IBaseViewThread *PluginData::get_base_view()
{
	ATLASSERT(NULL != base_view_);
	return base_view_;
}

GrymCore::IBitmap *PluginData::get_bmp_save()
{
	if ((NULL == bmp_save_) && (NULL != base_view_))
	{
		if (HBITMAP bmp = ::LoadBitmap(ATL::_AtlBaseModule.GetResourceInstance(), MAKEINTRESOURCE(IDB_SAVE)))
		{
			try
			{
				// передаем загруженную картинку под контроль создаваемому объекту-оболочке
				bmp_save_ = base_view_->GetFactory()->CreateBitmap(*reinterpret_cast<OLE_HANDLE *>(&bmp), VARIANT_FALSE);
			}
			catch(...)
			{
				::DeleteObject(bmp);
			}
		}
	}
	return bmp_save_;
}

HRESULT PluginData::add_mapcontextmenu_commands(const PluginDataPtr &this_ptr)
{
	ATLASSERT(NULL != base_view_);
	ATLASSERT(this == this_ptr.get());
	if (GrymCore::IPopupMenuPtr map_context_menu = base_view_->GetMap()->GetContextMenu())
	{
		my_submenu_ = map_context_menu->AddSubMenu(OLESTR("L075MapUserNotes"), 500, GPCommon::LoadResourceBSTR(IDS_TITLE_CMGROUP));
		if (NULL != my_submenu_)
		{
			my_submenu_->AddCommand(CmdAddNote::CreateInstance(this_ptr));
			my_submenu_->AddCommand(CmdEditNote::CreateInstance(this_ptr));
			my_submenu_->AddCommand(CmdRestoreNotes::CreateInstance(this_ptr));
			closed_notes_submenu_ = my_submenu_->AddSubMenu(OLESTR("R500ClosedNotes"), 500, GPCommon::LoadResourceBSTR(IDS_TITLE_GRP_CLOSEDNOTES));
			if (NULL != closed_notes_submenu_)
			{
				return S_OK;
			}
		}
	}
	return E_FAIL;
}

void PluginData::load_item(const tstd::tstring &serialized, const PluginDataPtr &this_ptr)
{
	try
	{
		MapNoteShellPtr note(new MapNoteShell(this_ptr, serialized));
		note_add(this_ptr, note);
		note->Show();
	}catch(...){}
}

void PluginData::load(const PluginDataPtr &this_ptr)
{
	ATLASSERT(NULL != base_view_);
	ATLASSERT(this == this_ptr.get());
	if (NULL != base_view_)
	{
		if (GrymCore::IBaseReferencePtr base_ref = base_view_->GetBaseReference())
		{
			USES_CONVERSION;
			tstd::tstring filename(_T("MapNotes\\"));
			filename += OLE2CT(base_ref->GetName());
			filename += _T(".mns");
			datapath_ = GPCommon::GetUserDataPath(filename);
			if (!datapath_.empty())
			{
				try // игнорируем отсутствие данных
				{
					GPCommon::PropertySet data;
					data.LoadFromFile(datapath_);
					data.ForEachProperty(boost::bind(&PluginData::load_item, this, _2, this_ptr));
				}catch(...){}
			}
			
		}
	}
}

void PluginData::save()
{
	if (!datapath_.empty())
	{
		try
		{
			GPCommon::PropertySet data;

			int gen = 0;
			for (mapnote_vector::iterator it = notes_.begin(), end = notes_.end(); it != end; ++it)
			{
				if (it->shell_->IsNoteToSave())
				{
					char buf[20];
					sprintf(buf, "%05ld", ++gen);
					data.SetValueString(buf, it->shell_->Serialize());
				}
			}

			data.SaveToFile(datapath_);
		}catch(...){}
	}
}

bool PluginData::HaveHiddenNotes()
{
	for (mapnote_vector::iterator it = notes_.begin(), end = notes_.end(); it != end; ++it)
	{
		if (it->shell_->IsNoteHidden())
		{
			return true;
		}
	}
	return false;
}

void PluginData::RestoreMapNotes()
{
	for (mapnote_vector::iterator it = notes_.begin(), end = notes_.end(); it != end; ++it)
	{
		if (it->shell_->IsNoteHidden())
		{
			it->shell_->Show();
		}
	}
}

void PluginData::note_add(const PluginDataPtr &this_ptr, const MapNoteShellPtr &note)
{
	NoteItem item;
	item.shell_ = note;
	item.cmd_goto_ = CmdGoToNote::CreateInstance(this_ptr, note);
	item.cmd_gotoclosed_ = CmdGoToClosedNote::CreateInstance(this_ptr, note);
	notes_.push_front(item);
	update_notes_pos_info();
	ATLASSERT(NULL != my_submenu_);
	my_submenu_->AddCommand(item.cmd_goto_);
	ATLASSERT(NULL != closed_notes_submenu_);
	closed_notes_submenu_->AddCommand(item.cmd_gotoclosed_);
}

void PluginData::note_remove(const MapNoteShellPtr &note)
{
	ATLASSERT(note.get());
	ATLASSERT(NULL != note->GetCallout());
	ATLASSERT((note->GetPosInfo() >= 0) && (note->GetPosInfo() < int(notes_.size())) && (note == notes_[note->GetPosInfo()].shell_));
	if (GrymCore::IMapGraphicsPtr map_graphics = get_base_view()->GetMap())
	{
		NoteItem item = notes_[note->GetPosInfo()];
		map_graphics->RemoveGraphic(note->GetCallout());

		ATLASSERT(NULL != my_submenu_);
		my_submenu_->RemoveCommand(item.cmd_goto_);
		ATLASSERT(NULL != closed_notes_submenu_);
		closed_notes_submenu_->RemoveCommand(item.cmd_gotoclosed_);
		get_base_view()->GetMap()->Invalidate(VARIANT_FALSE);
		
		notes_.erase(notes_.begin() + item.shell_->GetPosInfo());
		update_notes_pos_info();
	}
}

void PluginData::bring_to_front(const MapNoteShellPtr &note, bool redraw)
{
	ATLASSERT(note.get());
	ATLASSERT(NULL != note->GetCallout());
	ATLASSERT((note->GetPosInfo() >= 0) && (note->GetPosInfo() < int(notes_.size())) && (note == notes_[note->GetPosInfo()].shell_));
	if (GrymCore::IMapGraphicsPtr map_graphics = get_base_view()->GetMap())
	{
		map_graphics->BringToFront(note->GetCallout(), redraw ? VARIANT_TRUE : VARIANT_FALSE);
		if (note->GetPosInfo() > 0) // не на самом верху - будем двигать
		{
			std::rotate(notes_.begin(), notes_.begin() + note->GetPosInfo(), notes_.begin() + note->GetPosInfo() + 1);
			update_notes_pos_info();
		}
	}
}

void PluginData::update_notes_pos_info()
{
	int i = -1;
	for (mapnote_vector::iterator it = notes_.begin(), end = notes_.end(); it != end; ++it)
	{
		it->shell_->SetPosInfo(++i);
	}
}

MapNoteShellPtr PluginData::note_by_position(GrymCore::IMapPoint *position)
{
	try
	{
		if (GrymCore::IMapGraphicsPtr map_graphics = get_base_view()->GetMap())
		{
			if (GrymCore::ICalloutPtr callout = map_graphics->GetGraphicByPosition(position))
			{
				for (mapnote_vector::iterator it = notes_.begin(), end = notes_.end(); it != end; ++it)
				{
					if (it->shell_->GetCallout() == callout)
					{
						return it->shell_;
					}
				}
			}
		}
	}catch(...){}
	return MapNoteShellPtr();
}
