<?php
include_once ('connect.php');
include_once ('utils.php');

class ClientUsage
{
    function Format ($format, $argv)
    {
        $query = 'SELECT COUNT(DISTINCT CLIENTS.OBJECTID) FROM COMPANY_MEMBERS,CLIENTS WHERE COMPANY_MEMBERS.FKCOMPANY=' . $argv [0];
        $query .= ' AND COMPANY_MEMBERS.FKCLIENTS=CLIENTS.OBJECTID AND CLIENTS.ENABLED=1';
        $enabledClientsCount = mysql_query ($query, $GLOBALS ['dbConnection']);
        $data = mysql_fetch_array ($enabledClientsCount, MYSQL_NUM);  
        $nEnabledCount = $data [0];
        mysql_free_result ($enabledClientsCount);

        $query = 'SELECT COUNT(DISTINCT COMPANY_MEMBERS.FKCLIENTS) FROM COMPANY_MEMBERS WHERE FKCOMPANY=' . $argv [0];
        $clientsCount = mysql_query ($query, $GLOBALS ['dbConnection']);
        $data = mysql_fetch_array ($clientsCount, MYSQL_NUM);  
        echo mysql_error ();
        $nCount = $data [0];
        mysql_free_result ($clientsCount);        

        return "<A href=\"endpoint.php?id={$argv[0]}\">{$nEnabledCount}/{$nCount}</A>";        
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
RenderHeader ('Компании');
echo '<BODY>';
RenderMenu ('companies.php');
?>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php
$query = 'SELECT * FROM COMPANY ORDER BY COMPANY_NAME';

$titles = array ('ID', 'Name', 'Clients', 'Phone', 'eMail', 'Contact', 'Address', 'Comments');
$names = array ('OBJECTID', 'COMPANY_NAME', 'PHONE', 'EMAIL', 'CONTACT', 'ADDRESS', 'COMMENTS');
$patterns = array ("%1\$d",
                   "<A href=\"edit_company.php?id=%1\$d\">%2\$s</A>",
                   "",
                   "%3\$s",
                   "%4\$s",
                   "%5\$s",
                   "%6\$s",
                   "%7\$s");
$formatters = array (NULL,
                     NULL,
                     new ClientUsage (),
                     NULL,
                     NULL,
                     NULL,
                     NULL,
                     NULL);

RenderTable ($query, $titles, $names, $patterns, $formatters);
?>
</TABLE>
</BODY></HTML>
