////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           HardwareKey.cs
//
//  Facility:       �������������� � ���������� ���������.
//
//
//  Abstract:       ����� ��� ������ � ������ ��������, ����������� � ����������.
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
 * $History: HardwareKey.cs $
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 30.04.07   Time: 20:57
 * Updated in $/LightCom/.NET/Agro/Agro/LightcomCommon
 * 
 * *****************  Version 5  *****************
 * User: Sergey       Date: 30.03.07   Time: 21:18
 * Updated in $/LightCom/.NET/Agro/Agro/LightcomCommon
 * 
 * *****************  Version 4  *****************
 * User: Sergey       Date: 4.03.07    Time: 12:33
 * Updated in $/LightCom/.NET/Agro/Agro/LightcomCommon
 * 
 * *****************  Version 1  *****************
 * User: Sergey       Date: 4.03.07    Time: 11:09
 * Created in $/GPSTracing.root/Src/LightcomCommon
 * 
 * *****************  Version 3  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:37
 * Updated in $/gps/Lightcom.Common
 * 
 */

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace LightCom.WinCE
{
    /// <summary>
    /// ����� ��� ������ � ������ ��������, ����������� � ����������.
    /// </summary>
    public class HardwareKey
    {
        /// <summary>
        /// ������-����������� ��� �������� ����
        /// </summary>
        private string KeySeparator = "\n";

        /// <summary>
        /// ������������� �������� ��������
        /// </summary>
        public enum LicensePropertyId
        {
            lpPlatformId,           // ����� ����������� �����
            lpPresetId,             // ����� ����������� �����
            lpLicenseOwner,         // �������� ��������
            lpDistributorName,      // ��� �������������
            lpDistributorArea,      // ������ �������������
            lpHash,                 // ���
            lpNumber,               // ����� ��������
            lpMaxValue              // ������������ ��������
        };

        /// <summary>
        /// ������� ������ �����
        /// </summary>
        public const int currentVersion = 2;

        /// <summary>
        /// �����������
        /// </summary>
        public HardwareKey ()
        {
            licenseProperties = new SortedList <LicensePropertyId, string> ();
            Reset ();
        }

        /// <summary>
        /// ������� �������
        /// </summary>
        public void Reset ()
        {
            this.Properties.Clear ();
        }

        /// <summary>
        /// ���������� �������� � ���� strFileName
        /// </summary>
        /// <param name="strFileName">��� ����� ��������</param>
        /// <param name="strCustomKey">������������ �����, ������������� ��������</param>
        /// <returns>true, ���� ���� ��� ������� ��������</returns>
        internal bool SaveLicense (string strFileName, string strCustomKey)
        {
            try
            {
                StreamWriter sw = new StreamWriter (strFileName);
                SaveLicense(sw, strCustomKey);

                sw.Close ();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ���������� �������� � �������� �����
        /// </summary>
        /// <param name="sw">�����, � ������� ������������ ��������</param>
        /// <param name="strCustomKey">������������ �����, ������������� ��������</param>
        public void SaveLicense(StreamWriter sw, string strCustomKey)
        {
            string strKey = string.Empty;

            sw.WriteLine (currentVersion);
            int count = this.Properties.Count;
            sw.WriteLine (count);
            for (int idx = 0; idx < count; ++idx)
            {
                LicensePropertyId key = this.Properties.Keys [idx];
                string val = ((int) key).ToString ();
                sw.WriteLine (val);
                strKey += val + this.KeySeparator;

                val = this.Properties [key];
                sw.WriteLine (val);
                strKey += val + this.KeySeparator;
            }
            strKey += strCustomKey;

            MD5 md5 = new MD5CryptoServiceProvider ();
            byte [] bytes = UnicodeEncoding.Unicode.GetBytes (strKey);
            byte [] hash = md5.ComputeHash (bytes);
            sw.Write (Convert.ToBase64String (hash));
        }

        /// <summary>
        /// ��������� �������� �� ����� strFileName
        /// </summary>
        /// <param name="strFileName">��� ����� ��������</param>
        /// <param name="strCustomKey">������������ �����, ������������� ��������</param>
        /// <returns>true, ���� ���� ��� ������� ��������</returns>
        public bool LoadLicense (string strFileName, string strCustomKey)
        {
            StreamReader sr = null;
            try
            {
                sr = new StreamReader (strFileName);
                return LoadLicense(sr, strCustomKey);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (sr != null) sr.Close ();
            }

            Reset ();

            return false;
        }

        /// <summary>
        /// ��������� �������� �� ������ sr
        /// </summary>
        /// <param name="sr">����� ��� �������� ��������</param>
        /// <param name="strCustomKey">������������ �����, ������������� ��������</param>
        /// <returns>true, ���� �������� ������� ���������</returns>
        private bool LoadLicense(StreamReader sr, string strCustomKey)
        {
            string strKey = string.Empty;
            Reset ();
            int version = Convert.ToInt32 (sr.ReadLine ());
            int count = Convert.ToInt32 (sr.ReadLine ());
            if (count < 1) return false;
            for (int idx = 0; idx < count; ++idx)
            {
                string val = sr.ReadLine ();
                LicensePropertyId key = (LicensePropertyId) Convert.ToInt32(val);
                strKey += val + this.KeySeparator;
                val = sr.ReadLine ();
                strKey += val + this.KeySeparator;
                Properties.Add (key, val);
            }
            string strHash = sr.ReadLine ();

            strKey += strCustomKey;
            MD5 md5 = new MD5CryptoServiceProvider ();
            byte [] bytes = UnicodeEncoding.Unicode.GetBytes (strKey);
            byte [] hash = md5.ComputeHash (bytes);
            string strComputedHash = Convert.ToBase64String (hash);
            if (strComputedHash != strHash)
            {
                Reset ();
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Preset ID
        /// </summary>
        public string PresetId
        {
            get
            {
                string result;
                if (licenseProperties.TryGetValue (LicensePropertyId.lpPresetId, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                licenseProperties [LicensePropertyId.lpPresetId] = value;
            }
        }

        /// <summary>
        /// Platform ID
        /// </summary>
        public string PlatformId
        {
            get
            {
                string result;
                if (licenseProperties.TryGetValue (LicensePropertyId.lpPlatformId, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                licenseProperties [LicensePropertyId.lpPlatformId] = value;
            }
        }
        
        /// <summary>
        /// �������� ��������
        /// </summary>
        public string LicenseOwner
        {
            get
            {
                string result;
                if (licenseProperties.TryGetValue (LicensePropertyId.lpLicenseOwner, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                licenseProperties [LicensePropertyId.lpLicenseOwner] = value;
            }
        }

        /// <summary>
        /// ��� ������������
        /// </summary>
        public string DistributorName
        {
            get
            {
                string result;
                if (licenseProperties.TryGetValue (LicensePropertyId.lpDistributorName, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                licenseProperties [LicensePropertyId.lpDistributorName] = value;
            }
        }

        /// <summary>
        /// ������ ������������
        /// </summary>
        public string DistributorArea
        {
            get
            {
                string result;
                if (licenseProperties.TryGetValue (LicensePropertyId.lpDistributorArea, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                licenseProperties [LicensePropertyId.lpDistributorArea] = value;
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        //public string Hash
        //{
        //    get
        //    {
        //        string result;
        //        if (licenseProperties.TryGetValue (LicensePropertyId.lpHash, out result))
        //        {
        //            return result;
        //        }

        //        return null;
        //    }
        //    set
        //    {
        //        licenseProperties [LicensePropertyId.lpHash] = value;
        //    }
        //}

        /// <summary>
        /// ����� ��������
        /// </summary>
        public string Number
        {
            get
            {
                string result;
                if (licenseProperties.TryGetValue (LicensePropertyId.lpNumber, out result))
                {
                    return result;
                }

                return null;
            }
            set
            {
                licenseProperties [LicensePropertyId.lpNumber] = value;
            }
        }

        /// <summary>
        /// �������� ��������
        /// </summary>
        private SortedList<LicensePropertyId, string> licenseProperties;

        /// <summary>
        /// �������� ��������
        /// </summary>
        public SortedList<LicensePropertyId, string> Properties
        {
            get { return this.licenseProperties; }
        }
    }
}