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
RenderHeader ('CONNECTIONS table');
echo '<BODY>';
RenderMenu ('connections_table.php');
RenderCommonCommands ('CONNECTIONS');
echo '<BR>';
ShowTable ('CONNECTIONS', 'LAST_ACCESS DESC');
?>
</BODY>
</HTML>