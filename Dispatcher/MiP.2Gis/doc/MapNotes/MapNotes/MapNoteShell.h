#pragma once

class PluginData;
typedef boost::shared_ptr<PluginData> PluginDataPtr;
typedef boost::weak_ptr<PluginData> PluginDataWeakPtr;

/** оболочка для выноски, с дополнительной информацией о нужном фрагменте карты.
    позволяет осуществлять сериализацию выноски
*/

class MapNoteShell : boost::noncopyable,
	private ATL::IDispEventSimpleImpl<1, MapNoteShell, &__uuidof(GrymCore::_ICalloutEvents)>
{
public:
	MapNoteShell(const PluginDataPtr &my_data, GrymCore::IMapRect *map_bounds, GrymCore::IMapPoint *anchor, const tstd::tstring &text);
	MapNoteShell(const PluginDataPtr &my_data, const tstd::tstring &serialized);
	~MapNoteShell(void);
	
	void SetPosInfo(int pos);
	int GetPosInfo() const {return pos_;}

	tstd::tstring Serialize() const;

	void Show();
	void UpdateText(const tstd::tstring &text);
	bool IsNoteToSave() const;
	bool IsNoteHidden() const {return !visible_ && !closed_by_user_;}
	bool IsNoteVisible() const {return visible_;}
	bool IsClosedByUser() const {return closed_by_user_;}
	tstd::tstring GetText() const;
	GrymCore::IMapRectPtr GetMapBounds() const;
	GrymCore::IMapPointPtr GetAnchor() const;
	GrymCore::ICalloutPtr GetCallout() const;

	BEGIN_SINK_MAP(MapNoteShell)
		SINK_ENTRY_INFO(1, __uuidof(GrymCore::_ICalloutEvents), 1, OnClose, &func_info_OnClose__)
		SINK_ENTRY_INFO(1, __uuidof(GrymCore::_ICalloutEvents), 2, OnButtonAction, &func_info_OnButtonAction__)
	END_SINK_MAP()
private:
	static ATL::_ATL_FUNC_INFO func_info_OnClose__;
	static ATL::_ATL_FUNC_INFO func_info_OnButtonAction__;

	PluginDataWeakPtr my_data_weak_;
	GrymCore::ICalloutPtr callout_;
	/// выноска отображается на карте
	bool visible_;
	/// выноска закрыта пользователем явно, кнопкой закрытия
	bool closed_by_user_;
	/// границы карты, которые нужно установить при переходе к заметке
	GrymCore::IMapRectPtr map_bounds_;
	/// индекс кнопки сохранения
	long save_button_index_;
	/// индекс кнопки закрытия
	long close_button_index_;
	/// позиция в списке выносок
	long pos_;

	// [id(1), helpstring("The callout is to be destroyed")] void OnClose(void);
	void __stdcall OnClose();
	// [id(2), helpstring("The action betton is clicked")] void OnButtonAction([in] LONG nIndex);
	void __stdcall OnButtonAction(long nIndex);
};

typedef boost::shared_ptr<MapNoteShell> MapNoteShellPtr;
