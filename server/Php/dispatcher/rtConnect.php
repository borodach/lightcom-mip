<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           rtConnect.php
//
//  Facility:       Обработка запросов диспетчеров.
//
//
//  Abstract:       Обработка запроса rtConnect.
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
// Обработка запроса на установление соединения.
//
//      $version      - версия запроса.
//      $dbConnection - дескриптор соединения с базой данных.
//      $data         - тело запроса.
//      $rnd          - маркер запроса.
//
// Ничего не возвращает.   
//

function ProcessConnectRequest ($version, $dbConnection, $data, $rnd)
{
    include_once ('dispatcher/errorcode.php');
    include ('dispatcher/errormsg.php');
    include_once ('mysql/connections.php');

    //
    //Read request details.
    //

    $strLogin = ReadString ($data);
    $strDomain = ReadString ($data);
    $strPassword = ReadString ($data);

    //
    // Key to encrypt data to be sent.
    //

    $strKey = ReadString ($data);
    
    //
    // Read login details.
    //

    $nLoginId = -1;
    $nCapacity = -1;

    $nResult = GetLoginDetails ($dbConnection,
                                $strLogin,
                                $strDomain,
                                $strPassword,
                                $nLoginId,
                                $nCapacity);   
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
    //Generate connection keys.
    //

    $ekey = '';
    $dkey = '';
    if (!GenerateKey ($ekey, $dkey))
    {
        WriteInt ($strResult, rtError);
        WriteString ($strResult, utf8_encode ($errorMessages [ecCryptoError]));
        WriteResponse ($strResult);

        return;    
    }

    $nConnectionId = 0;

    //$semKey = ftok (__FILE__, '1');
    //$semID = sem_get ($semKey);
    //sem_acquire ($semID);

    $nResult = CreateConnection ($dbConnection,
                                 $nLoginId,
                                 $nCapacity,
                                 $strKey,
                                 $dkey,
                                 $nConnectionId);   
    //sem_release ($semID);

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

    $strData = '';
    WriteInt ($strData, $nConnectionId);
    WriteString ($strData, $ekey);
    $response = '';
    $nResult = CreateRequest (rtConnect, $strKey, $rnd, $strData, $response);

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