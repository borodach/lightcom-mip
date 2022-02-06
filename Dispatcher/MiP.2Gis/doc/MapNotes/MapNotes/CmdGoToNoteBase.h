#pragma once

#include "PluginData.h"

class CmdGoToNoteBase : public GPCommon::VisibleStateCommandBase
{
	typedef GPCommon::VisibleStateCommandBase inherited;
protected:
	CmdGoToNoteBase();
	~CmdGoToNoteBase();
public: // GrymCore::ICommandPlacement
	STDMETHOD(get_Position)(long *pVal);
public: // GrymCore::ICommandAppearance
	STDMETHOD(get_Title)(BSTR *pVal);
//public: // GrymCore::ICommandState
	//STDMETHOD(get_Available)(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal);
public: // GrymCore::ICommandAction
	STDMETHOD(raw_OnCommand)(GrymCore::IContextBase *pContext);
protected:
	PluginDataWeakPtr my_data_weak_;
	MapNoteShellPtr note_;

	template <class T>
	static GrymCore::ICommandActionPtr CreateInstance(const PluginDataPtr &my_data, const MapNoteShellPtr &note);
};
