#pragma once

#include "PluginData.h"

class CmdRestoreNotes : public GPCommon::VisibleStateCommandBase
{
public:
	static GrymCore::ICommandActionPtr CreateInstance(const PluginDataPtr &my_data);
protected:
	CmdRestoreNotes(void);
	~CmdRestoreNotes(void);
public: // GrymCore::ICommandState
	STDMETHOD(get_Available)(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal);
public: // GrymCore::ICommandAction
	STDMETHOD(raw_OnCommand)(GrymCore::IContextBase *pContext);
private:
	PluginDataWeakPtr my_data_weak_;
};
