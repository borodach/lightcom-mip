<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('domains.php');
RenderCommonCommands ('DOMAINS');
?>
<BR>
<?php
ShowTable ('DOMAINS', 'FKLOGINS');
?>
</body>
</html>