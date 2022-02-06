#pragma once

#include "CmdGoToNoteBase.h"

template <class T>
GrymCore::ICommandActionPtr CmdGoToNoteBase::CreateInstance(const PluginDataPtr &my_data, const MapNoteShellPtr &note)
{
	ATLASSERT(my_data.get() && note.get());

	ATL::CComObject<T> *obj;
	ATLVERIFY(S_OK == ATL::CComObject<T>::CreateInstance(&obj));
	GrymCore::ICommandActionPtr rv = obj;
	ATLASSERT(NULL != rv);

	obj->my_data_weak_ = my_data;
	obj->note_ = note;

	return rv;
}
