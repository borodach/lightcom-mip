///////////////////////////////////////////////////////////////////////////////
//
//  File:           MainForm.Designer.cs
//
//  Facility:       
//
//
//  Abstract:
//
//  Environment:    VC 7.1
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  31-10-2005
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: MainForm.Designer.cs $                         
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 10.07.07   Time: 8:02
 * Created in $/LightCom/.NET/MiP/CEClient
 * Реализована поддержка Windows Mobile GPS API
 */

namespace LightCom.MiP.CEClient
{
    /// <summary>
    /// Summary description for MainForm.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox gpsState;
        private System.Windows.Forms.PictureBox httpState;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Test;
        private System.Windows.Forms.TabPage Map;
        private System.Windows.Forms.PictureBox mapBox;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox positionInfo;
        private System.Windows.Forms.ContextMenu mapMenu;
        private System.Windows.Forms.MenuItem menuItemZoom200;
        private System.Windows.Forms.MenuItem menuItemZoom100;
        private System.Windows.Forms.MenuItem menuItemZoom75;
        private System.Windows.Forms.MenuItem menuItemZoom50;
        private System.Windows.Forms.MenuItem menuItemZoom25;
        private System.Windows.Forms.MenuItem menuItemZoom150;
        private System.Windows.Forms.MenuItem menuItemZoom300;
        private System.Windows.Forms.Label publisherLabel;
      
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose ( bool disposing )
        {
            base.Dispose ( disposing );
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MainForm));
            this.StartBtn = new System.Windows.Forms.Button ();
            this.label1 = new System.Windows.Forms.Label ();
            this.gpsState = new System.Windows.Forms.PictureBox ();
            this.httpState = new System.Windows.Forms.PictureBox ();
            this.publisherLabel = new System.Windows.Forms.Label ();
            this.tabControl1 = new System.Windows.Forms.TabControl ();
            this.Test = new System.Windows.Forms.TabPage ();
            this.positionInfo = new System.Windows.Forms.TextBox ();
            this.button1 = new System.Windows.Forms.Button ();
            this.button5 = new System.Windows.Forms.Button ();
            this.Map = new System.Windows.Forms.TabPage ();
            this.mapBox = new System.Windows.Forms.PictureBox ();
            this.mapMenu = new System.Windows.Forms.ContextMenu ();
            this.menuItemZoom300 = new System.Windows.Forms.MenuItem ();
            this.menuItemZoom200 = new System.Windows.Forms.MenuItem ();
            this.menuItemZoom150 = new System.Windows.Forms.MenuItem ();
            this.menuItemZoom100 = new System.Windows.Forms.MenuItem ();
            this.menuItemZoom75 = new System.Windows.Forms.MenuItem ();
            this.menuItemZoom50 = new System.Windows.Forms.MenuItem ();
            this.menuItemZoom25 = new System.Windows.Forms.MenuItem ();
            this.tabControl1.SuspendLayout ();
            this.Test.SuspendLayout ();
            this.Map.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // StartBtn
            // 
            this.StartBtn.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StartBtn.Location = new System.Drawing.Point (3, 173);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size (84, 19);
            this.StartBtn.TabIndex = 1;
            this.StartBtn.Text = "Start";
            this.StartBtn.Click += new System.EventHandler (this.OnStartStop);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular);
            this.label1.Location = new System.Drawing.Point (3, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size (30, 16);
            this.label1.Text = "GPS:";
            // 
            // gpsState
            // 
            this.gpsState.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gpsState.Location = new System.Drawing.Point (47, 153);
            this.gpsState.Name = "gpsState";
            this.gpsState.Size = new System.Drawing.Size (40, 16);
            // 
            // httpState
            // 
            this.httpState.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.httpState.Location = new System.Drawing.Point (152, 153);
            this.httpState.Name = "httpState";
            this.httpState.Size = new System.Drawing.Size (40, 16);
            // 
            // publisherLabel
            // 
            this.publisherLabel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.publisherLabel.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular);
            this.publisherLabel.Location = new System.Drawing.Point (104, 153);
            this.publisherLabel.Name = "publisherLabel";
            this.publisherLabel.Size = new System.Drawing.Size (37, 16);
            this.publisherLabel.Text = "HTTP:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add (this.Test);
            this.tabControl1.Controls.Add (this.Map);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular);
            this.tabControl1.Location = new System.Drawing.Point (0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size (232, 221);
            this.tabControl1.TabIndex = 0;
            // 
            // Test
            // 
            this.Test.Controls.Add (this.positionInfo);
            this.Test.Controls.Add (this.StartBtn);
            this.Test.Controls.Add (this.publisherLabel);
            this.Test.Controls.Add (this.httpState);
            this.Test.Controls.Add (this.gpsState);
            this.Test.Controls.Add (this.label1);
            this.Test.Controls.Add (this.button1);
            this.Test.Controls.Add (this.button5);
            this.Test.Location = new System.Drawing.Point (4, 22);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size (224, 195);
            this.Test.Text = "Test";
            // 
            // positionInfo
            // 
            this.positionInfo.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positionInfo.Font = new System.Drawing.Font ("Arial", 10F, System.Drawing.FontStyle.Regular);
            this.positionInfo.HideSelection = false;
            this.positionInfo.Location = new System.Drawing.Point (3, 6);
            this.positionInfo.Multiline = true;
            this.positionInfo.Name = "positionInfo";
            this.positionInfo.ReadOnly = true;
            this.positionInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.positionInfo.Size = new System.Drawing.Size (217, 141);
            this.positionInfo.TabIndex = 13;
            this.positionInfo.Text = "Нет данных";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point (152, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size (68, 19);
            this.button1.TabIndex = 12;
            this.button1.Text = "No Auto";
            this.button1.Click += new System.EventHandler (this.OnNoAuto);
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.Location = new System.Drawing.Point (98, 173);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size (43, 19);
            this.button5.TabIndex = 12;
            this.button5.Text = "Save";
            this.button5.Click += new System.EventHandler (this.button5_Click);
            // 
            // Map
            // 
            this.Map.Controls.Add (this.mapBox);
            this.Map.Location = new System.Drawing.Point (4, 22);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size (224, 195);
            this.Map.Text = "Map";
            // 
            // mapBox
            // 
            this.mapBox.ContextMenu = this.mapMenu;
            this.mapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox.Location = new System.Drawing.Point (0, 0);
            this.mapBox.Name = "mapBox";
            this.mapBox.Size = new System.Drawing.Size (224, 195);
            // 
            // mapMenu
            // 
            this.mapMenu.MenuItems.Add (this.menuItemZoom300);
            this.mapMenu.MenuItems.Add (this.menuItemZoom200);
            this.mapMenu.MenuItems.Add (this.menuItemZoom150);
            this.mapMenu.MenuItems.Add (this.menuItemZoom100);
            this.mapMenu.MenuItems.Add (this.menuItemZoom75);
            this.mapMenu.MenuItems.Add (this.menuItemZoom50);
            this.mapMenu.MenuItems.Add (this.menuItemZoom25);
            // 
            // menuItemZoom300
            // 
            this.menuItemZoom300.Text = "300%";
            this.menuItemZoom300.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // menuItemZoom200
            // 
            this.menuItemZoom200.Text = "200%";
            this.menuItemZoom200.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // menuItemZoom150
            // 
            this.menuItemZoom150.Text = "150%";
            this.menuItemZoom150.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // menuItemZoom100
            // 
            this.menuItemZoom100.Checked = true;
            this.menuItemZoom100.Text = "100%";
            this.menuItemZoom100.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // menuItemZoom75
            // 
            this.menuItemZoom75.Text = "75%";
            this.menuItemZoom75.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // menuItemZoom50
            // 
            this.menuItemZoom50.Text = "50%";
            this.menuItemZoom50.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // menuItemZoom25
            // 
            this.menuItemZoom25.Text = "25%";
            this.menuItemZoom25.Click += new System.EventHandler (this.OnMapMenu);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size (232, 221);
            this.Controls.Add (this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "МиП Клиент";
            this.Closed += new System.EventHandler (this.MainForm_Closed);
            this.tabControl1.ResumeLayout (false);
            this.Test.ResumeLayout (false);
            this.Map.ResumeLayout (false);
            this.ResumeLayout (false);

        }
        #endregion
    }    
}
