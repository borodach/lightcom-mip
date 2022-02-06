///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPoint.cs
//
//  Facility:       ����� �������� � 2Gis
//
//
//  Abstract:       ����� ��� �������� ����� ��������
//
//  Environment:    VC# 8.0
//
//  Author:         ������ �.�.
//
//  Creation Date:  03-11-2007
//
//  Copyright (C) OOO "�������", 2007. ��� ����� ��������.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: BindPoint.cs $
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:33
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 11.03.07   Time: 14:40
 * Created in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
* 
*/
using System;
using System.IO;
using LightCom.Common;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// ��������� � ����� ��������
    /// </summary>
    public class BindPoint:IDisposable
    {
        /// <summary>
        /// ������ �� ������
        /// </summary>
        MiPPlugin plugin;

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="plugin">������ �������</param>
        public BindPoint (MiPPlugin mipPlugin)
        {
            geoPoint = new GlobalPoint ();
            mapPoint = new GlobalPoint ();
            strDescription = string.Empty;
            enabled = true;
            plugin = mipPlugin;
        }
        
        #region "IDisposable implementation"
        ///
        /// Track whether Dispose has been called.
        /// 
        private bool disposed = false;

        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
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

        
        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// Managed and unmanaged resources can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">If disposing equals true, the method has 
        /// been called directly or indirectly by a user's code. 
        /// </param>
        private void Dispose (bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.                    

                    this.callout.Dispose ();
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
        ~BindPoint ()
        {
            Dispose (false);
        }
        #endregion;
        #region "Properties"        

        /// <summary>
        /// ������ ������ "�������"
        /// </summary>
        int �loseButtonIdx;

        /// <summary>
        /// ������ ������ "�������������"
        /// </summary>
        int enableButtonIdx;

        /// <summary>
        /// ������ ������ "��������"
        /// </summary>
        int propertiesButtonIdx;

        /// <summary>
        /// �������������� �����
        /// </summary>
        private GlobalPoint geoPoint;

        /// <summary>
        /// �������������� �����
        /// </summary>
        public GlobalPoint PointOnEarth
        {
            get
            {
                return geoPoint;
            }            
        }

        /// <summary>
        /// ����� �� �����
        /// </summary>
        private GlobalPoint mapPoint;

        /// <summary>
        /// ����� �� �����
        /// </summary>
        public GlobalPoint PointOnMap
        {
            get
            {
                return mapPoint;
            }
        }

        /// <summary>
        /// �������� ����� ��������
        /// </summary>
        private string strDescription;

        /// <summary>
        /// �������� ����� ��������
        /// </summary>
        public string Description
        {
            get
            {
                return this.strDescription;
            }
            set
            {
                this.strDescription = value;
            }
        }

        /// <summary>
        /// ������� ����������
        /// </summary>
        private bool enabled;
        /// <summary>
        /// ������� ����������
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }

        /// <summary>
        /// ����������� ����� �� �����
        /// </summary>
        private SafeComWrapper<GrymCore.Callout> callout = new SafeComWrapper<GrymCore.Callout> ();

        /// <summary>
        /// ������ ���������� ����� ��������
        /// </summary>
        private static Bitmap enabledBindPoint;

        /// <summary>
        /// ������ ����������� ����� ��������
        /// </summary>
        private static Bitmap disabledBindPoint;

        /// <summary>
        /// ������ ��������
        /// </summary>
        private static Bitmap closeBindPoint;

        /// <summary>
        /// ������ �������
        /// </summary>
        private static Bitmap propertiesBindPoint;
        
        static BindPoint ()
        {
            enabledBindPoint = Properties.Resources.CheckedButton;
            disabledBindPoint = Properties.Resources.UncheckedButton;
            closeBindPoint = Properties.Resources.CloseButton;
            propertiesBindPoint = Properties.Resources.PropertiesButton;
        }
#endregion;

        /// <summary>
        /// ��������� �������� �������
        /// </summary>
        /// <returns></returns>
        public string HTMLString
        {
            get
            {
                string desc = Description.Replace (System.Environment.NewLine, "<br>");
                desc = desc.Replace ("&", "&amp;");
                desc = desc.Replace (">", "&gt;");
                desc = desc.Replace ("<", "&lt;");
                desc = desc.Replace ("\"", "&quot;");
                desc = desc.Replace ("\r", "");

                //string result = string.Format ("<b>{0}</b><br/><b>GPS:</b>&nbsp;{1} {2}<br/><b>�����:</b>&nbsp;{3} {4}",
                string result = string.Format ("<b>{0}</b><br/><b>������:</b>&nbsp;{1}<br/><b>�������:</b>&nbsp;{2}",
                    desc,
                    FormatEarthCoordinate (geoPoint.y), 
                    FormatEarthCoordinate(geoPoint.x)
                    /*this.mapPoint.x, this.mapPoint.y*/);

                return result;
            }
        }

        /// <summary>
        /// ������������� �������������� ����������
        /// </summary>
        /// <param name="value">������������ �������� ����������</param>
        /// <returns>����� �� ��������� ����������</returns>
        public static string FormatEarthCoordinate (double value)
        {
            NumberFormatInfo provider = new NumberFormatInfo ();
            provider.NumberDecimalSeparator = ",";
            string result = value.ToString ("0.00000000####", provider);

            return result;
        }

        /// <summary>
        /// ������ ���������� �� ������
        /// </summary>
        /// <param name="value">������ � �����������</param>
        /// <returns>������������ �������� ����������</returns>
        public static double ParseEarthCoordinate (string value)
        {
            value = value.Trim ();
            value = value.Replace (",", ".");
            NumberFormatInfo provider = new NumberFormatInfo ();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = " ";

            double result = 0;
            try
            {
                result = Convert.ToDouble (value, provider);
                return result;
            }
            catch (Exception)
            {
            }

            return result;
        }

        /// <summary>
        /// ���������, �������� �� ������ �������� �����������
        /// </summary>
        /// <param name="value">������ � �����������</param>
        /// <returns>true, ���� ������ �������� �������� ����������</returns>
        public static bool IsValidEarthCoordinate (string value)
        {
            value = value.Trim ();
            value = value.Replace (",", ".");
            NumberFormatInfo provider = new NumberFormatInfo ();
            provider.NumberDecimalSeparator = ".";
            provider.NumberGroupSeparator = " ";

            double result = 0;
            try
            {
                result = Convert.ToDouble (value, provider);
                return true;
            }
            catch (Exception)
            {
            }          

            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// ���������� ������� � �����.
        /// </summary>
        /// <param name="stream">����� ��� ����������.</param>
        /// <returns>true, ���� �������� ��������� �������.</returns>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public bool SaveGuts (StreamWriter stream)
        {
            try
            {
                NumberFormatInfo provider = new NumberFormatInfo ();
                provider.NumberDecimalSeparator = ",";
                provider.NumberGroupSeparator = "";

                GlobalPoint global = this.PointOnEarth;
                GlobalPoint map = this.PointOnMap;
                stream.WriteLine (Convert.ToString (global.x, provider) + " " +
                    Convert.ToString (global.y, provider) + " " +
                    Convert.ToString (map.x, provider) + " " +
                    Convert.ToString (map.y, provider));
                stream.WriteLine (this.Description);
                stream.WriteLine (this.Enabled ? "1" : "0");
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// �������� ������� �� ������.
        /// </summary>
        /// <param name="stream">����� ��� ��������������.</param>
        /// <returns>true, ���� �������� ��������� �������.</returns>
        ///
        ////////////////////////////////////////////////////////////////////////////////

        public bool RestoreGuts (StreamReader stream)
        {
            Reset ();

            try
            {
                System.Text.RegularExpressions.Regex re =
                    new System.Text.RegularExpressions.Regex ("\\s+");

                NumberFormatInfo provider = new NumberFormatInfo ();
                provider.NumberDecimalSeparator = ",";
                provider.NumberGroupSeparator = "";

                string strLine = stream.ReadLine ();
                strLine = strLine.Trim ();
                string [] strTokens = re.Split (strLine);
                
                this.PointOnEarth.x = Convert.ToDouble (strTokens [0], provider);
                this.PointOnEarth.y = Convert.ToDouble (strTokens [1], provider);

                this.PointOnMap.x = Convert.ToDouble (strTokens [2], provider);
                this.PointOnMap.y = Convert.ToDouble (strTokens [3], provider);
                this.Description = stream.ReadLine ();
                this.Enabled = Convert.ToInt32 (stream.ReadLine ().Trim ()) != 0;
            }
            catch (Exception/* e*/)
            {
                Reset ();
                return false;
            }

            return true;
        }

        /// <summary>
        /// ������� ��� ������ �������.
        /// </summary>
        public void Reset ()
        {
            this.Description = string.Empty;
            this.PointOnEarth.x = this.PointOnEarth.y = 0;
            this.PointOnMap.x = this.PointOnMap.y = 0;
            this.Enabled = true;
        }

        /// <summary>
        /// ������� ����������� ������ �� �����
        /// </summary>
        public void RemoveCallout ()
        {
            if (callout.COMObject == null) return;

            IntPtr iUnknown = Marshal.GetIUnknownForObject (plugin.Map);
            GrymCore.IMapGraphics objMap = (GrymCore.IMapGraphics) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IMapGraphics));
            Marshal.Release (iUnknown);
            objMap.RemoveGraphic (callout.COMObject);

            callout.COMObject = null;
        }

        /// <summary>
        /// ������� ����������� ������ �� �����
        /// </summary>
        public void CreateCallout ()
        {
            RemoveCallout ();
            GrymCore.IGrymObjectFactory objFactory = plugin.BaseView.Factory;
            IntPtr iUnknown = Marshal.GetIUnknownForObject (plugin.Map);
            GrymCore.IMapGraphics objMap = (GrymCore.IMapGraphics) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IMapGraphics));
            Marshal.Release (iUnknown);

            GrymCore.IMapPoint objPoint = objFactory.CreateMapPoint (PointOnMap.x, PointOnMap.y);
            SafeComWrapper<GrymCore.IMapPoint> pt = new SafeComWrapper<GrymCore.IMapPoint> (objPoint);
            using (pt)
            {
                GrymCore.Callout objCallout = objMap.CreateCallout (objPoint, HTMLString, false);
                callout.COMObject = objCallout;

                //
                // Close button
                //

                GrymCore.IBitmap objBmpClose = objFactory.CreateBitmap (closeBindPoint.GetHbitmap ().ToInt32 (), false);
                this.�loseButtonIdx = callout.COMObject.AddButton (objBmpClose);

                //
                // Properties button
                //

                GrymCore.IBitmap objBmpProperties = objFactory.CreateBitmap (propertiesBindPoint.GetHbitmap ().ToInt32 (), false);
                this.propertiesButtonIdx = callout.COMObject.AddButton (objBmpProperties);

                //
                // Enable/disable button
                //

                GrymCore.IBitmap objBmpChecked = objFactory.CreateBitmap (enabledBindPoint.GetHbitmap ().ToInt32 (), false);
                GrymCore.IBitmap objBmpNormal = objFactory.CreateBitmap (disabledBindPoint.GetHbitmap ().ToInt32 (), false);
                this.enableButtonIdx = callout.COMObject.AddCheckButton (objBmpNormal, objBmpChecked, this.Enabled);

                callout.COMObject.OnButtonAction += new GrymCore._ICalloutEvents_OnButtonActionEventHandler (OnButtonAction);
                callout.COMObject.OnClose += new GrymCore._ICalloutEvents_OnCloseEventHandler (COMObject_OnClose);
                
                objMap.AddGraphic (callout.COMObject);
            }
        }

        void COMObject_OnClose ()
        {
            throw new Exception ("The method or operation is not implemented.");
        }
        /// <summary>
        /// ���������� ������ �� �������
        /// </summary>
        /// <param name="nIndex">������ ������</param>
        void OnButtonAction (int nIndex)
        {
            if (nIndex == this.�loseButtonIdx)
            {
                DialogResult res = MessageBox.Show (Properties.Resources.DeleteBindPointQuestion, 
                    Properties.Resources.PluginName, 
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Yes)
                {
                    RemoveCallout ();
                    plugin.RemoveBindPoint (this);
                }
            }
            else if (nIndex == this.enableButtonIdx)
            {
                this.enabled = callout.COMObject.get_ButtonChecked (this.enableButtonIdx);
                plugin.BindPointStateChanged (this);
            }
            else if (nIndex == this.propertiesButtonIdx)
            {
                if (ShowProperties ())
                {
                    this.CreateCallout ();
                    plugin.BindPointPropertiesChanged (this);
                }
            }
        }

        /// <summary>
        /// ���������� ���� �������� ������� ����� ��������.
        /// </summary>
        /// <returns>true, ���� ����� �������� ����������</returns>
        public bool ShowProperties ()
        {
            BindPointProperties dlg = new BindPointProperties (this);
            dlg.ShowDialog ();
            bool result = false;
            if (!dlg.IsOK) return false;
            if (dlg.Latitude != this.PointOnEarth.y)
            {
                this.PointOnEarth.y = dlg.Latitude;
                result = true;
            }

            if (dlg.Longitude != this.PointOnEarth.x)
            {
                this.PointOnEarth.x = dlg.Longitude;
                result = true;
            }

            string strDescription = dlg.Description.Trim ();
            
            if (strDescription != this.Description)
            {
                this.strDescription = strDescription;
                result = true;
            }

            return result;
        }
    }
}