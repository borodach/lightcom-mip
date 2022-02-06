#pragma once

#include <comdef.h>
#include <atlstr.h>

#include <boost/noncopyable.hpp>
#include <boost/smart_ptr.hpp>

#include "tstd.h"

namespace GPCommon
{

_bstr_t LoadResourceBSTR(UINT id);
tstd::tstring LoadResourceString(UINT id);
void StringReplace(tstd::tstring &source, const tstd::tstring &lookup, const tstd::tstring &replace);
tstd::tstring HTMLMakeEscaped(const tstd::tstring &orig);
tstd::tstring HTMLUnescape(const tstd::tstring &orig);

struct IBitmapHolder : boost::noncopyable
{
  virtual ~IBitmapHolder() = 0 {}
  virtual HBITMAP GetBitmap() const = 0;
};

typedef boost::shared_ptr<IBitmapHolder> IBitmapHolderPtr;

IBitmapHolderPtr LoadResourceBitmap(UINT id, bool delayed = true);

tstd::tstring GetUserDataPath(const tstd::tstring &tail);

} // namespace GPCommon

