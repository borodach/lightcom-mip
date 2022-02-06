///////////////////////////////////////////////////////////////////////////////
//
//  File:           AddBindPointCommand.cs
//
//  Facility:       Плагин MiP для 2Gis
//                  
//
//
//  Abstract:       Обработчик команды меню "Добавить точку привязки"
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  15-Mar-2007
//
//  Copyright (C) OOO "ЛайтКом", 2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: AddBindPointCommand.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:43
 * Created in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// Обработчик команды меню "Добавить точку привязки"
    /// </summary>
    public class AddBindPointCommand : GrymCore.ICommandAction, GrymCore.ICommandAppearance, GrymCore.ICommandPlacement, GrymCore.ICommandState
    {
        /// <summary>
        /// Ссылка на объект - плагин
        /// </summary>
        private MiPPlugin plugin;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pluginObject">Ссылка на объект - плагин</param>
        public AddBindPointCommand (MiPPlugin pluginObject)
        {
            plugin = pluginObject;
        }

        #region ICommandAction Members

        /// <summary>
        /// Обработчик команды меню
        /// </summary>
        /// <param name="pContext">Контекст команды</param>
        public void OnCommand (GrymCore.IContextBase pContext)
        {
            try
            {
                IntPtr iUnknown = Marshal.GetIUnknownForObject (pContext);
                GrymCore.IPopupMenuMapViewContext context = (GrymCore.IPopupMenuMapViewContext) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IPopupMenuMapViewContext));
                Marshal.Release (iUnknown);

                BindPoint bp = new BindPoint (plugin);
                bp.Description = string.Empty;
                bp.PointOnMap.x = context.MapPos.X;
                bp.PointOnMap.y = context.MapPos.Y;
                if (bp.ShowProperties ())
                {
                    bp.CreateCallout ();
                    plugin.AddBindPoint (bp);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show (e.ToString (), Properties.Resources.PluginName);
            }
        }

        #endregion

        #region ICommandAppearance Members

        /// <summary>
        /// Название скина
        /// </summary>
        public string ImageName
        {
            get { return ""; }
        }

        /// <summary>
        /// Название команды
        /// </summary>
        public string Title
        {
            get { return Properties.Resources.AddBindPointMenuItemName; }
        }

        /// <summary>
        /// Иконка команды
        /// </summary>
        /// <param name="bsType"></param>
        /// <returns></returns>
        public int get_DefaultBitmap (string bsType)
        {
            return 0;
        }

        #endregion

        #region ICommandPlacement Members

        /// <summary>
        /// Имя группы команд
        /// </summary>
        public string GroupName
        {
            get { return Properties.Resources.MenuGroupName; }
        }

        /// <summary>
        /// Позиция команды в меню
        /// </summary>
        public int Position
        {
            get { return 0; }
        }

        /// <summary>
        /// Приоритет команды
        /// </summary>
        public int Priority
        {
            get { return 100; }
        }

        #endregion

        #region ICommandState Members

        /// <summary>
        /// Признак доступости команды
        /// </summary>
        /// <param name="pContext"></param>
        /// <returns></returns>
        public bool get_Available (GrymCore.IContextBase pContext)
        {
            return plugin.AddBindPointEnabled ();
        }

        /// <summary>
        /// Отмечена ли команда
        /// </summary>
        /// <param name="pContext"></param>
        /// <returns></returns>
        public bool get_Checked (GrymCore.IContextBase pContext)
        {
            return false;
        }

        /// <summary>
        /// Включена ли команда
        /// </summary>
        /// <param name="pContext"></param>
        /// <returns></returns>
        public bool get_Enabled (GrymCore.IContextBase pContext)
        {
            return plugin.AddBindPointEnabled ();
        }

        #endregion
    }
}
