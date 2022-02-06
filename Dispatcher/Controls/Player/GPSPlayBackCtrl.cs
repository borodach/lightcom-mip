using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.MiP.Cache;
using LightCom.MiP.Dispatcher.Common;

namespace LightCom.MiP.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class GPSPlayBackCtrl : System.Windows.Forms.UserControl
	{

        public delegate void GPSDataCacheLoadedHandler (GPSPlayBackCtrl sender, GPSDataCache cache);
		[Description("Загружены новые GPS данные"),Category("MiP")]
        public event GPSDataCacheLoadedHandler GPSDataCacheLoaded;

        [Description("Изменение положения объектов"),Category("MiP")]
        public event PlaybackControl.TimeChangedEventHandler GPSDataChanged;


		private System.Windows.Forms.Button PlayBtn;
		private System.Windows.Forms.Button ForwardWindBtn;
		private System.Windows.Forms.Button EndBtn;
        private System.Windows.Forms.Button StartBtn;
		private System.Windows.Forms.Label PBCurrentTime;
		private System.Windows.Forms.Label PBStartDateTime;
		private System.Windows.Forms.Label PBEndDateTime;
		private System.Windows.Forms.Timer PlayBackTimer;
		private System.Windows.Forms.ProgressBar timeProgressBar;
		private System.Windows.Forms.ContextMenu loadSaveGPSDataMenu;
		private System.Windows.Forms.Button LoadSaveDataBtn;
		private System.Windows.Forms.MenuItem LoadItem;
		private System.Windows.Forms.MenuItem saveItem;
		private System.Windows.Forms.OpenFileDialog openGPSDataDlg;
		private System.Windows.Forms.SaveFileDialog saveGPSDataDlg;
		private System.Windows.Forms.TrackBar trackTimeBar;
		private System.Windows.Forms.RadioButton timeRadioButton;
		private System.Windows.Forms.RadioButton eventRadioButton;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private MenuItem menuItemLoadFromServer;
		private System.ComponentModel.IContainer components;
        private MenuItem appendItem;
        private ToolTip toolTip1;
        private ImageList imageList1;
        private LightCom.MiP.Dispatcher.Common.LoadFromServerForm loadFromServerDlg;

		public GPSPlayBackCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            loadFromServerDlg = new MiP.Dispatcher.Common.LoadFromServerForm (null);
			// TODO: Add any initialization after the InitComponent call
			m_playbackCtrl = new GPSDataPlayer();
			m_playbackCtrl.Stopped = true;
			this.TimeBasedPlaybackSpeed = 1;
			this.EventBasedPlaybackDelay = TimeSpan.FromMilliseconds(1000);
			this.PlaybackMode = GPSDataPlayer.PlaybackModeEnum.pmEventBased;
			this.DirectionMode = GPSDataPlayer.PlaybackDirectionEnum.pdForward;
			PlayBackTimer.Enabled = true;

			eventRadioButton.Checked = true;

			if (null == m_playbackCtrl.Cache) 
			{
				EnableControls(false);
			}

			this.ChangeEventHandler = new PlaybackControl.TimeChangedEventHandler(this.TimerChange);

			m_controlToolTip = new ToolTip(this.components);
			m_controlToolTip.ShowAlways = true;
			m_controlToolTip.SetToolTip(EndBtn, "Переместиться к концу данных");
			m_controlToolTip.SetToolTip(StartBtn, "Переместиться к началу данных");
			m_controlToolTip.SetToolTip(trackTimeBar, "Перемотать данные");
			m_controlToolTip.SetToolTip(ForwardWindBtn, "Поменять направление проигрыванния данных");
			m_controlToolTip.SetToolTip(PlayBtn, "Начать/остановить проигрыванние данных");
			m_controlToolTip.SetToolTip(eventRadioButton, "Режим проигрыванния данных по событию");
			m_controlToolTip.SetToolTip(timeRadioButton, "Режим проигрыванния данных по времени");
			m_controlToolTip.SetToolTip(numericUpDown1, "Скорость проигрыванния данных");
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				PlayBackTimer.Enabled = false;
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
            this.components = new System.ComponentModel.Container ();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (GPSPlayBackCtrl));
            this.PlayBtn = new System.Windows.Forms.Button ();
            this.ForwardWindBtn = new System.Windows.Forms.Button ();
            this.EndBtn = new System.Windows.Forms.Button ();
            this.StartBtn = new System.Windows.Forms.Button ();
            this.timeProgressBar = new System.Windows.Forms.ProgressBar ();
            this.PBCurrentTime = new System.Windows.Forms.Label ();
            this.PBStartDateTime = new System.Windows.Forms.Label ();
            this.PBEndDateTime = new System.Windows.Forms.Label ();
            this.PlayBackTimer = new System.Windows.Forms.Timer (this.components);
            this.LoadSaveDataBtn = new System.Windows.Forms.Button ();
            this.loadSaveGPSDataMenu = new System.Windows.Forms.ContextMenu ();
            this.LoadItem = new System.Windows.Forms.MenuItem ();
            this.appendItem = new System.Windows.Forms.MenuItem ();
            this.menuItemLoadFromServer = new System.Windows.Forms.MenuItem ();
            this.saveItem = new System.Windows.Forms.MenuItem ();
            this.openGPSDataDlg = new System.Windows.Forms.OpenFileDialog ();
            this.saveGPSDataDlg = new System.Windows.Forms.SaveFileDialog ();
            this.trackTimeBar = new System.Windows.Forms.TrackBar ();
            this.timeRadioButton = new System.Windows.Forms.RadioButton ();
            this.eventRadioButton = new System.Windows.Forms.RadioButton ();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown ();
            this.label1 = new System.Windows.Forms.Label ();
            this.toolTip1 = new System.Windows.Forms.ToolTip (this.components);
            this.imageList1 = new System.Windows.Forms.ImageList (this.components);
            ((System.ComponentModel.ISupportInitialize) (this.trackTimeBar)).BeginInit ();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDown1)).BeginInit ();
            this.SuspendLayout ();
            // 
            // PlayBtn
            // 
            this.PlayBtn.ImageKey = "btnPlay.png";
            this.PlayBtn.ImageList = this.imageList1;
            this.PlayBtn.Location = new System.Drawing.Point (142, 62);
            this.PlayBtn.Name = "PlayBtn";
            this.PlayBtn.Size = new System.Drawing.Size (43, 24);
            this.PlayBtn.TabIndex = 4;
            this.toolTip1.SetToolTip (this.PlayBtn, "Воспроизведение / пауза");
            this.PlayBtn.Click += new System.EventHandler (this.PlayBtn_Click);
            // 
            // ForwardWindBtn
            // 
            this.ForwardWindBtn.ImageKey = "btnForward.png";
            this.ForwardWindBtn.ImageList = this.imageList1;
            this.ForwardWindBtn.Location = new System.Drawing.Point (185, 62);
            this.ForwardWindBtn.Name = "ForwardWindBtn";
            this.ForwardWindBtn.Size = new System.Drawing.Size (32, 24);
            this.ForwardWindBtn.TabIndex = 5;
            this.toolTip1.SetToolTip (this.ForwardWindBtn, "Прямое/обратное направление воспроизведения");
            this.ForwardWindBtn.Click += new System.EventHandler (this.ForwardWindBtn_Click);
            // 
            // EndBtn
            // 
            this.EndBtn.ImageKey = "btnRight.png";
            this.EndBtn.ImageList = this.imageList1;
            this.EndBtn.Location = new System.Drawing.Point (217, 62);
            this.EndBtn.Name = "EndBtn";
            this.EndBtn.Size = new System.Drawing.Size (32, 24);
            this.EndBtn.TabIndex = 6;
            this.toolTip1.SetToolTip (this.EndBtn, "Перейти в конец");
            this.EndBtn.Click += new System.EventHandler (this.EndBtn_Click);
            // 
            // StartBtn
            // 
            this.StartBtn.ImageKey = "btnLeft.png";
            this.StartBtn.ImageList = this.imageList1;
            this.StartBtn.Location = new System.Drawing.Point (110, 62);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size (32, 24);
            this.StartBtn.TabIndex = 3;
            this.toolTip1.SetToolTip (this.StartBtn, "Перейти в начало");
            this.StartBtn.Click += new System.EventHandler (this.StartBtn_Click);
            // 
            // timeProgressBar
            // 
            this.timeProgressBar.Location = new System.Drawing.Point (8, 35);
            this.timeProgressBar.Name = "timeProgressBar";
            this.timeProgressBar.Size = new System.Drawing.Size (400, 8);
            this.timeProgressBar.TabIndex = 6;
            // 
            // PBCurrentTime
            // 
            this.PBCurrentTime.Font = new System.Drawing.Font ("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.PBCurrentTime.Location = new System.Drawing.Point (124, 5);
            this.PBCurrentTime.Name = "PBCurrentTime";
            this.PBCurrentTime.Size = new System.Drawing.Size (168, 20);
            this.PBCurrentTime.TabIndex = 8;
            this.PBCurrentTime.Text = "14.12.2005 13:51";
            this.PBCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PBStartDateTime
            // 
            this.PBStartDateTime.Location = new System.Drawing.Point (3, 9);
            this.PBStartDateTime.Name = "PBStartDateTime";
            this.PBStartDateTime.Size = new System.Drawing.Size (111, 17);
            this.PBStartDateTime.TabIndex = 9;
            this.PBStartDateTime.Text = "12.12.2005 12:02";
            this.PBStartDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PBEndDateTime
            // 
            this.PBEndDateTime.Location = new System.Drawing.Point (291, 9);
            this.PBEndDateTime.Name = "PBEndDateTime";
            this.PBEndDateTime.Size = new System.Drawing.Size (122, 16);
            this.PBEndDateTime.TabIndex = 10;
            this.PBEndDateTime.Text = "16.12.2005 14:45";
            this.PBEndDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PlayBackTimer
            // 
            this.PlayBackTimer.Tick += new System.EventHandler (this.PlayBackTimer_Tick);
            // 
            // LoadSaveDataBtn
            // 
            this.LoadSaveDataBtn.Image = ((System.Drawing.Image) (resources.GetObject ("LoadSaveDataBtn.Image")));
            this.LoadSaveDataBtn.Location = new System.Drawing.Point (255, 62);
            this.LoadSaveDataBtn.Name = "LoadSaveDataBtn";
            this.LoadSaveDataBtn.Size = new System.Drawing.Size (30, 24);
            this.LoadSaveDataBtn.TabIndex = 8;
            this.toolTip1.SetToolTip (this.LoadSaveDataBtn, "Меню операций с файлами");
            this.LoadSaveDataBtn.Click += new System.EventHandler (this.LoadSaveDataBtn_Click);
            // 
            // loadSaveGPSDataMenu
            // 
            this.loadSaveGPSDataMenu.MenuItems.AddRange (new System.Windows.Forms.MenuItem [] {
            this.LoadItem,
            this.appendItem,
            this.menuItemLoadFromServer,
            this.saveItem});
            // 
            // LoadItem
            // 
            this.LoadItem.Index = 0;
            this.LoadItem.Text = "Загрузить из файла...";
            this.LoadItem.Click += new System.EventHandler (this.LoadItem_Click);
            // 
            // appendItem
            // 
            this.appendItem.Index = 1;
            this.appendItem.Text = "Добавить из файла...";
            this.appendItem.Click += new System.EventHandler (this.appendItem_Click);
            // 
            // menuItemLoadFromServer
            // 
            this.menuItemLoadFromServer.Index = 2;
            this.menuItemLoadFromServer.Text = "Загрузить с сервера...";
            this.menuItemLoadFromServer.Visible = true;
            this.menuItemLoadFromServer.Click += new System.EventHandler (this.OnLoadFromServer);
            // 
            // saveItem
            // 
            this.saveItem.Index = 3;
            this.saveItem.Text = "Сохранить в файл...";
            this.saveItem.Click += new System.EventHandler (this.saveItem_Click);
            // 
            // openGPSDataDlg
            // 
            this.openGPSDataDlg.DefaultExt = "txt";
            this.openGPSDataDlg.FileName = "trace";
            this.openGPSDataDlg.Filter = "Файлы с мобильных устройств|*.txt|Файлы с данными GPS|*.gps|Все файлы|*.*";
            this.openGPSDataDlg.Title = "Загрузка GPS данных из файла";
            // 
            // saveGPSDataDlg
            // 
            this.saveGPSDataDlg.DefaultExt = "gps";
            this.saveGPSDataDlg.FileName = "data";
            this.saveGPSDataDlg.Filter = "Файлы с данными GPS|*.gps|Все файлы|*.*";
            this.saveGPSDataDlg.Title = "Сохранение GPS данных в файл";
            // 
            // trackTimeBar
            // 
            this.trackTimeBar.AutoSize = false;
            this.trackTimeBar.LargeChange = 60;
            this.trackTimeBar.Location = new System.Drawing.Point (0, 27);
            this.trackTimeBar.Name = "trackTimeBar";
            this.trackTimeBar.Size = new System.Drawing.Size (416, 22);
            this.trackTimeBar.TabIndex = 0;
            this.trackTimeBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackTimeBar.MouseDown += new System.Windows.Forms.MouseEventHandler (this.trackTimeBar_MouseDown);
            this.trackTimeBar.ValueChanged += new System.EventHandler (this.trackTimeBar_ValueChanged);
            this.trackTimeBar.Scroll += new System.EventHandler (this.trackTimeBar_Scroll);
            this.trackTimeBar.MouseUp += new System.Windows.Forms.MouseEventHandler (this.trackTimeBar_MouseUp);
            this.trackTimeBar.KeyDown += new System.Windows.Forms.KeyEventHandler (this.trackTimeBar_KeyDown);
            // 
            // timeRadioButton
            // 
            this.timeRadioButton.Location = new System.Drawing.Point (8, 71);
            this.timeRadioButton.Name = "timeRadioButton";
            this.timeRadioButton.Size = new System.Drawing.Size (93, 24);
            this.timeRadioButton.TabIndex = 2;
            this.timeRadioButton.Text = "по времени";
            this.toolTip1.SetToolTip (this.timeRadioButton, "Воспроизведение по времени");
            this.timeRadioButton.CheckedChanged += new System.EventHandler (this.timeRadioButton_CheckedChanged);
            // 
            // eventRadioButton
            // 
            this.eventRadioButton.Location = new System.Drawing.Point (8, 52);
            this.eventRadioButton.Name = "eventRadioButton";
            this.eventRadioButton.Size = new System.Drawing.Size (97, 24);
            this.eventRadioButton.TabIndex = 1;
            this.eventRadioButton.Text = "по событиям";
            this.toolTip1.SetToolTip (this.eventRadioButton, "Воспроизведение по событиям");
            this.eventRadioButton.CheckedChanged += new System.EventHandler (this.eventRadioButton_CheckedChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point (354, 66);
            this.numericUpDown1.Maximum = new decimal (new int [] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal (new int [] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size (54, 20);
            this.numericUpDown1.TabIndex = 7;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip (this.numericUpDown1, "Скорость воспроизведения");
            this.numericUpDown1.Value = new decimal (new int [] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler (this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point (291, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size (58, 16);
            this.label1.TabIndex = 17;
            this.label1.Text = "скорость:";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer) (resources.GetObject ("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName (0, "btnPause.png");
            this.imageList1.Images.SetKeyName (1, "btnPlay.png");
            this.imageList1.Images.SetKeyName (2, "btnRight.png");
            this.imageList1.Images.SetKeyName (3, "btnLeft.png");
            this.imageList1.Images.SetKeyName (4, "btnBack.png");
            this.imageList1.Images.SetKeyName (5, "btnForward.png");
            // 
            // GPSPlayBackCtrl
            // 
            this.Controls.Add (this.label1);
            this.Controls.Add (this.numericUpDown1);
            this.Controls.Add (this.eventRadioButton);
            this.Controls.Add (this.timeRadioButton);
            this.Controls.Add (this.trackTimeBar);
            this.Controls.Add (this.LoadSaveDataBtn);
            this.Controls.Add (this.PBEndDateTime);
            this.Controls.Add (this.PBStartDateTime);
            this.Controls.Add (this.PBCurrentTime);
            this.Controls.Add (this.timeProgressBar);
            this.Controls.Add (this.StartBtn);
            this.Controls.Add (this.EndBtn);
            this.Controls.Add (this.ForwardWindBtn);
            this.Controls.Add (this.PlayBtn);
            this.Name = "GPSPlayBackCtrl";
            this.Size = new System.Drawing.Size (416, 95);
            ((System.ComponentModel.ISupportInitialize) (this.trackTimeBar)).EndInit ();
            ((System.ComponentModel.ISupportInitialize) (this.numericUpDown1)).EndInit ();
            this.ResumeLayout (false);

		}
		#endregion

		private void PlayBtn_Click(object sender, System.EventArgs e)
		{
			PlayBtnStateChange(!m_playbackCtrl.Stopped);
		}

		/// <summary>
		/// Изменение текста на кнопке PlayBtn
		/// </summary>
		/// <param name="state">если true то контрол в режиме останова, иначе false</param>
		public void PlayBtnStateChange(bool state)
		{
			if (state)
			{
				PlayBtn.ImageKey = "btnPlay.png";
			}
			else
			{
                PlayBtn.ImageKey = "btnPause.png";
			}

			m_playbackCtrl.Stopped = state;
		}

		/// <summary>
		/// обработчик сообщения от таймера
		/// </summary>
		/// <param name="sender">таймер</param>
		/// <param name="e">аргументы таймера</param>
		private void PlayBackTimer_Tick(object sender, System.EventArgs e)
		{
			m_playbackCtrl.ProcessTimerEvent(TimeSpan.FromMilliseconds(PlayBackTimer.Interval));
		}

		/// <summary>
		/// обработчик кнопки направления проигрывателя данных
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ForwardWindBtn_Click(object sender, System.EventArgs e)
		{
			if (GPSDataPlayer.PlaybackDirectionEnum.pdBackward == m_playbackCtrl.PlaybackDirection)
				ChangeDirectionMode(GPSDataPlayer.PlaybackDirectionEnum.pdForward);
			else 
				ChangeDirectionMode(GPSDataPlayer.PlaybackDirectionEnum.pdBackward);
		}

		/// <summary>
		/// изменение направление проигрывания данных
		/// </summary>
		/// <param name="mode">режим проигрывания (инверсия)</param>
		public void ChangeDirectionMode(GPSDataPlayer.PlaybackDirectionEnum mode)
		{
			m_playbackCtrl.PlaybackDirection = mode;

			if(GPSDataPlayer.PlaybackDirectionEnum.pdForward == mode)
			{
                ForwardWindBtn.ImageKey = "btnForward.png";
			}
			else
			{
                ForwardWindBtn.ImageKey = "btnBack.png";
			}
		}

		private void StartBtn_Click(object sender, System.EventArgs e)
		{
			//this.PBPlayPersent.Text = "0%";
			this.timeProgressBar.Value = this.timeProgressBar.Minimum;
			m_playbackCtrl.MoveToStart();
		}

		private void EndBtn_Click(object sender, System.EventArgs e)
		{
			//this.PBPlayPersent.Text = "100%";
			this.timeProgressBar.Value = this.timeProgressBar.Maximum;
			m_playbackCtrl.MoveToFinish();
		}

		/// <summary>
		/// внутренний обработчик события от GPSDataPlayer для изменения внешнего вида контрола
		/// </summary>
		/// <param name="sourse"></param>
		/// <param name="oldTime"></param>
		/// <param name="newEvent"></param>
		private void TimerChange(PlaybackControl source, DateTime oldTime, DateTime newEvent)
		{
            if (newEvent == m_playbackCtrl.LastProcessedEvent) return;
            this.trackTimeBar.Value = TimeToTrackBarPosition (m_playbackCtrl.CurrentTime - m_playbackCtrl.StartTime);

            if (null != GPSDataChanged)
            {
                GPSDataChanged (source, oldTime, newEvent);
            }

            if (m_playbackCtrl.CurrentTime == m_playbackCtrl.FinishTime &&
                m_playbackCtrl.PlaybackDirection == GPSDataPlayer.PlaybackDirectionEnum.pdForward)
            {
                this.PlayBtnStateChange (true);
            }

            if (m_playbackCtrl.CurrentTime == m_playbackCtrl.StartTime &&
                m_playbackCtrl.PlaybackDirection == GPSDataPlayer.PlaybackDirectionEnum.pdBackward)
            {
                this.PlayBtnStateChange (true);
            }
		}

		/// <summary>
		/// метод активирует/деактивирует компоненты на контроле
		/// </summary>
		/// <param name="isEnable">если true то активны, иначе false</param>
		private void EnableControls(bool isEnable)
		{
			this.eventRadioButton.Enabled = isEnable;
			this.timeRadioButton.Enabled = isEnable;
			this.PlayBtn.Enabled = isEnable;
			this.StartBtn.Enabled = isEnable;
			this.EndBtn.Enabled = isEnable;
			this.ForwardWindBtn.Enabled = isEnable;
			this.numericUpDown1.Enabled = isEnable;
			this.trackTimeBar.Enabled = isEnable;
		}

		/// <summary>
		/// инициализация контрола из подгружаемого кэша с данными
		/// </summary>
		private void InitCtrlFromCache()
		{
			try
			{
				TimeSpan diff = m_playbackCtrl.FinishTime - m_playbackCtrl.StartTime;

                DateTime currentTime = m_playbackCtrl.CurrentTime.ToLocalTime ();
                DateTime stratTime = m_playbackCtrl.StartTime.ToLocalTime ();
                DateTime finishTime = m_playbackCtrl.FinishTime.ToLocalTime ();

                this.PBStartDateTime.Text = FormatDateTime (stratTime);;
                this.PBEndDateTime.Text = FormatDateTime (finishTime);
                this.PBCurrentTime.Text = FormatDateTime (currentTime);
				//this.PBPlayPersent.Text = "0%";
                this.trackTimeBar.Minimum = TrackBarStartPosition;
				this.trackTimeBar.Maximum = TimeToTrackBarPosition (diff);
				this.trackTimeBar.Value = this.trackTimeBar.Minimum;
			}
			catch(Exception)
			{
				return;
			}

			EnableControls(true);
		}

		private void speedBar_Scroll(object sender, System.EventArgs e)
		{
		}

		/// <summary>
		/// загрузка данных из файла
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadItem_Click(object sender, System.EventArgs e)
		{
			LoadGPSDataFromFile(false);
		}

		/// <summary>
		/// сохранение данных в файл
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveItem_Click(object sender, System.EventArgs e)
		{
			SaveGPSDataToFile();
		}

		public void SaveGPSDataToFile()
		{
			if (DialogResult.OK != saveGPSDataDlg.ShowDialog ())
			{
				return;
			}

			string strFileName = saveGPSDataDlg.FileName;
			try
			{
				FileStream fs = new FileStream (strFileName, 
					FileMode.Create, 
					FileAccess.Write);

				if (! PlayerCache.SaveGuts (fs))
				{
					MessageBox.Show ("Не удалось сохранить данные.", 
						"Ошибка.");
				}
				else
				{
					MessageBox.Show ("Данные успешно сохранены.", "Информация.");
				}
				fs.Close ();
			}
			catch (Exception ex)
			{
				MessageBox.Show (ex.Message, "Ошибка загрузки.");
			}
		}

		public void LoadGPSDataFromFile(bool append)
		{
            if (DialogResult.OK != openGPSDataDlg.ShowDialog ())
            {
                return;
            }

            this.PlayBtnStateChange (true);

            string strFileName = openGPSDataDlg.FileName;
            FileStream fs = null;
            try
            {
                fs = new FileStream (strFileName,
                    FileMode.Open,
                    FileAccess.Read);
                GPSDataCache cache = new GPSDataCache ();
                bool loadOK = true;
                if (!cache.RestoreGuts (fs))
                {
                    ClientTrack track = new ClientTrack ();
                    fs.Seek (0, SeekOrigin.Begin);
                    if (track.RestoreGuts (fs))
                    {
                        cache.Reset ();
                        cache.Add (track);
                    }
                    else
                    {
                        loadOK = false;                        
                    }
				}
				
				if (null != GPSDataCacheLoaded)
				{
					GPSDataCacheLoaded(this, cache);
				}

                DateTime stratTime = cache.FirstEvent.ToLocalTime ();
                DateTime finishTime = cache.LastEvent.ToLocalTime ();
                TimeSpan diff = finishTime - stratTime;

                this.trackTimeBar.Minimum = TrackBarStartPosition;
                this.trackTimeBar.Maximum = TimeToTrackBarPosition (diff);
                this.trackTimeBar.Value = this.trackTimeBar.Minimum;

                if (append)
                {
                    if (null != PlayerCache)
                        cache.Add (PlayerCache);
                }

                PlayerCache = cache;

                if (loadOK)
                {
                    //MessageBox.Show ("Данные успешно загружены.", "Информация.");
                }
                else
                {
                    MessageBox.Show ("Не удалось загрузить данные, возможно, файл поврежден.",
                                     "Ошибка.");
                }				
				
			}
			catch (Exception ex)
			{
				MessageBox.Show (ex.Message, "Ошибка загрузки.");
			}
            finally
            {
                if (fs != null)
                    fs.Close ();
            }
		}

		private void LoadSaveDataBtn_Click(object sender, System.EventArgs e)
		{
			Point clientPnt = this.PointToClient(Cursor.Position);
			loadSaveGPSDataMenu.Show(this, clientPnt);
		}

		private void trackTimeBar_Scroll(object sender, System.EventArgs e)
		{
			//if (m_isScrollTimeBar)
			//{
			//	DateTime dt = m_playbackCtrl.StartTime;
			//	m_playbackCtrl.MoveToTime(dt.AddMinutes((double)trackTimeBar.Value));
			//}
		}

		private void trackTimeBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_isScrollTimeBar = true;
            m_bOldPlaybackState = m_playbackCtrl.Stopped;
			m_playbackCtrl.Stopped = true;
			PlayBtnStateChange(m_playbackCtrl.Stopped);
		}

		private void trackTimeBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_isScrollTimeBar = false;
            m_playbackCtrl.Stopped = m_bOldPlaybackState;
			PlayBtnStateChange(m_playbackCtrl.Stopped);
		}

		private void eventRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			ChangePlaybackMode(GPSDataPlayer.PlaybackModeEnum.pmEventBased);
		}

		private void timeRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			ChangePlaybackMode(GPSDataPlayer.PlaybackModeEnum.pmTimeBased);
		}

		/// <summary>
		/// изменение режима проигрывания данных
		/// </summary>
		/// <param name="mode">режим проигрывания данных</param>
		public void ChangePlaybackMode(GPSDataPlayer.PlaybackModeEnum mode)
		{
			m_playbackCtrl.PlaybackMode = mode;

			if (GPSDataPlayer.PlaybackModeEnum.pmEventBased == mode)
			{
				numericUpDown1.Maximum = MaxEventSpeed;
				numericUpDown1.Value = (decimal)(m_eventBasedPlaybackDelay.TotalMilliseconds / m_playbackCtrl.EventBasedPlaybackDelay.TotalMilliseconds);
			}
			else
			{
				numericUpDown1.Maximum = MaxTimeSpeed;
				numericUpDown1.Value = (decimal)m_playbackCtrl.TimeBasedPlaybackSpeed;
			}
		}

		private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
		{
			NumericUpDown speed = sender as NumericUpDown;
			int speedVal = (0 == (int)speed.Value) ? 1 : (int)speed.Value;
			if (GPSDataPlayer.PlaybackModeEnum.pmEventBased == m_playbackCtrl.PlaybackMode)
			{
				m_playbackCtrl.EventBasedPlaybackDelay = TimeSpan.FromMilliseconds(m_eventBasedPlaybackDelay.TotalMilliseconds / speedVal);
			}		
			else
			{
				m_playbackCtrl.TimeBasedPlaybackSpeed = (double)speedVal;
			}
		}

		//Свойства
		/// <summary>
		/// Кэш данных GPS.
		/// </summary>        
		public GPSDataCache PlayerCache
		{
			get{return m_playbackCtrl.Cache;}
			set
			{
				m_playbackCtrl.Cache = value;
				InitCtrlFromCache();
			}
		}

		/// <summary>
		/// проигрыватель данных GPS.
		/// </summary>
		private GPSDataPlayer m_playbackCtrl;

		/// <summary>
		/// подписка на событие изменения GPS данных
		/// </summary>
		public PlaybackControl.TimeChangedEventHandler ChangeEventHandler
		{
			set{m_playbackCtrl.TimeChanged += value;}
//			get{return m_playbackCtrl.;}
		}

		/// <summary>
		/// временная задержка во время проигрывания данных в событийном режиме
		/// </summary>
		private TimeSpan m_eventBasedPlaybackDelay;

        [Description ("Задержка между событиями при проигрывании в собыийном режиме"), Category ("MiP")]
		public TimeSpan EventBasedPlaybackDelay
		{
			get{return m_playbackCtrl.EventBasedPlaybackDelay;}
			set
			{
				m_playbackCtrl.EventBasedPlaybackDelay = value;
				m_eventBasedPlaybackDelay = value;
			}
		}

		/// <summary>
		/// Режим проигрывания данных (временной или событийный)
		/// </summary>
        [Description ("Режим проигрывания данных (временной или событийный)"), Category ("MiP")]
		public GPSDataPlayer.PlaybackModeEnum PlaybackMode
		{
			get{return m_playbackCtrl.PlaybackMode;}
			set
			{
				if (GPSDataPlayer.PlaybackModeEnum.pmEventBased == value)
				{
					eventRadioButton.Checked = true;
				}
				else
				{
					timeRadioButton.Checked = true;
				}
				ChangePlaybackMode(value);
			}
		}

		/// <summary>
		/// направление проигрывания данных (вперед или назад)
		/// </summary>
        [Description ("Направление проигрывания данных (вперед или назад)"), Category ("MiP")]
		public GPSDataPlayer.PlaybackDirectionEnum DirectionMode
		{
			get{return m_playbackCtrl.PlaybackDirection;}
			set{ChangeDirectionMode(value);}
		}

		/// <summary>
		/// флаг перетаскивания бегунка прокрутки данных пользователем
		/// </summary>
		private bool m_isScrollTimeBar = false;

		/// <summary>
		/// скорость проигрывания данных во временном режиме проигрывания
		/// </summary>
        [Description ("Скорость проигрывания данных во временном режиме проигрывания"), Category ("MiP")]
		public double TimeBasedPlaybackSpeed
		{
			get{return m_playbackCtrl.TimeBasedPlaybackSpeed;}
			set{m_playbackCtrl.TimeBasedPlaybackSpeed = value;}
		}

		/// <summary>
		/// максимальная скорость проигрывания для временного режима
		/// </summary>
		private const decimal MaxTimeSpeed = 1000;

		/// <summary>
		/// максимальная скорость проигрывания для событийного режима
		/// </summary>
		private const decimal MaxEventSpeed = 10;

		/// <summary>
		/// всплывающая подсказка на контролах
		/// </summary>
		private ToolTip m_controlToolTip;

        /// <summary>
        /// Обработчик команды загрузки данных с сервера.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoadFromServer (object sender, EventArgs e)
        {
            GPSDataCache cache = new GPSDataCache ();
            loadFromServerDlg.KeyToConnect = this.KeyToConnect;
            loadFromServerDlg.Cache = cache;
            DialogResult res = loadFromServerDlg.ShowDialog ();
            if (res != DialogResult.OK)
            {
                return;
            }

            this.PlayBtnStateChange (true);

            if (null != GPSDataCacheLoaded)
            {
                GPSDataCacheLoaded (this, cache);
            }

            DateTime stratTime = cache.FirstEvent.ToLocalTime ();
            DateTime finishTime = cache.LastEvent.ToLocalTime ();
            TimeSpan diff = finishTime - stratTime;

            this.trackTimeBar.Minimum = TrackBarStartPosition;
            this.trackTimeBar.Maximum = TimeToTrackBarPosition (diff);
            this.trackTimeBar.Value = this.trackTimeBar.Minimum;
            PlayerCache = cache;
        }

        /// <summary>
        /// Ключ шифрования начального запроса к серверу
        /// </summary>
        public byte [] KeyToConnect
        {
            get { return this.eKey; }
            set { this.eKey = value; }
        }

        /// <summary>
        /// Ключ шифрования начального запроса к серверу
        /// </summary>
        private byte [] eKey;

        /// <summary>
        /// Старое состояние кнопки Play.Используется для перетаскивания 
        /// ползунка.
        /// </summary>
        private bool m_bOldPlaybackState = true;

        private void trackTimeBar_ValueChanged (object sender, EventArgs e)
        {
            TimeSpan ts = TrackBarPositionToTime (trackTimeBar.Value);
            m_playbackCtrl.MoveToTime (m_playbackCtrl.StartTime + ts);

            TimeSpan diff = m_playbackCtrl.CurrentTime - m_playbackCtrl.StartTime;
            TimeSpan diffTotal = m_playbackCtrl.FinishTime - m_playbackCtrl.StartTime;
            this.PBCurrentTime.Text = FormatDateTime (m_playbackCtrl.CurrentTime);
            if (diffTotal.TotalSeconds != 0)
            {
                //this.PBPlayPersent.Text = (int) (diff.TotalSeconds / diffTotal.TotalSeconds) + "%";
            }
        }

        public void trackTimeBar_KeyDown (object sender, KeyEventArgs e)
        {
          int d = 0;
          int eventBasedSpeed = (int) (m_eventBasedPlaybackDelay.TotalMilliseconds / m_playbackCtrl.EventBasedPlaybackDelay.TotalMilliseconds);
          switch (e.KeyCode)
            {
                case Keys.Left:
                    d = - trackTimeBar.SmallChange;
                    break;
                case Keys.Right:
                    d = trackTimeBar.SmallChange;
                    break;
                case Keys.Up:
                    if (this.m_playbackCtrl.PlaybackMode == GPSDataPlayer.PlaybackModeEnum.pmEventBased)
                    {
                        d = trackTimeBar.SmallChange * eventBasedSpeed;
                    }
                    else
                    {
                        d = (int) Math.Round (trackTimeBar.SmallChange * this.m_playbackCtrl.TimeBasedPlaybackSpeed);
                    }
                    break;
                case Keys.Down:
                    if (this.m_playbackCtrl.PlaybackMode == GPSDataPlayer.PlaybackModeEnum.pmEventBased)
                    {
                        d = - trackTimeBar.SmallChange * eventBasedSpeed;
                    }
                    else
                    {
                        d = - (int) Math.Round (trackTimeBar.SmallChange * this.m_playbackCtrl.TimeBasedPlaybackSpeed);
                    }
                    break;
                case Keys.PageDown:
                    d = - trackTimeBar.LargeChange;
                    break;
                case Keys.PageUp:
                    d = - trackTimeBar.LargeChange;
                    break;
                case Keys.Home:
                    trackTimeBar.Value = trackTimeBar.Minimum;
                    e.Handled = true;
                    return;
                case Keys.End:
                    trackTimeBar.Value = trackTimeBar.Maximum;
                    e.Handled = true;
                    return;
            }

            if (0 == d) return;

            e.Handled = true;
            if (this.m_playbackCtrl.PlaybackMode == GPSDataPlayer.PlaybackModeEnum.pmEventBased)
            {
                DateTime dt = m_playbackCtrl.CurrentTime; 
                if (d > 0)
                {
                    while (d -- > 0)
                    {
                        dt = this.m_playbackCtrl.Cache.GetNextEvent (dt);
                    }
                }
                else
                {
                    while (d ++ < 0)
                    {
                        dt = this.m_playbackCtrl.Cache.GetPrevEvent (dt);
                    }
                }

                TimeSpan ts = dt - m_playbackCtrl.StartTime;
                this.trackTimeBar.Value = TimeToTrackBarPosition (ts);

            }
            else if (this.m_playbackCtrl.PlaybackMode == GPSDataPlayer.PlaybackModeEnum.pmTimeBased)
            {
                int newVal = trackTimeBar.Value + d;
                if (newVal < trackTimeBar.Minimum) newVal = trackTimeBar.Minimum;
                else if (newVal > trackTimeBar.Maximum) newVal = trackTimeBar.Maximum;
    
                trackTimeBar.Value = newVal;
            }
        }

        #region "Преобразование времени"

        /// <summary>
        /// Форматирует время для отображения в элементе управленияю
        /// </summary>
        /// <param name="dt">Время</param>
        /// <returns></returns>
        public static string FormatDateTime (DateTime dt)
        {
            string result = dt.ToLocalTime ().ToString ("dd.MM.yyyy HH:mm:ss");
            return result;
        }

        /// <summary>
        /// Преобразует интервал времени в позицию TrackBar
        /// </summary>
        /// <param name="dt">Разница во времени между стартовой позицией и 
        /// моментом времени, который преобразуется в позицию.</param>
        /// <returns></returns>
        public static int TimeToTrackBarPosition (TimeSpan dt)
        {
            return (int) dt.TotalSeconds + TrackBarStartPosition;
        }

        /// <summary>
        /// Преобразует позицию TrackBar в интервал времени
        /// </summary>
        /// <param name="pos">Позиция TrackBar</param>
        /// <returns>интервал вреени, соответствующий позиции</returns>
        public static TimeSpan TrackBarPositionToTime (int pos)
        {
            return TimeSpan.FromSeconds (pos - TrackBarStartPosition);
        }

        /// <summary>
        /// Значение стартовой позиции TrackBar
        /// </summary>
        public static int TrackBarStartPosition
        {
            get { return Int32.MinValue; }
        }

        #endregion

        private void appendItem_Click (object sender, EventArgs e)
        {
            this.LoadGPSDataFromFile (true);
        }
    }
}
