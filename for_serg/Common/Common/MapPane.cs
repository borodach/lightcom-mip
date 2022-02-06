//////////////////////////////////////////////////////////////////////////////////////////////
///Класс: MapPane
///
///Описание: Базовый класс для объектов MapPane (кусочек карты). Для определения базовых
///			методов работы с кусочками карты, получение данных, изображения заключенного
///			в описываемом кусочке карты.
///автор:	К.С. Дураков
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
		/// возвращает физическую область занимаемую кусочком карты
		/// </summary>
		/// <returns>структура Rectangle описывающая физическую область куска карты</returns>
		public Rectangle GetPhysicalDimension()
		{
			return this.m_physicalDimensionRect;
		}

		/// <summary>
		/// возвращает географическую область занимаемую кусочком карты
		/// </summary>
		/// <returns>структура RectangleF описывающая географическую область куска карты</returns>
		public RectangleF GetGEODimension()
		{
			return this.m_geoDimensionRect;
		}

		/// <summary>
		/// абстрактный метод, в переопределенном виде возвращает растровое представление куска карты
		/// </summary>
		/// <param name="retImage">выходной параметр для возврата картинки с куском карты</param>
		public abstract bool GetPaneImage(out Image retImage);

		/// <summary>
		/// область занимаемая кусочком карты на карте вцелом
		/// </summary>
		protected Rectangle m_physicalDimensionRect;

		/// <summary>
		/// географическая область описываемая кусочком карты
		/// </summary>
		protected RectangleF m_geoDimensionRect;

		/// <summary>
		/// растровое представление описываемого кусочка карты
		/// </summary>
		protected Image m_paneImage;
	}
}
