<?php
    include_once ('connect.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    //include_once ('logon.php');
    return;
}
include_once ('html_header.php');
include_once ('main_menu.php');
RenderHeader ('LOGINS table');
echo '<BODY>';
RenderMenu ('logins_table.php');
RenderCommonCommands ('LOGINS');
echo '<BR>';
ShowTable ('LOGINS', 'LOGIN');
?>
</BODY>
</HTML>