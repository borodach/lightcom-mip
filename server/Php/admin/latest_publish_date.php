<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('latest_publish_date.php');
RenderCommonCommands ('LATEST_PUBLISH_DATE');
?><BR>
<?php
ShowTable ('GPS_DATA', 'PUBLISH_TIME DESC');
?>
</body>
</html>