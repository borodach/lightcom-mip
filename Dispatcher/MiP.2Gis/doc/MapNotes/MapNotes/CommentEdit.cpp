// CommentEdit.cpp : Implementation of CommentEdit

#include "stdafx.h"
#include "CommentEdit.h"

struct CommentEdit::InitParams
{
	InitParams(const tstd::tstring &name) : name_(name) {}
	
	const tstd::tstring &name_;
};

tstd::tstring CommentEdit::Execute(const tstd::tstring &name, const tstd::tstring &comment)
{
	CommentEdit dlg;
	dlg.result_ = comment;
	InitParams init(name);
	if (IDOK == dlg.DoModal(::GetActiveWindow(), reinterpret_cast<LPARAM>(&init)))
	{
		return dlg.result_;
	}
	else
	{
		return comment;
	}
}

// CommentEdit
CommentEdit::CommentEdit()
{
}

CommentEdit::~CommentEdit()
{
}

LRESULT CommentEdit::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	ATLASSERT(lParam);
	InitParams *init = reinterpret_cast<InitParams *>(lParam);
	ATL::CString title;
	GetWindowText(title);
	title += init->name_.c_str();
	SetWindowText(title);
	
	ATL::CWindow edit = GetDlgItem(IDC_EDIT);
	tstd::tstring text = result_;
	GPCommon::StringReplace(text, _T("\r"), _T(""));
	GPCommon::StringReplace(text, _T("\n"), _T("\r\n"));
	edit.SetWindowText(text.c_str());
	edit.SetFocus();
	edit.SendMessage(EM_SETSEL);
	
	this->CenterWindow();
	return 0;  // Let the system set the focus
}

LRESULT CommentEdit::OnClickedOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
	ATL::CString text;
	GetDlgItem(IDC_EDIT).GetWindowText(text);
	result_ = static_cast<const tstd::char_type *>(text);
	GPCommon::StringReplace(result_, _T("\r\n"), _T("\n"));
	EndDialog(wID);
	return 0;
}

LRESULT CommentEdit::OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
	EndDialog(wID);
	return 0;
}
