<?php
    include ('render_header.php');
    include ('common_commands.php');
    include ('show_table.php');
?>
<?php
RenderHeader ('company_members.php');
RenderCommonCommands ('COMPANY_MEMBERS');
?>
<BR>
<?php
ShowTable ('COMPANY_MEMBERS', 'FKCLIENTS');
?>
</body>
</html>