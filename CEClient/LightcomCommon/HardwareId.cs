////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           HardwareId.cs
//
//  Facility:       ������ ����������� ����������� ������ ����������.
//
//
//  Abstract:       ������ ������ ��� ������ presetId � platformId ����������.
//
//  Environment:    VC# 8
//
//  Author:         ������ �. �.
//
//  Creation Date:  03/06/2006
//
//  Copyright (C) OOO "�������", 2005-2006. ��� ����� ��������.
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: HardwareId.cs $
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:33
 * Updated in $/LightCom/.NET/Agro/Agro/LightcomCommon
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 4.03.07    Time: 11:09
 * Created in $/GPSTracing.root/Src/LightcomCommon
 * 
 * *****************  Version 2  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using LightCom.WinCE;

namespace LightCom.WinCE
{
    class HardwareId
    {
       
        protected const int MANUFACTUREID_INVALID  = 0x01;
        protected const int SERIALNUM_INVALID      = 0x02;
        protected const int IOCTL_HAL_GET_DEVICEID = 0x01010054;
        
        /// <summary>
        /// ������ ��������� ���������� �� ����������
        /// </summary>
        /// <param name="presetId">Preset ID bytes</param>
        /// <param name="platformId">Platform ID bytes</param>
        /// <returns>true, ���� ������ �������� �������</returns>
        public static bool GetDeviceID (out byte [] presetId, 
                                        out byte [] platformId)
        {
            presetId = null;
            platformId = null;
            try
            {
                byte [] buffer =new byte [256];
                int nBufferSize = buffer.Length;
                byte [] bytes = BitConverter.GetBytes (nBufferSize);
                bytes.CopyTo (buffer, 0);

                //
                // Request device ID using szBuffer
                //
                
                int dwReturned = 0;
                if (! API.KernelIoControl (IOCTL_HAL_GET_DEVICEID, 
                                           null, 
                                           0, 
                                           buffer, 
                                           buffer.Length, 
                                           out dwReturned))
                {
                    return false;
                }

                int dwPresetIDOffset   = BitConverter.ToInt32 (buffer, 4);
                int dwPresetIDBytes    = BitConverter.ToInt32 (buffer, 8);
                int dwPlatformIDOffset = BitConverter.ToInt32 (buffer, 12);
                int dwPlatformIDBytes  = BitConverter.ToInt32 (buffer, 16);

                presetId = new byte [dwPresetIDBytes]; 
                platformId = new byte [dwPlatformIDBytes];
                int idx = 0;
                for (idx = 0; idx < dwPresetIDBytes; ++ idx)
                {
                    presetId [idx] = buffer [idx + dwPresetIDOffset];
                }
                for (idx = 0; idx < dwPlatformIDBytes; ++ idx)
                {
                    platformId [idx] = buffer [idx + dwPlatformIDOffset];
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
