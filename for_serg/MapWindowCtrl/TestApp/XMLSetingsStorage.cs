////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           XMLSetingsStorage.cs
//
//  Facility:       ���������� ��������.
//
//
//  Abstract:       ������ �������� ����������� ������ XMLSetingsStorage, 
//                  ������������ ������� ��������� � XML �����.
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
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
/// ��������� ������������� ������� - ��� ������ ������������ � 
/// ��������� ���������.
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
/// ����� ���������� ���������� ������ � �������� ���������.
/// </summary>
/// <returns>
/// ���������� true, ���� ����� ���� ������ �������.
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
/// ��������� �������������, ����������� ��� �������� ������.
/// </summary>
/// <returns>
/// ���������� true, ���� ������������� ������ �������.
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
/// ��������� ��������.
/// </summary>
/// <param name="strType">IN �������� ����/��������� ���������</param>
/// <param name="strName">IN �������� ���������</param>
/// <param name="strValue">IN �������� ���������</param>
/// <returns>���������� true, ���� ������ ������ �������.</returns>
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
/// <param name="strType">IN �������� ����/��������� ���������</param>
/// <param name="strName">IN �������� ���������</param>
/// <param name="strValue">OUT �������� ���������</param>
/// <returns>���������� true, ���� ������ ������ �������.</returns>
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
/// XML ��������.
/// </summary>
/// 

protected XmlDocument m_Document = new XmlDocument ();

///
/// <summary>
/// ��� ����� � �����������.
/// </summary>
/// 

public string FileName {get {return this.m_strFileName;} set {this.m_strFileName = value;}}
protected string m_strFileName;

/// 
/// <summary>
/// ��� ��������� ���� XML ���������.
/// </summary>
/// 

protected const string m_strDocumentName = "settings";

}
}