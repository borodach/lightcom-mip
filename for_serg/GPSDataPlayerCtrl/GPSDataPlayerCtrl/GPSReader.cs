////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           GPSReader.cs
//
//  Facility:       Чтение данных GPS приемника.
//
//
//  Abstract:       Модуль содержит полезные функции.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  24/10/2005
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.Globalization;

namespace GPS.Common
{
///
/// <summary>
/// Чтение данных из GPS приемника
/// </summary>
/// 

public class GPSReader
{

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Извлекает координаты из RMC команды.
/// </summary>
/// <param name="strData">RMC команда</param>
/// <param name="fLongitude">Долгота.</param>
/// <param name="fLatitude">Широта.</param>
/// <returns>true if operation completed sucessfully</returns>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public static bool GetPosition (string strData, 
    out double fLongitude, 
    out double fLatitude,
    out double fSpeed)
{
    fSpeed = 0;
    fLongitude = 0;
    fLatitude = 0;
    try
    {
        NumberFormatInfo provider = new NumberFormatInfo ();
        provider.NumberDecimalSeparator = ".";
        provider.NumberGroupSeparator = " ";

        if (! IsGPSDataValid (strData)) return false;

        char [] separator = new char [1];
        separator [0] = ',';
        char [] zeros = new char [1];
        zeros [0] = '0';
        
        string [] parsedData = strData.Split (separator);
        parsedData [5].TrimStart (zeros);
        fLongitude = Convert.ToDouble (parsedData [5], provider);
        /*
        fLongitude = (int) (nTmp / 100) + ((nTmp % 100) / 60.0) + 
            (fLongitude - nTmp) / 36;
        */    

        uint nDeg = (uint) (((uint) fLongitude) / 100);
        fLongitude = nDeg + ((fLongitude - nDeg * 100) / 60.0);

        if (parsedData [6] == "S") fLongitude = - fLongitude;

        parsedData [3].TrimStart (zeros);
        
        fLatitude = Convert.ToDouble (parsedData [3], provider);
        /*
        fLatitude = (int) (nTmp / 100) + ((nTmp % 100) / 60.0) + 
            (fLatitude - nTmp) / 36;
        */ 
        nDeg = (uint) (((uint) fLatitude) / 100);
        fLatitude = nDeg + ((fLatitude - nDeg * 100) / 60.0);

        if (parsedData [4] == "W") fLatitude = - fLatitude;

        parsedData [5].TrimStart (zeros);
        fSpeed = Convert.ToDouble (parsedData [7], provider);
        fSpeed = fSpeed * 1.853;

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
/// Check if GPS data is valid.
/// </summary>
/// <param name="strData">GPD data.</param>
/// <returns>Check result.</returns>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public static bool IsGPSDataValid (string strData)
{
    try
    {
        char [] separator = new char [1];
        separator [0] = ',';
        string [] parsedData = strData.Split (separator);
        if (parsedData [0] != "$GPRMC") return false;
        if (parsedData [2] != "A") return false;
        if (parsedData [3] == "" || parsedData [4] == "" ||
            parsedData [5] == "" || parsedData [6] == "") return false;
    
        int checSum = 0;
        for (int i = 1; strData [i] != '*'; ++ i)
        {
            checSum ^= (int) (strData [i]);
        }
    
        string strChecSum = '*' + System.Uri.HexEscape  ((char)checSum).Substring (1);
    
        if ( ! strData.EndsWith (strChecSum)) return false;
    }
    catch (Exception)
    {
        return false;
    }

    return true;
}
}
}