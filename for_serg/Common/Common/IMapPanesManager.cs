//////////////////////////////////////////////////////////////////////////
///интерфейс:	IMapPanesManager
///описание:	Описывает поведение менеджера по склейке кусков карты
///				для заданной области.
///автор:		К.С. Дураков
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
		/// возвращает собранный кусок карты для заданных физических координат
		/// </summary>
		/// <param name="mapDimension">область для которой необходимо собрать изображение</param>
		/// <param name="panes">выходной параметр, список кусков входящих в заданную область</param>
		/// <returns>true если удачно, иначе false</returns>
		bool GetMapPanes(Rectangle mapDimension, out MapPane [] panes);
		/// <summary>
		/// возвращает собранный кусок карты для заданных географических координат
		/// </summary>
		/// <param name="mapDimension">область для которой необходимо собрать изображение</param>
		/// <param name="mapImage">выходной параметр, список кусков входящих в заданную область</param>
		/// <returns>true если удачно, иначе false</returns>
		bool GetMapPanes(RectangleF mapDimension, out MapPane [] panes);
		/// <summary>
		/// сбрасывает состояние менеджера
		/// </summary>
		void Reset();
		/// <summary>
		/// проверка на то что менеджер может функционировать
		/// </summary>
		bool IsInit
		{
			get;
		}
		/// <summary>
		/// Объект для работы с картой представленной географическими координатами.
		/// </summary>
		IPositionMapper PositionMapper
		{
			get;
		}
	}
}
