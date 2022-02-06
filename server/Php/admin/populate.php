<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<META http-equiv=Content-Type content="text/html; charset=windows-1251">
<HEAD><TITLE>Populate tables with test data</TITLE>
<body>
<?php
include ('../mysql/mysqlconnectiondetails.php');

//
//Connaect to database.
//

$dbConnection = mysql_connect ($mysqlHost, 
                               $mysqlUser, 
                               $mysqlPassword);
if (FALSE == $dbConnection) 
{
    echo "Failed to connect to mySQL server. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';            

    return;
}
if ( FALSE == mysql_select_db ($mysqlDB))
{
    echo "Failed to select database {$mysqlDB}. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';            

    return;
}

//
//Clear tables.
//

$tables = array ('COMPANY', 'COMPANY_MEMBERS', 'CONNECTIONS', 'DOMAINS', 'LOGINS');
while ($table_name = current ($tables)) 
{
    $query = "DELETE FROM  {$table_name}"; 
    $result = mysql_query ($query, $dbConnection);
    if (FALSE == $result) 
    {
        echo "Failed to clear {$table_name} table. ";
        echo 'mySQL error: ' . mysql_error () . '<BR>';

        return;
    }
    next ($tables);
}

//
//Add company.
//

$query = 'INSERT INTO COMPANY (COMPANY_NAME, PHONE, EMAIL, CONTACT, ADDRESS, COMMENTS) VALUES ("MIP", "2112233", "mip@mip.ru", "CEO", "Company address", "Test company entry")'; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to populate COMPANY table. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}

$query = 'SELECT LAST_INSERT_ID()'; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to get primary key of COMPANY. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}
$row = mysql_fetch_array ($result, MYSQL_NUM);  
$companyId = $row [0];

//
//Add login.
//

$query = 'INSERT INTO LOGINS (LOGIN, DOMAIN, PASSWORD, CAPACITY, ENABLED, COMMENTS) VALUES ("admin",  "Administration", "password", 100, 1, "Administrative login")'; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to populate LOGINS table. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}
$query = 'SELECT LAST_INSERT_ID()'; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to get primary key of LOGIN. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}
$row = mysql_fetch_array ($result, MYSQL_NUM);  
$loginId = $row [0];


//
//Register domain.
//

$query = "INSERT INTO DOMAINS (FKLOGINS, FKCOMPANY) VALUES ('{$loginId}', '{$companyId}')"; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to populate DOMAINS table. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}


//
//Populate company.
//

$query = 'SELECT OBJECTID FROM CLIENTS WHERE ID="client"'; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to get primary key of client. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}
$row = mysql_fetch_array ($result, MYSQL_NUM);  
$clientId = $row [0];

$query = "INSERT INTO COMPANY_MEMBERS (FKCLIENTS, FKCOMPANY) VALUES ({$clientId}, {$companyId})"; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to populate LOGINS table. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}


//
//Update Clients.
//

$query = "UPDATE CLIENTS SET FRIENDLYNAME=\"Kud's car\", COMMENTS='Test mobile client'"; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to update CLIENTS table. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}

//
//Add connaection.
//
/*
$query = "INSERT INTO CONNECTIONS (FKLOGINS, LAST_ACCESS, EKEY, DKEY) VALUES ({$loginId}, now(), 'khKUH769ZiouY&*IUH76Jh9*', 'lkjOuo8uoH(*hihy76hjkjh76GHJH')"; 
$result = mysql_query ($query, $dbConnection);
if (FALSE == $result) 
{
    echo "Failed to add CONNECTION. ";
    echo 'mySQL error: ' . mysql_error () . '<BR>';

    return;
}
*/
echo 'OK';

?>
</body>
</html>