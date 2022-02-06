<?php
include_once ('connect.php');
include_once ('utils.php');

class LockRecord
{
    function Format ($format, $argv)
    {
        $check = "";
        if (0 == $argv [3]) $check = " CHECKED";
        return "<INPUT type=\"checkbox\" name=\"upd_{$argv[0]}\"{$check}>";    
    }
};

if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    //include_once ('logon.php');
    return;
}
include_once ('html_header.php');
include_once ('main_menu.php');
RenderHeader ('Lock mobile clients');
echo '<BODY>';
RenderMenu ('lock_endpoint.php');

$ids = ExtractIdFromPostRequest ('upd_');
if (strcasecmp ('POST', $_SERVER ['REQUEST_METHOD']) == 0)
{
    $query = 'UPDATE CLIENTS SET ENABLED=1';
    $result = mysql_query ($query, $GLOBALS ['dbConnection']);
    if (FALSE == $result)
    {
        echo mysql_error ();
        echo '<BR>';
    }
}


$queries = array 
(
    "UPDATE CLIENTS SET ENABLED=0 WHERE OBJECTID=%d",
);
$result = BatchExecuteSQL ($ids, $queries);
if (NULL  != $result)
{
    $nCount = count ($result);
    for ($i = 0; $i < $nCount; ++ $i)
    {
        echo $result [$i];
        echo '<BR>';
    } 
}
?>
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="lock_endpoint.php">
<TABLE>
<TR><TD>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php
$query = 'SELECT * FROM CLIENTS ORDER BY ID';
$titles = array ('ID', 'TextID', 'Name', 'Lock?');
$names = array ('OBJECTID', 'ID', 'FRIENDLYNAME', 'ENABLED');
$patterns = array ("%1\$d",
                   "<A href=\"edit_endpoint.php?id=%1\$d\">%2\$s</A>",
                   "%3\$s",
                   "");
$formatters = array (NULL,
                     NULL,
                     NULL,
                     new LockRecord ());
RenderTable ($query, $titles, $names, $patterns, $formatters);
?>
</TABLE>
</TD></TR>
<TR>
    <TD align="right">
        <INPUT name="lock" type="button" id="lock" value="Lock" onClick="submit ();">
        <INPUT name="cancel" type="button" id="cancel" value="Cancel" onClick="navigate ('endpoint.php');">
    </TD>

</TR>
</TABLE>
</FORM>
</BODY></HTML>