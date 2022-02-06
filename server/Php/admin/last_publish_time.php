<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('last_publish_time.php');
RenderCommonCommands ('LAST_PUBLISH_TIME');
?><BR>
<?php
ShowTable ('LAST_PUBLISH_TIME', 'FKCLIENTS');
?>
</body>
</html>