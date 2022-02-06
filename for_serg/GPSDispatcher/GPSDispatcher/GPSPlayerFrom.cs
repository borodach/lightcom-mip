using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GPS.Dispatcher.Controls;
using GPS.Dispatcher.Cache;
using GPS.Common;

namespace GPSDispatcher
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class GPSPlayerForm : System.Windows.Forms.Form
	{
		private struct ObjectData
		{
			public ObjectPosition m_position;
			public GPS.Dispatcher.UI.ClientDisplayInfo m_cdi;
			public GPS.Dispatcher.UI.DisplayInfoSettings m_gdis;
			public string m_clientID;
			public bool m_showOnMap;
			public bool m_groupSettings;

			public bool IsInit
			{
				get 
				{
					return (null != m_cdi && null != m_position);
				}
			}
		}

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.MainMenu GPSPlayerFormMenu;
		private System.Windows.Forms.MenuItem viewMenuItem;
		private System.Windows.Forms.MenuItem loadMapMenuItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem moveMapMenuItem;
		private System.Windows.Forms.MenuItem centerMapMenuItem;
		private System.Windows.Forms.MenuItem zoomMapMenuItem;
		private System.Windows.Forms.MenuItem zoomInMenuItem;
		private System.Windows.Forms.MenuItem zoomOutMenuItem;
		private GPS.Dispatcher.Controls.MapWindowCtrl mapWindowCtrl1;
		private GPS.Dispatcher.Controls.GPSPlayBackCtrl gpsPlayBackCtrl1;
		private GPS.Dispatcher.Controls.ClientList clientList1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GPSPlayerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			this.gpsPlayBackCtrl1.ChangeEventHandler = new GPS.Dispatcher.Controls.PlaybackControl.TimeChangedEventHandler (PlaybackCtrlTimeChangedEventHandler);
			this.gpsPlayBackCtrl1.GPSDataCacheLoaded += new GPS.Dispatcher.Controls.GPSPlayBackCtrl.GPSDataCacheLoadedHandler(gpsPlayBackCtrl1_GPSDataCacheLoaded);
			this.clientList1.ClientListAction += new GPS.Dispatcher.Controls.ClientList.ClientListActionHandler(clientList1_ClientListAction);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.mapWindowCtrl1 = new GPS.Dispatcher.Controls.MapWindowCtrl();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.GPSPlayerFormMenu = new System.Windows.Forms.MainMenu();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.viewMenuItem = new System.Windows.Forms.MenuItem();
			this.loadMapMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.moveMapMenuItem = new System.Windows.Forms.MenuItem();
			this.centerMapMenuItem = new System.Windows.Forms.MenuItem();
			this.zoomMapMenuItem = new System.Windows.Forms.MenuItem();
			this.zoomInMenuItem = new System.Windows.Forms.MenuItem();
			this.zoomOutMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.gpsPlayBackCtrl1 = new GPS.Dispatcher.Controls.GPSPlayBackCtrl();
			this.clientList1 = new GPS.Dispatcher.Controls.ClientList();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.mapWindowCtrl1);
			this.groupBox1.Location = new System.Drawing.Point(0, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(576, 447);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Окно карты";
			// 
			// mapWindowCtrl1
			// 
			this.mapWindowCtrl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mapWindowCtrl1.Location = new System.Drawing.Point(8, 16);
			this.mapWindowCtrl1.MapMode = GPS.Dispatcher.Controls.MapWindowCtrl.MapViewMode.Move;
			this.mapWindowCtrl1.Name = "mapWindowCtrl1";
			this.mapWindowCtrl1.RightButtonCenterTo = false;
			this.mapWindowCtrl1.Size = new System.Drawing.Size(560, 424);
			this.mapWindowCtrl1.TabIndex = 0;
			this.mapWindowCtrl1.Zoom = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.gpsPlayBackCtrl1);
			this.groupBox2.Location = new System.Drawing.Point(0, 455);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(576, 136);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Проигрыватель данных";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.clientList1);
			this.groupBox3.Location = new System.Drawing.Point(584, 8);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(264, 583);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Объекты";
			// 
			// GPSPlayerFormMenu
			// 
			this.GPSPlayerFormMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							  this.menuItem9,
																							  this.viewMenuItem});
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 0;
			this.menuItem9.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem10});
			this.menuItem9.Text = "Файл";
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 0;
			this.menuItem10.Text = "Выход";
			this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
			// 
			// viewMenuItem
			// 
			this.viewMenuItem.Index = 1;
			this.viewMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						 this.loadMapMenuItem,
																						 this.menuItem1,
																						 this.moveMapMenuItem,
																						 this.centerMapMenuItem,
																						 this.zoomMapMenuItem,
																						 this.menuItem7,
																						 this.menuItem8});
			this.viewMenuItem.Text = "Карта";
			// 
			// loadMapMenuItem
			// 
			this.loadMapMenuItem.Index = 0;
			this.loadMapMenuItem.Text = "Загрузить карту";
			this.loadMapMenuItem.Click += new System.EventHandler(this.loadMapMenuItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 1;
			this.menuItem1.Text = "-";
			// 
			// moveMapMenuItem
			// 
			this.moveMapMenuItem.Index = 2;
			this.moveMapMenuItem.Text = "Перетащить";
			this.moveMapMenuItem.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// centerMapMenuItem
			// 
			this.centerMapMenuItem.Index = 3;
			this.centerMapMenuItem.Text = "Центрировать";
			this.centerMapMenuItem.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// zoomMapMenuItem
			// 
			this.zoomMapMenuItem.Index = 4;
			this.zoomMapMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							this.zoomInMenuItem,
																							this.zoomOutMenuItem});
			this.zoomMapMenuItem.Text = "Массштаб";
			// 
			// zoomInMenuItem
			// 
			this.zoomInMenuItem.Index = 0;
			this.zoomInMenuItem.Text = "Увеличить";
			this.zoomInMenuItem.Click += new System.EventHandler(this.zoomInMenuItem_Click);
			// 
			// zoomOutMenuItem
			// 
			this.zoomOutMenuItem.Index = 1;
			this.zoomOutMenuItem.Text = "Уменьшить";
			this.zoomOutMenuItem.Click += new System.EventHandler(this.zoomOutMenuItem_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 5;
			this.menuItem7.Text = "-";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 6;
			this.menuItem8.Text = "Менеджер карт";
			// 
			// gpsPlayBackCtrl1
			// 
			this.gpsPlayBackCtrl1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.gpsPlayBackCtrl1.DirectionMode = GPS.Dispatcher.Controls.GPSDataPlayer.PlaybackDirectionEnum.pdForward;
			this.gpsPlayBackCtrl1.EventBasedPlaybackDelay = System.TimeSpan.Parse("00:00:01");
			this.gpsPlayBackCtrl1.Location = new System.Drawing.Point(80, 16);
			this.gpsPlayBackCtrl1.Name = "gpsPlayBackCtrl1";
			this.gpsPlayBackCtrl1.PlaybackMode = GPS.Dispatcher.Controls.GPSDataPlayer.PlaybackModeEnum.pmEventBased;
			this.gpsPlayBackCtrl1.PlayerCash = null;
			this.gpsPlayBackCtrl1.Size = new System.Drawing.Size(416, 112);
			this.gpsPlayBackCtrl1.TabIndex = 0;
			this.gpsPlayBackCtrl1.TimeBasedPlaybackSpeed = 1;
			// 
			// clientList1
			// 
			this.clientList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clientList1.AutoLoadListsFromFile = false;
			this.clientList1.FileDisplayClients = "D:\\VC#Projects\\ClientList\\Clients\\Clients.dcl";
			this.clientList1.FileMobileClients = "D:\\VC#Projects\\ClientList\\Clients.mcl";
			this.clientList1.Location = new System.Drawing.Point(8, 16);
			this.clientList1.Name = "clientList1";
			this.clientList1.Size = new System.Drawing.Size(248, 560);
			this.clientList1.TabIndex = 0;
			// 
			// GPSPlayerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(848, 593);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Menu = this.GPSPlayerFormMenu;
			this.MinimumSize = new System.Drawing.Size(640, 480);
			this.Name = "GPSPlayerForm";
			this.Text = "Окно проигрывателя";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new GPSPlayerForm());
		}

		private void loadMapMenuItem_Click(object sender, System.EventArgs e)
		{
			LoadMap();
		}

		/// <summary>
		/// Загружает карту для использования, с выводом диалогового окна со списком доступных карт.
		/// </summary>
		/// <returns>true если удачно, иначе false</returns>
		public bool LoadMap()
		{
			bool result = false;

			MapsLoader dlg = new MapsLoader();
			if(DialogResult.OK == dlg.ShowDialog())
			{
				if (null != dlg.manager) 
				{
					MapPoint mp = new MapPoint();
					mp.x = 0;
					mp.y = 0;
					GlobalPoint gp = new GlobalPoint();
					dlg.manager.PositionMapper.MapToGlobal(mp, gp);
					mapWindowCtrl1.MapDataSource.MapPanesManager = dlg.manager;
					mapWindowCtrl1.MapPosition = gp;
					mapWindowCtrl1.RedrawMap();
					result = true;
				}
			}
			
			return result;
		}

		/// <summary>
		/// Обработка сообщений от проигрывателя данных GPS.
		/// </summary>
		/// <param name="sender">проигрыватель данных GPS, инициатор события</param>
		/// <param name="oldTime"></param>
		/// <param name="newEvent"></param>
		private void PlaybackCtrlTimeChangedEventHandler(PlaybackControl sender, DateTime oldTime, DateTime newEvent)
		{
			if (sender.LastProcessedEvent != newEvent)
			{
				if (!mapWindowCtrl1.IsInitMapManager)
				{
					return;
				}

				if (null == m_objectsPosition)
					return;

				ObjectPosition [] pos = new ObjectPosition [m_objectsPosition.Length];
				sender.Cache.GetPositonsAtTime(pos, newEvent);
				
				for (int i = 0; i < pos.Length; i ++)
				{
					m_objectsPosition [i].m_position = pos [i];
				}

				RedrawObjects ();
			}
		}

		/// <summary>
		/// Отрисовывает все объекты на карте, в соответствии с последними загруженными данными в m_objectsPosition [].
		/// </summary>
		private void RedrawObjects ()
		{
			if (null == m_objectsPosition)
				return;

			mapWindowCtrl1.RedrawMap();

			
			for (int i = 0; i < m_objectsPosition.Length; i ++)
			{
				DrawObject (m_objectsPosition [i]);
			}
		}

		private void DrawObject (ObjectData obj)
		{
			DrawObject (obj, false);
		}

		private void DrawObject (ObjectData obj, bool centerMapToObject)
		{
			GlobalPoint pnt = new GlobalPoint ();
			if (obj.IsInit)
			{
				if (!obj.m_showOnMap)
					return;

				if (true == centerMapToObject)
				{
					GlobalPoint gp = new GlobalPoint ();
					gp.x = obj.m_position.X;
					gp.y = obj.m_position.Y;
					mapWindowCtrl1.MapCenterTo (gp);
				}

				pnt.x = obj.m_position.X;
				pnt.y = obj.m_position.Y;
				DrawingObjectInfo doi = new DrawingObjectInfo ();
				doi.LeftUpperCorner = pnt;

				if (!obj.m_groupSettings)
					doi.DisplaySettings = obj.m_cdi.ClientDisplaySettings;
				else
					doi.DisplaySettings = obj.m_gdis;

				mapWindowCtrl1.DrawObject (doi);
			}
		}

		private void gpsPlayBackCtrl1_GPSDataCacheLoaded(object sender, GPSDataCache cache)
		{
			m_objectsPosition = new ObjectData [cache.Count];
			
			MobileClientInfo mci;
			for (int i = 0; i < cache.Count; i ++)
			{
				mci = new MobileClientInfo();
				mci.ClientId = cache [i].ClientId;

				clientList1.Add(mci);

//				m_objectsPosition [i].m_position = new ObjectPosition ();
				m_objectsPosition [i].m_clientID = cache [i].ClientId;
			}
			
		}

		private void menuItem10_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.Move;
			ChangeMapMenuItemState(mapWindowCtrl1.MapMode);
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.Center;
			ChangeMapMenuItemState(mapWindowCtrl1.MapMode);
		}

		private void ChangeMapMenuItemState(MapWindowCtrl.MapViewMode mode)
		{
			moveMapMenuItem.Checked = false;
			centerMapMenuItem.Checked = false;
			zoomOutMenuItem.Checked = false;
			zoomInMenuItem.Checked = false;
			zoomMapMenuItem.Checked = false;
			
			switch(mode) {
				case MapWindowCtrl.MapViewMode.Move:
					moveMapMenuItem.Checked = true;
					break;
				case MapWindowCtrl.MapViewMode.Center:
					centerMapMenuItem.Checked = true;
					break;
				case MapWindowCtrl.MapViewMode.ZoomIn:
					zoomInMenuItem.Checked = true;
					break;
				case MapWindowCtrl.MapViewMode.ZoomOut:
					zoomOutMenuItem.Checked = true;
					break;
			default:
					break;
			}
		}

		/// <summary>
		/// Поиск последних обновленных данных клиента по идентификатору.
		/// </summary>
		/// <param name="clientId">Идентификатор клиента.</param>
		/// <returns>Данные клиента, если поиск не удачный то пустая структура.</returns>
		private ObjectData ObjectDataFromClientID (string clientId)
		{
			for (int i = 0; i < m_objectsPosition.Length; i ++)
			{
				if (m_objectsPosition [i].m_cdi.ClientId == clientId)
				{
					return m_objectsPosition [i];
				}
			}

			return new ObjectData ();
		}

		/// <summary>
		/// Сохраняет последние характеристики объекта для более быстрого использования при отображении.
		/// </summary>
		/// <param name="cdi">Характеристики отображения объекта</param>
		/// <param name="showOnMap">Показать объект на карте если true иначе не показывать</param>
		/// <param name="groupSettings">Использовать для отображения групповые настройки, если true</param>
		/// <param name="dispGroupSettings">Характеристики отображения группы, к которой принадлежит объект</param>
		/// <returns></returns>
		private ObjectData SetObjectData (GPS.Dispatcher.UI.ClientDisplayInfo cdi, bool showOnMap, bool groupSettings,
			GPS.Dispatcher.UI.DisplayInfoSettings dispGroupSettings)
		{
			for (int i = 0; i < m_objectsPosition.Length; i ++)
			{
				if (m_objectsPosition [i].m_clientID == cdi.ClientId)
				{
					m_objectsPosition [i].m_cdi = cdi;
					m_objectsPosition [i].m_showOnMap = showOnMap;
					m_objectsPosition [i].m_groupSettings = groupSettings;
					m_objectsPosition [i].m_gdis = dispGroupSettings;

					return m_objectsPosition [i];
				}
			}

			return new ObjectData ();
		}

		private void zoomInMenuItem_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.ZoomIn;
			ChangeMapMenuItemState(mapWindowCtrl1.MapMode);
		}

		private void zoomOutMenuItem_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.ZoomOut;
			ChangeMapMenuItemState(mapWindowCtrl1.MapMode); 
		}

		private void clientList1_ClientListAction(object sender, GPS.Dispatcher.UI.ClientDisplayInfo[] clients, GPS.Dispatcher.Controls.ClientPage.ActionType atype)
		{
			ClientPage page = (ClientPage) sender;
			switch (atype)
			{
				case GPS.Dispatcher.Controls.ClientPage.ActionType.ShowOnMap:
					for (int i = 0; i < clients.Length; i ++)
					{
						ObjectData obj = SetObjectData (clients [i], page.ShowGroupOnMap, page.DisplayClients.OnlyGroupSettings, page.DisplayClients.GroupDisplayInfoSettings);

						DrawObject (obj, true);
					}
					break;

				case GPS.Dispatcher.Controls.ClientPage.ActionType.UpdateClientInfo:
					SetObjectData (clients [0], page.ShowGroupOnMap, page.DisplayClients.OnlyGroupSettings, page.DisplayClients.GroupDisplayInfoSettings);
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// Массив структур для хранения последних обновленных данных о клиентах.
		/// </summary>
		private ObjectData [] m_objectsPosition;

	}
}
