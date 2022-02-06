///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPointProperties.cs
//
//  Facility:       ������ MiP ��� 2Gis
//
//
//  Abstract:       ������ ������� �������
//
//  Environment:    VC# 8.0
//
//  Author:         ������ �.�.
//
//  Creation Date:  03-18-2007
//
//  Copyright (C) OOO "�������", 2007. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// ������ ������� ������
    /// </summary>
    public partial class MobileObjectListForm: Form
    {

        /// <summary>
        /// ������ - ������
        /// </summary>
        private MiPPlugin plugin;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="plugin">������ - ������</param>
        public MobileObjectListForm (MiPPlugin objPlugin)
        {
            InitializeComponent ();

            plugin = objPlugin;
            
        }

        /// <summary>
        /// ���������� ������ OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOK (object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.OK;
            //this.Close ();
        }

        /// <summary>
        /// ���������� ������ Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel (object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            //this.Close ();
        }
    }
}