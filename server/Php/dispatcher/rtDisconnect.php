<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtDisconnect.php
//
//  Facility:       Обработка запросов диспетчеров.
//
//
//  Abstract:       Обработка запроса rtDisconnect.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  16/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');

//
// Обработка запроса на разрыв соединения.
//
//      $version      - версия запроса.
//      $dbConnection - дескриптор соединения с базой данных.
//      $eKey         - ключ для шифрования данных.
//      $data         - тело запроса.
//      $id           - идентификатор соединения.
//
// Ничего не возвращает.   
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