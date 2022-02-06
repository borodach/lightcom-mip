///////////////////////////////////////////////////////////////////////////////
//
//  File:           MiPInstaller.cs
//
//  Facility:       Плагин МиП для 2GiS
//
//
//  Abstract:       Исталлятор
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  04-05-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: MiPInstaller.cs $
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 5.04.07    Time: 8:09
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
* 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    [RunInstaller (true)]
    public partial class MiPInstaller: Installer
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public MiPInstaller ()
        {
            InitializeComponent ();
        }

        /// <summary>
        /// Инсталляция
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install (System.Collections.IDictionary stateSaver)
        {
            System.Runtime.InteropServices.RegistrationServices rs = new System.Runtime.InteropServices.RegistrationServices ();
            rs.RegisterAssembly (System.Reflection.Assembly.GetExecutingAssembly (), System.Runtime.InteropServices.AssemblyRegistrationFlags.None);
            base.Install (stateSaver);
        }

        /// <summary>
        /// Деинсталляция
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall (System.Collections.IDictionary savedState)
        {
            System.Runtime.InteropServices.RegistrationServices rs = new System.Runtime.InteropServices.RegistrationServices ();
            rs.UnregisterAssembly (System.Reflection.Assembly.GetExecutingAssembly ());
            base.Uninstall (savedState);
        }
    }
}