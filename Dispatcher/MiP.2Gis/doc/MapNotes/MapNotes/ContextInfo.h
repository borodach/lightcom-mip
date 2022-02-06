#pragma once

class MapNoteShell;
typedef boost::shared_ptr<MapNoteShell> MapNoteShellPtr;
class PluginData;
typedef boost::shared_ptr<PluginData> PluginDataPtr;

struct __declspec(uuid("0AF8B48C-7AB1-40c2-9DB6-8152DD9B06D2"))
IMapNoteContextInfo : public IUnknown
{
	virtual MapNoteShellPtr __stdcall GetPosNote() = 0;
};

_COM_SMARTPTR_TYPEDEF(IMapNoteContextInfo, __uuidof(IMapNoteContextInfo));

/** ����� ��������� �������������� ���������� ��� ����� �� �����, ������������ ���������
	����� ���� �������� � ������� ��������� ������� - ����� �� ��������� ���������� ��� ������ ������� ������
*/
class ATL_NO_VTABLE ContextInfo : 
	public ATL::CComObjectRootEx<ATL::CComSingleThreadModel>,
	public IMapNoteContextInfo
{
public:
	/// ���� � ��������� ��� ��� ���� ����������, �� ��� ����� ������� � ���������
	static IMapNoteContextInfoPtr GetInfo(const PluginDataPtr &my_data, GrymCore::IContextBase *pContext);
protected:
	ContextInfo();
	~ContextInfo();

	BEGIN_COM_MAP(ContextInfo)
		COM_INTERFACE_ENTRY(IMapNoteContextInfo)
	END_COM_MAP()
public: // IMapNoteContextInfo
	virtual MapNoteShellPtr __stdcall GetPosNote();
private:
	MapNoteShellPtr note_in_pos_;
};
