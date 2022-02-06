///////////////////////////////////////////////////////////////////////////////
//
//  File:           ObjectPosition.cs
//
//  Facility:       �������� GPS �������� � ��������� ��������.
//
//
//  Abstract:       ����� ��� �������� �������� � ���������� ��������� �������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  11-11-2005
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.IO;
using GPS.Common;

namespace GPS.Dispatcher.Cache
{

/// 
/// <summary>
/// ����� ��� �������� �������� � ���������� ��������� �������.
/// </summary>
/// 

public class ObjectPosition: IComparable, IPersistant, ICloneable
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
    Debug.WriteLine ("������������ ������ GPS.Dispatcher.Cache.ObjectPosition.", "Info");

    ObjectPosition obj1 = new ObjectPosition ();
    Debug.WriteLine ("Default object:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    obj1.X = 80.123;
    obj1.Y = 55.456;
    obj1.Speed = 60.5;
    obj1.Timestamp = DateTime.Now;
    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    ObjectPosition obj2 = new ObjectPosition (obj1);
    Debug.WriteLine ("Clone (obj2):");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();

    obj2.Speed = 110;
    Debug.WriteLine ("Original:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    Debug.WriteLine ("Clone:");
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

    obj1.Timestamp += TimeSpan.FromMinutes (10);
    Debug.WriteLine ("Comparision obj1 > obj2:");
    Debug.Indent ();
    Debug.WriteLine ("obj1 == obj2: " + (obj1 == obj2).ToString ());
    Debug.WriteLine ("obj1.Equals (obj2): " + (obj1.Equals (obj2)));
    Debug.WriteLine ("obj1.CompareTo (obj2): " + (obj1.CompareTo (obj2)));
    Debug.WriteLine ("obj1 != obj2: " + (obj1 != obj2).ToString ());
    Debug.WriteLine ("obj1 < obj2: " + (obj1 < obj2).ToString ());
    Debug.WriteLine ("obj1 > obj2: " + (obj1 > obj2).ToString ());
    Debug.Unindent ();

    obj1.Timestamp -= TimeSpan.FromMinutes (20);
    Debug.WriteLine ("Comparision obj1 < obj2:");
    Debug.Indent ();
    Debug.WriteLine ("obj1 == obj2: " + (obj1 == obj2).ToString ());
    Debug.WriteLine ("obj1.Equals (obj2): " + (obj1.Equals (obj2)));
    Debug.WriteLine ("obj1.CompareTo (obj2): " + (obj1.CompareTo (obj2)));
    Debug.WriteLine ("obj1 != obj2: " + (obj1 != obj2).ToString ());
    Debug.WriteLine ("obj1 < obj2: " + (obj1 < obj2).ToString ());
    Debug.WriteLine ("obj1 > obj2: " + (obj1 > obj2).ToString ());
    Debug.Unindent ();

    MemoryStream writer = new MemoryStream ();
    Debug.WriteLine ("Persistance.");
    Debug.WriteLine ("obj1:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.WriteLine ("SaveGuts returned: " + obj1.SaveGuts (writer).ToString ());
    Debug.Unindent ();
    writer.Flush ();

    writer.Seek (0, SeekOrigin.Begin);
    ObjectPosition restoredObject = new ObjectPosition ();
    
    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
    
    Debug.WriteLine ("������������ ������ GPS.Dispatcher.Cache.ObjectPosition ���������.", "Info");
}

//
// Object initialisation methods.
//

/// 
/// <summary>
/// Default constructor.
/// </summary>
/// 

public ObjectPosition ()
{
    Reset ();
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ����������� �����������.
/// </summary>
/// <param name="other">���������� ������.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public ObjectPosition (ObjectPosition other)
{
    m_X = other.m_X;
    m_Y = other.m_Y;
    m_Speed = other.m_Speed;
    m_Timestamp = other.m_Timestamp;
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
    string strResult = string.Format ("timestamp: {0}; x: {1}; y: {2}; speed: {3};",
        Timestamp, X, Y, Speed);
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
    m_X = 0;
    m_Y = 0;
    m_Speed = 0;
    m_Timestamp = DateTime.MinValue;
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
    return new ObjectPosition (this);
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
    if (! (obj is ObjectPosition))
    {
        throw new ArgumentException("object is not a ObjectPosition");
    }
    ObjectPosition other = (ObjectPosition) obj;

    return m_Timestamp.CompareTo (other.m_Timestamp);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ��������.
/// </summary>
/// <param name="obj">������ ��� ���������.</param>
/// <returns>true, ���� this == obj</returns>
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

public static bool operator == (ObjectPosition one, ObjectPosition other)
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

public static bool operator != (ObjectPosition one, ObjectPosition other)
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

public static bool operator < (ObjectPosition one, ObjectPosition other)
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

public static bool operator > (ObjectPosition one, ObjectPosition other)
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
    return m_Timestamp.GetHashCode ();
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
        Utils.Write (stream, (int) (m_X * 10000000));
        Utils.Write (stream, (int) (m_Y * 10000000));
        Utils.Write (stream, (int) (m_Speed * 1000));
        Utils.Write (stream, m_Timestamp);
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
        uint nVersion = Utils.ReadUInt (stream);
        if (nVersion > m_nLatestVersion) return false;
        m_X = ((double) Utils.ReadInt (stream)) / 10000000;
        m_Y = ((double) Utils.ReadInt (stream)) / 10000000;
        m_Speed = ((double) Utils.ReadInt (stream)) / 1000;
        m_Timestamp = Utils.ReadDateTime (stream);
    }
    catch (Exception)
    {
        return false;
    }

    return true;
}

//
// Data members
//

/// 
/// <summary>
/// �������������� ���������� X (�������) � ��������.
/// </summary>
///

public double X {get {return m_X;}  set {m_X = value;}}
protected double m_X;

/// 
/// <summary>
/// �������������� ���������� Y (������) � ��������.
/// </summary>
///

public double Y {get {return m_Y;}  set {m_Y = value;}}
protected double m_Y;

/// 
/// <summary>
/// �������� � ��/�..
/// </summary>
///

public double Speed {get {return m_Speed;}  set {m_Speed = value;}}
protected double m_Speed;

/// 
/// <summary>
/// �����, � ������� ���� ����� ��������.
/// </summary>
///

public DateTime Timestamp {get {return m_Timestamp;}  set {m_Timestamp = value;}}
protected DateTime m_Timestamp;

/// 
/// <summary>
/// Persistant object latest version.
/// </summary>
/// 

protected const uint m_nLatestVersion = 1;
}

}
