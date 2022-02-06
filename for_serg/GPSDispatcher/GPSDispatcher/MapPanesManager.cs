//////////////////////////////////////////////////////////////////////////
///класс:		MapPanesManager
///описание:	содержит в себе список доступных кусочков карты, определ€ет
///				поиск в этом списке кусочков по условию, режим кешировани€ 
///				кусочков.
//////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Collections;
using System.IO;
using GPS.Common;
using System.Globalization;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public class MapPanesManager : IMapPanesManager, ISettings, IDisposable
	{
		public MapPanesManager()
		{
		}

		public void Reset()
		{
		}

		public bool GetMapPanes(Rectangle mapDimension, out MapPane [] panes)
		{
			panes = null;
			return this.GetMapPanesFromSpecificRect(mapDimension, new RectangleF(0, 0, 0, 0), false, out panes);
		}

		public bool GetMapPanes(RectangleF mapDimension, out MapPane [] panes)
		{
			panes = null;
			return this.GetMapPanesFromSpecificRect(new Rectangle(0, 0, 0, 0), mapDimension, true, out panes);
		}

		private bool GetMapPanesFromSpecificRect(Rectangle physicalRect, RectangleF geoRect, bool isGeo, out MapPane [] panes)
		{
			panes = new MapPane[this.m_arrayPanes.Length];
			int countPanes = 0;
			for (int i = 0; i < this.m_arrayPanes.Length; i++)
			{
				if(!isGeo)
				{
					if (physicalRect.IntersectsWith(this.m_arrayPanes[i].GetPhysicalDimension()))
					{
						panes[countPanes] = this.m_arrayPanes[i];
						countPanes++;
					}
				}
				else
				{
					if (geoRect.IntersectsWith(this.m_arrayPanes[i].GetGEODimension()))
					{
						panes[countPanes] = this.m_arrayPanes[i];
						countPanes++;
					}
				}
			}

			MapPane [] buffer = new MapPane[countPanes];
			Array.Copy(panes, buffer, countPanes);
			panes = buffer;

			if (panes.Length > 0)
				return true;
			else
				return false;
		}

		#region ISettings Members
		public bool Save(ISettingsStorage storage)
		{
			// TODO:  Add MapPanesManager.Save implementation
			return false;
		}

		public bool Load(ISettingsStorage storage)
		{
			bool result = true;
			string val;
			try
			{
				result &= storage.Read("mapName", "name", out m_mapName);

				NumberFormatInfo nf = NumberFormatInfo.InvariantInfo;
				SimpleMapper mapper = new SimpleMapper();
				result &= storage.Read("mapGEODimension", "left", out val);
				mapper.MapX = double.Parse(val, nf);
				result &= storage.Read("mapGEODimension", "top", out val);
				mapper.MapY = double.Parse(val, nf);
				result &= storage.Read("mapGEODimension", "gx", out val);
				mapper.dx = double.Parse(val, nf);
				result &= storage.Read("mapGEODimension", "gy", out val);
				mapper.dy = double.Parse(val, nf);
				m_mapper = mapper;

				string mapDirectory;
				result &= storage.Read("mapDirectory", "mapFilesDirectory", out mapDirectory);
				mapDirectory = Utils.GetExeDirectory() + "\\" + mapDirectory;
				int filesCount;
				result &= storage.Read("mapDirectory", "filesCount", out val);
				filesCount = int.Parse(val);
				string fileName, tmpFileName;
				int x, y, w, h;
				m_arrayPanes = new MapPane[filesCount];
				for (int i = 0; i < filesCount; i++) 
				{
					tmpFileName = "mapFile" + (i + 1).ToString();
					result &= storage.Read(tmpFileName, "name", out val);
					fileName = mapDirectory + val;
					result &= storage.Read(tmpFileName, "top", out val);
					y = int.Parse(val);
					result &= storage.Read(tmpFileName, "left", out val);
					x = int.Parse(val);
					result &= storage.Read(tmpFileName, "width", out val);
					w = int.Parse(val);
					result &= storage.Read(tmpFileName, "height", out val);
					h = int.Parse(val);

					MapPoint mpLeftUp = new MapPoint();
					mpLeftUp.x = x;
					mpLeftUp.y = y;
					MapPoint mpRightDown = new MapPoint();
					mpRightDown.x = x + w;
					mpRightDown.y = y + h;
					GlobalPoint gpLeftUp = new GlobalPoint();
					GlobalPoint gpRightDown = new GlobalPoint();

					m_mapper.MapToGlobal(mpLeftUp, gpLeftUp);
					m_mapper.MapToGlobal(mpRightDown, gpRightDown);

					m_arrayPanes[i] = new DiskMapPane(fileName, new Rectangle(x, y, w, h),
						new RectangleF((float)gpLeftUp.x, (float)gpLeftUp.y,
						(float)(gpRightDown.x - gpLeftUp.x), (float)(gpRightDown.y - gpLeftUp.y)));
				}
			}
			catch(Exception)
			{
				result = false;
			}

			return result;
		}
		#endregion

		#region IDisposable Members

		public void Dispose()
		{
		}

		#endregion

		/// <summary>
		/// ќбъект дл€ работы с картой представленной географическими координатами.
		/// </summary>
		protected IPositionMapper m_mapper;
		public IPositionMapper PositionMapper {get{return m_mapper;}}

		/// <summary>
		/// Ќазвание карты.
		/// </summary>
		private string m_mapName;
		public string MapName {get{return m_mapName;}}

		/// <summary>
		/// ћассив всех файлов карты.
		/// </summary>
		private MapPane [] m_arrayPanes;

		/// <summary>
		/// ≈сли контрол готов к работе то true, иначе false.
		/// </summary>
		private bool m_isInit;
		public bool IsInit
		{
			get{return this.m_isInit;}
		}
	}
}
