///////////////////////////////////////////////////////////////////////////////
//
//  File:           BaseDataSource.cs
//
//  Facility:       Взаимодействие с сервером.
//
//
//  Abstract:       Базовый класс для получения с сервера данных о мобильных 
//                  клиентах.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  10-12-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.IO;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.MiP.Cache;

namespace LightCom.MiP.DataSource
{
/// 
/// <summary>
/// Базовый класс для получения с сервера данных о мобильных клиентах.
/// </summary>
/// 

public abstract class BaseDataSource: IGPSDataSource
{
/// 
/// <summary>
/// Тип запроса, отправляемго на сервер.
/// </summary>
/// 

public enum RequestType
{
    rtInvalid = 0,
    rtConnect,
    rtDisconnect,
    rtGetClientList,
    rtGetClientsInfo,
    rtGetEvents,
    rtReadGPSData,
    rtReadLatestGPSData,
    rtUpdateMobileClientInfo,
    rtMaxValue
};

/// 
/// <summary>
/// Тип ответа сервера.
/// </summary>
/// 

public enum ResponseType
{
    rtInvalid = 0,
    rtOK,
    rtError,
    rtMaxValue
};

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Шифрует буфер dataToEncrypt.
/// </summary>
/// <param name="dataToEncrypt">Буфер, который нужно зашифровать.</param>
/// <param name="key">Ключ. Если равен null, то данные просто 
/// копируются.</param>
/// <param name="encryptedData">Зашифрованный буфер.</param>
/// <returns>true, усли операция выполнилась успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool Encrypt (byte [] dataToEncrypt, 
                              byte [] key,
                              out byte [] encryptedData);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Расшифровывает буфер dataToDecrypt.
/// </summary>
/// <param name="dataToDecrypt">Буфер, который нужно расшифровать.</param>
/// <param name="key">Ключ. Если равен null, то данные просто 
/// копируются.</param>
/// <param name="decryptedData">Расшифрованный буфер.</param>
/// <returns>true, усли операция выполнилась успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool Decrypt (byte [] dataToDecrypt, 
                              byte [] key,
                              out byte [] decryptedData);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Генерирует ключ для шифрования данных.
/// </summary>
/// <param name="encryptionKey">Ключ для шифрования.</param>
/// <param name="decryptionKey">Ключ для дешифрования.</param>
/// <returns>true, усли операция выполнилась успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool GenerateKey (out byte [] encryptionKey, 
                                    out byte [] decryptionKey);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Отправляет данные на сервер
/// </summary>
/// <param name="dataToSend">Отпраляемые на сервер данные.</param>
/// <param name="response">Ответ сервера.</param>
/// <returns>true, усли операция выполнилась успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool Send (byte [] dataToSend, 
                            out Stream response);


////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Default constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public BaseDataSource ()
{
    ASCIIEncoding m_ASCIIEncoder = new ASCIIEncoding ();
    m_KeyToConnect = null;
    m_EncryptionKey = null;
    m_DecryptionKey = null;
    m_nConnectionId = m_nInvalidConnectionId;
    m_strLogin = string.Empty;
    m_strDomain = string.Empty;
    m_strPassword = string.Empty;
    m_strLastError = "OK";
    m_Random = new Random ();
    m_Version = 1;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает true, если соединение установлено.
/// </summary>
/// <returns>Возвращает true, если соединение установлено.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool IsConnected  ()
{
    return m_nInvalidConnectionId != m_nConnectionId;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает строчку с текстом последней ошибки.
/// </summary>
/// <returns>Возвращает строчку с текстом последней ошибки.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public string GetLastErrorText ()
{
    return m_strLastError;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Вычисляет контрольную сумму.
/// </summary>
/// <param name="data">Данные, для которых нужно подсчитать контрольную 
/// сумму.</param>
/// <returns>Возвращает контрольную сумму.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public int CalculateCRC (byte [] data)
{
    return Utils.CalculateCRC (data);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Обработка запроса: проверка валидности и расшифровка. Метод годится для 
/// обработки ответов сервера и запросов клиентов.
/// </summary>
/// <param name="requestStream">Поток, содержащий запрос.</param>
/// <param name="keyToDecrypt">Ключ для расшифровки.</param>
/// <param name="decodedRequest">Расшифрованный запрос.</param>
/// <param name="type">Тип запроса.</param>
/// <param name="nRandomValue">Случайное число, внедренное в запрос.</param>
/// <returns></returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool ReadRequestHeader (Stream requestStream, 
                               byte [] keyToDecrypt,
                               out Stream decodedRequest,
                               out RequestType type,                               
                               out int nRandomValue)
{
    type = RequestType.rtInvalid;
    decodedRequest = null;
    nRandomValue = 0;
    try
    {
        //
        //Проверка версии запроса.
        //

        int nVersion = Utils.ReadInt (requestStream);
        if (nVersion > m_Version)
        {
            m_strLastError = "Ошибка при чтении ответа сервера: неподдерживаемый формат ответа.";
            requestStream.Close ();
            return false;
        }

        //
        //Чтение типа запроса.
        //

        type = (RequestType) Utils.ReadInt (requestStream);

        //
        //Чтение контрольной суммы.
        //

        int nCRC = Utils.ReadInt (requestStream);

        //
        //Чтение зашифрованных данных.
        //

        int nSize = Utils.ReadInt (requestStream);
        if (nSize < 0)
        {
            m_strLastError = "Ошибка при чтении ответа сервера: неправильный размер буфера.";
            requestStream.Close ();
            return false;
        }

        //
        //##Читаем ответ сервера.
        //

        byte [] encryptedData = new byte [nSize];
        requestStream.Read (encryptedData, 0, nSize);        
        byte [] decryptedData;

        //
        //Расшифровываем запрос.
        //

        if ( ! Decrypt (encryptedData, keyToDecrypt, out decryptedData))
        {
            m_strLastError = "Не удалось расшифровать полученные данные.";
            requestStream.Close ();
            return false;    
        }
        requestStream.Close ();

        //
        //Проверяем контрольную сумму.
        //

        int nCalculatedCRC = CalculateCRC (decryptedData);
        if (nCalculatedCRC != nCRC)
        {
            m_strLastError = "Ошибка при чтении ответа сервера: неправильная контрольная сумма.";
                return false;    
        }

        decodedRequest = new MemoryStream (decryptedData, 
            0, 
            decryptedData.Length, 
            false);

        nRandomValue = Utils.ReadInt (decodedRequest);
    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Отправляет запрос на сервер.
/// </summary>
/// <param name="type">Тп запроса.</param>
/// <param name="data">Данные, которые нужно передать.</param>
/// <param name="keyToEncrypt">Ключ для шифрования данных.</param>
/// <param name="response">Ответ сервера.</param>
/// <returns>true, усли операция выполнилась успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

bool SendRequest (RequestType type, 
                  byte [] data,
                  byte [] keyToEncrypt,
                  out Stream response)
{
    response = null;
    try
    {
        //
        //Подготавливаем запрос. Сначала записывается версия и тип 
        //запроса.
        //

        MemoryStream ms = new MemoryStream ();
        Utils.Write (ms, m_Version);
        Utils.Write (ms, (int) type);           
        if (type != RequestType.rtConnect)
        {
            Utils.Write (ms, m_nConnectionId);
        }
    
        //
        //Записываем контрольную сумму секретных данных, 
        //которые еще не зашифрованы.
        //

        Utils.Write (ms, CalculateCRC (data));

        //
        //Шифруем секретный буфер.
        //

        byte [] encryptedData;
        if ( ! Encrypt (data, keyToEncrypt, out encryptedData))
        {
            m_strLastError = "Не удалось зашифровать передаваемые данные.";
            return false;    
        }

        //
        //Записываем размер зашифрованного буфера и сам буфер и отправляем 
        //запрос на сервер.
        //

        Utils.Write (ms, encryptedData.Length);
        ms.Write (encryptedData, 0, encryptedData.Length);

        if (! Send (Utils.GetStreamData (ms), out response))
        {
            return false;
        }
        
        /*
        if (type == RequestType.rtDisconnect)
        {
            byte [] buf = new byte [1024];
            response.Read (buf, 0, 1024);
            ASCIIEncoding enc = new ASCIIEncoding ();
            string res = enc.GetString (buf, 0, 1024);
            int i = 0;
        }*/ 

        //
        //Проверка заголовка ответа.
        //

        ResponseType responseType = (ResponseType) Utils.ReadInt (response);
        if (responseType != ResponseType.rtOK)
        {
            m_strLastError = Utils.ReadString (response);
            response.Close ();
            return false;
        }

        return true;
    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Создает поток для записи в него секретных данных.
/// </summary>
/// <returns>Поток для записи секретных данных.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////
///
MemoryStream CreateProtectedStream ()
{
    m_nRandomValue = m_Random.Next ();    
    MemoryStream ms = new MemoryStream ();
    Utils.Write (ms, m_nRandomValue);
    return ms;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Устанавливает соединение с сервером.
/// </summary>
/// <returns>true, если соединение установлено успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Connect ()
{
    if (IsConnected ()) return true;
    try
    {
        byte [] key;
        if (! GenerateKey (out key, out m_DecryptionKey))
        {
            m_strLastError = "Не удалось сгенерировать ключи для шифрования данных.";
            return false;
        }

        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        
        Utils.Write (secretStream, m_strLogin);
        Utils.Write (secretStream, m_strDomain);
        Utils.Write (secretStream, m_strPassword);
        Utils.Write (secretStream, key.Length);
        secretStream.Write (key, 0, key.Length);

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtConnect, 
            secretData, 
            m_KeyToConnect,
            out response))
        {
            return false;
        }
        
        //
        //Читаем ответ.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
                                 m_DecryptionKey, 
                                 out decodedResponse, 
                                 out type, 
                                 out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "Некорректный ответ сервера";
            return false;
        }
        if (type != RequestType.rtConnect)
        {
            m_strLastError = "Неожиданный тип ответа";
            return false;
        }

        //
        //##Читаем идентификатор соединения и ключ для шифрования запросов.
        //

        m_nConnectionId = Utils.ReadInt (decodedResponse);
        if (m_nInvalidConnectionId == m_nConnectionId)
        {
            m_strLastError = "Сервер отказал в соединении.";
            return false;
        }

        int nKeyLength = Utils.ReadInt (decodedResponse);
        if (nKeyLength < 0)
        {
            m_strLastError = "Ответ сервера содержит некорректые данные.";
            return false;
        }
        if (0 == nKeyLength) 
        {
            m_EncryptionKey = null;
        }
        else
        {
            m_EncryptionKey = new byte [nKeyLength];
            decodedResponse.Read (m_EncryptionKey, 0, m_EncryptionKey.Length);
        }
    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }
    
    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Разрывает соединение с сервером.
/// </summary>
/// <returns>true, если соединение разорвано успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool Disconnect ()
{
    if (! IsConnected ()) return true;
    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        Utils.Write (secretStream, m_nConnectionId);
        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtDisconnect, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            m_nConnectionId = m_nInvalidConnectionId;
            return false;
        }

        //
        //Ответ читать не нужно.
        //
        response.Close ();
        m_nConnectionId = m_nInvalidConnectionId;            

    }
    catch (Exception e)
    {
        m_nConnectionId = m_nInvalidConnectionId;
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Получение списка идентификаторов мобильных клиентов.
/// </summary>
/// <param name="strClientId">Массив идентификаторов мобильных клиентов.
/// </param>
/// <returns>true, если список был получен успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetMobileClientList (out string [] strClientId)
{
    strClientId = null;
    if (! IsConnected ()) 
    {
        m_strLastError = "Не установлено соединение с сервером.";
        return false;
    }

    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        Utils.Write (secretStream, m_nConnectionId);                
        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtGetClientList, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //Читаем ответ.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "Некорректный ответ сервера";
            return false;
        }
        if (type != RequestType.rtGetClientList)
        {
            m_strLastError = "Неожиданный тип ответа";
            return false;
        }

        //
        //##Читаем идентификаторы клиентов.
        //

        int nCount = Utils.ReadInt (decodedResponse);
        if (nCount > 0)
        {
            strClientId = new string [nCount];
        }
        else
        {
            return true;
        }
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            strClientId [nIdx] = Utils.ReadString (decodedResponse);    
        }

    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Загрузка описаний заданных мобильных клиентов.
/// </summary>
/// <param name="strClientId">Массив идентификаторов мобильных клиентов.
/// </param>
/// <param name="clientsInfo">Массив описаний мобильных клиентов.</param>
/// <returns>true, если список был получен успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetMobileClientsInfo (string [] strClientId, 
    out MobileClientList clientsInfo)
{
    clientsInfo = null;
    if (! IsConnected ()) 
    {
        m_strLastError = "Не установлено соединение с сервером.";
        return false;
    }

    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();

        int nSize = strClientId.Length;
        Utils.Write (secretStream, nSize);
        for (int nIdx = 0; nIdx < nSize; ++ nIdx)
        {
            Utils.Write (secretStream, strClientId [nIdx]);
        }
        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtGetClientsInfo, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //Читаем ответ.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "Некорректный ответ сервера";
            return false;
        }
        if (type != RequestType.rtGetClientsInfo)
        {
            m_strLastError = "Неожиданный тип ответа";
            return false;
        }

        //
        //Загрузка описаний мобильных клиентов.
        //

        clientsInfo = new MobileClientList ();
        int nCount = Utils.ReadInt (decodedResponse);
        for (int nIdx = 0; nIdx < nCount; ++nIdx)
        {
            MobileClientInfo info = new MobileClientInfo ();
            if (! info.RestoreGuts (decodedResponse))
            {
                m_strLastError = "Ошибка при чтении информации о мобильных объектах.";
                return false;
            }
            clientsInfo.Add (info);
        }

    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Получение первого события для заданной группы мобильных клиентов.
/// </summary>
/// <param name="strClientId">Массив идентификаторов мобильных клиентов.
/// </param>
/// <param name="firstEvent">Время первого события.</param>
/// <returns>true, если операция была выполнена успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool GetFirstEvent (string [] strClientId, 
    out DateTime firstEvent)
{
    DateTime lastEvent;
    return GetEvents (strClientId, out firstEvent, out lastEvent);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Получение последнего события для заданной группы мобильных клиентов.
/// </summary>
/// <param name="strClientId">>Массив идентификаторов мобильных клиентов.
/// </param>
/// <param name="lastEvent">Время последнего события.</param>
/// <returns>true, если операция была выполнена успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool GetLastEvent (string [] strClientId, out DateTime lastEvent)
{
    DateTime firstEvent;
    return GetEvents (strClientId, out firstEvent, out lastEvent);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Получение последнего события для заданной группы мобильных клиентов.
/// </summary>
/// <param name="strClientId">>Массив идентификаторов мобильных клиентов.
/// </param>
/// <param name="firstEvent">Время первого события.</param>
/// <param name="lastEvent">Время последнего события.</param>
/// <returns>true, если операция была выполнена успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetEvents (string [] strClientId, 
    out DateTime firstEvent, 
    out DateTime lastEvent)
{
    firstEvent = DateTime.MinValue;
    lastEvent = DateTime.MinValue;

    if (! IsConnected ()) 
    {
        m_strLastError = "Не установлено соединение с сервером.";
        return false;
    }

    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();

        int nSize = strClientId.Length;
        Utils.Write (secretStream, nSize);
        for (int nIdx = 0; nIdx < nSize; ++ nIdx)
        {
            Utils.Write (secretStream, strClientId [nIdx]);
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtGetEvents, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //Читаем ответ.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "Некорректный ответ сервера";
            return false;
        }
        if (type != RequestType.rtGetEvents)
        {
            m_strLastError = "Неожиданный тип ответа";
            return false;
        }

        firstEvent = Utils.ReadDateTime (decodedResponse);
        lastEvent = Utils.ReadDateTime (decodedResponse);
    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Загружает в кэш сведения о положении заданных клиентов за интервал
/// [from; to]
/// </summary>
/// <param name="cache">Кэш. В нем должен храниться список интересующих 
/// клиентов.</param>
/// <returns>true, если операция была выполнена успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool ReadGPSData (GPSDataCache cache, 
    DateTime from, 
    DateTime to)
{
    if (! IsConnected ()) 
    {
        m_strLastError = "Не установлено соединение с сервером.";
        return false;
    }

    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        int nClientCount = cache.Count;
        Utils.Write (secretStream, nClientCount);
        Utils.Write (secretStream, to);

        for (int nIdx = 0; nIdx < nClientCount; ++nIdx)
        {
            ObjectPositions clientData = cache [nIdx];
            int nIndex = clientData.GetIdxAtTime (from);
            if (nIndex >= 0) clientData.RemoveRange (0, nIndex + 1);
            Utils.Write (secretStream, clientData.ClientId);
            DateTime lastTime = clientData.LastEvent;
            if (DateTime.MinValue == lastTime) lastTime = from;
            Utils.Write (secretStream, lastTime);
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtReadGPSData, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //Читаем ответ.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "Некорректный ответ сервера";
            return false;
        }
        if (type != RequestType.rtReadGPSData)
        {
            m_strLastError = "Неожиданный тип ответа";
            return false;
        }

        //
        //##Загрузка сведений о положении мобильных объектов.
        //

        GPSDataCache readData = new GPSDataCache ();
        if (!readData.RestoreGuts (decodedResponse))
        {
            m_strLastError = "Ошибка при чтении сведений о положнии мобильных объектов.";
            return false;
        }
        cache.Add (readData);
    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }

    return true;                        
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Загружает в кэш сведения о положении заданных клиентов за интервал
/// [now - historyDepth; now]
/// </summary>
/// <param name="cache">Кэш. В нем должен храниться список интересующих 
/// клиентов.</param>
/// <param name="historyDepth">Величина интервала "истории" перемещений.
/// </param>
/// <returns>true, если операция была выполнена успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool ReadLatestGPSData (GPSDataCache cache, 
    TimeSpan historyDepth)
{
    if (! IsConnected ()) 
    {
        m_strLastError = "Не установлено соединение с сервером.";
        return false;
    }

    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        int nClientCount = cache.Count;
        Utils.Write (secretStream, nClientCount);
        Utils.Write (secretStream, (int) Math.Round (historyDepth.TotalSeconds));

        for (int nIdx = 0; nIdx < nClientCount; ++nIdx)
        {
            ObjectPositions clientData = cache [nIdx];
            Utils.Write (secretStream, clientData.ClientId);
            DateTime lastTime = clientData.LastEvent;
            Utils.Write (secretStream, lastTime);
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtReadLatestGPSData, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //Читаем ответ.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "Некорректный ответ сервера";
            return false;
        }
        if (type != RequestType.rtReadLatestGPSData)
        {
            m_strLastError = "Неожиданный тип ответа";
            return false;
        }

        //
        //##Загрузка сведений о положении мобильных объектов.
        //

        GPSDataCache readData = new GPSDataCache ();
        if (!readData.RestoreGuts (decodedResponse))
        {
            m_strLastError = "Ошибка при чтении сведений о положнии мобильных объектов.";
            return false;
        }

        cache.Add (readData);        

        //
        //Удаление ненужных записей.
        //

        int nCount = cache.Count;
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPositions obj = cache [nIdx];
            DateTime lastEvent = obj.LastEvent;
            if (DateTime.MinValue == lastEvent) continue;
            lastEvent = lastEvent - historyDepth;
            int nIndex = obj.GetIdxAtTime (lastEvent);
            if (nIndex > 0) obj.RemoveRange (0, nIndex + 1);
        }

    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }
    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Обновляет хранимые на сервере описания о мобильных клиентов.
/// </summary>
/// <param name="clientInfo">Новые описания мобильных клиентов.</param>
/// <returns>true, если операция была выполнена успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool UpdateMobileClientInfo (MobileClientList clientInfo)
{
    if (! IsConnected ()) 
    {
        m_strLastError = "Не установлено соединение с сервером.";
        return false;
    }

    try
    {
        //
        //Подготавливаем буфер с данными, которые нужно зашифровать.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        int nClientCount = clientInfo.Count;
        Utils.Write (secretStream, nClientCount);

        for (int nIdx = 0; nIdx < nClientCount; ++nIdx)
        {
            MobileClientInfo info = clientInfo [nIdx];
            if ( !info.SaveGuts (secretStream)) return false;
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //Отправляем запрос.
        //

        Stream response;
        if (! SendRequest (RequestType.rtUpdateMobileClientInfo, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //Ответ читать не нужно.
        //

        response.Close ();

    }
    catch (Exception e)
    {
        m_strLastError = "Неожиданная ошибка: " + e.Message;
        return false;
    }
    return true;            
}

//
//Ключ для шифрования запроса на установление соединения.
//

public byte [] KeyToConnect {get {return m_KeyToConnect;} set {m_KeyToConnect = value;}}
protected byte [] m_KeyToConnect;

//
//Ключ для шифрования отправляемых данных.
//

public byte [] EncryptionKey {get {return m_EncryptionKey;} set {m_EncryptionKey = value;}}
protected byte [] m_EncryptionKey;

//
//Ключ для шифрования принимаемых данных.
//

public byte [] DecryptionKey {get {return m_DecryptionKey;} set {m_DecryptionKey = value;}}
protected byte [] m_DecryptionKey;

//
//Идентификатор невалидного соединения с сервером.
//

public const int m_nInvalidConnectionId = -1;

//
//Идентификатор соединения с сервером.
//

public int ConnectionId {get {return m_nConnectionId;} set {m_nConnectionId = value;}}
protected int m_nConnectionId;

//
//Логин.
//

public string Login {get {return m_strLogin;} set {m_strLogin = value;}}
protected string m_strLogin;

//
//Домен.
//

public string Domain {get {return m_strDomain;} set {m_strDomain = value;}}
protected string m_strDomain;

//
//Пароль.
//

public string Password {get {return m_strPassword;} set {m_strPassword = value;}}
protected string m_strPassword;

//
//Описание последней ошибки.
//

protected string m_strLastError;

//
//Версия партокола взаимодействия с сервером.
//

protected int m_Version;

//
//Случайное значение, идентифицирующе запрос.
//

protected int m_nRandomValue;

//
//Генератор случайных чисел.
//

protected Random m_Random;
}
}
