<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtDisconnect.php
//
//  Facility:       ��������� �������� �����������.
//
//
//  Abstract:       ��������� ������� rtDisconnect.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  16/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');

//
// ��������� ������� �� ������ ����������.
//
//      $version      - ������ �������.
//      $dbConnection - ���������� ���������� � ����� ������.
//      $eKey         - ���� ��� ���������� ������.
//      $data         - ���� �������.
//      $id           - ������������� ����������.
//
// ������ �� ����������.   
//

function ProcessDisconnectRequest ($version, $dbConnection, $eKey, $data, $id)
{
    include_once ('dispatcher/errorcode.php');
    include_once ('dispatcher/errormsg.php');
    include_once ('mysql/connections.php');

    //
    //Read request details.
    //

    $nConnectionId = ReadInt ($data);
    if ($nConnectionId != $id)
    {
        $strResult = '';
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecBadFormat]));
        WriteResponse ($strResult);

        return;    
    }

    $nResult = DropConnection ($dbConnection, $id);

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

    //
    // Create response.
    //

    $strResult = '';
    WriteInt ($strResult, rtOK);
    WriteResponse ($strResult);
}
?>