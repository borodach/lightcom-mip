////////////////////////////////////////////////////////////////////////////////
//
//  File:           CientDisplayInfo.cs
//
//  Facility:       Отображение мобильных клиентов.
//
//
//  Abstract:       Класс содержит сведения о способе отображения мобильных
//                  клиентов.
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
using GPS.Common;
using GPS.Dispatcher.Common;

namespace GPS.Dispatcher.UI
{

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Класс содержит сведения о способе отображения мобильных клиентов.
/// </summary>
/// 

public class ClientDisplayInfo: IComparable, IPersistant, ICloneable, IProperties
{

	public delegate void ClientDisplayInfoChangedHandler(ClientDisplayInfo client);
	public event ClientDisplayInfoChangedHandler ClientDisplayInfoChanged;
////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Метод выполняет тестирование класса
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public static void Test ()
{
    Debug.WriteLine ("Тестирование класса GPS.Dispatcher.UI.ClientDisplayInfo", "Info");

    ClientDisplayInfo obj1 = new ClientDisplayInfo ();
    Debug.WriteLine ("Default object:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();
    obj1.ClientId = "Client#1";
    
    Debug.WriteLine ("Initialized object (obj1):");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    ClientDisplayInfo obj2 = new ClientDisplayInfo (obj1);
    Debug.WriteLine ("Clone (obj2):");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();

    obj2.ClientId = "Client#2";
    Debug.WriteLine ("Original:");
    Debug.Indent ();
    Debug.WriteLine (obj1);
    Debug.Unindent ();

    Debug.WriteLine ("Clone:");
    Debug.Indent ();
    Debug.WriteLine (obj2);
    Debug.Unindent ();

    obj2.ClientId = "Client#1";
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
    ClientDisplayInfo restoredObject = new ClientDisplayInfo ();
    
    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
}

{
    ClientDisplayInfo o = new ClientDisplayInfo ();
    MemoryStream writer = new MemoryStream ();
    Debug.WriteLine ("Persistance of empty object.");

    Debug.WriteLine ("SaveGuts returned: " + o.SaveGuts (writer).ToString ());
    Debug.Unindent ();
    writer.Flush ();

    writer.Seek (0, SeekOrigin.Begin);
    ClientDisplayInfo restoredObject = new ClientDisplayInfo ();
    restoredObject.ClientId = "client";
        
    Debug.WriteLine ("restored object:");
    Debug.Indent ();
    Debug.WriteLine ("RestoreGuts returned: " + restoredObject.RestoreGuts (writer).ToString ());
    Debug.WriteLine (restoredObject);
    Debug.Unindent ();
}

    Debug.WriteLine ("Тестирование класса GPS.Dispatcher.UI.ClientDisplayInfo завешенно.", "Info");
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ClientDisplayInfo ()
{
        Reset ();
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Copy constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public ClientDisplayInfo (ClientDisplayInfo other)
{
    m_ClientId = other.m_ClientId;
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
    string strResult = string.Format ("ClientId: {0}", ClientId);
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
    m_ClientId = string.Empty;
	ClientDisplaySettings = new DisplayInfoSettings();
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
    return new ClientDisplayInfo (this);
}


//
// Comparision methods.
//

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Сравнение объектов.
/// </summary>
/// <param name="obj">Объект для сравнения.</param>
/// <returns> -1, если   this < obj
///            0, если   this == obj
///            1, если   this > obj
/// </returns>
///
////////////////////////////////////////////////////////////////////////////////

public int CompareTo (object obj)
{
    if (! (obj is ClientDisplayInfo))
    {
        throw new ArgumentException ("object is not a ClientDisplayInfo");
    }
    ClientDisplayInfo other = (ClientDisplayInfo) obj;
    return m_ClientId.CompareTo (other.m_ClientId);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сравнение объектов.
/// </summary>
/// <param name="obj">Объект для сравнения.</param>
/// <returns>true, если one == other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public override bool Equals (object obj)
{
    return 0 == CompareTo (obj);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сравнение объектов.
/// </summary>
/// <param name="obj">Объект для сравнения.</param>
/// <returns>true, если one == other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator == (ClientDisplayInfo one, ClientDisplayInfo other)
{
    if (null == (one as object) && null == (other as object)) return true;
    if (null == (one as object) || null == (other as object)) return false;

    return 0 == one.CompareTo (other);    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сравнение объектов.
/// </summary>
/// <param name="obj">Объект для сравнения.</param>
/// <returns>true, если one != other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator != (ClientDisplayInfo one, ClientDisplayInfo other)
{
    if (null == (one as object) && null == (other as object)) return false;
    if (null == (one as object) || null == (other as object)) return true;

    return 0 != one.CompareTo (other);    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сравнение объектов.
/// </summary>
/// <param name="obj">Объект для сравнения.</param>
/// <returns>true, если one < other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator < (ClientDisplayInfo one, ClientDisplayInfo other)
{
    if (null == (one as object) && null == (other as object)) return false;
    if (null == (one as object)) return true;
    if (null == (other as object)) return false;

    return one.CompareTo (other) < 0;    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Сравнение объектов.
/// </summary>
/// <param name="obj">Объект для сравнения.</param>
/// <returns>true, если one > other</returns>
///
////////////////////////////////////////////////////////////////////////////////

public static bool operator > (ClientDisplayInfo one, ClientDisplayInfo other)
{
    if (null == (one as object)) return false;
    if (null == (other as object)) return true;

    return one.CompareTo (other) > 0;    
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Подсчет хэша.
/// </summary>
/// <returns>Хэш объекта.</returns>
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
        Utils.Write (stream, m_ClientId);
		ClientDisplaySettings.SaveGuts(stream);
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
        uint nVersion = Utils.ReadUInt (stream);
        if (nVersion > m_nLatestVersion) return false;
        m_ClientId = Utils.ReadString (stream);
		ClientDisplaySettings.RestoreGuts(stream);
    }
    catch (Exception)
    {
        return false;
    }

    return true;
}

	#region Реализация интерфейса IProperties

	public ObjectInfo GetObjectInfo()
	{
		if (null == m_mobileClientInfo)
			return null;

		ObjectInfo obj = new ObjectInfo();
		obj.Name = m_ClientId;
		obj.Text = "Клиент: " + m_ClientId;
		PropertyGroup pg = new PropertyGroup();
		pg.BaseObject = obj;
		pg.Name = m_ClientId;
		obj.BaseProperties = pg;
		GetPropertyList(pg);
		pg = new PropertyGroup();
		pg.Name = "DisplaySettings";
		pg.Text = "Настройки отображения";
		pg.BaseObject = obj;
		GetPropertyList(pg);
		obj.PropertyGroups.Add(pg);
		
		return obj;
	}

	public void GetPropertyList(PropertyGroup group)
	{
		if (null == m_mobileClientInfo)
			return;

		if (group.Name == m_ClientId)
		{
			group.Properties.Add(GetProperty("FriendlyName"));
			group.Properties.Add(PropertyInfo.Separator);
			group.Properties.Add(GetProperty("Company"));
			group.Properties.Add(PropertyInfo.Separator);
			group.Properties.Add(GetProperty("Comments"));
			group.Properties.Add(PropertyInfo.Separator);
			group.Properties.Add(GetProperty("SoftName"));
			group.Properties.Add(PropertyInfo.Separator);
			group.Properties.Add(GetProperty("SoftVer"));
		}
		else
			ClientDisplaySettings.GetPropertyList(group);
	}

	public void SetProppertyList(PropertyGroup group)
	{
		if (null == m_mobileClientInfo)
			return; 

		if (group.Name == m_ClientId)
			for (int i = 0; i < group.Properties.Count; i++)
			{
				SetProperty((PropertyInfo)group.Properties[i]);
			}
		else
			ClientDisplaySettings.SetProppertyList(group);

		OnClientDisplayInfoChanged();
	}

	public PropertyInfo GetProperty(string propertyName)
	{
		if (null == m_mobileClientInfo)
			return null;

		PropertyInfo prop = new PropertyInfo();
		prop.Name = propertyName;
		prop.Type = PropertyInfo.PropertyType.Text;

		if ("FriendlyName" == propertyName)
		{
			prop.Text = "Дружественное имя объекта: ";
			prop.Value = m_mobileClientInfo.FriendlyName;
		}
		if ("Company" == propertyName)
		{
			prop.Text = "Компания владелец: ";
			prop.Value = m_mobileClientInfo.Company;
			prop.Enabled = false;
		}
		if ("Comments" == propertyName)
		{
			prop.Text = "Заметки: ";
			prop.Value = m_mobileClientInfo.Comments;
		}
		if ("SoftName" == propertyName)
		{
			prop.Text = "Программное обеспечение: ";
			prop.Value = m_mobileClientInfo.SoftwareName;
			prop.Enabled = false;
		}
		if ("SoftVer" == propertyName)
		{
			prop.Text = "Версия программного обеспечения: ";
			prop.Value = m_mobileClientInfo.SoftwareVersion;
			prop.Enabled = false;
		}

		return prop;
	}

	public void SetProperty(PropertyInfo val)
	{
		if (null == m_mobileClientInfo)
			return;

		if ("FriendlyName" == val.Name)
			m_mobileClientInfo.FriendlyName = (string)val.Value;
		if ("Comments" == val.Name)
			m_mobileClientInfo.Comments = (string)val.Value;
	}

	#endregion


	public void ShowObjectProperty(MobileClientInfo mci)
	{
		m_mobileClientInfo = mci;
		PropertyDialog dlg = new PropertyDialog();
		dlg.AddProperty((IProperties)this);
		dlg.ShowDialog();
	}

	public void SetMobileClientInfo(MobileClientInfo mci)
	{
		m_mobileClientInfo = mci;
	}

	private void OnClientDisplayInfoChanged()
	{
		if (null != ClientDisplayInfoChanged)
			ClientDisplayInfoChanged(this);
	}
//
// Data members.
//

/// 
/// <summary>
/// Уникальный идентификатор мобльного клиента.
/// </summary>
///

public string ClientId {get {return m_ClientId;} set {m_ClientId = value;}}
protected string m_ClientId;

protected DisplayInfoSettings m_clientDisplaySettings;
/// <summary>
/// Способ отображения клиента на экране
/// </summary>
public DisplayInfoSettings ClientDisplaySettings
{
	get{return m_clientDisplaySettings;}
	set{m_clientDisplaySettings = value;}
}
/// 
/// <summary>
/// Persistant object latest version.
/// </summary>
/// 

	protected const uint m_nLatestVersion = 1;

	private MobileClientInfo m_mobileClientInfo;
}

}