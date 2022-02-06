///////////////////////////////////////////////////////////////////////////////
//
//  File:           Win32WindowHandle.cs
//
//  Facility:       ������ � ������
//                  
//
//
//  Abstract:       ������� ���������� ���������� IWin32Window
//
//
//  Environment:    MSVC# 2005
//
//  Author:         ������ �.�.
//
//  Creation Date:  22-Mar-2007
//
//  Copyright (C) OOO "�������", 2006. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: Win32WindowHandle.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:27
 * Created in $/LightCom/.NET/MiP/Dispatcher/Common
*/
using System;
using System.Windows.Forms;

namespace LightCom.MiP.Dispatcher.Common
{
    /// <summary>
    /// ������� ���������� ���������� IWin32Window
    /// </summary>
    public class Win32WindowHandle: IWin32Window
    {

        /// <summary>
        /// Default constructor
        /// </summary>
        public Win32WindowHandle (): this (IntPtr.Zero)
        {
        }

        /// <summary>
        /// �onstructor
        /// </summary>
        public Win32WindowHandle (IntPtr hwnd)
        {
            Handle = hwnd;
        }

        /// <summary>
        /// Handle to the window
        /// </summary>
        public IntPtr Handle 
        {
            get { return handleValue; }
            set { handleValue = value; }
        }

        /// <summary>
        /// Handle to the window
        /// </summary>
        private IntPtr handleValue;
    }
}
