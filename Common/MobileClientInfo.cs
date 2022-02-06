////////////////////////////////////////////////////////////////////////////////
//
//  File:           MobileClientInfo.cs
//
//  Facility:       Mobile client representation.
//
//
//  Abstract:       ����� ��� �������� �������� � ��������� �������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  11-11-2005
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.IO;
using System.Diagnostics;
using LightCom.Common;

namespace LightCom.MiP.Common
{
/// 
/// <summary>
/// ����� ��� �������� �������� � ��������� �������.
/// </summary>
/// 

public class MobileClientInfo: IComparable, IPersistent
{
////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ����� ��������� ������������ ������
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////
/*
public static void Test ()
{
    Debug.WriteLine ("������������ ������ GPS.Common.MobileClientInfo.", "Info");

    MobileClientInfo obj1 = new MobileClientInfo ();
    Debug.WriteLine ("Default object:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    obj1.ClientId = "Client#1";
    obj1.FriendlyName = "Mobile client";
    obj1.Company = "Company name";
    obj1.LastEvent = DateTime.Now;
    obj1.SoftwareName = "mip";
    obj1.SoftwareVersion = "1.0";
    obj1.Comments = "Some comments";

    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    MobileClientInfo obj2 = new MobileClientInfo ();
    obj2.ClientId = "Client#1";
    obj2.FriendlyName = "Mobile client1";
    obj2.Company = "Company name1";
    obj2.LastEvent = DateTime.Now + TimeSpan.FromMinutes (1);
    obj2.SoftwareName = "mip1";
    obj2.SoftwareVersion = "1.01";
    obj2.Comments = "Some comments1";
    Debug.WriteLine ("Initialized object (obj2):");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();
    
    Debug.WriteLine ("Comparision obj1 == obj2:");
    Debug.Indent ();
    Debug.WriteLine ("obj1 == obj2: " + (obj1 == obj2).ToString ());
    Debug.WriteLine ("obj1.Equals (obj2): " + (obj1.Equals (obj2)));
    Debug.WriteLine ("obj1.CompareTo (obj2): " + (obj1.CompareTo (obj2)));
    Debug.WriteLine ("obj1 != obj2: " + (obj1 != obj2).ToString ());
    Debug.WriteLine ("obj1 < obj2: " + (obj1 < obj2).ToString ());
    Debug.WriteLine ("obj1 > obj2: " + (obj1 > obj2).ToString ());
    Debug.Unindent ();

    obj1.ClientId = "ZZZ";
    Debug.WriteLine ("Comparision obj1 > obj2:");
    Debug.Indent ();
    Debug.WriteLine ("obj1 == obj2: " + (obj1 == obj2).ToString ());
    Debug.WriteLine ("obj1.Equals (obj2): " + (obj1.Equals (obj2)));
    Debug.WriteLine ("obj1.CompareTo (obj2): " + (obj1.CompareTo (obj2)));
    Debug.WriteLine ("obj1 != obj2: " + (obj1 != obj2).ToString ());
    Debug.WriteLine ("obj1 < obj2: " + (obj1 < obj2).ToString ());
    Debug.WriteLine ("obj1 > obj2: " + (obj1 > obj2).ToString ());
    Debug.Unindent ();

    obj1.ClientId = "AAA";
    Debug.WriteLine ("Comparision obj1 < obj2:");
    Debug.Indent ();
    Debug.WriteLine ("obj1 == obj2: " + (obj1 == obj2).ToString ());
    Debug.WriteLine ("obj1.Equals (obj2): " + (obj1.Equals (obj2)));
    Debug.WriteLine ("obj1.CompareTo (obj2): " + (obj1.CompareTo (obj2)));
    Debug.WriteLine ("obj1 != obj2: " + (obj1 != obj2).ToString ());
    Debug.WriteLine ("obj1 < obj2: " + (obj1 < obj2).ToString ());
    Debug.WriteLine ("obj1 > obj2: " + (obj1 > obj2).ToString ());
    Debug.Unindent ();

    Debug.WriteLine ("������������ ������ GPS.Common.MobileClientInfo ���������.", "Info");
}
 * */

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public MobileClientInfo ()
{
    Reset ();
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
    m_ClientId = string.Empty;
    m_FriendlyName = string.Empty;
    m_Company = string.Empty;
    m_LastEvent = DateTime.MinValue;
    m_SoftwareName = string.Empty;
    m_SoftwareVersion = string.Empty;
    m_Comments = string.Empty;
}

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
        Utils.Write (stream, m_ClientId);
        Utils.Write (stream, m_FriendlyName);
        Utils.Write (stream, m_Company);
        Utils.Write (stream, m_LastEvent);    
        Utils.Write (stream, m_SoftwareName);
        Utils.Write (stream, m_SoftwareVersion);
        Utils.Write (stream, m_Comments);
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
    {   Reset ();
        int nVersion = Utils.ReadInt (stream);
        if (nVersion > m_nLatestVersion) return false;

        m_ClientId = Utils.ReadString (stream);
        m_FriendlyName = Utils.ReadString (stream);
        m_Company = Utils.ReadString (stream);
        m_LastEvent = Utils.ReadDateTime (stream);    
        m_SoftwareName = Utils.ReadString (stream);
        m_SoftwareVersion = Utils.ReadString (stream);
        m_Comments = Utils.ReadString (stream);
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
/// ���������� ��������� �������� �������.
/// </summary>
/// <returns>��������� �������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public override string ToString ()
{
    string strResult = string.Format ("ClientId: {0}; FriendlyName: {1}; Company: {2}; LastEvent: {3}; SoftwareName: {4}; SoftwareVersion: {5}; Comments: {6}", 
        ClientId, FriendlyName, Company, LastEvent, SoftwareName, SoftwareVersion, Comments);

    return strResult;
}

//
// Comparision methods.
//

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns> -1, ����   this < obj
///            0, ���� this == obj
///            1, ����   this > obj
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public int CompareTo (object obj)
{
    if (! (obj is MobileClientInfo))
    {
        throw new ArgumentException ("object is not a MobileClientInfo");
    }
    MobileClientInfo other = (MobileClientInfo) obj;
    return m_ClientId.CompareTo (other.m_ClientId);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns>true, ���� one == other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public override bool Equals (object obj)
{
    return 0 == CompareTo (obj);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns>true, ���� one == other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator == (MobileClientInfo one, MobileClientInfo other)
{
    if (null == (one as object) && null == (other as object)) return true;
    if (null == (one as object) || null == (other as object)) return false;

    return 0 == one.CompareTo (other);    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns>true, ���� one != other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator != (MobileClientInfo one, MobileClientInfo other)
{
    if (null == (one as object) && null == (other as object)) return false;
    if (null == (one as object) || null == (other as object)) return true;

    return 0 != one.CompareTo (other);    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns>true, ���� one < other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator < (MobileClientInfo one, MobileClientInfo other)
{
    if (null == (one as object) && null == (other as object)) return false;
    if (null == (one as object)) return true;
    if (null == (other as object)) return false;

    return one.CompareTo (other) < 0;    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns>true, ���� one > other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator > (MobileClientInfo one, MobileClientInfo other)
{
    if (null == (one as object)) return false;
    if (null == (other as object)) return true;

    return one.CompareTo (other) > 0;    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������� ����.
/// </summary>
/// <returns>��� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public override int GetHashCode ()
{
    return m_ClientId.GetHashCode ();
}
                                                                      /// 
/// <summary>
/// ���������� ������������� ��������� �������.
/// </summary>
///

public string ClientId {get {return m_ClientId;} set {m_ClientId = value;}}
protected string m_ClientId;

/// 
/// <summary>
/// ������������� �������� ���������� �������.
/// </summary>
///

public string FriendlyName {get {return m_FriendlyName;} set {m_FriendlyName = value;}}
protected string m_FriendlyName;

/// 
/// <summary>
/// �������� �������� - ��������� ���������� �������.
/// </summary>
///

public string Company {get {return m_Company;} set {m_Company = value;}}
protected string m_Company;

/// 
/// <summary>
/// ����� ��������� ���������� ��������� �� ���������� �������.
/// </summary>
///

public DateTime LastEvent {get {return m_LastEvent;} set {m_LastEvent = value;}}
protected DateTime m_LastEvent;

/// 
/// <summary>
/// �������� ����������� ��.
/// </summary>
///

public string SoftwareName {get {return m_SoftwareName;} set {m_SoftwareName = value;}}
protected string m_SoftwareName;

/// 
/// <summary>
/// ������ ����������� ��.
/// </summary>
///

public string SoftwareVersion {get {return m_SoftwareVersion;} set {m_SoftwareVersion = value;}}
protected string m_SoftwareVersion;

/// 
/// <summary>
/// ������ � ������������.
/// </summary>
///

public string Comments {get {return m_Comments;} set {m_Comments = value;}}
protected string m_Comments;


/// 
/// <summary>
/// Persistant object latest version.
/// </summary>
/// 

protected const int m_nLatestVersion = 1;
}

}
