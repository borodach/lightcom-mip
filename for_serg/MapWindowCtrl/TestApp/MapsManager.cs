/********************************************************************
	created:	2006/01/12
	created:	12:1:2006   14:33
	filename: 	MapManager.cs
	file path:	TestMapSource
	file base:	MapManager
	file ext:	cs
	author:		�.�. �������
	
	purpose:	����� ��������� ��������� ����, ����������� ���� ������������ ����������.
*********************************************************************/

using System;
using System.Windows.Forms;
using System.Collections;
using GPS.Common;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for MapManager.
	/// </summary>
	public class MapsManager
	{
		/// <summary>
		/// ��������� �������� �������� ������� ������� ������������ ����.
		/// </summary>
		public struct Map
		{
			public string m_mapName;
			public string m_mapRusName;
			public string MapRussianName
			{
				get{return m_mapRusName + " (" + m_mapName + ")";}
			}
			public DateTime m_lastUpdate;
			public string m_mapDirectory;
			public string m_mapFileName;
			public bool m_isUse;
			public int m_mapCode;
			public int MapCode
			{
				get{return m_mapCode;}
			}
			public string m_mapTag;
		};

		public MapsManager()
		{
			m_isInit = false;
		}

		/// <summary>
		/// ���������� ��������� � ������, ���������� ������������ ����� ���������� 
		/// ����������� �� ���������.
		/// </summary>
		/// <returns>true ���� �������, ����� false</returns>
		public bool InitManager()
		{
			XMLSetingsStorage storage = new XMLSetingsStorage();
			storage.FileName = Utils.GetExeDirectory() + "\\" + m_path;
			m_storage = storage;
	
			if (!storage.PreLoad())
			{
				m_isInit = false;
				return false;
			}
			else
			{
				m_isInit = true;
				return true;
			}
		}

		/// <summary>
		/// ��������� �� ���������� ��������� ������ � ������ ������������ ����������.
		/// </summary>
		/// <returns>���� �������� ������ ������� true, ����� false</returns>
		public bool LoadMaps()
		{
			bool result = true;

			try
			{
				if (!m_isInit)
				{
					result = false;
				}
				else
				{
					string val;
					result &= m_storage.Read("webMapsListSource", "source", out m_webMapsListSource);
					result &= m_storage.Read("mapCount", "count", out val);
					int mapCount = int.Parse(val);
					m_mapsList = new Map[mapCount];
					Map map;
					string tmpMapName;
					for (int i = 0; i < mapCount; i++)
					{
						tmpMapName = "map" + (i + 1).ToString();
						map = new Map();
						map.m_mapTag = tmpMapName;
						result &= m_storage.Read(tmpMapName, "mapCode", out val);
						map.m_mapCode = int.Parse(val);
						result &= m_storage.Read(tmpMapName, "name", out map.m_mapName);
						result &= m_storage.Read(tmpMapName, "rusName", out map.m_mapRusName);
						result &= m_storage.Read(tmpMapName, "mapDirectory", out map.m_mapDirectory);
						result &= m_storage.Read(tmpMapName, "mapFile", out map.m_mapFileName);
						result &= m_storage.Read(tmpMapName, "lastUpdate", out val);
						map.m_lastUpdate = DateTime.Parse(val);
						result &= m_storage.Read(tmpMapName, "state", out val);
						map.m_isUse = (1 == int.Parse(val)) ? true : false;

						m_mapsList[i] = map;
					}
				}

			}
			catch(Exception)
			{
				result = false;
			}

			return result;
		}

		/// <summary>
		/// ��������� � �������������� ������ ����������� ��� ������ ������ � ������.
		/// </summary>
		/// <param name="index">������ ����� � �������, ������� ���������� ���������.</param>
		/// <returns>���������� �������� ��� ������ �����, ������� � �������������.</returns>
		public IMapPanesManager LoadMap(int index)
		{
			bool result = true;
			Map map = m_mapsList[index];
			XMLSetingsStorage storage = new XMLSetingsStorage();
			storage.FileName = Utils.GetExeDirectory() + "\\" + map.m_mapDirectory + map.m_mapFileName;
			result &= storage.PreLoad();
			MapPanesManager manager = new MapPanesManager();
			result &= manager.Load(storage);

			if (result)
			{
				return manager;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// ��������� ���������� ��������� �����.
		/// </summary>
		/// <param name="map">������������ �����</param>
		/// <returns>���� �������� ������ ������� �� true.</returns>
		public bool SaveMapSettings(Map map)
		{
			bool result = true;
			result &= m_storage.Write(map.m_mapTag, "state", (map.m_isUse) ? "1" : "0");
			m_storage.Flush();
			return result;
		}

		/// <summary>
		/// ���� � ����� � ��������� ����.
		/// </summary>
		protected const string m_path = "maps\\maps.xml";

		/// <summary>
		/// ������ ����������� ����.
		/// </summary>
		protected Map [] m_mapsList;
		public Map [] Maps
		{
			get{return m_mapsList;}
		}

		protected string m_webMapsListSource;

		protected ISettingsStorage m_storage;

		protected bool m_isInit;
		public bool IsInit
		{
			get{return m_isInit;}
		}
	}
}
