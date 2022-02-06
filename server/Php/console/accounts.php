<?php
include_once ('connect.php');
include_once ('utils.php');

class AccountUsage
{
    function Format ($format, $argv)
    {
        $query = 'SELECT COUNT(CONNECTIONS.FKLOGINS) FROM CONNECTIONS WHERE CONNECTIONS.FKLOGINS=' . $argv [0];
        $connectionCount = mysql_query ($query, $GLOBALS ['dbConnection']);
        $data = mysql_fetch_array ($connectionCount, MYSQL_NUM);  
        $nConnectionsCount = $data [0];
        mysql_free_result ($connectionCount);

        return "<A href=\"connections.php?id={$argv [0]}\">" . $nConnectionsCount . '/' . $argv [3] . '</A>';
    }
};

class BooleanData
{
    function Format ($format, $argv)
    {
        if ( $argv [4]) return 'true';
        else return 'false';
    }
};

if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    //include_once ('logon.php');
    return;
}
include_once ('html_header.php');
include_once ('main_menu.php');
RenderHeader ('Accounts');
echo '<BODY>';
RenderMenu ('accounts.php');
?>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php
$query = 'SELECT * FROM LOGINS ORDER BY DOMAIN, LOGIN';
$titles = array ('ID', 'Login', 'Usage', 'Enabled', 'Password', 'Comments');
$names = array ('OBJECTID', 'DOMAIN', 'LOGIN', 'CAPACITY', 'ENABLED', 'PASSWORD', 'COMMENTS');
$patterns = array ("%1\$d",
                   "<A href=\"edit_account.php?id=%1\$d\">%2\$s\\%3\$s</A>",
                   "",
                   "",
                   "%6\$s",
                   "%7\$s",);
$formatters = array (NULL,
                     NULL,
                     new AccountUsage (),
                     new BooleanData (),
                     NULL,
                     NULL);


RenderTable ($query, $titles, $names, $patterns, $formatters);
?>
</TABLE>
</BODY></HTML>