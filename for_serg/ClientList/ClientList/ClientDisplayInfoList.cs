////////////////////////////////////////////////////////////////////////////////
//
//  File:           ClientDisplayInfoList.cs
//
//  Facility:       Отображение мобильных клиентов.
//
//
//  Abstract:       Контейнер описаний способа отображения мобильных клиентов.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
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
/// Контейнер описаний способа отображения мобильных клиентов.
/// </summary>
/// 

public class ClientDisplayInfoList
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
    Debug.WriteLine ("Тестирование класса GPS.Dispatcher.UI.ClientDisplayInfoList.", "Info");

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

    Debug.WriteLine ("Тестирование класса GPS.Dispatcher.UI.ClientDisplayInfoList завешенно.", "Info");
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
/// Возвращает текстовое описание объекта.
/// </summary>
/// <returns>Текстовое описание объекта.</returns>
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
/// Сбрасывает объект в начальное состояние.
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
/// Клонировние объекта.
/// </summary>
/// <returns>Точная копия объекта.</returns>
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
/// Добавляет сведения о мгновенном положении объекта. Если в данном о
/// бъекте уже есть сведения за такое же время, то они перезаписываются 
/// - дубликаты в хранилище не заводятся. После добавления хранилище 
/// сортируется по времени.
/// </summary>
/// <param name="obj">Сведения о мгновенном положении объекта.</param>
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
/// Удаляет сведения о мгновенном положении объекта по индексу.
/// </summary>
/// <param name="idx">Индекс удаляемого объекта.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Remove (int idx)
{
    m_Storage.RemoveAt (idx);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Добавляет сведения о мобильных клиентах.
/// </summary>
/// <param name="clients">Cведения о мобильных клиентах.</param>
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
/// Количество объектов ClientDisplayInfo, хранимых в данном объекте.
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
/// Объект ClientDisplayInfoList с индексом idx .
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
/// Сведения о способе отображения мобильного клиента с идентификатором
/// strClientId.
/// </summary>
/// <param name="strClientId">Идентификатор мобильного клиента.</param>
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
/// Сортированный по времени вектор сведений о способе отображения
/// мобильных клиентов.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

protected ArrayList m_Storage;

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Ссведения о способе отображения мобильных клиентов по-умолчанию.
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
/// имя группы мобильных клиентов.
/// </summary>
private string m_nameList;
public string NameList
{
	get{return m_nameList;}
	set{m_nameList = value;}
}

/// <summary>
/// Если true то при отображении на экране используется только стиль группы, иначе объекты группы могут
/// отображаться с применением индивидуальных параметров.
/// </summary>
private bool m_onlyGroupSettings;
public bool OnlyGroupSettings
{
	get{return m_onlyGroupSettings;}
	set{m_onlyGroupSettings = value;}
}

private DisplayInfoSettings m_displayInfoSettings;
/// <summary>
/// Способ отображения группы на экране
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