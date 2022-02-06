<?php
    include_once ('connect.php');

if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    return;
}
?>

<HTML>
<META http-equiv=Content-Type content="text/html; charset=utf-8">
<HEAD><TITLE>Result frame</TITLE>
<BODY>
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="query_result">
<?php

if (array_key_exists ('query', $_POST)) 
{
    $query = $_POST ['query'];
    if ('' == $query)
    {
        echo '<INPUT TYPE="hidden" ID="query" NAME="query">';
        echo 'Query is empty';

        return;
    }

    $result = mysql_query ($query, $GLOBALS ['dbConnection']);
    if (FALSE == $result)
    {
        echo '<INPUT TYPE="hidden" ID="query" NAME="query">';
        echo 'Error: ' . mysql_error ();

        return;
    }

    if (mysql_num_rows ($result) > 0)
    {
        echo "<TABLE cellSpacing=1 cellPadding=3 border=1>\n";
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
                echo '<td>' . htmlspecialchars ($val) . '</td>';
            }
            echo "</tr>\n";
        }
         echo "</table>\n";
    }
    else
    {
        echo 'Table is empty';
    }

    echo "<INPUT TYPE=\"hidden\" ID=\"query\" NAME=\"query\" VALUE=\"{$query}\">";
}
else
{
    echo '<INPUT TYPE="hidden" ID="query" NAME="query">';
}
?>
</FORM>
</BODY>
</HTML>