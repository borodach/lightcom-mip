///////////////////////////////////////////////////////////////////////////////
//
//  File:           BaseDataSource.cs
//
//  Facility:       �������������� � ��������.
//
//
//  Abstract:       ������� ����� ��� ��������� � ������� ������ � ��������� 
//                  ��������.
//
//  Environment:    VC 7.1
//
//  Author:         ������ �.�.
//
//  Creation Date:  10-12-2005
//
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Text;
using System.IO;
using LightCom.Common;
using LightCom.MiP.Common;
using LightCom.MiP.Cache;

namespace LightCom.MiP.DataSource
{
/// 
/// <summary>
/// ������� ����� ��� ��������� � ������� ������ � ��������� ��������.
/// </summary>
/// 

public abstract class BaseDataSource: IGPSDataSource
{
/// 
/// <summary>
/// ��� �������, ������������ �� ������.
/// </summary>
/// 

public enum RequestType
{
    rtInvalid = 0,
    rtConnect,
    rtDisconnect,
    rtGetClientList,
    rtGetClientsInfo,
    rtGetEvents,
    rtReadGPSData,
    rtReadLatestGPSData,
    rtUpdateMobileClientInfo,
    rtMaxValue
};

/// 
/// <summary>
/// ��� ������ �������.
/// </summary>
/// 

public enum ResponseType
{
    rtInvalid = 0,
    rtOK,
    rtError,
    rtMaxValue
};

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������� ����� dataToEncrypt.
/// </summary>
/// <param name="dataToEncrypt">�����, ������� ����� �����������.</param>
/// <param name="key">����. ���� ����� null, �� ������ ������ 
/// ����������.</param>
/// <param name="encryptedData">������������� �����.</param>
/// <returns>true, ���� �������� ����������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool Encrypt (byte [] dataToEncrypt, 
                              byte [] key,
                              out byte [] encryptedData);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// �������������� ����� dataToDecrypt.
/// </summary>
/// <param name="dataToDecrypt">�����, ������� ����� ������������.</param>
/// <param name="key">����. ���� ����� null, �� ������ ������ 
/// ����������.</param>
/// <param name="decryptedData">�������������� �����.</param>
/// <returns>true, ���� �������� ����������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool Decrypt (byte [] dataToDecrypt, 
                              byte [] key,
                              out byte [] decryptedData);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ���� ��� ���������� ������.
/// </summary>
/// <param name="encryptionKey">���� ��� ����������.</param>
/// <param name="decryptionKey">���� ��� ������������.</param>
/// <returns>true, ���� �������� ����������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool GenerateKey (out byte [] encryptionKey, 
                                    out byte [] decryptionKey);

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������ �� ������
/// </summary>
/// <param name="dataToSend">����������� �� ������ ������.</param>
/// <param name="response">����� �������.</param>
/// <returns>true, ���� �������� ����������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public abstract bool Send (byte [] dataToSend, 
                            out Stream response);


////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// Default constructor.
/// </summary>
///
////////////////////////////////////////////////////////////////////////////////

public BaseDataSource ()
{
    ASCIIEncoding m_ASCIIEncoder = new ASCIIEncoding ();
    m_KeyToConnect = null;
    m_EncryptionKey = null;
    m_DecryptionKey = null;
    m_nConnectionId = m_nInvalidConnectionId;
    m_strLogin = string.Empty;
    m_strDomain = string.Empty;
    m_strPassword = string.Empty;
    m_strLastError = "OK";
    m_Random = new Random ();
    m_Version = 1;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� true, ���� ���������� �����������.
/// </summary>
/// <returns>���������� true, ���� ���������� �����������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool IsConnected  ()
{
    return m_nInvalidConnectionId != m_nConnectionId;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������� � ������� ��������� ������.
/// </summary>
/// <returns>���������� ������� � ������� ��������� ������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public string GetLastErrorText ()
{
    return m_strLastError;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ����������� �����.
/// </summary>
/// <param name="data">������, ��� ������� ����� ���������� ����������� 
/// �����.</param>
/// <returns>���������� ����������� �����.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public int CalculateCRC (byte [] data)
{
    return Utils.CalculateCRC (data);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� �������: �������� ���������� � �����������. ����� ������� ��� 
/// ��������� ������� ������� � �������� ��������.
/// </summary>
/// <param name="requestStream">�����, ���������� ������.</param>
/// <param name="keyToDecrypt">���� ��� �����������.</param>
/// <param name="decodedRequest">�������������� ������.</param>
/// <param name="type">��� �������.</param>
/// <param name="nRandomValue">��������� �����, ���������� � ������.</param>
/// <returns></returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool ReadRequestHeader (Stream requestStream, 
                               byte [] keyToDecrypt,
                               out Stream decodedRequest,
                               out RequestType type,                               
                               out int nRandomValue)
{
    type = RequestType.rtInvalid;
    decodedRequest = null;
    nRandomValue = 0;
    try
    {
        //
        //�������� ������ �������.
        //

        int nVersion = Utils.ReadInt (requestStream);
        if (nVersion > m_Version)
        {
            m_strLastError = "������ ��� ������ ������ �������: ���������������� ������ ������.";
            requestStream.Close ();
            return false;
        }

        //
        //������ ���� �������.
        //

        type = (RequestType) Utils.ReadInt (requestStream);

        //
        //������ ����������� �����.
        //

        int nCRC = Utils.ReadInt (requestStream);

        //
        //������ ������������� ������.
        //

        int nSize = Utils.ReadInt (requestStream);
        if (nSize < 0)
        {
            m_strLastError = "������ ��� ������ ������ �������: ������������ ������ ������.";
            requestStream.Close ();
            return false;
        }

        //
        //##������ ����� �������.
        //

        byte [] encryptedData = new byte [nSize];
        requestStream.Read (encryptedData, 0, nSize);        
        byte [] decryptedData;

        //
        //�������������� ������.
        //

        if ( ! Decrypt (encryptedData, keyToDecrypt, out decryptedData))
        {
            m_strLastError = "�� ������� ������������ ���������� ������.";
            requestStream.Close ();
            return false;    
        }
        requestStream.Close ();

        //
        //��������� ����������� �����.
        //

        int nCalculatedCRC = CalculateCRC (decryptedData);
        if (nCalculatedCRC != nCRC)
        {
            m_strLastError = "������ ��� ������ ������ �������: ������������ ����������� �����.";
                return false;    
        }

        decodedRequest = new MemoryStream (decryptedData, 
            0, 
            decryptedData.Length, 
            false);

        nRandomValue = Utils.ReadInt (decodedRequest);
    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ���������� ������ �� ������.
/// </summary>
/// <param name="type">�� �������.</param>
/// <param name="data">������, ������� ����� ��������.</param>
/// <param name="keyToEncrypt">���� ��� ���������� ������.</param>
/// <param name="response">����� �������.</param>
/// <returns>true, ���� �������� ����������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

bool SendRequest (RequestType type, 
                  byte [] data,
                  byte [] keyToEncrypt,
                  out Stream response)
{
    response = null;
    try
    {
        //
        //�������������� ������. ������� ������������ ������ � ��� 
        //�������.
        //

        MemoryStream ms = new MemoryStream ();
        Utils.Write (ms, m_Version);
        Utils.Write (ms, (int) type);           
        if (type != RequestType.rtConnect)
        {
            Utils.Write (ms, m_nConnectionId);
        }
    
        //
        //���������� ����������� ����� ��������� ������, 
        //������� ��� �� �����������.
        //

        Utils.Write (ms, CalculateCRC (data));

        //
        //������� ��������� �����.
        //

        byte [] encryptedData;
        if ( ! Encrypt (data, keyToEncrypt, out encryptedData))
        {
            m_strLastError = "�� ������� ����������� ������������ ������.";
            return false;    
        }

        //
        //���������� ������ �������������� ������ � ��� ����� � ���������� 
        //������ �� ������.
        //

        Utils.Write (ms, encryptedData.Length);
        ms.Write (encryptedData, 0, encryptedData.Length);

        if (! Send (Utils.GetStreamData (ms), out response))
        {
            return false;
        }
        
        /*
        if (type == RequestType.rtDisconnect)
        {
            byte [] buf = new byte [1024];
            response.Read (buf, 0, 1024);
            ASCIIEncoding enc = new ASCIIEncoding ();
            string res = enc.GetString (buf, 0, 1024);
            int i = 0;
        }*/ 

        //
        //�������� ��������� ������.
        //

        ResponseType responseType = (ResponseType) Utils.ReadInt (response);
        if (responseType != ResponseType.rtOK)
        {
            m_strLastError = Utils.ReadString (response);
            response.Close ();
            return false;
        }

        return true;
    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������� ����� ��� ������ � ���� ��������� ������.
/// </summary>
/// <returns>����� ��� ������ ��������� ������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////
///
MemoryStream CreateProtectedStream ()
{
    m_nRandomValue = m_Random.Next ();    
    MemoryStream ms = new MemoryStream ();
    Utils.Write (ms, m_nRandomValue);
    return ms;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ������������� ���������� � ��������.
/// </summary>
/// <returns>true, ���� ���������� ����������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool Connect ()
{
    if (IsConnected ()) return true;
    try
    {
        byte [] key;
        if (! GenerateKey (out key, out m_DecryptionKey))
        {
            m_strLastError = "�� ������� ������������� ����� ��� ���������� ������.";
            return false;
        }

        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        
        Utils.Write (secretStream, m_strLogin);
        Utils.Write (secretStream, m_strDomain);
        Utils.Write (secretStream, m_strPassword);
        Utils.Write (secretStream, key.Length);
        secretStream.Write (key, 0, key.Length);

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtConnect, 
            secretData, 
            m_KeyToConnect,
            out response))
        {
            return false;
        }
        
        //
        //������ �����.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
                                 m_DecryptionKey, 
                                 out decodedResponse, 
                                 out type, 
                                 out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "������������ ����� �������";
            return false;
        }
        if (type != RequestType.rtConnect)
        {
            m_strLastError = "����������� ��� ������";
            return false;
        }

        //
        //##������ ������������� ���������� � ���� ��� ���������� ��������.
        //

        m_nConnectionId = Utils.ReadInt (decodedResponse);
        if (m_nInvalidConnectionId == m_nConnectionId)
        {
            m_strLastError = "������ ������� � ����������.";
            return false;
        }

        int nKeyLength = Utils.ReadInt (decodedResponse);
        if (nKeyLength < 0)
        {
            m_strLastError = "����� ������� �������� ����������� ������.";
            return false;
        }
        if (0 == nKeyLength) 
        {
            m_EncryptionKey = null;
        }
        else
        {
            m_EncryptionKey = new byte [nKeyLength];
            decodedResponse.Read (m_EncryptionKey, 0, m_EncryptionKey.Length);
        }
    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }
    
    return true;
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ���������� � ��������.
/// </summary>
/// <returns>true, ���� ���������� ��������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool Disconnect ()
{
    if (! IsConnected ()) return true;
    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        Utils.Write (secretStream, m_nConnectionId);
        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtDisconnect, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            m_nConnectionId = m_nInvalidConnectionId;
            return false;
        }

        //
        //����� ������ �� �����.
        //
        response.Close ();
        m_nConnectionId = m_nInvalidConnectionId;            

    }
    catch (Exception e)
    {
        m_nConnectionId = m_nInvalidConnectionId;
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ������ ��������������� ��������� ��������.
/// </summary>
/// <param name="strClientId">������ ��������������� ��������� ��������.
/// </param>
/// <returns>true, ���� ������ ��� ������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetMobileClientList (out string [] strClientId)
{
    strClientId = null;
    if (! IsConnected ()) 
    {
        m_strLastError = "�� ����������� ���������� � ��������.";
        return false;
    }

    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        Utils.Write (secretStream, m_nConnectionId);                
        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtGetClientList, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //������ �����.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "������������ ����� �������";
            return false;
        }
        if (type != RequestType.rtGetClientList)
        {
            m_strLastError = "����������� ��� ������";
            return false;
        }

        //
        //##������ �������������� ��������.
        //

        int nCount = Utils.ReadInt (decodedResponse);
        if (nCount > 0)
        {
            strClientId = new string [nCount];
        }
        else
        {
            return true;
        }
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            strClientId [nIdx] = Utils.ReadString (decodedResponse);    
        }

    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// �������� �������� �������� ��������� ��������.
/// </summary>
/// <param name="strClientId">������ ��������������� ��������� ��������.
/// </param>
/// <param name="clientsInfo">������ �������� ��������� ��������.</param>
/// <returns>true, ���� ������ ��� ������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetMobileClientsInfo (string [] strClientId, 
    out MobileClientList clientsInfo)
{
    clientsInfo = null;
    if (! IsConnected ()) 
    {
        m_strLastError = "�� ����������� ���������� � ��������.";
        return false;
    }

    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();

        int nSize = strClientId.Length;
        Utils.Write (secretStream, nSize);
        for (int nIdx = 0; nIdx < nSize; ++ nIdx)
        {
            Utils.Write (secretStream, strClientId [nIdx]);
        }
        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtGetClientsInfo, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //������ �����.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "������������ ����� �������";
            return false;
        }
        if (type != RequestType.rtGetClientsInfo)
        {
            m_strLastError = "����������� ��� ������";
            return false;
        }

        //
        //�������� �������� ��������� ��������.
        //

        clientsInfo = new MobileClientList ();
        int nCount = Utils.ReadInt (decodedResponse);
        for (int nIdx = 0; nIdx < nCount; ++nIdx)
        {
            MobileClientInfo info = new MobileClientInfo ();
            if (! info.RestoreGuts (decodedResponse))
            {
                m_strLastError = "������ ��� ������ ���������� � ��������� ��������.";
                return false;
            }
            clientsInfo.Add (info);
        }

    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ������� ������� ��� �������� ������ ��������� ��������.
/// </summary>
/// <param name="strClientId">������ ��������������� ��������� ��������.
/// </param>
/// <param name="firstEvent">����� ������� �������.</param>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool GetFirstEvent (string [] strClientId, 
    out DateTime firstEvent)
{
    DateTime lastEvent;
    return GetEvents (strClientId, out firstEvent, out lastEvent);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ���������� ������� ��� �������� ������ ��������� ��������.
/// </summary>
/// <param name="strClientId">>������ ��������������� ��������� ��������.
/// </param>
/// <param name="lastEvent">����� ���������� �������.</param>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool GetLastEvent (string [] strClientId, out DateTime lastEvent)
{
    DateTime firstEvent;
    return GetEvents (strClientId, out firstEvent, out lastEvent);
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� ���������� ������� ��� �������� ������ ��������� ��������.
/// </summary>
/// <param name="strClientId">>������ ��������������� ��������� ��������.
/// </param>
/// <param name="firstEvent">����� ������� �������.</param>
/// <param name="lastEvent">����� ���������� �������.</param>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool GetEvents (string [] strClientId, 
    out DateTime firstEvent, 
    out DateTime lastEvent)
{
    firstEvent = DateTime.MinValue;
    lastEvent = DateTime.MinValue;

    if (! IsConnected ()) 
    {
        m_strLastError = "�� ����������� ���������� � ��������.";
        return false;
    }

    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();

        int nSize = strClientId.Length;
        Utils.Write (secretStream, nSize);
        for (int nIdx = 0; nIdx < nSize; ++ nIdx)
        {
            Utils.Write (secretStream, strClientId [nIdx]);
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtGetEvents, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //������ �����.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "������������ ����� �������";
            return false;
        }
        if (type != RequestType.rtGetEvents)
        {
            m_strLastError = "����������� ��� ������";
            return false;
        }

        firstEvent = Utils.ReadDateTime (decodedResponse);
        lastEvent = Utils.ReadDateTime (decodedResponse);
    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� � ��� �������� � ��������� �������� �������� �� ��������
/// [from; to]
/// </summary>
/// <param name="cache">���. � ��� ������ ��������� ������ ������������ 
/// ��������.</param>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
///
////////////////////////////////////////////////////////////////////////////////

public bool ReadGPSData (GPSDataCache cache, 
    DateTime from, 
    DateTime to)
{
    if (! IsConnected ()) 
    {
        m_strLastError = "�� ����������� ���������� � ��������.";
        return false;
    }

    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        int nClientCount = cache.Count;
        Utils.Write (secretStream, nClientCount);
        Utils.Write (secretStream, to);

        for (int nIdx = 0; nIdx < nClientCount; ++nIdx)
        {
            ObjectPositions clientData = cache [nIdx];
            int nIndex = clientData.GetIdxAtTime (from);
            if (nIndex >= 0) clientData.RemoveRange (0, nIndex + 1);
            Utils.Write (secretStream, clientData.ClientId);
            DateTime lastTime = clientData.LastEvent;
            if (DateTime.MinValue == lastTime) lastTime = from;
            Utils.Write (secretStream, lastTime);
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtReadGPSData, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //������ �����.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "������������ ����� �������";
            return false;
        }
        if (type != RequestType.rtReadGPSData)
        {
            m_strLastError = "����������� ��� ������";
            return false;
        }

        //
        //##�������� �������� � ��������� ��������� ��������.
        //

        GPSDataCache readData = new GPSDataCache ();
        if (!readData.RestoreGuts (decodedResponse))
        {
            m_strLastError = "������ ��� ������ �������� � �������� ��������� ��������.";
            return false;
        }
        cache.Add (readData);
    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }

    return true;                        
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� � ��� �������� � ��������� �������� �������� �� ��������
/// [now - historyDepth; now]
/// </summary>
/// <param name="cache">���. � ��� ������ ��������� ������ ������������ 
/// ��������.</param>
/// <param name="historyDepth">�������� ��������� "�������" �����������.
/// </param>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool ReadLatestGPSData (GPSDataCache cache, 
    TimeSpan historyDepth)
{
    if (! IsConnected ()) 
    {
        m_strLastError = "�� ����������� ���������� � ��������.";
        return false;
    }

    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        int nClientCount = cache.Count;
        Utils.Write (secretStream, nClientCount);
        Utils.Write (secretStream, (int) Math.Round (historyDepth.TotalSeconds));

        for (int nIdx = 0; nIdx < nClientCount; ++nIdx)
        {
            ObjectPositions clientData = cache [nIdx];
            Utils.Write (secretStream, clientData.ClientId);
            DateTime lastTime = clientData.LastEvent;
            Utils.Write (secretStream, lastTime);
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtReadLatestGPSData, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //������ �����.
        //

        int nRnd;
        Stream decodedResponse;
        RequestType type;
        if (! ReadRequestHeader (response, 
            m_DecryptionKey, 
            out decodedResponse, 
            out type, 
            out nRnd))
        {
            response.Close ();
            return false;
        }
        response.Close ();

        if (nRnd != m_nRandomValue)
        {
            m_strLastError = "������������ ����� �������";
            return false;
        }
        if (type != RequestType.rtReadLatestGPSData)
        {
            m_strLastError = "����������� ��� ������";
            return false;
        }

        //
        //##�������� �������� � ��������� ��������� ��������.
        //

        GPSDataCache readData = new GPSDataCache ();
        if (!readData.RestoreGuts (decodedResponse))
        {
            m_strLastError = "������ ��� ������ �������� � �������� ��������� ��������.";
            return false;
        }

        cache.Add (readData);        

        //
        //�������� �������� �������.
        //

        int nCount = cache.Count;
        for (int nIdx = 0; nIdx < nCount; ++ nIdx)
        {
            ObjectPositions obj = cache [nIdx];
            DateTime lastEvent = obj.LastEvent;
            if (DateTime.MinValue == lastEvent) continue;
            lastEvent = lastEvent - historyDepth;
            int nIndex = obj.GetIdxAtTime (lastEvent);
            if (nIndex > 0) obj.RemoveRange (0, nIndex + 1);
        }

    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }
    return true;            
}

////////////////////////////////////////////////////////////////////////////////
/// 
/// <summary>
/// ��������� �������� �� ������� �������� � ��������� ��������.
/// </summary>
/// <param name="clientInfo">����� �������� ��������� ��������.</param>
/// <returns>true, ���� �������� ���� ��������� �������.</returns>
/// 
////////////////////////////////////////////////////////////////////////////////

public bool UpdateMobileClientInfo (MobileClientList clientInfo)
{
    if (! IsConnected ()) 
    {
        m_strLastError = "�� ����������� ���������� � ��������.";
        return false;
    }

    try
    {
        //
        //�������������� ����� � �������, ������� ����� �����������.
        //

        MemoryStream secretStream = CreateProtectedStream ();
        int nClientCount = clientInfo.Count;
        Utils.Write (secretStream, nClientCount);

        for (int nIdx = 0; nIdx < nClientCount; ++nIdx)
        {
            MobileClientInfo info = clientInfo [nIdx];
            if ( !info.SaveGuts (secretStream)) return false;
        }

        byte [] secretData = Utils.GetStreamData (secretStream);

        //
        //���������� ������.
        //

        Stream response;
        if (! SendRequest (RequestType.rtUpdateMobileClientInfo, 
            secretData, 
            m_EncryptionKey,
            out response))
        {
            return false;
        }

        //
        //����� ������ �� �����.
        //

        response.Close ();

    }
    catch (Exception e)
    {
        m_strLastError = "����������� ������: " + e.Message;
        return false;
    }
    return true;            
}

//
//���� ��� ���������� ������� �� ������������ ����������.
//

public byte [] KeyToConnect {get {return m_KeyToConnect;} set {m_KeyToConnect = value;}}
protected byte [] m_KeyToConnect;

//
//���� ��� ���������� ������������ ������.
//

public byte [] EncryptionKey {get {return m_EncryptionKey;} set {m_EncryptionKey = value;}}
protected byte [] m_EncryptionKey;

//
//���� ��� ���������� ����������� ������.
//

public byte [] DecryptionKey {get {return m_DecryptionKey;} set {m_DecryptionKey = value;}}
protected byte [] m_DecryptionKey;

//
//������������� ����������� ���������� � ��������.
//

public const int m_nInvalidConnectionId = -1;

//
//������������� ���������� � ��������.
//

public int ConnectionId {get {return m_nConnectionId;} set {m_nConnectionId = value;}}
protected int m_nConnectionId;

//
//�����.
//

public string Login {get {return m_strLogin;} set {m_strLogin = value;}}
protected string m_strLogin;

//
//�����.
//

public string Domain {get {return m_strDomain;} set {m_strDomain = value;}}
protected string m_strDomain;

//
//������.
//

public string Password {get {return m_strPassword;} set {m_strPassword = value;}}
protected string m_strPassword;

//
//�������� ��������� ������.
//

protected string m_strLastError;

//
//������ ��������� �������������� � ��������.
//

protected int m_Version;

//
//��������� ��������, ��������������� ������.
//

protected int m_nRandomValue;

//
//��������� ��������� �����.
//

protected Random m_Random;
}
}
