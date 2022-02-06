////////////////////////////////////////////////////////////////////////////////
//
//  File:           ObjectPositions.cs
//
//  Facility:       �������� GPS �������� � ��������� ��������.
//
//
//  Abstract:       ����� ��� �������� �������� � ��������� ������� �� 
//                  ��������� ������ �������.
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
using System.Collections;
using System.IO;
using GPS.Common;

namespace GPS.Dispatcher.Cache
{
///
/// <summary>
/// ����� ��� �������� �������� � ��������� ������� �� ��������� 
/// ������ �������.
/// </summary>
/// 

public class ObjectPositions : IComparable, IPersistant, ICloneable
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
    Debug.WriteLine ("������������ ������ GPS.Dispatcher.Cache.ObjectPositions.", "Info");

    ObjectPositions obj1 = new ObjectPositions ();
    Debug.WriteLine ("Default object:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    obj1.ClientId = "Client#1";

    ObjectPosition obj = new ObjectPosition ();
    obj.X = 80.123;
    obj.Y = 55.456;
    obj.Speed = 60.5;
    obj.Timestamp = DateTime.Now;
    obj1.Add (obj);
    obj = new ObjectPosition ();
    obj.X = 80.122;
    obj.Y = 55.457;
    obj.Speed = 50.0;
    obj.Timestamp = DateTime.Now - TimeSpan.FromMinutes (2);
    obj1 [obj.Timestamp] = obj;
    obj = new ObjectPosition ();
    obj.X = 80.121;
    obj.Y = 55.458;
    obj.Speed = 45.0;
    obj.Timestamp = DateTime.Now - TimeSpan.FromMinutes (3);
    obj1.Add (obj);

    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    ObjectPositions obj2 = new ObjectPositions (obj1);
    Debug.WriteLine ("Clone (obj2):");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();

    obj2 [0].Speed = 110;
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
        ObjectPositions restoredObject = new ObjectPositions ();
        
        Debug.WriteLine ("restored object:");
        Debug.Indent ();
        Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
        Debug.WriteLine (restoredObject);
        Debug.Unindent ();
    }

    {
        ObjectPositions o = new ObjectPositions ();
        MemoryStream writer = new MemoryStream ();
        Debug.WriteLine ("Persistance of empty object.");

        Debug.WriteLine ("SaveGuts returned: " + o.SaveGuts (writer).ToString ());
        Debug.Unindent ();
        writer.Flush ();

        writer.Seek (0, SeekOrigin.Begin);
        ObjectPositions restoredObject = new ObjectPositions ();
        restoredObject.ClientId = "client";
            
        Debug.WriteLine ("restored object:");
        Debug.Indent ();
        Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
        Debug.WriteLine (restoredObject);
        Debug.Unindent ();
    }
    Debug.WriteLine ("Positions of object.");
    Debug.WriteLine (obj1);
    Debug.Write ("First: ");
    Debug.WriteLine (obj1.FirstEvent);
    Debug.Write ("Last: ");
    Debug.WriteLine (obj1.LastEvent);
    Debug.Write ("At index 1: ");
    Debug.WriteLine (obj1 [1].Timestamp);
    DateTime now = DateTime.Now;
    Debug.Write ("At time " +  now.ToString () + ": ");
    Debug.WriteLine (obj1 [now].Timestamp);
    //Debug.Write ("At time " +  DateTime.MinValue.ToString () + ": ");
    //Debug.WriteLine (obj1 [DateTime.MinValue].Timestamp);
    Debug.Write ("At time " +  obj1 [1].Timestamp.ToString () + ": ");
    Debug.WriteLine (obj1 [obj1 [1].Timestamp]);   
    DateTime time = obj1 [1].Timestamp + TimeSpan.FromSeconds (10);
    Debug.Write ("At time " +  time.ToString () + ": ");
    Debug.WriteLine (obj1 [time]);   
    time = obj1 [1].Timestamp - TimeSpan.FromSeconds (10);
    Debug.Write ("At time " +  time.ToString () + ": ");
    Debug.WriteLine (obj1 [time]);   


    //Debug.Write ("After time " +  now.ToString () + ": ");
    //Debug.WriteLine (obj1.GetPositionAfterTime (now).Timestamp);
    Debug.Write ("After time " +  DateTime.MinValue.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionAfterTime (DateTime.MinValue).Timestamp);
    //Debug.Write ("After time " +  DateTime.MaxValue.ToString () + ": ");
    //Debug.WriteLine (obj1.GetPositionAfterTime (DateTime.MaxValue).Timestamp);
    Debug.Write ("After time " +  obj1 [1].Timestamp.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionAfterTime (obj1 [1].Timestamp));   
    time = obj1 [1].Timestamp + TimeSpan.FromSeconds (10);
    Debug.Write ("After time " +  time.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionAfterTime (time));   
    time = obj1 [1].Timestamp - TimeSpan.FromSeconds (10);
    Debug.Write ("After time " +  time.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionAfterTime (time));

    Debug.Write ("Before time " +  now.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionBeforeTime (now).Timestamp);
    //Debug.Write ("Before time " +  DateTime.MinValue.ToString () + ": ");
    //Debug.WriteLine (obj1.GetPositionBeforeTime (DateTime.MinValue).Timestamp);
    Debug.Write ("Before time " +  DateTime.MaxValue.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionBeforeTime (DateTime.MaxValue).Timestamp);
    Debug.Write ("Before time " +  obj1 [1].Timestamp.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionBeforeTime (obj1 [1].Timestamp));   
    time = obj1 [1].Timestamp + TimeSpan.FromSeconds (10);
    Debug.Write ("Before time " +  time.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionBeforeTime (time));   
    time = obj1 [1].Timestamp - TimeSpan.FromSeconds (10);
    Debug.Write ("Before time " +  time.ToString () + ": ");
    Debug.WriteLine (obj1.GetPositionBeforeTime (time));
    
    obj2 [0].X = 100;
    obj2 [1].Timestamp = DateTime.Now + TimeSpan.FromMinutes (10);
    obj2 [2].Timestamp = DateTime.Now - TimeSpan.FromMinutes (20);

    Debug.WriteLine ("obj1: ");
    Debug.WriteLine (obj1);
    Debug.WriteLine ("obj2: ");
    Debug.WriteLine (obj2);

    obj1.Add (obj2);
    Debug.WriteLine ("obj1.Add (obj2): ");
    Debug.WriteLine (obj1);

    DateTime [] startTimes = 
    {
        obj1.FirstEvent - TimeSpan.FromSeconds (30),
        obj1.FirstEvent - TimeSpan.FromSeconds (30),
        obj1.FirstEvent - TimeSpan.FromSeconds (30),
        obj1.FirstEvent + TimeSpan.FromSeconds (30),
        obj1.FirstEvent + TimeSpan.FromSeconds (30),
        obj1.FirstEvent + TimeSpan.FromSeconds (30),
        obj1.LastEvent + TimeSpan.FromMinutes (1)
    };
    DateTime [] finishTimes = 
    {
        obj1.FirstEvent - TimeSpan.FromSeconds (10),
        obj1.FirstEvent + TimeSpan.FromMinutes (10),
        obj1.LastEvent + TimeSpan.FromMinutes (10),
        obj1.FirstEvent + TimeSpan.FromSeconds (40),
        obj1.LastEvent - TimeSpan.FromSeconds (120),
        obj1.LastEvent + TimeSpan.FromMinutes (10),
        obj1.LastEvent + TimeSpan.FromMinutes (10)
    };

    for (int i = 0; i < startTimes.Length; ++i)
    {
        Debug.WriteLine ("");
        Debug.Write (string.Format ("{0}. ", i + 1));
        Debug.WriteLine (string.Format ("Extract from {0} to {1}.", startTimes [i], finishTimes [i]));
        Debug.WriteLine (obj1.Extract (startTimes [i], finishTimes [i], true));
        //Debug.WriteLine (obj1.Extract (startTimes [i], finishTimes [i], false));
    }

    Debug.WriteLine ("������������ ������ GPS.Dispatcher.Cache.ObjectPositions ���������.", "Info");
}

//
// Initialization/Deinitialization.
//

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Default constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPositions ()
{
    m_ClientId = String.Empty;
    m_Storage = new ArrayList ();
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ����������� �����������.
/// </summary>
/// <param name="other">���������� ������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPositions (ObjectPositions other)
{
    m_ClientId = other.m_ClientId.Clone () as string;
    int size = other.m_Storage.Count;
    m_Storage = new ArrayList (size);
    for (int idx = 0; idx < size; ++idx)
    {
        ObjectPosition position = other.m_Storage [idx] as ObjectPosition;
        if (null != position)
        {
            m_Storage.Add (position.Clone () as ObjectPosition);
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
    string strResult = string.Format ("ClientId: {0}; count: {1}",
        ClientId, Count);
    int size = Count;
    for (int idx = 0; idx < size; ++idx)
    {
        strResult += string.Format ("\n{0}. ", idx + 1);
        ObjectPosition position = this [idx];
        if (null != position)
        {
            strResult += position.ToString ();
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
    return new ObjectPositions (this);
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
    if (! (obj is ObjectPositions))
    {
        throw new ArgumentException ("object is not a ObjectPosition");
    }
    ObjectPositions other = (ObjectPositions) obj;
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

public static bool operator == (ObjectPositions one, ObjectPositions other)
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

public static bool operator != (ObjectPositions one, ObjectPositions other)
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

public static bool operator < (ObjectPositions one, ObjectPositions other)
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

public static bool operator > (ObjectPositions one, ObjectPositions other)
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
        Utils.Write (stream, m_ClientId);
        int nCount = m_Storage.Count;
        Utils.Write (stream, nCount);
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPosition obj = 
                m_Storage [nIdx] as ObjectPosition;
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
        m_ClientId = Utils.ReadString (stream);
        int nCount = Utils.ReadInt (stream);
        if (nCount < 0) 
        {
            Reset ();
            return false;
        }
        m_Storage.Capacity = nCount;
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPosition obj = new ObjectPosition (); 
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

public void Add (ObjectPosition obj)
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
/// ������� nCount ���������, ������� � ������� idx
/// </summary>
/// <param name="nIdx">������ ������� ���������� �������.</param>
/// <param name="nCount">���������� ��������� ���������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void RemoveRange (int nIdx, int nCount)
{
    m_Storage.RemoveRange (nIdx, nCount);
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// �������� ������������ �������� �� ������� �� �������� ������ 
/// �������.
/// </summary>
/// <param name="from">������ ��������� ��� ���������� (������ 
/// ���������� � ��������)</param>
/// <param name="to">����� ��������� ��� ���������� (����� ����������
/// � ��������)</param>
/// <param name="bDeepCopy">true, ���� ����� ���������� ����� 
/// �������� ObjectPosition, ����� ������������ ������ �� �������.
/// </param>
/// <returns>������������ �������� �� ������� �� �������� 
/// ������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPositions Extract (DateTime from, DateTime to, bool bDeepCopy)
{
    ObjectPositions extractedPositios = new ObjectPositions ();
    extractedPositios.ClientId = this.ClientId;
    int count = m_Storage.Count;
    if (0 == count) return extractedPositios;

    ObjectPosition position = new ObjectPosition ();
    position.Timestamp = from;
    int fromIdx = m_Storage.BinarySearch (position);        
    if (-count - 1 == fromIdx) return extractedPositios;
    if (fromIdx < 0) fromIdx = -1 - fromIdx;

    position.Timestamp = to;
    int toIdx = m_Storage.BinarySearch (position);        
    if (-1 == toIdx) return extractedPositios;
    if (toIdx < 0) toIdx = -2 - toIdx;

    extractedPositios.m_Storage.Capacity = 1 + toIdx - fromIdx;
    if (bDeepCopy)
    {
        for (int idx = fromIdx; idx <= toIdx; ++ idx)
        {
            extractedPositios.m_Storage.Add ((m_Storage [idx] as ObjectPosition).Clone ());
        }                
    }
    else
    {
        for (int idx = fromIdx; idx <= toIdx; ++ idx)
        {
            extractedPositios.m_Storage.Add (m_Storage [idx]);
        }                
    }

    return extractedPositios;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� �������� �� �������. ���� � ������ ������� ��� ���� 
/// �������� �� ����� �� �����, �� ��� ���������������� - ��������� �
/// ��������� �� ���������. ����� ���������� ��������� �����������
/// �� �������. </summary>
/// <param name="positions">�������� �� �������.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (ObjectPositions positions)
{
    int count = positions.Count;
    for (int idx = 0; idx < count; ++ idx)
    {
        Add (positions [idx]);
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� ������ ������� �������, ��� ����� ������ ���������.
/// </summary>
/// <param name="time">�����.</param>
/// <returns>������ ������� �������, ��� ����� ������ ���������. 
/// -1, ���� �������� � �������� ��������� ���.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public int GetIdxAfterTime (DateTime time)
{
    ObjectPosition position = new ObjectPosition ();
    position.Timestamp = time;
    int count = m_Storage.Count;
    if (count <= 0) return -1;
    int idx = m_Storage.BinarySearch (position);
    if (idx < 0) idx = -1 - idx;
    for ( ; idx < count; ++idx)
    {
        if ((m_Storage [idx] as ObjectPosition) > position) return idx;
    }

    return -1;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� ������ ������� �������, ��� ����� ������ ���������.
/// </summary>
/// <param name="time">�����.</param>
/// <returns>������ ������� �������, ��� ����� ������ ���������. 
/// -1, ���� �������� � �������� ��������� ���.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public int GetIdxBeforeTime (DateTime time)
{
    ObjectPosition position = new ObjectPosition ();
    position.Timestamp = time;
    int count = m_Storage.Count;
    if (count <= 0) return -1;
    int idx = m_Storage.BinarySearch (position);

    if (idx < 0) idx = -1 - idx;
    if (idx == count) return count - 1;
    for ( ; idx >= 0; --idx)
    {
        if ((m_Storage [idx] as ObjectPosition) < position)
        {
            return idx;
        }
    }

    return -1;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� ������ ������� �������, ��� ����� ������ ���� �����
/// ���������.</summary>
/// <param name="time">�����.</param>
/// <returns>������ ������� �������, ��� ����� ������ ���� �����
/// ���������. -1, ���� �������� � �������� ��������� ���.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public int GetIdxAtTime (DateTime time)
{
    ObjectPosition position = new ObjectPosition ();
    position.Timestamp = time;
    int count = m_Storage.Count;
    if (count <= 0) return -1;
    int idx = m_Storage.BinarySearch (position);

    if (idx < 0) idx = -1 - idx;
    if (idx == count) return count - 1;
    for ( ; idx >= 0; --idx)
    {
        if (!((m_Storage [idx] as ObjectPosition) > position))
        {
            return idx;
        }
    }

    return -1;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� ������ ������, ��� ����� ������ ���������. </summary>
/// <param name="time">�����.</param>
/// <returns>������ ������, ��� ����� ������ ���������. 
/// null, ���� �������� � �������� ��������� ���.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPosition GetPositionAfterTime (DateTime time)
{
    int idx = GetIdxAfterTime (time);
    if (idx < 0) return null;
    else return m_Storage [idx] as ObjectPosition;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� ������ ������, ��� ����� ������ ���������. </summary>
/// <param name="time">�����.</param>
/// <returns>������ ������, ��� ����� ������ ���������. 
/// null, ���� �������� � �������� ��������� ���.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPosition GetPositionBeforeTime (DateTime time)
{
    int idx = GetIdxBeforeTime (time);
    if (idx < 0) return null;
    else return m_Storage [idx] as ObjectPosition;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� ������ ������, ��� ����� ������ ���� ����� ���������.
/// </summary>
/// <param name="time">�����.</param>
/// <returns>������ ������, ��� ����� ������ ���� �����
/// ���������. null, ���� �������� � �������� ��������� ���.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPosition GetPositionAtTime (DateTime time)
{
    int idx = GetIdxAtTime (time);
    if (idx < 0) return null;
    else return m_Storage [idx] as ObjectPosition;
}

//
// Properties
//

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ���������� �������� ObjectPosition, �������� � ������ �������.
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
/// ������ ObjectPosition � �������� idx .
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPosition this [int idx]
{
    get {return m_Storage [idx] as ObjectPosition;}
    set {m_Storage [idx] = value; m_Storage.Sort ();}
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ��������� ������� �� ������ ������� time.
/// </summary>
/// <param name="time">�����.</param>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPosition this [DateTime time]
{
    get {return GetPositionAtTime (time);}
    set {Add (value);}
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ����� ������� ������� (������ ���������, ��������� � ������ 
/// �������). ���� ������ ������� ��� ���������� DateTime.MinValue.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public DateTime FirstEvent
{
    get
    {
        if (m_Storage.Count > 0)
        {
            return (m_Storage [0] as ObjectPosition).Timestamp;
        }
        else
        {
            return DateTime.MinValue;
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ����� ���������� ������� (����� ���������, ��������� � ������ 
/// �������). ���� ������ ������� ��� ���������� DateTime.MaxValue.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public DateTime LastEvent
{
    get
    {
        int count = m_Storage.Count;
        if (count > 0)
        {
            return (m_Storage [count - 1] as ObjectPosition).Timestamp;
        }
        else
        {
            return DateTime.MinValue;
        }
    }
}

//
// Data members.
//

///
/// <summary>
/// ���������� ������������� ���������� �������.
/// </summary>
///

public string ClientId {get {return m_ClientId;} set {m_ClientId = value;}}
protected string m_ClientId;

///
/// <summary>
/// ������������� �� ������� ������ ��������� ���������� �������.
/// </summary>
///

protected ArrayList m_Storage;

///
/// <summary>
/// Persistant object latest version.
/// </summary>
///

protected const uint m_nLatestVersion = 1;

///<link>aggregation</link>
///<clientCardinality>0..*</clientCardinality>

/*# GPS.Console.Cache.ObjectPosition lnkObjectPosition; */
}
}
