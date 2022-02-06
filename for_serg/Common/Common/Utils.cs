///////////////////////////////////////////////////////////////////////////////
//
//  File:           Utils.cs
//
//  Facility:       Common utils.
//
//
//  Abstract:       Разные полезные методы.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  11-11-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Text;
namespace GPS.Common
{
/// 
/// <summary>
/// Разные полезные методы.
/// </summary>
///

public class Utils
{

/// 
/// <summary>
/// Возвращает плное имя образа сборки.
/// </summary>
/// <returns>Возвращает плное имя образа сборки.
/// </returns>
/// 

public static string GetExePath ()
{
    System.Reflection.Assembly asm = 
        System.Reflection.Assembly.GetExecutingAssembly ();
    if (null != asm)
    {
        System.Reflection.Module [] modules = asm.GetModules ();
        if (null != modules && modules.Length > 0)
        {
            string strPath = modules [0].FullyQualifiedName;
            return strPath;                        
        }
    }

    return "";
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает папку, которой находится образ сборки.
/// </summary>
/// <returns>Возвращает папку,  которой находится образ сборки.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static string GetExeDirectory ()
{
    string strPath = GetExePath ();
    int nIdx = strPath.LastIndexOf ('\\');
    if (nIdx >= 0) 
    {
        return strPath.Substring (0, nIdx);                        
    }

    return "";
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Записывает целое число в поток.
/// </summary>
/// <param name="stream">Поток для записи.</param>
/// <param name="strData">Записываемое число.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public static void Write (Stream stream, uint nData)
{
    byte [] data = BitConverter.GetBytes (nData);            
    stream.Write (data, 0, data.Length);            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Читает целое число из потока.
/// </summary>
/// <param name="stream">Поток для чтения.</param>
/// <returns>Прочитанное число.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static uint ReadUInt (Stream stream)
{
    uint nResult = 0;
    byte [] data = BitConverter.GetBytes (nResult);            
    stream.Read (data, 0, data.Length);            
    nResult = BitConverter.ToUInt32 (data, 0);
    return nResult;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Записывает целое число в поток.
/// </summary>
/// <param name="stream">Поток для записи.</param>
/// <param name="strData">Записываемое число.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public static void Write (Stream stream, int nData)
{
    byte [] data = BitConverter.GetBytes (nData);            
    stream.Write (data, 0, data.Length);            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Читает целое число из потока.
/// </summary>
/// <param name="stream">Поток для чтения.</param>
/// <returns>Прочитанное число.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static int ReadInt (Stream stream)
{
    int nResult = 0;
    byte [] data = BitConverter.GetBytes (nResult);            
    stream.Read (data, 0, data.Length);            
    nResult = BitConverter.ToInt32 (data, 0);

    return nResult;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Записывает строчку в поток.
/// </summary>
/// <param name="stream">Поток для записи строки.</param>
/// <param name="strData">Записываемая строка.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public static void Write (Stream stream, string strData)
{
    byte [] data;
    UTF8Encoding encoder = new UTF8Encoding ();
    data = encoder.GetBytes (strData);
    Write (stream, data.Length);            
    stream.Write (data, 0, data.Length);            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Записывает строчку в поток.
/// </summary>
/// <param name="stream">Поток для чтения строки.</param>
/// <returns>Прочитанная строка.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public static string ReadString (Stream stream)
{
    int nSize = ReadInt (stream);
    if (nSize < 0) 
    {
        throw new Exception ("Ошибка чтения строки из потока: размер строки отрицательный.");
    }
    byte [] data = new byte [nSize];
    stream.Read (data, 0, data.Length);
    UTF8Encoding encoder = new UTF8Encoding ();
    return encoder.GetString (data, 0, data.Length);                        
}

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// Записывает DateTime в поток.
/// </summary>
/// <param name="stream">Поток для записи.</param>
/// <param name="dateTime">DateTime</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public static void Write (Stream stream, DateTime dateTime)
{
    string strDateTime = dateTime.ToString (m_strDateTimeFormat);
    Write (stream, strDateTime);
}

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// Чтение DateTime из потока.
/// </summary>
/// <param name="stream">Поток для чтения.</param>
/// <returns>Прочитанное значение.</returns> 
/// 
////////////////////////////////////////////////////////////////////////////////

public static DateTime ReadDateTime (Stream stream)
{
    string strDateTime = ReadString (stream);
    if (strDateTime.Length != m_strDateTimeFormat.Length)
    {
        throw new Exception ("Ошибка чтения даты и времени из потока: неправильный размер (\"" +
            strDateTime + "\").");    
    }

    //  01234567890123456789012
    //("dd.MM.yyyy HH:mm:ss.fff");
    int nDay = Convert.ToInt32 (strDateTime.Substring (0, 2));
    int nMonth = Convert.ToInt32 (strDateTime.Substring (3, 2));
    int nYear = Convert.ToInt32 (strDateTime.Substring (6, 4));
    int nHour = Convert.ToInt32 (strDateTime.Substring (11, 2));
    int nMinute = Convert.ToInt32 (strDateTime.Substring (14, 2));
    int nSecond = Convert.ToInt32 (strDateTime.Substring (17, 2));
    int nMillisecond = Convert.ToInt32 (strDateTime.Substring (20, 3));
    return new DateTime (nYear, nMonth, nDay, nHour, nMinute, nSecond, nMillisecond);

        
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

public static int CalculateCRC (byte [] data)
{
    int nSize = data.Length;
    int nResult = 0;
    for (int i = 0; i < nSize; ++ i) nResult += (int) data [i];
    return nResult;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает буфер с данными, помещенными в MemoryStream.
/// </summary>
/// <param name="ms">MemoryStream</param>
/// <returns>Возвращает буфер с данными, помещенными в MemoryStream.</returns>
///
////////////////////////////////////////////////////////////////////////////////
 
public static byte [] GetStreamData (MemoryStream ms)
{
    int nStreamSize = (int) ms.Length;
    byte [] secretData = new byte [nStreamSize];
    byte [] streamBuffer = ms.GetBuffer ();
    for (int  i = 0; i < nStreamSize; ++i)
    {
        secretData [i] = streamBuffer [i];
    }
    return secretData;
}

// 
//Формат даты и времени.
//
    
public const string m_strDateTimeFormat = "dd.MM.yyyy HH:mm:ss.fff";

}
}
