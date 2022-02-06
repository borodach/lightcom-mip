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
RenderHeader ('Delete companies');
echo '<BODY>';
RenderMenu ('delete_companies.php');

$ids = ExtractIdFromPostRequest ('del_');
$result = DeleteRecords ('COMPANY', $ids);
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
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="delete_companies">
<TABLE>
<TR><TD>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php
$query = 'SELECT * FROM COMPANY ORDER BY COMPANY_NAME';
$titles = array ('ID', 'Name', 'Delete?');
$names = array ('OBJECTID', 'COMPANY_NAME');
$patterns = array ("%1\$d",
                   "<A href=\"edit_company.php?id=%1\$d\">%2\$s</A>",
                   "<INPUT type=\"checkbox\" id=\"del_%1\$d\" name=\"del_%1\$d\">");
RenderTable ($query, $titles, $names, $patterns);
?>
</TABLE>
</TD></TR>
<TR>
    <TD align="right">
        <INPUT name="delete" type="button" id="delete" value="Delete" onClick="if (confirm ('Delete selected companies and related data?')) submit ();">
        <INPUT name="cancel" type="button" id="cancel" value="Cancel" onClick="navigate ('companies.php');">
    </TD>

</TR>
</TABLE>
</FORM>
</BODY></HTML>