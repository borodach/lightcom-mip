using System;
using GPS.Dispatcher.UI;
using GPS.Common;

namespace GPS.Common
{
	/// <summary>
	/// Summary description for DrawingObjectInfo.
	/// </summary>
	public class DrawingObjectInfo
	{
		public DrawingObjectInfo()
		{
			m_dispInfo = null;
			m_leftUp = new GlobalPoint ();
		}

		private GlobalPoint m_leftUp;
		public GlobalPoint LeftUpperCorner
		{
			get {return m_leftUp;}
			set {m_leftUp = value;}
		}

		private DisplayInfoSettings m_dispInfo;
		public DisplayInfoSettings DisplaySettings
		{
			get {return m_dispInfo;}
			set {m_dispInfo = value;}
		}
	}
}
