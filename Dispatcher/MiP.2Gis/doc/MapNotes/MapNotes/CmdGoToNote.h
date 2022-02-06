#pragma once

#include "CmdGoToNoteBase.h"

class CmdGoToNote : public CmdGoToNoteBase
{
	typedef CmdGoToNoteBase inherited;
public:
	static GrymCore::ICommandActionPtr CreateInstance(const PluginDataPtr &my_data, const MapNoteShellPtr &note);
public: // GrymCore::ICommandState
	STDMETHOD(get_Available)(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal);
};
