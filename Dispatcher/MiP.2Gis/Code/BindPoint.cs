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
        /// Конструктор
        /// </summary>
        public BindPoint ()
        {
            geoPoint = new GlobalPoint ();
            mapPoint = new MapPoint ();
        }



        /// <summary>
        /// Географическая точка
        /// </summary>
        private GlobalPoint geoPoint;

        /// <summary>
        /// Географическая точка
        /// </summary>
        public MapPoint PointOnEarth
        {
            get
            {
                return geoPoint;
            }            
        }

        /// <summary>
        /// Точка на карте
        /// </summary>
        private MapPoint mapPoint;

        /// <summary>
        /// Точка на карте
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