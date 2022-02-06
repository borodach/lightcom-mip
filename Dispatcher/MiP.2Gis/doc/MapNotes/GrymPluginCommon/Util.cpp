#include "StdAfx.h"
#include ".\util.h"

namespace GPCommon
{

tstd::tstring LoadResourceString(UINT id)
{
  ATL::CString str;
  str.LoadString(id);
  return static_cast<const tstd::char_type *>(str);
}

_bstr_t LoadResourceBSTR(UINT id)
{
  ATL::CStringW str;
  str.LoadString(id);
  return _bstr_t(str.AllocSysString(), false);
}

void StringReplace(tstd::tstring &source, const tstd::tstring &lookup, const tstd::tstring &replace)
{
  if (!lookup.empty())
  {
    size_t start = 0;
    while (true)
    {
      size_t pos = source.find(lookup, start);
      if (tstd::tstring::npos == pos)
      {
        return;
      }
      source.replace(pos, lookup.length(), replace);
      start = pos + replace.length();
    }
  }
}

tstd::tstring HTMLMakeEscaped(const tstd::tstring &orig)
{
  tstd::tstring rv = orig;
  StringReplace(rv, _T("&"), _T("&amp;"));
  StringReplace(rv, _T(">"), _T("&gt;"));
  StringReplace(rv, _T("<"), _T("&lt;"));
  StringReplace(rv, _T("\""), _T("&quot;"));
  StringReplace(rv, _T("\n"), _T("<br>"));
  StringReplace(rv, _T("\r"), _T(""));
  return rv;
}

tstd::tstring HTMLUnescape(const tstd::tstring &orig)
{
  tstd::tstring rv = orig;
  StringReplace(rv, _T("<br>"), _T("\n"));
  StringReplace(rv, _T("&gt;"), _T(">"));
  StringReplace(rv, _T("&lt;"), _T("<"));
  StringReplace(rv, _T("&quot;"), _T("\""));
  StringReplace(rv, _T("&amp;"), _T("&"));
  return rv;
}

namespace
{

class BitmapHolder : public IBitmapHolder
{
public:
  BitmapHolder(HBITMAP bmp) : bmp_(bmp) {ATLASSERT(bmp_);}
  ~BitmapHolder() {::DeleteObject(bmp_);}
  
  virtual HBITMAP GetBitmap() const {return bmp_;}
private:
  HBITMAP bmp_;
};

class DelayLoadBitmapHolder : public IBitmapHolder
{
public:
  DelayLoadBitmapHolder(UINT id) : id_(id), bmp_(0) {ATLASSERT(id_);}
  ~DelayLoadBitmapHolder() {if (bmp_) ::DeleteObject(bmp_);}
  
  virtual HBITMAP GetBitmap() const
  {
    if (!bmp_ && id_)
    {
      bmp_ = ::LoadBitmap(ATL::_AtlBaseModule.GetResourceInstance(), MAKEINTRESOURCE(id_));
      id_ = 0;
    }
    return bmp_;
  }
private:
  mutable UINT id_;
  mutable HBITMAP bmp_;
};

} // namespace

IBitmapHolderPtr LoadResourceBitmap(UINT id, bool delayed)
{
  if (delayed)
  {
    return IBitmapHolderPtr(new DelayLoadBitmapHolder(id));
  }
  HBITMAP bmp = ::LoadBitmap(ATL::_AtlBaseModule.GetResourceInstance(), MAKEINTRESOURCE(id));
  if (bmp)
  {
    return IBitmapHolderPtr(new BitmapHolder(bmp));
  }
  return IBitmapHolderPtr();
}

tstd::tstring GetUserDataPath(const tstd::tstring &tail)
{
  tstd::tstring rv;
  LPITEMIDLIST pidl = 0;
  if (S_OK == ::SHGetSpecialFolderLocation(NULL, CSIDL_APPDATA, &pidl))
  {
    TCHAR szPath[MAX_PATH + 200];
    if (::SHGetPathFromIDList(pidl, szPath))
    {
      rv = szPath;
      if (rv.empty() || (_T('\\') != *rv.rbegin()))
      {
        rv += _T('\\');
      }
      rv += _T("Grym\\");
      rv += tail;
    }
    IMalloc *pMalloc = 0;
    if (SUCCEEDED(::SHGetMalloc(&pMalloc)))
    {
      pMalloc->Free(pidl);
      pMalloc->Release();
    }
  }
  return rv;
}

} // namespace GPCommon
