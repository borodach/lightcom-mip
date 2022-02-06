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
RenderHeader ('SQL console');
echo '<BODY>';
RenderMenu ('console.php');
?>
<A href="console_frameset.html">Launch SQL console<A>
</BODY></HTML>

