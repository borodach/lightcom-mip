using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using GPS.Common;
using GPS.Dispatcher.UI;
using GPS.Dispatcher.Common;
using System.IO;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class ClientList : System.Windows.Forms.UserControl, IPersistant
	{
		/// <summary>
		/// Обработчик событий при выполнение действий над мобильными клиентами
		/// </summary>
		public delegate void ClientListActionHandler(object sender, ClientDisplayInfo [] clients, GPS.Dispatcher.Controls.ClientPage.ActionType atype);
		/// <summary>
		/// Событие сообщающее о том, что необходимо провести обработку действия мобильного клиента
		/// </summary>
		public event ClientListActionHandler ClientListAction;

		private System.Windows.Forms.TabControl clientListTabCtrl;
		private ClientPage allClientPage;
		private System.Windows.Forms.ToolBar clientListToolBar;
		private System.Windows.Forms.ToolBarButton removeGroupBtn;
		private System.Windows.Forms.ToolBarButton addGroupBtn;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private System.Windows.Forms.ToolBarButton propertyBtn;
		private System.Windows.Forms.ImageList tabImageList;
		private System.Windows.Forms.ImageList toolBarImageList;
		private System.ComponentModel.IContainer components;

		public ClientList()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			m_mobileClients = new MobileClientList();
			allClientPage.DisplayClients.OnlyGroupSettings = true;
			this.allClientPage.ReloadImage();

			if (true == AutoLoadListsFromFile)
			{
				LoadClientDataFromFile();
			}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ClientList));
			this.clientListTabCtrl = new System.Windows.Forms.TabControl();
			this.allClientPage = new GPS.Dispatcher.Controls.ClientPage();
			this.tabImageList = new System.Windows.Forms.ImageList(this.components);
			this.clientListToolBar = new System.Windows.Forms.ToolBar();
			this.removeGroupBtn = new System.Windows.Forms.ToolBarButton();
			this.addGroupBtn = new System.Windows.Forms.ToolBarButton();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.propertyBtn = new System.Windows.Forms.ToolBarButton();
			this.toolBarImageList = new System.Windows.Forms.ImageList(this.components);
			this.clientListTabCtrl.SuspendLayout();
			this.SuspendLayout();
			// 
			// clientListTabCtrl
			// 
			this.clientListTabCtrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clientListTabCtrl.Controls.Add(this.allClientPage);
			this.clientListTabCtrl.ImageList = this.tabImageList;
			this.clientListTabCtrl.Location = new System.Drawing.Point(0, 32);
			this.clientListTabCtrl.Name = "clientListTabCtrl";
			this.clientListTabCtrl.SelectedIndex = 0;
			this.clientListTabCtrl.Size = new System.Drawing.Size(248, 376);
			this.clientListTabCtrl.TabIndex = 0;
			// 
			// allClientPage
			// 
			this.allClientPage.AutoScroll = true;
			this.allClientPage.ImageIndex = 0;
			this.allClientPage.IsTreeVisible = false;
			this.allClientPage.Location = new System.Drawing.Point(4, 23);
			this.allClientPage.Name = "allClientPage";
			this.allClientPage.ShowGroupOnMap = false;
			this.allClientPage.Size = new System.Drawing.Size(240, 349);
			this.allClientPage.TabIndex = 0;
			this.allClientPage.Text = "Все";
			this.allClientPage.DisplayClientAction += new GPS.Dispatcher.Controls.ClientPage.DisplayClientActionHandler(this.page_DisplayClientAction);
			this.allClientPage.ChangeImage += new GPS.Dispatcher.Controls.ClientPage.ChangeImageHandler(this.clientPage_ChangeImage);
			// 
			// tabImageList
			// 
			this.tabImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.tabImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("tabImageList.ImageStream")));
			this.tabImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// clientListToolBar
			// 
			this.clientListToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clientListToolBar.AutoSize = false;
			this.clientListToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																								 this.addGroupBtn,
																								 this.removeGroupBtn,
																								 this.toolBarButton1,
																								 this.propertyBtn});
			this.clientListToolBar.ButtonSize = new System.Drawing.Size(12, 12);
			this.clientListToolBar.Divider = false;
			this.clientListToolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.clientListToolBar.DropDownArrows = true;
			this.clientListToolBar.ImageList = this.toolBarImageList;
			this.clientListToolBar.Location = new System.Drawing.Point(0, 0);
			this.clientListToolBar.Name = "clientListToolBar";
			this.clientListToolBar.ShowToolTips = true;
			this.clientListToolBar.Size = new System.Drawing.Size(248, 24);
			this.clientListToolBar.TabIndex = 1;
			this.clientListToolBar.Wrappable = false;
			this.clientListToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.clientListToolBar_ButtonClick);
			// 
			// removeGroupBtn
			// 
			this.removeGroupBtn.ImageIndex = 2;
			this.removeGroupBtn.Text = "-";
			this.removeGroupBtn.ToolTipText = "Удалить текущую группу";
			// 
			// addGroupBtn
			// 
			this.addGroupBtn.ImageIndex = 0;
			this.addGroupBtn.ToolTipText = "Добавить новую группу";
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// propertyBtn
			// 
			this.propertyBtn.ImageIndex = 1;
			this.propertyBtn.ToolTipText = "Показать свойства группы";
			// 
			// toolBarImageList
			// 
			this.toolBarImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.toolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolBarImageList.ImageStream")));
			this.toolBarImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ClientList
			// 
			this.Controls.Add(this.clientListToolBar);
			this.Controls.Add(this.clientListTabCtrl);
			this.Name = "ClientList";
			this.Size = new System.Drawing.Size(248, 408);
			this.clientListTabCtrl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		#region IPersistant Members

		public void Reset()
		{
			// TODO:  Add ClientList.Reset implementation
		}

		public bool SaveGuts(System.IO.Stream stream)
		{
			// TODO:  Add ClientList.SaveGuts implementation
			return false;
		}

		public bool RestoreGuts(System.IO.Stream stream)
		{
			// TODO:  Add ClientList.RestoreGuts implementation
			return false;
		}

		#endregion

		#region Controls Message Handlers
		
		private void clientListToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			
			if(removeGroupBtn == e.Button)
			{
				DeleteActivePage();
			}

			if (addGroupBtn == e.Button)
			{
				AddNewPage();
			}

			if (propertyBtn == e.Button)
			{
				ActivePropertyPage();
			}
		}

		private void clientPage_ChangeImage(object sender, Image newImage)
		{
			Bitmap bmp = new Bitmap(newImage, tabImageList.ImageSize);
			ClientPage page = (ClientPage)sender;
			tabImageList.Images.Add(bmp);
			page.ImageIndex = tabImageList.Images.Count - 1;			
			
		}

		private void page_DisplayClientAction(ClientPage page, ClientDisplayInfo [] sender, GPS.Dispatcher.Controls.ClientPage.ActionType type)
		{
			switch(type)
			{
				case ClientPage.ActionType.Move:
					string [] groups = new string[clientListTabCtrl.TabPages.Count];
					for (int i = 0; i < clientListTabCtrl.TabPages.Count; i++)
					{
						groups[i] = clientListTabCtrl.TabPages[i].Text;
					}
					SelectGroup dlg = new SelectGroup();
					dlg.InitGroupList(groups);
					if (DialogResult.Cancel == dlg.ShowDialog())
						return;
					if (dlg.SelectedGroupName == page.Text)
						return;

					for (int i = 0; i < clientListTabCtrl.TabPages.Count; i++)
					{
						if (dlg.SelectedGroupName == clientListTabCtrl.TabPages[i].Text)
						{
							for (int j = 0; j < sender.Length; j ++)
							{
								page.Remove(sender[j].ClientId, false);
								((ClientPage)clientListTabCtrl.TabPages[i]).Add(sender[j]);
							}
							break;
						}
					}
					break;
				case ClientPage.ActionType.Delete:
					if (page != allClientPage)
						for (int i = 0; i < sender.Length; i++)
						{
							allClientPage.Add(sender[i]);
						}
					break;
				default:
					break;
			}

			if (null != ClientListAction)
			{
				ClientListAction(page, sender, type);
			}
		}

		#endregion

		public void ActivePropertyPage()
		{
			PropertyPage((ClientPage)clientListTabCtrl.SelectedTab);
		}

		private void PropertyPage(ClientPage page)
		{
			if (null == page)
				return;

			PropertyDialog dlg = new PropertyDialog();
			dlg.AddProperty((IProperties)page);
			dlg.ShowDialog();
		}

		public void DeleteActivePage()
		{
			DeletePage((ClientPage)clientListTabCtrl.SelectedTab);
		}

		private void DeletePage(ClientPage page)
		{
			if (null == page)
				return;

			if (allClientPage == page)
			{
				MessageBox.Show("Данную группу нельзя удалить.", "Внимание!", MessageBoxButtons.OK, 
					MessageBoxIcon.Information);
				return;
			}
			if( MessageBox.Show("Вы действительно хотите удалить группу \"" + page.Text + "\"?", "Внимание!", 
				MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				page.RemoveAll();
				page.ChangeImage -= new GPS.Dispatcher.Controls.ClientPage.ChangeImageHandler(clientPage_ChangeImage);
				page.DisplayClientAction -= new GPS.Dispatcher.Controls.ClientPage.DisplayClientActionHandler(page_DisplayClientAction);
				clientListTabCtrl.TabPages.Remove(page);
			}
		}

		public void AddNewPage()
		{
			ClientPage page = new ClientPage("Группа 1");
			PropertyDialog dlg = new PropertyDialog();
			dlg.AddProperty((IProperties)page);
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				AddPage(page);
			}
		}

		/// <summary>
		/// добавляет группу клиентов, при этом инициализирует необходимые параметры и события
		/// для нормального функционирования группы.
		/// </summary>
		/// <param name="page">Добавляемая группа.</param>
		private void AddPage(ClientPage page)
		{
			page.ChangeImage += new GPS.Dispatcher.Controls.ClientPage.ChangeImageHandler(clientPage_ChangeImage);
			page.DisplayClientAction += new GPS.Dispatcher.Controls.ClientPage.DisplayClientActionHandler(page_DisplayClientAction);
			page.ReloadImage();
			clientListTabCtrl.TabPages.Add(page);
		}

		public void Add(MobileClientList objList)
		{
			for (int i = 0; i < objList.Count; i++)
			{
				Add(objList[i]);
			}
		}

		public void Add(MobileClientInfo obj)
		{
			if (null != m_mobileClients)
			{
				m_mobileClients.Add(obj);
				AddToDisplayList(obj);
			}
		}

		public bool LoadClientDataFromFile()
		{
			if (!File.Exists(FileMobileClients) || !File.Exists(FileDisplayClients))
			{
				MessageBox.Show("Не удается обнаружить файлы с данными, по заданному пути.\nПроверьте настройки программы.",
					"Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}

			if (MobileClients.Count > 0)
				MobileClients.Reset();
			clientListTabCtrl.TabPages.Clear();
			clientListTabCtrl.TabPages.Add(allClientPage);

			try
			{
				FileStream file = File.Open(FileMobileClients, FileMode.Open, FileAccess.Read);
			
				int count = Utils.ReadInt(file);
				for(int i = 0; i < count; i++)
				{
					MobileClientInfo mci = new MobileClientInfo();
					if (mci.RestoreGuts(file))
					{
						MobileClients.Add(mci);
					}
				}
				file.Close();

				file = File.Open(FileDisplayClients, FileMode.Open, FileAccess.Read);

				allClientPage.RestoreGuts(file);

				count = Utils.ReadInt(file);
				for (int i = 0; i < count; i++)
				{
					ClientPage page = new ClientPage();
					AddPage(page);
					if (!page.RestoreGuts(file))
					{
						MessageBox.Show("Не могу загрузить данные о группе отображения. Группа будет удалена из списка.",
							"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
						clientListTabCtrl.TabPages.Remove(page);
					}
				}
				file.Close();

				return true;
			}
			catch(Exception)
			{
					return false;
			}
		}

		public bool SaveClientDataToFile()
		{
			FileStream file;

			try
			{
				file = File.Open(FileMobileClients, FileMode.OpenOrCreate, FileAccess.Write);

				Utils.Write(file, MobileClients.Count);
				for (int i = 0; i < MobileClients.Count; i++)
				{
					MobileClients[i].SaveGuts(file);
				}
				file.Flush();
				file.Close();

				file = File.Open(FileDisplayClients, FileMode.OpenOrCreate, FileAccess.Write);

				allClientPage.SaveGuts(file);

				Utils.Write(file, clientListTabCtrl.TabPages.Count - 1);
				for (int i = 0; i < clientListTabCtrl.TabPages.Count; i++)
				{
					if (allClientPage == clientListTabCtrl.TabPages[i])
						continue;
					else
						((ClientPage)clientListTabCtrl.TabPages[i]).SaveGuts(file);
				}
				file.Flush();
				file.Close();
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		private void AddToDisplayList(MobileClientInfo obj)
		{
			for (int i = 0; i < clientListTabCtrl.TabPages.Count; i++)
			{
				if (((ClientPage)clientListTabCtrl.TabPages[i]).DisplayClients.DefaultClientDisplayInfo != 
					((ClientPage)clientListTabCtrl.TabPages[i]).DisplayClients[obj.ClientId])
				{
					return;
				}
			}

			allClientPage.Add(obj);
		}

		private int GetTabIndexFromTab(ClientPage page)
		{
			int res = -1;
			
			for (int i = 0; i < clientListTabCtrl.TabPages.Count; i++)
			{
				if (page == clientListTabCtrl.TabPages[i])
				{
					res = i; 
					break;
				}
			}

			return res;
		}

		public ClientDisplayInfo GetDisplayClientSettings (string clientID)
		{
			ClientDisplayInfo res = null;
			ClientPage page;

			for (int i = 0; i < clientListTabCtrl.TabPages.Count; i ++)
			{
				page = (ClientPage) clientListTabCtrl.TabPages [i];

				if (page.DisplayClients [clientID] != page.DisplayClients.DefaultClientDisplayInfo)
				{
					if (!page.ShowGroupOnMap)
						return res;

					res = page.DisplayClients [clientID];
					if (page.DisplayClients.OnlyGroupSettings)
					{
						res.ClientDisplaySettings = page.DisplayClients.GroupDisplayInfoSettings;
					}
				}
			}

			return res;
		}

		/// <summary>
		/// Автоматическая загрузка списков клиентов из файла.
		/// </summary>
		private bool m_autoLoadListsFromFile;
		public bool AutoLoadListsFromFile
		{
			get{return m_autoLoadListsFromFile;}
			set{m_autoLoadListsFromFile = value;}
		}

		/// <summary>
		/// список мобильных клиентов
		/// </summary>
		private MobileClientList m_mobileClients;
		public MobileClientList MobileClients
		{
			get{return m_mobileClients;}
			set{m_mobileClients = value;}
		}
		
		/// <summary>
		/// путь к файлу со списком мобильных клиентов
		/// </summary>
		private string m_fileMobileClients = /*Utils.GetExeDirectory() + */"D:\\VC#Projects\\ClientList\\Clients.mcl";
		public string FileMobileClients
		{
			get{return m_fileMobileClients;}
			set{m_fileMobileClients = value;}
		}

		/// <summary>
		/// путь к файлу со списком отображения клиентов
		/// </summary>
		private string m_fileDisplayClients = /*Utils.GetExeDirectory() + */"D:\\VC#Projects\\ClientList\\Clients\\Clients.dcl";

		public string FileDisplayClients
		{
			get{return m_fileDisplayClients;}
			set{m_fileDisplayClients = value;}
		}

	}
}
