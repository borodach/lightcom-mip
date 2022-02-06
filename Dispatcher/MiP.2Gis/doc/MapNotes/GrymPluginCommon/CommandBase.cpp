#include "StdAfx.h"
#include ".\commandbase.h"

namespace GPCommon
{

CommandBase::CommandBase(DWORD accelerator) :
  accelerator_(accelerator)
{
}

CommandBase::~CommandBase()
{
}

HRESULT WINAPI CommandBase::ICommandAcceleratorQIFunc(void* pv, REFIID riid, LPVOID* ppv, DWORD dw)
{
  CommandBase *pThis = static_cast<CommandBase *>(pv);
  *ppv = NULL;
  if (pThis->accelerator_)
  {
    *ppv = static_cast<GrymCore::ICommandAccelerator *>(pThis);
    pThis->AddRef();
    return S_OK;
  }
  return E_NOINTERFACE;
}

STDMETHODIMP CommandBase::get_Accelerator(LONG *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = accelerator_;
  return S_OK;
}

VisibleCommandBase::VisibleCommandBase(const _bstr_t &group_name, long priority, long position, const _bstr_t &image_name, 
    UINT rcid_title, const IBitmapHolderPtr &bmp_toolbar, const IBitmapHolderPtr &bmp_menu, DWORD accelerator) :
  CommandBase(accelerator),
  group_name_(group_name),
  priority_(priority),
  position_(position),
  image_name_(image_name),
  bmp_toolbar_(bmp_toolbar), bmp_menu_(bmp_menu),
  rcid_title_(rcid_title)
{
}

VisibleCommandBase::~VisibleCommandBase()
{
}

STDMETHODIMP VisibleCommandBase::get_GroupName(BSTR *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = group_name_.copy();
  return S_OK;
}

STDMETHODIMP VisibleCommandBase::get_Priority(long *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = priority_;
  return S_OK;
}

STDMETHODIMP VisibleCommandBase::get_Position(long *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = position_;
  return S_OK;
}

STDMETHODIMP VisibleCommandBase::get_ImageName(BSTR *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = image_name_.copy();
  return S_OK;
}

STDMETHODIMP VisibleCommandBase::get_DefaultBitmap(BSTR bsType, OLE_HANDLE *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = 0;
  if (!bsType)
  {
    return E_INVALIDARG;
  }
  if (!wcscmp(bsType, L"Toolbar"))
  {
    *pVal = bmp_toolbar_.get() ? reinterpret_cast<OLE_HANDLE>(bmp_toolbar_->GetBitmap()) : 0;
  }
  else if (!wcscmp(bsType, L"Menu"))
  {
    *pVal = bmp_menu_.get() ? reinterpret_cast<OLE_HANDLE>(bmp_menu_->GetBitmap()) : 0;
  }
  else
  {
    return E_INVALIDARG;
  }
  return (*pVal) ? S_OK : S_FALSE;
}

STDMETHODIMP VisibleCommandBase::get_Title(BSTR *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  if (rcid_title_)
  {
    title_ = LoadResourceBSTR(rcid_title_);
    rcid_title_ = 0;
  }
  *pVal = title_.copy();
  return (*pVal) ? S_OK : S_FALSE;
}

VisibleStateCommandBase::VisibleStateCommandBase(const _bstr_t &group_name, long priority, long position, const _bstr_t &image_name, 
    UINT rcid_title, const IBitmapHolderPtr &bmp_toolbar, const IBitmapHolderPtr &bmp_menu, DWORD accelerator) :
  VisibleCommandBase(group_name, priority, position, image_name, rcid_title, bmp_toolbar, bmp_menu, accelerator)
{
}

VisibleStateCommandBase::~VisibleStateCommandBase()
{
}

STDMETHODIMP VisibleStateCommandBase::get_Available(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = VARIANT_TRUE;
  return S_OK;
}

STDMETHODIMP VisibleStateCommandBase::get_Enabled(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = VARIANT_TRUE;
  return S_OK;
}

STDMETHODIMP VisibleStateCommandBase::get_Checked(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal)
{
  if (!pVal)
  {
    return E_POINTER;
  }
  *pVal = VARIANT_FALSE;
  return S_OK;
}

} // namespace GPCommon
