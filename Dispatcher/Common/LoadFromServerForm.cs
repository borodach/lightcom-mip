///////////////////////////////////////////////////////////////////////////////
//
//  File:           LoadFromServerForm.cs
//
//  Facility:       ���������������� ��������� ���
//                  
//
//
//  Abstract:       ������ �������� ���� � Web �������.
//
//
//  Environment:    MSVC# 2005
//
//  Author:         ������ �.�.
//
//  Creation Date:  21-Mar-2007
//
//  Copyright (C) OOO "�������", 2006. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: LoadFromServerForm.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:27
 * Created in $/LightCom/.NET/MiP/Dispatcher/Common
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LightCom.MiP.DataSource;
using LightCom.MiP.Cache;
using LightCom.MiP.Common;

namespace LightCom.MiP.Dispatcher.Common
{
    /// <summary>
    /// ������ �������� ���� � Web �������
    /// </summary>
    public partial class LoadFromServerForm : Form
    {
        /// <summary>
        /// ���� ���������� ���������� ������� � �������
        /// </summary>
        public byte [] KeyToConnect
        {
            get { return this.m_DataSource.KeyToConnect; }
            set { this.m_DataSource.KeyToConnect = value; }
        }
                
        /// <summary>
        /// HTTP �������� ������
        /// </summary>
        protected HTTPDataSource m_DataSource;
        
        /// <summary>
        /// ��� ����������� ��������
        /// </summary>
        GPSDataCache m_Cache;
        
        /// <summary>
        /// ��� ����������� ��������
        /// </summary>
        public GPSDataCache Cache
        {
            get 
            {
                return m_Cache;
            }
            set
            {
                m_Cache = value;
            }
        }

        /// <summary>
        /// �����������
        /// <param name="cache">��� �����������</param>
        /// </summary>
        public LoadFromServerForm (GPSDataCache cache)
        {
            m_DataSource = new HTTPDataSource ();
            InitializeComponent ();

            Cache = cache;
            DisableNetControls ();
        }

        /// <summary>
        /// ��������� ��������� ����������, ��������� ������� ���������� � 
        /// ��������
        /// </summary>
        protected void EnableNetControls ()
        {
            readButton.Enabled = true;
            clientList.Enabled = true;
            dateTimePicker2.Enabled = true;
            dateTimePicker1.Enabled = true;
            connectionButton.Text = "�������������";
        }

        /// <summary>
        /// ���������� ��������� ����������, ��������� ������� ���������� � 
        /// ��������
        /// </summary>
        protected void DisableNetControls ()
        {
            readButton.Enabled = false;
            clientList.Items.Clear ();
            clientList.Enabled = false;
            dateTimePicker2.Enabled = false;
            dateTimePicker1.Enabled = false;
            connectionButton.Text = "C����������";
        }

        /// <summary>
        /// ���������� ������� �� ������������.������ ���������� � ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionButton_Click (object sender, System.EventArgs e)
        {
            if (m_DataSource.IsConnected ())
            {
                if (!m_DataSource.Disconnect ())
                {
                    MessageBox.Show ("��� ������� ���������� ��������� ������:\n" + m_DataSource.GetLastErrorText (),
                                     "������.");
                }
                else
                {
                    MessageBox.Show ("���������� ������� ���������.", "����������.");
                }

                DisableNetControls ();
                return;
            }

            m_DataSource.Publisher.URL = URLTextBox.Text;
            m_DataSource.Login = userTextBox.Text;
            m_DataSource.Domain = domainTextBox.Text;
            m_DataSource.Password = passwordTextBox.Text;
            if (!m_DataSource.Connect ())
            {
                MessageBox.Show ("��� ������������ ���������� ��������� ������:\n" + m_DataSource.GetLastErrorText (),
                    "������.");
                return;
            }

            string [] clients = null;
            if (!m_DataSource.GetMobileClientList (out clients))
            {
                MessageBox.Show ("��� ��������� ������ ��������������� ��������� �������� ��������� ������:\n" + m_DataSource.GetLastErrorText (),
                    "������.");
                m_DataSource.Disconnect ();
                return;
            }

            EnableNetControls ();
            clientList.Items.Clear ();
            int nSize = clients.Length;
            for (int nIdx = 0; nIdx < nSize; ++nIdx)
            {
                clientList.Items.Add (clients [nIdx]);
            }
            for (int nIdx = 0; nIdx < nSize; ++nIdx)
            {
                clientList.SetSelected (nIdx, true);
            }

            if (nSize < 1)
            {
                this.readButton.Enabled = false;
            }

            //MessageBox.Show ("���������� ������� �������.", "����������.");
            this.mainTabControl.SelectedTab = this.tabGetData;
        }

        /// <summary>
        /// ���������� ��������� ���������� ��������� � ������ ��������� 
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clientIdComboBox_SelectedIndexChanged (object sender, System.EventArgs e)
        {
            dateTimePicker1.MinDate = DateTimePicker.MinDateTime;
            dateTimePicker1.MaxDate = DateTimePicker.MaxDateTime;
            dateTimePicker1.Value = DateTime.Now;

            dateTimePicker2.MinDate = DateTimePicker.MinDateTime;
            dateTimePicker2.MaxDate = DateTimePicker.MaxDateTime;
            dateTimePicker2.Value = DateTime.Now;

            int count = clientList.SelectedIndices.Count;
            if (count < 1)
            {
                this.readButton.Enabled = false;
                return;
            }
            string [] clientIdList = new string [count];
            int idx = 0;
            foreach (string clientId in clientList.SelectedItems)
            {
                clientIdList [idx++] = clientId;
            }

            DateTime fromTime, toTime;
            if (!m_DataSource.GetEvents (clientIdList, out fromTime, out toTime))
            {
                MessageBox.Show ("��� ��������� ��������� ������� ��������� ������:\n" + m_DataSource.GetLastErrorText (),
                    "������.");
                return;
            }

            dateTimePicker1.MinDate = fromTime;
            dateTimePicker1.MaxDate = toTime;
            dateTimePicker1.Value = fromTime;

            dateTimePicker2.MinDate = fromTime;
            dateTimePicker2.MaxDate = toTime;
            dateTimePicker2.Value = toTime;
        }

        /// <summary>
        /// ���������� ������� ������ ������ � �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void readButton_Click (object sender, System.EventArgs e)
        {
            int count = clientList.SelectedIndices.Count;
            if (count < 1)
            {
                this.readButton.Enabled = false;
                return;
            }

            GPSDataCache cache = m_Cache;
            
            foreach (string clientId in clientList.SelectedItems)
            {
                ObjectPositions pos = new ObjectPositions ();
                pos.ClientId = clientId;
                cache.Add (pos);
                int idx = 0;
            }

            
            if (!m_DataSource.ReadGPSData (cache, dateTimePicker1.Value, dateTimePicker2.Value))
            {
                MessageBox.Show ("��� �������� GPS ������ ��������� ������:\n" + m_DataSource.GetLastErrorText (),
                    "������.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            m_DataSource.Disconnect ();
            this.Close ();

            //MessageBox.Show ("������ ������� ���������.", "����������.");
        }

        /// <summary>
        /// ��������� ������� �� ������ "Cancel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelButton (object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close ();
        }

        /// <summary>
        /// ���������� �������� �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose (object sender, FormClosedEventArgs e)
        {
            m_DataSource.Disconnect ();
        }

    }
}