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
RenderHeader ('Соединения');
echo '<BODY>';
RenderMenu ('connections.php');

$ids = ExtractIdFromPostRequest ('del_');
$result = DeleteRecords ('CONNECTIONS', $ids);
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
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="delete_companies">
<TABLE>
<TR><TD>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php

$query = 'SELECT CONNECTIONS.OBJECTID, CONNECTIONS.FKLOGINS, CONNECTIONS.LAST_ACCESS, LOGINS.LOGIN, LOGINS.DOMAIN, LOGINS.CAPACITY, CONNECTIONS.HOST, CONNECTIONS.IP FROM CONNECTIONS, LOGINS WHERE CONNECTIONS.FKLOGINS=LOGINS.OBJECTID ORDER BY LOGINS.OBJECTID, CONNECTIONS.LAST_ACCESS DESC';
if (array_key_exists ('id', $_GET))
{
    $id = $_GET['id'];
    $query = "SELECT CONNECTIONS.OBJECTID, CONNECTIONS.FKLOGINS, CONNECTIONS.LAST_ACCESS, LOGINS.LOGIN, LOGINS.DOMAIN, LOGINS.CAPACITY, CONNECTIONS.HOST, CONNECTIONS.IP FROM CONNECTIONS, LOGINS WHERE CONNECTIONS.FKLOGINS=LOGINS.OBJECTID AND LOGINS.OBJECTID={$id} ORDER BY LOGINS.OBJECTID, CONNECTIONS.LAST_ACCESS DESC";
}


$titles = array ('ID', 'Last access', 'Login', 'Capacity', 'Host', 'IP', 'Drop?');
$names = array ('OBJECTID', 'FKLOGINS', 'LAST_ACCESS', 'DOMAIN', 'LOGIN', 'CAPACITY', 'HOST', 'IP');
$patterns = array ("%1\$d",
                   "%3\$s",
                   "<A href=\"edit_account.php?id=%2\$d\">%4\$s\\%5\$s</A>",
                   "%6\$d",
                   "%7\$s",
                   "%8\$s",
                   "<INPUT type=\"checkbox\" name=\"del_%1\$d\">"
                   );
RenderTable ($query, $titles, $names, $patterns);
?>
</TABLE>
</TD></TR>
<TR>
    <TD align="right">
        <INPUT name="drop" type="button" id="drop" value="Drop" onClick="if (confirm ('Drop selected connections?')) submit ();">
    </TD>
</TR>
</TABLE>
</FORM>
</BODY></HTML>