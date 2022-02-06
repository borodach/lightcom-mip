///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPointProperties.cs
//
//  Facility:       Плагин MiP для 2Gis
//
//
//  Abstract:       Диалог свойств точи привязки
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  03-18-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
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
    /// Диалог свойств точи привязки
    /// </summary>
    public partial class BindPointProperties : Form
    {
        /// <summary>
        /// Была ли форма закрыта кнопкой OK
        /// </summary>
        private bool bOK;

        /// <summary>
        /// Была ли форма закрыта кнопкой OK
        /// </summary>
        public bool IsOK
        {
            get { return this.bOK; }
        }

        /// <summary>
        /// Описание точки привязки
        /// </summary>
        public string Description
        {
            get 
            {
                return this.txtDescription.Text;
            }
        }

        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude
        {
            get {return BindPoint.ParseEarthCoordinate (this.txtLatitude.Text);}
        }

        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude
        {
            get { return BindPoint.ParseEarthCoordinate (this.txtLongitude.Text); }
        }        

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="bp">Точка привязки</param>
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
        /// Обработчик кнопки OK
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
        /// Обработчик кнопки Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel (object sender, EventArgs e)
        {
            bOK = false;
            this.Close ();
        }

        /// <summary>
        /// Обработчик нажатий на клавиатуру
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