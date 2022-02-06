////////////////////////////////////////////////////////////////////////////////
//
//  File:           MapWindow.cs
//
//  Facility:       ���������� ������������ GPS ������.
//
//
//  Abstract:       ����� ������������� ����� ������� ��� ����������� ����� �� 
//                  ������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  20-11-2005
//
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using GPS.Common;


namespace GPS.Dispatcher.Controls
{
/// 
/// <summary>
/// ����� ������������� ����� ������� ��� ����������� ����� �� ������.
/// </summary>
/// 

public class MapWindow
{


////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������ ������� ����������� � �������������� ����������� (��������).
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public double GeoWidth 
{
    get 
    {
		if (null == PositionMapper) return -1;

        MapPoint mapLeftUpper = new MapPoint ();
        this.PositionMapper.GlobalToMap (m_Position, mapLeftUpper);
        mapLeftUpper.x += (int) (m_Width * m_Zoom);
        mapLeftUpper.y += (int) (m_Height * m_Zoom);
        GlobalPoint global = new GlobalPoint ();
        this.PositionMapper.MapToGlobal (mapLeftUpper, global);

        return global.x - m_Position.x;
    }
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������ ������� ����������� � �������������� ����������� (��������).
/// </summary>
/// 
////////////////////////////////////////////////////////////////////////////////

public double GeoHeight 
{
    get 
    {
		if (null == PositionMapper) return -1;

        MapPoint mapLeftUpper = new MapPoint ();
        this.PositionMapper.GlobalToMap (m_Position, mapLeftUpper);
        mapLeftUpper.x += (int) (m_Width * m_Zoom);
        mapLeftUpper.y += (int) (m_Height * m_Zoom);
        GlobalPoint global = new GlobalPoint ();
        this.PositionMapper.MapToGlobal (mapLeftUpper, global);

        return global.y - m_Position.y;
    }
}


////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������� ���� ����������� �� ��������� ������.
/// </summary>
/// <param name="dx">�������� �� ����������� � _��������_.</param>
/// <param name="dy">�������� �� ��������� � _��������_.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void Move (int dx, int dy)
{
	if (null == PositionMapper || null == m_MapDataSource) return;

    MapPoint mapLeftUpper = new MapPoint ();
    this.PositionMapper.GlobalToMap (m_Position, mapLeftUpper);
    mapLeftUpper.x += (int) (dx * m_Zoom);
    mapLeftUpper.y += (int) (dy * m_Zoom);
    GlobalPoint global = new GlobalPoint ();
    this.PositionMapper.MapToGlobal (mapLeftUpper, m_Position);

    m_MapDataSource.FixPoint (m_Position);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������� ����������� ������������ �������� �����. ���������� 
/// �������� � �������� ����������� ������ �������� ���� ������� �����������.
/// </summary>
/// <param name="x">���������� X ������.</param>
/// <param name="y">���������� Y ������.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void CenterTo (int x, int y)
{
	if (null == m_MapDataSource) return;

    int dx = x - m_Width / 2;
    int dy = y - m_Height / 2;
    Move (dx, dy);
    m_MapDataSource.FixPoint (m_Position);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������� ����������� ������������ �������� �������������� �����.
/// </summary>
/// <param name="center">���������� ������.</param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void CenterTo (GlobalPoint center)
{
    m_Position.x = center.x - GeoWidth / 2;
    m_Position.y = center.y - GeoHeight / 2;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ����������� ���������� ������� ����������� � �������������� ����������.
/// </summary>
/// <param name="x">���������� X � ������� ��������� ������� �����������.
/// </param>
/// <param name="y">���������� Y � ������� ��������� ������� �����������.
/// </param>
/// <returns>�������������� ����������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public GlobalPoint GeoPointFromWindowPoint (int x, int y)
{
    MapPoint mapLeftUpper = new MapPoint ();
    this.PositionMapper.GlobalToMap (m_Position, mapLeftUpper);
    mapLeftUpper.x += (int) (x * m_Zoom);
    mapLeftUpper.y += (int) (y * m_Zoom);
    GlobalPoint global = new GlobalPoint ();
    this.PositionMapper.MapToGlobal (mapLeftUpper, global);
    return global;
}

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// ����������� �������������� ���������� � ���������� ������� �����������.
/// </summary>
/// <param name="geoPoint">�������������� ����������.</param>
/// <param name="x">���������� X � ������� ��������� ������� �����������.
/// </param>
/// <param name="y">���������� Y � ������� ��������� ������� �����������.
/// </param>
/// 
////////////////////////////////////////////////////////////////////////////////

public void WindowPointFromGeoPoint (GlobalPoint geoPoint, out int x, out int y)
{
    MapPoint mapLeftUpper = new MapPoint ();
    PositionMapper.GlobalToMap (m_Position, mapLeftUpper);
    MapPoint mapPoint = new MapPoint ();
    PositionMapper.GlobalToMap (geoPoint, mapPoint);
    x = (int) ((mapPoint.x - mapLeftUpper.x) / m_Zoom);
    y = (int) ((mapPoint.y - mapLeftUpper.y) / m_Zoom);            
}        

//////////////////////////////////////////////////////////////////////////////// 
/// 
/// <summary>
/// ������������ �������� ������� ����������.
/// </summary>
/// <param name="g">Graphics, � ������� �������� �������� �����.</param>
/// <param name="dstX">���������� X (� ������� ��������� ������� �����������) 
/// �������� ������ ��������������� ���������</param>
/// <param name="dstY">���������� Y (� ������� ��������� ������� �����������) 
/// �������� ������ ��������������� ���������</param>
/// <param name="srcX">���������� X �������� ������ ���� �������������� �
/// Graphics, � ������� ����� ��������� �������� �����.</param>
/// <param name="srcY">���������� � �������� ������ ���� �������������� �
/// Graphics, � ������� ����� ��������� �������� �����.</param>
/// <param name="nWidth">������ ��������������� ��������� � �������� ������� 
/// �����������.</param>
/// <param name="nHeight">������ ��������������� ��������� � �������� ������� 
/// �����������.</param>
/// 
//////////////////////////////////////////////////////////////////////////////// 

public void DrawMap (Graphics g, 
                     int dstX, 
                     int dstY,                         
                     int srcX, 
                     int srcY,
                     int nWidth, 
                     int nHeight)
{
	if (null == PositionMapper || null == m_MapDataSource) return;

    GlobalPoint geoLeftUpper = GeoPointFromWindowPoint (srcX, srcY);
    GlobalPoint geoRightBottom = 
        GeoPointFromWindowPoint (srcX + nWidth, srcY + nHeight);

    MapPoint mapLeftUpper = new MapPoint ();
    mapLeftUpper.x = dstX;
    mapLeftUpper.y = dstY;
    MapPoint mapRightBottom = new MapPoint ();
    mapRightBottom.x = dstX + nWidth;
    mapRightBottom.y = dstY + nHeight;
    m_MapDataSource.DrawMap (g,
                             geoLeftUpper, geoRightBottom, 
                             mapLeftUpper, mapRightBottom);
}

///
/// <summary>
/// ��������� �������������� ���������.
/// </summary>
/// 

public IPositionMapper PositionMapper {get {return m_MapDataSource.PositionMapper;}}

protected IMapSource m_MapDataSource;

public IMapSource MapDataSource {get {return m_MapDataSource;} set {m_MapDataSource = value;}}

/// 
/// <summary>
/// �������������� ���������� ������ �������� ���� ���� �����������.
/// </summary>
///
 
public GlobalPoint Position 
{
    get {return m_Position;} 
    set 
	{
		m_Position = value; 
		if (null != m_MapDataSource)
		{
			m_MapDataSource.FixPoint (m_Position);
		}
	}
}    
protected GlobalPoint m_Position;      

/// 
/// <summary>
/// ������� ���������� �����. ���� > 1, �� ����� �����������, ���� < 1, �� 
/// �������������. ��������������� �������� �����������.
/// </summary>
/// 

public double Zoom {get {return m_Zoom;} set {m_Zoom = value;}}
protected double m_Zoom = 1.0;      

///
/// <summary>
/// ������ ������� ����������� � ��������.
/// </summary>
///

public int Width {get {return m_Width;} set {m_Width = value;}}
protected int m_Width;

///
/// <summary>
/// ������ ������� ����������� � ��������. ��� ���������������.
/// </summary>
///

public int Height {get {return m_Height;} set {m_Height = value;}}
protected int m_Height;
}
}