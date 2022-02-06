///////////////////////////////////////////////////////////////////////////////
//
//  File:           BindPointCallout.cs
//
//  Facility:       Плагин MiP для 2Gis
//
//
//  Abstract:       Класс для работы с объектами "точка привязки" карты 2Gis
//
//  Environment:    VC# 8.0
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  03-11-2007
//
//  Copyright (C) OOO "ЛайтКом", 2007. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
* $History: BindPointCalloutManager.cs $
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 11.03.07   Time: 14:40
 * Created in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
* 
*/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// Класс для работы с объектами "точка привязки" карты 2GiS 
    /// </summary>    
    public class BindPointCalloutManager
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="plugin2Gis">Объект плагина</param>
        public BindPointCalloutManager (MiPPlugin plugin2Gis)
        {
            plugin = plugin2Gis;
        }

        /// <summary>
        /// Префикс тега точки привязки
        /// </summary>
        public const string BindPointTagPrefix = "MiP_BP_";

        /// <summary>
        /// Удаление всех точек привязки с карты
        /// </summary>
        public void Clear ()
        {
            ComWrapper<GrymCore.IMapGraphics> map = new ComWrapper<GrymCore.IMapGraphics> ();
            map.COMObject = plugin.Map as GrymCore.IMapGraphics;             
            using (map)
            {
                GrymCore.IMapGraphics objMap = map.COMObject;
                int count = objMap.GraphicCount;
                for (int idx = count - 1; idx >= 0; --idx)
                {
                    ComWrapper<GrymCore.IGraphicBase> graphic = new ComWrapper<GrymCore.IGraphicBase> ();
                    graphic.COMObject = objMap.get_GraphicByIndex (idx);
                    using (graphic)
                    {
                        if (graphic.COMObject.Tag.StartsWith (BindPointTagPrefix))
                        {
                            objMap.RemoveGraphic (graphic.COMObject);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Создает графические объекты для всех точек привязки
        /// </summary>
        public void CreateAll ()
        {
            Clear ();

            ComWrapper<GrymCore.IGrymObjectFactory> factory = new ComWrapper<GrymCore.IGrymObjectFactory> ();
            factory.COMObject = plugin.BaseView.Factory;
            using (factory)
            {
                GrymCore.IGrymObjectFactory objFactory = factory.COMObject;
                ComWrapper<GrymCore.IMapGraphics> map = new ComWrapper<GrymCore.IMapGraphics> ();
                map.COMObject = (GrymCore.IMapGraphics) Marshal.GetTypedObjectForIUnknown (Marshal.GetIUnknownForObject (plugin.Map), typeof (GrymCore.CalloutClass));
                using (map)
                {
                    GrymCore.IMapGraphics objMap = map.COMObject;
                    int count = plugin.BingingPoints.BindPoints.Count;
                    for (int idx = 0; idx < count; ++idx)
                    {
                        BindPoint bp = plugin.BingingPoints.BindPoints [idx];
                        ComWrapper<GrymCore.IMapPoint> pt = new ComWrapper<GrymCore.IMapPoint> (objFactory.CreateMapPoint (bp.PointOnMap.x, bp.PointOnMap.y));
                        using (pt)
                        {
                            ComWrapper<GrymCore.Callout> callout = new ComWrapper<GrymCore.Callout> ();
                            callout.COMObject = objMap.CreateCallout (pt.COMObject, bp.ToString (), false);
                            callout.COMObject.Tag = BindPointTagPrefix + ToString ();
                            callout.COMObject.OnButtonAction += new GrymCore._ICalloutEvents_OnButtonActionEventHandler (COMObject_OnButtonAction);
                        }
                    }                    
                    
                    //objFactory.CreateMapPoint ();
                    //objMap.CreateCallout ();
                }
            }
        }

        void COMObject_OnButtonAction (int nIndex)
        {
            throw new Exception ("The method or operation is not implemented.");
        }
        
        /// <summary>
        /// Изменяет точку привязки на карте
        /// </summary>
        /// <param name="idx">Индекс точки привязки</param>
        public void Update (int idx)
        {
        }

        /// <summary>
        /// Объект плагина
        /// </summary>
        private MiPPlugin plugin;
    }
}
