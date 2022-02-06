<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           clients.php
//
//  Facility:       Работа с соединениями диспетчеров.
//
//
//  Abstract:       Функции для работы с описаниями мобильных клиентов.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  19/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('dispatcher/clientinfo.php');
include_once ('mysql/utils.php');

//
// Читает список идентификаторов клиентов.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $loginID      - идентификатор логина диспетчера.
//      $сlients     - список идентификаторов мобильных клиентов.
//
// Возвращает код завершения. 
//

function GetClientList ($dbConnection, $loginID, &$сlients)
{
    $сlients = NULL;
    $query = "SELECT DISTINCT (CLIENTS.ID) FROM DOMAINS, COMPANY_MEMBERS, CLIENTS " .
             "WHERE " .
             "DOMAINS.FKLOGINS={$loginID} AND ".
             "DOMAINS.FKCOMPANY=COMPANY_MEMBERS.FKCOMPANY AND " .
             "COMPANY_MEMBERS.FKCLIENTS=CLIENTS.OBJECTID AND " .
             "CLIENTS.ENABLED=1"; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    $bDataRead = FALSE;
    while ($row = mysql_fetch_array ($result, MYSQL_NUM))
    {
        $bDataRead = TRUE;
        $сlients [] = $row [0];       
    }  

    if ($bDataRead) mysql_free_result ($result);

    return ecOK;       
}

//
// Читает сведения о клиенте.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $loginID      - идентификатор логина диспетчера.
//      $сlientId     - идентификатор клиента.
//      $clientInfo   - описание клиента.
//
// Возвращает код завершения. 
//

function GetClientInfo ($dbConnection, $loginID, $clientId, &$clientInfo)
{
    $clientInfo = NULL;

    $query = "SELECT CLIENTS.ID, CLIENTS.PRODUCT_NAME, CLIENTS.PRODUCT_VERSION, CLIENTS.FRIENDLYNAME, CLIENTS.COMMENTS, COMPANY.COMPANY_NAME " .
             "FROM DOMAINS, COMPANY_MEMBERS, CLIENTS, COMPANY " .
             "WHERE " .
             "DOMAINS.FKLOGINS={$loginID} AND ".
             "DOMAINS.FKCOMPANY=COMPANY_MEMBERS.FKCOMPANY AND " .
             "COMPANY_MEMBERS.FKCLIENTS=CLIENTS.OBJECTID AND " .
             "COMPANY.OBJECTID=DOMAINS.FKCOMPANY AND " .
             "CLIENTS.ENABLED=1 AND " .
             "CLIENTS.ID=\"{$clientId}\""; 

     $result = mysql_query ($query, $dbConnection);
     if (FALSE == $result) return ecDBError;

     $query = "SELECT LAST_PUBLISH_TIME.PUBLISH_TIME FROM " .
              "CLIENTS, LAST_PUBLISH_TIME " .
              "WHERE " .
              "CLIENTS.ENABLED=1 AND " .
              "LAST_PUBLISH_TIME.FKCLIENTS=CLIENTS.OBJECTID AND " .
              "CLIENTS.ID=\"{$clientId}\""; 

     $result1 = mysql_query ($query, $dbConnection);
     if (FALSE == $result1) return ecDBError;
 
     $bDataRead = FALSE;

     while ($row = mysql_fetch_array ($result, MYSQL_NUM))
     {
         if (NULL == $clientInfo) 
         {
             $clientInfo = new ClientInfo ();
             $clientInfo->m_ClientId = $row [0];
             $clientInfo->m_SoftwareName = $row [1];
             $clientInfo->m_SoftwareVersion = $row [2];
             $clientInfo->m_FriendlyName = $row [3];
             $clientInfo->m_Comments = $row [4];
             $clientInfo->m_Company = $row [5];
             $row1 = mysql_fetch_array ($result1, MYSQL_NUM);
             if ($row1)
             {
                mysql_free_result ($result1);
                $clientInfo->m_LastEvent = ParseMySQLDateTime ($row1 [0]);   
             }
             else
             {
                $clientInfo->m_LastEvent = 0;      
             }

         }
         else
         {
             $clientInfo->m_Company .= ', ' . $row [5];
         }
    }  
    
    if ($bDataRead) mysql_free_result ($result);

    return ecOK;       
}

//
// Изменяет сведения о клиенте.
//
//      $dbConnection - дескриптор соединения с базой данных.
//      $loginID      - идентификатор логина диспетчера.
//      $clientInfo   - описание клиента.
//
// Возвращает код завершения. 
//

function UpdateClientInfo ($dbConnection, $loginID, $clientInfo)
{
    //
    // Проверяем, есть ли такой клиент и имеет ли данный логин право на 
    // работу с таким клиентом.
    //

    $clientId = $clientInfo->m_ClientId;
    $query = "SELECT CLIENTS.ID " .
             "FROM DOMAINS, COMPANY_MEMBERS, CLIENTS " .
             "WHERE " .
             "DOMAINS.FKLOGINS={$loginID} AND ".
             "DOMAINS.FKCOMPANY=COMPANY_MEMBERS.FKCOMPANY AND " .
             "COMPANY_MEMBERS.FKCLIENTS=CLIENTS.OBJECTID AND " .
             "CLIENTS.ENABLED=1 AND " .
             "CLIENTS.ID=\"{$clientId}\""; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;

    $row = mysql_fetch_array ($result, MYSQL_NUM);
    if (! $row) return ecOK;
    mysql_free_result ($result);

    //
    // Update table.
    //

    $strFriendlyName = mysql_real_escape_string ($clientInfo->m_FriendlyName, $dbConnection);
    $strComments = mysql_real_escape_string ($clientInfo->m_Comments, $dbConnection);

    $query = "UPDATE CLIENTS SET FRIENDLYNAME=\"{$strFriendlyName}\", COMMENTS=\"{$strComments}\" " .
             "WHERE " .
             "CLIENTS.ID=\"{$clientId}\""; 

    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) return ecDBError;
                  
         
    return ecOK;
}


?>