///////////////////////////////////////////////////////////////////////////////
//
//  File:           ClientTrack.cs
//
//  Facility:       Хранение трека клиента в потоке
//                  
//
//
//  Abstract:       Расширяет класс ObjectPositions возможностью загружать
//                  из потока множество объектов ObjectPositions.
//
//
//  Environment:    MSVC# 2005
//
//  Author:         Зайцев С.А.
//
//  Creation Date:  15-Nov-2006
//
//  Copyright (C) OOO "ЛайтКом", 2006. Все права защищены.
//
///////////////////////////////////////////////////////////////////////////////

/*
 * $History: ClientTrack.cs $
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:22
 * Updated in $/LightCom/.NET/MiP/Common
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 4.03.07    Time: 16:02
 * Updated in $/LightCom/.NET/MiP/Mip.Common
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 4.03.07    Time: 15:54
 * Updated in $/LightCom/.NET/MiP/Common
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:07
 * Updated in $/gps/Common
 * Устранен вечный цикл в коде загрузки данных из потока
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:47
 * Created in $/gps/Common
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using LightCom.Common;

namespace LightCom.MiP.Common
{
    /// <summary>
    /// Расширяет класс ObjectPositions возможностью загружать
    //  из потока множество объектов ObjectPositions.
    /// </summary>
    public class ClientTrack: ObjectPositions
    {
        /// <summary>
        /// Загружает из потока множество объектов класса ObjectPositions
        /// </summary>
        /// <param name="stream">Поток для чтения</param>
        /// <returns>Всегда возвращает true</returns>
        public new bool RestoreGuts (Stream stream)
        {
            //ObjectPositions positions = new ObjectPositions ();
            Reset ();
            //27,48
            int oldCount = 0;
            while (RestoreGuts (stream, this.Storage))
            {
                if (oldCount == this.Count)
                    break;

                oldCount = this.Count;
            }

            return true;
        }
    }
}