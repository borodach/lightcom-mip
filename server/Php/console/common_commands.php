<?php
function RenderCommonCommands ($tableName)
{
    echo '<H3>Common commands.</H3>';
    if (($_SERVER ['REQUEST_METHOD'] == 'POST') && 
        ($_POST ['type'] == 'common_commands'))
    {
        $dbConnection = $GLOBALS ['dbConnection'];
        switch ($_POST ['action'])
        {
            case 'create':
                $queryLines = file ('./sql/' . $tableName . '.SQL'); 
                $query = '';
                for ($nIdx = 0; $nIdx < count ($queryLines); ++$nIdx)
                {
                    $query = $query . $queryLines [$nIdx];    
                }
                echo '<B>Last executed command result: </B>';
                $result = mysql_query ($query, $dbConnection);
                if (FALSE == $result) 
                {
                    echo "Failed to create table {$tableName}. ";
                    echo 'mySQL error: ' . mysql_error () . '<BR>';
                }
                else
                {
                    echo "Table {$tableName} created successfully.<BR>";
                }
                
                break;

            case 'drop':
                $query = "drop table {$tableName}"; 
                echo '<B>Last executed command result: </B>';
                $result = mysql_query ($query, $dbConnection);
                if (FALSE == $result) 
                {
                    echo "Failed to drop table {$tableName}. ";
                    echo 'mySQL error: ' . mysql_error () . '<BR>';
                }
                else
                {
                    echo "Table {$tableName} dropped successfully.<BR>";
                }

                break;
            case 'clear':
                $query = "delete from {$tableName}"; 
                echo '<B>Last executed command result: </B>';
                $result = mysql_query ($query, $dbConnection);
                if (FALSE == $result) 
                {
                    echo "Failed to clear table {$tableName}. ";
                    echo 'mySQL error: ' . mysql_error () . '<BR>';
                }
                else
                {
                    echo "Table {$tableName} cleared successfully.<BR>";
                }
                break;
            case 'alter':
                $queryLines = file ('./sql/ALTER_' . $tableName . '.SQL'); 
                $query = '';
                for ($nIdx = 0; $nIdx < count ($queryLines); ++$nIdx)
                {
                    $query = $query . $queryLines [$nIdx];    
                }
                echo '<B>Last executed command result: </B>';
                $result = mysql_query ($query, $dbConnection);
                if (FALSE == $result) 
                {
                    echo "Failed to alter table {$tableName}. ";
                    echo 'mySQL error: ' . mysql_error () . '<BR>';
                }
                else
                {
                    echo "Table {$tableName} changed successfully.<BR>";
                }
                
                break;
        };
    }
?>
<form name="common_commands" method="post">
<input type="hidden" name="type" id="type" value="common_commands">
<input type="hidden" name="action" id="action" value="none">
<table>
    <tr>
    <td> 
        <input type="button" value = "Create table"
         onclick="javascript:document.forms ['common_commands'].elements ['action'].value = 'create'; document.forms ['common_commands'].submit ();"
         >
    </td>

<?php
if (file_exists ('./sql/ALTER_' . $tableName . '.SQL'))
{
?>
    <td></td>
    <td> 
        <input type="button" value = "Alter table"
         onclick="javascript:document.forms ['common_commands'].elements ['action'].value = 'alter'; document.forms ['common_commands'].submit ();"
         >
    </td>
<?php
}
?>
    <td></td>
    <td> 
        <input type="button" value = "Drop table"
         onclick="javascript:document.forms ['common_commands'].elements ['action'].value = 'drop'; document.forms ['common_commands'].submit ();"
         >
    </td>
    <td></td>
    <td> 
        <input type="button" value = "Clear table"
         onclick="javascript:document.forms ['common_commands'].elements ['action'].value = 'clear'; document.forms ['common_commands'].submit ();"
         >
    </td>
    </tr>
<table>
</form>
<?php
}
?>