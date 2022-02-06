///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSDataReceiver.cs
//
//  Facility:       ���������� ������������ GPS ������.
//
//
//  Abstract:       ����������� ����� ������ ������ � ������ "��������������".
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
using GPS.Common;
using GPS.Dispatcher.DataSource;

namespace GPS.Dispatcher.Controls
{
/// 
/// <summary>
/// ����������� ����� ������ ������ � ������ "��������������".
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
/// ��������� GPS ������, �������� �� �� DataSource.
/// </summary>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
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
/// ������� ���������� ������ ��-���������.
/// </summary>
/// 

public static TimeSpan DefaultRefreshFreq = TimeSpan.FromSeconds (30);

/// 
/// <summary>
/// �����, ��������� ����� ���������� ���������� ������.
/// </summary>
///

public TimeSpan TimeAfterLastRefresh {get {return m_TimeAfterLastRefresh;} set {m_TimeAfterLastRefresh = value;}}
protected TimeSpan m_TimeAfterLastRefresh;

/// 
/// <summary>
/// ������� ���������� ������.
/// </summary>
///

public TimeSpan RefreshFreq {get {return m_RefreshFreq;} set {m_RefreshFreq = value;}}
protected TimeSpan m_RefreshFreq;

/// 
/// <summary>
/// �������� ��������� "�������" �����������.
/// </summary>
///

public TimeSpan HistoryDepth {get {return m_HistoryDepth;} set {m_HistoryDepth = value;}}
protected TimeSpan m_HistoryDepth;

/// 
/// <summary>
/// ��������� ��� ���������� ������.
/// </summary>
///

public IGPSDataSource DataSource {get {return m_DataSource;} set {m_DataSource = value;}}
protected IGPSDataSource m_DataSource;
}
}
