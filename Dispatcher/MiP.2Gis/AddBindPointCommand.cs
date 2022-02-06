///////////////////////////////////////////////////////////////////////////////
//
//  File:           AddBindPointCommand.cs
//
//  Facility:       ������ MiP ��� 2Gis
//                  
//
//
//  Abstract:       ���������� ������� ���� "�������� ����� ��������"
//
//
//  Environment:    MSVC# 2005
//
//  Author:         ������ �.�.
//
//  Creation Date:  15-Mar-2007
//
//  Copyright (C) OOO "�������", 2006. ��� ����� ��������.
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
    /// ���������� ������� ���� "�������� ����� ��������"
    /// </summary>
    public class AddBindPointCommand : GrymCore.ICommandAction, GrymCore.ICommandAppearance, GrymCore.ICommandPlacement, GrymCore.ICommandState
    {
        /// <summary>
        /// ������ �� ������ - ������
        /// </summary>
        private MiPPlugin plugin;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="pluginObject">������ �� ������ - ������</param>
        public AddBindPointCommand (MiPPlugin pluginObject)
        {
            plugin = pluginObject;
        }

        #region ICommandAction Members

        /// <summary>
        /// ���������� ������� ����
        /// </summary>
        /// <param name="pContext">�������� �������</param>
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
        /// �������� �����
        /// </summary>
        public string ImageName
        {
            get { return ""; }
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        public string Title
        {
            get { return Properties.Resources.AddBindPointMenuItemName; }
        }

        /// <summary>
        /// ������ �������
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
        /// ��� ������ ������
        /// </summary>
        public string GroupName
        {
            get { return Properties.Resources.MenuGroupName; }
        }

        /// <summary>
        /// ������� ������� � ����
        /// </summary>
        public int Position
        {
            get { return 0; }
        }

        /// <summary>
        /// ��������� �������
        /// </summary>
        public int Priority
        {
            get { return 100; }
        }

        #endregion

        #region ICommandState Members

        /// <summary>
        /// ������� ���������� �������
        /// </summary>
        /// <param name="pContext"></param>
        /// <returns></returns>
        public bool get_Available (GrymCore.IContextBase pContext)
        {
            return plugin.AddBindPointEnabled ();
        }

        /// <summary>
        /// �������� �� �������
        /// </summary>
        /// <param name="pContext"></param>
        /// <returns></returns>
        public bool get_Checked (GrymCore.IContextBase pContext)
        {
            return false;
        }

        /// <summary>
        /// �������� �� �������
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
