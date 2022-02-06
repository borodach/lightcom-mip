////////////////////////////////////////////////////////////////////////////////
//
//  File:           MobileClientList.cs
//
//  Facility:       Mobile client representation.
//
//
//  Abstract:       Список описний мобильных клиентов.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
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
/// Список описний мобильных клиентов.
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
/// Сбрасывает объект в начальное состояние.
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
/// Добавляет сведения о мобильном клиенте. Если в данном объекте
///  уже есть сведения об этом клиенте, то они перезаписываются
/// - дубликаты в хранилище не заводятся. После добавления хранилище 
/// сортируется по времени.
/// </summary>
/// <param name="obj">Сведения о мобильном клиенте.</param>
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
/// Удаляет сведения о мобильном клиенте по индексу.
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
/// Количество объектов MobileClientInfo, хранимых в данном объекте.
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
/// Объект MobileClientInfo с индексом idx .
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
/// Сведения о мобильном клиенте с заданным идентификатором.
/// </summary>
/// <param name="time">Время.</param>
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
/// Сортированный по времени вектор описаний мобильных клиентов.
/// </summary>
///

protected ArrayList m_Storage = new ArrayList ();

///<link>aggregation</link>
///<supplierCardinality>0..1</supplierCardinality>
}
}
