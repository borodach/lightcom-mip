///////////////////////////////////////////////////////////////////////////////
//
//  File:           WM5GPSTransmitter.cs
//
//  Facility:       Организация потока обработки данных.
//                  
//
//
//  Abstract:       Класс расширяет GPSTransmitter возможностью чтения GPS 
//                  сигналов с помощью Windows Mobile 5 GPS API.
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  29-Jun-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: WM5GPSTransmitter.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Created in $/LightCom/.NET/MiP/CEClient
 * Реализована поддержка Windows Mobile GPS API
*/
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.Gps;


namespace LightCom.MiP.CEClient
{
    /// <summary>
    /// Класс расширяет GPSTransmitter возможностью чтения GPS 
    /// сигналов с помощью Windows Mobile 5 GPS API.
    /// </summary>
    internal class WM5GPSTransmitter : GPSTransmitter, IDisposable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="publisher">Ссылка на объект, выполняющий пуюликацию сведений
        /// </param>
        internal WM5GPSTransmitter (IPublisher publisher)
            : base (publisher)
        {
            hGPS = IntPtr.Zero;
            Reset ();
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
        public new void Dispose ()
        {
            base.Dispose ();
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
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.

                Reset ();
            }
            disposed = true;
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~WM5GPSTransmitter ()
        {
            Dispose (false);
        }
#endregion;

#region "ISettings methods";

        /// <summary>
        /// Сбрасывает состояние объекта в начальное состояние.
        /// </summary>
        public override void Reset ()
        {
            base.Reset ();
            this.CloseGps ();
        }        
#endregion;

        #region "GPSListener methods";

        /// <summary>
        /// Признак того, что GPS порт открыт
        /// </summary>
        public override bool IsOpen
        {
            get { return IntPtr.Zero != hGPS; }
        }

        // <summary>
        /// Закрывает GPS соединение
        /// </summary>
        public override void CloseGps ()
        {
            try
            {
                if (IsOpen)
                {
                    LightCom.WinCE.WinMobile5GPSWrapper.GPSCloseDevice (hGPS);
                    hGPS = IntPtr.Zero;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Открывает GPS соединение
        /// </summary>
        public override bool OpenGps ()
        {
            try
            {
                if (!IsOpen)
                {
                    this.hGPS = LightCom.WinCE.WinMobile5GPSWrapper.GPSOpenDevice (IntPtr.Zero, IntPtr.Zero, null, 0);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return IsOpen;
        }
        #endregion;

        #region "PipeLine methods";
        /// <summary>
        /// Обработка элемента конвейера.
        /// Метод выполняется в отдельном потоке.
        /// </summary>
        /// <param name="obj">Обрабатываемый элемент.</param>
        protected override bool GetDataItem (out object obj)
        {
            Thread.Sleep ((int) this.WaitTimeOut);
            obj = null;

            try
            {
                if (!this.IsOpen)
                {
                    this.OpenGps ();
                    Thread.Sleep ((int) this.WaitTimeOut);
                    if (!IsOpen)
                    {
                        this.GPSReceiverState = State.Error;
                        OnGPSDataRead (null);
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                this.GPSReceiverState = State.Error;
                OnGPSDataRead (null);
                return false;
            }


            LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION pos = new LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION ();
            int result = LightCom.WinCE.WinMobile5GPSWrapper.GPSGetPosition (hGPS, pos, 10 * this.m_nWaitTimeout, 0);
            if (result != 0)
            {
                this.GPSReceiverState = State.Error;
                OnGPSDataRead (null);
                return false;
            }

            if ((pos.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LATITUDE) != 0 &&
                (pos.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LONGITUDE) != 0)
            {
                this.GPSReceiverState = State.OK;
                this.Put (pos.RMC);
            }
            else
            {
                this.GPSReceiverState = State.Warning;
                //this.Put ("$GPRMC,180258,A,4056.4604,N,07216.9673,W,3.965,263.0,130800,14.8,W*41");

            }
            OnGPSDataRead (pos);
         

            return false;
        }
        #endregion;

        /// <summary>
        /// Дескриптор драйвера GPS
        /// </summary>
        private IntPtr hGPS = IntPtr.Zero;
    }
}
