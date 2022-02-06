using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;

using GPS.Common;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public sealed class MapWindowCtrl : System.Windows.Forms.UserControl
	{
		public enum MapViewMode
		{
			ZoomIn, 
			ZoomOut,
			Move,
			Center,
			None
		};

		public delegate void OnMapViewChangeEventHandler(PictureBox sender, MapWindow source);
		public event OnMapViewChangeEventHandler MapViewChange;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapWindowCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			mapWindowBox.Image = new Bitmap(this.Width, this.Height);
			MapMode = MapViewMode.Move;
			//инициализация элемента отвечающего за отображение карты на экране.
			m_mapWnd = new MapWindow();
			m_mapWnd.Height = this.Height;
			m_mapWnd.Width = this.Width;
			MapDataSource = new MapSource();
			m_mapWnd.Position = new GlobalPoint();
			//инициализация изображения для кэширования видимой части карты.
			m_cashMap = new Bitmap(this.Width, this.Height);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mapWindowBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// mapWindowBox
			// 
			this.mapWindowBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mapWindowBox.Location = new System.Drawing.Point(4, 4);
			this.mapWindowBox.Name = "mapWindowBox";
			this.mapWindowBox.Size = new System.Drawing.Size(192, 168);
			this.mapWindowBox.TabIndex = 0;
			this.mapWindowBox.TabStop = false;
			this.mapWindowBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapWindowBox_MouseUp);
			this.mapWindowBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapWindowBox_MouseMove);
			this.mapWindowBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapWindowBox_MouseDown);
			// 
			// MapWindowCtrl
			// 
			this.Controls.Add(this.mapWindowBox);
			this.Name = "MapWindowCtrl";
			this.Size = new System.Drawing.Size(200, 176);
			this.Resize += new System.EventHandler(this.MapWindowCtrl_Resize);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// перерисовывает отображаемую часть карты
		/// </summary>
		public void RedrawMap()
		{
			if (!m_isInit) return;

			if (mapWindowBox.Width > 0 && mapWindowBox.Height > 0)
			{
				Graphics g = Graphics.FromImage(mapWindowBox.Image);
				g.Clear(Color.Gray);

				try
				{
					m_mapWnd.DrawMap(g, 0, 0, 0, 0, mapWindowBox.Width, mapWindowBox.Height);
					
					if (null != MapViewChange)
					{
						MapViewChange(mapWindowBox, m_mapWnd);
					}
				}
				catch(Exception)
				{
//					MessageBox.Show("Ошибка при работе с компонентом отрисовки карты.");
				}

				mapWindowBox.Refresh();
				//mapWindowBox.Image = m_cashMap;
			}
		}

		/// <summary>
		/// обработка нажатия левой кнопки мыши в области контрола
		/// </summary>
		/// <param name="sender">источник события</param>
		/// <param name="e">аргументы события</param>
		private void mapWindowBox_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (MapViewMode.None == MapMode || null == m_mapWnd.MapDataSource)
			{
				return;
			}

			if (MouseButtons.Left == e.Button)
			{
				Point pnt;
				pnt = this.PointToClient(Cursor.Position);
				switch(MapMode) 
				{
					case MapViewMode.Move:
						m_isDrag = true;
						m_basePoint.X = e.X;
						m_basePoint.Y = e.Y;
						m_cashMap = (Image)mapWindowBox.Image.Clone();
						break;
					case MapViewMode.ZoomIn:
						Zoom -= ZoomStep;
						m_mapWnd.CenterTo(pnt.X, pnt.Y);
						RedrawMap();
						break;
					case MapViewMode.ZoomOut:
						Zoom += ZoomStep;
						m_mapWnd.CenterTo(pnt.X, pnt.Y);
						RedrawMap();
						break;
					case MapViewMode.Center:
						m_mapWnd.CenterTo(pnt.X, pnt.Y);
						RedrawMap();
						break;
					default:
						break;
				}
			}
/*				if (MapViewMode.Move == MapMode)
				{
					m_isDrag = true;
					m_basePoint.X = e.X;
					m_basePoint.Y = e.Y;
					m_cashMap = (Image)mapWindowBox.Image.Clone();
				}
				else
				{
					Point pnt;
					pnt = this.PointToClient(Cursor.Position);

					if (MapViewMode.ZoomIn == MapMode) 
					{
						Zoom += ZoomStep;
						m_mapWnd.CenterTo(pnt.X, pnt.Y);
					}
					if (MapViewMode.ZoomOut == MapMode)
					{
						Zoom -= ZoomStep;
						m_mapWnd.CenterTo(pnt.X, pnt.Y);
					}
					if (MapViewMode.Center == MapMode)
					{
						m_mapWnd.CenterTo(pnt.X, pnt.Y);
					}

					RedrawMap();
				}
			}*/
			
			if (MouseButtons.Right == e.Button)
			{
				if(RightButtonCenterTo)
				{
					Point pnt;
					pnt = this.PointToClient(Cursor.Position);
					m_mapWnd.CenterTo(pnt.X, pnt.Y);

					RedrawMap();
				}
			}
		}

		private void mapWindowBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (m_isDrag)
			{
				int offsetX = e.X - m_basePoint.X;
				int offsetY = e.Y - m_basePoint.Y;
				int srcX, srcY, dstX, dstY, dstW, dstH;

				if (offsetX > 0) 
				{
					srcX = offsetX; 
					dstX = 0; 
					dstW = m_cashMap.Width - offsetX; 
				}
				else
				{
					srcX = 0;
					dstX = -offsetX;
					dstW = m_cashMap.Width - dstX;
				}

				if (offsetY > 0) 
				{
					srcY = offsetY; 
					dstY = 0; 
					dstH = m_cashMap.Height - offsetY; 
				}
				else
				{
					srcY = 0;
					dstY = -offsetY;
					dstH = m_cashMap.Height - dstY;
				}

				Graphics g = Graphics.FromImage(mapWindowBox.Image);
				g.Clear(Color.White);

				g.DrawImage(m_cashMap, srcX, srcY, new Rectangle(dstX, dstY, dstW, dstH), GraphicsUnit.Pixel);

				mapWindowBox.Refresh();
				g.Dispose();
			}
		}

		private void mapWindowBox_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (MouseButtons.Left == e.Button)
			{
				if (m_isDrag)
				{
					int offsetX = e.X - m_basePoint.X;
					int offsetY = e.Y - m_basePoint.Y;
					m_basePoint.X = e.X;
					m_basePoint.Y = e.Y;

					try
					{
						m_mapWnd.Move(-offsetX, -offsetY);
					}
					catch(Exception)
					{
						MessageBox.Show("Ошибка при работе с компонентом, отвечающим за работу с картой.");
					}
				
					RedrawMap();

					m_isDrag = false;
				}
			}
		}
	
		private void MapWindowCtrl_Resize(object sender, System.EventArgs e)
		{
			mapWindowBox.Width = this.Width;
			mapWindowBox.Height = this.Height;
			m_mapWnd.Width = this.Width;
			m_mapWnd.Height = this.Height;

			mapWindowBox.Image.Dispose();
			mapWindowBox.Image = new Bitmap(this.Width, this.Height);

			RedrawMap();
		}

		public void MapMove(int dx, int dy)
		{
			m_mapWnd.Move(dx, dy);
			RedrawMap();
		}

		public void MapCenterTo(int x, int y)
		{
			m_mapWnd.CenterTo(x, y);
			RedrawMap();
		}

		public void MapCenterTo(GlobalPoint point)
		{
			m_mapWnd.CenterTo(point);
			RedrawMap();
		}

		public GlobalPoint GeoPointFromWindowPoint(int x, int y)
		{
			return m_mapWnd.GeoPointFromWindowPoint(x, y);
		}

		public void WindowPointFromGeoPoint(GlobalPoint point, out int x, out int y)
		{
			m_mapWnd.WindowPointFromGeoPoint(point, out x, out y);
		}

		public void DrawObject (DrawingObjectInfo doi)
		{
			int x, y;
			int mx, my;

			WindowPointFromGeoPoint (MapPosition, out mx, out my);
			WindowPointFromGeoPoint (doi.LeftUpperCorner, out x, out y);
			Graphics g = Graphics.FromImage (mapWindowBox.Image);
			g.DrawImageUnscaled (doi.DisplaySettings.ClientImage, x - mx, y - my, 
				doi.DisplaySettings.ImageSize.Width, doi.DisplaySettings.ImageSize.Height);
			mapWindowBox.Refresh ();
		}

		/// <summary>
		/// Проверяет попадает ли запрашиваемая точка в видимую, на экране, область карты.
		/// </summary>
		/// <param name="pnt">Точка которую необходимо проверить</param>
		/// <returns>Если true то точка попадает в видимую на экране область карты, иначе false.</returns>
		public bool GeoPointInVisibleGeoMapRectangle (GlobalPoint pnt)
		{
			return ((MapPosition.x < pnt.x && (MapPosition.x + GeoWidth) > pnt.x) && 
				(MapPosition.y < pnt.y && (MapPosition.y + GeoHeight) > pnt.y));
		}

		/// <summary>
		/// Класс для отображения карты на экране.
		/// </summary>
		private MapWindow m_mapWnd;

		/// <summary>
		/// Кеш изображения карты, видимой на экране.
		/// </summary>
		private Image m_cashMap;

		/// <summary>
		/// если контрол инициализирован то true, иначе false.
		/// </summary>
		private bool m_isInit = false;
		
		/// <summary>
		/// Координаты в момент начала перетаскивания изображения карты по экрану.
		/// </summary>
		private Point m_basePoint = new Point();
		private System.Windows.Forms.PictureBox mapWindowBox;

		/// <summary>
		/// если активен режим перетаскивания то true, иначе false.
		/// </summary>
		private bool m_isDrag = false;

		/// <summary>
		/// Поставщик данных о карте для контрола.
		/// </summary>
		public IMapSource MapDataSource
		{
			get{return m_mapWnd.MapDataSource;}
			set
			{
				m_mapWnd.MapDataSource = value; 
				m_isInit = true;
			}
		}

		public bool IsInitMapManager
		{
			get { return (null != m_mapWnd.MapDataSource.MapPanesManager); }
		}

		public GlobalPoint MapPosition
		{
			get{return m_mapWnd.Position;}
			set{m_mapWnd.Position = value;}
		}

		public double GeoHeight
		{
			get{return m_mapWnd.GeoHeight;}
		}

		public double GeoWidth
		{
			get{return m_mapWnd.GeoWidth;}
		}

		public const double MaxZoom = 10;
		public const double MinZoom = 0;
		public const double ZoomStep = 0.1F;

		public double Zoom
		{
			get{return m_mapWnd.Zoom;}
			set
			{
				if (value > MinZoom && value < MaxZoom)
				{
					m_mapWnd.Zoom = value;
				}
			}
		}

		private MapViewMode m_mode;
		public MapViewMode MapMode
		{
			get{return m_mode;}
			set{m_mode = value;}
		}

		/// <summary>
		/// указывает есть ли центрирование карты по правой кнопке мыши
		/// </summary>
		private bool m_isCenterRB = true;
		public bool RightButtonCenterTo
		{
			get{return m_isCenterRB;}
			set{m_isCenterRB = value;}
		}
	}
}
