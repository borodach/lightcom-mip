using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using GPS.Dispatcher.Controls;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for MapsLoader.
	/// </summary>
	public class MapsLoader : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox mapsList;
		private System.Windows.Forms.Button loadMapBtn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cancelBtn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MapsLoader()
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
			this.mapsList = new System.Windows.Forms.ComboBox();
			this.loadMapBtn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mapsList
			// 
			this.mapsList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.mapsList.Location = new System.Drawing.Point(8, 32);
			this.mapsList.Name = "mapsList";
			this.mapsList.Size = new System.Drawing.Size(216, 21);
			this.mapsList.TabIndex = 0;
			// 
			// loadMapBtn
			// 
			this.loadMapBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.loadMapBtn.Location = new System.Drawing.Point(232, 8);
			this.loadMapBtn.Name = "loadMapBtn";
			this.loadMapBtn.Size = new System.Drawing.Size(72, 24);
			this.loadMapBtn.TabIndex = 1;
			this.loadMapBtn.Text = "Загрузить";
			this.loadMapBtn.Click += new System.EventHandler(this.loadMapBtn_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(216, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Список карт доступных к загрузке:";
			// 
			// cancelBtn
			// 
			this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelBtn.Location = new System.Drawing.Point(232, 32);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new System.Drawing.Size(72, 24);
			this.cancelBtn.TabIndex = 3;
			this.cancelBtn.Text = "Отмена";
			// 
			// MapsLoader
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelBtn;
			this.ClientSize = new System.Drawing.Size(312, 60);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.loadMapBtn);
			this.Controls.Add(this.mapsList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MapsLoader";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Загрузка карты";
			this.Load += new System.EventHandler(this.MapsLoader_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void MapsLoader_Load(object sender, System.EventArgs e)
		{
			mng = new MapsManager();
			mng.InitManager();
			
			if (!mng.LoadMaps())
			{
				this.Close();
			}

			MapsManager.Map [] maps = mng.Maps;
			for (int i = 0; i < maps.Length; i++)
			{
				if (maps[i].m_isUse) 
				{
					mapsList.Items.Add(maps[i].m_mapRusName);
				}
			}
		}

		private void loadMapBtn_Click(object sender, System.EventArgs e)
		{
			if (0 == mapsList.Items.Count)
			{
				return;
			}

			if (-1 == mapsList.SelectedIndex)
			{
				return;
			}

			manager = mng.LoadMap(mapsList.SelectedIndex);

			Close();
		}

		private MapsManager mng;
		public IMapPanesManager manager = null;
	}
}
