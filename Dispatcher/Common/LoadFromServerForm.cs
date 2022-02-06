///////////////////////////////////////////////////////////////////////////////
//
//  File:           LoadFromServerForm.cs
//
//  Facility:       Пользовательский интерфейс МиП
//                  
//
//
//  Abstract:       Диалог загрузки кэша с Web сервера.
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  21-Mar-2007
//
//  Copyright (C) OOO "ЛайтКом", 2006. Все права защищены.
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
    /// Диалог загрузки кэша с Web сервера
    /// </summary>
    public partial class LoadFromServerForm : Form
    {
        /// <summary>
        /// Ключ шифрования начального запроса к серверу
        /// </summary>
        public byte [] KeyToConnect
        {
            get { return this.m_DataSource.KeyToConnect; }
            set { this.m_DataSource.KeyToConnect = value; }
        }
                
        /// <summary>
        /// HTTP источник данных
        /// </summary>
        protected HTTPDataSource m_DataSource;
        
        /// <summary>
        /// Кэш перемещений объектов
        /// </summary>
        GPSDataCache m_Cache;
        
        /// <summary>
        /// Кэш перемещений объектов
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
        /// Конструктор
        /// <param name="cache">Кэш перемещений</param>
        /// </summary>
        public LoadFromServerForm (GPSDataCache cache)
        {
            m_DataSource = new HTTPDataSource ();
            InitializeComponent ();

            Cache = cache;
            DisableNetControls ();
        }

        /// <summary>
        /// Включение элементов управления, требующих наличия соединения с 
        /// сервером
        /// </summary>
        protected void EnableNetControls ()
        {
            readButton.Enabled = true;
            clientList.Enabled = true;
            dateTimePicker2.Enabled = true;
            dateTimePicker1.Enabled = true;
            connectionButton.Text = "Отсоединиться";
        }

        /// <summary>
        /// Выключение элементов управления, требующих наличия соединения с 
        /// сервером
        /// </summary>
        protected void DisableNetControls ()
        {
            readButton.Enabled = false;
            clientList.Items.Clear ();
            clientList.Enabled = false;
            dateTimePicker2.Enabled = false;
            dateTimePicker1.Enabled = false;
            connectionButton.Text = "Cоединиться";
        }

        /// <summary>
        /// Обработчик команды на установление.разрыв соединения с сервером
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectionButton_Click (object sender, System.EventArgs e)
        {
            if (m_DataSource.IsConnected ())
            {
                if (!m_DataSource.Disconnect ())
                {
                    MessageBox.Show ("При разрыве соединения произошла ошибка:\n" + m_DataSource.GetLastErrorText (),
                                     "Ошибка.");
                }
                else
                {
                    MessageBox.Show ("Соединение успешно разорвано.", "Информация.");
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
                MessageBox.Show ("При установлении соединения произошла ошибка:\n" + m_DataSource.GetLastErrorText (),
                    "Ошибка.");
                return;
            }

            string [] clients = null;
            if (!m_DataSource.GetMobileClientList (out clients))
            {
                MessageBox.Show ("При получении списка идентификаторов мобильных клиентов произошла ошибка:\n" + m_DataSource.GetLastErrorText (),
                    "Ошибка.");
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

            //MessageBox.Show ("Соединение успешно создано.", "Информация.");
            this.mainTabControl.SelectedTab = this.tabGetData;
        }

        /// <summary>
        /// Обработчик изменения выделенных элементов в списке мобильных 
        /// объектов
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
                MessageBox.Show ("При получении диапазона событий произошла ошибка:\n" + m_DataSource.GetLastErrorText (),
                    "Ошибка.");
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
        /// Обработчик команды чтения данных с сервера
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
                MessageBox.Show ("При загрузке GPS данных произошла ошибка:\n" + m_DataSource.GetLastErrorText (),
                    "Ошибка.");
                return;
            }

            this.DialogResult = DialogResult.OK;
            m_DataSource.Disconnect ();
            this.Close ();

            //MessageBox.Show ("Данные успешно загружены.", "Информация.");
        }

        /// <summary>
        /// Обрабтчик нажатия на кнопку "Cancel"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancelButton (object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close ();
        }

        /// <summary>
        /// Обработчик закрытия формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose (object sender, FormClosedEventArgs e)
        {
            m_DataSource.Disconnect ();
        }

    }
}