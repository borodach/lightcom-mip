<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtGetClientList.php
//
//  Facility:       Обработка запросов диспетчеров.
//
//
//  Abstract:       Обработка запроса rtGetClientList.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  19/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');
include_once ('mysql/clients.php');

//
// Обработка запроса на получение списка мобильных клиентов.
//
//      $version      - версия запроса.
//      $dbConnection - дескриптор соединения с базой данных.
//      $eKey         - ключ для шифрования данных.
//      $loginID      - идентификатор логина.
//      $data         - тело запроса.
//      $rnd          - маркер запроса.
//
// Ничего не возвращает.   
//

function ProcessGetClientListRequest ($version, $dbConnection, $eKey, $loginID, $data, $rnd)
{
    include_once ('dispatcher/errorcode.php');
    include ('dispatcher/errormsg.php');    
    $clients = NULL;
    $nResult = GetClientList ($dbConnection,
                              $loginID,
                              $clients);   

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

    $strData = '';
    $nClientCount = 0;
    if (NULL != $clients) $nClientCount = count ($clients);
    WriteInt ($strData, $nClientCount);
    for ($nIdx = 0; $nIdx < $nClientCount; ++$nIdx) 
    {
        WriteString ($strData, $clients [$nIdx]);
    }

    //
    // Create response.
    //

    $response = '';
    $nResult = CreateRequest (rtGetClientList, $eKey, $rnd, $strData, $response);
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