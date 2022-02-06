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
RenderHeader ('Lock accounts');
echo '<BODY>';
RenderMenu ('lock_account.php');

$ids = ExtractIdFromPostRequest ('upd_');
if (strcasecmp ('POST', $_SERVER ['REQUEST_METHOD']) == 0)
{
    $query = 'UPDATE LOGINS SET ENABLED=1';
    $result = mysql_query ($query, $GLOBALS ['dbConnection']);
    if (FALSE == $result)
    {
        echo mysql_error ();
        echo '<BR>';
    }
}


$queries = array 
(
    "UPDATE LOGINS SET ENABLED=0 WHERE OBJECTID=%d",
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
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="lock_account.php">
<TABLE>
<TR><TD>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php
$query = 'SELECT * FROM LOGINS ORDER BY DOMAIN, LOGIN';
$titles = array ('ID', 'Name', 'Lock?');
$names = array ('OBJECTID', 'DOMAIN', 'LOGIN', 'ENABLED');
$patterns = array ("%1\$d",
                   "<A href=\"edit_account.php?id=%1\$d\">%2\$s\\%3\$s</A>",
                   "");
$formatters = array (NULL,
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