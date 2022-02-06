////////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           GPSTransmitter.cs
//
//  Facility:       ����������� ������ ��������� ������.
//
//
//  Abstract:       ����� ��������� �������� ������-������: ������ ������
//                  �� GPS ��������� � �������� �� �� ������.
//
//  Environment:    VC# 7.1
//
//  Author:         ������ �. �.
//
//  Creation Date:  19/09/2005
//
////////////////////////////////////////////////////////////////////////////////

/*
 * $History: GPSTransmitter.cs $
 * 
 * *****************  Version 12  *****************
 * User: Sergey       Date: 10.07.07   Time: 7:59
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 11  *****************
 * User: Sergey       Date: 25.06.07   Time: 8:29
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 10  *****************
 * User: Sergey       Date: 4.03.07    Time: 17:02
 * Updated in $/LightCom/.NET/MiP/CEClient
 * 
 * *****************  Version 9  *****************
 * User: Sergey       Date: 4.03.07    Time: 10:51
 * Updated in $/gps/EndPoint
 * 
 * *****************  Version 8  *****************
 * User: Sergey       Date: 18.11.06   Time: 20:21
 * Updated in $/gps/EndPoint
 * �������� ���������� �����, ����� RMC ������ �������� �� �����
 * 
 * *****************  Version 7  *****************
 * User: Sergey       Date: 18.11.06   Time: 19:04
 * Updated in $/gps/EndPoint
 * ��������� �������� Publisher
 * 
 * *****************  Version 6  *****************
 * User: Sergey       Date: 18.11.06   Time: 14:39
 * Updated in $/gps/EndPoint
 * 
 */

using System;
using System.Threading;
using System.IO;
using System.Text;
using System.Windows.Forms;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.Gps;

namespace LightCom.MiP.CEClient
{

    /// 
    /// <summary>
    /// ����� ��������� �������� ������-������: ������ ������ �� GPS ��������� � 
    /// �������� �� �� ������.
    /// </summary>
    ///

    public abstract class GPSTransmitter : GPSListener
    {

#if DEBUG1
        string [] testData;
        int testIdx = 0;
#endif


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="publisher">������ �� ������, ����������� ���������� ��������
        /// </param>
        internal GPSTransmitter (IPublisher publisher)
        {
            this.m_Publisher = publisher;
            this.Reset ();

#if DEBUG1
            try
            {
                FileStream fs = File.OpenRead (Utils.ExeDirectory + "\\testdata.txt");
                StreamReader sr = new StreamReader (fs);
                string data = sr.ReadToEnd ();
                char [] delimiters = { '\n' };
                testData = data.Split (delimiters);
                for (int i = 0; i < testData.Length; ++i)
                {
                    testData [i] = testData [i].Trim ();
                }
            }
            catch (Exception)
            {
            }
#endif
        }

        #region "ISettings methods";

        /// <summary>
        /// ���������� ��������� ������� � ��������� ���������.
        /// </summary>
        public override void Reset ()
        {
            this.m_MaxSendTimeOut = TimeSpan.FromMinutes (10);
            this.m_nMinSendTimeOut = 1000;
            this.m_LastSendTime = DateTime.MinValue;
            this.m_strLastGPSInfo = "";
            this.m_nWaitTimeout = 10000;
            this.m_nMaxSize = 1;
            this.m_strClientId = "Undefined";
        }

        /// <summary>
        ///  ������� ��������� ��� ���������� ��������
        /// </summary>
        internal const string storageCategery = "Transmitter";

        /// <summary>
        /// ��������� ��������� ������� � �������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ����������.</param>
        /// <returns>���������� true, ���� ���������� ������ �������.
        /// </returns>
        public override bool Save (SettingsStorage storage)
        {
            bool bResult = storage.Write (storageCategery, "MaxSendTimeOut", m_MaxSendTimeOut);
            bResult &= storage.Write (storageCategery, "MinSendTimeOut", m_nMinSendTimeOut);

            bResult &= storage.Write (storageCategery, "WaitTimeout", m_nWaitTimeout);
            bResult &= storage.Write (storageCategery, "MaxQueueSize", m_nMaxSize);
            bResult &= storage.Write (storageCategery, "ClientId", m_strClientId);

            return bResult;
        }

        /// <summary>
        /// ��������� ��������� ������� �� ��������� ���������.
        /// </summary>
        /// <param name="storage">��������� ���������� ����������.</param>
        /// <returns>���������� true, ���� �������� ������ �������.
        /// </returns>
        public override bool Load (SettingsStorage storage)
        {
            this.Reset ();
            TimeSpan defMaxSendTimeOut = m_MaxSendTimeOut;
            storage.Read (storageCategery, "MaxSendTimeOut", out m_MaxSendTimeOut, defMaxSendTimeOut);
            int defMinSendTimeOut = m_nMinSendTimeOut;
            storage.Read (storageCategery, "MinSendTimeOut", out m_nMinSendTimeOut, defMinSendTimeOut);

            int defWaitTimeout = m_nWaitTimeout;
            storage.Read (storageCategery, "WaitTimeout", out m_nWaitTimeout, defWaitTimeout);

            int defMaxSize = m_nMaxSize;
            storage.Read (storageCategery, "MaxQueueSize", out m_nMaxSize, defMaxSize);

            string defClientId = m_strClientId;
            storage.Read (storageCategery, "ClientId", out this.m_strClientId, defClientId);

            return true;
        }
        #endregion

        #region "PipeLine methods";

        /// <summary>
        /// ��������� �������� ���������.
        /// ����� ����������� � ��������� ������.
        /// </summary>
        /// <param name="obj">�������������� �������.</param>
        protected override void ProcessItem (object obj)
        {
            if (!(obj is string)) return;
            string strValue = obj as string;

            DateTime timeToSend = this.m_LastSendTime + this.m_MaxSendTimeOut;


            double fLongitude, fLatitude, fSpeed, fCourse;
            DateTime fixTime;
            if (!LightCom.Gps.GPSReader.GetPosition (strValue,
                                                      out fLongitude,
                                                      out fLatitude,
                                                      out fSpeed,
                                                      out fCourse,
                                                      out fixTime))
            {
                return;
            }


            double fLastLongitude, fLastLatitude, fLastSpeed, fLastCourse;
            DateTime lastFixTime;
            if (!LightCom.Gps.GPSReader.GetPosition (m_strLastGPSInfo,
                                                      out fLastLongitude,
                                                      out fLastLatitude,
                                                      out fLastSpeed,
                                                      out fLastCourse,
                                                      out lastFixTime))
            {
                fLastLongitude = fLastLatitude = fLastSpeed = fLastCourse = -1;
            }

            if (timeToSend > DateTime.Now)
            {
                if (fLastLongitude == fLongitude && fLastLatitude == fLatitude)
                {
                    //
                    // ������ ���������� �� �����.
                    //

                    return;
                }
            }

            this.m_LastSendTime = DateTime.Now;
            this.m_strLastGPSInfo = strValue;

            Stream receiveStream;
            byte [] data;

            ClientRequestCodec codec = new ClientRequestCodec ();
            codec.ClientId = this.ClientId;
            codec.GPSInfo = this.m_strLastGPSInfo;
            codec.ProductVersion = m_strProductVersion;
            data = codec.Encode ();

            this.m_Publisher.UserAgent = m_strProductName;
            if (!this.m_Publisher.Publish (data, out receiveStream))
            {
                this.PutBack (obj);
                this.PublisherState = State.Error;
            }
            else
            {
                this.PublisherState = State.OK;
                this.ProcessResponse (receiveStream);
                receiveStream.Close ();

            }

            Thread.Sleep ((int) this.m_nMinSendTimeOut);
        }

        #endregion;


        /// <summary>
        /// ��������� ������ �������.
        /// </summary>
        /// <param name="response">����� �������.</param>
        /// <returns>���������� true, ���� ��������� ������ �������.</returns>
        virtual protected bool ProcessResponse (Stream receiveStream)
        {
            if (null == receiveStream) return true;
            //if (0 == response.Length) return true;

            string strCode;

            try
            {
                //
                // Read command.
                //

                strCode = "";
                do
                {
                    int nByte = receiveStream.ReadByte ();
                    if (-1 == nByte || ((int) '\n') == nByte) break;
                    strCode += (char) nByte;
                }
                while (true);

                //
                // Process 'execute' command.
                //

                if ("execute" == strCode)
                {
                    string strFileName = "";
                    do
                    {
                        int nByte = receiveStream.ReadByte ();
                        if (-1 == nByte || ((int) '\n') == nByte) break;
                        strFileName += (char) nByte;
                    }
                    while (true);

                    string strCurDir = Utils.ExeDirectory;

                    strCurDir += "\\";
                    strFileName = strCurDir + strFileName;

                    FileStream fileStream = new FileStream (strFileName,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None,
                        4096);
                    do
                    {
                        int nByte = receiveStream.ReadByte ();
                        if (-1 == nByte) break;
                        fileStream.WriteByte ((byte) nByte);
                    }
                    while (true);

                    fileStream.Flush ();
                    fileStream.Close ();
                    receiveStream.Close ();

                    string strCommandLine = strFileName;

                    if (LightCom.WinCE.API.CreateProcess (strCommandLine))
                    {
                        MainForm.form.Close ();
                        //Application.Exit ();
                    }
                    else
                    {
                        File.Delete (strFileName);
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// 
        /// <summary>
        /// ������ ��������.
        /// </summary>
        ///

        internal const string m_strProductVersion = "1.0";

        /// 
        /// <summary>
        /// �������� ��������.
        /// </summary>
        ///

        internal const string m_strProductName = "mip";

        /// 
        /// <summary>
        /// Copyright string.
        /// </summary>
        ///

        public const string m_strProductCopyright = "Copyright 2005. Zaitsev S.A. Russia Federation, Berdsk. All rights reserved.";

        /// 
        /// <summary>
        /// ��������� ������, ���������� �� GPS ���������.
        /// </summary>
        /// 

        protected string m_strLastGPSInfo;

        /// 
        /// <summary>
        /// ����� ��������� �������� ������ �� ������.
        /// </summary>
        ///

        protected DateTime m_LastSendTime;

        /// 
        /// <summary>
        /// �������� �������� �������� � �����������, ���� ��� �� ����������.
        /// </summary>
        /// 

        protected TimeSpan m_MaxSendTimeOut;

        /// 
        /// <summary>
        /// �������� �������� ������ �� ������.
        /// </summary>
        /// 

        protected int m_nMinSendTimeOut;

        /// 
        /// <summary>
        /// ������������� �������.
        /// </summary>
        /// 

        public string ClientId { get { return this.m_strClientId; } set { this.m_strClientId = value; } }
        protected string m_strClientId;

        /// 
        /// <summary>
        /// ������ �� publisher.
        /// </summary>
        /// 

        public IPublisher Publisher
        {
            get
            {
                return m_Publisher;
            }
            set
            {
                m_Publisher = value;
            }
        }
        protected IPublisher m_Publisher;


    }
}
