///////////////////////////////////////////////////////////////////////////////
//
//  File:           MipPlugin.cs
//
//  Facility:       Плагин MiP для 2Gis
//
//
//  Abstract:       Интерфейс плагина для 2Gis
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
* $History: MipPlugin.cs $
 * 
 * *****************  Version 14  *****************
 * User: Serg         Date: 12.08.08   Time: 21:18
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 13  *****************
 * User: Sergey       Date: 11.04.07   Time: 22:34
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 12  *****************
 * User: Sergey       Date: 11.04.07   Time: 20:10
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 11  *****************
 * User: Sergey       Date: 5.04.07    Time: 8:09
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 10  *****************
 * User: Sergey       Date: 4.04.07    Time: 20:25
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 9  *****************
 * User: Sergey       Date: 4.04.07    Time: 8:11
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:26
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 18.03.07   Time: 14:24
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 18.03.07   Time: 10:31
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * Добавлен диалог свойств плагина
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 17.03.07   Time: 11:02
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 16.03.07   Time: 23:04
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:44
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 16.03.07   Time: 19:33
 * Updated in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 11.03.07   Time: 14:40
 * Created in $/LightCom/.NET/MiP/Dispatcher/MiP.2Gis
* 
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LightCom.Common;
using LightCom.MiP.Common;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    [Guid ("5BCB4DF9-41A3-4bc0-A2DF-9DB89419C6C4"),
       ClassInterface (ClassInterfaceType.None)]
    [ComVisible (true)]
    public class MiPPlugin: IDisposable, GrymCore.IGrymPlugin, GrymCore.IGrymPluginInfo, GrymCore.IGrymPluginOptions
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public MiPPlugin ()
        {
            //bingingPoints = new MapBindPoints (this);
            mapperBP = new BindPointMapper ();
            simpleMapper = new SimpleMapper ();
            settings = new XMLSetingsStorage ();
        }
        #region "IDisposable implementation"
        // Track whether Dispose has been called.
        private bool disposed = false;



        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose ()
        {
            Dispose (true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue 
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize (this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the 
        // runtime from inside the finalizer and you should not reference 
        // other objects. Only unmanaged resources can be disposed.
        private void Dispose (bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    //this.BingingPoints.Dispose ();
                    this.Root = null;
                    this.BaseView = null;
                    this.Map = null;
                    this.SubMenu = null;
                    this.Device = null;
                    foreach (MobileObjectGraphic obj in mobileObjects)
                    {
                        obj.Dispose ();
                    }
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.                
            }
            disposed = true;
        }


        ~MiPPlugin ()
        {
            Dispose (false);
        }
        #endregion
        #region "Properties"
        
        /// <summary>
        /// Объект прилохжения 2Gis
        /// </summary>
        private SafeComWrapper<GrymCore.Grym> root = new SafeComWrapper<GrymCore.Grym> ();

        /// <summary>
        /// Объект прилохжения 2Gis
        /// </summary>
        public GrymCore.Grym Root
        {
            get
            {
                return root.COMObject;
            }
            set
            {
                root.COMObject = value;
            }
        }

        /// <summary>
        /// Объект текущая карта
        /// </summary>
        private SafeComWrapper<GrymCore.IBaseViewThread> baseView = new SafeComWrapper<GrymCore.IBaseViewThread> ();

        /// <summary>
        /// Объект текущая карта
        /// </summary>
        public GrymCore.IBaseViewThread BaseView
        {
            get
            {
                return baseView.COMObject;
            }
            set
            {
                baseView.COMObject = value;
            }
        }

        /// <summary>
        /// Объект карта
        /// </summary>
        private SafeComWrapper<GrymCore.IMap> map = new SafeComWrapper<GrymCore.IMap> ();

        /// <summary>
        /// Объект карта
        /// </summary>
        public GrymCore.IMap Map
        {
            get
            {
                return map.COMObject;
            }
            set
            {
                map.COMObject = value;
            }
        }

        /// <summary>
        /// Точки привязки
        /// </summary>
        //private MapBindPoints bingingPoints;
        //public MapBindPoints BingingPoints
        //{
        //    get
        //    {
        //        return bingingPoints;
        //    }
        //}

        /// <summary>
        /// Подменю контекстного меню
        /// </summary>
        private SafeComWrapper <GrymCore.ISubMenu> pluginSubMenu = new SafeComWrapper <GrymCore.ISubMenu> ();
        
        /// <summary>
        /// Подменю контекстного меню
        /// </summary>
        public GrymCore.ISubMenu SubMenu
        {
            get
            {
                return pluginSubMenu.COMObject;
            }
            set
            {
                pluginSubMenu.COMObject = value;
            }
        }

        /// <summary>
        /// Графическое устройство
        /// </summary>
        public SafeComWrapper<GrymCore.IDevice> pluginDevice = new SafeComWrapper<GrymCore.IDevice> ();
        
        /// <summary>
        /// Графическое устройство
        /// </summary>
        public GrymCore.IDevice Device
        {
            get
            {
                return pluginDevice.COMObject;
            }
            set
            {
                pluginDevice.COMObject = value;
            }
        }

        
        /// <summary>
        /// Преобразователь координат.
        /// </summary>
        private BindPointMapper mapperBP;

        /// <summary>
        /// Преобразователь координат.
        /// </summary>
        public BindPointMapper MapperBP 
        {
            get 
            {
                return mapperBP;
            }
        }

        /// <summary>
        /// Простой преобразователь координат.
        /// </summary>
        private SimpleMapper simpleMapper;

        /// <summary>
        /// Простой преобразователь координат.
        /// </summary>
        public SimpleMapper MapperSimple
        {
            get
            {
                return simpleMapper;
            }
        }

        /// <summary>
        /// Команда на добавление точки привязки.
        /// </summary>
        //private AddBindPointCommand addBindPointCommand;

        /// <summary>
        /// Хранилище настроек
        /// </summary>
        private XMLSetingsStorage settings;

        /// <summary>
        /// Хранилище настроек
        /// </summary>
        public XMLSetingsStorage Settings
        {
            get
            {
                return this.settings;
            }
        }

        /// <summary>
        /// Признак видимости точек привязки
        /// </summary>
        public bool IsBindPointsVisible
        {
            get 
            {
                int nVal;
                Settings.Read ("MapBinding", "DisplayPoints", out nVal, 0);
                return nVal != 0;
            }
            set
            {
                Settings.Write ("MapBinding", "DisplayPoints", value ? 1 : 0);
            }
        }

        /// <summary>
        /// Количество точек карты в одном метре
        /// </summary>
        double MapDx
        {
            get
            {
                double val;
                Settings.Read ("MapBinding", "Dx", out val, 100);
                return val;
            }
            set
            {
                Settings.Write ("MapBinding", "Dx", value);
            }
        }

        /// <summary>
        /// Количество точек карты в одном метре
        /// </summary>
        double MapDy
        {
            get
            {
                double val;
                Settings.Read ("MapBinding", "Dy", out val, 100);
                return val;
            }
            set
            {
                Settings.Write ("MapBinding", "Dy", value);
            }
        }

        /// <summary>
        /// Преобразователь координат
        /// </summary>
        private GrymCore.IMapCoordinateTransformationGeo transformer = null;

        /// <summary>
        /// Мобильные объекты
        /// </summary>
        private List<MobileObjectGraphic> mobileObjects = new List<MobileObjectGraphic> ();

        /// <summary>
        /// Мобильные объекты
        /// </summary>
        public List<MobileObjectGraphic> MobileObjects
        {
            get { return mobileObjects; }
        }

        /// <summary>
        /// Окно управления воспроизведением
        /// </summary>
        private PlayerForm playerForm;

        /// <summary>
        /// Окно со списком клиентов
        /// </summary>
       //private MobileObjectList clientListForm;
       
        #endregion
        #region "Registration"
        //-------------------------------------------------
        // Регистрация в COM-категории "2Gis Grym Plugins"

        // CATID категории "2Gis Grym Plugins"
        const String GCAT = "{687FCBEC-C7A2-4D4C-B79B-DA650E7C29F7}";

        // Вызывается при регистрации класса в качестве COM-сервера
        [ComRegisterFunctionAttribute]
        public static void AppendInto2GisCat (String key)
        {               
            // Добавляем запись в реестр.
            StringBuilder sb = new StringBuilder (key);
            sb.Replace ("HKEY_CLASSES_ROOT\\", "");
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey (sb.ToString (), true);
            RegistryKey ic = rk.OpenSubKey ("Implemented Categories", true);
            ic.CreateSubKey (GCAT);
            ic.Close ();
            rk.Close ();
        }

        // Вызывается при удалении регистрации COM-сервера
        [ComUnregisterFunctionAttribute]                                  
        public static void RemoveFrom2GisCat (String key)
        {
            // Удаляем запись из реестра.
            StringBuilder sb = new StringBuilder (key);
            sb.Replace ("HKEY_CLASSES_ROOT\\", "");
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey (sb.ToString (), true);
            RegistryKey ic = rk.OpenSubKey ("Implemented Categories", true);
            ic.DeleteSubKey (GCAT);
            ic.Close ();
            rk.Close ();
        }
        #endregion
        #region IGrymPlugin Members

        /// <summary>
        /// Инициализация модуля
        /// </summary>
        /// <param name="pRoot">Объект приложения 2Gis</param>
        /// <param name="pBaseView">Базовый объект карты</param>
        public void Initialize (GrymCore.Grym pRoot, GrymCore.IBaseViewThread pBaseView)
        {               
            Root = pRoot;
            BaseView = pBaseView;
            Map = pBaseView.Map;
            
            IntPtr iUnknown = Marshal.GetIUnknownForObject (Map);
            Device = (GrymCore.IDevice) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IDevice));
            Marshal.Release (iUnknown);

            Settings.FileName = String.Format ("{0}\\{1}_Setings.xml", Utils.ExeDirectory, BaseView.BaseReference.Name);
            Settings.PreLoad ();
            IsBindPointsVisible = IsBindPointsVisible;

            try
            {
           
            GrymCore.IBaseReference objMapInfo = BaseView.BaseReference;
            //ComWrapper<GrymCore.IBaseReference> mapInfo = new ComWrapper<GrymCore.IBaseReference> (objMapInfo);
            //using (mapInfo)
            {
                //bingingPoints.Reset ();
                //bingingPoints.Tag = objMapInfo.Name;
                ////MessageBox.Show ("МиП");
                //bingingPoints.Load ();
                //bingingPoints.Tag = objMapInfo.Name;
                //bingingPoints.Description = objMapInfo.FullName;
                
                ////bingingPoints.BindPoints.Add (bp);
                //if (this.IsBindPointsVisible)
                //{
                //    bingingPoints.CreateCallouts (this);
                //}

                /*
                string path = Utils.ExeDirectory + "\\trace.txt";
                LightCom.MiP.Common.ClientTrack track = new LightCom.MiP.Common.ClientTrack ();
                FileStream fs = new FileStream (path, FileMode.Open);
                
                track.RestoreGuts (fs);
                fs.Close ();

                iUnknown = Marshal.GetIUnknownForObject (Map);
                GrymCore.IMapGraphics objMap = (GrymCore.IMapGraphics) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IMapGraphics));
                Marshal.Release (iUnknown);

                int i = 0;
                foreach (ObjectPosition op in track.Storage)
                {
                    if (i++ % 7 != 0)
                        continue;
                    MobileObjectGraphic obj = new MobileObjectGraphic (this);
                    obj.PointOnEarth.x = op.X;
                    obj.PointOnEarth.y = op.Y;
                    GlobalToMap (obj.PointOnEarth, obj.Point);
                    mobileObjects.Add (obj);
                    
                    objMap.AddGraphic (obj);
                }
                */
                
                //while (Marshal.ReleaseComObject (objMap) > 0);

                playerForm = new PlayerForm (this);
                playerForm.Show (new LightCom.MiP.Dispatcher.Common.Win32WindowHandle (new IntPtr (Map.HWindow)));

                //clientListForm = new MobileObjectList (this);
                //clientListForm.Show (new LightCom.MiP.Dispatcher.Common.Win32WindowHandle (new IntPtr (Map.HWindow)));

            }

            GrymCore.IPopupMenu contextMenu = Map.ContextMenu;
            SubMenu = contextMenu.AddSubMenu (Properties.Resources.MenuGroupName,
                    0,
                    Properties.Resources.MenuSubMenuName);
            //addBindPointCommand = new AddBindPointCommand (this);
            //SubMenu.AddCommand (addBindPointCommand);
            
            }
            catch (Exception e)
            {
                MessageBox.Show (e.ToString (), Properties.Resources.PluginName);
            }
        }

        /// <summary>
        /// Обработчик закрытия карты
        /// </summary>
        public void Terminate ()
        {
            playerForm.Close ();
            MapDx = MapDx;
            MapDy = MapDy;
            Settings.Flush ();
            foreach (MobileObjectGraphic obj in mobileObjects)
            {
                obj.Dispose ();
            }
            mobileObjects.Clear ();

            Root = null;
            BaseView = null;
            Map = null;
            SubMenu = null;
            Device = null;
            //bingingPoints.Save ();
            //bingingPoints.Reset ();            

            System.GC.Collect ();
        }

        #endregion
        #region IGrymPluginInfo Members

        public string Copyright
        {
            get
            {
                return Properties.Resources.PluginCopyright;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.PluginDescription;
            }
        }

        public int Icon
        {
            get
            {
                try
                {
                    Icon icon = Properties.Resources.PluginIcon;
                    return icon.Handle.ToInt32 ();
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        public string Name
        {
            get
            {
                return Properties.Resources.PluginName;
            }
        }

        #endregion
        #region IGrymPluginOptions Members

        /// <summary>
        /// Отображает диалог свойств плагина
        /// </summary>
        /// <returns></returns>
        bool GrymCore.IGrymPluginOptions.OptionDialog ()
        {
            //try
            //{
            //    PluginPropertiesForm dlg = new PluginPropertiesForm (this);

            //    bool oldVal = this.IsBindPointsVisible;
            //    DialogResult res = dlg.ShowDialog ();

            //    if (oldVal != this.IsBindPointsVisible)
            //    {
            //        if (!this.IsBindPointsVisible)
            //        {
            //            foreach (BindPoint bp in BingingPoints.BindPoints)
            //            {
            //                bp.RemoveCallout ();
            //            }
            //        }
            //        else
            //        {
            //            foreach (BindPoint bp in BingingPoints.BindPoints)
            //            {
            //                bp.CreateCallout ();
            //            }
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show (e.ToString (), Properties.Resources.PluginName);
            //}

            return true;
        }
        #endregion
  
        /// <summary>
        /// Преобразование географических координат в координаты на карте
        /// </summary>
        /// <param name="globalPoint">Географические координаты</param>
        /// <param name="mapPoint">Координаты на карте</param>
        /// <returns>true, если преобразование прошло успешно</returns>
        public bool GlobalToMap (GlobalPoint globalPoint, GrymCore.IMapPoint mapPoint)
        {
            mapPoint.Set (globalPoint.x, globalPoint.y);

            if (null == transformer)
            {
                IntPtr iUnknown = Marshal.GetIUnknownForObject (this.Map.CoordinateTransformation);
                transformer = (GrymCore.IMapCoordinateTransformationGeo) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IMapCoordinateTransformationGeo));
                Marshal.Release (iUnknown);
            }

            GrymCore.IMapPoint tmp = transformer.GeoToLocal (mapPoint);
            mapPoint.X = tmp.X;
            mapPoint.Y = tmp.Y;

            return true;
        }

        /// <summary>
        /// Удаление всех мобильных объектов с карты
        /// </summary>
        public void RemoveMobileObjects ()
        {
            IntPtr iUnknown = Marshal.GetIUnknownForObject (this.Map);
            GrymCore.IMapGraphics objMap = (GrymCore.IMapGraphics) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IMapGraphics));
            Marshal.Release (iUnknown);

            foreach (MobileObjectGraphic mog in MobileObjects)
            {
                objMap.RemoveGraphic (mog);
                mog.Dispose ();
            }
            MobileObjects.Clear ();
        }

        /// <summary>
        /// Добавление на крату мобильных объектов
        /// </summary>
        public void CreateMobileObjects ()
        {
            IntPtr iUnknown = Marshal.GetIUnknownForObject (this.Map);
            GrymCore.IMapGraphics objMap = (GrymCore.IMapGraphics) Marshal.GetTypedObjectForIUnknown (iUnknown, typeof (GrymCore.IMapGraphics));
            Marshal.Release (iUnknown);

            foreach (MobileObjectGraphic mog in MobileObjects)
            {
                objMap.AddGraphic (mog);
            }
        }

    }
}
