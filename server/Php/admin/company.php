<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('company.php');
RenderCommonCommands ('COMPANY');
?>
<BR>
<?php
ShowTable ('COMPANY', 'COMPANY_NAME');
?>
</body>
</html>