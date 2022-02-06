//////////////////////////////////////////////////////////////////////////
///�����:		DiskMapPane
///��������:	����� ������������ ��� �������� ������ ����� � �����
///
//////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;

namespace GPS.Dispatcher.Controls
{
	/// <summary>
	/// 
	/// </summary>
	public class DiskMapPane : MapPane
	{
		public DiskMapPane()
		{
			this.m_paneImagePath = null;
		}

		public DiskMapPane(string paneImagePath)
		{
			this.m_paneImagePath = paneImagePath;
//			this.DisasmPaneName(this.m_paneImagePath);
		}

		public DiskMapPane(string paneImagePath, Rectangle rect) : this(paneImagePath)
		{
			this.m_physicalDimensionRect = rect;
		}

		public DiskMapPane(string paneImagePath, Rectangle rect, RectangleF rectF) : this(paneImagePath, rect)
		{
			this.m_geoDimensionRect = rectF;
		}

		/// <summary>
		/// ���������������� ����� �������� ������ ��� ����������� ������������ ������ ����� �����
		/// </summary>
		/// <param name="retImage">�������� �������� ��� �������� ������</param>
		/// <returns>true ���� ������, false ���� ���</returns>
		public override bool GetPaneImage(out Image retImage)
		{
			retImage = null;

			if(null == this.m_paneImage)
			{
				if(!this.LoadPaneImageFromPath(out this.m_paneImage))
				{
					return false;
				}
			}

			retImage = this.m_paneImage;
			return true;
		}

		/// <summary>
		/// ��������� ����� ����� ����� � �����
		/// </summary>
		/// <param name="paneImage">�������� �������� ��� ��������� ������������ ������</param>
		/// <returns>true ���� ����� ��������, false ���� ���</returns>
		private bool LoadPaneImageFromPath(out Image paneImage)
		{
			paneImage = null;
			
			if (null == this.m_paneImagePath)
			{
				return false;
			}

			try
			{
				paneImage = Image.FromFile(this.m_paneImagePath);
				if (null != paneImage)
					return true;
				else
					return false;
			}
			catch(Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// ������ ����� ����� ��� ��������� �������������� ����������
		/// </summary>
		/// <param name="paneName">��� ����� �����</param>
		/// <returns>���� ��� ������ �� true ����� false</returns>
		private bool DisasmPaneName(string paneName)
		{
			string sData;
			int length;
			string split = "\\";
			int pos = paneName.LastIndexOf(split) + 1;
			paneName = paneName.Substring(pos, paneName.Length - pos);
			int left, top, width, height;
			paneName = paneName.ToLower();
			length = paneName.IndexOf("y");
			sData = paneName.Substring(3, length - 3);
			left = int.Parse(sData);
			pos = length + 1;
			length = paneName.IndexOf("z");
			sData = paneName.Substring(pos, length - pos);
			top = int.Parse(sData);
			length = paneName.IndexOf("w");
			pos = length + 1;
			length = paneName.IndexOf("h");
			sData = paneName.Substring(pos, length - pos);
			width = int.Parse(sData);
			pos = length + 1;
			length = paneName.IndexOf(".");
			sData = paneName.Substring(pos, length - pos);
			height = int.Parse(sData);
			this.m_physicalDimensionRect = new Rectangle(left, top, width, height);

			return true;
		}

		/// <summary>
		/// ��������������� ������ ����� �����
		/// </summary>
		private string m_paneImagePath;
	}
}
