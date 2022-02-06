using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GPS.Dispatcher.Common
{
	/// <summary>
	/// Summary description for PropertyDialog.
	/// </summary>
	public class PropertyDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox propertiesGroup;
		private System.Windows.Forms.TreeView objectsTree;
		private System.Windows.Forms.Button okBtn;
		private System.Windows.Forms.Button cancelBtn;
		private System.Windows.Forms.Button applyBtn;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PropertyDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_iproperties = new Hashtable();
			m_propertyPanels = new Hashtable();
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
				m_iproperties.Clear();
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.objectsTree = new System.Windows.Forms.TreeView();
			this.propertiesGroup = new System.Windows.Forms.GroupBox();
			this.okBtn = new System.Windows.Forms.Button();
			this.cancelBtn = new System.Windows.Forms.Button();
			this.applyBtn = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.objectsTree);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 464);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Объекты";
			// 
			// objectsTree
			// 
			this.objectsTree.HideSelection = false;
			this.objectsTree.ImageIndex = -1;
			this.objectsTree.Location = new System.Drawing.Point(8, 16);
			this.objectsTree.Name = "objectsTree";
			this.objectsTree.SelectedImageIndex = -1;
			this.objectsTree.Size = new System.Drawing.Size(184, 440);
			this.objectsTree.TabIndex = 0;
			this.objectsTree.Click += new System.EventHandler(this.objectsTree_Click);
			this.objectsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.objectsTree_AfterSelect);
			// 
			// propertiesGroup
			// 
			this.propertiesGroup.Location = new System.Drawing.Point(216, 8);
			this.propertiesGroup.Name = "propertiesGroup";
			this.propertiesGroup.Size = new System.Drawing.Size(408, 464);
			this.propertiesGroup.TabIndex = 1;
			this.propertiesGroup.TabStop = false;
			this.propertiesGroup.Text = "Свойства";
			// 
			// okBtn
			// 
			this.okBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okBtn.Location = new System.Drawing.Point(344, 480);
			this.okBtn.Name = "okBtn";
			this.okBtn.TabIndex = 2;
			this.okBtn.Text = "ОК";
			this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
			// 
			// cancelBtn
			// 
			this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelBtn.Location = new System.Drawing.Point(424, 480);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.TabIndex = 3;
			this.cancelBtn.Text = "Отмена";
			// 
			// applyBtn
			// 
			this.applyBtn.Location = new System.Drawing.Point(536, 480);
			this.applyBtn.Name = "applyBtn";
			this.applyBtn.TabIndex = 4;
			this.applyBtn.Text = "Применить";
			this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
			// 
			// PropertyDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(632, 510);
			this.Controls.Add(this.applyBtn);
			this.Controls.Add(this.cancelBtn);
			this.Controls.Add(this.okBtn);
			this.Controls.Add(this.propertiesGroup);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PropertyDialog";
			this.ShowInTaskbar = false;
			this.Text = "Свойства";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void AddProperty(IProperties properties)
		{
			if (null == properties)
			{
				return;
			}

			ObjectInfo obj = properties.GetObjectInfo();
			
			m_iproperties.Add(obj.Name, properties);

			TreeNode root = new TreeNode();
			root.Tag = obj.BaseProperties;
			root.Text = obj.Text;

			AddPropertyGroupToTree(obj.PropertyGroups, root);
			objectsTree.Nodes.Add(root);
		}

		private void AddPropertyGroupToTree(ArrayList propertyGroup, TreeNode ownerNode)
		{
			PropertyGroup group;
			TreeNode node;
			for(int i = 0; i < propertyGroup.Count; i++)
			{
				group = (PropertyGroup)propertyGroup[i];
				node = new TreeNode();
				node.Text = group.Text;
				node.Tag = group;

				if (0 != group.Count) 
				{
					AddPropertyGroupToTree(group.SubGroups, node);
				}

				ownerNode.Nodes.Add(node);
			}
		}

		private void objectsTree_Click(object sender, System.EventArgs e)
		{

		}

		private void objectsTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode rootNode = e.Node;

			while(null != rootNode.Parent)
			{
				rootNode = rootNode.Parent;
			}

			IProperties prop = (IProperties)m_iproperties[((PropertyGroup)rootNode.Tag).Name];

			if (null != prop)
			{
				AddPropertyPanel(prop, (PropertyGroup)e.Node.Tag);
			}
		}

		private void AddPropertyPanel(IProperties properties, PropertyGroup group)
		{
			PropertyPanel pnl = (PropertyPanel)m_propertyPanels[group.Name];

			if (null != m_currenPropertyPanel)
			{
				m_currenPropertyPanel.Visible = false;
			}

			if (null == pnl)
			{
				pnl = new PropertyPanel();
				pnl.Bounds = new Rectangle(propertiesGroup.ClientRectangle.Left + 5, propertiesGroup.ClientRectangle.Top + 12,
					propertiesGroup.ClientRectangle.Width - 10, propertiesGroup.ClientRectangle.Height - 17);
				pnl.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

				pnl.PropertyList = group;
				pnl.InitializeControls();

				propertiesGroup.Controls.Add(pnl);
				m_propertyPanels.Add(group.Name, pnl);
			}

			m_currenPropertyPanel = pnl;
			m_currenPropertyPanel.Visible = true;
		}

		//Variables
		private Hashtable m_iproperties;
		private Hashtable m_propertyPanels;
		private PropertyPanel m_currenPropertyPanel;

		private void applyBtn_Click(object sender, System.EventArgs e)
		{
			SaveProperties();
		}

		private void SaveProperties()
		{
			IDictionaryEnumerator enumerator = m_propertyPanels.GetEnumerator();

			while (enumerator.MoveNext())
			{
				PropertyPanel pnl = (PropertyPanel)enumerator.Value;
				
				if (null != pnl)
				{
					IProperties prop = (IProperties)m_iproperties[pnl.PropertyList.BaseObject.Name];

					if (null != prop)
					{
						prop.SetProppertyList(pnl.PropertyList);
					}
				}
			}
		}

		private void okBtn_Click(object sender, System.EventArgs e)
		{
			SaveProperties();
		}
	}
}
