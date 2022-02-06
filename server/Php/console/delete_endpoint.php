<?php
include_once ('connect.php');
include_once ('utils.php');
if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    //include_once ('logon.php');
    return;
}
include_once ('html_header.php');
include_once ('main_menu.php');
RenderHeader ('Delete client');
echo '<BODY>';
RenderMenu ('delete_endpoint.php');

$ids = ExtractIdFromPostRequest ('del_');
$result = DeleteRecords ('CLIENTS', $ids);
if (! is_null ($result))
{
    $nCount = count ($result);
    for ($i = 0; $i < $nCount; ++ $i)
    {
        echo $result [$i];
        echo '<BR>';
    } 
}
?>
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="delete_endpoint.php">
<TABLE>
<TR><TD>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php
$query = 'SELECT * FROM CLIENTS ORDER BY ID';
$titles = array ('ID', 'TextID', 'Name', 'Delete?');
$names = array ('OBJECTID', 'ID', 'FRIENDLYNAME');
$patterns = array ("%1\$d",
                   "<A href=\"edit_endpoint.php?id=%1\$d\">%2\$s</A>",
                   "%3\$s",
                   "<INPUT type=\"checkbox\" id=\"del_%1\$d\" name=\"del_%1\$d\">");
RenderTable ($query, $titles, $names, $patterns);
?>
</TABLE>
</TD></TR>
<TR>
    <TD align="right">
        <INPUT name="delete" type="button" id="delete" value="Delete" onClick="if (confirm ('Delete selected accounts and related data?')) submit ();">
        <INPUT name="cancel" type="button" id="cancel" value="Cancel" onClick="navigate ('endpoint.php');">
    </TD>

</TR>
</TABLE>
</FORM>
</BODY></HTML>