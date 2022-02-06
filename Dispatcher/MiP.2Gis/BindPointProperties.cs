///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPointProperties.cs
//
//  Facility:       ������ MiP ��� 2Gis
//
//
//  Abstract:       ������ ������� ���� ��������
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

/*
* $History: BindPointProperties.cs $
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.03.07   Time: 14:24
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
* 
*/
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
    /// ������ ������� ���� ��������
    /// </summary>
    public partial class BindPointProperties : Form
    {
        /// <summary>
        /// ���� �� ����� ������� ������� OK
        /// </summary>
        private bool bOK;

        /// <summary>
        /// ���� �� ����� ������� ������� OK
        /// </summary>
        public bool IsOK
        {
            get { return this.bOK; }
        }

        /// <summary>
        /// �������� ����� ��������
        /// </summary>
        public string Description
        {
            get 
            {
                return this.txtDescription.Text;
            }
        }

        /// <summary>
        /// ������
        /// </summary>
        public double Latitude
        {
            get {return BindPoint.ParseEarthCoordinate (this.txtLatitude.Text);}
        }

        /// <summary>
        /// �������
        /// </summary>
        public double Longitude
        {
            get { return BindPoint.ParseEarthCoordinate (this.txtLongitude.Text); }
        }        

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="bp">����� ��������</param>
        public BindPointProperties (BindPoint bp)
        {
            bOK = false;
            InitializeComponent ();
            try
            {
                this.txtDescription.Text = bp.Description;
                this.txtLatitude.Text = BindPoint.FormatEarthCoordinate (bp.PointOnEarth.y);
                this.txtLongitude.Text = BindPoint.FormatEarthCoordinate (bp.PointOnEarth.x);
            }
            catch (Exception e)
            {
                MessageBox.Show (e.ToString (), Properties.Resources.PluginName);
            }
        }

        /// <summary>
        /// ���������� ������ OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOK (object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty (txtDescription.Text.Trim ()))
            {
                MessageBox.Show (Properties.Resources.InvalidBindPointDescMessage, Properties.Resources.PluginName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtDescription.Focus ();

                return;
            }

            if (! BindPoint.IsValidEarthCoordinate (txtLatitude.Text))
            {
                MessageBox.Show (Properties.Resources.InvalidBindPointLatMessage, Properties.Resources.PluginName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLatitude.Focus ();

                return;
            }

            if (!BindPoint.IsValidEarthCoordinate (txtLongitude.Text))
            {
                MessageBox.Show (Properties.Resources.InvalidBindPointLongMessage, Properties.Resources.PluginName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLongitude.Focus ();

                return;
            }

            bOK = true;
            this.Close ();
        }

        /// <summary>
        /// ���������� ������ Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel (object sender, EventArgs e)
        {
            bOK = false;
            this.Close ();
        }

        /// <summary>
        /// ���������� ������� �� ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyDown (object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnOK (this, null);
            }
        }
    }
}