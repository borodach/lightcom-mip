using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using GPS.Dispatcher.Cache;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class GPSPlayBackCtrl : System.Windows.Forms.UserControl
	{
		public delegate void GPSDataCacheLoadedHandler (object sender, GPSDataCache cache);
		public event GPSDataCacheLoadedHandler GPSDataCacheLoaded;

		private System.Windows.Forms.Button PlayBtn;
		private System.Windows.Forms.Button ForwardWindBtn;
		private System.Windows.Forms.Button EndBtn;
		private System.Windows.Forms.Button StartBtn;
		private System.Windows.Forms.Label PBPlayPersent;
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
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.ComponentModel.IContainer components;

		public GPSPlayBackCtrl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

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
			this.components = new System.ComponentModel.Container();
			this.PlayBtn = new System.Windows.Forms.Button();
			this.ForwardWindBtn = new System.Windows.Forms.Button();
			this.EndBtn = new System.Windows.Forms.Button();
			this.StartBtn = new System.Windows.Forms.Button();
			this.timeProgressBar = new System.Windows.Forms.ProgressBar();
			this.PBPlayPersent = new System.Windows.Forms.Label();
			this.PBCurrentTime = new System.Windows.Forms.Label();
			this.PBStartDateTime = new System.Windows.Forms.Label();
			this.PBEndDateTime = new System.Windows.Forms.Label();
			this.PlayBackTimer = new System.Windows.Forms.Timer(this.components);
			this.LoadSaveDataBtn = new System.Windows.Forms.Button();
			this.loadSaveGPSDataMenu = new System.Windows.Forms.ContextMenu();
			this.LoadItem = new System.Windows.Forms.MenuItem();
			this.saveItem = new System.Windows.Forms.MenuItem();
			this.openGPSDataDlg = new System.Windows.Forms.OpenFileDialog();
			this.saveGPSDataDlg = new System.Windows.Forms.SaveFileDialog();
			this.trackTimeBar = new System.Windows.Forms.TrackBar();
			this.timeRadioButton = new System.Windows.Forms.RadioButton();
			this.eventRadioButton = new System.Windows.Forms.RadioButton();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.trackTimeBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// PlayBtn
			// 
			this.PlayBtn.Location = new System.Drawing.Point(144, 80);
			this.PlayBtn.Name = "PlayBtn";
			this.PlayBtn.Size = new System.Drawing.Size(72, 24);
			this.PlayBtn.TabIndex = 0;
			this.PlayBtn.Text = ">";
			this.PlayBtn.Click += new System.EventHandler(this.PlayBtn_Click);
			// 
			// ForwardWindBtn
			// 
			this.ForwardWindBtn.Location = new System.Drawing.Point(216, 80);
			this.ForwardWindBtn.Name = "ForwardWindBtn";
			this.ForwardWindBtn.Size = new System.Drawing.Size(32, 24);
			this.ForwardWindBtn.TabIndex = 1;
			this.ForwardWindBtn.Text = ">";
			this.ForwardWindBtn.Click += new System.EventHandler(this.ForwardWindBtn_Click);
			// 
			// EndBtn
			// 
			this.EndBtn.Location = new System.Drawing.Point(248, 80);
			this.EndBtn.Name = "EndBtn";
			this.EndBtn.Size = new System.Drawing.Size(32, 24);
			this.EndBtn.TabIndex = 3;
			this.EndBtn.Text = ">|";
			this.EndBtn.Click += new System.EventHandler(this.EndBtn_Click);
			// 
			// StartBtn
			// 
			this.StartBtn.Location = new System.Drawing.Point(112, 80);
			this.StartBtn.Name = "StartBtn";
			this.StartBtn.Size = new System.Drawing.Size(32, 24);
			this.StartBtn.TabIndex = 4;
			this.StartBtn.Text = "|<";
			this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
			// 
			// timeProgressBar
			// 
			this.timeProgressBar.Location = new System.Drawing.Point(8, 56);
			this.timeProgressBar.Name = "timeProgressBar";
			this.timeProgressBar.Size = new System.Drawing.Size(400, 8);
			this.timeProgressBar.TabIndex = 6;
			// 
			// PBPlayPersent
			// 
			this.PBPlayPersent.BackColor = System.Drawing.Color.Transparent;
			this.PBPlayPersent.Location = new System.Drawing.Point(190, 8);
			this.PBPlayPersent.Name = "PBPlayPersent";
			this.PBPlayPersent.Size = new System.Drawing.Size(40, 16);
			this.PBPlayPersent.TabIndex = 7;
			this.PBPlayPersent.Text = "57%";
			// 
			// PBCurrentTime
			// 
			this.PBCurrentTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.PBCurrentTime.Location = new System.Drawing.Point(120, 24);
			this.PBCurrentTime.Name = "PBCurrentTime";
			this.PBCurrentTime.Size = new System.Drawing.Size(168, 24);
			this.PBCurrentTime.TabIndex = 8;
			this.PBCurrentTime.Text = "14.12.2005 13:51";
			this.PBCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PBStartDateTime
			// 
			this.PBStartDateTime.Location = new System.Drawing.Point(8, 32);
			this.PBStartDateTime.Name = "PBStartDateTime";
			this.PBStartDateTime.Size = new System.Drawing.Size(112, 16);
			this.PBStartDateTime.TabIndex = 9;
			this.PBStartDateTime.Text = "12.12.2005 12:02";
			this.PBStartDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PBEndDateTime
			// 
			this.PBEndDateTime.Location = new System.Drawing.Point(296, 32);
			this.PBEndDateTime.Name = "PBEndDateTime";
			this.PBEndDateTime.Size = new System.Drawing.Size(112, 16);
			this.PBEndDateTime.TabIndex = 10;
			this.PBEndDateTime.Text = "16.12.2005 14:45";
			this.PBEndDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PlayBackTimer
			// 
			this.PlayBackTimer.Tick += new System.EventHandler(this.PlayBackTimer_Tick);
			// 
			// LoadSaveDataBtn
			// 
			this.LoadSaveDataBtn.Location = new System.Drawing.Point(384, 80);
			this.LoadSaveDataBtn.Name = "LoadSaveDataBtn";
			this.LoadSaveDataBtn.Size = new System.Drawing.Size(24, 24);
			this.LoadSaveDataBtn.TabIndex = 12;
			this.LoadSaveDataBtn.Click += new System.EventHandler(this.LoadSaveDataBtn_Click);
			// 
			// loadSaveGPSDataMenu
			// 
			this.loadSaveGPSDataMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.LoadItem,
																								this.saveItem});
			// 
			// LoadItem
			// 
			this.LoadItem.Index = 0;
			this.LoadItem.Text = "Загрузить из файла...";
			this.LoadItem.Click += new System.EventHandler(this.LoadItem_Click);
			// 
			// saveItem
			// 
			this.saveItem.Index = 1;
			this.saveItem.Text = "Сохранить в файл...";
			this.saveItem.Click += new System.EventHandler(this.saveItem_Click);
			// 
			// openGPSDataDlg
			// 
			this.openGPSDataDlg.DefaultExt = "gps";
			this.openGPSDataDlg.FileName = "data";
			this.openGPSDataDlg.Filter = "Файлы с данными GPS|*.gps|Все файлы|*.*";
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
			this.trackTimeBar.Location = new System.Drawing.Point(8, 48);
			this.trackTimeBar.Name = "trackTimeBar";
			this.trackTimeBar.Size = new System.Drawing.Size(400, 16);
			this.trackTimeBar.TabIndex = 13;
			this.trackTimeBar.TickStyle = System.Windows.Forms.TickStyle.None;
			this.trackTimeBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackTimeBar_MouseUp);
			this.trackTimeBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trackTimeBar_MouseDown);
			this.trackTimeBar.Scroll += new System.EventHandler(this.trackTimeBar_Scroll);
			// 
			// timeRadioButton
			// 
			this.timeRadioButton.Location = new System.Drawing.Point(8, 88);
			this.timeRadioButton.Name = "timeRadioButton";
			this.timeRadioButton.TabIndex = 14;
			this.timeRadioButton.Text = "по времени";
			this.timeRadioButton.CheckedChanged += new System.EventHandler(this.timeRadioButton_CheckedChanged);
			// 
			// eventRadioButton
			// 
			this.eventRadioButton.Location = new System.Drawing.Point(8, 69);
			this.eventRadioButton.Name = "eventRadioButton";
			this.eventRadioButton.TabIndex = 15;
			this.eventRadioButton.Text = "по событию";
			this.eventRadioButton.CheckedChanged += new System.EventHandler(this.eventRadioButton_CheckedChanged);
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(296, 82);
			this.numericUpDown1.Maximum = new System.Decimal(new int[] {
																		   1000,
																		   0,
																		   0,
																		   0});
			this.numericUpDown1.Minimum = new System.Decimal(new int[] {
																		   1,
																		   0,
																		   0,
																		   0});
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(72, 20);
			this.numericUpDown1.TabIndex = 16;
			this.numericUpDown1.Value = new System.Decimal(new int[] {
																		 1,
																		 0,
																		 0,
																		 0});
			this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(302, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 17;
			this.label1.Text = "скорость:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 16);
			this.label2.TabIndex = 18;
			this.label2.Text = "начало:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(312, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 19;
			this.label3.Text = "конец:";
			// 
			// GPSPlayBackCtrl
			// 
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.eventRadioButton);
			this.Controls.Add(this.timeRadioButton);
			this.Controls.Add(this.trackTimeBar);
			this.Controls.Add(this.LoadSaveDataBtn);
			this.Controls.Add(this.PBEndDateTime);
			this.Controls.Add(this.PBStartDateTime);
			this.Controls.Add(this.PBCurrentTime);
			this.Controls.Add(this.PBPlayPersent);
			this.Controls.Add(this.timeProgressBar);
			this.Controls.Add(this.StartBtn);
			this.Controls.Add(this.EndBtn);
			this.Controls.Add(this.ForwardWindBtn);
			this.Controls.Add(this.PlayBtn);
			this.Name = "GPSPlayBackCtrl";
			this.Size = new System.Drawing.Size(416, 112);
			((System.ComponentModel.ISupportInitialize)(this.trackTimeBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

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
				PlayBtn.Text = ">";
			}
			else
			{
				PlayBtn.Text = "Stop";
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
				ForwardWindBtn.Text = ">";
			}
			else
			{
				ForwardWindBtn.Text = "<";
			}
		}

		private void StartBtn_Click(object sender, System.EventArgs e)
		{
			this.PBPlayPersent.Text = "0%";
			this.timeProgressBar.Value = this.timeProgressBar.Minimum;
			m_playbackCtrl.MoveToStart();
		}

		private void EndBtn_Click(object sender, System.EventArgs e)
		{
			this.PBPlayPersent.Text = "100%";
			this.timeProgressBar.Value = this.timeProgressBar.Maximum;
			m_playbackCtrl.MoveToFinish();
		}

		/// <summary>
		/// внутренний обработчик события от GPSDataPlayer для изменения внешнего вида контрола
		/// </summary>
		/// <param name="sourse"></param>
		/// <param name="oldTime"></param>
		/// <param name="newEvent"></param>
		private void TimerChange(PlaybackControl sourse, DateTime oldTime, DateTime newEvent)
		{
			TimeSpan diff = m_playbackCtrl.CurrentTime - m_playbackCtrl.StartTime;
			TimeSpan diffTotal = m_playbackCtrl.FinishTime - m_playbackCtrl.StartTime;
			this.PBCurrentTime.Text = m_playbackCtrl.CurrentTime.ToShortDateString() + " " + m_playbackCtrl.CurrentTime.ToShortTimeString();
//			this.timeProgressBar.Value = (int)diff.TotalMinutes;
			this.trackTimeBar.Value = (int)diff.TotalMinutes;
			double persent = (diff.TotalMilliseconds / diffTotal.TotalMilliseconds) * 100;
			this.PBPlayPersent.Text = (int)persent + "%";
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
		private void InitCtrlFromCash()
		{
			try
			{
				TimeSpan diff = m_playbackCtrl.FinishTime - m_playbackCtrl.StartTime;
				this.PBStartDateTime.Text = m_playbackCtrl.StartTime.ToShortDateString() + " " + m_playbackCtrl.StartTime.ToShortTimeString();
				this.PBEndDateTime.Text = m_playbackCtrl.FinishTime.ToShortDateString() + " " + m_playbackCtrl.FinishTime.ToShortTimeString();
				this.PBCurrentTime.Text = m_playbackCtrl.CurrentTime.ToShortDateString() + " " + m_playbackCtrl.CurrentTime.ToShortTimeString();
				this.PBPlayPersent.Text = "0%";
//				this.timeProgressBar.Minimum = 0;
//				this.timeProgressBar.Maximum = (int)diff.TotalMinutes;
//				this.timeProgressBar.Step = 1;
//				this.timeProgressBar.Value = this.timeProgressBar.Minimum;
				this.trackTimeBar.Minimum = 0;
				this.trackTimeBar.Maximum = (int)diff.TotalMinutes;
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
			LoadGPSDataFromFile();
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

				if (! PlayerCash.SaveGuts (fs))
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

		public void LoadGPSDataFromFile()
		{
			if (DialogResult.OK != openGPSDataDlg.ShowDialog ())
			{
				return;
			}

			string strFileName = openGPSDataDlg.FileName;
			try
			{
				FileStream fs = new FileStream (strFileName, 
					FileMode.Open, 
					FileAccess.Read);
				GPSDataCache cache = new GPSDataCache ();
				if (! cache.RestoreGuts (fs))
				{
					MessageBox.Show ("Не удалось загрузить данные, возможно, файл поврежден.", 
						"Ошибка.");
				}
				else
				{
					PlayerCash = cache;

					if (null != GPSDataCacheLoaded)
					{
						GPSDataCacheLoaded(this, cache);
					}

					MessageBox.Show ("Данные успешно загружены.", "Информация.");
				}
				fs.Close ();
			}
			catch (Exception ex)
			{
				MessageBox.Show (ex.Message, "Ошибка загрузки.");
			}
		}

		private void LoadSaveDataBtn_Click(object sender, System.EventArgs e)
		{
			Point clientPnt = this.PointToClient(Cursor.Position);
			loadSaveGPSDataMenu.Show(this, clientPnt);
		}

		private void trackTimeBar_Scroll(object sender, System.EventArgs e)
		{
			if (m_isScrollTimeBar)
			{
				DateTime dt = m_playbackCtrl.StartTime;
				m_playbackCtrl.MoveToTime(dt.AddMinutes((double)trackTimeBar.Value));
			}
		}

		private void trackTimeBar_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_isScrollTimeBar = true;
			m_playbackCtrl.Stopped = true;
			PlayBtnStateChange(m_playbackCtrl.Stopped);
		}

		private void trackTimeBar_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			m_isScrollTimeBar = false;
			m_playbackCtrl.Stopped = false;
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
		public GPS.Dispatcher.Cache.GPSDataCache PlayerCash
		{
			get{return m_playbackCtrl.Cache;}
			set
			{
				m_playbackCtrl.Cache = value;
				InitCtrlFromCash();
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
	}
}
