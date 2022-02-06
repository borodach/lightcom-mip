///////////////////////////////////////////////////////////////////////////////
//
//  File:           GPSDataCache.cs
//
//  Facility:       Хранение GPS сведений о мобильных клиентах.
//
//
//  Abstract:       Класс для хранения сведений о состояних объектов за 
//                  некоторый период времени.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  11-11-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Diagnostics;
using System.Collections;
using System.IO;
using GPS.Common;

namespace GPS.Dispatcher.Cache
{
/// 
/// <summary>
/// Класс для хранения сведений о состояних объектов за некоторый период 
/// времени.
/// </summary>
/// 

public class GPSDataCache: ICloneable, IPersistant
{

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Метод выполняет тестирование класса
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public static void Test ()
{
    Debug.WriteLine ("Тестирование класса GPS.Dispatcher.Cache.GPSDataCache.", "Info");

    GPSDataCache obj1 = new GPSDataCache ();
    Debug.WriteLine ("Default object:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    ObjectPositions pos = new ObjectPositions ();
    pos.ClientId = "Client#1";
    ObjectPosition obj = new ObjectPosition ();
    obj.X = 80.123;
    obj.Y = 55.456;
    obj.Speed = 60.5;
    obj.Timestamp = DateTime.Now;
    pos.Add (obj);
    obj = new ObjectPosition ();
    obj.X = 80.122;
    obj.Y = 55.457;
    obj.Speed = 50.0;
    obj.Timestamp = DateTime.Now - TimeSpan.FromMinutes (2);
    pos.Add (obj);
    obj = new ObjectPosition ();
    obj.X = 80.121;
    obj.Y = 55.458;
    obj.Speed = 45.0;
    obj.Timestamp = DateTime.Now - TimeSpan.FromMinutes (3);
    pos.Add (obj);
    obj1.Add (pos);

    System.Threading.Thread.Sleep (1000);
    pos = new ObjectPositions ();
    pos.ClientId = "Client#2";
    obj = new ObjectPosition ();
    obj.X = 80.123;
    obj.Y = 55.456;
    obj.Speed = 60.5;
    obj.Timestamp = DateTime.Now;
    pos.Add (obj);
    obj = new ObjectPosition ();
    obj.X = 80.122;
    obj.Y = 55.457;
    obj.Speed = 50.0;
    obj.Timestamp = DateTime.Now - TimeSpan.FromMinutes (2);
    pos.Add (obj);
    obj = new ObjectPosition ();
    obj.X = 80.121;
    obj.Y = 55.458;
    obj.Speed = 45.0;
    obj.Timestamp = DateTime.Now - TimeSpan.FromMinutes (3);
    pos.Add (obj);
    obj1 [pos.ClientId] = pos;


    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    GPSDataCache obj2 = new GPSDataCache (obj1);
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
    GPSDataCache restoredObject = new GPSDataCache ();
    
    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
}

{
    GPSDataCache o = new GPSDataCache ();
    MemoryStream writer = new MemoryStream ();
    Debug.WriteLine ("Persistance of empty object.");

    Debug.WriteLine ("SaveGuts returned: " + o.SaveGuts (writer).ToString ());
    Debug.Unindent ();
    writer.Flush ();

    writer.Seek (0, SeekOrigin.Begin);
    GPSDataCache restoredObject = new GPSDataCache ();
        
    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
}

    Debug.WriteLine ("Positions of objects.");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    Debug.WriteLine (obj1);
    Debug.Write ("First: ");
    Debug.WriteLine (obj1.FirstEvent);
    Debug.Write ("Second: ");
    Debug.WriteLine (obj1.GetNextEvent (obj1.FirstEvent));
    Debug.Write ("Last: ");
    Debug.WriteLine (obj1.LastEvent);
    Debug.Write ("Pre-last: ");
    Debug.WriteLine (obj1.GetPrevEvent (obj1.LastEvent));

    Debug.Write ("At time " + obj1.FirstEvent + ": ");
    Debug.WriteLine (obj1.GetEvent (obj1.FirstEvent));
    Debug.Write ("At time " + (obj1.FirstEvent + TimeSpan.FromSeconds (45)) + ": ");
    Debug.WriteLine (obj1.GetEvent (obj1.FirstEvent + TimeSpan.FromSeconds (45)));

    ObjectPosition [] positions = new ObjectPosition [obj1.Count];
    DateTime time = DateTime.Now - TimeSpan.FromMilliseconds (3000);
    obj1.GetPositonsAtTime (positions, time);
    Debug.WriteLine ("Positions at time " + time + ": ");
    for (int i = 0; i < obj1.Count; ++i)
    {
        Debug.WriteLine (positions [i]);
    }

    Debug.WriteLine ("obj1: ");
    Debug.WriteLine (obj1);
    Debug.WriteLine ("");

    DateTime startTime = obj1.FirstEvent + TimeSpan.FromMinutes (1.5);
    DateTime finishTime = obj1.LastEvent;

    Debug.WriteLine (string.Format ("Extract from {0} to {1}", startTime, finishTime));
    Debug.WriteLine (obj1.Extract (startTime, finishTime, false));
    Debug.WriteLine ("Client#2:");
    Debug.WriteLine (obj1 ["Client#2"]);

    obj2 = obj1.Clone () as GPSDataCache;
    ObjectPositions clone = obj2 ["Client#2"].Clone () as ObjectPositions;
    clone.ClientId = "New Client";
    obj2.Add (clone);
    ObjectPosition position = new ObjectPosition ();
    position.X = 10;
    position.Y = 10;
    obj2 ["Client#2"].Add (position);
    position = obj2 ["Client#2"] [1];
    position.Speed = 5;
    obj2 ["Client#2"].Add (position);

    Debug.WriteLine ("");
    Debug.WriteLine ("obj1:");
    Debug.WriteLine (obj1);
    Debug.WriteLine ("obj2:");
    Debug.WriteLine (obj2);
    obj1.Add (obj2);
    Debug.WriteLine ("obj1.Add (obj2): ");
    Debug.WriteLine (obj1);

    Debug.WriteLine ("Тестирование класса GPS.Dispatcher.Cache.GPSDataCache завешенно.", "Info");
}

//
// Initialization/Deinitialization.
//

/// 
/// <summary>
/// Default constructor.
/// </summary>
///

public GPSDataCache ()
{
    m_Storage = new ArrayList ();
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Конструктор копирования.
/// </summary>
/// <param name="other">Копируемый объект.</param>
///
////////////////////////////////////////////////////////////////////////////////

public GPSDataCache (GPSDataCache other)
{
    int size = other.m_Storage.Count;
    m_Storage = new ArrayList (size);
    for (int idx = 0; idx < size; ++idx)
    {
        ObjectPositions positions = other.m_Storage [idx] as ObjectPositions;
        if (null != positions)
        {
            m_Storage.Add (positions.Clone() as ObjectPositions);
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
/// Возвращает текстовое описание объекта.
/// </summary>
/// <returns>Текстовое описание объекта.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public override string ToString ()
{
    string strResult = string.Format ("count: {0}", Count);
    int size = Count;
    for (int idx = 0; idx < size; ++idx)
    {
        strResult += string.Format ("\n{0}. ", idx + 1);
        ObjectPositions obj = this [idx];
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
/// Сбрасывает объект в начальное состояние.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public void Reset()
{
    m_Storage.Clear();
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Клонировние объекта.
/// </summary>
/// <returns>Точная копия объекта.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public Object Clone()
{
    return new GPSDataCache (this);
}

//
// Persistance.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сохраняет состояние объекта в поток stream.
/// </summary>
/// <param name="stream">Поток, в который выполняется сохранение.
/// </param>
/// <returns>true, если операция выполнилась успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool SaveGuts (Stream stream)
{
    try
    {
        Utils.Write (stream, m_nLatestVersion);
        int nCount = m_Storage.Count;
        Utils.Write (stream, nCount);
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPositions obj =
                m_Storage [nIdx] as ObjectPositions;
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
/// Восстанавливает сосояние объекта из потока stream.
/// </summary>
/// <param name="stream">Поток, из которго выполняется восстановление.
/// </param>
/// <returns>true, если операция выполнилась успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool RestoreGuts (Stream stream)
{
    try
    {
        Reset ();
        uint nVersion = Utils.ReadUInt (stream);
        if (nVersion > m_nLatestVersion) return false;
        int nCount = Utils.ReadInt (stream);
        if (nCount < 0) 
        {
            Reset ();
            return false;
        }
        m_Storage.Capacity = nCount;
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPositions obj = new ObjectPositions ();
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

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает индекс клиента с заданным идентификатором.
/// </summary>
/// <param name="strClientId">Уникальный идентификатор мобильного 
/// клиента</param>
/// <returns>Индекс клиента с заданным идентификатором или -1.
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public int GetClientIndex (string strClientId)
{
    ObjectPositions obj = new ObjectPositions ();
    obj.ClientId = strClientId;

    return m_Storage.BinarySearch (obj);
}

//
// Access to storage.
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Добавляет сведения о положениях объекта. Если в данном объекте уже 
/// есть сведения о таком клиенте, то они перезаписываются 
/// - дубликаты в хранилище не заводятся. После добавления хранилище 
/// сортируется по идетификатору клиентов
/// </summary>
/// <param name="obj">Сведения о положениях объекта.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (ObjectPositions obj)
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
/// Удаляет сведения о положениях мобильного клиента по индексу.
/// </summary>
/// <param name="idx">Индекс удаляемого объекта.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Remove(int idx)
{
    m_Storage.RemoveAt(idx);
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Удаляет nCount элементов, начиная с индекса idx
/// </summary>
/// <param name="nIdx">Индекс первого удаляемого объекта.</param>
/// <param name="nCount">Количество удаляемых элементов.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void RemoveRanпe (int nIdx, int nCount)
{
    m_Storage.RemoveRange (nIdx, nCount);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Выделяет подмножество сведений об мобильных клиентах за заданный 
/// период времени.
/// </summary>
/// <param name="from">Начало интервала для извлечения (начало 
/// включается в интервал)</param>
/// <param name="to">Конец интервала для извлечения (конец включается
/// в интервал)</param>
/// <param name="bDeepCopy">true, если нужно возвращать копии 
/// объектов ObjectPositions, иначе возвращаются ссылки на объекты.
/// </param>
/// <returns>Подмножество сведений о мобильных клиентах за заданный 
/// период.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public GPSDataCache Extract (DateTime from, DateTime to, bool bDeepCopy)
{
    GPSDataCache extractedCache = new GPSDataCache ();
    int count = m_Storage.Count;
    extractedCache.m_Storage.Capacity = count;
    for (int idx = 0; idx < count; ++ idx)
    {
        extractedCache.m_Storage.Add (this [idx].Extract (from, to, bDeepCopy));
    }

    return extractedCache;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Добавляет сведения о мобильных клиентах. Если в данном объекте уже 
/// есть сведения о таком клиенте, то они перезаписываются 
/// - дубликаты в хранилище не заводятся. После добавления хранилище 
/// сортируется по идетификатору клиентов.
/// </summary>
/// <param name="positions">Сведения об объекте.</param>
///
////////////////////////////////////////////////////////////////////////////////

public void Add (GPSDataCache cache)
{
    int count = cache.Count;
    for (int idx = 0; idx < count; ++ idx)
    {
        ObjectPositions newPositions = cache [idx];
        ObjectPositions positions = this [newPositions.ClientId];
        if (null == positions) 
        {
            Add (newPositions);    
        }
        else
        {
            positions.Add (newPositions);    
        }
    }    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает время следующего события после времени time.
/// </summary>
/// <param name="time">Время.</param>
/// <returns>Время следующего события после времени time. Если
/// такого события нет, возвращает DateTime.MaxValue.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public DateTime GetNextEvent (DateTime time)
{
    DateTime result = DateTime.MaxValue;
    int nCount = m_Storage.Count;
    if (nCount <= 0)
    {
        return result;
    }

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
        if (null == obj) continue;
        ObjectPosition position = obj.GetPositionAfterTime (time);
        if (null == position) continue;
        DateTime nextEvent = position.Timestamp;                    
        if (nextEvent < result) result = nextEvent;
    }

    return result;                
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает время самого позднего события, не превышающего time.
/// </summary>
/// <param name="time">Время.</param>
/// <returns>Время следующего события, не превышающего time. Если
/// такого события нет, возвращает DateTime.MinValue.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public DateTime GetEvent (DateTime time)
{
    DateTime result = DateTime.MinValue;
    int nCount = m_Storage.Count;
    if (nCount <= 0)
    {
        return result;
    }

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
        if (null == obj) continue;
        ObjectPosition position = obj.GetPositionAtTime (time);
        if (null == position) continue;
        DateTime prevEvent = position.Timestamp;                    
        if (prevEvent > result) result = prevEvent;
    }

    return result;                
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Возвращает время предыдущего события перед времени time.
/// </summary>
/// <param name="time">Время.</param>
/// <returns>Время предыдущего события перед времени time. Если
/// такого события нет, возвращает DateTime.MinValue.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public DateTime GetPrevEvent (DateTime time)
{
    DateTime result = DateTime.MinValue;
    int nCount = m_Storage.Count;
    if (nCount <= 0)
    {
        return result;
    }

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
        if (null == obj) continue;
        ObjectPosition position = obj.GetPositionBeforeTime (time);
        if (null == position) continue;
        DateTime prevEvent = position.Timestamp;                    
        if (prevEvent > result) result = prevEvent;
    }

    return result;                
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Получение сведений о положении всех хранимых в  данном объекте 
/// мобильных клиентах на заданный момент времени.
/// </summary>
/// <param name="positions">Вектор, в который записываются положения
/// всех мобильных клиентов. Размер вектора должен быть не меньше, 
/// чем Count - данный метод не занимается выделением памяти для
/// сохранения сведений о положении клиентов.</param>
/// <returns>true, если опреация прошла успешно.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetPositonsAtTime (ObjectPosition [] positions, DateTime time)
{
    int nCount = m_Storage.Count;
    if (positions.Length < nCount) return false;

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
        if (null == obj) 
        {
            positions [nIdx] = null;
            continue;
        }

        ObjectPosition position = obj [time];
        positions [nIdx] = position;                
    }

    return true;
}

//
// Properties
//

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Количество хранимых объектов ObjectPoditions.
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
/// Доступ с сведениям о положениях мобильного клиента по индексу 
/// клиента.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public ObjectPositions this [int idx]
{
    get {return m_Storage[idx] as ObjectPositions;}
    set {m_Storage [idx] = value; m_Storage.Sort ();}
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Доступ с сведениям о положениях мобильного клиента по идентификатору
/// клиента.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ObjectPositions this [string strClientId]
{
    get
    {
        ObjectPositions obj = new ObjectPositions ();
        obj.ClientId = strClientId;

        int nIdx = m_Storage.BinarySearch (obj);
        if (nIdx >= 0) return m_Storage [nIdx] as ObjectPositions;
        else return null;
    }
    set
    {
        Add (value as ObjectPositions);
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Получение самых последних сведений о положении всех хранимых в 
/// данном объекте мобильных клиентах.
/// </summary>
/// <param name="positions">Вектор, в который записываются положения
/// всех мобильных клиентов. Размер вектора должен быть не меньше, 
/// чем Count - данный метод не занимается выделением памяти для
/// сохранения сведений о положении клиентов.</param>
/// <returns>true, если опреация прошла успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool GetLatestPositons (ObjectPosition [] positions)
{
    int nCount = m_Storage.Count;
    if (positions.Length < nCount) return false;

    for (int nIdx = 0; nIdx < nCount; ++ nIdx)
    {
        ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
        if (null == obj) 
        {
            positions [nIdx] = null;
            continue;
        }

        int nPositionsCount = obj.Count;
        if (nPositionsCount <= 0)
        {
            positions [nIdx] = null;
            continue;
        }

        positions [nIdx] = obj [nPositionsCount - 1];                
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Время самого первого события.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public DateTime FirstEvent
{
    get
    {
        DateTime result = DateTime.MaxValue;
        int nCount = m_Storage.Count;
        if (nCount <= 0)
        {
            return result;
        }

        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
            if (null == obj) continue;
            DateTime firstEvent = obj.FirstEvent;
            if (firstEvent < result) result = firstEvent;
        }

        return result;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Время самого последнего события.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public DateTime LastEvent
{
    get
    {
        DateTime result = DateTime.MinValue;
        int nCount = m_Storage.Count;
        if (nCount <= 0)
        {
            return result;
        }

        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPositions obj = m_Storage [nIdx] as ObjectPositions;
            if (null == obj) continue;
            DateTime lastEvent = obj.LastEvent;
            if (lastEvent > result) result = lastEvent;
        }

        return result;
    }
}

//
// Data members.
//

/// 
/// <summary>
/// Хранилище сведений о положениях мобильных клиентов.
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

/*# ObjectPositions lnkObjectPositions; */
}
}
