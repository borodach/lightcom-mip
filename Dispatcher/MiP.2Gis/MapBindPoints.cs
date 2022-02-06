///////////////////////////////////////////////////////////////////////////////
//
//  File:           MapBindPoints.cs
//
//  Facility:       Точки привязки в 2Gis
//
//
//  Abstract:       Хранение точек привязки для одной карты
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
* $History: MapBindPoints.cs $
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
using System.IO;
using LightCom.Common;

namespace LightCom.MiP.Dispatcher.Plugin2Gis
{
    /// <summary>
    /// Хранение точек привязки для одной карты
    /// </summary>
    public class MapBindPoints: IDisposable
    {

        /// <summary>
        /// Ссылка на плагин
        /// </summary>
        MiPPlugin plugin;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="plugin">Объект плагина</param>
        public MapBindPoints (MiPPlugin mipPlugin)
        {
            plugin = mipPlugin;
            this.Description = String.Empty;
            this.Tag = String.Empty;
            this.StoreFolder = Utils.ExeDirectory + "\\Bindings\\";
            bindPoints = new List<BindPoint> ();
        }

        #region "IDisposable implementation"
        ///
        /// Track whether Dispose has been called.
        /// 
        private bool disposed = false;

        /// <summary>
        /// Implement IDisposable.
        /// Do not make this method virtual.
        /// A derived class should not be able to override this method.
        /// </summary>
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
        
        /// <summary>
        /// Dispose(bool disposing) executes in two distinct scenarios.
        /// Managed and unmanaged resources can be disposed.
        /// If disposing equals false, the method has been called by the 
        /// runtime from inside the finalizer and you should not reference 
        /// other objects. Only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">If disposing equals true, the method has 
        /// been called directly or indirectly by a user's code. 
        /// </param>
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

                    this.Reset ();
                }

                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // If disposing is false, 
                // only the following code is executed.
                
            }
            disposed = true;
        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~MapBindPoints ()
        {
            Dispose (false);
        }
        #endregion;

        /// <summary>
        /// Создание графических объектов для всех точек привязки
        /// </summary>
        /// <param name="plugin">Плагин</param>
        public void CreateCallouts (MiPPlugin plugin)
        {
            foreach (BindPoint bp in this.BindPoints)
            {
                bp.CreateCallout ();
            }
        }

        /// <summary>
        /// Удаление графических объектов всех точек привязки
        /// </summary>
        /// <param name="plugin">Плагин</param>
        public void RemoveCallouts (MiPPlugin plugin)
        {
            foreach (BindPoint bp in this.BindPoints)
            {
                bp.RemoveCallout ();
            }
        }

        /// <summary>
        /// Очистка объекта
        /// </summary>
        public void Reset ()
        {
            this.Description = String.Empty;
            this.Tag = String.Empty;
            this.StoreFolder = Utils.ExeDirectory + "\\Bindings\\";

            foreach (BindPoint bp in this.BindPoints)
            {
                bp.Dispose ();
            }

            this.BindPoints.Clear ();
        }

        /// <summary>
        /// Сохранение точек привязки в файле
        /// </summary>
        /// <param name="fileName">Имя файла для записи</param>
        /// <returns>true, если запись прошла успешно</returns>
        public bool Save (string fileName)
        {
            StreamWriter sw = null;
            try
            {
                FileStream fs = new FileStream (fileName, FileMode.Create);
                sw = new StreamWriter (fs, System.Text.Encoding.Default);
                sw.WriteLine (this.Tag);
                sw.WriteLine (this.Description);
                sw.WriteLine (this.BindPoints.Count);
                foreach (BindPoint bp in this.BindPoints)
                {
                    if (!bp.SaveGuts (sw))
                    {
                        return false;
                    }
                }
                sw.Close ();

                return true;
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                if (sw != null) sw.Close ();
            }
        }

        /// <summary>
        /// Сохранение точек привязки в файле
        /// </summary>
        /// <returns>true, если запись прошла успешно</returns>
        public bool Save ()
        {
            try
            {
                Directory.CreateDirectory (this.StoreFolder);
                return Save (this.FileName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Загрузка из файла
        /// </summary>
        /// <returns>true, если загруска прошла успешно</returns>
        public bool Load ()
        {
            return Load (FileName);
        }

        /// <summary>
        /// Загрузка из файла
        /// </summary>
        /// <param name="fileName">Имя файла для загрузки</param>
        /// <returns>true, если загруска прошла успешно</returns>
        public bool Load (string fileName)
        {
            Reset ();
            StreamReader sr = null;
            try
            {
                FileStream fs = new FileStream (fileName, FileMode.Open);
                sr = new StreamReader (fs, System.Text.Encoding.Default);
                this.Tag = sr.ReadLine ().Trim ();
                this.Description = sr.ReadLine ().Trim ();
                int count = Convert.ToInt32 (sr.ReadLine ().Trim ());
                this.BindPoints.Capacity = count;
                for (int idx = 0; idx < count; ++idx)
                {
                    BindPoint bp = new BindPoint (plugin);
                    if (!bp.RestoreGuts (sr))
                    {
                        Reset ();
                        return false;
                    }
                    this.BindPoints.Add (bp);
                }

                return true;
            }
            catch (Exception e)
            {
                Reset ();
                return false;
            }
            finally
            {
                if (sr != null) sr.Close ();
            }
        }

        #region "Properties"

        /// <summary>
        /// Идентификатор карты
        /// </summary>
        private string strMapTag;

        /// <summary>
        /// Идентификатор карты
        /// </summary>
        public string Tag
        {
            get
            {
                return this.strMapTag;
            }
            set
            {
                this.strMapTag = value.Replace (System.Environment.NewLine, " ");
            }
        }

        /// <summary>
        /// Описание карты
        /// </summary>
        private string strMapDescription;
        public string Description
        {
            get
            {
                return this.strMapDescription;
            }
            set
            {
                this.strMapDescription = value.Replace (System.Environment.NewLine, " ");
            }
        }

        /// <summary>
        /// Папка для хранения точек привязки
        /// </summary>
        private string strMapStoreFolder;

        /// <summary>
        /// Папка для хранения точек привязки. В конце должен быть "\\".
        /// </summary>
        public string StoreFolder
        {
            get
            {
                return this.strMapStoreFolder;
            }
            set
            {
                this.strMapStoreFolder = value;
            }
        }

        /// <summary>
        /// Точки привязки
        /// </summary>
        private List <BindPoint> bindPoints;

        /// <summary>
        /// Точки привязки
        /// </summary>
        public List<BindPoint> BindPoints
        {
            get
            {
                return this.bindPoints;
            }
        }

        /// <summary>
        /// Имя файла для схранения карты
        /// </summary>
        public string FileName
        {
            get
            {
                return string.Format ("{0}{1}.txt", this.StoreFolder, this.Tag);
            }
        }
        #endregion;
    }
}
