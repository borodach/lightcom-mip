///////////////////////////////////////////////////////////////////////////////
//
//  File:           PlaybackControl.cs
//
//  Facility:       Управление отображением GPS данных.
//
//
//  Abstract:       Базовый класс, управляющий отображением GPS данных.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  15-11-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.MiP.Cache;

namespace LightCom.MiP.Dispatcher.Common
{
/// 
/// <summary>
/// Summary description for PlaybackControl.
/// </summary>
///

public class PlaybackControl
{
////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Default constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public PlaybackControl ()
{
    m_Cache = null;
    m_CurrentTime = DateTime.MinValue;
    m_bStopped = true;
    m_LastProcessedEvent = DateTime.MinValue;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Делегат обработки изменения текущего времени.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public delegate void TimeChangedEventHandler (PlaybackControl source, 
                                              DateTime oldTime,
                                              DateTime newEvent);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Событие "изменилось текущее время".
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public event TimeChangedEventHandler TimeChanged;

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

public virtual void ProcessTimerEvent (TimeSpan span)
{
    if (! Stopped) CurrentTime = CurrentTime + span;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// GPS cache accessor and mutator.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public GPSDataCache Cache
{
    get {return m_Cache;}
    set 
    {
        m_Cache = value;
        CurrentTime = StartTime;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Current time accessor and mutator.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

virtual public DateTime CurrentTime
{
    get {return m_CurrentTime;}
    set 
    {
        DateTime startTime = StartTime;
        DateTime finishTime = FinishTime;

        if (value < startTime) value = startTime;
        else if (value > finishTime) value = finishTime;

        if (value == m_CurrentTime) return;
        DateTime oldTime = m_CurrentTime;
        m_CurrentTime = value;
        if (TimeChanged != null && m_Cache != null) 
        {
            DateTime newEvent = m_Cache.GetEvent (m_CurrentTime);
            TimeChanged (this, oldTime, newEvent);
            if (null != m_Cache)
            {
                m_LastProcessedEvent = newEvent;
            }
        }

    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Start time.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public DateTime StartTime
{
    get 
    {
        if (null == m_Cache) return DateTime.MinValue;
        else return m_Cache.FirstEvent;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Finish time.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public DateTime FinishTime
{
    get 
    {
        if (null == m_Cache) return DateTime.MaxValue;
        else return m_Cache.LastEvent;
    }
}

//
// Data members.
//

/// 
/// <summary>
/// GPS cache.
/// </summary>
/// 

protected GPSDataCache m_Cache;

/// 
/// <summary>
/// Current time.
/// </summary>
/// 

protected DateTime m_CurrentTime;

/// 
/// <summary>
/// Остановлено ли проигрывание.
/// </summary>
/// 

virtual public bool Stopped {get {return m_bStopped;}set {m_bStopped = value;}}
protected bool m_bStopped;

/// 
/// <summary>
/// Время последнего события для которого вызывался 
/// TimeChangedEventHandler. Если это значение изменится, то текущие
/// позиции клиентов изменились.
/// </summary>
/// 

public DateTime LastProcessedEvent {get {return m_LastProcessedEvent;} set {m_LastProcessedEvent = value;}}
protected DateTime m_LastProcessedEvent;
}
}
