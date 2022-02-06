using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing;
using System.Windows.Forms;

namespace GisPlugin
{
    [Guid ("5BCB4DF9-41A3-4bc0-A2DF-9DB89419C6C4"),
       ClassInterface (ClassInterfaceType.None)]
    [ComVisible (true)]
    public class MiPPlugin: IDisposable, GrymCore.IGrymPlugin, GrymCore.IGrymPluginInfo, GrymCore.IGrymPluginOptions
    {
        public MiPPlugin ()
        {  
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
                    // Dispose managed resources.                    
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.
                this.Root = null;
                this.BaseView = null;
                this.Map = null;
            }
            disposed = true;
        }


        ~MiPPlugin ()
        {
            Dispose (false);
        }
#endregion

        #region "Properties"
        private GrymCore.Grym root = null;
        public GrymCore.Grym Root
        {
            get
            {
                return root;    
            }
            set 
            {
                if (root != null)
                {
                    while (Marshal.ReleaseComObject (root) > 0);
                }
                root = value;
            }
        }

        private GrymCore.IBaseViewThread baseView = null;
        public GrymCore.IBaseViewThread BaseView
        {
            get
            {
                return baseView;
            }
            set
            {
                if (baseView != null)
                {
                    while (Marshal.ReleaseComObject (baseView) > 0);
                }
                baseView = value;
            }
        }

        private GrymCore.IMap map = null;
        public GrymCore.IMap Map
        {
            get
            {
                return map;
            }
            set
            {
                if (map != null)
                {
                    while (Marshal.ReleaseComObject (map) > 0)
                        ;
                }
                map = value;
            }
        }
        #endregion

        #region IGrymPlugin Members

        public void Initialize (GrymCore.Grym pRoot, GrymCore.IBaseViewThread pBaseView)
        {
            Root = pRoot;
            BaseView = pBaseView;
            Map = pBaseView.Map;
            GrymCore.IMapRect rect = Map.FullExtent;
            string strMsg = string.Format ("{0}:{1} - {2}:{3}", rect.MinX, rect.MinY, rect.MaxX, rect.MaxY);
            MessageBox.Show (strMsg);

            while (Marshal.ReleaseComObject (rect) > 0);

        }

        public void Terminate ()
        {
            Root = null;
            BaseView = null;
            Map = null;
        }

        #endregion

        #region "Registration"
        //-------------------------------------------------
        // Регистрация в COM-категории "2Gis Grym Plugins"

        // CATID категории "2Gis Grym Plugins"
        const String GCAT = "{687FCBEC-C7A2-4D4C-B79B-DA650E7C29F7}";

        // Вызывается при регистрации класса в качестве COM-сервера
        [ComRegisterFunctionAttribute]
        public static void AppendInto2GisCat(String key)
        {
            // Добавляем запись в реестр.
            StringBuilder sb = new StringBuilder(key);
            sb.Replace("HKEY_CLASSES_ROOT\\", "");
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);
            RegistryKey ic = rk.OpenSubKey("Implemented Categories", true);
            ic.CreateSubKey(GCAT);
            ic.Close();
            rk.Close();
        }

        // Вызывается при удалении регистрации COM-сервера
        [ComUnregisterFunctionAttribute]
        public static void RemoveFrom2GisCat(String key)
        {
            // Удаляем запись из реестра.
            StringBuilder sb = new StringBuilder(key);
            sb.Replace("HKEY_CLASSES_ROOT\\", "");
            RegistryKey rk = Registry.ClassesRoot.OpenSubKey(sb.ToString(), true);
            RegistryKey ic = rk.OpenSubKey("Implemented Categories", true);
            ic.DeleteSubKey(GCAT);
            ic.Close();
            rk.Close();
        }
        #endregion

        #region IGrymPluginInfo Members

        public string Copyright
        {
            get
            {
                return "OOO ЛайтКом 2007";
            }
        }

        public string Description
        {
            get
            {
                return "Модуль МиП предназначен для отображения на карте города мобильных объектов.";
            }
        }

        
        public int Icon
        {
            get
            {
                try
                {
                    Icon icon = new Icon (typeof (MiPPlugin), "PluginIcon.ico");
                    return icon.Handle.ToInt32 ();
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public string Name
        {
            get
            {
                return "МиП";
            }
        }

        #endregion
        #region IGrymPluginOptions Members

        bool GrymCore.IGrymPluginOptions.OptionDialog ()
        {
            MessageBox.Show ("Plugin Options");
            return true;
        }

        #endregion
    }
}
