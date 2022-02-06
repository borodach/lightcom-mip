////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           SettingsManager.cs
//
//  Facility:       ���������� �����������.
//
//
//  Abstract:       ������ �������� �����, ����������� ��������� � �����������
//                  �������� ���� �������� ����������.
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
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
 * ��������� ��������� FilePublisher
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
/// ����� ��������� ���� ������������ ��� ���������� ISettingsStorage.
/// � ��� �������� ������ ���������� ����� ����������. ��� ������ 
/// ������� ISettingsStorage ����������� ����� ���� ������� ��� ������ 
/// ����������, �������� � SettingsManager.
/// </summary>
///
 
public class SettingsManager : ISettings 
{
////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// �����������.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

internal SettingsManager ()
{
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� ��������� ���� ��������, ������� ����������� ������ 
/// ��������.
/// </summary>
/// <param name="storage">��������� ��� ���������� ��������.</param>
/// <returns>���������� true, ���� ���������� ���� �������� ������
/// �������.
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
/// ��������� ��������� ���� ��������, ������� ����������� ������ 
/// ��������.
/// </summary>
/// <param name="storage">��������� ��� �������� ��������.</param>
/// <returns>���������� true, ���� �������� ���� �������� ������
/// �������.
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
/// ���������� ��������� ���� �������� � ��������� ���������.
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
/// ��������� ��������� ���� ��������, ������� ����������� ������ 
/// ��������.
/// </summary>
/// <returns>���������� true, ���� ���������� ���� �������� ������
/// �������.
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
/// ��������� ��������� ���� ��������, ������� ����������� ������ 
/// ��������.
/// </summary>
/// <returns>���������� true, ���� �������� ���� �������� ������
/// �������.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Load () 
{
    return m_Storage.PreLoad () && Load (m_Storage);
}


///
/// <summary>
/// ����������� ��������� ������.
/// </summary>
/// 

public static SettingsManager instance = null;

/// 
/// <summary>
/// ������ ����������� ��������.
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
/// ��������� ��������.
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