<?php
function ShowTable ($tableName, $columnToSort = "")
{
include ('../mysql/mysqlconnectiondetails.php');

        echo "<H3>{$tableName} table content.</H3>";
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

        $query = "SELECT * FROM {$tableName}"; 
        if ($columnToSort != "") $query = $query . " ORDER BY {$columnToSort}";

        $result = mysql_query ($query, $dbConnection);
        if (FALSE == $result) 
        {
            echo "Failed to get table {$tableName} content. ";
            echo 'mySQL error: ' . mysql_error () . '<BR>';

            return;
        }

        if (mysql_num_rows ($result) > 0)
        {
            echo "<table cellSpacing=1 cellPadding=3 border=1>\n";
            for ($nIdx = 0; $row = mysql_fetch_array ($result, MYSQL_BOTH); ++$nIdx) 
            {
                $nCount = count ($row);
                if (0 == $nIdx)
                {
                    echo "<tr>\n";
                    for ($nColumn = 0; $nColumn < $nCount; ++ $nColumn)
                    {
                        next ($row);
                        echo '<td align="center"><B>' . key ($row) . '</B></td>';
                        next ($row);
                        ++ $nColumn;
                    }
                    echo "</tr>\n";
                }

                echo "<tr>\n";
                for ($nColumn = 0; $nColumn < $nCount / 2; ++ $nColumn)
                {
                    $val = $row ["" . $nColumn];
                    if ('' == $val) $val = '<empty>';
                    echo '<td align="center">' . htmlspecialchars ($val) . '</td>';
                }
                echo "</tr>\n";
            }
             echo "</table>\n";
        }
        else
        {
            echo 'Table is empty';
        }

        mysql_free_result ($result);
}
?>