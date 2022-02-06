<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
    include ('../mysql/mysqlconnectiondetails.php');
?>
<?php
RenderHeader ('clients.php');
RenderCommonCommands ('CLIENTS');

echo '<BR>';
echo '<H3>New client creation.</H3>';

if (($_SERVER ['REQUEST_METHOD'] == 'POST') && 
    ($_POST ['type'] == 'new_client'))
{
    $bEnabled = 0;
    if ('on' == $_POST['clientEnabled']) $bEnabled = 1;

    $dbConnection = mysql_connect ($mysqlHost, 
                                   $mysqlUser, 
                                   $mysqlPassword);
    if (FALSE != $dbConnection) 
    {
        if ( FALSE != mysql_select_db ($mysqlDB))
        {

            $query = "insert into CLIENTS (ID, PRODUCT_NAME, PRODUCT_VERSION, ENABLED, FRIENDLYNAME, COMMENTS) VALUES ('{$_POST['clientId']}', '{$_POST['productName']}', '{$_POST['productVersion']}', {$bEnabled}, '{$_POST['friendlyName']}', '{$_POST['comments']}')"; 

            echo '<B>Last executed command result: </B>';
            $result = mysql_query ($query, $dbConnection);
            if (FALSE == $result) 
            {
                echo "Failed to create a new client. ";
                echo 'mySQL error: ' . mysql_error () . '<BR>';
            }
            else
            {
                echo "New client created successfully.<BR>";
            }
        }
        else
        {
            echo "Failed to select database {$mysqlDB}. ";
            echo 'mySQL error: ' . mysql_error () . '<BR>';            
        }
    }
    else
    {
        echo "Failed to connect to mySQL server. ";
        echo 'mySQL error: ' . mysql_error () . '<BR>';            
    }
}       
?>
<form name="new_client" method="post">
<input type="hidden" name="type" id="type" value="new_client">
<table>
    <tr>
    <td>Client ID:</td>         
    <td><input type="input" name="clientId" id="clientId"></td>
    </tr>

    <tr>
    <td>Product name:</td>         
    <td><input type="input" name="productName" id="productName"></td>
    </tr>

    <tr>
    <td>Product version:</td>         
    <td><input type="input" name="productVersion" id="productVersion"></td>
    </tr>

    <tr>
    <td>Enabled:</td>         
    <td><input type="checkbox" CHECKED name="clientEnabled" id="clientEnabled"></td>
    </tr>

    <tr>
    <td>Friendly name:</td>         
    <td><input type="input" name="friendlyName" id="friendlyName"></td>
    </tr>

    <tr>
    <td>Comments:</td>         
    <td><input type="input" name="comments" id="comments"></td>
    </tr>
    
    <tr>
    <td> 
        <input type="submit" value = "Create">
    </td>
    <td></td>
    </tr>
<table>
</form>
<BR>
<?php
ShowTable ('CLIENTS', 'ID');
?>

</body>
</html>