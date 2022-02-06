#pragma once

class PluginData;
typedef boost::shared_ptr<PluginData> PluginDataPtr;
typedef boost::weak_ptr<PluginData> PluginDataWeakPtr;

class MapNoteShell;
typedef boost::shared_ptr<MapNoteShell> MapNoteShellPtr;

class PluginData
{
public:
	PluginData(GrymCore::IBaseViewThread *base_view);
	~PluginData(void);

	HRESULT Init(const PluginDataPtr &this_ptr);
	void Terminate();
	
	/// восстанавливает отображение заметок (если они были скрыты)
	void RestoreMapNotes();
	/// имеются скрытые заметки
	bool HaveHiddenNotes();

	GrymCore::IBitmap *get_bmp_save();
	GrymCore::IBaseViewThread *get_base_view();
	
	void note_add(const PluginDataPtr &this_ptr, const MapNoteShellPtr &note);
	void note_remove(const MapNoteShellPtr &note);
	void bring_to_front(const MapNoteShellPtr &note, bool redraw);
	MapNoteShellPtr note_by_position(GrymCore::IMapPoint *position);
private:
	struct NoteItem
	{
		MapNoteShellPtr shell_;
		GrymCore::ICommandActionPtr cmd_goto_, cmd_gotoclosed_;
	};
	typedef std::deque<NoteItem> mapnote_vector;

	GrymCore::IBaseViewThreadPtr base_view_;
	GrymCore::IBitmapPtr bmp_save_;
	GrymCore::IMenuPtr my_submenu_;
	GrymCore::IMenuPtr closed_notes_submenu_;
	
	/// сортировка по убыванию "приоритета" - в начале списка находятся более новые выноски
	mapnote_vector notes_;
	tstd::tstring datapath_;
	
	HRESULT add_mapcontextmenu_commands(const PluginDataPtr &this_ptr);
	void load(const PluginDataPtr &this_ptr);
	void load_item(const tstd::tstring &serialized, const PluginDataPtr &this_ptr);
	void save();
	
	/// обновляет в MapNoteShell информацию о позиции в нашем списке
	void update_notes_pos_info();
};
