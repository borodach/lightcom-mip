<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('raw_gps_data.php');
RenderCommonCommands ('RAW_GPS_DATA');
?>
<BR>
<?php
ShowTable ('RAW_GPS_DATA', 'PUBLISH_TIME DESC');
?>
</body>
</html>