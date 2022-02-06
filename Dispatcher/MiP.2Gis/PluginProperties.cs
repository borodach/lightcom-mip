///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPointProperties.cs
//
//  Facility:       Плагин MiP для 2Gis
//
//
//  Abstract:       Диалог свойств плагина
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
    /// Диалог свойств плагин
    /// </summary>
    public partial class MobileObjectList: Form
    {

        /// <summary>
        /// Объект - плагин
        /// </summary>
        private MiPPlugin plugin;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="plugin">Объект - плагин</param>
        public MobileObjectList (MiPPlugin objPlugin)
        {
            InitializeComponent ();

            plugin = objPlugin;
            this.cbBindPointsEnabled.Checked = plugin.IsBindPointsVisible;
            //this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Обработчик кнопки OK
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOK (object sender, EventArgs e)
        {
            plugin.IsBindPointsVisible = this.cbBindPointsEnabled.Checked;
            this.DialogResult = DialogResult.OK;
            //this.Close ();
        }

        /// <summary>
        /// Обработчик кнопки Cancel
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