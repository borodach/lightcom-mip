<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           connections.php
//
//  Facility:       ������ � ������������ �����������.
//
//
//  Abstract:       ������� ��� ������ � ������������ �����������.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  17/12/2005
//
///////////////////////////////////////////////////////////////////////////////

//
// ������������ ����� ����� ������ � ��������.
//

define('nMaxConnectionTTL', 3600);

//
// ����������� ����� ����� ������ � ��������.
//

define('nMinConnectionTTL', 900);


//
// ������ �������� � �������� ����������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $connectionID - ������������� ���������� � �����������.
//      $ekey - ���� ��� ���������� ������� ����������.
//      $dkey - ���� ��� ����������� �������� ����������.
//      $loginID - ������������� ������ ����������.
//
// ���������� ��� ����������. 
//

function GetConnectionDetails ($dbConnection, $connectionID, 
                               &$ekey, &$dkey, &$loginID)
{
    $query = 'SELECT FKLOGINS, EKEY, DKEY FROM CONNECTIONS WHERE OBJECTID=' . $connectionID; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    $row = mysql_fetch_array ($result, MYSQL_NUM);  
    if (FALSE == $row) 
    {
        mysql_free_result ($result);
        return ecNoConnection;    
    }

    $loginID =  $row [0];
    $ekey = pack ('H*', $row [1]);
    $dkey = pack ('H*', $row [2]);
    mysql_free_result ($result);

    return ecOK;    
}                                

//
// ��������� ����� ��������� ������� ����� ���������� $connectionID.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $connectionID - ������������� ���������� � �����������.
//
// ���������� ��� ����������. 
//

function UpdateLastAccessTime ($dbConnection, $connectionID)
{
    $query = 'UPDATE CONNECTIONS SET LAST_ACCESS=NOW() WHERE OBJECTID=' . $connectionID; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    return ecOK;    
}

//
// �������� ����������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $connectionID - ������������� ���������� � �����������.
//
// ���������� ��� ����������. 
//

function DropConnection ($dbConnection, $connectionID)
{
    $query = 'DELETE FROM CONNECTIONS WHERE OBJECTID=' . $connectionID; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    return ecOK;    
}

//
// ������ �������� � login.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $strLogin - �����.
//      $strDomain - �����.
//      $strPassword - ������.
//      $nLoginId - ������������� ������.
//      $nCapacity - ����� ��������.
//
// ���������� ��� ����������. 
//

function GetLoginDetails ($dbConnection,
                          $strLogin,
                          $strDomain,
                          $strPassword,
                          &$nLoginId,
                          &$nCapacity)   
{
    $strLogin = mysql_real_escape_string ($strLogin, $dbConnection);
    $strDomain = mysql_real_escape_string ($strDomain, $dbConnection);
    $strPassword = mysql_real_escape_string ($strPassword, $dbConnection);

    $query = "SELECT OBJECTID, CAPACITY, ENABLED FROM LOGINS WHERE LOGIN=\"{$strLogin}\" AND DOMAIN=\"{$strDomain}\" AND PASSWORD=\"{$strPassword}\""; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    $row = mysql_fetch_array ($result, MYSQL_NUM);  
    if (FALSE == $row) 
    {
        mysql_free_result ($result);
        return ecInvalidCredentials;    
    }

    $nLoginId =  $row [0];
    $nCapacity =  $row [1]; 
    $bEnabled =  $row [2]; 
    mysql_free_result ($result);
    if (0 == $bEnabled) return ecLoginDisabled;

    return ecOK;            
}

//
// �������� ������ ����������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $nLoginId - ������������� ������.
//      $dateTime - �����, ����������, ������ �������� ������������.
//
// ���������� ��� ����������. 
//

function DropOldConnections ($dbConnection, $nLoginId, $dateTime)
{
    $strDateTime = date ('Y-m-d H:i:s', $dateTime);
    $query = "DELETE FROM CONNECTIONS WHERE FKLOGINS={$nLoginId} AND LAST_ACCESS<\"{$strDateTime}\""; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;
        
    return ecOK;
}

//
// ���������� ������ ����������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $nLoginId - ������������� ������.
//      $ekey - ���� ��� ����������.
//      $dkey - ���� ��� �����������.
//
// ���������� ��� ����������. 
//

function InsertConnection ($dbConnection,
                           $nLoginId,
                           $ekey,
                           $dkey,
                           &$nConnectionId)
{
    //
    // Insert new record.
    //

    $ip = $_SERVER ['REMOTE_ADDR'];
    $host = $_SERVER ['REMOTE_HOST'];
    $query = "INSERT INTO CONNECTIONS (FKLOGINS, LAST_ACCESS, EKEY, DKEY, IP, HOST) VALUES ({$nLoginId}, NOW(), \"{$ekey}\", \"{$dkey}\", \"{$ip}\", \"{$host}\")"; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    //
    // Read primary key of inserted record.
    //

    $query = 'SELECT LAST_INSERT_ID()'; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;
    $row = mysql_fetch_array ($result, MYSQL_NUM);  
    if (FALSE == $row) return ecDBError;
    $nConnectionId = $row [0];            

    return ecOK;
}

//
// ������� ���������� ����������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $nLoginId - ������������� ������.
//      $nCount - ���������� ����������.
//
// ���������� ��� ����������. 
//

function GetConnectionCount ($dbConnection,
                             $nLoginId,
                             &$nCount)
{
    //
    // Count connections.
    //

    $query = "SELECT COUNT(*) FROM CONNECTIONS WHERE FKLOGINS={$nLoginId}"; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    $row = mysql_fetch_array ($result, MYSQL_NUM);  
    if (FALSE == $row) 
    {
        mysql_free_result ($result);
        return ecDBError;    
    }

    $nCount = $row [0];
    mysql_free_result ($result);

    return ecOK;
}



//
// �������� ����������.
//
//      $dbConnection - ���������� ���������� � ����� ������.
//      $nLoginId - ������������� ������.
//      $nCapacity - ����� ��������.
//      $ekey - ���� ��� ����������.
//      $dkey - ���� ��� �����������.
//
// ���������� ��� ����������. 
//

function CreateConnection ($dbConnection,
                           $nLoginId,
                           $nCapacity,
                           $ekey,
                           $dkey,
                           &$nConnectionId)
{   
    $now = time ();

    //
    // ������� ������ ����������.
    //

    $nResult = DropOldConnections ($dbConnection, 
                                   $nLoginId, 
                                   $now - nMaxConnectionTTL);
    if (ecOK != $nResult) return $nResult;

    $eKey = bin2hex ($ekey);
    $dKey = bin2hex ($dkey); 


    if (0 == $nCapacity)
    {
        //
        // ��� ����������� �� ���������� �����������.
        // ��������� ����� ����������.
        //
        
        $nResult = InsertConnection ($dbConnection,
                                     $nLoginId,
                                     $eKey,
                                     $dKey,
                                     $nConnectionId);
        return $nResult;
    }
    
    //
    // ������� ���������� ����������.
    //

    $nCount = 0;
    $nResult = GetConnectionCount ($dbConnection,
                                   $nLoginId,
                                   $nCount);
    if (ecOK != $nResult) return $nResult;

    if ($nCount >= $nCapacity)
    {
        //
        // ������� ������ ����������.
        //

        $nResult = DropOldConnections ($dbConnection, 
                                       $nLoginId, 
                                       $now - nMinConnectionTTL);
        if (ecOK != $nResult) return $nResult;
    }

    $nResult = GetConnectionCount ($dbConnection,
                                   $nLoginId,
                                   $nCount);
    if (ecOK != $nResult) return $nResult;

    //
    // ������� ���������� ����������.
    //

    if ($nCount >= $nCapacity) return ecNoFreeLicenses;

    //
    // ��������� ����� ����������.
    //

    return InsertConnection ($dbConnection,
                             $nLoginId,
                             $eKey,
                             $dKey,
                             $nConnectionId);

}
?>