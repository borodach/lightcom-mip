/********************************************************************
	created:	2006/01/09
	created:	9:1:2006   16:28
	filename: 	MapDataSource.cs
	file base:	MapDataSource
	file ext:	cs
	author:		К.С. Дураков
	
	purpose:	Класс производит отрисовку карты из нужных фрагментов
				в заданную область.
*********************************************************************/

using System;
using System.Drawing;
using GPS.Common;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for MapDataSource.
	/// </summary>
	public class MapSource : IMapSource
	{
		public MapSource()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IMapSource Members

		/// <summary>
		/// рисуем запрошенный участок карты.
		/// </summary>
		/// <param name="g">графический контекст в который рисуем</param>
		/// <param name="geoLeftUpper">верхний левый угол нужного куска карты в ГЕО координатах</param>
		/// <param name="geoRightBottom">нижний правый угол нужного куска карты в ГЕО координатах</param>
		/// <param name="imgLeftUpper">верхний левый угол области в которую будем рисовать</param>
		/// <param name="imgRightBottom">нижний правый угол области в которую будем рисовать</param>
		public void DrawMap(System.Drawing.Graphics g, GPS.Common.GlobalPoint geoLeftUpper, GPS.Common.GlobalPoint geoRightBottom, GPS.Common.MapPoint imgLeftUpper, GPS.Common.MapPoint imgRightBottom)
		{
			if (null == MapPanesManager) return;
			RectangleF geoMapRect = new RectangleF((float)geoLeftUpper.x, (float)geoLeftUpper.y, 
				(float)(geoRightBottom.x - geoLeftUpper.x), (float)(geoRightBottom.y - geoLeftUpper.y));
			MapPoint mpLeftUp = new MapPoint();
			MapPoint mpRightDown = new MapPoint();
			PositionMapper.GlobalToMap(geoLeftUpper, mpLeftUp);
			PositionMapper.GlobalToMap(geoRightBottom, mpRightDown);
			
			Rectangle mapRect = new Rectangle(mpLeftUp.x, mpLeftUp.y, 
				mpRightDown.x - mpLeftUp.x, mpRightDown.y - mpLeftUp.y);

			MapPane [] panes;

			if (!MapPanesManager.GetMapPanes(geoMapRect, out panes))
			{
				return;
			}

			float resolutionX = (float)(imgLeftUpper.x - imgRightBottom.x) / (mpLeftUp.x - mpRightDown.x);
			float resolutionY = (float)(imgLeftUpper.y - imgRightBottom.y) / (mpLeftUp.y - mpRightDown.y);
			Image paneImg;
			for (int i = 0; i < panes.Length; i++)
			{
				//получаем размеры кусочка
				Rectangle paneDimension = panes[i].GetPhysicalDimension();
				//получаем пересечение кусочка с областью видимой карты
				Rectangle intersectRect = Rectangle.Intersect(mapRect, paneDimension);
				//берем отображаемый прямоугольник из кусочка
				RectangleF srcRect = new RectangleF(((paneDimension.Left > mapRect.Left) ? 0 : mapRect.Left - paneDimension.Left),
					((paneDimension.Top > mapRect.Top) ? 0 : mapRect.Top - paneDimension.Top),
					intersectRect.Width, intersectRect.Height);
				//определяем прямоугольник в который будет происходить отрисовка на экране
				//определяем прямоугольник в который будет происходить отрисовка на экране
				RectangleF destRect = new RectangleF((intersectRect.Left - mapRect.Left) * resolutionX,
					(intersectRect.Top - mapRect.Top) * resolutionY,
					intersectRect.Width * resolutionX + 1,
					intersectRect.Height * resolutionY + 1);
				//получаем картинку кусочка
				if(panes[i].GetPaneImage(out paneImg))
				{
					if(null != paneImg)
					{
						g.DrawImage(paneImg, destRect, srcRect, GraphicsUnit.Pixel);
						//paneImg.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// используемый объект для преобразования координат.
		/// </summary>
		public GPS.Common.IPositionMapper PositionMapper
		{
			get{return (null == m_panesMngr) ? null : m_panesMngr.PositionMapper;}
		}

		public void FixPoint(GPS.Common.GlobalPoint point)
		{
			// TODO:  Add MapDataSource.FixPoint implementation
		}

		#endregion

		//Properties
		//менеджер для загрузки кусков карты.
		private IMapPanesManager m_panesMngr;
		public IMapPanesManager MapPanesManager
		{
			get{return m_panesMngr;}
			set{m_panesMngr = value;}
		}
	}
}
