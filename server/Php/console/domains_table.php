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
RenderHeader ('DOMAINS table');
echo '<BODY>';
RenderMenu ('domains_table.php');
RenderCommonCommands ('DOMAINS');
echo '<BR>';
ShowTable ('DOMAINS', 'FKLOGINS');
?>
</BODY>
</HTML>