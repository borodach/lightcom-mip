///////////////////////////////////////////////////////////////////////////////
//
//  File:           MainForm.cs
//
//  Facility:       
//
//
//  Abstract:
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  31-10-2005
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: MainForm.cs $
 * 
 * *****************  Version 15  *****************
 * User: Sergey       Date: 10.07.07   Time: 7:59
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 14  *****************
 * User: Sergey       Date: 25.06.07   Time: 8:29
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 13  *****************
 * User: Sergey       Date: 3.04.07    Time: 21:53
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 12  *****************
 * User: Sergey       Date: 2.04.07    Time: 21:13
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 11  *****************
 * User: Sergey       Date: 4.03.07    Time: 17:02
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 10  *****************
 * User: Sergey       Date: 4.03.07    Time: 10:51
 * Updated in $/gps/EndPoint
 * 
 * *****************  Version 9  *****************
 * User: Sergey       Date: 19.11.06   Time: 23:40
 * Updated in $/gps/EndPoint
 * Добавлена поддержка кэширования
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 19.11.06   Time: 0:22
 * Updated in $/gps/EndPoint
 * Реализовано автоцентрирование на карте и добавлена возможность
 * масштабирования
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 18.11.06   Time: 21:20
 * Updated in $/gps/EndPoint
 * Полностью реализовано вычисление параметров простого алгоритма привязки
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 18.11.06   Time: 19:09
 * Updated in $/gps/EndPoint
 * Добавлена поддержка сохранения сведений о положении объекта в файл
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:39
 * Updated in $/gps/EndPoint
 * 
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.Gps;

namespace LightCom.MiP.CEClient
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        internal static string m_strLicenseOwner = "";
        internal static string m_strLicenseNumber = "";
        internal static string m_strLicenseDistributorName = "";
        internal static string m_strLicenseDistributorArea = "";

        public MainForm ()
        {
            if (ObjectsCreator.instance == null) return;
                        
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent ();

            Bitmap pict = new Bitmap (gpsState.Width, gpsState.Height);
            Graphics.FromImage (pict).Clear (Color.Green);
            this.gpsState.Image = pict;

            pict = new Bitmap (httpState.Width, httpState.Height);
            Graphics.FromImage (pict).Clear (Color.Green);
            this.httpState.Image = pict;

            
        }

        GPSListener.State m_HTTPState = GPSListener.State.OK;
        protected void OnPublisherStateChanged (GPSListener source,
            GPSListener.State state)
        {
            if (m_bClosing)
            {
                return;
            }

            lock (this)
            {
                m_HTTPState = state;
                this.Invoke (new EventHandler (this.OnHTTPPublisherStateChangedHandler));
            }
        }
        protected void OnHTTPPublisherStateChangedHandler (Object sender, EventArgs e)
        {

            GPSTransmitter.State state = m_HTTPState;
            Color color = Color.Yellow;
            if (state == GPSTransmitter.State.OK)
            {
                color = Color.Green;
            }

            if (state == GPSTransmitter.State.Error)
            {
                color = Color.Red;
            }

            if (state == GPSTransmitter.State.Warning)
            {
                color = Color.Yellow;
            }

            Bitmap pict = new Bitmap (httpState.Width, httpState.Height);
            Graphics.FromImage (pict).Clear (color);
            this.httpState.Image = pict;

        }


        GPSListener.State m_GPSReceiverState = GPSListener.State.OK;
        protected void OnGPSReceiverStateChanged (GPSListener source,
            GPSListener.State state)
        {
            if (m_bClosing)
            {
                return;
            }

            lock (this)
            {

                m_GPSReceiverState = state;
                this.Invoke (new EventHandler (this.OnGPSReceiverStateChangedHandler));
            }
        }

        protected void OnGPSReceiverStateChangedHandler (Object sender, EventArgs e)
        {
            GPSTransmitter.State state = m_GPSReceiverState;
            Color color = Color.Yellow;
            if (state == GPSTransmitter.State.OK)
            {
                color = Color.Green;
            }

            if (state == GPSTransmitter.State.Error)
            {
                color = Color.Red;
            }

            if (state == GPSTransmitter.State.Warning)
            {
                color = Color.Yellow;
            }

            Bitmap pict = new Bitmap (gpsState.Width, gpsState.Height);
            Graphics.FromImage (pict).Clear (color);
            this.gpsState.Image = pict;
        }

        void DrawPosition (Graphics g, int x, int y, Pen pen)
        {
            int nSize = 3;
            g.DrawLine (pen, x - nSize, y - nSize, x + nSize,  y + nSize);
            g.DrawLine (pen, x + nSize, y - nSize, x - nSize,  y + nSize);
            y++;
            g.DrawLine (pen, x - nSize, y - nSize, x + nSize,  y + nSize);
            g.DrawLine (pen, x + nSize, y - nSize, x - nSize,  y + nSize);
            y -= 2;
            g.DrawLine (pen, x - nSize, y - nSize, x + nSize,  y + nSize);
            g.DrawLine (pen, x + nSize, y - nSize, x - nSize,  y + nSize);
            y++; x--;
            g.DrawLine (pen, x - nSize, y - nSize, x + nSize,  y + nSize);
            g.DrawLine (pen, x + nSize, y - nSize, x - nSize,  y + nSize);
            x += 2;
            g.DrawLine (pen, x - nSize, y - nSize, x + nSize,  y + nSize);
            g.DrawLine (pen, x + nSize, y - nSize, x - nSize,  y + nSize);
        }

        void ShowMapOld (double fLatitude, double fLongitude)
        {
            GlobalPoint global = new GlobalPoint ();
            global.x = fLongitude;
            global.y = fLatitude;
            MapPoint map = new MapPoint ();
            MapPoint another_map = new MapPoint ();

            this.m_simple_mapper.GlobalToMap (global, map);
            this.m_mapper.GlobalToMap (global, another_map);

            int x;
            int y;

            if (this.bUseSimpleFirst)
            {
                x = map.x;
                y = map.y;
            }
            else
            {
                x = another_map.x;
                y = another_map.y;
            }                          

            int nWidth = 230;
            int nHeight = 200;

            int x0 = x - (x % nWidth);
            int y0 = y - (y % nHeight);
            String strFileName = string.Format (Utils.ExeDirectory + "\\jpg\\lux{0}y{1}z0.jpg",
                                        x0,
                                        y0);
            try
            {
                Bitmap bmp = new Bitmap (strFileName);
                Graphics g = Graphics.FromImage (bmp);


                Pen pen = new Pen (Color.Blue);
                DrawPosition (g, map.x - x0, map.y - y0, pen);
                pen = new Pen (Color.Red);
                DrawPosition (g,
                              another_map.x - x0,
                              another_map.y - y0,

                              pen);

                this.mapBox.Image = bmp;

            }
            catch (Exception)
            {
            }
        }
        

        FileMapBuilder mapBuilder = null;
        double scaleX = 1;
        double scaleY = 1;


        void ShowMap (double fLatitude, double fLongitude)
        {
            //
            // Преобразуем географические координаты в кроординаты на карте
            //

            GlobalPoint global = new GlobalPoint ();
            global.x = fLongitude;
            global.y = fLatitude;
            MapPoint map = new MapPoint ();
            MapPoint another_map = new MapPoint ();

            this.m_simple_mapper.GlobalToMap (global, map);
            this.m_mapper.GlobalToMap (global, another_map);

            int x;
            int y;
            int dXSimple = 0, dYSimple = 0;
            int dXBind = 0, dYBind = 0;

            //
            // Выбираем точку центрирования
            //

            if (this.bUseSimpleFirst)
            {
                x = map.x;
                y = map.y;

                dXBind = map.x - another_map.x;
                dYBind = map.y - another_map.y; 
            }
            else
            {
                x = another_map.x;
                y = another_map.y;

                dXSimple = another_map.x - map.x;
                dYSimple = another_map.y - map.y; 
            }
            
            //
            // Рисуем картинку
            //

            try
            {
                //
                // Готовим bitmap
                //

                int nWindowWidth = mapBox.Width;
                int nWindowHeight = mapBox.Height;

                //int x0 = x - (x % nWindowWidth);
                //int y0 = y - (y % nWindowHeight);

                Bitmap bmp = new Bitmap (mapBox.Width, mapBox.Height);
                Graphics g = Graphics.FromImage (bmp);                
                g.FillRectangle (new SolidBrush (Color.White), 0, 0, nWindowWidth, nWindowHeight);

                //
                // Рисуем карту
                //                

                Rectangle graphicsRect = new Rectangle (0, 0, nWindowWidth, nWindowHeight);
                int nMapWidth = (int) Math.Round (graphicsRect.Width * scaleX);
                int nMapHeight = (int) Math.Round (graphicsRect.Height * scaleY);  
                int nMapX = x - nMapWidth / 2; 
                int nMapY = y - nMapHeight / 2;
                Rectangle mapRect = new Rectangle (nMapX,
                                                   nMapY,
                                                   nMapWidth,
                                                   nMapHeight);

                mapBuilder.Draw (g, graphicsRect, mapRect);

                //
                // Рисуем положение на карте
                //

                int cx = nWindowWidth / 2;
                int cy = nWindowHeight / 2;
                
                Pen pen = new Pen (Color.Blue);
                DrawPosition (g,
                              cx + (int) Math.Round (dXSimple / scaleX),
                              cy + (int) Math.Round (dYSimple / scaleY), 
                              pen);
                
                 pen = new Pen (Color.Red);
                 DrawPosition (g,
                               cx + (int) Math.Round (dXBind / scaleX),
                               cy + (int) Math.Round (dYBind / scaleY), 
                               pen);
                
                this.mapBox.Image = bmp;

            }
            catch (Exception)
            {
            }
        }

        ///
        /// <summary>
        /// Последние полученные от GPS приемника данные
        /// </summary>
        ///

        LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION m_LastPosition;
        bool m_bClosing = false;
        
        /// <summary>
        /// Поучение данных от GPS приемника
        /// </summary>
        /// <param name="source">Источник данных</param>
        /// <param name="fLatitude">Широта</param>
        /// <param name="fLongitude">Долгота</param>
        /// <param name="fSpeed">Скорость</param>
        /// <param name="fCourse">Истинное направление курса в градусах </param>
        /// <param name="strRawGPS">Необработанные данные, полученные от приемника GPS</param>
        public void UpdatePosition (GPSListener source,
            LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION position)
        {
            if (m_bClosing)
            {
                return;
            }
            lock (this)
            {
                if (position != null)
                {
                    if (0 == position.dwValidFields)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }

                m_LastPosition = position;

                this.Invoke (new EventHandler (this.UpdatePositionHandler));
            }
        }

        /// 
        /// <summary>
        /// Обработка GPS координат
        /// </summary>
        ///

        public void UpdatePositionHandler (Object sender, EventArgs e)
        {
            //MainForm.form.ShowMap (55.00824, 82.93559);  
            string strText = "";
            if (null != m_LastPosition && 0 != m_LastPosition.dwValidFields)
            {
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LATITUDE)!= 0 &&
                    (m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LONGITUDE) != 0)
                {
                    MainForm.form.ShowMap (m_LastPosition.dblLatitude, m_LastPosition.dblLongitude);
                }
                
                
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_UTC_TIME) != 0)
                {
                    strText += string.Format ("ВРЕМЯ: {0}\r\n", ((DateTime) (m_LastPosition.stUTCTime)).ToLocalTime().ToString ());
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LATITUDE) != 0)
                {
                    strText += string.Format ("Ш: {0}\r\n", Math.Round (m_LastPosition.dblLatitude, 10));
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LONGITUDE) != 0)
                {
                    strText += string.Format ("Д: {0}\r\n", Math.Round (m_LastPosition.dblLongitude, 10));
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_SPEED) != 0)
                {
                    strText += string.Format ("СКОРОСТЬ: {0} км/ч\r\n", Math.Round (m_LastPosition.speedKMH, 1));
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_HEADING) != 0)
                {
                    strText += string.Format ("КУРС: {0}\r\n", Math.Round (m_LastPosition.flHeading, 1));
                }
                
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_SATELLITES_IN_VIEW) != 0)
                {
                    strText += string.Format ("СПУТНИКИ: {0}\r\n", m_LastPosition.dwSatellitesInView);
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_HORIZONTAL_DILUTION_OF_PRECISION) != 0)
                {
                    strText += string.Format ("HDOP: {0}\r\n", m_LastPosition.flHorizontalDilutionOfPrecision);
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_VERTICAL_DILUTION_OF_PRECISION) != 0)
                {
                    strText += string.Format ("VDOP: {0}\r\n", m_LastPosition.flVerticalDilutionOfPrecision);
                }
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_POSITION_DILUTION_OF_PRECISION) != 0)
                {
                    strText += string.Format ("PDOP: {0}\r\n", m_LastPosition.flPositionDilutionOfPrecision);
                }
                if (((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_ALTITUDE_WRT_SEA_LEVEL) != 0) &&
                    ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_ALTITUDE_WRT_ELLIPSOID) != 0))
                {
                    strText += string.Format ("ВЫСОТА НАД МОРЕМ: {0}м\r\n", m_LastPosition.flAltitudeWRTSeaLevel);
                    strText += string.Format ("ВЫСОТА НАД WGS84: {0}м\r\n", m_LastPosition.flAltitudeWRTEllipsoid);
                }
            }
            else
            {
                strText = "Нет данных\r\n";
            }
            MainForm.form.positionInfo.Text = strText;
        }
        

        public static MainForm form;

        bool bUseSimpleFirst = true;
        bool bUseFilePublisher = true;

        static void Main () 
        {
            //LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION pos = new LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION ("$GPRMC,180258,A,4056.4604,N,07216.9673,W,3.965,263.0,130800,14.8,W*41");
            //string rmc = pos.RMC;
            //LightCom.Gps.GPSReader.IsGPSDataValid (rmc);

            try
            {
                if (!MainForm.TestCode ())
                {
                    MessageBox.Show ("Данная версия программы не имеет лицензии.",
                                     "Проверка лицензии", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }


            if (IntPtr.Zero == LightCom.WinCE.API.CreateMutex ("{69EC4BF5-05FB-49cf-9471-0E858D363B0E}") ||
                System.Runtime.InteropServices.Marshal.GetLastWin32Error () == LightCom.WinCE.API.ERROR_ALREADY_EXISTS)
            {
                return;
            }

            LightCom.WinCE.API.CeRunAppAtEvent (LightCom.Common.Utils.ExePath,
                LightCom.WinCE.API.NotificationEventType.NOTIFICATION_EVENT_NONE);
            LightCom.WinCE.API.CeRunAppAtEvent (LightCom.Common.Utils.ExePath,
                LightCom.WinCE.API.NotificationEventType.NOTIFICATION_EVENT_WAKEUP);
            
            form = new MainForm ();

            double gx = 0, gy = 0;

            try
            {
                FileStream fs = new FileStream (Utils.ExeDirectory + 
                                                "\\binding.txt", 
                                                FileMode.Open);
                form.m_mapper.RestoreGuts (fs);
                form.m_mapper.CalculatePixelSize (out gx, out gy);
                fs.Close ();
            }
            catch (Exception  e)
            {
                MessageBox.Show (e.ToString (), Utils.ExeDirectory + 
                                                "\\binding.txt");
            }

            try
            {
                FileStream fs = new FileStream (Utils.ExeDirectory + "\\effective_binding.txt", 
                                                FileMode.Create);
                form.m_mapper.SaveGuts (fs);
                fs.Close ();
            }
            catch (Exception  e)
            {
                MessageBox.Show (e.ToString (), Utils.ExeDirectory + "\\effective_binding.txt");
            }

            form.LoadSettings (gx, gy);            

            //
            // Сохраняем эффективные настройки
            //

            XMLSetingsStorage stg = new XMLSetingsStorage ();
            stg.FileName = Utils.ExeDirectory + "\\effective_settings.txt";
            SettingsManager.instance.Save (stg);
            form.SaveSettings (stg);
            stg.Flush ();

            if (form.bUseFilePublisher)
            {
                ObjectsCreator.transmitter.Publisher = FilePublisher.instance;
                form.publisherLabel.Text = "ФАЙЛ:";
            }
            else
            {
                ObjectsCreator.transmitter.Publisher = HTTPPublisher.instance;
                form.publisherLabel.Text = "HTTP:";
            }

            form.OnStartStop (form, null);

            Application.Run (form);
        }

        internal void LoadSettings (double gx, double gy)
        {
            const string strMapCategory = "Map";
            SettingsStorage stg = SettingsManager.instance.Storage;
            int nUseSimpleFirst;
            double val;
            stg.Read (strMapCategory, "Gx", out val, gx);
            m_simple_mapper.MapX = val;
            stg.Read (strMapCategory, "Gy", out val, gy);
            m_simple_mapper.MapY = val;
            stg.Read (strMapCategory, "dx", out val, m_mapper.dx);
            m_simple_mapper.dx = val;
            stg.Read (strMapCategory, "dy", out val, m_mapper.dy);
            m_simple_mapper.dy = val;
            stg.Read (strMapCategory, "UseSimpleAlgorithmFirst", out nUseSimpleFirst, 0);
            bUseSimpleFirst = (0 != nUseSimpleFirst);

            int nUseFilePublisher;
            stg.Read ("Publisher", "UseFilePublisher", out nUseFilePublisher, 1);
            bUseFilePublisher = (0 != nUseFilePublisher);

            int cacheSize;
            stg.Read ("MapCache", "Size", out cacheSize, 16);
            mapBuilder = new FileMapBuilder (cacheSize);

            string strVal = string.Empty;
            stg.Read (LightCom.MiP.CEClient.GPSTransmitter.storageCategery, "Port", out strVal, null);
            if (! string.IsNullOrEmpty (strVal))
            {
                ObjectsCreator.transmitter = new LightCom.MiP.CEClient.COMTransmitter (HTTPPublisher.instance);
                ObjectsCreator.transmitter.Load (stg);
            }

            ObjectsCreator.transmitter.PublisherStateChanged +=
                new GPSListener.StateChangedEventHandler (this.OnPublisherStateChanged);
            ObjectsCreator.transmitter.GPSReceiverStateChanged +=
                new GPSListener.StateChangedEventHandler (this.OnGPSReceiverStateChanged);
            ObjectsCreator.transmitter.GPSDataRead += this.UpdatePosition;
        }

        internal void SaveSettings (SettingsStorage stg)
        {
            const string strMapCategory = "Map";
            stg.Write (strMapCategory, "Gx", m_simple_mapper.MapX);
            stg.Write (strMapCategory, "Gy", m_simple_mapper.MapY);
            stg.Write (strMapCategory, "dx", m_simple_mapper.dx);
            stg.Write (strMapCategory, "dy", m_simple_mapper.dy);
            stg.Write (strMapCategory, "UseSimpleAlgorithmFirst", bUseSimpleFirst ? 1 : 0);
            stg.Write ("Publisher", "UseFilePublisher", bUseFilePublisher ? 1 : 0);
            stg.Write ("MapCache", "Size", mapBuilder.Cache.CacheSize);
        }

        private void MainForm_Closed (object sender, System.EventArgs e)
        {
            lock (this)
            {
                m_bClosing = true;
            }

            this.positionInfo.Text = "Завершение работы ...";
            ObjectsCreator.transmitter.Stop (500);
        }

        private void button6_Click (object sender, System.EventArgs e)
        {
            LightCom.WinCE.API.CeRunAppAtEvent (Utils.ExePath,
                LightCom.WinCE.API.NotificationEventType.NOTIFICATION_EVENT_NONE);
            LightCom.WinCE.API.CeRunAppAtEvent (Utils.ExePath,
                LightCom.WinCE.API.NotificationEventType.NOTIFICATION_EVENT_WAKEUP);
                
        }

        private void OnNoAuto (object sender, System.EventArgs e)
        {
            LightCom.WinCE.API.CeRunAppAtEvent (Utils.ExePath,
                LightCom.WinCE.API.NotificationEventType.NOTIFICATION_EVENT_NONE);
        }
        
        private void button5_Click (object sender, System.EventArgs e)
        {
            try
            {
                StreamWriter fs = new StreamWriter (Utils.ExeDirectory + "\\positions.txt", 
                    true, 
                    new System.Text.ASCIIEncoding ());
                fs.WriteLine (this.positionInfo.Text);
                fs.Close ();
            }
            catch (Exception )
            {
            }

        }

        //private void OnClose (object sender, System.EventArgs e)
        //{
        //    lock (this)
        //    {
        //        m_bClosing = true;
        //    }

        //    this.positionInfo.Text = "Завершение работы ...";
        //    ObjectsCreator.transmitter.Stop (0);
        //    this.Close ();
        //}
        
        //int n = 0;            
        private void OnStartStop (object sender, System.EventArgs e)
        {
            /*
            double [] pt = {
                               55.001734, 82.927657, 
                               55.020035, 82.891879, 
                               55.007834, 82.916235, 
                               55.052264, 82.896723, 
                               55.055980, 82.892176, 
                               55.058461, 82.911924, 
                               55.065476, 82.930040, 
                               55.038181, 82.943451, 
                               55.041158, 82.963307, 
                               55.048540, 82.961125, 
                               55.029675, 82.989270, 
                               55.037039, 82.978424, 
                               55.026741, 82.922017
                           };
            n += 2;
            if (n >= pt.Length) n = 0;
            this.ShowMap (pt [n], pt [n + 1]);
            */
            if (ObjectsCreator.transmitter.IsAlive ())
            {
                ObjectsCreator.transmitter.Stop (2000);
            }
            else
            {
                ObjectsCreator.transmitter.Start ();
            }

            if (ObjectsCreator.transmitter.IsAlive ())
            {
                this.StartBtn.Text = "Stop";
            }
            else
            {
                this.StartBtn.Text = "Start";
            }
        }

        protected LightCom.Common.BindPointMapper m_mapper = new LightCom.Common.BindPointMapper ();
        protected LightCom.Common.SimpleMapper m_simple_mapper = new LightCom.Common.SimpleMapper ();

        private void OnMapMenu (object sender, EventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (null == item) return;

            foreach (MenuItem i in item.Parent.MenuItems)
            {
                i.Checked = false;
            }

            item.Checked = true;

            if (item == menuItemZoom300)
            {
                scaleX = scaleY = 1 / 3.0;
            }
            else if (item == menuItemZoom200)
            {
                scaleX = scaleY = 1 / 2.0;
            }
            else if (item == menuItemZoom150)
            {
                scaleX = scaleY = 1 / 1.5;
            }
            else if (item == menuItemZoom100)
            {
                scaleX = scaleY = 1.0;
            }
            else if (item == menuItemZoom75)
            {
                scaleX = scaleY = 1 / 0.75;
            }
            else if (item == menuItemZoom50)
            {
                scaleX = scaleY = 2;
            }
            else if (item == menuItemZoom25)
            {
                scaleX = scaleY = 4;
            }
            if (null != m_LastPosition && 0 != m_LastPosition.dwValidFields)
            {
                if ((m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LATITUDE) != 0 &&
                    (m_LastPosition.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LONGITUDE) != 0)
                {
                    MainForm.form.ShowMap (m_LastPosition.dblLatitude, m_LastPosition.dblLongitude);
                }
            }
        }

        /// <summary>
        /// Защита он нелицензированного использования.
        /// </summary>
        /// <returns>true, если копия легальная</returns>
        private static bool TestCode ()
        {
            string strAppFolder = LightCom.Common.Utils.ExeDirectory;
            string strKeyFolder = strAppFolder + "\\Key";
            try
            {
                Directory.CreateDirectory (strKeyFolder);
            }
            catch (Exception)
            {
            }

            byte [] presetId;
            byte [] platformId;
            if (!LightCom.WinCE.HardwareId.GetDeviceID (out presetId, out platformId))
            {
                return false;
            }

            string strPresetId = Convert.ToBase64String (presetId);
            string strPlatformId = Convert.ToBase64String (platformId);
            try
            {
                StreamWriter sw = new StreamWriter (strKeyFolder + "\\HardwareId.txt");
                sw.WriteLine (strPresetId);
                sw.Write (strPlatformId);
                sw.Close ();
            }
            catch (Exception)
            {
            }


            LightCom.WinCE.HardwareKey key = new LightCom.WinCE.HardwareKey ();
            if (!key.LoadLicense (strKeyFolder + "\\BaseKey.txt", "MiP 1.5: Собственность компании OOO \"ЛайтКом\""))
            {
                return false;
            }
            if (strPresetId != key.PresetId || strPlatformId != key.PlatformId)
            {
                return false;
            }

            MainForm.m_strLicenseOwner = key.LicenseOwner;
            MainForm.m_strLicenseDistributorName = key.DistributorName;
            MainForm.m_strLicenseDistributorArea = key.DistributorArea;
            MainForm.m_strLicenseNumber = key.Number;

            /*
            string strKeyName = "HKEY_CLASSES_ROOT\\CLSID\\{CFB84EDA-089D-4231-BDD6-E576AA04E0B4}\\InprocServer32";
            try
            {
                //Microsoft.Win32.Registry.SetValue (strKeyName, "ThreadingModel", "Apartment");
                string strVal = Microsoft.Win32.Registry.GetValue (strKeyName, "ThreadingModel", string.Empty) as string;
                if (null == strVal)
                {
                    return false;
                }
                if ("Apartment" != strVal)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
             * System.Diagnostics.Trace.WriteLine (e.ToString ());
                return false;
            }
            */

            return true;
        }
       
    }    
}
