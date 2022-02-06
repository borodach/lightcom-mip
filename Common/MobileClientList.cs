////////////////////////////////////////////////////////////////////////////////
//
//  File:           MobileClientList.cs
//
//  Facility:       Mobile client representation.
//
//
//  Abstract:       ������ ������� ��������� ��������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  18-11-2005
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;

namespace LightCom.MiP.Common
{
///
///<summary>
/// ������ ������� ��������� ��������.
///</summary>
///

public class MobileClientList
{
///<link>aggregation</link>
///<supplierCardinality>0..*</supplierCardinality>

/*# GPS.Common.MobileClientInfo lnkMobileClientInfo; */

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������ � ��������� ���������.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public void Reset()
{
    m_Storage.Clear ();
}

//
// Access to storage.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� �������� � ��������� �������. ���� � ������ �������
///  ��� ���� �������� �� ���� �������, �� ��� ����������������
/// - ��������� � ��������� �� ���������. ����� ���������� ��������� 
/// ����������� �� �������.
/// </summary>
/// <param name="obj">�������� � ��������� �������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (MobileClientInfo obj)
{
    int nIdx = m_Storage.BinarySearch (0, m_Storage.Count, obj, null);
    if (nIdx >= 0)
    {
        m_Storage [nIdx] = obj;
    }
    else
    {
        m_Storage.Add (obj);
        m_Storage.Sort ();
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������� �������� � ��������� ������� �� �������.
/// </summary>
/// <param name="idx">������ ���������� �������.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Remove (int idx)
{
    m_Storage.RemoveAt (idx);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� �������� � ��������� ��������.
/// </summary>
/// <param name="clients">C������� � ��������� ��������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (MobileClientList clients)
{
    int count = clients.Count;
    for (int idx = 0; idx < count; ++ idx)
    {
        Add (clients [idx]);
    }
}

//
// Properties
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� �������� MobileClientInfo, �������� � ������ �������.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public int Count
{
    get {return m_Storage.Count;}
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������ MobileClientInfo � �������� idx .
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public MobileClientInfo this [int idx]
{
    get {return m_Storage [idx] as MobileClientInfo;}
    set {m_Storage [idx] = value; m_Storage.Sort ();}
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// �������� � ��������� ������� � �������� ���������������.
/// </summary>
/// <param name="time">�����.</param>
///
////////////////////////////////////////////////////////////////////////////////

public MobileClientInfo this [string strClientId]
{
    get
    {
        MobileClientInfo obj = new MobileClientInfo ();
        obj.ClientId = strClientId;

        int nIdx = m_Storage.BinarySearch (0, m_Storage.Count, obj, null);
        if (nIdx >= 0) return m_Storage [nIdx] as MobileClientInfo;
        else return null;
    }
    set
    {
        Add (value as MobileClientInfo);
    }
}

//
// Data members.
//

///
/// <summary>
/// ������������� �� ������� ������ �������� ��������� ��������.
/// </summary>
///

protected ArrayList m_Storage = new ArrayList ();

///<link>aggregation</link>
///<supplierCardinality>0..1</supplierCardinality>
}
}
