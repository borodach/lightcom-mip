<?php
include_once ('connect.php');
if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    //include_once ('logon.php');
    return;
}
include_once ('html_header.php');
include_once ('main_menu.php');
RenderHeader ('Low level table operations');
echo '<BODY>';
RenderMenu ('table_management.php');
?>
</BODY></HTML>
