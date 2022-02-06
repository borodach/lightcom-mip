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
RenderHeader ('RAW_GPS_DATA table');
echo '<BODY>';
RenderMenu ('raw_gps_data_table.php');
RenderCommonCommands ('RAW_GPS_DATA');
echo '<BR>';
ShowTable ('RAW_GPS_DATA', 'PUBLISH_TIME DESC');
?>
</BODY>
</HTML>