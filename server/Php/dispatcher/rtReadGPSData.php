<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtReadGPSData.php
//
//  Facility:       ��������� �������� �����������.
//
//
//  Abstract:       ��������� �������� rtReadGPSData � rtLatestReadGPSData.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  22/12/2005
//
///////////////////////////////////////////////////////////////////////////////

define ('nGPSCacheVersion', 1);
define ('nObjectPositionsVersion', 1);
define ('nObjectPositionVersion', 1);

include_once ('ioutils.php');
include_once ('mysql/gpsdata.php');

//
// ������ GPS ������.
//
//      $requestType  - ��� ������� (rtReadGPSData ��� rtLatestReadGPSData).
//      $version      - ������ �������.
//      $dbConnection - ���������� ���������� � ����� ������.
//      $eKey         - ���� ��� ���������� ������.
//      $loginID      - ������������� ������.
//      $readRequest  - �������� ����, ��� ������ ����� ���������.
//      $id           - ������������� ����������.
//      $rnd          - ������ �������.
//
// ������ �� ����������.   
//

function ReadAllGPSData ($requestType, $version, $dbConnection, $eKey, $loginID, $readRequest, $rnd)
{
    
    $nClientCount = 0;
    if (NULL != $readRequest) $nClientCount = count ($readRequest);

    $strData = '';
    WriteInt ($strData, nGPSCacheVersion);
    WriteInt ($strData, $nClientCount);

    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx) 
    {
        $clientId = $readRequest [$nIdx] ['clientId'];
        $positions = NULL;
        $nResult = ReadGPSData ($dbConnection, 
                                $loginID, 
                                $clientId,
                                $readRequest [$nIdx] ['fromTime'],
                                $readRequest [$nIdx] ['toTime'],
                                $positions);

        if (ecOK != $nResult)
        {
            $strResult = '';
            WriteInt ($strResult, rtError);
            WriteString ($strResult, utf8_encode ($errorMessages [$nResult]));
            WriteResponse ($strResult);

            return;    
        }

        $nPositionCount = 0;
        if (NULL != $positions) $nPositionCount = count ($positions);

        WriteInt ($strData, nObjectPositionsVersion);
        WriteString ($strData, $clientId);
        WriteInt ($strData, $nPositionCount);

        for ($nPos = 0; $nPos < $nPositionCount; ++$nPos) 
        {
            WriteInt ($strData, nObjectPositionVersion);    
            WriteInt ($strData, intval ($positions [$nPos] ['x'] * 10000000));    
            WriteInt ($strData, intval ($positions [$nPos] ['y'] * 10000000));    
            WriteInt ($strData, intval ($positions [$nPos] ['speed'] * 1000));    
            WriteDateTime ($strData, $positions [$nPos] ['fixTime']);    
        }
    }

    //
    // Create response.
    //

    $response = '';
    $nResult = CreateRequest ($requestType, $eKey, $rnd, $strData, $response);
    if (ecOK != $nResult)
    {
        $strResult = '';
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [$nResult]));
        WriteResponse ($strResult);

        return;    
    }    

    $strResult = '';
    WriteInt ($strResult, rtOK);
    WriteResponse ($strResult . $response);    
}

//
// ��������� ������� �� ��������� �������� � ��������� ��������� �������� �
// �������� �������� �������.
//
//      $version      - ������ �������.
//      $dbConnection - ���������� ���������� � ����� ������.
//      $eKey         - ���� ��� ���������� ������.
//      $loginID      - ������������� ������.
//      $data         - ���� �������.
//      $id           - ������������� ����������.
//      $rnd          - ������ �������.
//
// ������ �� ����������.   
//

function ProcessReadGPSData ($version, $dbConnection, $eKey, $loginID, $data, $rnd)
{
    include_once ('dispatcher/errorcode.php');
    include ('dispatcher/errormsg.php');    

    $nClientCount = ReadInt ($data);
    if ($nClientCount < 0)
    {
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecBadFormat]));
        WriteResponse ($strResult);

        return;
    }
    $toTime = ReadDateTime ($data);

    $readRequest = NULL;

    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx)
    {
        $clientId = ReadString ($data);
        $fromTime = ReadDateTime ($data);

        /*
        $f = fopen ('d:\log\log.txt', 'w');
        fputs ($f, gmdate ('Y-m-d H:i:s',$fromTime) . ' - ' . gmdate ('Y-m-d H:i:s',$toTime));
        fclose ($f);
        */

        $readRequest [] = array ('clientId' => $clientId,
                                 'fromTime' => $fromTime,
                                 'toTime'   => $toTime);
    }

    ReadAllGPSData (rtReadGPSData, $version, $dbConnection, $eKey, $loginID, $readRequest, $rnd);
}


//
// ��������� ������� �� ��������� ������ �������� � ��������� ��������� ��������.
//
//      $version      - ������ �������.
//      $dbConnection - ���������� ���������� � ����� ������.
//      $eKey         - ���� ��� ���������� ������.
//      $loginID      - ������������� ������.
//      $data         - ���� �������.
//      $rnd          - ������ �������.
//
// ������ �� ����������.   
//

function ProcessReadLatestGPSData ($version, $dbConnection, $eKey, $loginID, $data, $rnd)
{
    include_once ('dispatcher/errorcode.php');
    include ('dispatcher/errormsg.php');    

    $nClientCount = ReadInt ($data);
    if ($nClientCount < 0)
    {
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecBadFormat]));
        WriteResponse ($strResult);

        return;
    }
    $nTimeSpan = ReadInt ($data);
    if ($nTimeSpan < 0)
    {
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecBadFormat]));
        WriteResponse ($strResult);

        return;
    }

    $readRequest = NULL;

    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx)
    {
        
        $clientId = ReadString ($data);
        $fromTime = ReadDateTime ($data);
        $toTime   = 0;

        $nResult = GetTimeInterval ($dbConnection, $clientId, $nTimeSpan, $fromTime, $toTime);
        if ($nResult != ecOK)
        {
            WriteInt ($strResult, rtError);
            WriteString ($strResult, utf8_encode ($errorMessages [$nResult]));
            WriteResponse ($strResult);

            return;
        }

        $readRequest [] = array ('clientId' => $clientId,
                                 'fromTime' => $fromTime,
                                 'toTime'   => $toTime);
    }

    ReadAllGPSData (rtReadLatestGPSData, $version, $dbConnection, $eKey, $loginID, $readRequest, $rnd);
}
?>