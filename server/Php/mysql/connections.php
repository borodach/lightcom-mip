<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           connections.php
//
//  Facility:       Работа с соединениями диспетчеров.
//
//
//  Abstract:       Функции для работы с соединениями диспетчеров.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  17/12/2005
//
///////////////////////////////////////////////////////////////////////////////

//
// Максимальное время жизни сессии в секундах.
//

define('nMaxConnectionTTL', 3600);

//
// Минимальное время жизни сессии в секундах.
//

define('nMinConnectionTTL', 900);


//
// Читает сведения о заданном соединении.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $connectionID - идентификатор соединения с диспетчером.
//      $ekey - ключ для шифрования ответов диспетчеру.
//      $dkey - ключ для расшифровки запросов диспетчера.
//      $loginID - идентификатор логина диспетчера.
//
// Возвращает код завершения. 
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
// Обновляет время послднего доступа через соединение $connectionID.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $connectionID - идентификатор соединения с диспетчером.
//
// Возвращает код завершения. 
//

function UpdateLastAccessTime ($dbConnection, $connectionID)
{
    $query = 'UPDATE CONNECTIONS SET LAST_ACCESS=NOW() WHERE OBJECTID=' . $connectionID; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    return ecOK;    
}

//
// Удаление соединения.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $connectionID - идентификатор соединения с диспетчером.
//
// Возвращает код завершения. 
//

function DropConnection ($dbConnection, $connectionID)
{
    $query = 'DELETE FROM CONNECTIONS WHERE OBJECTID=' . $connectionID; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    return ecOK;    
}

//
// Чтение сведений о login.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $strLogin - логин.
//      $strDomain - домен.
//      $strPassword - пароль.
//      $nLoginId - идентификатор логина.
//      $nCapacity - объем лицензии.
//
// Возвращает код завершения. 
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
// Удаление старых соединений.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $nLoginId - идентификатор логина.
//      $dateTime - время, соединения, старее которого уничтожаются.
//
// Возвращает код завершения. 
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
// Добавление нового соединения.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $nLoginId - идентификатор логина.
//      $ekey - ключ для шифрования.
//      $dkey - ключ для расшифровки.
//
// Возвращает код завершения. 
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
// Подсчет количества соединений.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $nLoginId - идентификатор логина.
//      $nCount - количество соединений.
//
// Возвращает код завершения. 
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
// Создание соединения.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $nLoginId - идентификатор логина.
//      $nCapacity - объем лицензии.
//      $ekey - ключ для шифрования.
//      $dkey - ключ для расшифровки.
//
// Возвращает код завершения. 
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
    // Удаляем старые соединения.
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
        // Нет ограничений на количество подключений.
        // Добавляем новое соединение.
        //
        
        $nResult = InsertConnection ($dbConnection,
                                     $nLoginId,
                                     $eKey,
                                     $dKey,
                                     $nConnectionId);
        return $nResult;
    }
    
    //
    // Считаем количество соединений.
    //

    $nCount = 0;
    $nResult = GetConnectionCount ($dbConnection,
                                   $nLoginId,
                                   $nCount);
    if (ecOK != $nResult) return $nResult;

    if ($nCount >= $nCapacity)
    {
        //
        // Удаляем старые соединения.
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
    // Считаем количество соединений.
    //

    if ($nCount >= $nCapacity) return ecNoFreeLicenses;

    //
    // Добавляем новое соединение.
    //

    return InsertConnection ($dbConnection,
                             $nLoginId,
                             $eKey,
                             $dKey,
                             $nConnectionId);

}
?>