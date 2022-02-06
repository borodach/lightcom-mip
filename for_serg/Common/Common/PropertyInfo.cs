/********************************************************************
	created:	2006/02/09
	created:	9:1:2006   16:28
	filename: 	PropertyInfo.cs
	file base:	PropertyInfo
	file ext:	cs
	author:		К.С. Дураков
	
	purpose:	Класс является описателем свойств для которых необходимо
				динамически создать элементы управления.
*********************************************************************/
using System;
using System.Collections;

namespace GPS.Dispatcher.Common
{
	/// <summary>
	/// Summary description for PropertyInfo.
	/// </summary>
	public class PropertyInfo
	{
		/// <summary>
		/// Перечисление типов свойст
		/// </summary>
		public enum PropertyType
		{
			Picture,	//выбор изображения
			Text,		//ввод текста
			List,		//выбор из выпадающего списка
			Integer,	//ввод целого числа
			Float,		//ввод дробного числа
			Color,		//выбор цвета
			File,		//выбор файла
			Check,		//флажок
			Switch,		//переключатель
			Separator,	//разделитель (не используется)
			Group		//группа элементов
		};

		public PropertyInfo()
		{
			//
			// TODO: Add constructor logic here
			//
			ListValues = new ArrayList();
			Enabled = true;
		}

		protected string m_name;
		/// <summary>
		/// имя создаваемого элемента управления
		/// </summary>
		public string Name
		{
			get{return m_name;}
			set{m_name = value;}
		}

		protected string m_text;
		/// <summary>
		/// текст который необходимо задать элементу
		/// </summary>
		public string Text
		{
			get{return m_text;}
			set{m_text = value;}
		}

		protected PropertyType m_type;
		/// <summary>
		/// тип элемента управления (См. Перечисление типов свойств)
		/// </summary>
		public PropertyType Type
		{
			get{return m_type;}
			set{m_type = value;}
		}

		protected object m_max;
		/// <summary>
		/// максимально допустимое значение для свойств.
		/// В некоторых случаях имеет расширенное испльзование:
		/// - для типа Group содержит долю длинны, которую может 
		///		занять группа в базовом элементе;
		///	- для открытия файла/изображения содержит фильтр отбора файлов.
		/// </summary>
		public object Max
		{
			get{return m_max;}
			set{m_max = value;}
		}

		protected object m_min;
		/// <summary>
		/// минимально допустимое значение для элемента.
		/// </summary>
		public object Min
		{
			get{return m_min;}
			set{m_min = value;}
		}

		protected object m_value;
		/// <summary>
		/// значение элемента.
		/// </summary>
		public object Value
		{
			get{return m_value;}
			set{m_value = value;}
		}

		protected ArrayList m_listVales;
		/// <summary>
		/// список значений которые имеют отношение к данному элементу,
		/// например для типа Group и Switch содержит описание подчиненных элементов.
		/// </summary>
		public ArrayList ListValues
		{
			get{return m_listVales;}
			set{m_listVales = value;}
		}

		protected string [] m_dependentProperties;
		/// <summary>
		/// не используется
		/// </summary>
		public string [] DependentProperties
		{
			get{return m_dependentProperties;}
			set{m_dependentProperties = value;}
		}

		protected string m_leftCtrlName;
		/// <summary>
		/// имя элемента справа от которого необходимо разместить текущий элемент,
		/// используется только для типа Group.
		/// </summary>
		public string LeftCtrlName
		{
			get{return m_leftCtrlName;}
			set{m_leftCtrlName = value;}
		}

		protected bool m_enabled;
		/// <summary>
		/// Разрешить / запретить обращение к элементу.
		/// </summary>
		public bool Enabled
		{
			get {return m_enabled;}
			set {m_enabled = value;}
		}

		private static PropertyInfo m_separator;
		public static PropertyInfo Separator
		{
			get
			{
				if (null == m_separator)
				{
					m_separator = new PropertyInfo();
					m_separator.Type = PropertyType.Separator;
				}

				return m_separator;
			}
		}
	}
}
