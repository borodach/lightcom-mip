using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using GPS.Dispatcher.Common;
using GPS.Common;

namespace GPS.Dispatcher.UI
{
	/// <summary>
	/// Summary description for DisplayInfoSettings.
	/// </summary>
	public class DisplayInfoSettings : IProperties, IPersistant
	{
		public enum ClientImageType
		{
			Circle,
			Cross,
			Rectangle,
			Image
		};

		public DisplayInfoSettings()
		{
			Reset();
		}

		#region Реализация интерфейса IProperties.

		public ObjectInfo GetObjectInfo()
		{
			// TODO:  Add DisplayInfoSettings.GetObjectInfo implementation
			return null;
		}

		public void GetPropertyList(PropertyGroup group)
		{
			if ("DisplaySettings" == group.Name) 
			{
				group.Properties.Add(GetProperty("BaseTypeImage"));
				group.Properties.Add(GetProperty("BmpTypeImage"));
				group.Properties.Add(PropertyInfo.Separator);
				group.Properties.Add(PropertyInfo.Separator);
				group.Properties.Add(GetProperty("ImageSize"));
			}
		}

		public void SetProppertyList(PropertyGroup group)
		{
			for (int i = 0; i < group.Properties.Count; i++)
			{
				SetProperty((PropertyInfo)group.Properties[i]);
			}
		}

		public PropertyInfo GetProperty(string propertyName)
		{
			PropertyInfo prop = new PropertyInfo();
			prop.Name = propertyName;

			if ("BaseTypeImage" == prop.Name)
			{
				prop.Text = "Использовать стандартные примитивы:";
				prop.Type = PropertyInfo.PropertyType.Switch;
				prop.Value = (ImageType != ClientImageType.Image);
				prop.ListValues.Add(GetProperty("BaseTypeImageList"));
				prop.ListValues.Add(PropertyInfo.Separator);
				prop.ListValues.Add(GetProperty("BaseTypeImageColor"));
			}
			if ("BaseTypeImageList" == prop.Name)
			{
				prop.Text = "Использовать примитив: ";
				prop.Type = PropertyInfo.PropertyType.List;
				prop.ListValues.Add("Круг");
				prop.ListValues.Add("Крест");
				prop.ListValues.Add("Прямоугольник");
				prop.Value = GetStringFromImageType(ImageType);
			}
			if ("BaseTypeImageColor" == prop.Name)
			{
				prop.Text = "Использовать цвет: ";
				prop.Type = PropertyInfo.PropertyType.Color;
				prop.Value = ImageForeColor;
			}
			if ("BmpTypeImage" == prop.Name)
			{
				prop.Text = "Использовать изображение:";
				prop.Type = PropertyInfo.PropertyType.Switch;
				prop.Value = (ImageType == ClientImageType.Image);
				prop.ListValues.Add(GetProperty("BmpTypeImageCtrl"));
			}
			if ("BmpTypeImageCtrl" == prop.Name)
			{
				prop.Text = "Выбор изображения:";
				prop.Type = PropertyInfo.PropertyType.Picture;
				prop.Max = "Файлы изображений (*.bmp; *.jpg; *.gif)|*.bmp;*.jpg;*.gif;*.jpeg";
				prop.Value = ImagePath;
			}
			if ("ImageSize" == prop.Name) 
			{
				prop.Text = "Размер изображения: ";
				prop.Type = PropertyInfo.PropertyType.Group;
				prop.Max = 1;
				prop.ListValues.Add(GetProperty("ImageWidth"));
				prop.ListValues.Add(PropertyInfo.Separator);
				prop.ListValues.Add(GetProperty("ImageHeight"));
			}
			if ("ImageWidth" == prop.Name)
			{
				prop.Text = "Длинна: ";
				prop.Type = PropertyInfo.PropertyType.Integer;
				prop.Max = (decimal)m_maxImageWidth;
				prop.Min = (decimal)m_minImageWidth;
				prop.Value = (decimal)ImageSize.Width;
			}
			if ("ImageHeight" == prop.Name)
			{
				prop.Text = "Высота: ";
				prop.Type = PropertyInfo.PropertyType.Integer;
				prop.Max = (decimal)m_maxImageHeight;
				prop.Min = (decimal)m_minImageHeight;
				prop.Value = (decimal)ImageSize.Height;
			}
			return prop;
		}

		private ClientImageType GetImageTypeFromString(string str)
		{
			if ("Крест" == str)
				return ClientImageType.Cross;
			if ("Прямоугольник" == str)
				return ClientImageType.Rectangle;

			return ClientImageType.Circle;
		}
		private string GetStringFromImageType(ClientImageType type)
		{
			if (ClientImageType.Rectangle == type)
				return "Прямоугольник";
			if (ClientImageType.Cross == type)
				return "Крест";

			return "Круг";
		}

		public void SetProperty(PropertyInfo val)
		{
			if ("BaseTypeImage" == val.Name)
			{
				if ((bool)val.Value)
				{
					for (int i = 0; i < val.ListValues.Count; i++)
					{
						SetProperty((PropertyInfo)val.ListValues[i]);
					}
				}
			}
			if ("BaseTypeImageList" == val.Name)
			{
				ImageType = GetImageTypeFromString((string)val.Value);
			}
			if ("BaseTypeImageColor" == val.Name)
			{
				ImageForeColor = (Color)val.Value;
			}
			if ("BmpTypeImage" == val.Name)
			{
				if (true == (bool)val.Value) 
				{
					ImageType = ClientImageType.Image;
 
					for (int i = 0; i < val.ListValues.Count; i ++)
					{
						SetProperty((PropertyInfo)val.ListValues[i]);
					}
				}
			}
			if ("BmpTypeImageCtrl" == val.Name)
			{
				ImagePath = (string)val.Value;
			}
			if ("ImageSize" == val.Name)
			{
				for (int i = 0; i < val.ListValues.Count; i ++)
				{
					SetProperty((PropertyInfo)val.ListValues[i]);
				}
			}
			if ("ImageWidth" == val.Name)
			{
				m_clientImageSize.Width = Convert.ToInt32(val.Value);
			}
			if ("ImageHeight" == val.Name)
			{
				m_clientImageSize.Height = Convert.ToInt32(val.Value);
			}
		}

		#endregion

		#region IPersistant Members

		public void Reset()
		{
			ImageType = m_deffClientImageType;
			ImageSize = m_deffClientImageSize;
			ImageForeColor = m_deffForeColor;
			ImagePath = "";
			m_clientImage = null;
			m_imageChanged = true;
		}

		public bool SaveGuts(System.IO.Stream stream)
		{
			bool result = true;

			try
			{
				Utils.Write(stream, (int)ImageType);
				Utils.Write(stream, ImageSize.Width);
				Utils.Write(stream, ImageSize.Height);
				Utils.Write(stream, ImagePath);
				ColorConverter clr = new ColorConverter();
				Utils.Write(stream, clr.ConvertToString(ImageForeColor));
			}
			catch(Exception)
			{
				result = false;
			}

			return result;
		}

		public bool RestoreGuts(System.IO.Stream stream)
		{
			bool result = true;

			try
			{
				int w, h;
				ImageType = (ClientImageType)Utils.ReadInt(stream);
				w = Utils.ReadInt(stream);
				h = Utils.ReadInt(stream);
				ImageSize = new Size(w, h);
				ImagePath = Utils.ReadString(stream);
				ColorConverter clr = new ColorConverter();
				ImageForeColor = (Color)clr.ConvertFromString(Utils.ReadString(stream));
				ImageChanged = true;
			}
			catch(Exception)
			{
				result = false;
			}

			return result;
		}

		#endregion

		/// <summary>
		/// Тип изображения клиента по умолчанию (круг).
		/// </summary>
		protected const ClientImageType m_deffClientImageType = ClientImageType.Circle;
		/// <summary>
		/// Размеры отображаемого прямоугольника по умолчанию (10 Х 10).
		/// </summary>
		protected Size m_deffClientImageSize = new Size(10, 10);
		/// <summary>
		/// Цвет изображения по умолчанию (красный).
		/// </summary>
		protected Color m_deffForeColor = Color.Red;

		private ClientImageType m_clientImageType;
		/// <summary>
		/// Тип изображения клиента.
		/// </summary>
		public ClientImageType ImageType
		{
			get{return m_clientImageType;}
			set
			{
				if (m_clientImageType != value)
				{
					m_clientImage = null;
					m_imageChanged = true;
				}

				m_clientImageType = value;
			}
		}

		private Size m_clientImageSize;
		/// <summary>
		/// Размеры изображения клиента
		/// </summary>
		public Size ImageSize
		{
			get{return m_clientImageSize;}
			set
			{
				m_clientImageSize = value;
			}
		}

		private Color m_foreColor;
		/// <summary>
		/// Цвет изображения клиента
		/// </summary>
		public Color ImageForeColor
		{
			get{return m_foreColor;}
			set
			{
				if (!m_foreColor.Equals(value))
				{
					m_clientImage = null;
					m_imageChanged = true;
				}

				m_foreColor = value;
			}
		}

		private string m_imagePath;
		/// <summary>
		/// Путь к картинке с необходимым изображением клиента
		/// </summary>
		public string ImagePath
		{
			get{return m_imagePath;}
			set{m_imagePath = value;}
		}

		private Image m_clientImage;
		/// <summary>
		/// Изображение для отображения клиента
		/// </summary>
		public Image ClientImage
		{
			get
			{
				if (null == m_clientImage)
				{
					if (ClientImageType.Image == ImageType)
					{
						try
						{
							Image tmpImg = Image.FromFile(m_imagePath);
							m_clientImage = new Bitmap (ImageSize.Width, ImageSize.Height);
							Graphics g = Graphics.FromImage (m_clientImage);
							g.DrawImage (tmpImg, new Rectangle (0, 0, ImageSize.Width, ImageSize.Height),
								new Rectangle(0, 0, tmpImg.Width, tmpImg.Height), GraphicsUnit.Pixel);
						}
						catch(Exception)
						{
							m_clientImage = null;
						}
					}
					else
					{
						m_clientImage = new Bitmap(ImageSize.Width, ImageSize.Height);
						Graphics g = Graphics.FromImage(m_clientImage);
						g.Clear(Color.White);
						Brush brush = new SolidBrush(ImageForeColor);
						if (ClientImageType.Circle == ImageType)
						{
							g.FillEllipse(brush, 0, 0, ImageSize.Width - 1, ImageSize.Height - 1);
						}
						else
						{
							if (ClientImageType.Rectangle == ImageType)
							{
								g.FillRectangle(brush, 0, 0, ImageSize.Width, ImageSize.Height);
							}
							else
							{
								if (ClientImageType.Cross == ImageType)
								{
									Pen pen = new Pen(ImageForeColor);
									g.DrawLine(pen, 0, 0, ImageSize.Width - 1, ImageSize.Height - 1);
									g.DrawLine(pen, 0, ImageSize.Height - 1, ImageSize.Width - 1, 0);
								}
							}
						}
					}
				}

				return m_clientImage;
			}
			set{m_clientImage = value;}
		}

		/// <summary>
		/// Если изображение изменилось то true.
		/// </summary>
		private bool m_imageChanged;
		public bool ImageChanged
		{
			get{return m_imageChanged;}
			set{m_imageChanged = value;}
		}


		private const int m_maxImageWidth = 20;
		private const int m_minImageWidth = 5;
		private const int m_maxImageHeight = 20;		
		private const int m_minImageHeight = 5;
	}
}
