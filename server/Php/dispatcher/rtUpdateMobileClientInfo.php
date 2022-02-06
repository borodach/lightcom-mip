<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtUpdateMobileClientInfo.php
//
//  Facility:       ��������� �������� �����������.
//
//
//  Abstract:       ��������� ������� rtUpdateMobileClientInfo.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  21/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');

//
// ��������� ������� �� ��������� �������� ��������� ��������.
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

function ProcessUpdateMobileClientInfo ($version, $dbConnection, $eKey, $loginID, $data, $rnd)
{
    include_once ('dispatcher/errorcode.php');
    include ('dispatcher/errormsg.php');
    include_once ('mysql/clients.php');

    $clientInfo = NULL;
    $nClientCount = ReadInt ($data);
    if ($nClientCount < 0)
    {
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecBadFormat]));
        WriteResponse ($strResult);

        return;
    }

    $clientInfo = new ClientInfo ();

    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx)
    {
          
        $nResult = $clientInfo->RestoreGuts ($data);
        if (ecOK != $nResult)
        {
            WriteInt ($strResult, rtError);
            WriteString ($strResult, utf8_encode ($errorMessages [$nResult]));
            WriteResponse ($strResult);

            return;
        }

        $nResult = UpdateClientInfo ($dbConnection, $loginID, $clientInfo);   

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

    $strResult = '';
    WriteInt ($strResult, rtOK);
    WriteResponse ($strResult);

}
?>