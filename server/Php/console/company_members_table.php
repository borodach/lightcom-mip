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
RenderHeader ('COMPANY_MEMBERS table');
echo '<BODY>';
RenderMenu ('company_members_table.php');
RenderCommonCommands ('COMPANY_MEMBERS');
echo '<BR>';
ShowTable ('COMPANY_MEMBERS', 'FKCOMPANY');
?>
</BODY>
</HTML>