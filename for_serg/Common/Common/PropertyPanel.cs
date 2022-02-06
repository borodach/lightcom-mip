/********************************************************************
	created:	2006/02/09
	created:	9:1:2006   16:28
	filename: 	PropertyPanel.cs
	file base:	PropertyPanel
	file ext:	cs
	author:		К.С. Дураков
	
	purpose:	Класс является расширением класса Panel, с возможностью 
				динамически формировать элементы управления для управлениями
				свойств объектов, а так же обрабатывать сообщения от
				созданных элементов, для последующего их сохранения.
*********************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace GPS.Dispatcher.Common
{
	/// <summary>
	/// Summary description for PropertyPanel.
	/// </summary>
	public class PropertyPanel : Panel
	{
		public PropertyPanel()
		{
			m_currentLocationCtrl = new Point(m_offsetLeft, m_offsetTop);
		}

		public void InitializeControls()
		{
			if (null == PropertyList)
				return;
			
			for(int i = 0; i < PropertyList.Properties.Count; i++)
			{
				CreatePropertyControl(this, (PropertyInfo)PropertyList.Properties[i]);
			}
		}

		#region Динамическое создание элементов управления (Не оптимизированно).

		private void InitializeControl(Control ctrl, string name, string text, Point location, AnchorStyles anchor, 
										Rectangle bounds, DockStyle dock)
		{
			ctrl.Name = name;
			ctrl.Text = text;
			ctrl.Location = location;
			ctrl.Anchor = anchor;
			ctrl.Dock = dock;

			if (!bounds.IsEmpty)
				ctrl.Bounds = bounds;

		}

		private Label CreateLabel(string name, string text, Point location, AnchorStyles anchor, bool autoSize, 
								Rectangle bounds, DockStyle dock)
		{
			Label lbl = new Label();
			InitializeControl(lbl, name, text, location, anchor, bounds, dock);
			lbl.AutoSize = autoSize;

			return lbl;
		}

		/// <summary>
		/// динамически формирует элемент и размещает его на базовом контроле
		/// </summary>
		/// <param name="baseCtrl">базовый контрол в коллекцию которого будет создваться элемент</param>
		/// <param name="propCtrl">описание свойства для которого формируется элемент</param>
		private void CreatePropertyControl(Control baseCtrl, PropertyInfo propCtrl)
		{
			
			switch (propCtrl.Type)
			{
				case PropertyInfo.PropertyType.Separator:
					m_currentLocationCtrl.Y += m_offsetTop;
					break;
				//создать флажок
				case PropertyInfo.PropertyType.Check:
					CheckBox box = new CheckBox();
					InitializeControl(box, propCtrl.Name, propCtrl.Text, m_currentLocationCtrl, AnchorStyles.Top | AnchorStyles.Left,
										Rectangle.Empty, DockStyle.None);
					box.Width = baseCtrl.Width - m_offsetTop * 2;
					box.Checked = (bool)propCtrl.Value;
					box.CheckedChanged += new EventHandler(OnCheckBoxValueChange);
					baseCtrl.Controls.Add(box);
					m_currentLocationCtrl.Y = box.Bottom;// + m_offsetTop;
					break;
				//создать элемент выбора цвета
				case PropertyInfo.PropertyType.Color:
					Label textLbl = CreateLabel(propCtrl.Name + "Lbl", propCtrl.Text, m_currentLocationCtrl, 
						AnchorStyles.Top | AnchorStyles.Left, true, Rectangle.Empty, DockStyle.None);
					baseCtrl.Controls.Add(textLbl);
					Label colorLbl = CreateLabel(propCtrl.Name, "", new Point(textLbl.Right, textLbl.Top),
												AnchorStyles.Top |AnchorStyles.Left, false, Rectangle.Empty, DockStyle.None);
					colorLbl.BackColor = (Color)propCtrl.Value;
					colorLbl.Click += new EventHandler(OnColorChange);
					baseCtrl.Controls.Add(colorLbl);
					m_currentLocationCtrl.Y = colorLbl.Bottom;// + m_offsetTop;
					break;
				//создать элемент открытия файла
				case PropertyInfo.PropertyType.Picture:
				case PropertyInfo.PropertyType.File:
					GroupBox gbox = new GroupBox();
					gbox.Text = propCtrl.Text;
					gbox.Location = m_currentLocationCtrl;
					gbox.Width = baseCtrl.Width - m_offsetLeft * 2;
					gbox.Name = propCtrl.Name;
					TextBox tbox = new TextBox();
					tbox.Location = new Point(m_offsetLeft, m_offsetTop * 4);
					tbox.Width = gbox.Width - m_offsetLeft * 2 - 25;
					tbox.Name = propCtrl.Name + "Path";
					tbox.Text = (string)propCtrl.Value;
					Button btn = new Button();
					btn.Text = "...";
					btn.Location = new Point(tbox.Right, tbox.Top);
					btn.Width = 25;
					btn.Name = propCtrl.Name + "Btn";
					btn.Height = tbox.Height;
					btn.Click += new EventHandler(OnFilePathEdit);

					gbox.Height = tbox.Height + (int)(tbox.Top * 1.5);

					if (PropertyInfo.PropertyType.Picture == propCtrl.Type)
					{
						PictureBox pb = new PictureBox();
						pb.SizeMode = PictureBoxSizeMode.StretchImage;
						pb.Name = propCtrl.Name + "Pic";
						pb.Location = new Point((int)(baseCtrl.Width / 2 - pb.Width /2), tbox.Bottom + m_offsetTop);
						pb.Width = pb.Height;

						try
						{
							pb.Image = Image.FromFile(tbox.Text);
						}
						catch(Exception)
						{}

						gbox.Height += pb.Height;

						gbox.Controls.Add(pb);
					}

					gbox.Controls.Add(tbox);
					gbox.Controls.Add(btn);
					baseCtrl.Controls.Add(gbox);
					m_currentLocationCtrl.Y = gbox.Bottom;// + m_offsetTop;
					break;
				//создать элемент выбора целочисленного или дробного числа
				case PropertyInfo.PropertyType.Integer:
				case PropertyInfo.PropertyType.Float:
					textLbl = CreateLabel(propCtrl.Name + "Lbl", propCtrl.Text, m_currentLocationCtrl, 
						AnchorStyles.Top | AnchorStyles.Left, true, Rectangle.Empty, DockStyle.None);
					NumericUpDown nud = new NumericUpDown();
					
					if (PropertyInfo.PropertyType.Float == propCtrl.Type)
						nud.DecimalPlaces = 3;
					
					nud.Location = new Point(textLbl.Right, textLbl.Top);
					nud.Anchor = AnchorStyles.Top | AnchorStyles.Left;
					nud.Name = propCtrl.Name;
					nud.Value = (decimal)propCtrl.Value;
					nud.Minimum = (decimal)propCtrl.Min;
					nud.Maximum = (decimal)propCtrl.Max;
					nud.ValueChanged += new EventHandler(OnNUDValueChanged);
					baseCtrl.Controls.Add(textLbl);
					baseCtrl.Controls.Add(nud);
					m_currentLocationCtrl.Y = nud.Bottom;// + m_offsetTop;
					break;
				//элемент выпадающий список
				case PropertyInfo.PropertyType.List:
					textLbl = CreateLabel(propCtrl.Name + "Lbl", propCtrl.Text, m_currentLocationCtrl, 
						AnchorStyles.Top | AnchorStyles.Left, true, Rectangle.Empty, DockStyle.None);
					ComboBox cbox = new ComboBox();
					InitializeControl(cbox, propCtrl.Name, "", new Point(textLbl.Right, textLbl.Top),
										AnchorStyles.Left | AnchorStyles.Top, Rectangle.Empty, DockStyle.None);
					cbox.DropDownStyle = ComboBoxStyle.DropDownList;

					for (int i = 0; i < propCtrl.ListValues.Count; i ++)
					{
						cbox.Items.Add(propCtrl.ListValues[i]);
					}

					cbox.SelectedItem = propCtrl.Value;
					cbox.SelectedIndexChanged += new EventHandler(OnListValueChange);

					if (baseCtrl.Width <= cbox.Left + cbox.Width)
					{
						cbox.Width = baseCtrl.Width - cbox.Left - m_offsetLeft;
					}

					baseCtrl.Controls.Add(textLbl);
					baseCtrl.Controls.Add(cbox);

					m_currentLocationCtrl.Y = cbox.Bottom;// + m_offsetTop;
					break;
				//элемент для ввода текста
				case PropertyInfo.PropertyType.Text:
					textLbl = CreateLabel(propCtrl.Name + "Lbl", propCtrl.Text, m_currentLocationCtrl, 
											AnchorStyles.Top | AnchorStyles.Left, true, Rectangle.Empty, DockStyle.None);
					TextBox tbox1 = new TextBox();
					tbox1.Anchor = AnchorStyles.Left | AnchorStyles.Top;
					tbox1.Name = propCtrl.Name;
					tbox1.Location = new Point(textLbl.Right, textLbl.Top);
					tbox1.Width = baseCtrl.Width - m_offsetLeft - textLbl.Width - m_offsetTop * 2;
					tbox1.Text = (string)propCtrl.Value;
					tbox1.TextChanged += new EventHandler(OnTextBoxValueChanged);
					tbox1.Enabled = propCtrl.Enabled;
					baseCtrl.Controls.Add(textLbl);
					baseCtrl.Controls.Add(tbox1);
					m_currentLocationCtrl.Y = tbox1.Bottom;// + m_offsetTop;
					break;
				//элемент группа, для группировки элементов
				case PropertyInfo.PropertyType.Group:
					GroupBox gbox1 = new GroupBox();
					
					if (null != propCtrl.LeftCtrlName)
					{
						Control ctrl = GetControlByName(baseCtrl, (string)propCtrl.LeftCtrlName);
						if (null != ctrl)
						{
							gbox1.Location = new Point(ctrl.Right + m_offsetLeft, ctrl.Top);
						}
					}
					else
					{
						gbox1.Location = m_currentLocationCtrl;
					}
					gbox1.Text = propCtrl.Text;
					gbox1.Width = (baseCtrl.Width - m_offsetLeft * 2) / (int)propCtrl.Max;
					gbox1.Height = baseCtrl.Height - m_offsetTop * 2;
					gbox1.Name = propCtrl.Name;

					baseCtrl.Controls.Add(gbox1);

					Point oldPoint = m_currentLocationCtrl;
					m_currentLocationCtrl = new Point(m_offsetLeft, m_offsetTop * 4);
					for(int i = 0; i < propCtrl.ListValues.Count; i++)
					{
						CreatePropertyControl(gbox1, (PropertyInfo)propCtrl.ListValues[i]);
					}

					gbox1.Height = m_currentLocationCtrl.Y + m_offsetTop;
					m_currentLocationCtrl = oldPoint;

					if (null == propCtrl.LeftCtrlName)
						m_currentLocationCtrl.Y = gbox1.Bottom;// + m_offsetTop;

					break;
				//элемент переключатель
				case PropertyInfo.PropertyType.Switch:
					RadioButton rb = new RadioButton();
					rb.Text = propCtrl.Text;
					rb.Name = propCtrl.Name;
					rb.Location = m_currentLocationCtrl;
					rb.Width = baseCtrl.Width - m_offsetTop * 2;
					
					baseCtrl.Controls.Add(rb);

					Point oldPnt = m_currentLocationCtrl;
					m_currentLocationCtrl.Y = rb.Bottom;// + m_offsetTop;
					m_currentLocationCtrl.X += m_offsetLeft;

					for (int i = 0; i < propCtrl.ListValues.Count; i++)
					{
						CreatePropertyControl(baseCtrl, (PropertyInfo)propCtrl.ListValues[i]);
					}

					m_currentLocationCtrl.X = oldPnt.X;
					rb.CheckedChanged += new EventHandler(OnSwitchCheckedChange);
					rb.Checked = (bool)propCtrl.Value;
					SetChildControlEnable(propCtrl, rb.Parent, rb.Checked);
					break;

				default:
					break;
			}
		}

		#endregion
		#region Обработчики сообщений от динамически созданных элементов управления.

		/// <summary>
		/// событие по нажатию кнопки открытия файла
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnFilePathEdit(object sender, EventArgs e)
		{
			Button btn = (Button)sender;
			string propName = btn.Name.Substring(0, btn.Name.Length - 3);

			TextBox tbox = (TextBox)GetControlByName(btn.Parent, propName + "Path");

			if (null != tbox)
			{
				PropertyInfo prop = GetPropertyByName(propName, PropertyList.Properties);

				if(null == prop)
					return;

				OpenFileDialog dlg = new OpenFileDialog();
				dlg.InitialDirectory = tbox.Text;
				dlg.Filter = (string)prop.Max;

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					prop.Value = dlg.FileName;
					tbox.Text = dlg.FileName;

					if (PropertyInfo.PropertyType.Picture == prop.Type)
					{
						try
						{
							PictureBox pb = (PictureBox)GetControlByName(btn.Parent, prop.Name + "Pic");
							if (null != pb)
							{
								pb.Image = Image.FromFile(tbox.Text);
							}
						}
						catch(Exception)
						{}
					}
				}

			}
		}

		/// <summary>
		/// событие на изменение значения статуса группы переключателей
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSwitchCheckedChange(object sender, EventArgs e)
		{
			RadioButton rb = (RadioButton)sender;
			PropertyInfo prop = GetPropertyByName(rb.Name, PropertyList.Properties);

			if (null != prop)
			{
				prop.Value = rb.Checked;

				SetChildControlEnable(prop, rb.Parent, rb.Checked);
			}
		}

		private void SetChildControlEnable(PropertyInfo prop, Control parentCtrl, bool isEnable)
		{
			Control ctrl;
			for (int i = 0; i < prop.ListValues.Count; i++)
			{
				ctrl = GetControlByName(parentCtrl, ((PropertyInfo)prop.ListValues[i]).Name);

				if (null != ctrl)
				{
					ctrl.Enabled = isEnable;
				}
			}
		}

		/// <summary>
		/// событие на изменение значения статуса флажка
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnCheckBoxValueChange(object sender, EventArgs e)
		{
			CheckBox box = (CheckBox)sender;
			PropertyInfo prop = GetPropertyByName(box.Name, PropertyList.Properties);

			if(null != prop)
			{
				prop.Value = box.Checked;
			}
		}

		/// <summary>
		/// событие на изменения цвета
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnColorChange(object sender, EventArgs e)
		{
			Label colorLbl = (Label)sender;
			PropertyInfo prop = GetPropertyByName(colorLbl.Name, PropertyList.Properties);

			if(null != prop)
			{
				ColorDialog dlg = new ColorDialog();
				dlg.Color = colorLbl.BackColor;
				if(dlg.ShowDialog() == DialogResult.OK)
				{
					colorLbl.BackColor = dlg.Color;
					prop.Value = dlg.Color;
				}
			}
		}

		/// <summary>
		/// событие на изменение значения числа
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnNUDValueChanged(object sender, EventArgs e)
		{
			NumericUpDown nud = (NumericUpDown)sender;
			PropertyInfo prop = GetPropertyByName(nud.Name, PropertyList.Properties);

			if (null != prop)
			{
				prop.Value = nud.Value;
			}
		}

		/// <summary>
		/// событие на изменение значения выпадающего списка
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnListValueChange(object sender, EventArgs e)
		{
			ComboBox cbox = (ComboBox)sender;
			PropertyInfo prop = GetPropertyByName(cbox.Name, PropertyList.Properties);

			if(null != prop)
			{
				prop.Value = cbox.SelectedItem;
			}
		}

		/// <summary>
		/// событие на изменение значения контрола TextBox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnTextBoxValueChanged(object sender, EventArgs e)
		{
			TextBox tbox = (TextBox)sender;
			PropertyInfo prop = GetPropertyByName(tbox.Name, PropertyList.Properties);

			if (null != prop)
			{
				prop.Value = tbox.Text;
			}
		}

		#endregion

		/// <summary>
		/// возвращает контрол из списка контролов базового элемента по заданному имени
		/// </summary>
		/// <param name="baseCtrl">базовый контрол</param>
		/// <param name="controlName">имя интересуемого контрола</param>
		/// <returns>найденный контрол, иначе null</returns>
		private Control GetControlByName(Control baseCtrl, string controlName)
		{
			Control ctrl = null;
			for(int i = 0; i < baseCtrl.Controls.Count; i++)
			{
				if (baseCtrl.Controls[i].Name == controlName)
				{
					ctrl = baseCtrl.Controls[i];
					break;
				}
			}
			return ctrl;
		}

		/// <summary>
		/// возвращает описание свойства из списка по заданному имени
		/// </summary>
		/// <param name="propName">имя свойства описание которого надо вернуть</param>
		/// <returns>найденное описание свойства, если неудачно то null</returns>
		private PropertyInfo GetPropertyByName(string propName, ArrayList props)
		{
			PropertyInfo res = null;

			for(int i = 0; i < props.Count; i++)
			{
				if(props[i] is PropertyInfo)
				{
					if (propName == ((PropertyInfo)props[i]).Name)
					{
						res = (PropertyInfo)props[i];
						break;
					}
					else
					{
						res = GetPropertyByName(propName, ((PropertyInfo)props[i]).ListValues);
						if (res != null)
						{
							break;
						}
					}
				}
				else
					break;
			}

			return res;
		}


		/// <summary>
		/// текущее положение левого верхнего угла для добавляемого контрола относительно родителя
		/// </summary>
		private Point m_currentLocationCtrl;
		private PropertyGroup m_propertyList;
		/// <summary>
		/// Список свойств отображаемых на панели
		/// </summary>
		public PropertyGroup PropertyList
		{
			get
			{
				return m_propertyList;
			}
			set
			{
				m_propertyList = value;
			}
		}

		/// <summary>
		/// базовое смещение левой границы дочернего контрола от левой границы контрола родителя
		/// </summary>
		private const int m_offsetLeft = 5;
		/// <summary>
		/// базовое смещение верхней границы дочернего контрола от верхней границы контрола родителя
		/// </summary>
		private const int m_offsetTop = 5;

	}
}
