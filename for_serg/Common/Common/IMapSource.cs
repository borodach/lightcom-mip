///////////////////////////////////////////////////////////////////////////////
//
//  File:           IMapSource.cs
//
//  Facility:       ����������� GPS ������
//
//
//  Abstract:       ���������, ��������������� ������ � �����.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  20-11-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using GPS.Common;

namespace GPS.Dispatcher.Controls
{
/// 
/// <summary>
/// ���������, ��������������� ������ � �����.
/// </summary>
/// 

public interface IMapSource
{

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// �������� �������� ����� � �������� Graphics. 
/// </summary>
/// <param name="g">� ���� ������ ���������� �������� �����.</param>
/// <param name="geoLeftUpper">�������������� ����� ������� ���� ����������� 
/// ��������� �����.</param>
/// <param name="geoRightBottom">�������������� ������ ������ ���� ����������� 
/// ��������� �����.</param>
/// <param name="imgLeftUpper">����� ������� ���� �������������� � image, 
/// � ������� ����� ���������� �������� </param>
/// <param name="imgRightBottom">������ ������ ���� �������������� � image, 
/// � ������� ����� ���������� ��������</param>
/// 
////////////////////////////////////////////////////////////////////////////////

void DrawMap (Graphics g, 
              GlobalPoint geoLeftUpper, 
              GlobalPoint geoRightBottom,
              MapPoint imgLeftUpper,
              MapPoint imgRightBottom);

//
// ��������� ��� �������������� ������������� ��������� � �������� � �������.
//

IPositionMapper PositionMapper
{
    get;
}

IMapPanesManager MapPanesManager
{
	get;
	set;
}
////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���� ����� ��������� ��� �����, �� �� ���������� ����������, ����� 
/// ��� �� �������� �����.
/// </summary>
/// <param name="point"></param>
/// 
////////////////////////////////////////////////////////////////////////////////

void FixPoint (GlobalPoint point);        

}

//public class SimpleMapSource : IMapSource
//{
//
//////////////////////////////////////////////////////////////////////////////////
/////
///// <summary>
///// �������� �������� ����� � �������� Graphics. 
///// </summary>
///// <param name="g">� ���� ������ ���������� �������� �����.</param>
///// <param name="geoLeftUpper">�������������� ����� ������� ���� ����������� 
///// ��������� �����.</param>
///// <param name="geoRightBottom">�������������� ������ ������ ���� ����������� 
///// ��������� �����.</param>
///// <param name="imgLeftUpper">����� ������� ���� �������������� � image, 
///// � ������� ����� ���������� �������� </param>
///// <param name="imgRightBottom">������ ������ ���� �������������� � image, 
///// � ������� ����� ���������� ��������</param>
///// 
//////////////////////////////////////////////////////////////////////////////////
//
//public void DrawMap (Graphics g,
//                     GlobalPoint geoLeftUpper, 
//                     GlobalPoint geoRightBottom,
//                     MapPoint imgLeftUpper,
//                     MapPoint imgRightBottom)
//{
//    MapPoint leftUpper = new MapPoint ();
//    MapPoint rightBottom = new MapPoint ();
//    m_PositionMapper.GlobalToMap (geoLeftUpper, leftUpper);
//    m_PositionMapper.GlobalToMap (geoRightBottom, rightBottom);
//
//    Rectangle destRect = new Rectangle (imgLeftUpper.x, imgLeftUpper.y,
//        imgRightBottom.x - imgLeftUpper.x,
//        imgRightBottom.y - imgLeftUpper.y);
//
//    Rectangle srcRect = new Rectangle (leftUpper.x, leftUpper.y,
//        rightBottom.x - leftUpper.x,
//        rightBottom.y - leftUpper.y);
//   
//    g.DrawImage (m_Map, destRect, srcRect, GraphicsUnit.Pixel);
//}
//
//////////////////////////////////////////////////////////////////////////////////
///// 
///// <summary>
///// ���� ����� ��������� ��� �����, �� �� ���������� ����������, ����� 
///// ��� �� �������� �����.
///// </summary>
///// <param name="point"></param>
///// 
//////////////////////////////////////////////////////////////////////////////////
//
//public void FixPoint (GlobalPoint point)
//{
//    MapPoint pt = new MapPoint ();
//    GlobalPoint leftUpper = new GlobalPoint ();
//    GlobalPoint rightBottom = new GlobalPoint ();
//    pt.x = 0;
//    pt.y = 0;
//    m_PositionMapper.MapToGlobal (pt, leftUpper);
//    pt.x = m_Map.Width;
//    pt.y = m_Map.Height;
//    m_PositionMapper.MapToGlobal (pt, rightBottom);
//    if (point.x < leftUpper.x) point.x = leftUpper.x;
//    if (point.y > leftUpper.y) point.y = leftUpper.y;
//    if (point.x > rightBottom.x) point.x = rightBottom.x;
//    if (point.y < rightBottom.y) point.y = rightBottom.y;            
//}
//
////
//// ��������� ��� �������������� ������������� ��������� � �������� � �������.
////
//
//public IPositionMapper PositionMapper
//{
//    get {return m_PositionMapper;}
//    set {m_PositionMapper = value;}
//}
//protected IPositionMapper m_PositionMapper;
//
////
//// �������� � ������.
////
//
//public Bitmap Map {get {return m_Map;} set {m_Map = value;}}
//protected Bitmap m_Map;
//}
}
