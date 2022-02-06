using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GPS.Dispatcher.Controls;

namespace TestApp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private GPS.Dispatcher.Controls.GPSPlayBackCtrl gpsPlayBackCtrl1;
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
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
            this.gpsPlayBackCtrl1.ChangeEventHandler = new GPS.Dispatcher.Controls.PlaybackControl.TimeChangedEventHandler(this.TimerHandler);			
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
			this.gpsPlayBackCtrl1 = new GPS.Dispatcher.Controls.GPSPlayBackCtrl();
			this.SuspendLayout();
			// 
			// gpsPlayBackCtrl1
			// 
			this.gpsPlayBackCtrl1.EventBasedPlaybackDelay = System.TimeSpan.Parse("00:00:01");
			this.gpsPlayBackCtrl1.Location = new System.Drawing.Point(40, 200);
			this.gpsPlayBackCtrl1.Name = "gpsPlayBackCtrl1";
			this.gpsPlayBackCtrl1.PlaybackMode = GPS.Dispatcher.Controls.GPSDataPlayer.PlaybackModeEnum.pmEventBased;
			this.gpsPlayBackCtrl1.Size = new System.Drawing.Size(416, 112);
			this.gpsPlayBackCtrl1.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 322);
			this.Controls.Add(this.gpsPlayBackCtrl1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
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

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void TimerHandler(GPS.Dispatcher.Controls.PlaybackControl sender,
			DateTime oldTime, DateTime newEvent)
		{
			
		}
	}
}
