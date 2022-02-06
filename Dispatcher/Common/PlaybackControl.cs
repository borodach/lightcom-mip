///////////////////////////////////////////////////////////////////////////////
//
//  File:           PlaybackControl.cs
//
//  Facility:       ���������� ������������ GPS ������.
//
//
//  Abstract:       ������� �����, ����������� ������������ GPS ������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
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
/// ������� ��������� ��������� �������� �������.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public delegate void TimeChangedEventHandler (PlaybackControl source, 
                                              DateTime oldTime,
                                              DateTime newEvent);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������� "���������� ������� �����".
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public event TimeChangedEventHandler TimeChanged;

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��� ������ � ������ play ���������� � ��������� ��������������
/// ��������� ������������ ������. ���� ����� � ��� ���������� ��
/// ����� ����������� �������, ������ ���� ����� ������� ������, 
/// ��������� ������ ������� � ��������������, ����������� ��� 
/// ����������� ������ � ���������� ���������.
/// </summary>
/// <param name="span">�����, ��������� � ������� ����������� ������ 
/// ����� ������. ��� �������� �����, � �� playback �����. ������ ����
/// ��������� ����������� ��������� �������� �������, ����������� 
/// ��������, ������� ���������.</param>
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
/// ����������� �� ������������.
/// </summary>
/// 

virtual public bool Stopped {get {return m_bStopped;}set {m_bStopped = value;}}
protected bool m_bStopped;

/// 
/// <summary>
/// ����� ���������� ������� ��� �������� ��������� 
/// TimeChangedEventHandler. ���� ��� �������� ���������, �� �������
/// ������� �������� ����������.
/// </summary>
/// 

public DateTime LastProcessedEvent {get {return m_LastProcessedEvent;} set {m_LastProcessedEvent = value;}}
protected DateTime m_LastProcessedEvent;
}
}
