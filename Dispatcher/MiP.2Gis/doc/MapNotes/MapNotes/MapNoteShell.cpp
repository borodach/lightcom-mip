#include "StdAfx.h"
#include ".\mapnoteshell.h"
#include "PluginData.h"

ATL::_ATL_FUNC_INFO MapNoteShell::func_info_OnClose__ = {CC_STDCALL, VT_EMPTY, 0, {}};
ATL::_ATL_FUNC_INFO MapNoteShell::func_info_OnButtonAction__ = {CC_STDCALL, VT_EMPTY, 1, {VT_I4}};

MapNoteShell::MapNoteShell(const PluginDataPtr &my_data, GrymCore::IMapRect *map_bounds, GrymCore::IMapPoint *anchor, const tstd::tstring &text) :
	my_data_weak_(my_data),
	visible_(false),
	closed_by_user_(false),
	map_bounds_(map_bounds),
	save_button_index_(-1),
	close_button_index_(-1),
	pos_(-1)
{
	ATLASSERT(my_data.get() && map_bounds && anchor);
	if (GrymCore::IMapGraphicsPtr map_graphics = my_data->get_base_view()->GetMap())
	{
		callout_ = map_graphics->CreateCallout(anchor, _bstr_t(text.c_str()), VARIANT_FALSE);
		if (callout_)
		{
			close_button_index_ = callout_->AddStandardButton(GrymCore::CalloutStandardButtonTypeClose);
			save_button_index_ = callout_->AddCheckButton(my_data->get_bmp_save(), 0, VARIANT_FALSE);
			DispEventAdvise(callout_);
			return;
		}
	}
	throw std::runtime_error("Can't create callout shell");
}

MapNoteShell::MapNoteShell(const PluginDataPtr &my_data, const tstd::tstring &serialized) :
	my_data_weak_(my_data),
	visible_(false),
	closed_by_user_(false),
	save_button_index_(-1),
	close_button_index_(-1),
	pos_(-1)
{
	ATLASSERT(my_data.get());
	if (GrymCore::IGrymObjectFactoryPtr factory = my_data->get_base_view()->GetFactory())
	{
		class local
		{
		public:
			local(const tstd::tstring &val) : curr_(val.c_str()) {}
			int next_int()
			{
				if (!*curr_) return 0;
				LPTSTR next;
				int rv = _tcstol(curr_, &next, 10);
				curr_ = *next ? (next + 1) : next;
				return rv;
			}
			LPCTSTR tail() const {return curr_;}
		private:
			LPCTSTR curr_;
		};
		local parse(serialized);
		int x = parse.next_int();
		int y = parse.next_int();
		GrymCore::IDevPointPtr vector = factory->CreateDevPoint(x, y);
		x = parse.next_int();
		y = parse.next_int();
		GrymCore::IDevSizePtr size = factory->CreateDevSize(x, y);
		x = parse.next_int();
		y = parse.next_int();
		GrymCore::IMapPointPtr anchor = factory->CreateMapPoint(x, y);
		int minx = parse.next_int();
		int miny = parse.next_int();
		int maxx = parse.next_int();
		int maxy = parse.next_int();
		map_bounds_ = factory->CreateMapRect(minx, miny, maxx, maxy);
		if (parse.tail() && *parse.tail())
		{
			if (GrymCore::IMapGraphicsPtr map_graphics = my_data->get_base_view()->GetMap())
			{
				callout_ = map_graphics->CreateCallout2(anchor, _bstr_t(parse.tail()), VARIANT_FALSE, vector, size);
				if (callout_)
				{
					close_button_index_ = callout_->AddStandardButton(GrymCore::CalloutStandardButtonTypeClose);
					save_button_index_ = callout_->AddCheckButton(my_data->get_bmp_save(), 0, VARIANT_TRUE);
					DispEventAdvise(callout_);
					return;
				}
			}
		}
	}
	throw std::runtime_error("Can't read serialized data");
}

MapNoteShell::~MapNoteShell(void)
{
	if (NULL != callout_)
	{
		DispEventUnadvise(callout_);
	}
}

void MapNoteShell::SetPosInfo(int pos)
{
	pos_ = pos;
}

tstd::tstring MapNoteShell::Serialize() const
{
	ATLASSERT(NULL != callout_);
	tstd::tstringstream rv;
	if (NULL != callout_)
	{
		GrymCore::IDevPointPtr vector = callout_->GetVector();
		rv << int(vector->GetX()) << _T(':') << int(vector->GetY()) << _T(':');
		GrymCore::IDevSizePtr size = callout_->GetSize();
		rv << int(size->GetWidth()) << _T(':') << int(size->GetHeight()) << _T(':');
		GrymCore::IMapPointPtr anchor = callout_->GetAnchor();
		rv << int(anchor->GetX()) << _T(':') << int(anchor->GetY()) << _T(':');
		
		rv << int(map_bounds_->GetMinX()) << _T(':') << int(map_bounds_->GetMinY()) << _T(':')
			<< int(map_bounds_->GetMaxX()) << _T(':') << int(map_bounds_->GetMaxY()) << _T(':');
		
		rv << static_cast<LPCTSTR>(callout_->GetText());
	}
	return rv.str();
}

void MapNoteShell::Show()
{
	if (!visible_)
	{
		if (PluginDataPtr my_data = my_data_weak_.lock())
		{
			if (GrymCore::IMapGraphicsPtr map_graphics = my_data->get_base_view()->GetMap())
			{
				map_graphics->AddGraphic(callout_);
				visible_ = true;
				closed_by_user_ = false;
			}
		}
	}
}

void MapNoteShell::UpdateText(const tstd::tstring &text)
{
	ATLASSERT(NULL != callout_);
	callout_->PutText(_bstr_t(text.c_str()));
	if (visible_)
	{
		if (PluginDataPtr my_data = my_data_weak_.lock())
		{
			my_data->get_base_view()->GetMap()->Invalidate(VARIANT_FALSE);
		}
	}
}

bool MapNoteShell::IsNoteToSave() const
{
	ATLASSERT(NULL != callout_);
	return !closed_by_user_ && (save_button_index_ >= 0) && callout_->GetButtonChecked(save_button_index_);
}

tstd::tstring MapNoteShell::GetText() const
{
	ATLASSERT(NULL != callout_);
	return tstd::tstring(static_cast<LPCTSTR>(callout_->GetText()));
}

GrymCore::IMapRectPtr MapNoteShell::GetMapBounds() const
{
	ATLASSERT(NULL != map_bounds_);
	return map_bounds_;
}

GrymCore::IMapPointPtr MapNoteShell::GetAnchor() const
{
	ATLASSERT(NULL != callout_);
	return callout_->GetAnchor();
}

GrymCore::ICalloutPtr MapNoteShell::GetCallout() const
{
	return callout_;
}

void __stdcall MapNoteShell::OnClose()
{
	visible_ = false;
}

void __stdcall MapNoteShell::OnButtonAction(long nIndex)
{
	if (nIndex == close_button_index_)
	{
		closed_by_user_ = true;
	}
}
