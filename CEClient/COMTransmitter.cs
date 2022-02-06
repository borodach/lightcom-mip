///////////////////////////////////////////////////////////////////////////////
//
//  File:           COMTransmitter.cs
//
//  Facility:       Организация потока обработки данных.
//
//
//  Abstract:       Класс расширяет GPSTransmitter возможностью чтения GPS 
//                  сигналов из COM порта.
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  28-Jun-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: COMTransmitter.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Created in $/LightCom/.NET/MiP/CEClient
 * Реализована поддержка Windows Mobile GPS API
*/
using System;
using System.IO;
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
    //  сигналов из COM порта
    /// </summary>
    internal class COMTransmitter : GPSTransmitter, IDisposable
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="publisher">Ссылка на объект, выполняющий пуюликацию сведений
        /// </param>
        internal COMTransmitter (IPublisher publisher)
            : base (publisher)
        {
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
        ~COMTransmitter ()
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
            this.PortName = "COM5:";
            this.BaudRate = 9600;
            this.CloseGps ();
        }

        /// <summary>
        /// Сохраняет настройки объекта в заданном хранилище.
        /// </summary>
        /// <param name="storage">Интерфейс сохранения параметров.</param>
        /// <returns>Возвращает true, если сохранение прошло успешно.
        /// </returns>
        public override bool Save (SettingsStorage storage)
        {
            bool bResult = base.Save (storage);
            bResult &= storage.Write (storageCategery, "Port", PortName);
            bResult &= storage.Write (storageCategery, "BaudRate", BaudRate);

            return bResult;
        }

        /// <summary>
        /// Загружает настройки объекта из заданного хранилища.
        /// </summary>
        /// <param name="storage">Интерфейс сохранения параметров.</param>
        /// <returns>Возвращает true, если загрузка прошла успешно.
        /// </returns>
        public override bool Load (SettingsStorage storage)
        {
            this.Reset ();
            base.Load (storage);
            string defPortName = PortName;

            string strVal;
            storage.Read (storageCategery, "Port", out strVal, defPortName);
            PortName = strVal;

            int defBaudRate = BaudRate;
            int nVal;
            storage.Read (storageCategery, "BaudRate", out nVal, defBaudRate);
            BaudRate = nVal;

            return true;
        }
        #endregion
        #region "GPSListener methods";
        /// <summary>
        /// Закрывает GPS соединение
        /// </summary>
        public override void CloseGps ()
        {
            try
            {
                if (IsOpen)
                {
                    this.m_Port.Close ();
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
                    this.m_Port.Open ();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return IsOpen;
        }


        /// <summary>
        /// Признак того, что GPS порт открыт
        /// </summary>
        public override bool IsOpen
        {
            get { return this.m_Port.IsOpen; }
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
                }
            }
            catch (Exception)
            {
            }

            string [] gpsCommands = null;
            bool bResult = false;
#if DEBUG1
            if (testData.Length > 0)
            {

                gpsCommands = new string [1];
                gpsCommands [0] = testData [testIdx];
                bResult = true;


                testIdx = (testIdx + 1) % testData.Length;
            }
#else
        bResult = LightCom.Gps.GPSReader.ReadGPSData (m_Port, out gpsCommands);
#endif


            //bool bResult = true;
            //string [] gpsCommands = {"$GPRMC,125721.16,A,5500.109373,N,08255.547568,E,026.6,220.0,221105,,*39"};
            if (bResult && null != gpsCommands)
            {
                //dbg MessageBox.Show ("Data read");
                for (int nIdx = 0; nIdx < gpsCommands.Length; ++nIdx)
                {
                    if (LightCom.Gps.GPSReader.IsGPSDataValid (gpsCommands [nIdx]))
                    {
                        LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION pos = new LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION (gpsCommands [nIdx]);

                        if ((pos.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LATITUDE) != 0 &&
                            (pos.dwValidFields & LightCom.WinCE.WinMobile5GPSWrapper.GPS_VALID.GPS_VALID_LONGITUDE) != 0)
                        {
                            this.GPSReceiverState = State.OK;
                            this.Put (gpsCommands [nIdx]);
                        }
                        else
                        {
                            this.GPSReceiverState = State.Warning;
                        }
                        OnGPSDataRead (pos);
                        
                    }
                    else
                    {
                        if (LightCom.Gps.GPSReader.IsGPSDataValid (gpsCommands [nIdx]))
                        {
                            this.GPSReceiverState = State.Warning;
                            LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION pos = new LightCom.WinCE.WinMobile5GPSWrapper.GPS_POSITION ();
                            OnGPSDataRead (pos);
                        }
                    }
                }
                return false;
            }
            else
            {
                this.GPSReceiverState = State.Error;
                OnGPSDataRead (null);
                this.CloseGps ();
            }

            return false;
        }
        #endregion;

        /// <summary>
        /// Имя порта GPS приемника.
        /// </summary>
        public string PortName { get { return this.m_Port.PortName; } set { this.m_Port.PortName= value; } }

        /// <summary>
        /// COM port name.
        /// </summary>
        protected System.IO.Ports.SerialPort m_Port = new System.IO.Ports.SerialPort ();

        /// <summary>
        /// COM port baud rate.
        /// </summary>
        protected int BaudRate
        {
            get { return m_Port.BaudRate; }
            set { m_Port.BaudRate = value; }
        }
    }
}
