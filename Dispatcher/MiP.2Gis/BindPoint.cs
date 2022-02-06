///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPoint.cs
//
//  Facility:       Точки привязки в 2Gis
//
//
//  Abstract:       Класс для хранения точек привязки
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  03-11-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
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
    /// Инфрмация о точке привязки
    /// </summary>
    public class BindPoint:IDisposable
    {
        /// <summary>
        /// Ссылка на плагин
        /// </summary>
        MiPPlugin plugin;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="plugin">Объект плагина</param>
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
        /// Деструктор
        /// </summary>
        ~BindPoint ()
        {
            Dispose (false);
        }
        #endregion;
        #region "Properties"        

        /// <summary>
        /// Индекс кнопки "закрыть"
        /// </summary>
        int сloseButtonIdx;

        /// <summary>
        /// Индекс кнопки "редактировать"
        /// </summary>
        int enableButtonIdx;

        /// <summary>
        /// Индекс кнопки "свойства"
        /// </summary>
        int propertiesButtonIdx;

        /// <summary>
        /// Географическая точка
        /// </summary>
        private GlobalPoint geoPoint;

        /// <summary>
        /// Географическая точка
        /// </summary>
        public GlobalPoint PointOnEarth
        {
            get
            {
                return geoPoint;
            }            
        }

        /// <summary>
        /// Точка на карте
        /// </summary>
        private GlobalPoint mapPoint;

        /// <summary>
        /// Точка на карте
        /// </summary>
        public GlobalPoint PointOnMap
        {
            get
            {
                return mapPoint;
            }
        }

        /// <summary>
        /// Описание точки привязки
        /// </summary>
        private string strDescription;

        /// <summary>
        /// Описание точки привязки
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
        /// Признак активности
        /// </summary>
        private bool enabled;
        /// <summary>
        /// Признак активности
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
        /// Графическая метка на карте
        /// </summary>
        private SafeComWrapper<GrymCore.Callout> callout = new SafeComWrapper<GrymCore.Callout> ();

        /// <summary>
        /// Кнопка включенной точки привязки
        /// </summary>
        private static Bitmap enabledBindPoint;

        /// <summary>
        /// Кнопка выключенной точки привязки
        /// </summary>
        private static Bitmap disabledBindPoint;

        /// <summary>
        /// Кнопка закрытия
        /// </summary>
        private static Bitmap closeBindPoint;

        /// <summary>
        /// Кнопка свойств
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
        /// Текстовое описанеи объекта
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

                //string result = string.Format ("<b>{0}</b><br/><b>GPS:</b>&nbsp;{1} {2}<br/><b>Карта:</b>&nbsp;{3} {4}",
                string result = string.Format ("<b>{0}</b><br/><b>Широта:</b>&nbsp;{1}<br/><b>Долгота:</b>&nbsp;{2}",
                    desc,
                    FormatEarthCoordinate (geoPoint.y), 
                    FormatEarthCoordinate(geoPoint.x)
                    /*this.mapPoint.x, this.mapPoint.y*/);

                return result;
            }
        }

        /// <summary>
        /// Форматировние географической координаты
        /// </summary>
        /// <param name="value">Вещественное значение координаты</param>
        /// <returns>Срока со значением координаты</returns>
        public static string FormatEarthCoordinate (double value)
        {
            NumberFormatInfo provider = new NumberFormatInfo ();
            provider.NumberDecimalSeparator = ",";
            string result = value.ToString ("0.00000000####", provider);

            return result;
        }

        /// <summary>
        /// Чтение координаты из строки
        /// </summary>
        /// <param name="value">Строка с координатой</param>
        /// <returns>Вещественное значение координаты</returns>
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
        /// Проверяет, является ли строка валидной координатой
        /// </summary>
        /// <param name="value">Строка с координатой</param>
        /// <returns>true, если строка содержит валидную координату</returns>
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
        /// Сохранение объекта в поток.
        /// </summary>
        /// <param name="stream">Поток для сохранения.</param>
        /// <returns>true, если операция выполнена успешно.</returns>
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
        /// Загрузка объекта из потока.
        /// </summary>
        /// <param name="stream">Поток для восстановления.</param>
        /// <returns>true, если операция выполнена успешно.</returns>
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
        /// Очищает все данные объекта.
        /// </summary>
        public void Reset ()
        {
            this.Description = string.Empty;
            this.PointOnEarth.x = this.PointOnEarth.y = 0;
            this.PointOnMap.x = this.PointOnMap.y = 0;
            this.Enabled = true;
        }

        /// <summary>
        /// Удаляет графический объект на карте
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
        /// Создает графический объект на карте
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
                this.сloseButtonIdx = callout.COMObject.AddButton (objBmpClose);

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
        /// Обработчик кликов на кнопках
        /// </summary>
        /// <param name="nIndex">Индекс кнопки</param>
        void OnButtonAction (int nIndex)
        {
            if (nIndex == this.сloseButtonIdx)
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
        /// Отображает окно настроек свойств точки привязки.
        /// </summary>
        /// <returns>true, если точка привязки изменилась</returns>
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