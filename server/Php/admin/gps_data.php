<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('gps_data.php');
RenderCommonCommands ('GPS_DATA');
?><BR>
<?php
ShowTable ('GPS_DATA', 'PUBLISH_TIME DESC');
?>
</body>
</html>