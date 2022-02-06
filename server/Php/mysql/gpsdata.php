<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           gpsdata.php
//
//  Facility:       ������ � ������������ �����������.
//
//
//  Abstract:       ������� ��� ������ � GPS �������.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  21/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('mysql/utils.php');

//
// ������ ����� ������� � ���������� �������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $loginID      - ������������� ������ ����������.
//      $�lientId     - ������������� �������.
//      $firstTime    - ����� ������� �������.
//      $lastTime     - ����� ���������� �������.
//
// ���������� ��� ����������. 
//

function GetEvents ($dbConnection, $loginID, $clientId, &$firstTime, &$lastTime)
{
    $firstTime = 0;
    $lastTime = 0;

    //
    // ���������, ���� �� ����� ������ � ����� �� ������ ����� ����� �� 
    // ������ � ����� ��������.
    //

    $objectId = 0;
    $query = "SELECT CLIENTS.OBJECTID " .
             "FROM DOMAINS, COMPANY_MEMBERS, CLIENTS " .
             "WHERE " .
             "DOMAINS.FKLOGINS={$loginID} AND ".
             "DOMAINS.FKCOMPANY=COMPANY_MEMBERS.FKCOMPANY AND " .
             "COMPANY_MEMBERS.FKCLIENTS=CLIENTS.OBJECTID AND " .
             "CLIENTS.ENABLED=1 AND " .
             "CLIENTS.ID=\"{$clientId}\""; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) 
    {
        return ecDBError;
    }

    $row = mysql_fetch_array ($result, MYSQL_NUM);
    if (! $row) return ecOK;
    $objectId = $row [0];
    mysql_free_result ($result);

    $query = "SELECT MIN(FIX_TIME), MAX(FIX_TIME) " .
             "FROM GPS_DATA " .
             "WHERE " .
             "FKCLIENTS=\"{$objectId}\""; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    $row = mysql_fetch_array ($result, MYSQL_NUM);
    if (! $row) return ecOK;
    $firstTime = ParseMySQLDateTime ($row [0]);
    $lastTime = ParseMySQLDateTime ($row [1]);

    mysql_free_result ($result);

    return ecOK;
}

//
// ������ GPS ������ �� �������� �������� �������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $loginID      - ������������� ������ ����������.
//      $�lientId     - ������������� �������.
//      $fromTime    - ����� ������� �������.
//      $toTime       - ����� ���������� �������.
//      $positions    - ����������� GPS ������.
//
// ���������� ��� ����������. 
//

function ReadGPSData ($dbConnection, 
                      $loginID, 
                      $clientId,
                      $fromTime,
                      $toTime,
                      &$positions)
{
    $positions = NULL;

    //
    // ���������, ���� �� ����� ������ � ����� �� ������ ����� ����� �� 
    // ������ � ����� ��������.
    //

    $objectId = 0;
    $query = "SELECT CLIENTS.OBJECTID " .
             "FROM DOMAINS, COMPANY_MEMBERS, CLIENTS " .
             "WHERE " .
             "DOMAINS.FKLOGINS={$loginID} AND ".
             "DOMAINS.FKCOMPANY=COMPANY_MEMBERS.FKCOMPANY AND " .
             "COMPANY_MEMBERS.FKCLIENTS=CLIENTS.OBJECTID AND " .
             "CLIENTS.ENABLED=1 AND " .
             "CLIENTS.ID=\"{$clientId}\""; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) 
    {
        return ecDBError;
    }

    $row = mysql_fetch_array ($result, MYSQL_NUM);
    if (! $row) return ecOK;
    $objectId = $row [0];
    mysql_free_result ($result);

    //
    // ������ GPS ������.
    //

    $strFromTime = gmdate ('Y-m-d H:i:s', $fromTime);
    $strToTime = gmdate ('Y-m-d H:i:s', $toTime);

    $query = "SELECT FIX_TIME, LATITUDE, LONGITUDE, GROUND_SPEED  " .
             "FROM GPS_DATA " .
             "WHERE " .
             "FKCLIENTS = \"{$objectId}\" AND ".
             "FIX_TIME >= \"{$strFromTime}\" AND ".
             "FIX_TIME <= \"{$strToTime}\""; 

    /*
    $f = fopen ('d:\log\log.txt', 'w');
    fputs ($f, $query);
    fclose ($f);*/
    

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) 
    {
        return ecDBError;
    }

    $bFreeResult = FALSE;
    while ($row = mysql_fetch_array ($result, MYSQL_NUM))
    {
        $bFreeResult = TRUE;
        $positions [] = array ('x'       => $row [2],
                               'y'       => $row [1],
                               'speed'   => $row [3],
                               'fixTime' => ParseMySQLDateTime ($row [0]));
    }
    
    if ($bFreeResult) mysql_free_result ($result);

    return ecOK;
}

//
// ��������� ������� ��������� ������� ��� �������� ������ �� ���������
// $nTimeSpan ������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $loginID      - ������������� ������ ����������.
//      $�lientId     - ������������� �������.
//      $nTimeSpan    - ����� ��������� � ��������.
//      $fromtTime    - ����� ������� �������.
//      $toTime       - ����� ���������� �������.
//
// ���������� ��� ����������. 
//

function GetTimeInterval ($dbConnection, 
                          $clientId, 
                          $nTimeSpan, 
                          &$fromTime, 
                          &$toTime)
{
    $fromTime = 0; 
    $toTime = 0;

    $query = "SELECT MAX(GPS_DATA.FIX_TIME) " .
             "FROM GPS_DATA, CLIENTS " .
             "WHERE " .
             "GPS_DATA.FKCLIENTS = CLIENTS.OBJECTID  AND ".
             "CLIENTS.ID = \"{$clientId}\""; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) 
    {
        return ecDBError;
    }

    
    if ($row = mysql_fetch_array ($result, MYSQL_NUM)) 
    {
        $toTime = ParseMySQLDateTime ($row [0]);
        $tmpTime = $toTime - $nTimeSpan;
        if ($fromTime < $tmpTime) $fromTime = $tmpTime;

        mysql_free_result ($result);    
    }

    return ecOK;
}
?>
