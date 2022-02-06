using System;
using LightCom.Common;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class BindPoint: IPersistent
    {
        /// <summary>
        /// �����������
        /// </summary>
        public BindPoint ()
        {
            geoPoint = new GlobalPoint ();
            mapPoint = new MapPoint ();
        }



        /// <summary>
        /// �������������� �����
        /// </summary>
        private GlobalPoint geoPoint;

        /// <summary>
        /// �������������� �����
        /// </summary>
        public MapPoint PointOnEarth
        {
            get
            {
                return geoPoint;
            }            
        }

        /// <summary>
        /// ����� �� �����
        /// </summary>
        private MapPoint mapPoint;

        /// <summary>
        /// ����� �� �����
        /// </summary>
        public MapPoint PointOnMap
        {
            get
            {
                return mapPoint;
            }
        }
    }
}