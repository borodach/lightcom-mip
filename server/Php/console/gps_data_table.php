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
RenderHeader ('GPS_DATA table');
echo '<BODY>';
RenderMenu ('gps_data_table.php');
RenderCommonCommands ('GPS_DATA');
echo '<BR>';
ShowTable ('GPS_DATA', 'PUBLISH_TIME DESC');
?>
</BODY>
</HTML>