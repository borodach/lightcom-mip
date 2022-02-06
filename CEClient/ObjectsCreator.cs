////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           ObjectsCreator.cs
//
//  Facility:       Создание статических объектов.
//
//
//  Abstract:       Класс обеспечивает правильный порядок создания статических 
//                  объектов.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  19/09/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: ObjectsCreator.cs $
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 10.07.07   Time: 7:59
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 4.03.07    Time: 17:02
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 18.11.06   Time: 19:06
 * Updated in $/gps/EndPoint
 * Добавлена поддержка FilePublisher
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:39
 * Updated in $/gps/EndPoint
 * 
 */

using System;
using LightCom.Common;
using LightCom.MiP.Common;

namespace LightCom.MiP.CEClient
{

/// 
/// <summary>
/// Класс обеспечивает правильный порядок создания статических объектов.
/// </summary>
/// 

public class ObjectsCreator
{

    public static GPSTransmitter transmitter;

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Метод создает статические объекты.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////
    
public ObjectsCreator ()
{
    //
    // Создаем объекты.
    //

    HTTPPublisher.instance = new HTTPPublisher ();

    if (System.Environment.OSVersion.Version.Major >= 5)
    {
        transmitter = new LightCom.MiP.CEClient.WM5GPSTransmitter (HTTPPublisher.instance);
    }
    else
    {
        transmitter = new LightCom.MiP.CEClient.COMTransmitter (HTTPPublisher.instance);
    }

    FilePublisher.instance = new FilePublisher ();
    SettingsManager.instance = new SettingsManager ();

    //
    // Загружаем их настройки.
    //

    SettingsManager.instance.Load ();
}

/// 
/// <summary>
/// Статический экземпляр класса.
/// </summary>
/// 

public static ObjectsCreator instance = new ObjectsCreator ();
}
}


