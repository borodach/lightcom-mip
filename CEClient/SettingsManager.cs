////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           SettingsManager.cs
//
//  Facility:       Управление настройками.
//
//
//  Abstract:       Модуль содержит класс, управляющий загрузкой и сохранением
//                  настроек всех объектов приложения.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  13/09/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: SettingsManager.cs $
 * 
 * *****************  Version 9  *****************
 * User: Sergey       Date: 10.07.07   Time: 7:59
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 4.03.07    Time: 17:02
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 18.11.06   Time: 19:06
 * Updated in $/gps/EndPoint
 * Добавлена поддержка FilePublisher
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:39
 * Updated in $/gps/EndPoint
 * 
 */

using System;
using LightCom.Common;

namespace LightCom.MiP.CEClient
{
/// 
/// <summary>
/// Класс выполняет роль разветвителя для интерфейса ISettingsStorage.
/// В нем хранится массив реализаций этого интерфейса. При вызове 
/// методов ISettingsStorage выполняется вызов этих методов для каждой 
/// реализации, хранимой в SettingsManager.
/// </summary>
///
 
public class SettingsManager : ISettings 
{
////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Конструктор.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

internal SettingsManager ()
{
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Сохраняет настройки всех объектов, которые управляются данным 
/// объектом.
/// </summary>
/// <param name="storage">Хранилище для сохранения настроек.</param>
/// <returns>Возвращает true, если сохранение всех настроек прошло
/// успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////
    
public bool Save (SettingsStorage storage)
{
    bool bResult = true;
    int nSize = m_Objects.Length;
    for (int nIdx = 0; nIdx < nSize; ++ nIdx)
    {
        ISettings settings = m_Objects [nIdx];
        if (null == settings) continue;
        bResult &= settings.Save (storage);
    }

    return bResult;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Загружает настройки всех объектов, которые управляются данным 
/// объектом.
/// </summary>
/// <param name="storage">Хранилище для загрузки настроек.</param>
/// <returns>Возвращает true, если загрузка всех настроек прошла
/// успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Load (SettingsStorage storage)
{
    bool bResult = true;
    int nSize = m_Objects.Length;
    for (int nIdx = 0; nIdx < nSize; ++ nIdx)
    {
        ISettings settings = m_Objects [nIdx];
        if (null == settings) continue;
        bResult &= settings.Load (storage);
    }

    return bResult;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Сбрасывает состояние всех объектов в начальное состояние.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Reset ()
{
    int nSize = m_Objects.Length;
    for (int nIdx = 0; nIdx < nSize; ++ nIdx)
    {
        ISettings settings = m_Objects [nIdx];
        if (null == settings) continue;
        settings.Reset ();
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Сохраняет настройки всех объектов, которые управляются данным 
/// объектом.
/// </summary>
/// <returns>Возвращает true, если сохранение всех настроек прошло
/// успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Save () 
{
    return Save (m_Storage) && m_Storage.Flush ();
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Загружает настройки всех объектов, которые управляются данным 
/// объектом.
/// </summary>
/// <returns>Возвращает true, если загрузка всех настроек прошла
/// успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Load () 
{
    return m_Storage.PreLoad () && Load (m_Storage);
}


///
/// <summary>
/// Статический экземпляр класса.
/// </summary>
/// 

public static SettingsManager instance = null;

/// 
/// <summary>
/// Массив управляемых объектов.
/// </summary>
///
 
protected ISettings [] m_Objects = new ISettings [] 
{
    HTTPPublisher.instance, 
    ObjectsCreator.transmitter,
    FilePublisher.instance,
};

//
/// <summary>
/// Хранилище настроек.
/// </summary>
/// 

public SettingsStorage Storage 
{
    get {return this.m_Storage;}
    set {m_Storage = value;}
}

protected SettingsStorage m_Storage = new XMLSetingsStorage ();
}
}