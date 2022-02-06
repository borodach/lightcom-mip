namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    partial class PlayerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose (bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose ();
            }
            base.Dispose (disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container ();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (PlayerForm));
            this.mainMenu1 = new System.Windows.Forms.MainMenu (this.components);
            this.gpsPlayBackCtrl1 = new LightCom.MiP.Dispatcher.Controls.GPSPlayBackCtrl ();
            this.btnShow = new System.Windows.Forms.Button ();
            this.SuspendLayout ();
            // 
            // gpsPlayBackCtrl1
            // 
            this.gpsPlayBackCtrl1.DirectionMode = LightCom.MiP.Dispatcher.Controls.GPSDataPlayer.PlaybackDirectionEnum.pdForward;
            this.gpsPlayBackCtrl1.EventBasedPlaybackDelay = System.TimeSpan.Parse ("00:00:01");
            this.gpsPlayBackCtrl1.KeyToConnect = null;
            this.gpsPlayBackCtrl1.Location = new System.Drawing.Point (1, -2);
            this.gpsPlayBackCtrl1.Name = "gpsPlayBackCtrl1";
            this.gpsPlayBackCtrl1.PlaybackMode = LightCom.MiP.Dispatcher.Controls.GPSDataPlayer.PlaybackModeEnum.pmEventBased;
            this.gpsPlayBackCtrl1.PlayerCache = null;
            this.gpsPlayBackCtrl1.Size = new System.Drawing.Size (416, 98);
            this.gpsPlayBackCtrl1.TabIndex = 0;
            this.gpsPlayBackCtrl1.TimeBasedPlaybackSpeed = 1;
            this.gpsPlayBackCtrl1.GPSDataChanged += new LightCom.MiP.Dispatcher.Common.PlaybackControl.TimeChangedEventHandler (this.OnGPSDataChanged);
            this.gpsPlayBackCtrl1.GPSDataCacheLoaded += new LightCom.MiP.Dispatcher.Controls.GPSPlayBackCtrl.GPSDataCacheLoadedHandler (this.OnGPSDataLoaded);
            // 
            // btnShow
            // 
            this.btnShow.Location = new System.Drawing.Point (144, 90);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size (131, 23);
            this.btnShow.TabIndex = 1;
            this.btnShow.Text = "Показать объект";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler (this.btnShow_Click);
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size (419, 120);
            this.Controls.Add (this.btnShow);
            this.Controls.Add (this.gpsPlayBackCtrl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "PlayerForm";
            this.ShowInTaskbar = false;
            this.Text = "Панель управления МиП";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.PlayerForm_FormClosing);
            this.ResumeLayout (false);

        }

        #endregion

        private LightCom.MiP.Dispatcher.Controls.GPSPlayBackCtrl gpsPlayBackCtrl1;
        private System.Windows.Forms.Button btnShow;
    }
}