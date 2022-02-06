using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for MapsManagerForm.
	/// </summary>
	public class MapsManagerForm : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapsManagerForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.activateBtn = new System.Windows.Forms.Button();
			this.deactivateBtn = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.activeMapsList = new System.Windows.Forms.ListBox();
			this.inactiveMapsList = new System.Windows.Forms.ListBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.DetailsTabCtrl = new System.Windows.Forms.TabControl();
			this.mapDetailsPage = new System.Windows.Forms.TabPage();
			this.mapIsUseLabel = new System.Windows.Forms.Label();
			this.mapLastUpdateLabel = new System.Windows.Forms.Label();
			this.mapRusNameLabel = new System.Windows.Forms.Label();
			this.mapNameLabel = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.mapUploadPage = new System.Windows.Forms.TabPage();
			this.closeBtn = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.DetailsTabCtrl.SuspendLayout();
			this.mapDetailsPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.activateBtn);
			this.groupBox1.Controls.Add(this.deactivateBtn);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.activeMapsList);
			this.groupBox1.Controls.Add(this.inactiveMapsList);
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(552, 168);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Используемые карты";
			// 
			// activateBtn
			// 
			this.activateBtn.Location = new System.Drawing.Point(248, 96);
			this.activateBtn.Name = "activateBtn";
			this.activateBtn.Size = new System.Drawing.Size(56, 23);
			this.activateBtn.TabIndex = 5;
			this.activateBtn.Text = "<<";
			this.activateBtn.Click += new System.EventHandler(this.activateBtn_Click);
			// 
			// deactivateBtn
			// 
			this.deactivateBtn.Location = new System.Drawing.Point(248, 72);
			this.deactivateBtn.Name = "deactivateBtn";
			this.deactivateBtn.Size = new System.Drawing.Size(56, 23);
			this.deactivateBtn.TabIndex = 4;
			this.deactivateBtn.Text = ">>";
			this.deactivateBtn.Click += new System.EventHandler(this.deactivateBtn_Click);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(312, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "Не используемые карты:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Используемые карты:";
			// 
			// activeMapsList
			// 
			this.activeMapsList.Location = new System.Drawing.Point(16, 32);
			this.activeMapsList.Name = "activeMapsList";
			this.activeMapsList.Size = new System.Drawing.Size(224, 121);
			this.activeMapsList.TabIndex = 0;
			this.activeMapsList.SelectedIndexChanged += new System.EventHandler(this.activeMapsList_SelectedIndexChanged);
			// 
			// inactiveMapsList
			// 
			this.inactiveMapsList.Location = new System.Drawing.Point(312, 32);
			this.inactiveMapsList.Name = "inactiveMapsList";
			this.inactiveMapsList.Size = new System.Drawing.Size(224, 121);
			this.inactiveMapsList.Sorted = true;
			this.inactiveMapsList.TabIndex = 1;
			this.inactiveMapsList.SelectedIndexChanged += new System.EventHandler(this.inactiveMapsList_SelectedIndexChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.DetailsTabCtrl);
			this.groupBox2.Location = new System.Drawing.Point(0, 168);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(552, 232);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Дополнительно:";
			// 
			// DetailsTabCtrl
			// 
			this.DetailsTabCtrl.Controls.Add(this.mapDetailsPage);
			this.DetailsTabCtrl.Controls.Add(this.mapUploadPage);
			this.DetailsTabCtrl.Location = new System.Drawing.Point(8, 16);
			this.DetailsTabCtrl.Name = "DetailsTabCtrl";
			this.DetailsTabCtrl.SelectedIndex = 0;
			this.DetailsTabCtrl.Size = new System.Drawing.Size(536, 208);
			this.DetailsTabCtrl.TabIndex = 0;
			// 
			// mapDetailsPage
			// 
			this.mapDetailsPage.Controls.Add(this.richTextBox1);
			this.mapDetailsPage.Controls.Add(this.label7);
			this.mapDetailsPage.Controls.Add(this.mapIsUseLabel);
			this.mapDetailsPage.Controls.Add(this.mapLastUpdateLabel);
			this.mapDetailsPage.Controls.Add(this.mapRusNameLabel);
			this.mapDetailsPage.Controls.Add(this.mapNameLabel);
			this.mapDetailsPage.Controls.Add(this.label6);
			this.mapDetailsPage.Controls.Add(this.label5);
			this.mapDetailsPage.Controls.Add(this.label4);
			this.mapDetailsPage.Controls.Add(this.label3);
			this.mapDetailsPage.Location = new System.Drawing.Point(4, 22);
			this.mapDetailsPage.Name = "mapDetailsPage";
			this.mapDetailsPage.Size = new System.Drawing.Size(528, 182);
			this.mapDetailsPage.TabIndex = 0;
			this.mapDetailsPage.Text = "Детали";
			// 
			// mapIsUseLabel
			// 
			this.mapIsUseLabel.Location = new System.Drawing.Point(176, 80);
			this.mapIsUseLabel.Name = "mapIsUseLabel";
			this.mapIsUseLabel.Size = new System.Drawing.Size(160, 16);
			this.mapIsUseLabel.TabIndex = 7;
			// 
			// mapLastUpdateLabel
			// 
			this.mapLastUpdateLabel.Location = new System.Drawing.Point(176, 56);
			this.mapLastUpdateLabel.Name = "mapLastUpdateLabel";
			this.mapLastUpdateLabel.Size = new System.Drawing.Size(160, 16);
			this.mapLastUpdateLabel.TabIndex = 6;
			// 
			// mapRusNameLabel
			// 
			this.mapRusNameLabel.Location = new System.Drawing.Point(176, 32);
			this.mapRusNameLabel.Name = "mapRusNameLabel";
			this.mapRusNameLabel.Size = new System.Drawing.Size(160, 16);
			this.mapRusNameLabel.TabIndex = 5;
			// 
			// mapNameLabel
			// 
			this.mapNameLabel.Location = new System.Drawing.Point(176, 8);
			this.mapNameLabel.Name = "mapNameLabel";
			this.mapNameLabel.Size = new System.Drawing.Size(160, 16);
			this.mapNameLabel.TabIndex = 4;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(100, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "Статус:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(168, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "Последнее обновление карты:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 32);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Русское название карты:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 0;
			this.label3.Text = "Название карты:";
			// 
			// mapUploadPage
			// 
			this.mapUploadPage.Location = new System.Drawing.Point(4, 22);
			this.mapUploadPage.Name = "mapUploadPage";
			this.mapUploadPage.Size = new System.Drawing.Size(528, 182);
			this.mapUploadPage.TabIndex = 1;
			this.mapUploadPage.Text = "Карты доступные к загрузке";
			// 
			// closeBtn
			// 
			this.closeBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.closeBtn.Location = new System.Drawing.Point(472, 408);
			this.closeBtn.Name = "closeBtn";
			this.closeBtn.TabIndex = 2;
			this.closeBtn.Text = "Закрыть";
			this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 104);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 16);
			this.label7.TabIndex = 8;
			this.label7.Text = "Описание:";
			// 
			// richTextBox1
			// 
			this.richTextBox1.Location = new System.Drawing.Point(72, 104);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(448, 72);
			this.richTextBox1.TabIndex = 9;
			this.richTextBox1.Text = "Недоступно";
			// 
			// MapsManagerForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(552, 434);
			this.Controls.Add(this.closeBtn);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MapsManagerForm";
			this.Text = "Менеджер карт";
			this.Load += new System.EventHandler(this.MapsManagerForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.DetailsTabCtrl.ResumeLayout(false);
			this.mapDetailsPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void MapsManagerForm_Load(object sender, System.EventArgs e)
		{
			m_mapsManager = new MapsManager();

			if (!m_mapsManager.InitManager() || !m_mapsManager.LoadMaps())
			{
				MessageBox.Show("Не удалось провести инициализацию менеджера.", "Ошибка");
				this.Close();
			}

			MapsManager.Map [] maps = m_mapsManager.Maps;

			for (int i = 0; i < maps.Length; i++)
			{
				if (maps[i].m_isUse)
				{
					m_activeMapsArray.Add(maps[i]);
					//activeMapsList.Items.Add(maps[i].m_mapRusName);
				}
				else
				{
					m_inactiveMapsArray.Add(maps[i]);
					//inactiveMapsList.Items.Add(maps[i].m_mapRusName);
				}
			}
			ResumeBindingsMapLists();
		}

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListBox activeMapsList;
		private System.Windows.Forms.ListBox inactiveMapsList;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button deactivateBtn;
		private System.Windows.Forms.Button activateBtn;
		private System.Windows.Forms.Button closeBtn;

		private MapsManager m_mapsManager;

		private ArrayList m_activeMapsArray = new ArrayList();
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TabControl DetailsTabCtrl;
		private System.Windows.Forms.TabPage mapDetailsPage;
		private System.Windows.Forms.TabPage mapUploadPage;
		private System.Windows.Forms.Label mapNameLabel;
		private System.Windows.Forms.Label mapRusNameLabel;
		private System.Windows.Forms.Label mapLastUpdateLabel;
		private System.Windows.Forms.Label mapIsUseLabel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private ArrayList m_inactiveMapsArray = new ArrayList();

		private void deactivateBtn_Click(object sender, System.EventArgs e)
		{
			object tmpObject = activeMapsList.SelectedItem;

			if (null == tmpObject)
			{
				return;
			}

			InverseMapState(tmpObject);
		}

		private void InverseMapState(object objMap)
		{
			MapsManager.Map map = (MapsManager.Map)objMap;

			if (map.m_isUse)
			{
				map.m_isUse = false;
				m_activeMapsArray.Remove(objMap);
				m_inactiveMapsArray.Add(map);

				ResumeBindingsMapLists();
			}
			else
			{
				map.m_isUse = true;
				m_inactiveMapsArray.Remove(objMap);
				m_activeMapsArray.Add(map);

				ResumeBindingsMapLists();
			}
		}

		private void ResumeBindingsMapLists()
		{
			BindingManagerBase b;
			if (0 == m_activeMapsArray.Count)
			{
				activeMapsList.DataSource = null;
			}
			else
			{
				if (null == activeMapsList.DataSource)
				{
					activeMapsList.DataSource = m_activeMapsArray;
					activeMapsList.DisplayMember = "MapRussianName";
					activeMapsList.ValueMember = "MapCode";
				}

				b = activeMapsList.BindingContext[m_activeMapsArray];
				b.ResumeBinding();
			}
			if (0 == m_inactiveMapsArray.Count)
			{
				inactiveMapsList.DataSource = null;
			}
			else
			{
				if (null == inactiveMapsList.DataSource)
				{
					inactiveMapsList.DataSource = m_inactiveMapsArray;
					inactiveMapsList.DisplayMember = "MapRussianName";
					inactiveMapsList.ValueMember = "MapCode";
				}

				b = inactiveMapsList.BindingContext[m_inactiveMapsArray];
				b.ResumeBinding();
			}
		}

		private void activateBtn_Click(object sender, System.EventArgs e)
		{
			object tmpObject = inactiveMapsList.SelectedItem;

			if (null == tmpObject)
			{
				return;
			}

			InverseMapState(tmpObject);
		}

		private void SaveSettings()
		{
			for (int i = 0; i < m_activeMapsArray.Count; i++)
			{
				m_mapsManager.SaveMapSettings((MapsManager.Map)m_activeMapsArray[i]);
			}
			for (int i = 0; i < m_inactiveMapsArray.Count; i++)
			{
				m_mapsManager.SaveMapSettings((MapsManager.Map)m_inactiveMapsArray[i]);
			}
		}

		private void closeBtn_Click(object sender, System.EventArgs e)
		{
			SaveSettings();
		}

		private void DisplayMapDetails(MapsManager.Map map)
		{
			mapNameLabel.Text = map.m_mapName;
			mapRusNameLabel.Text = map.m_mapRusName;
			mapIsUseLabel.Text = (map.m_isUse) ? "используется" : "не используется";
			mapLastUpdateLabel.Text = map.m_lastUpdate.ToLongDateString();
		}

		private void activeMapsList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			object obj = activeMapsList.SelectedItem;
			if (null != obj)
			{
				DisplayMapDetails((MapsManager.Map)obj);
			}
		}

		private void inactiveMapsList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			object obj = inactiveMapsList.SelectedItem;

			if (null != obj)
			{
				DisplayMapDetails((MapsManager.Map)obj);
			}
		}
	}
}
