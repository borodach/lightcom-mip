///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSDataReceiver.cs
//
//  Facility:       Управление отображением GPS данных.
//
//
//  Abstract:       Отображение самых свежих данных в режиме "радиоприемника".
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  15-11-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using GPS.Common;
using GPS.Dispatcher.DataSource;

namespace GPS.Dispatcher.Controls
{
/// 
/// <summary>
/// Отображение самых свежих данных в режиме "радиоприемника".
/// </summary>
///

public class GPSDataReceiver: PlaybackControl
{
////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Default constructor.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public GPSDataReceiver () : base ()
{
    m_RefreshFreq = DefaultRefreshFreq;
    m_TimeAfterLastRefresh = TimeSpan.Zero;
    m_DataSource = null;
}

//
// Main business logic.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Для работы в режиме play необходимо с некоторой периодичностью
/// обновлять отображанмые данные. Этот класс и его наследники не
/// имеют встроенного таймера, вместо него нужен внешний таймер, 
/// вызывающй данную функцию с периодичностью, достаточной для 
/// отображения данных с приемлемой скоростью.
/// </summary>
/// <param name="span">Время, прошедшее с момента предыдущего вызова 
/// этого метода. Это реальное время, а не playback время. Классы сами
/// выполняют необходимое изменение масштаба времени, направления 
/// движения, способа изменения.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public override void ProcessTimerEvent (TimeSpan span)
{
    if (Stopped) return;
    m_TimeAfterLastRefresh += span;
    if (m_TimeAfterLastRefresh >= m_RefreshFreq)
    {
        m_TimeAfterLastRefresh = TimeSpan.Zero;
        Refresh ();
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Обновляет GPS данные, загружая их из DataSource.
/// </summary>
/// <returns>true, если операция была выполнена успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Refresh ()
{
    if (null == Cache) return false;
    if (null == m_DataSource) return false;
    bool bResult = m_DataSource.ReadLatestGPSData (Cache, HistoryDepth);
    if (! bResult) return bResult;
    CurrentTime = Cache.LastEvent;

    return true;
}

//
// Data members.
//

/// 
/// <summary>
/// Частота обновления данных по-умолчанию.
/// </summary>
/// 

public static TimeSpan DefaultRefreshFreq = TimeSpan.FromSeconds (30);

/// 
/// <summary>
/// Время, прошедшее после последнего обновления данных.
/// </summary>
///

public TimeSpan TimeAfterLastRefresh {get {return m_TimeAfterLastRefresh;} set {m_TimeAfterLastRefresh = value;}}
protected TimeSpan m_TimeAfterLastRefresh;

/// 
/// <summary>
/// Частота обновления данных.
/// </summary>
///

public TimeSpan RefreshFreq {get {return m_RefreshFreq;} set {m_RefreshFreq = value;}}
protected TimeSpan m_RefreshFreq;

/// 
/// <summary>
/// Величина интервала "истории" перемещений.
/// </summary>
///

public TimeSpan HistoryDepth {get {return m_HistoryDepth;} set {m_HistoryDepth = value;}}
protected TimeSpan m_HistoryDepth;

/// 
/// <summary>
/// Интерфейс для обновления данных.
/// </summary>
///

public IGPSDataSource DataSource {get {return m_DataSource;} set {m_DataSource = value;}}
protected IGPSDataSource m_DataSource;
}
}
