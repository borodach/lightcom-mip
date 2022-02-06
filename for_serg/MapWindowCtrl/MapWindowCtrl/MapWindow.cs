////////////////////////////////////////////////////////////////////////////////
//
//  File:           MapWindow.cs
//
//  Facility:       Управление отображением GPS данных.
//
//
//  Abstract:       Класс предоставляет набор функций для отображения карты на 
//                  экране.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
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
/// Класс предоставляет набор функций для отображения карты на экране.
/// </summary>
/// 

public class MapWindow
{


////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Высота области отображения в географических координатах (градусах).
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
/// Высота области отображения в географических координатах (градусах).
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
/// Смещает окно отображения на указанный вектор.
/// </summary>
/// <param name="dx">Смещение по горизонтали в _пикселях_.</param>
/// <param name="dy">Смещение по аертикали в _пикселях_.</param>
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
/// Центрирует область отображения относительно заданной точки. Координаты 
/// задаются в пикселях относително левого верхнего угла области отображения.
/// </summary>
/// <param name="x">Координата X центра.</param>
/// <param name="y">Координата Y центра.</param>
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
/// Центрирует область отображения относительно заданной географической точки.
/// </summary>
/// <param name="center">Координаты центра.</param>
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
/// Преобразует координаты области отображения в географические координаты.
/// </summary>
/// <param name="x">Координата X в системе координат области отображения.
/// </param>
/// <param name="y">Координата Y в системе координат области отображения.
/// </param>
/// <returns>Географические координаты.</returns>
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
/// Преобразует географические координаты в координаты области отображения.
/// </summary>
/// <param name="geoPoint">Географические координаты.</param>
/// <param name="x">Координата X в системе координат области отображения.
/// </param>
/// <param name="y">Координата Y в системе координат области отображения.
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
/// Отрисовывает фрагмент области тображения.
/// </summary>
/// <param name="g">Graphics, в который рисуется фрагмент карты.</param>
/// <param name="dstX">Координата X (в системе координат области отображения) 
/// верхнего левого отрисовываемого фрагмента</param>
/// <param name="dstY">Координата Y (в системе координат области отображения) 
/// верхнего левого отрисовываемого фрагмента</param>
/// <param name="srcX">Координата X верхнего левого угла прямоугольника в
/// Graphics, в который будет отрисован фрагмент карты.</param>
/// <param name="srcY">Координата Н верхнего левого угла прямоугольника в
/// Graphics, в который будет отрисован фрагмент карты.</param>
/// <param name="nWidth">Ширина отрисовываемого фрагмента в пикселях области 
/// отображения.</param>
/// <param name="nHeight">Высота отрисовываемого фрагмента в пикселях области 
/// отображения.</param>
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
/// Интерфейс преобразования координат.
/// </summary>
/// 

public IPositionMapper PositionMapper {get {return m_MapDataSource.PositionMapper;}}

protected IMapSource m_MapDataSource;

public IMapSource MapDataSource {get {return m_MapDataSource;} set {m_MapDataSource = value;}}

/// 
/// <summary>
/// Географические координаты левого верхнего угла окна отображения.
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
/// Степень уменьшения карты. Если > 1, то карта уменьшается, если < 1, то 
/// увеличивается. Неположительные значения недопустимы.
/// </summary>
/// 

public double Zoom {get {return m_Zoom;} set {m_Zoom = value;}}
protected double m_Zoom = 1.0;      

///
/// <summary>
/// Ширина области отображения в пикселях.
/// </summary>
///

public int Width {get {return m_Width;} set {m_Width = value;}}
protected int m_Width;

///
/// <summary>
/// Высота области отображения в пикселях. Она неположительная.
/// </summary>
///

public int Height {get {return m_Height;} set {m_Height = value;}}
protected int m_Height;
}
}