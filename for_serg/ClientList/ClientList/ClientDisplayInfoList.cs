////////////////////////////////////////////////////////////////////////////////
//
//  File:           ClientDisplayInfoList.cs
//
//  Facility:       ����������� ��������� ��������.
//
//
//  Abstract:       ��������� �������� ������� ����������� ��������� ��������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  18-11-2005
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;
using System.Collections;
using GPS.Common;


namespace GPS.Dispatcher.UI
{

/// 
/// <summary>
/// ��������� �������� ������� ����������� ��������� ��������.
/// </summary>
/// 

public class ClientDisplayInfoList
{

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ����� ��������� ������������ ������
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public static void Test ()
{
    Debug.WriteLine ("������������ ������ GPS.Dispatcher.UI.ClientDisplayInfoList.", "Info");

    ClientDisplayInfoList obj1 = new ClientDisplayInfoList ();
    Debug.WriteLine ("Default object:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    ClientDisplayInfo obj = new ClientDisplayInfo ();
    obj1.DefaultClientDisplayInfo = new ClientDisplayInfo ();
    obj1.DefaultClientDisplayInfo.ClientId = "Default display info";
    obj.ClientId = "Client#2";
    obj1.Add (obj);
    obj = new ClientDisplayInfo ();
    obj.ClientId = "Client#1";
    obj1.Add (obj);
    obj = new ClientDisplayInfo ();
    obj.ClientId = "Client#3";
    obj = new ClientDisplayInfo ();
    obj.ClientId = "Client#4";
    obj1 ["Client#1"] = obj;
 
    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    ClientDisplayInfoList obj2 = new ClientDisplayInfoList (obj1);
    Debug.WriteLine ("Clone (obj2):");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();

    obj2 [0].ClientId = "new name";

    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    Debug.WriteLine ("Clone (obj2):");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();

{
    MemoryStream writer = new MemoryStream ();
    Debug.WriteLine ("Persistance.");
    Debug.WriteLine ("obj1:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.WriteLine ("SaveGuts returned: " + obj1.SaveGuts (writer).ToString ());
    Debug.Unindent ();
    writer.Flush ();

    writer.Seek (0, SeekOrigin.Begin);
    ClientDisplayInfoList restoredObject = new ClientDisplayInfoList ();

    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
}

{
    ClientDisplayInfoList o = new ClientDisplayInfoList ();
    MemoryStream writer = new MemoryStream ();
    Debug.WriteLine ("Persistance of empty object.");

    Debug.WriteLine ("SaveGuts returned: " + o.SaveGuts (writer).ToString ());
    Debug.Unindent ();
    writer.Flush ();

    writer.Seek (0, SeekOrigin.Begin);
    ClientDisplayInfoList restoredObject = new ClientDisplayInfoList ();
    
    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
}

    Debug.WriteLine ("������������ ������ GPS.Dispatcher.UI.ClientDisplayInfoList ���������.", "Info");
}


////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ClientDisplayInfoList ()
{
    m_Storage = new ArrayList ();
    m_DefaultClientDisplayInfo = new ClientDisplayInfo ();
	m_displayInfoSettings = new DisplayInfoSettings();
	m_onlyGroupSettings = false;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Copy constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

ClientDisplayInfoList (ClientDisplayInfoList other)
{
    if (null != other.m_DefaultClientDisplayInfo) 
    {
        m_DefaultClientDisplayInfo = 
            other.m_DefaultClientDisplayInfo.Clone () as ClientDisplayInfo;
    }
    else
    {
        m_DefaultClientDisplayInfo = null;
    }

    int size = other.m_Storage.Count;
    m_Storage = new ArrayList (size);
    for (int idx = 0; idx < size; ++idx)
    {
        ClientDisplayInfo displayInfo = other.m_Storage [idx] as ClientDisplayInfo;
        if (null != displayInfo)
        {
            m_Storage.Add (displayInfo.Clone () as ClientDisplayInfo);
        }
        else
        {
            m_Storage [idx] = null;
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ��������� �������� �������.
/// </summary>
/// <returns>��������� �������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public override string ToString ()
{
    string strResult = string.Format ("Default: {0}; count: {1}",
        this.DefaultClientDisplayInfo, Count);
    int size = Count;
    for (int idx = 0; idx < size; ++idx)
    {
        strResult += string.Format ("\n{0}. ", idx + 1);
        ClientDisplayInfo obj = this [idx];
        if (null != obj)
        {
            strResult += obj.ToString ();
        }
        else
        {
            strResult += "<null>";   
        }
    }

    return strResult;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������ � ��������� ���������.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public void Reset ()
{
    m_Storage.Clear ();
    if (null != m_DefaultClientDisplayInfo) 
    {
        m_DefaultClientDisplayInfo.Reset ();
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ����������� �������.
/// </summary>
/// <returns>������ ����� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public Object Clone ()
{
    return new ClientDisplayInfoList (this);
}

//
// Persistance.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������� ������� � ����� stream.
/// </summary>
/// <param name="stream">�����, � ������� ����������� ����������.
/// </param>
/// <returns>true, ���� �������� ����������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool SaveGuts (Stream stream)
{
    try
    {
        Utils.Write (stream, m_nLatestVersion);
        if (null ==  m_DefaultClientDisplayInfo)
        {
            return false;
        }

        if ( !m_DefaultClientDisplayInfo.SaveGuts (stream))
        {
            return false;
        }
        int nCount = m_Storage.Count;
        Utils.Write (stream, nCount);
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ClientDisplayInfo obj = 
                m_Storage [nIdx] as ClientDisplayInfo;
            if (null == obj) return false;
            if (! obj.SaveGuts (stream)) return false;
        }
    }
    catch (Exception)
    {
        return false;
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������������� �������� ������� �� ������ stream.
/// </summary>
/// <param name="stream">�����, �� ������� ����������� ��������������.
/// </param>
/// <returns>true, ���� �������� ����������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool RestoreGuts (Stream stream)
{
    try
    {
        Reset ();
        uint nVersion = Utils.ReadUInt (stream);
        if (nVersion > m_nLatestVersion) return false;
        m_DefaultClientDisplayInfo = new ClientDisplayInfo ();
        if ( !m_DefaultClientDisplayInfo.RestoreGuts (stream))
        {
            return false;
        }

        int nCount = Utils.ReadInt (stream);
        if (nCount < 0) 
        {
            Reset ();
            return false;
        }
        m_Storage.Capacity = nCount;
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ClientDisplayInfo obj = new ClientDisplayInfo (); 
            if (! obj.RestoreGuts (stream)) return false;
            m_Storage.Add (obj);
        }

        m_Storage.Sort ();
                        
    }
    catch (Exception)
    {
        return false;
    }

    return true;
}

//
// Access to storage.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� �������� � ���������� ��������� �������. ���� � ������ �
/// ������ ��� ���� �������� �� ����� �� �����, �� ��� ���������������� 
/// - ��������� � ��������� �� ���������. ����� ���������� ��������� 
/// ����������� �� �������.
/// </summary>
/// <param name="obj">�������� � ���������� ��������� �������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (ClientDisplayInfo obj)
{
    int nIdx = m_Storage.BinarySearch (obj);
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
/// ������� �������� � ���������� ��������� ������� �� �������.
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

public void Add (ClientDisplayInfoList clients)
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
/// ���������� �������� ClientDisplayInfo, �������� � ������ �������.
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
/// ������ ClientDisplayInfoList � �������� idx .
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ClientDisplayInfo this [int idx]
{
    get {return m_Storage [idx] as ClientDisplayInfo;}
    set {m_Storage [idx] = value;}
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// �������� � ������� ����������� ���������� ������� � ���������������
/// strClientId.
/// </summary>
/// <param name="strClientId">������������� ���������� �������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public ClientDisplayInfo this [string strClientId]
{
    get
    {
        ClientDisplayInfo obj = new ClientDisplayInfo ();
        obj.ClientId = strClientId;

        int nIdx = m_Storage.BinarySearch (obj);
        if (nIdx >= 0) return m_Storage [nIdx] as ClientDisplayInfo;
        else return m_DefaultClientDisplayInfo;
    }
    set
    {
        Add (value as ClientDisplayInfo);
    }
}

//
// Data members.
//

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ������������� �� ������� ������ �������� � ������� �����������
/// ��������� ��������.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

protected ArrayList m_Storage;

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� � ������� ����������� ��������� �������� ��-���������.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ClientDisplayInfo DefaultClientDisplayInfo
{
    get {return m_DefaultClientDisplayInfo;}
    set {m_DefaultClientDisplayInfo = value;}
}

protected ClientDisplayInfo m_DefaultClientDisplayInfo;

/// 
/// <summary>
/// Persistant object latest version.
/// </summary>
/// 

protected const uint m_nLatestVersion = 1;

/// <summary>
/// ��� ������ ��������� ��������.
/// </summary>
private string m_nameList;
public string NameList
{
	get{return m_nameList;}
	set{m_nameList = value;}
}

/// <summary>
/// ���� true �� ��� ����������� �� ������ ������������ ������ ����� ������, ����� ������� ������ �����
/// ������������ � ����������� �������������� ����������.
/// </summary>
private bool m_onlyGroupSettings;
public bool OnlyGroupSettings
{
	get{return m_onlyGroupSettings;}
	set{m_onlyGroupSettings = value;}
}

private DisplayInfoSettings m_displayInfoSettings;
/// <summary>
/// ������ ����������� ������ �� ������
/// </summary>
public DisplayInfoSettings GroupDisplayInfoSettings
{
	get{return m_displayInfoSettings;}
	set{m_displayInfoSettings = value;}
}

///<link>aggregation</link>
///<supplierCardinality>0..*</supplierCardinality>
/*# GPS.Dispatcher.UI.ClientDisplayInfo lnkClientDisplayInfo; */
}
}