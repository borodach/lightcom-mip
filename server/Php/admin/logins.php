<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('logins.php');
RenderCommonCommands ('LOGINS');
?>
<BR>
<?php
ShowTable ('LOGINS', 'LOGIN');
?>
</body>
</html>