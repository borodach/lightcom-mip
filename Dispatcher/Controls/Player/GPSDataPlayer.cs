///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSDataPlayer.cs
//
//  Facility:       Управление отображением GPS данных.
//
//
//  Abstract:       Проигрывание хранимых в кэше перемщений клиентов.
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
using LightCom.Common;
using LightCom.MiP.Dispatcher.Common;

namespace LightCom.MiP.Dispatcher.Controls
{
///
/// <summary>
/// Проигрывание хранимых в кэше перемщений клиентов.
/// </summary>
/// 

public class GPSDataPlayer : PlaybackControl
{
/// 
/// <summary>
/// Направление проигрывания.
/// </summary>
/// 

public enum PlaybackDirectionEnum
{
    pdForward,
    pdBackward
};

/// 
/// <summary>
/// Режим проигрывания (по времени или по событиям).
/// </summary>
/// 

public enum PlaybackModeEnum
{
    pmTimeBased,
    pmEventBased
};

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Default c'tor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public GPSDataPlayer ()
{
    m_PlaybackDirection = PlaybackDirectionEnum.pdForward;
    m_PlaybackMode = DefaultPlaybackMode;
    m_TimeBasedPlaybackSpeed = DefaultTimeBasedPlaybackSpeed;
    m_EventBasedPlaybackDelay = DefaultEventBasedPlaybackDelay;            
    m_TimeAfterLastRefresh = TimeSpan.Zero;
}

//
// Main business logic.
//

//
// Перемотка.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Перемотка в заданный момент времени.
/// </summary>
/// <param name="time">Новое текущее время.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void MoveToTime (DateTime time)
{
    CurrentTime = time;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Перемотка на следующее событие.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public void MoveToNextEvent ()
{
    if (null != m_Cache) 
    {
        CurrentTime = m_Cache.GetNextEvent (CurrentTime);
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Перемотка на предыдущее событие.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public void MoveToPrevEvent ()
{
    if (null != m_Cache) 
    {
        CurrentTime = m_Cache.GetPrevEvent (CurrentTime);
    }
}

/// 
/// <summary>
/// Перемотка в начало.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public void MoveToStart ()
{
    if (null != m_Cache) 
    {
        CurrentTime = m_Cache.FirstEvent;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Перемотка в конец.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public void MoveToFinish ()
{
    if (null != m_Cache) 
    {
        CurrentTime = m_Cache.LastEvent;
    }
}

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

public override void ProcessTimerEvent (TimeSpan span)
{
    if (Stopped) return;
    switch (m_PlaybackMode)
    {
        case PlaybackModeEnum.pmTimeBased:
            span = new TimeSpan ((Int64) (span.Ticks * this.m_TimeBasedPlaybackSpeed));
            if (PlaybackDirectionEnum.pdForward == m_PlaybackDirection)
            {
                CurrentTime = CurrentTime + span;
            }
            else
            {
                CurrentTime = CurrentTime - span;
            }                   

            break;
        case PlaybackModeEnum.pmEventBased:
            m_TimeAfterLastRefresh += span;
            if (m_TimeAfterLastRefresh >= m_EventBasedPlaybackDelay)
            {
                m_TimeAfterLastRefresh = TimeSpan.Zero;
                if (PlaybackDirectionEnum.pdForward == m_PlaybackDirection)
                {
                    MoveToNextEvent ();
                }
                else
                {
                    MoveToPrevEvent ();
                }
            }
            break;
    }
}

//
// Data members.
//
    
/// 
/// <summary>
/// Режим проигрывания (по времени или по событиям) по-умолчанию.
/// </summary>
///

public static PlaybackModeEnum DefaultPlaybackMode = PlaybackModeEnum.pmTimeBased;

/// 
/// <summary>
/// Скорость проигрывания по-умолчанию в time based типе проигрывания 
/// (прошедшее реальное время умножается на это зачение).
/// </summary>
///


public static double DefaultTimeBasedPlaybackSpeed = 1.0;

/// 
/// <summary>
/// Задержка по-умолчанию между событиями в event based режиме 
/// проигрывания.
/// </summary>
///

public static TimeSpan DefaultEventBasedPlaybackDelay = TimeSpan.FromSeconds (1.0);

/// 
/// <summary>
/// Направление проигрывания.
/// </summary>
///

public PlaybackDirectionEnum PlaybackDirection {get {return m_PlaybackDirection;} set {m_PlaybackDirection = value;}}
protected PlaybackDirectionEnum m_PlaybackDirection;

/// 
/// <summary>
/// Режим проигрывания (по времени или по событиям).
/// </summary>
///

public PlaybackModeEnum PlaybackMode {get {return m_PlaybackMode;} set {m_PlaybackMode = value;}}
protected PlaybackModeEnum m_PlaybackMode;

/// 
/// <summary>
/// Время, прошедшее после изменения позиции в event based режиме.
/// </summary>
///


protected TimeSpan m_TimeAfterLastRefresh;

/// 
/// <summary>
/// Скорость проигрывания в time based типе проигрывания (прошедшее 
/// реальное время умножается на это зачение).
/// </summary>
///

public double TimeBasedPlaybackSpeed {get {return m_TimeBasedPlaybackSpeed;}set {m_TimeBasedPlaybackSpeed = value;}}
protected double m_TimeBasedPlaybackSpeed;

/// 
/// <summary>
/// Задержка между событиями в event based режиме проигрывания.
/// </summary>
///

public TimeSpan EventBasedPlaybackDelay {get {return m_EventBasedPlaybackDelay;} set {m_EventBasedPlaybackDelay = value;}}
protected TimeSpan m_EventBasedPlaybackDelay;
}
}
