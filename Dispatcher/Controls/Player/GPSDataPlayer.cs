///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSDataPlayer.cs
//
//  Facility:       ���������� ������������ GPS ������.
//
//
//  Abstract:       ������������ �������� � ���� ���������� ��������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
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
/// ������������ �������� � ���� ���������� ��������.
/// </summary>
/// 

public class GPSDataPlayer : PlaybackControl
{
/// 
/// <summary>
/// ����������� ������������.
/// </summary>
/// 

public enum PlaybackDirectionEnum
{
    pdForward,
    pdBackward
};

/// 
/// <summary>
/// ����� ������������ (�� ������� ��� �� ��������).
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
// ���������.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� � �������� ������ �������.
/// </summary>
/// <param name="time">����� ������� �����.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void MoveToTime (DateTime time)
{
    CurrentTime = time;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� �� ��������� �������.
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
/// ��������� �� ���������� �������.
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
/// ��������� � ������.
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
/// ��������� � �����.
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
/// ����� ������������ (�� ������� ��� �� ��������) ��-���������.
/// </summary>
///

public static PlaybackModeEnum DefaultPlaybackMode = PlaybackModeEnum.pmTimeBased;

/// 
/// <summary>
/// �������� ������������ ��-��������� � time based ���� ������������ 
/// (��������� �������� ����� ���������� �� ��� �������).
/// </summary>
///


public static double DefaultTimeBasedPlaybackSpeed = 1.0;

/// 
/// <summary>
/// �������� ��-��������� ����� ��������� � event based ������ 
/// ������������.
/// </summary>
///

public static TimeSpan DefaultEventBasedPlaybackDelay = TimeSpan.FromSeconds (1.0);

/// 
/// <summary>
/// ����������� ������������.
/// </summary>
///

public PlaybackDirectionEnum PlaybackDirection {get {return m_PlaybackDirection;} set {m_PlaybackDirection = value;}}
protected PlaybackDirectionEnum m_PlaybackDirection;

/// 
/// <summary>
/// ����� ������������ (�� ������� ��� �� ��������).
/// </summary>
///

public PlaybackModeEnum PlaybackMode {get {return m_PlaybackMode;} set {m_PlaybackMode = value;}}
protected PlaybackModeEnum m_PlaybackMode;

/// 
/// <summary>
/// �����, ��������� ����� ��������� ������� � event based ������.
/// </summary>
///


protected TimeSpan m_TimeAfterLastRefresh;

/// 
/// <summary>
/// �������� ������������ � time based ���� ������������ (��������� 
/// �������� ����� ���������� �� ��� �������).
/// </summary>
///

public double TimeBasedPlaybackSpeed {get {return m_TimeBasedPlaybackSpeed;}set {m_TimeBasedPlaybackSpeed = value;}}
protected double m_TimeBasedPlaybackSpeed;

/// 
/// <summary>
/// �������� ����� ��������� � event based ������ ������������.
/// </summary>
///

public TimeSpan EventBasedPlaybackDelay {get {return m_EventBasedPlaybackDelay;} set {m_EventBasedPlaybackDelay = value;}}
protected TimeSpan m_EventBasedPlaybackDelay;
}
}
