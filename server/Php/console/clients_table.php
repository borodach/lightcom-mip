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
RenderHeader ('CLIENTS table');
echo '<BODY>';
RenderMenu ('clients_table.php');
RenderCommonCommands ('CLIENTS', 'ID');
echo '<BR>';
ShowTable ('CLIENTS', 'ID');
?>
</BODY>
</HTML>