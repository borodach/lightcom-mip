<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           dgate.php
//
//  Facility:       CGI-скрипт, принимающий данные от диспетчеров.
//
//
//  Abstract:       Управление обработой запросов диспетчеров.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  17/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('dispatcher/errorcode.php');
include_once ('dispatcher/requestparser.php');
include_once ('dispatcher/ioutils.php');

include_once ('dispatcher/rtConnect.php');
include_once ('dispatcher/rtDisconnect.php');
include_once ('dispatcher/rtGetClientList.php');
include_once ('dispatcher/rtGetClientsInfo.php');
include_once ('dispatcher/rtUpdateMobileClientInfo.php');
include_once ('dispatcher/rtGetEvents.php');
include_once ('dispatcher/rtReadGPSData.php');


define('nMaxRequestLength', 1024 * 1024);

function dgate ()
{
    include_once ('dispatcher/errormsg.php');
    include_once ('mysql/mysqlconnectiondetails.php');

    //
    // Connect to database.
    //

    $dbConnection = mysql_connect ($mysqlHost, 
                                   $mysqlUser, 
                                   $mysqlPassword);
    if (FALSE == $dbConnection) 
    {
        $strResult = '';
        $strMessage = utf8_encode ($errorMessages [ecDBConnectionFailed]);
        
        WriteInt ($strResult, rtError);
        WriteString ($strResult, $strMessage);
        WriteResponse ($strResult);
    }
    
    if ( FALSE == mysql_select_db ($mysqlDB))
    {
        $strResult = '';
        $strMessage = utf8_encode ($errorMessages [ecDBConnectionFailed]);
        
        WriteInt ($strResult, rtError);
        WriteString ($strResult, $strMessage);
        WriteResponse ($strResult);
    }

    //
    // Read all the posted data into $request string.
    //

    $hData = fopen ('php://input', 'rb');
    //$hData = fopen ('php://stdin', 'rb');
    
    if (NULL == $hData) 
    {
        $strResult = '';
        $strMessage = utf8_encode ($errorMessages [ecBadFormat]);
        
        WriteInt ($strResult, rtError);
        WriteString ($strResult, $strMessage);
        WriteResponse ($strResult);
        return;
    }

    $request = fread ($hData, nMaxRequestLength);
    fclose ($hData);

    //
    // Parse the request.
    //

    $type = rtMinValue;
    $id = nInvalidConnectionId;
    $rnd = 0;
    $data = '';
    $eKey = '';
    $loginID = 0;
    $version = 0;

    $nResult = ParseRequest ($request, $dbConnection, $type, $id, $rnd, $data, 
                             $eKey, $loginID, $version);

    //
    // Process parsing error.
    //

    if (ecOK != $nResult) 
    {
        $strResult = '';
        $strMessage = utf8_encode ($errorMessages [$nResult]);
        
        WriteInt ($strResult, rtError);
        WriteString ($strResult, $strMessage);
        WriteResponse ($strResult);

        return;
    }

    switch ($type)
    {
    case rtConnect:
        ProcessConnectRequest ($version, $dbConnection, $data, $rnd);
        return;
    case rtDisconnect:
        ProcessDisconnectRequest ($version, $dbConnection, $eKey, $data, $id);
        return;
    case rtGetClientList:
        ProcessGetClientListRequest ($version, $dbConnection, $eKey, $loginID, $data, $rnd);
        return;
    case rtGetClientsInfo:
        ProcessGetClientsInfo ($version, $dbConnection, $eKey, $loginID, $data, $rnd);
        return;
    case rtGetEvents:
        ProcessGetEvents ($version, $dbConnection, $eKey, $loginID, $data, $rnd);
        return;
    case rtReadGPSData:
        ProcessReadGPSData ($version, $dbConnection, $eKey, $loginID, $data, $rnd);
        return;
    case rtReadLatestGPSData:
        ProcessReadLatestGPSData ($version, $dbConnection, $eKey, $loginID, $data, $rnd);
        return;
    case rtUpdateMobileClientInfo:
        ProcessUpdateMobileClientInfo ($version, $dbConnection, $eKey, $loginID, $data, $rnd);
        return;
    };
}

dgate ();
?>

