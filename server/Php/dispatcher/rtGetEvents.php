<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtGetEvents.php
//
//  Facility:       ��������� �������� �����������.
//
//
//  Abstract:       ��������� ������� rtGetEvents.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  21/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');
include_once ('mysql/gpsdata.php');

//
// ��������� ������� �� ��������� ������ ��������� ��������.
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

function ProcessGetEvents ($version, $dbConnection, $eKey, $loginID, $data, $rnd)
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

    $resFirstTime = 0;
    $resLastTime = 0;

    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx)
    {
        $firstTime = 0;
        $lastTime = 0;
        $clientId = ReadString ($data);
        $nResult = GetEvents ($dbConnection, $loginID, $clientId, $firstTime, $lastTime);   

        if ($firstTime != 0 && $lastTime != 0)
        {
            if ($resLastTime < $lastTime) $resLastTime = $lastTime;
            if ($resFirstTime > $firstTime || 0 == $resFirstTime) 
                $resFirstTime = $firstTime;
        }

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

    }

    $strData = '';
    WriteDateTime ($strData, $resFirstTime);
    WriteDateTime ($strData, $resLastTime);    

    //
    // Create response.
    //

    $response = '';
    $nResult = CreateRequest (rtGetEvents, $eKey, $rnd, $strData, $response);
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