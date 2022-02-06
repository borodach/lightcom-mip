////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           XMLSetingsStorage.cs
//
//  Facility:       Сохранение настроек.
//
//
//  Abstract:       Модуль содержит определение класса XMLSetingsStorage, 
//                  позволяющего хранить настройки в XML файле.
//
//  Environment:    VC# 7.1
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  13/09/2005
//
////////////////////////////////////////////////////////////////////////////////


using System;
using System.Xml;

namespace GPS.Common
{

/// 
/// <summary>
/// Summary description for the class.
/// </summary>
///
 
public class XMLSetingsStorage : ISettingsStorage 
{

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// Constructor.
/// </summary>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public XMLSetingsStorage ()
{
    System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly ();
    if (null != asm)
    {
        this.m_strFileName = "settings.xml";
        System.Reflection.Module [] modules = asm.GetModules ();
        if (null != modules && modules.Length > 0)
        {
            
            string strPath = modules [0].FullyQualifiedName;
            int nIdx = strPath.LastIndexOf ('\\');
            if (nIdx >= 0) 
            {
                this.m_strFileName = strPath.Substring (0, nIdx);
                this.m_strFileName += "\\settings.xml";
            }
        }
    }
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Выполняет инициализацию объекта - все данные сбрасываются в 
/// начальное состояние.
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Reset ()
{
    m_Document = new XmlDocument ();
//    this.m_strFileName = Utils.GetExeDirectory () + "\\settings.xml" ;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Метод сбрасывает содержимое буфера в реальное хранилище.
/// </summary>
/// <returns>
/// Возвращает true, если сброс кэша прошел успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Flush ()
{
    XmlTextWriter writer = new XmlTextWriter (this.FileName, null);
    writer.Formatting = Formatting.Indented;
    try 
    {
        this.m_Document.Save (writer);
    }
    catch (Exception )
    {
        writer.Close ();
        return false;
    }
    writer.Close ();

    return true;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Выполняет инициализацию, необходимую для загрузки данных.
/// </summary>
/// <returns>
/// Возвращает true, если инициализация прошла успешно.
/// </returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool PreLoad ()
{
    this.Reset ();

    XmlTextReader reader = new XmlTextReader (this.FileName);

    try
    {
        this.m_Document.Load (reader);
    }
    catch (Exception)
    {
        reader.Close ();
        return false;
    }
    reader.Close ();

    return true;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Сохраняет параметр.
/// </summary>
/// <param name="strType">IN Название типа/категории параметра</param>
/// <param name="strName">IN Название параметра</param>
/// <param name="strValue">IN Значение параметра</param>
/// <returns>Возвращает true, если запись прошла успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Write (string strType, 
    string strName, 
    string strValue)
{
    
    XmlNode root = this.m_Document [m_strDocumentName];
    if (null == root)
    {
        root = this.m_Document.CreateElement (m_strDocumentName);
        this.m_Document.AppendChild (root);
    }

    XmlElement elem = root [strType];
    if (null == elem)
    {
        elem = this.m_Document.CreateElement (strType);
        root.AppendChild (elem);
    }
    elem.SetAttribute (strName, strValue);

    return true;

}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// 
/// </summary>
/// <param name="strType">IN Название типа/категории параметра</param>
/// <param name="strName">IN Название параметра</param>
/// <param name="strValue">OUT Значение параметра</param>
/// <returns>Возвращает true, если чтение прошло успешно.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Read (string strType, 
    string strName, 
    out string strValue)
{
    strValue = "";

    XmlNode root = this.m_Document [m_strDocumentName];
    if (null == root)
    {
        return false;
    }

    XmlElement elem = root [strType];
    if (null == elem)
    {
        return false;
    }
    strValue = elem.GetAttribute (strName);

    return true;
}

///
/// <summary>
/// XML документ.
/// </summary>
/// 

protected XmlDocument m_Document = new XmlDocument ();

///
/// <summary>
/// Имя файла с настройками.
/// </summary>
/// 

public string FileName {get {return this.m_strFileName;} set {this.m_strFileName = value;}}
protected string m_strFileName;

/// 
/// <summary>
/// Имя корневого узла XML документа.
/// </summary>
/// 

protected const string m_strDocumentName = "settings";

}
}