//////////////////////////////////////////////////////////////////////////////////////////////
///�����: MapPane
///
///��������: ������� ����� ��� �������� MapPane (������� �����). ��� ����������� �������
///			������� ������ � ��������� �����, ��������� ������, ����������� ������������
///			� ����������� ������� �����.
///�����:	�.�. �������
/////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Drawing;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class MapPane
	{
		public MapPane()
		{
			this.m_paneImage = null;
		}

		/// <summary>
		/// ���������� ���������� ������� ���������� �������� �����
		/// </summary>
		/// <returns>��������� Rectangle ����������� ���������� ������� ����� �����</returns>
		public Rectangle GetPhysicalDimension()
		{
			return this.m_physicalDimensionRect;
		}

		/// <summary>
		/// ���������� �������������� ������� ���������� �������� �����
		/// </summary>
		/// <returns>��������� RectangleF ����������� �������������� ������� ����� �����</returns>
		public RectangleF GetGEODimension()
		{
			return this.m_geoDimensionRect;
		}

		/// <summary>
		/// ����������� �����, � ���������������� ���� ���������� ��������� ������������� ����� �����
		/// </summary>
		/// <param name="retImage">�������� �������� ��� �������� �������� � ������ �����</param>
		public abstract bool GetPaneImage(out Image retImage);

		/// <summary>
		/// ������� ���������� �������� ����� �� ����� ������
		/// </summary>
		protected Rectangle m_physicalDimensionRect;

		/// <summary>
		/// �������������� ������� ����������� �������� �����
		/// </summary>
		protected RectangleF m_geoDimensionRect;

		/// <summary>
		/// ��������� ������������� ������������ ������� �����
		/// </summary>
		protected Image m_paneImage;
	}
}
