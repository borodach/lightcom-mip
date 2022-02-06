///////////////////////////////////////////////////////////////////////////////
//
//  File:           PlayerForm.cs
//
//  Facility:       Плагин MiP для 2Gis
//
//
//  Abstract:       Окно управления отображением перемещений
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
* $History: PlayerForm.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 3.04.07    Time: 21:53
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:26
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 18.03.07   Time: 14:24
 * Created in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
* 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LightCom.MiP.Common;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// Окно управления отображением перемещений
    /// </summary>
    public partial class PlayerForm: Form
    {
        /// <summary>
        /// Объект - плагин
        /// </summary>
        MiPPlugin plugin;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="objPlugin">Объект - плагин</param>
        public PlayerForm (MiPPlugin objPlugin)
        {
            InitializeComponent ();
            plugin = objPlugin;
            this.gpsPlayBackCtrl1.KeyToConnect = Properties.Resources.eKey;
        }

        /// <summary>
        /// Обработчик загрузки новых данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="cache"></param>
        private void OnGPSDataLoaded (LightCom.MiP.Dispatcher.Controls.GPSPlayBackCtrl sender, LightCom.MiP.Cache.GPSDataCache cache)
        {
            plugin.RemoveMobileObjects ();

            foreach (ObjectPositions op in cache.Storage)
            {
                MobileObjectGraphic mog = new MobileObjectGraphic (plugin);
                mog.Visible = false;
                mog.Tag = op.ClientId;
                plugin.MobileObjects.Add (mog);
            }

            plugin.CreateMobileObjects ();
        }

        /// <summary>
        /// Обработчик событий от окна управления воспроизведением
        /// </summary>
        /// <param name="source"></param>
        /// <param name="oldTime"></param>
        /// <param name="newEvent"></param>
        private void OnGPSDataChanged (LightCom.MiP.Dispatcher.Common.PlaybackControl source, DateTime oldTime, DateTime newEvent)
        {
            ObjectPosition [] positions = new ObjectPosition [source.Cache.Count];
            source.Cache.GetPositonsAtTime (positions, newEvent);

            for (int idx = 0; idx < source.Cache.Count; ++ idx)
            {
                if (idx >= plugin.MobileObjects.Count)
                    break;
                MobileObjectGraphic mog = plugin.MobileObjects [idx];
                mog.PointOnEarth.x = positions [idx].X;    
                mog.PointOnEarth.y = positions [idx].Y;

                plugin.GlobalToMap (mog.PointOnEarth, mog.Point);
                mog.Visible = true;
            }
            
            if (plugin.MobileObjects.Count > 0)
            {
                /*
                GrymCore.IMapRect rect = plugin.Map.GetMapVisibleRect ();
                double width = rect.Width;
                double height = rect.Height;

                double dx = plugin.MobileObjects [0].Point.X - (rect.MinX + rect.Width / 2);
                double dy = plugin.MobileObjects [0].Point.Y - (rect.MinY + rect.Height/ 2);

                rect.MinX += dx;
                rect.MinY += dy;
                rect.MaxX = rect.MinX + width;
                rect.MaxY = rect.MinY + height;

                plugin.Map.SetMapVisibleRect (rect);*/
                plugin.Map.Invalidate (true);
            }
            

        }

        /// <summary>
        /// Центрируется на первом мобильном объекте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShow_Click (object sender, EventArgs e)
        {
            if (plugin.MobileObjects.Count > 0)
            {
                /*
                GrymCore.IMapRect rect = plugin.Map.GetMapVisibleRect ();
                double width = rect.Width;
                double height = rect.Height;

                double dx = plugin.MobileObjects [0].Point.X - (rect.MinX + rect.Width / 2);
                double dy = plugin.MobileObjects [0].Point.Y - (rect.MinY + rect.Height/ 2);

                rect.MinX += dx;
                rect.MinY += dy;
                rect.MaxX = rect.MinX + width;
                rect.MaxY = rect.MinY + height;

                plugin.Map.SetMapVisibleRect (rect);*/
                plugin.Map.ShowPos (plugin.MobileObjects [0].Point, plugin.Device.Scale, true);
            }   
        }

        /// <summary>
        /// Обработчик попытки закрыть форму
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayerForm_FormClosing (object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }

      }
}