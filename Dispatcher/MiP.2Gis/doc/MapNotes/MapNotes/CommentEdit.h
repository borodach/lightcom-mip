// CommentEdit.h : Declaration of the CommentEdit

#pragma once

#include "resource.h"       // main symbols


// CommentEdit

class CommentEdit : public ATL::CDialogImpl<CommentEdit>
{
	typedef ATL::CDialogImpl<CommentEdit> inherited;
public:
	static tstd::tstring Execute(const tstd::tstring &name, const tstd::tstring &comment);

	enum {IDD = IDD_COMMENTEDIT};
protected:
	CommentEdit();
	~CommentEdit();

	BEGIN_MSG_MAP(CommentEdit)
		MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
		COMMAND_HANDLER(IDOK, BN_CLICKED, OnClickedOK)
		COMMAND_HANDLER(IDCANCEL, BN_CLICKED, OnClickedCancel)
	END_MSG_MAP()
private:
	struct InitParams;
	tstd::tstring result_;

	// Handler prototypes:
	//  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	//  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	//  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
};


