///////////////////////////////////////////////////////////////////////////////
//
//  File:           HTTPDataSource.cs
//
//  Facility:       Взаимодействие с сервером.
//
//
//  Abstract:       Реализация протокола взаимодействия с сервером, используя 
//                  транспорт HTTP и простое шифрование.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  11-12-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Text;
using LightCom.MiP.Common;
using LightCom.Common;

namespace LightCom.MiP.DataSource
{

///
/// <summary>
/// Реализация протокола взаимодействия с сервером, используя транспорт HTTP и
/// простое шифрование.
/// </summary>
/// 

public class HTTPDataSource: BaseDataSource
{

/*
public static void Test ()
{
    HTTPDataSource ds = new HTTPDataSource ();
    FileStream fs = new FileStream ("eKey.dat", FileMode.Open);
    byte [] eKey = new byte [fs.Length];
    fs.Read (eKey, 0, eKey.Length);
    fs.Close ();
    ds.m_KeyToConnect = eKey;
    ds.Publisher.URL = "http://localhost/new/dgate.php";
    ds.Login = "admin";
    ds.Domain = "Administration";
    ds.Password = "password";
    ds.Connect ();
    string [] strClients;
    bool bResult = ds.GetMobileClientList (out strClients);
    strClients = new string [3];
    strClients [0] = "test";
    strClients [1] = "hello";
    strClients [2] = "client";
    GPS.Common.MobileClientList clientsInfo = null;
    bResult =ds.GetMobileClientsInfo (strClients, out clientsInfo);
    clientsInfo [0].FriendlyName = "\" Friendly Дружественное \'имя name\'\"";
    clientsInfo [0].Comments = "\"Комментарий Comment\'\"";
    GPS.Common.MobileClientInfo info = new GPS.Common.MobileClientInfo ();
    info.ClientId = "a";
    info.Comments = "Hello";
    clientsInfo.Add (info);
    info = new GPS.Common.MobileClientInfo ();
    info.ClientId = "test";
    info.Comments = "Hi";
    clientsInfo.Add (info);
        
    bResult = ds.UpdateMobileClientInfo (clientsInfo);

    DateTime first, last;
    bResult = ds.GetEvents (strClients, out first, out last);

    GPS.Dispatcher.Cache.GPSDataCache cache = new GPS.Dispatcher.Cache.GPSDataCache ();
    GPS.Dispatcher.Cache.ObjectPositions pos = new GPS.Dispatcher.Cache.ObjectPositions ();
    pos.ClientId = "test";
    cache.Add (pos);
    pos = new GPS.Dispatcher.Cache.ObjectPositions ();
    pos.ClientId = "client";
    cache.Add (pos);

    bResult = ds.ReadLatestGPSData (cache, TimeSpan.FromDays (3.0));
    System.Windows.Forms.MessageBox.Show (cache.ToString ());

    ds.Disconnect ();


    GPS.Dispatcher.DataSource.HTTPDataSource ds = new GPS.Dispatcher.DataSource.HTTPDataSource ();
    byte [] eKey;
    byte [] dKey;
    ds.GenerateKey (out eKey, out dKey);
    FileStream fs = new FileStream ("eKey.dat", FileMode.Create);
    fs.Write (eKey, 0, eKey.Length);
    fs.Close ();
    fs = new FileStream ("dKey.dat", FileMode.Create);
    fs.Write (dKey, 0, dKey.Length);
    fs.Close ();


    HTTPDataSource ds = new HTTPDataSource ();
    FileStream fs = new FileStream ("eKey.dat", FileMode.Open);
    byte [] eKey = new byte [fs.Length];
    fs.Read (eKey, 0, eKey.Length);
    fs.Close ();
    
    fs = new FileStream ("dKey.dat", FileMode.Open);
    byte [] dKey = new byte [fs.Length];
    fs.Read (dKey, 0, dKey.Length);
    fs.Close ();
    
    string strTest = "Тестовая строка";
    UTF8Encoding enc = new UTF8Encoding ();
    byte [] data;
    ds.Encrypt (enc.GetBytes (strTest), eKey, out data);
    byte [] decrypted;
    ds.Decrypt (data, dKey, out decrypted);
    string strResult = enc.GetString (decrypted);

    //ds.Connect ();
}
*/

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Конструктор по-умолчанию.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////
            
public HTTPDataSource()
{
    m_HTTPPublisher = new HTTPPublisher ();
}

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

public override bool Encrypt (byte [] dataToEncrypt, 
    byte [] key,
    out byte [] encryptedData)
{
    if (null == key || key.Length <= 0)
    {
        encryptedData = (byte []) dataToEncrypt.Clone ();
        return true;
    }

    int nDataSize = dataToEncrypt.Length;
    int nKeySize = key.Length;
    encryptedData = new byte [nDataSize];
    for (int i = 0; i < nDataSize; ++ i)
    {
        int j = i % nKeySize;
        encryptedData [i] = (byte) (dataToEncrypt [i] ^ key [j]);
    }

    return true;
}

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

public override bool Decrypt (byte [] dataToDecrypt, 
    byte [] key,
    out byte [] decryptedData)
{
    if (null == key || key.Length <= 0)
    {
        decryptedData = (byte []) dataToDecrypt.Clone ();
        return true;
    }

    int nDataSize = dataToDecrypt.Length;
    int nKeySize = key.Length;
    decryptedData = new byte [nDataSize];
    for (int i = 0; i < nDataSize; ++ i)
    {
        int j = i % nKeySize;
        decryptedData [i] = (byte) (dataToDecrypt [i] ^ (byte) (key [j] ^ m_nMask));        
    }

    return true;
}

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

public override bool GenerateKey (out byte [] encryptionKey, 
    out byte [] decryptionKey)
{
    Random rnd = new Random ();
    encryptionKey = new byte [m_nKeyLength];
    decryptionKey = new byte [encryptionKey.Length];
    rnd.NextBytes (encryptionKey);
    int nSize = encryptionKey.Length;
    for (int i = 0; i < nSize; ++ i) 
        decryptionKey [i] = (byte) (m_nMask ^ encryptionKey [i]);

    return true;
}

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

public override bool Send (byte [] dataToSend, 
    out Stream response)
{
    response = null;
    Stream httpResponse;
    byte [] data = dataToSend;//HTTPPublisher.bin2hex (dataToSend);
    bool bResult = m_HTTPPublisher.Publish (data, out httpResponse);
    if (! bResult) 
    {
        m_strLastError = m_HTTPPublisher.Status;

        return false;
    }
    /*
    byte [] buf = new byte [1024];
    httpResponse.Read (buf, 0, 1024);
    ASCIIEncoding enc = new ASCIIEncoding ();
    string res = enc.GetString (buf, 0, 1024);
    */

    try
    {
        MemoryStream ms = new MemoryStream ();
        do 
        {
            int nValue;
            int nByte = httpResponse.ReadByte ();
            if (-1 == nByte || 13 == nByte || 10 == nByte) break;
            if (nByte >= (int) '0' && nByte <= (int) '9') nByte -= (int) '0';
            else if (nByte >= (int) 'a' && nByte <= (int) 'f')
            {
                nByte -= (int) 'a';
                nByte += 10;
            }
            else if (nByte >= (int) 'A' && nByte <= (int) 'F')
            {
                nByte -= (int) 'A';
                nByte += 10;
            }
            else return false;
            nValue = nByte << 4;

            nByte = httpResponse.ReadByte ();
            if (-1 == nByte) return false;
            if (nByte >= (int) '0' && nByte <= (int) '9') nByte -= (int) '0';
            else if (nByte >= (int) 'a' && nByte <= (int) 'f')
            {
                nByte -= (int) 'a';
                nByte += 10;
            }
            else if (nByte >= (int) 'A' && nByte <= (int) 'F')
            {
                nByte -= (int) 'A';
                nByte += 10;
            }
            else return false;
            nValue |= nByte;

            ms.WriteByte ((byte) nValue);
        } 
        while (true);    
        response = ms;
        response.Seek (0, SeekOrigin.Begin);
    }
    finally
    {
        httpResponse.Close ();
    }

    return bResult;
}

/// 
/// <summary>
/// HTTP publisher, реализующий передачу данных по HTTP.
/// </summary>
/// 

public HTTPPublisher Publisher
{
    get {return m_HTTPPublisher;}
    set {m_HTTPPublisher = value;}
}
protected HTTPPublisher m_HTTPPublisher;

/// 
/// <summary>
/// Длина ключа.
/// </summary>
/// 

protected const int m_nKeyLength = 32;

/// 
/// <summary>
/// Маска дешифрования.
/// </summary>
///

protected const byte m_nMask = 0xAA;
}
}
