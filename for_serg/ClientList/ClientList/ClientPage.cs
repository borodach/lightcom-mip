/********************************************************************
	created:	2006/02/03
	created:	3:2:2006   17:37
	filename: 	ClientPage.cs
	file base:	ClientPage
	file ext:	cs
	author:		К.С. Дураков
	
	purpose:	Класс представляет собой расширение TabPage.
				Предназначен для отображения списков клиентов на экране.
*********************************************************************/

using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using GPS.Dispatcher.UI;
using GPS.Common;
using GPS.Dispatcher.Common;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public class ClientPage : System.Windows.Forms.TabPage, IProperties, IPersistant
	{
		public delegate void ChangeImageHandler(object sender, Image newImage);
		/// <summary>
		/// Событие изменения изображения назначенного группе.
		/// </summary>
		public event ChangeImageHandler ChangeImage;

		public delegate void DisplayClientActionHandler(ClientPage page, ClientDisplayInfo [] sender, ActionType type);
		/// <summary>
		/// Событие произведения над клиентом заданного действия.
		/// </summary>
		public event DisplayClientActionHandler DisplayClientAction;

		public enum ActionType
		{
			ShowOnMap,
			Move,
			Centering, 
			Delete,
			UpdateClientInfo
		};

		private ContextMenu m_contextMenu;
		private ImageList m_clientImageList;

		public ClientPage()
		{
			InitializeComponents();
			Reset();
		}

		public ClientPage(string pageTitle) : this()
		{
			this.Text = pageTitle;
			this.Name = pageTitle;
		}

		#region Initialize ClientPage Components

		private void InitializeComponents()
		{
			this.SuspendLayout();

			//Инициализация древовидного отображения клиентов
			m_treeClients = new TreeView();
			m_treeClients.Bounds = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top,
				this.ClientRectangle.Width, this.ClientRectangle.Bottom - 20);
			m_treeClients.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
//			m_treeClients.Visible = true;

			//инициализация отображения клиентов в виде списка
			m_listViewClients = new ListView();
			m_listViewClients.Bounds = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top,
				this.ClientRectangle.Width, this.ClientRectangle.Bottom - 20);
			m_listViewClients.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			m_listViewClients.View = View.Details;
			m_listViewClients.Columns.Add("Имя", 100, HorizontalAlignment.Center);
			m_listViewClients.Columns.Add("Код", 40, HorizontalAlignment.Center);
			m_listViewClients.Columns.Add("Компания", 150, HorizontalAlignment.Center);
			m_listViewClients.Columns.Add("Связь", 80, HorizontalAlignment.Center);
			m_listViewClients.Columns.Add("Комментарий", 200, HorizontalAlignment.Center);
			m_listViewClients.FullRowSelect = true;
			m_listViewClients.GridLines = true;
//			m_listViewClients.Visible = false;

			//флаг отображения на карте
			m_isShow = new CheckBox();
			m_isShow.Bounds = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - 20,
				this.ClientRectangle.Width, 20);
			m_isShow.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
			m_isShow.Text = "Отобразить группу на карте";
			m_isShow.Checked = false;
			m_isShow.Visible = true;

			//добавление дерева на страницу
			this.Controls.AddRange(new Control[]{m_treeClients,
													m_listViewClients,
													m_isShow});

			//инициализация контекстного меню
			m_contextMenu = new ContextMenu();
			m_treeClients.ContextMenu = m_contextMenu;
			m_listViewClients.ContextMenu = m_contextMenu;
			m_contextMenu.Popup += new EventHandler(m_contextMenu_Popup);
			MenuItem mi = new MenuItem("Свойства объекта...");
			mi.Click += new EventHandler(OnPropertyObjectClick);
			m_contextMenu.MenuItems.Add(mi);
			m_contextMenu.MenuItems.Add(new MenuItem("-"));
			mi = new MenuItem("Показать на карте");
			mi.Click += new EventHandler(OnShowOnMapObjectClick);
			m_contextMenu.MenuItems.Add(mi);
			m_contextMenu.MenuItems.Add(new MenuItem("-"));
			mi = new MenuItem("Переместить...");
			mi.Click += new EventHandler(OnMoveObjectClick);
			m_contextMenu.MenuItems.Add(mi);
			mi = new MenuItem("Удалить из группы");
			mi.Click += new EventHandler(OnDeleteObjectClick);
			m_contextMenu.MenuItems.Add(mi);
			m_contextMenu.MenuItems.Add(new MenuItem("-"));
			mi = new MenuItem("Показать...");
			mi.Click += new EventHandler(OnShowTreeClick);
			m_contextMenu.MenuItems.Add(mi);

			this.ResumeLayout(false);
		}

		#endregion

		#region Обработка сообщений контекстного меню

		private void OnPropertyObjectClick(object sender, EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			if (mi.GetContextMenu().SourceControl == m_treeClients)
			{
				ShowSelectedObjectPropertyInTree();
			}
			else
			{
				ShowSelectedObjectPropertyInList();
			}
		}

		private void OnMoveObjectClick(object sender, EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			if (mi.GetContextMenu().SourceControl == m_treeClients)
			{
				OnDisplayClientAction(true, ActionType.Move);
			}
			else
			{
				OnDisplayClientAction(false, ActionType.Move);
			}
		}

		private void OnShowOnMapObjectClick(object sender, EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			if (mi.GetContextMenu().SourceControl == m_treeClients)
			{
				OnDisplayClientAction(true, ActionType.ShowOnMap);
			}
			else
			{
				OnDisplayClientAction(false, ActionType.ShowOnMap);
			}
		}

		private void OnDeleteObjectClick(object sender, EventArgs e)
		{
			MenuItem mi = (MenuItem)sender;
			if (mi.GetContextMenu().SourceControl == m_treeClients)
			{
				OnDisplayClientAction(true, ActionType.Delete);
			}
			else
			{
				OnDisplayClientAction(false, ActionType.Delete);
			}
		}

		private void OnDisplayClientAction(bool isTree, ActionType action)
		{
			string [] clientsId;

			if (isTree)
			{
				TreeNode node = GetRootTreeNode(m_treeClients.SelectedNode);

				if (null != node)
				{
					clientsId = new string[1];
					clientsId[0] = node.Tag.ToString();
				}
				else
					return;
			}
			else
			{
				if (0 != m_listViewClients.SelectedItems.Count)
				{
					clientsId = new string[m_listViewClients.SelectedItems.Count];
					IEnumerator clientEnum = m_listViewClients.SelectedItems.GetEnumerator();
					int i = 0;
					while (clientEnum.MoveNext())
					{
						clientsId[i] = ((ListViewItem)clientEnum.Current).Tag.ToString();
						i++;
					}
				}
				else
					return;
			}

			switch(action)
			{
				case ActionType.Delete:
					string msgBoxStr = "Вы действительно хотите удалить клиента: ";
					for (int i = 0; i < clientsId.Length; i++)
					{
						if (i > 0)
							msgBoxStr += ", ";

						msgBoxStr += clientsId[i];
					}
					if (DialogResult.Yes == MessageBox.Show(msgBoxStr + " из группы?", "Удаление...", 
						MessageBoxButtons.YesNo, MessageBoxIcon.Question))
						for(int i = 0; i < clientsId.Length; i++)
							Remove(clientsId[i], false);
					break;
				case ActionType.ShowOnMap:
				case ActionType.Move:
				case ActionType.Centering:
					ClientDisplayInfo [] cdi = new ClientDisplayInfo[clientsId.Length];
					for (int i = 0; i < clientsId.Length; i++)
						cdi[i] = DisplayClients[clientsId[i]];
					if (null != DisplayClientAction/* && DisplayClients.DefaultClientDisplayInfo != cdi*/)
						DisplayClientAction(this, cdi, action);
					break;
				default:
					break;
			}
		}

		private void OnShowTreeClick(object sender, EventArgs e)
		{
			IsTreeVisible = !IsTreeVisible;
		}

		#endregion

		#region Реализация интерфейса IProperties

		public ObjectInfo GetObjectInfo()
		{
			ObjectInfo obj = new ObjectInfo();
			obj.Name = this.Text;
			obj.Text = "Группа: " + this.Text;
			PropertyGroup grp = new PropertyGroup();
			grp.Name = this.Text;
			grp.BaseObject = obj;
			GetPropertyList(grp);
			obj.BaseProperties = grp;
			grp = new PropertyGroup();
			grp.Name = "DisplaySettings";
			grp.Text = "Настройка отображения";
			grp.BaseObject = obj;
			GetPropertyList(grp);
			obj.PropertyGroups.Add(grp);

			return obj;
		}

		public void GetPropertyList(PropertyGroup group)
		{
//			ArrayList propList = new ArrayList();

			if (group.Name == this.Text)
			{
				PropertyInfo prop = GetProperty("groupName");
				group.Properties.Add(prop);
				group.Properties.Add(GetProperty("separator"));
				group.Properties.Add(GetProperty("separator"));
				prop = GetProperty("showGroupOnDisplay");
				group.Properties.Add(prop);
				prop = GetProperty("allowClientSettings");
				group.Properties.Add(prop);
			}
			if ("DisplaySettings" == group.Name)
			{
				DisplayClients.GroupDisplayInfoSettings.GetPropertyList(group);
			}
		}

		public void SetProppertyList(PropertyGroup group)
		{
			if ("DisplaySettings" == group.Name)
				DisplayClients.GroupDisplayInfoSettings.SetProppertyList(group);
			else
				for (int i = 0; i < group.Properties.Count; i++)
				{
					SetProperty((PropertyInfo)group.Properties[i]);
				}

			ImageChanged();
		}

		public PropertyInfo GetProperty(string propertyName)
		{
			PropertyInfo prop = new PropertyInfo();
			prop.Name = propertyName;

			if ("groupName" == propertyName)
			{
				prop.Text = "Имя группы:";
				prop.Type = PropertyInfo.PropertyType.Text;
				prop.Value = this.Text;
			}
			if ("showGroupOnDisplay" == propertyName)
			{
				prop.Text = "Отображать клентов группы на карте.";
				prop.Type = PropertyInfo.PropertyType.Check;
				prop.Value = m_isShow.Checked;
			}
			if ("allowClientSettings" == propertyName)
			{
				prop.Text = "Использовать настройки отображения группы для всех клиентов";				
				prop.Type = PropertyInfo.PropertyType.Check;
				prop.Value = DisplayClients.OnlyGroupSettings;
			}
			if ("separator" == propertyName)
			{
				prop.Type = PropertyInfo.PropertyType.Separator;
			}

			return prop;
		}

		public void SetProperty(PropertyInfo val)
		{
			if ("groupName" == val.Name)
			{
				this.Text = (string)val.Value;
			}
			if ("showGroupOnDisplay" == val.Name)
			{
				m_isShow.Checked = (bool)val.Value;
			}
			if ("allowClientSettings" == val.Name)
			{
				DisplayClients.OnlyGroupSettings = (bool)val.Value;
			}
		}

		#endregion

		#region IPersistant Members

		public void Reset()
		{
			m_displayClients = new ClientDisplayInfoList();

			if (null != m_clientImageList)
				m_clientImageList.Images.Clear();

			m_clientImageList = new ImageList();
			m_clientImageList.ImageSize = new Size(16, 16);
			m_clientImageList.Images.Add(new Bitmap(16, 16));

			m_treeClients.ImageList = m_clientImageList;
			m_listViewClients.SmallImageList = m_clientImageList;

			IsTreeVisible = false;
		}

		public bool SaveGuts(System.IO.Stream stream)
		{
			bool result = true;

			try
			{
				Utils.Write(stream, this.Text);
				Utils.Write(stream, Convert.ToInt32(DisplayClients.OnlyGroupSettings));
				Utils.Write(stream, Convert.ToInt32(ShowGroupOnMap));
				Utils.Write(stream, Convert.ToInt32(IsTreeVisible));
				DisplayClients.GroupDisplayInfoSettings.SaveGuts(stream);
				Utils.Write(stream, DisplayClients.Count);

				for (int i = 0; i < DisplayClients.Count; i++)
				{
					result &= DisplayClients[i].SaveGuts(stream);
				}
			}
			catch(Exception)
			{

			}

			return result;
		}

		public bool RestoreGuts(System.IO.Stream stream)
		{
			bool result = true;

			try
			{
				this.Text = Utils.ReadString(stream);
				DisplayClients.OnlyGroupSettings = Convert.ToBoolean(Utils.ReadInt(stream));
				ShowGroupOnMap = Convert.ToBoolean(Utils.ReadInt(stream));
				IsTreeVisible = Convert.ToBoolean(Utils.ReadInt(stream));
				DisplayClients.GroupDisplayInfoSettings.RestoreGuts(stream);

				int clientCount = Utils.ReadInt(stream);
				for(int i = 0; i < clientCount; i++)
				{
					ClientDisplayInfo cdi = new ClientDisplayInfo();
					if (cdi.RestoreGuts(stream))
					{
						Add(cdi);
					}
				}

				ImageChanged();
			}
			catch(Exception)
			{
				result = false;
			}

			return result;
		}

		#endregion

		/// <summary>
		/// Добавляет мобильного клиента в список отображаемых на экране клиентов.
		/// </summary>
		/// <param name="obj">Объект с описанием мобильного клиента.</param>
		public void Add(MobileClientInfo obj)
		{
			if (null == obj)
			{
				return;
			}

			ClientDisplayInfo cdi;
			cdi = DisplayClients[obj.ClientId];

			if(cdi == DisplayClients.DefaultClientDisplayInfo)
			{
				cdi = new ClientDisplayInfo();
				cdi.ClientId = obj.ClientId;
				cdi.ClientDisplayInfoChanged += new GPS.Dispatcher.UI.ClientDisplayInfo.ClientDisplayInfoChangedHandler(UpdateClientDisplayInfo);
				DisplayClients.Add(cdi);
			}

			UpdateClientDisplayInfo(cdi);
		}

		/// <summary>
		/// Добавляет мобильного клиента в список отображаемых на экране клиентов.
		/// </summary>
		/// <param name="obj">Объект с описанием характера отображения мобильного клиента.</param>
		public void Add(ClientDisplayInfo obj)
		{
			if (DisplayClients.DefaultClientDisplayInfo != DisplayClients[obj.ClientId])
			{
				return;
			}

			obj.ClientDisplayInfoChanged += new GPS.Dispatcher.UI.ClientDisplayInfo.ClientDisplayInfoChangedHandler(UpdateClientDisplayInfo);
			DisplayClients.Add(obj);
			obj.ClientDisplaySettings.ImageChanged = true;
			UpdateClientDisplayInfo(obj);
		}

		private MobileClientInfo GetMobileClientInfo(string clientID)
		{
			ClientList parentCtrl = (ClientList)this.Parent.Parent;
			return parentCtrl.MobileClients[clientID];
		}

		/// <summary>
		/// Добавляет/обновляет данные мобильного клиента в используемые элементы управления.
		/// </summary>
		/// <param name="cdi">Объект с параметрами отображения мобильного клиента.</param>
		private void UpdateClientDisplayInfo(ClientDisplayInfo cdi)
		{
			MobileClientInfo mci = GetMobileClientInfo(cdi.ClientId);

			TreeNode tn = GetClientNode(cdi.ClientId);
			ListViewItem lvi = GetClientListViewItem(cdi.ClientId);

			if (null == tn)
			{
				tn = new TreeNode();
				m_treeClients.Nodes.Add(tn);
				tn.Tag = cdi.ClientId;
			}
			else
				if (tn.Nodes.Count != 0)
					tn.Nodes.Clear();

			if (null == lvi)
			{
				lvi = new ListViewItem();
				m_listViewClients.Items.Add(lvi);
				lvi.Tag = cdi.ClientId;
			}
			else
				if (0 != lvi.SubItems.Count)
					lvi.SubItems.Clear();

			if (null != mci)
			{
				tn.Text = mci.FriendlyName;
				tn.Nodes.Add(new TreeNode("Код клиента: " + mci.ClientId, -1, -1));
				tn.Nodes.Add(new TreeNode("Компания: " + mci.Company));
				tn.Nodes.Add(new TreeNode("Связь: " + mci.LastEvent.ToShortDateString() + 
					" " + mci.LastEvent.ToShortTimeString()));
				tn.Nodes.Add(new TreeNode("Комментарий: " + mci.Comments));

				lvi.Text = mci.FriendlyName;
				lvi.SubItems.Add(mci.ClientId);
				lvi.SubItems.Add(mci.Company);
				lvi.SubItems.Add(mci.LastEvent.ToShortDateString() + " " + mci.LastEvent.ToShortTimeString());
				lvi.SubItems.Add(mci.Comments);
			}
			else
			{
				tn.Text = cdi.ClientId;
				lvi.Text = cdi.ClientId;
			}

			if (cdi.ClientDisplaySettings.ImageChanged)
			{
				Bitmap bmp = new Bitmap(cdi.ClientDisplaySettings.ClientImage, m_clientImageList.ImageSize);
				m_clientImageList.Images.Add(bmp);
				tn.ImageIndex = m_clientImageList.Images.Count - 1;
				tn.SelectedImageIndex = tn.ImageIndex;

				lvi.ImageIndex = tn.ImageIndex;

				cdi.ClientDisplaySettings.ImageChanged = false;
			}

			if (null != DisplayClientAction)
			{
				DisplayClientAction (this, new ClientDisplayInfo [] {cdi}, ActionType.UpdateClientInfo);
			}
		}

		/// <summary>
		/// Удаляет мобильного клиента из списка отображения.
		/// </summary>
		/// <param name="index">Индек удаляемого клиента.</param>
		/// <param name="controlsOnly">Если true то удалить клиента только из элементов управления,
		///	иначе удалить мобильного клиента также из списка.</param>
		public void Remove(int index, bool controlsOnly)
		{
			if (-1 == index)
			{
				return;
			}

			ClientDisplayInfo cdi = DisplayClients[index];
			if (cdi == DisplayClients.DefaultClientDisplayInfo)
			{
				return;
			}

			TreeNode clientNode = GetClientNode(cdi.ClientId);
			if (null != clientNode)
			{
				m_treeClients.Nodes.Remove(clientNode);
			}

			ListViewItem lvi = GetClientListViewItem(cdi.ClientId);
			if (null != lvi)
			{
				m_listViewClients.Items.Remove(lvi);
			}

			if (false == controlsOnly) 
			{
				cdi.ClientDisplayInfoChanged -= new GPS.Dispatcher.UI.ClientDisplayInfo.ClientDisplayInfoChangedHandler(UpdateClientDisplayInfo);
				DisplayClients.Remove(index);

				if (null != DisplayClientAction)
					DisplayClientAction(this, new ClientDisplayInfo[] {cdi}, ClientPage.ActionType.Delete);

			}
		}

		/// <summary>
		/// Удаляет мобильного клиента из списка отображения.
		/// </summary>
		/// <param name="clientId">Идентификатор удаляемого клиента.</param>
		/// <param name="controlsOnly">Если true то удалить клиента только из элементов управления,
		///	иначе удалить мобильного клиента также из списка.</param>
		public void Remove(string clientId, bool controlsOnly)
		{
			Remove(GetClientIndex(clientId), controlsOnly);
		}

		/// <summary>
		/// Удалить все элементы из группы.
		/// </summary>
		public void RemoveAll()
		{
			for (int i = 0; i < DisplayClients.Count; i++)
			{
				Remove(i, false);
			}
		}

		/// <summary>
		/// Возвращает индекс мобильного клиента в коллекции по заданному идентификатору.
		/// </summary>
		/// <param name="clientId">Идентификатор клиента.</param>
		/// <returns>Индекс клиента в коллекции. -1 если не удачно.</returns>
		public int GetClientIndex(string clientId)
		{
			int result = -1;
			for (int i = 0; i < DisplayClients.Count; i++)
			{
				if (clientId == DisplayClients[i].ClientId)
				{
					result = i;
					break;
				}
			}

			return result;
		}

		/// <summary>
		/// Возвращает узел из дерева клиентов соответствующий запрашиваемому идентификатору клиента
		/// </summary>
		/// <param name="clientId">Идентификатор клиента</param>
		/// <returns>Узел найденного дерева. Null если не удачно.</returns>
		private TreeNode GetClientNode(string clientId)
		{
			TreeNode searchNode = null;
			IEnumerator treeEnum = m_treeClients.Nodes.GetEnumerator();

			while (treeEnum.MoveNext())
			{
				if (clientId == (string)((TreeNode)treeEnum.Current).Tag)
				{
					searchNode = (TreeNode)treeEnum.Current;

					break;
				}
			}

			return searchNode;
		}

		/// <summary>
		/// Возвращает элемент ListView соответствующий запрашиваемому идентификатору клиента
		/// </summary>
		/// <param name="clientId">Идентификатор клиента</param>
		/// <returns>Найденный элемент, если не найден то null.</returns>
		private ListViewItem GetClientListViewItem(string clientId)
		{
			ListViewItem searchItem = null;
			IEnumerator itemEnum = m_listViewClients.Items.GetEnumerator();

			while(itemEnum.MoveNext())
			{
				if (clientId == (string)((ListViewItem)itemEnum.Current).Tag)
				{
					searchItem = (ListViewItem)itemEnum.Current;
					break;
				}
			}

			return searchItem;
		}

		public void ReloadImage()
		{
			ImageChanged();
		}

		private void ImageChanged()
		{
			if (null != ChangeImage && DisplayClients.GroupDisplayInfoSettings.ImageChanged)
			{
				ChangeImage(this, DisplayClients.GroupDisplayInfoSettings.ClientImage);
				DisplayClients.GroupDisplayInfoSettings.ImageChanged = false;
				this.Refresh();
			}
		}

		private void m_contextMenu_Popup(object sender, EventArgs e)
		{
			ContextMenu cmenu = (ContextMenu)sender;
			EnableContextMenuItems(cmenu, false);
			if (cmenu.SourceControl == m_treeClients)
			{
				m_treeClients.SelectedNode = m_treeClients.GetNodeAt(m_treeClients.PointToClient(Cursor.Position));
				if (null != m_treeClients.SelectedNode)
				{
					if (null == m_treeClients.SelectedNode.Parent)
						EnableContextMenuItems(cmenu, true);
				}

				cmenu.MenuItems[cmenu.MenuItems.Count -1].Enabled = true;
				cmenu.MenuItems[cmenu.MenuItems.Count -1].Text = "Показать как список";
			}
			if (cmenu.SourceControl == m_listViewClients)
			{
				if (0 != m_listViewClients.SelectedItems.Count)
				{
					EnableContextMenuItems(cmenu, true);

					if (1 < m_listViewClients.SelectedItems.Count)
					{
						MenuItem mi = GetMenuItemByText(cmenu, "Показать на карте");

						if (null != mi)
							mi.Enabled = false;
					}
				}

				cmenu.MenuItems[cmenu.MenuItems.Count -1].Enabled = true;
				cmenu.MenuItems[cmenu.MenuItems.Count -1].Text = "Показать как дерево";
			}

		}

		/// <summary>
		/// Просто проставляет флаг Enabled всем пунктам указанного контекстного меню
		/// </summary>
		/// <param name="cmenu">Контекстное меню, пункты которого надлежит обработать</param>
		/// <param name="enabled">флаг состояния пунктов меню</param>
		private void EnableContextMenuItems(ContextMenu cmenu, bool enabled)
		{
			for (int i = 0; i < cmenu.MenuItems.Count; i++)
			{
				cmenu.MenuItems[i].Enabled = enabled;
			}
		}

		/// <summary>
		/// Возвращает найденный элемент в контекстном меню. Поиск осуществляется по надписи элемента.
		/// </summary>
		/// <param name="cmenu">Контекстное меню, в котором осуществляется поиск.</param>
		/// <param name="itemText">Текст по которому осуществляется поиск элемента.</param>
		/// <returns></returns>
		private MenuItem GetMenuItemByText(ContextMenu cmenu, string itemText)
		{
			foreach(MenuItem mi in cmenu.MenuItems)
			{
				if (mi.Text == itemText)
					return mi;
			}

			return null;
		}

		/// <summary>
		/// Показать свойства активного мобильного объекта, в элементе "дерево".
		/// </summary>
		private void ShowSelectedObjectPropertyInTree()
		{
			TreeNode node = GetRootTreeNode(m_treeClients.SelectedNode);

			if (null == node)
				return;

			ClientDisplayInfo cdi = DisplayClients[node.Tag.ToString()];
			cdi.ShowObjectProperty(GetMobileClientInfo(cdi.ClientId));
		}

		private void ShowSelectedObjectPropertyInList()
		{
			if (0 != m_listViewClients.SelectedItems.Count)
			{
				IEnumerator itemEnum = m_listViewClients.SelectedItems.GetEnumerator();
				PropertyDialog pd = new PropertyDialog();

				while (itemEnum.MoveNext())
				{
					ClientDisplayInfo cdi = DisplayClients[((ListViewItem)itemEnum.Current).Tag.ToString()];
					cdi.SetMobileClientInfo(GetMobileClientInfo(cdi.ClientId));
					pd.AddProperty((IProperties)cdi);
				}

				pd.ShowDialog();
			}
		}

		private TreeNode GetRootTreeNode(TreeNode node)
		{
			if (null == node)
				return null; 

			TreeNode resNode = node;

			while (null != resNode.Parent)
				resNode = resNode.Parent;

			return resNode;
		}

		/// <summary>
		/// Отобразить свойства выбранного клиента
		/// </summary>
		public void ShowObjectProperty()
		{
			if (m_treeClients.Visible)
				ShowSelectedObjectPropertyInTree();
			if (m_listViewClients.Visible);
				ShowSelectedObjectPropertyInList();
		}

		/// <summary>
		/// Отображение клиентов в виде дерева
		/// </summary>
		private TreeView m_treeClients;

		/// <summary>
		/// Отображение клиентов в виде списка
		/// </summary>
		private ListView m_listViewClients;

		/// <summary>
		/// Отображать на карте или нет
		/// </summary>
		private CheckBox m_isShow;
		public bool ShowGroupOnMap
		{
			set
			{
				m_isShow.Checked = value;
			}
			get{return m_isShow.Checked;}
		}

		/// <summary>
		/// список отображения мобильных клиентов
		/// </summary>
		private ClientDisplayInfoList m_displayClients;
		public ClientDisplayInfoList DisplayClients
		{
			get{return m_displayClients;}
			set{m_displayClients = value;}
		}

		/// <summary>
		/// Отобразить клиентов в виде дерева если true, иначе в виде списка.
		/// </summary>
		private bool m_isTreeOnScreen;
		public bool IsTreeVisible
		{
			get{return m_isTreeOnScreen;}
			set
			{
				m_isTreeOnScreen = value;
				m_treeClients.Visible = m_isTreeOnScreen;
				m_listViewClients.Visible = !m_isTreeOnScreen;
			}
		}

	}
}
