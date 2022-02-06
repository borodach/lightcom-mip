///////////////////////////////////////////////////////////////////////////////
//
//  File:           IMapSource.cs
//
//  Facility:       Отображение GPS данных
//
//
//  Abstract:       Интерфейс, предоставляющий доступ к карте.
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
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
/// Интерфейс, предоставляющий доступ к карте.
/// </summary>
/// 

public interface IMapSource
{

////////////////////////////////////////////////////////////////////////////////
///
/// <summary>
/// Копирует фрагмент карты в заданный Graphics. 
/// </summary>
/// <param name="g">В этот объект копируется фрагмент карты.</param>
/// <param name="geoLeftUpper">Географический левый верхний угол копируемого 
/// фрагмента карты.</param>
/// <param name="geoRightBottom">Географический правый нижний угол копируемого 
/// фрагмента карты.</param>
/// <param name="imgLeftUpper">Левый верхний угол прямоугольника в image, 
/// в который будет скопирован фрагмент </param>
/// <param name="imgRightBottom">Правый нижний угол прямоугольника в image, 
/// в который будет скопирован фрагмент</param>
/// 
////////////////////////////////////////////////////////////////////////////////

void DrawMap (Graphics g, 
              GlobalPoint geoLeftUpper, 
              GlobalPoint geoRightBottom,
              MapPoint imgLeftUpper,
              MapPoint imgRightBottom);

//
// Интерфейс для преобразования географически координат в экранные и обратно.
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
/// Если точка находится вне карты, то ее координаты изменяются, чтобы 
/// она не покидала карту.
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
///// Копирует фрагмент карты в заданный Graphics. 
///// </summary>
///// <param name="g">В этот объект копируется фрагмент карты.</param>
///// <param name="geoLeftUpper">Географический левый верхний угол копируемого 
///// фрагмента карты.</param>
///// <param name="geoRightBottom">Географический правый нижний угол копируемого 
///// фрагмента карты.</param>
///// <param name="imgLeftUpper">Левый верхний угол прямоугольника в image, 
///// в который будет скопирован фрагмент </param>
///// <param name="imgRightBottom">Правый нижний угол прямоугольника в image, 
///// в который будет скопирован фрагмент</param>
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
///// Если точка находится вне карты, то ее координаты изменяются, чтобы 
///// она не покидала карту.
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
//// Интерфейс для преобразования географически координат в экранные и обратно.
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
//// Картинка с картой.
////
//
//public Bitmap Map {get {return m_Map;} set {m_Map = value;}}
//protected Bitmap m_Map;
//}
}
