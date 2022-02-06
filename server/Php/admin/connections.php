<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('connections.php');
RenderCommonCommands ('CONNECTIONS');
?>
<BR>
<?php
ShowTable ('CONNECTIONS', 'FKLOGINS');
?>
</body>
</html>