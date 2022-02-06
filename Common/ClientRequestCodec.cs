///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           ClientRequestCodec.cs
//
//  Facility:       Обработка запросов на сервер.
//
//
//  Abstract:       Класс кодирует/декодирует запросы клиентов на сервер.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  19/09/2005
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: ClientRequestCodec.cs $
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 4.03.07    Time: 16:22
 * Updated in $/LightCom/.NET/MiP/Common
 * Первая после редизайна собирающаяся версия
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 4.03.07    Time: 15:54
 * Updated in $/LightCom/.NET/MiP/Common
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 4.03.07    Time: 10:51
 * Updated in $/gps/Common
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:40
 * Updated in $/gps/Common
 * 
 */

using System;
using System.Text;
using LightCom.Common;

namespace LightCom.MiP.Common
{

    /// 
    /// <summary>
    /// Класс кодирует/декодирует запросы клиентов на сервер.
    /// </summary>
    /// 

    public class ClientRequestCodec
    {

        //////////////////////////////////////////////////////////////////////////////// 
        /// 
        /// <summary>
        /// Декодирует запрос клиента.
        /// </summary>
        /// 
        ////////////////////////////////////////////////////////////////////////////////

        public bool Decode (byte [] data)
        {
            try
            {

                Reset ();
                if (null == data)
                    return false;
                if (0 == data.Length)
                    return false;

                byte [] readData;
                UTF8Encoding encoder = new UTF8Encoding ();
                string strReadData;

                int nIdx = 0;
                byte nSize;

                //
                // Читаем номер версии.
                //

                nSize = data [nIdx++];
                if (nSize + nIdx > data.Length)
                    return false;
                readData = new byte [nSize];
                for (byte i = 0; i < nSize; ++i)
                    readData [i] = data [nIdx];
                strReadData = encoder.GetString (readData, 0, nSize);
                m_nVersion = Convert.ToInt32 (strReadData);
                nIdx += readData.Length;

                //
                // Читаем название версии.
                //

                nSize = data [nIdx++];
                if (nSize + nIdx > data.Length)
                    return false;
                readData = new byte [nSize];
                for (byte i = 0; i < nSize; ++i)
                    readData [i] = data [nIdx++];
                readData = Encryption.Decrypt (readData, m_key);
                ProductVersion = encoder.GetString (readData, 0, readData.Length);

                //
                // Читаем client id.
                //

                nSize = data [nIdx++];
                if (nSize + nIdx > data.Length)
                    return false;
                readData = new byte [nSize];
                for (byte i = 0; i < nSize; ++i)
                    readData [i] = data [nIdx++];
                readData = Encryption.Decrypt (readData, m_key);
                ClientId = encoder.GetString (readData, 0, readData.Length);

                //
                // Читаем client id.
                //

                nSize = data [nIdx++];
                if (nSize + nIdx > data.Length)
                    return false;
                readData = new byte [nSize];
                for (byte i = 0; i < nSize; ++i)
                    readData [i] = data [nIdx++];
                readData = Encryption.Decrypt (readData, m_key);
                GPSInfo = encoder.GetString (readData, 0, readData.Length);


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
        /// Кодирует запрос для отправки.
        /// </summary>
        /// 
        //////////////////////////////////////////////////////////////////////////////// 

        public byte [] Encode ()
        {
            UTF8Encoding encoder = new UTF8Encoding ();
            int nSize = 0;
            byte [] [] binaryData = 
    {
        encoder.GetBytes (m_nLatestVersion.ToString ()),
        Encryption.Encrypt (encoder.GetBytes (ProductVersion), m_key),
        Encryption.Encrypt (encoder.GetBytes (ClientId), m_key),
        Encryption.Encrypt (encoder.GetBytes (GPSInfo), m_key)
    };

            //
            //##Подсчитываем размер отправленных данных.
            //

            int nCount = binaryData.Length;
            for (int i = 0; i < nCount; ++i)
            {
                nSize += binaryData [i].Length + 1;
            }

            int nIdx = 0;
            byte [] result = new byte [nSize];
            for (int i = 0; i < nCount; ++i)
            {
                result [nIdx++] = (byte) binaryData [i].Length;
                binaryData [i].CopyTo (result, nIdx);
                nIdx += binaryData [i].Length;
            }
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////
        /// 
        /// <summary>
        /// Reset object data.
        /// </summary>
        /// 
        //////////////////////////////////////////////////////////////////////////////// 

        public void Reset ()
        {
            m_strProductVersion = "";
            m_strClientId = "";
            m_strGPSInfo = "";
            m_nVersion = m_nLatestVersion;
        }

        //
        // Data members.
        //

        /// 
        /// <summary>
        /// Версия продукта.
        /// </summary>
        ///

        public string ProductVersion
        {
            get
            {
                return m_strProductVersion;
            }
            set
            {
                m_strProductVersion = value;
            }
        }
        protected string m_strProductVersion;

        /// 
        /// <summary>
        /// Client ID.
        /// </summary>
        ///

        public string ClientId
        {
            get
            {
                return m_strClientId;
            }
            set
            {
                m_strClientId = value;
            }
        }
        protected string m_strClientId;

        /// 
        /// <summary>
        /// GPS info.
        /// </summary>
        ///

        public string GPSInfo
        {
            get
            {
                return m_strGPSInfo;
            }
            set
            {
                m_strGPSInfo = value;
            }
        }
        protected string m_strGPSInfo;

        /// 
        /// <summary>
        /// Persistant object version.
        /// </summary>
        ///

        public int Version
        {
            get
            {
                return m_nVersion;
            }
        }
        protected int m_nVersion;

        /// 
        /// <summary>
        /// Значение текущей версии persistant object.
        /// </summary>
        /// 

        public const int m_nLatestVersion = 1;

        /// 
        /// <summary>
        /// Ключ для шифрования.
        /// </summary>
        ///

        protected byte m_key = 0x5A;

    }
}
