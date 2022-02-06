using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GPS.Dispatcher.Controls;
using GPS.Common;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private GPS.Dispatcher.Controls.MapWindowCtrl mapWindowCtrl1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.mapWindowCtrl1 = new GPS.Dispatcher.Controls.MapWindowCtrl();
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(112, 352);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 16);
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(192, 352);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 16);
			this.button2.TabIndex = 2;
			this.button2.Text = "button2";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// mapWindowCtrl1
			// 
			this.mapWindowCtrl1.Location = new System.Drawing.Point(8, 8);
			this.mapWindowCtrl1.MapMode = GPS.Dispatcher.Controls.MapWindowCtrl.MapViewMode.Move;
			this.mapWindowCtrl1.Name = "mapWindowCtrl1";
			this.mapWindowCtrl1.RightButtonCenterTo = true;
			this.mapWindowCtrl1.Size = new System.Drawing.Size(272, 272);
			this.mapWindowCtrl1.TabIndex = 3;
			this.mapWindowCtrl1.Zoom = 1;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3,
																					  this.menuItem4,
																					  this.menuItem5});
			this.menuItem1.Text = "Карта";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "Увеличить";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "Уменьшить";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 2;
			this.menuItem4.Text = "Центровать";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "Двигать";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(16, 288);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(264, 20);
			this.textBox1.TabIndex = 4;
			this.textBox1.Text = "textBox1";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(16, 312);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(264, 20);
			this.textBox2.TabIndex = 5;
			this.textBox2.Text = "textBox2";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(288, 377);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.mapWindowCtrl1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			MapsLoader dlg = new MapsLoader();
			dlg.ShowDialog();

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

				GlobalPoint pnt = new GlobalPoint();
				pnt.x = 82.9157624;
				pnt.y = 54.9950885;

				int x, y;
				mapWindowCtrl1.WindowPointFromGeoPoint(pnt, out x, out y);
				mapWindowCtrl1.MapCenterTo (x, y);
				mapWindowCtrl1.WindowPointFromGeoPoint(mapWindowCtrl1.MapPosition, out x, out y);
				textBox1.Text = "x: " + mapWindowCtrl1.MapPosition.x.ToString() + " : y: " + mapWindowCtrl1.MapPosition.y.ToString();
				textBox2.Text = "x: " + x.ToString() + " : y: " + y.ToString();


				DrawingObjectInfo doi = new DrawingObjectInfo ();
				doi.LeftUpperCorner = pnt;
				doi.DisplaySettings = new GPS.Dispatcher.UI.DisplayInfoSettings ();
				mapWindowCtrl1.DrawObject (doi);
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			MapsManagerForm mng = new MapsManagerForm();
			mng.ShowDialog();
		}

		private void menuItem2_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.ZoomIn;
		}

		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.ZoomOut;
		}

		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.Center;
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			mapWindowCtrl1.MapMode = MapWindowCtrl.MapViewMode.Move;
		}
	}
}
