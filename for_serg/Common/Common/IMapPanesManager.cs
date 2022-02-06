//////////////////////////////////////////////////////////////////////////
///���������:	IMapPanesManager
///��������:	��������� ��������� ��������� �� ������� ������ �����
///				��� �������� �������.
///�����:		�.�. �������
//////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using GPS.Common;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMapPanesManager
	{
		/// <summary>
		/// ���������� ��������� ����� ����� ��� �������� ���������� ���������
		/// </summary>
		/// <param name="mapDimension">������� ��� ������� ���������� ������� �����������</param>
		/// <param name="panes">�������� ��������, ������ ������ �������� � �������� �������</param>
		/// <returns>true ���� ������, ����� false</returns>
		bool GetMapPanes(Rectangle mapDimension, out MapPane [] panes);
		/// <summary>
		/// ���������� ��������� ����� ����� ��� �������� �������������� ���������
		/// </summary>
		/// <param name="mapDimension">������� ��� ������� ���������� ������� �����������</param>
		/// <param name="mapImage">�������� ��������, ������ ������ �������� � �������� �������</param>
		/// <returns>true ���� ������, ����� false</returns>
		bool GetMapPanes(RectangleF mapDimension, out MapPane [] panes);
		/// <summary>
		/// ���������� ��������� ���������
		/// </summary>
		void Reset();
		/// <summary>
		/// �������� �� �� ��� �������� ����� ���������������
		/// </summary>
		bool IsInit
		{
			get;
		}
		/// <summary>
		/// ������ ��� ������ � ������ �������������� ��������������� ������������.
		/// </summary>
		IPositionMapper PositionMapper
		{
			get;
		}
	}
}
