/********************************************************************
	created:	2006/01/09
	created:	9:1:2006   16:28
	filename: 	MapDataSource.cs
	file base:	MapDataSource
	file ext:	cs
	author:		�.�. �������
	
	purpose:	����� ���������� ��������� ����� �� ������ ����������
				� �������� �������.
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
		/// ������ ����������� ������� �����.
		/// </summary>
		/// <param name="g">����������� �������� � ������� ������</param>
		/// <param name="geoLeftUpper">������� ����� ���� ������� ����� ����� � ��� �����������</param>
		/// <param name="geoRightBottom">������ ������ ���� ������� ����� ����� � ��� �����������</param>
		/// <param name="imgLeftUpper">������� ����� ���� ������� � ������� ����� ��������</param>
		/// <param name="imgRightBottom">������ ������ ���� ������� � ������� ����� ��������</param>
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
				//�������� ������� �������
				Rectangle paneDimension = panes[i].GetPhysicalDimension();
				//�������� ����������� ������� � �������� ������� �����
				Rectangle intersectRect = Rectangle.Intersect(mapRect, paneDimension);
				//����� ������������ ������������� �� �������
				RectangleF srcRect = new RectangleF(((paneDimension.Left > mapRect.Left) ? 0 : mapRect.Left - paneDimension.Left),
					((paneDimension.Top > mapRect.Top) ? 0 : mapRect.Top - paneDimension.Top),
					intersectRect.Width, intersectRect.Height);
				//���������� ������������� � ������� ����� ����������� ��������� �� ������
				//���������� ������������� � ������� ����� ����������� ��������� �� ������
				RectangleF destRect = new RectangleF((intersectRect.Left - mapRect.Left) * resolutionX,
					(intersectRect.Top - mapRect.Top) * resolutionY,
					intersectRect.Width * resolutionX + 1,
					intersectRect.Height * resolutionY + 1);
				//�������� �������� �������
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
		/// ������������ ������ ��� �������������� ���������.
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
		//�������� ��� �������� ������ �����.
		private IMapPanesManager m_panesMngr;
		public IMapPanesManager MapPanesManager
		{
			get{return m_panesMngr;}
			set{m_panesMngr = value;}
		}
	}
}
