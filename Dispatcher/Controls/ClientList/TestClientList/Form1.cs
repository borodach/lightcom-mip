using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using GPS.Dispatcher.Controls;
using GPS.Common;

namespace TestClientList
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private ClientList clientList1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
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
			this.clientList1 = new GPS.Dispatcher.Controls.ClientList();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// clientList1
			// 
			this.clientList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.clientList1.AutoLoadListsFromFile = false;
			this.clientList1.FileDisplayClients = "D:\\VC#Projects\\ClientList\\Clients.dcl";
			this.clientList1.FileMobileClients = "D:\\VC#Projects\\ClientList\\Clients.mcl";
			this.clientList1.Location = new System.Drawing.Point(24, 8);
			this.clientList1.Name = "clientList1";
			this.clientList1.Size = new System.Drawing.Size(248, 280);
			this.clientList1.TabIndex = 0;
			this.clientList1.Load += new System.EventHandler(this.clientList1_Load);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(192, 296);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(104, 296);
			this.button2.Name = "button2";
			this.button2.TabIndex = 2;
			this.button2.Text = "button2";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(32, 296);
			this.button3.Name = "button3";
			this.button3.TabIndex = 3;
			this.button3.Text = "button3";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 326);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.clientList1);
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

		private void clientList1_Load(object sender, System.EventArgs e)
		{
		
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			MobileClientInfo mci = new MobileClientInfo();
			mci.ClientId = "10";
			mci.FriendlyName = "Первый клиент";
			mci.Comments = "Тестируем клиента №10";
			mci.Company = "\"ООО ЛайтКом\"";
			mci.LastEvent = DateTime.Now;

			clientList1.Add(mci);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if(clientList1.SaveClientDataToFile())
			{
				MessageBox.Show("Данные успешно сохранены!");
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			clientList1.LoadClientDataFromFile();
		}
	}
}
