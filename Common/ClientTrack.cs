///////////////////////////////////////////////////////////////////////////////
//
//  File:           ClientTrack.cs
//
//  Facility:       �������� ����� ������� � ������
//                  
//
//
//  Abstract:       ��������� ����� ObjectPositions ������������ ���������
//                  �� ������ ��������� �������� ObjectPositions.
//
//
//  Environment:    MSVC# 2005
//
//  Author:         ������ �.�.
//
//  Creation Date:  15-Nov-2006
//
//  Copyright (C) OOO "�������", 2006. ��� ����� ��������.
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
 * �������� ������ ���� � ���� �������� ������ �� ������
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
    /// ��������� ����� ObjectPositions ������������ ���������
    //  �� ������ ��������� �������� ObjectPositions.
    /// </summary>
    public class ClientTrack: ObjectPositions
    {
        /// <summary>
        /// ��������� �� ������ ��������� �������� ������ ObjectPositions
        /// </summary>
        /// <param name="stream">����� ��� ������</param>
        /// <returns>������ ���������� true</returns>
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