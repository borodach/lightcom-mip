///////////////////////////////////////////////////////////////////////////////
//
//  File:           MobileObjectGraphic.cs
//
//  Facility:       ������ MiP ��� 2Gis
//                  
//
//
//  Abstract:       ����� ������������ ������������� "��������� ������"
//
//
//  Environment:    MSVC# 2005
//
//  Author:         ������ �.�.
//
//  Creation Date:  16-Mar-2007
//
//  Copyright (C) OOO "�������", 2006. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: MobileObjectGraphic.cs $
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 18.03.07   Time: 14:24
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 17.03.07   Time: 11:02
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 16.03.07   Time: 23:04
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:43
 * Created in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
*/

using System;
using System.Collections.Generic;
using System.Text;
using LightCom.Common;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// ����� ������������ ������������� "��������� ������"
    /// </summary>
    public class MobileObjectGraphic: GrymCore.IGraphicCustomActive
    {
        /// <summary>
        /// ������ ������
        /// </summary>
        private MiPPlugin plugin;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="plugin">������ ������</param>
        public MobileObjectGraphic (MiPPlugin objPlugin)
        {
            globalPoint = new GlobalPoint ();
            this.plugin = objPlugin;
            internalPoint = new SafeComWrapper<GrymCore.IMapPoint> ();
            internalPoint.COMObject = plugin.BaseView.Factory.CreateMapPoint (0, 0);
            this.visible = true;
        }

        #region "IDisposable implementation"
        // Track whether Dispose has been called.
        private bool disposed = false;



        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose ()
        {
            Dispose (true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize (this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose (bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    internalPoint.COMObject = null;
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.                
            }
            disposed = true;
        }

        /// <summary>
        /// ����������
        /// </summary>
        ~MobileObjectGraphic ()
        {
            Dispose (false);
        }
        #endregion

        #region Properties

        /// <summary>
        /// ������� ��������� �������
        /// </summary>
        private bool visible;

        /// <summary>
        /// ������� ��������� �������
        /// </summary>
        public bool Visible
        {
            get { return this.visible; }
            set { this.visible = value; }
        }
        
        /// <summary>
        /// ������ ������� ������� (�������)
        /// </summary>
        private int maxWidth = 40000;
        private int minWidth = 8;

        /// <summary>
        /// ������ ������� ������� (�������)
        /// </summary>
        private int maxHeight = 60000;
        private int minHeight = 12;

        /// <summary>
        /// ���������� �������
        /// </summary>
        private SafeComWrapper<GrymCore.IMapPoint> internalPoint;

        /// <summary>
        /// ���������� �������
        /// </summary>
        public GrymCore.IMapPoint Point
        {
            get
            {
                return this.internalPoint.COMObject;
            }
        }

        /// <summary>
        /// �������������� ���������� �����
        /// </summary>
        private GlobalPoint globalPoint;

        /// <summary>
        /// �������������� ���������� �����
        /// </summary>
        public GlobalPoint PointOnEarth
        {
            get {return this.globalPoint;}
        }

        #endregion

        /// <summary>
        /// ��������� ������� � �������� ���������
        /// </summary>
        /// <param name="hDC">�������� ������������ ����������</param>
        /// <param name="x">���������� X � ��������</param>
        /// <param name="y">���������� Y � ��������</param>
        /// <param name="width">������ ������� � ��������</param>
        /// <param name="height">������ ������� � ��������</param>
        /// <param name="scale">�������</param>
        public void Draw (int hDC, int x, int y, int width, int height, int scale)
        {
            if (!this.Visible)
                return;
            Graphics g = Graphics.FromHdc (new IntPtr (hDC));

            int dx = width / 2;
            int dy = height / 2;

            Point [] points = { new Point (x - dx, y), new Point (x, y - dy), new Point (x + dx, y), new Point (x, y + dy), new Point (x - dx, y) };
            g.FillPolygon (Brushes.Red, points);
            g.DrawPolygon (Pens.Black, points);

            g.Dispose ();

        }

        /// <summary>
        /// ��������� ������ ������� ��� �������� ��������
        /// </summary>
        /// <param name="scale">�������</param>
        /// <param name="width">������ ������� ��� ��������� ��������</param>
        /// <param name="height">������ ������� ��� ��������� ��������</param>
        private void GetDevSize (int scale, out int width, out int height)
        {
            if (scale <= 1)
            {
                width = maxWidth;
                height = maxHeight;
                return;
            }

            width = maxWidth / scale;
            if (width < minWidth)
                width = minWidth;
            height = maxHeight / scale;
            if (height < minHeight)
                height = minHeight;
        }

        #region IGraphicCustomActive Members

        /// <summary>
        /// ��������� ������� �� �����.
        /// </summary>
        /// <param name="hDC"></param>
        public void Draw (int hDC)
        {
            GrymCore.IDevPoint devPoint = plugin.Device.MapToDevice (Point);
            int dx;
            int dy;
            this.GetDevSize (plugin.Device.Scale, out dx, out dy);

            Draw (hDC, devPoint.X, devPoint.Y, dx, dy, plugin.Device.Scale);

        }

        

        /// <summary>
        /// ���������� �������������� ������������� �������
        /// </summary>
        /// <param name="pDevice"></param>
        /// <returns></returns>
        public GrymCore.IDevRect GetBoundRect (GrymCore.IDevice pDevice)
        {

            GrymCore.IDevPoint devPoint = plugin.Device.MapToDevice (Point);
            //SafeComWrapper<GrymCore.IDevPoint> safePoint = new SafeComWrapper<GrymCore.IDevPoint> (devPoint);
            //using (safePoint)
            {
                int dx;
                int dy;
                this.GetDevSize (pDevice.Scale, out dx, out dy);
                dx /= 2;
                dy /= 2;

                GrymCore.IDevRect devRect = plugin.BaseView.Factory.CreateDevRect (devPoint.X - dx,
                                                                                   devPoint.Y - dy,
                                                                                   devPoint.X + dx,
                                                                                   devPoint.Y + dy);

                //SafeComWrapper<GrymCore.IDevRect> safeRect = new SafeComWrapper<GrymCore.IDevRect> (devRect);
                //using (safeRect)
                {
                    return devRect;
                }
            }
        }

        /// <summary>
        /// ���������, ����� �� �������� ����� � �������� �������
        /// </summary>
        /// <param name="pPos">����� �����</param>
        /// <returns>true, ���� ����� ����������� �������</returns>
        public bool IsMapPointInside (GrymCore.IMapPoint pPos)
        {
            GrymCore.IDevRect devRect = GetBoundRect (plugin.Device);
            GrymCore.IDevPoint devPoint = plugin.Device.MapToDevice (pPos);
            return (devPoint.X >= devRect.MinX) && (devPoint.X <= devRect.MaxX) &&
                   (devPoint.Y >= devRect.MinY) && (devPoint.Y <= devRect.MaxY);
            
        }

        /// <summary>
        /// ���������� �������� �������
        /// </summary>
        public void OnRemove ()
        {
            return;
        }

        /// <summary>
        /// ������ �������
        /// </summary>
        /// <param name="pDevice">����������� ����������</param>
        public void Print (GrymCore.IDevice pDevice)
        {
            GrymCore.IDevPoint devPoint = pDevice.MapToDevice (Point);
            int dx;
            int dy;
            this.GetDevSize (pDevice.Scale, out dx, out dy);

            Draw (pDevice.SafeDC, devPoint.X, devPoint.Y, dx, dy, pDevice.Scale);
        }

        /// <summary>
        /// ���������� ��������� Windows.
        /// </summary>
        /// <param name="message">��� ���������</param>
        /// <param name="wParam">wParam</param>
        /// <param name="lParam">lParam</param>
        /// <returns></returns>
        public bool ProcessMessage (int message, int wParam, int lParam)
        {
            return false;
        }

        /// <summary>
        /// ��� �������
        /// </summary>
        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }
        /// <summary>
        /// ��� �������
        /// </summary>
        private string tag = String.Empty;

        #endregion
    }
}
