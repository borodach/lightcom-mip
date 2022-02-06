<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtGetClientsInfo.php
//
//  Facility:       ��������� �������� �����������.
//
//
//  Abstract:       ��������� ������� rtGetClientsInfo.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  20/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');
include_once ('mysql/clients.php');

//
// ��������� ������� �� ��������� �������� � ��������� ��������.
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

function ProcessGetClientsInfo ($version, $dbConnection, $eKey, $loginID, $data, $rnd)
{
    include_once ('dispatcher/errorcode.php');
    include ('dispatcher/errormsg.php');    

    $clientInfo = NULL;
    $nClientCount = ReadInt ($data);
    if ($nClientCount < 0)
    {
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecBadFormat]));
        WriteResponse ($strResult);

        return;
    }

    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx)
    {
          
        $clientId = ReadString ($data);
        $client = NULL;

        $nResult = GetClientInfo ($dbConnection, $loginID, $clientId, $client);   

        //
        // Process error.
        //

        if (ecOK != $nResult)
        {
            WriteInt ($strResult, rtError);
            WriteString ($strResult, utf8_encode ($errorMessages [$nResult]));
            WriteResponse ($strResult);

            return;
        }

        if (NULL == $client) continue;
        $clientInfo [] = $client;

    }

    $strData = '';
    $nClientCount = 0;
    if (NULL != $clientInfo) $nClientCount = count ($clientInfo);
    WriteInt ($strData, $nClientCount);
    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx) 
    {
        $clientInfo [$nIdx]->SaveGuts ($strData);
    }

    //
    // Create response.
    //

    $response = '';
    $nResult = CreateRequest (rtGetClientsInfo, $eKey, $rnd, $strData, $response);
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
?>