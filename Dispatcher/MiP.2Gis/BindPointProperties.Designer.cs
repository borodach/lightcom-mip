namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    partial class BindPointProperties
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
            this.components = new System.ComponentModel.Container ();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (BindPointProperties));
            this.label1 = new System.Windows.Forms.Label ();
            this.txtDescription = new System.Windows.Forms.TextBox ();
            this.label2 = new System.Windows.Forms.Label ();
            this.label3 = new System.Windows.Forms.Label ();
            this.toolTip1 = new System.Windows.Forms.ToolTip (this.components);
            this.txtLatitude = new System.Windows.Forms.TextBox ();
            this.txtLongitude = new System.Windows.Forms.TextBox ();
            this.btnOK = new System.Windows.Forms.Button ();
            this.btnCancel = new System.Windows.Forms.Button ();
            this.SuspendLayout ();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point (12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size (60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Описание:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point (89, 8);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size (156, 20);
            this.txtDescription.TabIndex = 1;
            this.toolTip1.SetToolTip (this.txtDescription, "Описание точки привязки.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point (12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size (48, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Широта:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point (12, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size (53, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Долгота:";
            // 
            // txtLatitude
            // 
            this.txtLatitude.Location = new System.Drawing.Point (89, 34);
            this.txtLatitude.Name = "txtLatitude";
            this.txtLatitude.Size = new System.Drawing.Size (155, 20);
            this.txtLatitude.TabIndex = 2;
            this.txtLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip (this.txtLatitude, "Широта (Latitude) точки привязки. Должна быть задано в градусах.");
            // 
            // txtLongitude
            // 
            this.txtLongitude.Location = new System.Drawing.Point (90, 60);
            this.txtLongitude.Name = "txtLongitude";
            this.txtLongitude.Size = new System.Drawing.Size (155, 20);
            this.txtLongitude.TabIndex = 3;
            this.txtLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip (this.txtLongitude, "Долгота (Longitude) точки привязки. Должна быть задано в градусах.");
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point (16, 97);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size (75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler (this.OnOK);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point (169, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size (75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler (this.OnCancel);
            // 
            // BindPointProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size (258, 132);
            this.Controls.Add (this.txtLongitude);
            this.Controls.Add (this.txtLatitude);
            this.Controls.Add (this.btnCancel);
            this.Controls.Add (this.btnOK);
            this.Controls.Add (this.txtDescription);
            this.Controls.Add (this.label3);
            this.Controls.Add (this.label2);
            this.Controls.Add (this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon) (resources.GetObject ("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "BindPointProperties";
            this.Text = "Свойства точки привязки";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler (this.OnKeyDown);
            this.ResumeLayout (false);
            this.PerformLayout ();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtLatitude;
        private System.Windows.Forms.TextBox txtLongitude;
    }
}