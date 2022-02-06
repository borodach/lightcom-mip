<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           clientinfo.php
//
//  Facility:       ����� - �������� ���������� �������.
//
//
//  Abstract:       ����� - �������� ���������� �������.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  20/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');
include_once ('errorcode.php');

//
// ����� - �������� ���������� �������.
//

class ClientInfo
{
    //
    // ���������.
    //
    // ����� ����� ��������� ������ ������� ������.
    //

    var $m_LatestVersion = 1;

    // 
    // ���������� ������������� ���������� �������.
    //

    //
    // �����������.
    //

    function __construct () 
    {
        $this->Reset ();
    }

    //
    // ������� ������, �������� � �������.
    //

    function Reset ()
    {
        $this->m_ClientId = '';
        $this->m_FriendlyName = '';
        $this->m_Company = '';
        $this->m_LastEvent = '';
        $this->m_SoftwareName = '';
        $this->m_SoftwareVersion = '';
        $this->m_Comments = '';        
    }

    //
    // ��������� �� ������ ������ ����� �����. ����������� ����� ��������� ��
    // ������.
    //
    //      $strBuffer - ������, � ������� ������������ ��������.
    //
    // ������ �� ����������.
    //
    //

    function SaveGuts (&$strBuffer)
    {
        WriteInt      ($strBuffer, $this->m_LatestVersion);
        WriteString   ($strBuffer, $this->m_ClientId);
        WriteString   ($strBuffer, $this->m_FriendlyName);
        WriteString   ($strBuffer, $this->m_Company);
        WriteDateTime ($strBuffer, $this->m_LastEvent);    
        WriteString   ($strBuffer, $this->m_SoftwareName);
        WriteString   ($strBuffer, $this->m_SoftwareVersion);
        WriteString   ($strBuffer, $this->m_Comments);
    }

    //
    // ��������� �� ������ ������ ������. ����������� ������ ��������� ��
    // ������.
    //
    //      $strBuffer - ������, �� ������� ����������� ��������.
    //
    // ���������� ��� ������.
    //
    //

    function RestoreGuts (&$strBuffer)
    {
        $this->Reset ();
        $nVersion = ReadInt ($strBuffer);
        if ($nVersion > $this->m_LatestVersion ||
            $nVersion <= 0) return ecBadFormat;

        $this->m_ClientId        = ReadString   ($strBuffer);
        $this->m_FriendlyName    = ReadString   ($strBuffer);
        $this->m_Company         = ReadString   ($strBuffer);
        $this->m_LastEvent       = ReadDateTime ($strBuffer);    
        $this->m_SoftwareName    = ReadString   ($strBuffer);
        $this->m_SoftwareVersion = ReadString   ($strBuffer);
        $this->m_Comments        = ReadString   ($strBuffer);

        return ecOK;
    }
     
    var $m_ClientId;

    // 
    // ������������� �������� ���������� �������.
    //

    var $m_FriendlyName;

    // 
    // �������� �������� - ��������� ���������� �������.
    //
    
    var $m_Company;

    // 
    // ����� ��������� ���������� ��������� �� ���������� �������.
    //

    var $m_LastEvent;

    // 
    // �������� ����������� ��.
    //

    var $m_SoftwareName;

    // 
    // ������ ����������� ��.
    //


    var $m_SoftwareVersion;

    // 
    // ������ � ������������.
    //

    var $m_Comments;
}
?>