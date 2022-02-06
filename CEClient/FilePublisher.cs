///////////////////////////////////////////////////////////////////////////////
//
//  File:           FilePublisher.cs
//
//  Facility:       Сохранение результатов работы клиента MiP
//                  
//
//
//  Abstract:       Реализация интерфейса IPublisher для сохранения в файл
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  16-Nov-2006
//
//  Copyright (C) OOO "ЛайтКом", 2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: FilePublisher.cs $
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 4.03.07    Time: 17:02
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 4.03.07    Time: 10:51
 * Updated in $/gps/EndPoint
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:34
 * Updated in $/gps/EndPoint
 * Мелкое форматирование
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:32
 * Updated in $/gps/EndPoint
 * Заблокирована запись в файл пустых треков. Пустой трек - это признак
 * конца файла (иначе вечный цикл получается)
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:25
 * Updated in $/gps/EndPoint
 * Добавлен деструктор, выполняющий Flush
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:06
 * Updated in $/gps/EndPoint
 * Проведена первая отладка, исправлен ряд ошибок
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 19:08
 * Updated in $/gps/EndPoint
 * Альфа версия класса
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:47
 * Created in $/gps/EndPoint
 * 
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.MiP.Cache;

namespace LightCom.MiP.CEClient
{
    /// <summary>
    /// Реализация интерфейса IPublisher для сохранения в файл
    /// </summary>
    internal class FilePublisher : LightCom.Common.IPublisher, LightCom.Common.ISettings
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public FilePublisher ()
        {
            codec = new ClientRequestCodec ();
            track = new ClientTrack ();
            Reset ();
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~FilePublisher ()
        {
            Flush ();
        }

        //
        // ISettings interface implementation
        //

        /// <summary>
        /// Сбрасывает состояние объекта в начальное состояние.
        /// </summary>
        public void Reset ()
        {
            fileName = Utils.ExeDirectory + "\\Save\\trace.txt";
            lastFlushTime = DateTime.MaxValue;
            flushPeriod = TimeSpan.FromMinutes (15);
            track.Reset ();
        }

        /// <summary>
        /// Сохраняет настройки объекта в заданном хранилище.
        /// </summary>
        /// <param name="storage">Интерфейс сохранения параметров.</param>
        /// <returns>Возвращает true, если сохранение прошло успешно.
        /// </returns>
        public bool Save (SettingsStorage storage)
        {
            if (!storage.Write (strSettingsType,
                                  "FlushPeriod",
                                  flushPeriod))
            {
                return false;
            }
                        
            if (!storage.Write (strSettingsType,
                                "FileName",
                                fileName))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Загружает настройки объекта из заданного хранилища.
        /// </summary>
        /// <param name="storage">Интерфейс сохранения параметров.</param>
        /// <returns>Возвращает true, если загрузка прошла успешно.
        /// </returns>
        public bool Load (SettingsStorage storage)
        {
            Reset ();
            TimeSpan defFlushPeriog = flushPeriod;
            storage.Read (strSettingsType,
                          "FlushPeriod",
                           out flushPeriod,
                           defFlushPeriog);

            string defFileName = fileName;
            storage.Read (strSettingsType,
                          "FileName",
                           out fileName,
                           defFileName);

            return true;
        }

        /// <summary>
        /// Передает данные на сервер и получает ответ.
        /// </summary>
        /// <param name="data">Передаваемые на сервер данные.</param>
        /// <param name="receiveStream">Ответ сервера</param>
        /// <returns>Возвращает true, если передача прошла успешно.</returns>
        public bool Publish (byte [] data, out Stream receiveStream)
        {
            receiveStream = null;

            try
            {
                if (!codec.Decode (data))
                {
                    m_strStatus = "Ошибка декодирования";
                    return false;
                }

                track.ClientId = codec.ClientId;

                double fLongitude;
                double fLatitude;
                double fSpeed;
                double fCourse;
                DateTime fixTime;

                if (!LightCom.Gps.GPSReader.GetPosition (codec.GPSInfo,
                                                         out fLongitude,
                                                         out fLatitude,
                                                         out fSpeed,
                                                         out fCourse,
                                                         out fixTime))
                {
                    m_strStatus = "Ошибка разбора";
                    return false;
                }

                ObjectPosition position = new ObjectPosition ();
                position.X = fLongitude;
                position.Y = fLatitude;
                position.Speed = fSpeed;
                position.Course = fCourse;
                position.Timestamp = fixTime;
                    
                track.Add (position);
                m_strStatus = "Успешно закэшировано";

                if (lastFlushTime == DateTime.MaxValue)
                {
                    lastFlushTime = DateTime.Now;

                    return true;
                }

                DateTime now = DateTime.Now;
                DateTime flushTime = lastFlushTime + flushPeriod;

                if (flushTime > now)
                {
                    return true;
                }

                if (!Flush ())
                {
                    return false;
                }

                ClearTrack ();

            }
            catch (Exception  e)
            {
                m_strStatus = e.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Записывает кэш на диск
        /// </summary>
        /// <returns>true, если операция прошла успешно</returns>
        public bool Flush ()
        {
            if (track.Count < 1)
            {
                return true;
            }
            try
            {
                try
                {
                    FileInfo fi = new FileInfo (fileName);
                    if (!Directory.Exists (fi.DirectoryName))
                    {
                        Directory.CreateDirectory (fi.DirectoryName);
                    }
                }
                catch (Exception)
                {
                }

                lastFlushTime = DateTime.Now;
                FileStream fs = new FileStream (fileName, FileMode.Append, FileAccess.Write, FileShare.Read);

                //fs.Seek (0, SeekOrigin.End);
                if (! track.SaveGuts (fs))
                {
                    m_strStatus = "Ошибка ввода/вывода";
                    return false;
                }
                fs.Close ();
            }
            catch (Exception  e)
            {
                m_strStatus = e.Message;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Строка статуса последнего обращения к серверу.
        /// </summary>
        public string Status { get { return m_strStatus; } }
        protected string m_strStatus = m_strDefaultStatus;

        ///
        /// <summary>
        /// Строка статуса по-умолчанию.
        /// </summary>
        /// 

        protected const string m_strDefaultStatus = "None";

        /// <summary>
        /// HTTP user agent.
        /// </summary>
        public virtual string UserAgent { get { return this.m_strUserAgent; } set { this.m_strUserAgent = value; } }
        protected string m_strUserAgent = "";

        /// <summary>
        /// Удаляет все сведения о местоположении объекта
        /// </summary>
        public void ClearTrack ()
        {
            track.Reset ();
        }

        /// <summary>
        /// Список сведений о местоположении объекта
        /// </summary>
        public ClientTrack Track
        {
            get { return track; }
        }
        protected ClientTrack track;
        
        /// <summary>
        /// Время последнего сохранения в файл
        /// </summary>
        public DateTime LastFlushTime
        {
            get { return lastFlushTime; }
            set { lastFlushTime = value; }
        }
        protected DateTime lastFlushTime;

        /// <summary>
        /// Периодичность сохранения результатов в файл
        /// </summary>
        public TimeSpan FlushPeriod
        {
            get { return flushPeriod; }
            set { flushPeriod = value; }
        }
        protected TimeSpan flushPeriod;

        /// <summary>
        /// Имя файла
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        protected string fileName;

        /// <summary>
        /// Название категории настроек
        /// </summary>
        public const string strSettingsType = "FilePublisher";

        /// <summary>
        /// Кодек для расшифровки данных, передаваемых МиП клиентом.
        /// </summary>
        protected ClientRequestCodec codec;

        /// <summary>
        /// Статический экземпляр
        /// </summary>
        public static FilePublisher instance = null;
    }
}
