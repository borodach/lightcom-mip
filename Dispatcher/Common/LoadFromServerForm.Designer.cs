namespace LightCom.MiP.Dispatcher.Common
{
    partial class LoadFromServerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (LoadFromServerForm));
            this.readButton = new System.Windows.Forms.Button ();
            this.label13 = new System.Windows.Forms.Label ();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker ();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker ();
            this.label12 = new System.Windows.Forms.Label ();
            this.label1 = new System.Windows.Forms.Label ();
            this.label11 = new System.Windows.Forms.Label ();
            this.label10 = new System.Windows.Forms.Label ();
            this.label9 = new System.Windows.Forms.Label ();
            this.label8 = new System.Windows.Forms.Label ();
            this.passwordTextBox = new System.Windows.Forms.TextBox ();
            this.domainTextBox = new System.Windows.Forms.TextBox ();
            this.userTextBox = new System.Windows.Forms.TextBox ();
            this.URLTextBox = new System.Windows.Forms.TextBox ();
            this.connectionButton = new System.Windows.Forms.Button ();
            this.clientList = new System.Windows.Forms.ListBox ();
            this.mainTabControl = new System.Windows.Forms.TabControl ();
            this.tabConnect = new System.Windows.Forms.TabPage ();
            this.btnCancel1 = new System.Windows.Forms.Button ();
            this.tabGetData = new System.Windows.Forms.TabPage ();
            this.btnCancel2 = new System.Windows.Forms.Button ();
            this.mainTabControl.SuspendLayout ();
            this.tabConnect.SuspendLayout ();
            this.tabGetData.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // readButton
            // 
            this.readButton.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.readButton.Enabled = false;
            this.readButton.Location = new System.Drawing.Point (123, 116);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size (102, 23);
            this.readButton.TabIndex = 3;
            this.readButton.Text = "Загрузить";
            this.readButton.Click += new System.EventHandler (this.readButton_Click);
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point (0, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size (120, 16);
            this.label13.TabIndex = 45;
            this.label13.Text = "Диапазон времени:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "dd.MM.yyyy hh:mm:ss";
            this.dateTimePicker2.Enabled = false;
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point (283, 7);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size (135, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd.MM.yyyy hh:mm:ss";
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point (123, 7);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size (135, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point (0, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size (100, 14);
            this.label12.TabIndex = 42;
            this.label12.Text = "Объекты:";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point (262, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size (17, 16);
            this.label1.TabIndex = 45;
            this.label1.Text = "-";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point (0, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size (100, 14);
            this.label11.TabIndex = 54;
            this.label11.Text = "Пароль:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point (0, 57);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size (100, 14);
            this.label10.TabIndex = 53;
            this.label10.Text = "Домен:";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point (0, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size (112, 14);
            this.label9.TabIndex = 52;
            this.label9.Text = "Имя пользователя:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point (0, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size (100, 14);
            this.label8.TabIndex = 51;
            this.label8.Text = "Сервер:";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point (123, 78);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size (303, 20);
            this.passwordTextBox.TabIndex = 3;
            this.passwordTextBox.Text = "password";
            // 
            // domainTextBox
            // 
            this.domainTextBox.Location = new System.Drawing.Point (123, 54);
            this.domainTextBox.Name = "domainTextBox";
            this.domainTextBox.Size = new System.Drawing.Size (303, 20);
            this.domainTextBox.TabIndex = 2;
            this.domainTextBox.Text = "Administration";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point (123, 30);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size (303, 20);
            this.userTextBox.TabIndex = 1;
            this.userTextBox.Text = "admin";
            // 
            // URLTextBox
            // 
            this.URLTextBox.Location = new System.Drawing.Point (123, 6);
            this.URLTextBox.Name = "URLTextBox";
            this.URLTextBox.Size = new System.Drawing.Size (303, 20);
            this.URLTextBox.TabIndex = 6;
            this.URLTextBox.Text = "http://kud.unicon.ru/new/dgate.php";
            // 
            // connectionButton
            // 
            this.connectionButton.Location = new System.Drawing.Point (123, 116);
            this.connectionButton.Name = "connectionButton";
            this.connectionButton.Size = new System.Drawing.Size (102, 23);
            this.connectionButton.TabIndex = 4;
            this.connectionButton.Text = "Соединиться";
            this.connectionButton.Click += new System.EventHandler (this.connectionButton_Click);
            // 
            // clientList
            // 
            this.clientList.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.clientList.FormattingEnabled = true;
            this.clientList.Location = new System.Drawing.Point (123, 33);
            this.clientList.Name = "clientList";
            this.clientList.Size = new System.Drawing.Size (303, 69);
            this.clientList.TabIndex = 2;
            this.clientList.SelectedIndexChanged += new System.EventHandler (this.clientIdComboBox_SelectedIndexChanged);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add (this.tabConnect);
            this.mainTabControl.Controls.Add (this.tabGetData);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point (0, 0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size (440, 170);
            this.mainTabControl.TabIndex = 0;
            // 
            // tabConnect
            // 
            this.tabConnect.Controls.Add (this.label11);
            this.tabConnect.Controls.Add (this.URLTextBox);
            this.tabConnect.Controls.Add (this.label10);
            this.tabConnect.Controls.Add (this.btnCancel1);
            this.tabConnect.Controls.Add (this.connectionButton);
            this.tabConnect.Controls.Add (this.label9);
            this.tabConnect.Controls.Add (this.userTextBox);
            this.tabConnect.Controls.Add (this.label8);
            this.tabConnect.Controls.Add (this.domainTextBox);
            this.tabConnect.Controls.Add (this.passwordTextBox);
            this.tabConnect.Location = new System.Drawing.Point (4, 22);
            this.tabConnect.Name = "tabConnect";
            this.tabConnect.Padding = new System.Windows.Forms.Padding (3);
            this.tabConnect.Size = new System.Drawing.Size (432, 144);
            this.tabConnect.TabIndex = 0;
            this.tabConnect.Text = "Соединение";
            this.tabConnect.UseVisualStyleBackColor = true;
            // 
            // btnCancel1
            // 
            this.btnCancel1.Location = new System.Drawing.Point (324, 116);
            this.btnCancel1.Name = "btnCancel1";
            this.btnCancel1.Size = new System.Drawing.Size (102, 23);
            this.btnCancel1.TabIndex = 5;
            this.btnCancel1.Text = "Отмена";
            this.btnCancel1.Click += new System.EventHandler (this.OnCancelButton);
            // 
            // tabGetData
            // 
            this.tabGetData.Controls.Add (this.clientList);
            this.tabGetData.Controls.Add (this.label13);
            this.tabGetData.Controls.Add (this.label12);
            this.tabGetData.Controls.Add (this.dateTimePicker2);
            this.tabGetData.Controls.Add (this.btnCancel2);
            this.tabGetData.Controls.Add (this.readButton);
            this.tabGetData.Controls.Add (this.label1);
            this.tabGetData.Controls.Add (this.dateTimePicker1);
            this.tabGetData.Location = new System.Drawing.Point (4, 22);
            this.tabGetData.Name = "tabGetData";
            this.tabGetData.Padding = new System.Windows.Forms.Padding (3);
            this.tabGetData.Size = new System.Drawing.Size (432, 144);
            this.tabGetData.TabIndex = 1;
            this.tabGetData.Text = "Получение данных";
            this.tabGetData.UseVisualStyleBackColor = true;
            // 
            // btnCancel2
            // 
            this.btnCancel2.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel2.Location = new System.Drawing.Point (324, 116);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size (102, 23);
            this.btnCancel2.TabIndex = 4;
            this.btnCancel2.Text = "Отмена";
            this.btnCancel2.Click += new System.EventHandler (this.OnCancelButton);
            // 
            // LoadFromServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size (440, 170);
            this.Controls.Add (this.mainTabControl);
            this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
            this.Name = "LoadFromServerForm";
            this.Text = "Загрузка данных с сервера";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler (this.OnClose);
            this.mainTabControl.ResumeLayout (false);
            this.tabConnect.ResumeLayout (false);
            this.tabConnect.PerformLayout ();
            this.tabGetData.ResumeLayout (false);
            this.ResumeLayout (false);

        }

        #endregion

        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox domainTextBox;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.TextBox URLTextBox;
        private System.Windows.Forms.Button connectionButton;
        private System.Windows.Forms.ListBox clientList;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage tabConnect;
        private System.Windows.Forms.TabPage tabGetData;
        private System.Windows.Forms.Button btnCancel1;
        private System.Windows.Forms.Button btnCancel2;
    }
}