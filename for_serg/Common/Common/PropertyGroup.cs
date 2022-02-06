/********************************************************************
	created:	2006/02/09
	created:	9:1:2006   16:28
	filename: 	PropertyGroup.cs
	file base:	PropertyGroup
	file ext:	cs
	author:		К.С. Дураков
	
	purpose:	Класс является описателем групп свойств которые отобажаются
				в левой части диалогового окна в элементе TreeView.
*********************************************************************/

using System;
using System.Collections;

namespace GPS.Dispatcher.Common
{
	/// <summary>
	/// Summary description for PropertyGroup.
	/// </summary>
	public class PropertyGroup
	{
		public PropertyGroup()
		{
			m_subGroups = new ArrayList();
			m_properties = new ArrayList();
		}

		protected string m_groupName;
		/// <summary>
		/// имя группы свойст.
		/// </summary>
		public string Name
		{
			get{return m_groupName;}
			set{m_groupName = value;}
		}
	
		protected string m_groupText;
		/// <summary>
		/// текст который необходимо отобразить в TreeView
		/// </summary>
		public string Text
		{
			get{return m_groupText;}
			set{m_groupText = value;}
		}

		protected ArrayList m_subGroups;
		/// <summary>
		/// список подгрупп свойств.
		/// </summary>
		public ArrayList SubGroups
		{
			get{return m_subGroups;}
			set{m_subGroups = value;}
		}

		protected ArrayList m_properties;
		public ArrayList Properties
		{
			get {return m_properties;}
			set {m_properties = value;}
		}

		protected ObjectInfo m_baseObject;
		public ObjectInfo BaseObject
		{
			get {return m_baseObject;}
			set {m_baseObject = value;}
		}

		public int Count
		{
			get{return m_subGroups.Count;}
		}
	}
}
