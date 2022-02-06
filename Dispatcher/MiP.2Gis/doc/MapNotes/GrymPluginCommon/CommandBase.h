#pragma once

#include <atlcom.h>
#include "Util.h"

namespace GPCommon
{

class ATL_NO_VTABLE CommandBase :
	public ATL::CComObjectRootEx<ATL::CComSingleThreadModel>,
	public GrymCore::ICommandAccelerator,
	public GrymCore::ICommandAction
{
public:
	template <class T>
	static GrymCore::ICommandActionPtr CreateInstance()
	{
		ATL::CComObject<T> *obj;
		ATLVERIFY(S_OK == ATL::CComObject<T>::CreateInstance(&obj));
		GrymCore::ICommandActionPtr rv = obj;
		ATLASSERT(NULL != rv);
		return rv;
	}
protected:
	CommandBase(DWORD accelerator = 0);
	~CommandBase();

	BEGIN_COM_MAP(CommandBase)
		COM_INTERFACE_ENTRY(GrymCore::ICommandAction)
		COM_INTERFACE_ENTRY_FUNC(__uuidof(GrymCore::ICommandAccelerator), 0, ICommandAcceleratorQIFunc)
	END_COM_MAP()
public: // GrymCore::ICommandAccelerator
	STDMETHOD(get_Accelerator)(LONG *pVal);
public: // GrymCore::ICommandAction
	// нужно определить в классе-потомке:
	STDMETHOD(raw_OnCommand)(GrymCore::IContextBase *pContext) PURE;
protected:
	DWORD accelerator_;
private:
	static HRESULT WINAPI ICommandAcceleratorQIFunc(void* pv, REFIID riid, LPVOID* ppv, DWORD dw);
};

class ATL_NO_VTABLE VisibleCommandBase : public CommandBase,
	public GrymCore::ICommandPlacement,
	public GrymCore::ICommandAppearance
{
protected:
	VisibleCommandBase(const _bstr_t &group_name, long priority, long position, const _bstr_t &image_name, UINT rcid_title = 0,
		const IBitmapHolderPtr &bmp_toolbar = IBitmapHolderPtr(), const IBitmapHolderPtr &bmp_menu = IBitmapHolderPtr(), 
		DWORD accelerator = 0);
	~VisibleCommandBase();

	BEGIN_COM_MAP(VisibleCommandBase)
		COM_INTERFACE_ENTRY(GrymCore::ICommandPlacement)
		COM_INTERFACE_ENTRY(GrymCore::ICommandAppearance)
		COM_INTERFACE_ENTRY_CHAIN(CommandBase)
	END_COM_MAP()
public: // GrymCore::ICommandPlacement
	STDMETHOD(get_GroupName)(BSTR *pVal);
	STDMETHOD(get_Priority)(long *pVal);
	STDMETHOD(get_Position)(long *pVal);
public: // GrymCore::ICommandAppearance
	STDMETHOD(get_ImageName)(BSTR *pVal);
	STDMETHOD(get_DefaultBitmap)(BSTR bsType, OLE_HANDLE *pVal);
	STDMETHOD(get_Title)(BSTR *pVal);
protected:
	_bstr_t group_name_;
	long priority_;
	long position_;
	_bstr_t image_name_;
	IBitmapHolderPtr bmp_toolbar_, bmp_menu_;
	UINT rcid_title_;
	_bstr_t title_;
};

class ATL_NO_VTABLE VisibleStateCommandBase : public VisibleCommandBase,
	public GrymCore::ICommandState
{
protected:
	VisibleStateCommandBase(const _bstr_t &group_name, long priority, long position, const _bstr_t &image_name, UINT rcid_title = 0, 
		const IBitmapHolderPtr &bmp_toolbar = IBitmapHolderPtr(), const IBitmapHolderPtr &bmp_menu = IBitmapHolderPtr(), 
		DWORD accelerator = 0);
	~VisibleStateCommandBase();

	BEGIN_COM_MAP(VisibleStateCommandBase)
		COM_INTERFACE_ENTRY(GrymCore::IControlState)
		COM_INTERFACE_ENTRY(GrymCore::ICommandState)
		COM_INTERFACE_ENTRY_CHAIN(VisibleCommandBase)
	END_COM_MAP()
public: // GrymCore::ICommandState
	// default implementation: alvays available
	STDMETHOD(get_Available)(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal);
	// default implementation: alvays enabled
	STDMETHOD(get_Enabled)(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal);
	// default implementation: alvays not checked
	STDMETHOD(get_Checked)(GrymCore::IContextBase *pContext, VARIANT_BOOL *pVal);
};

} // namespace GPCommon
