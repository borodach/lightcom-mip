<?php
include_once ('connect.php');
include_once ('utils.php');

class CompanyList
{
    function Format ($format, $argv)
    {

        $query = 'SELECT DISTINCT COMPANY.COMPANY_NAME, COMPANY.OBJECTID FROM COMPANY, COMPANY_MEMBERS WHERE COMPANY_MEMBERS.FKCOMPANY=COMPANY.OBJECTID AND COMPANY_MEMBERS.FKCLIENTS=' . $argv [0];
        $connectionCount = mysql_query ($query, $GLOBALS ['dbConnection']);
        $bEmpty = true;
        $result = "<TABLE>\n";
        while ($row = mysql_fetch_array ($connectionCount, MYSQL_NUM))
        {
            $bEmpty = false;
            $result .= "<TR><TD>";
            $strName = htmlspecialchars ($row [0]);
            $result .= "<A href=\"edit_company.php?id={$row[1]}\">$strName</A>";
            $result .= "</TD></TR>\n";

        }
        mysql_free_result ($connectionCount);
        if ($bEmpty) $result = 'empty';
        else $result .= "</TABLE>\n";
        
        return $result;
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

class LastContact
{
    function Format ($format, $argv)
    {

        $query = 'SELECT PUBLISH_TIME FROM LAST_PUBLISH_TIME WHERE LAST_PUBLISH_TIME.FKCLIENTS=' . $argv [0];
        $connectionCount = mysql_query ($query, $GLOBALS ['dbConnection']);

        if ($row = mysql_fetch_array ($connectionCount, MYSQL_NUM))
        {
            mysql_free_result ($connectionCount);
            return $row [0];
        }
        else
        {
            mysql_free_result ($connectionCount);
            return "empty";
        }
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
RenderHeader ('Мобильные объекты');
echo '<BODY>';
RenderMenu ('endpoint.php');
?>
<TABLE cellSpacing=1 cellPadding=3 border=1>
<?php

$query = 'SELECT * FROM CLIENTS ORDER BY ID';
if (array_key_exists ('id', $_GET))
{
    $id = $_GET['id'];
    $query = "SELECT DISTINCT * FROM CLIENTS, COMPANY_MEMBERS WHERE CLIENTS.OBJECTID=COMPANY_MEMBERS.FKCLIENTS AND COMPANY_MEMBERS.FKCOMPANY={$id} ORDER BY ID";
}

$titles = array ('ID', 'TextID', 'Name', 'Companies', 'Product', 'Version', 'Enabled', 'Last contact', 'Comments');
$names = array ('OBJECTID', 'ID', 'PRODUCT_NAME', 'PRODUCT_VERSION', 'ENABLED', 'FRIENDLYNAME', 'COMMENTS');
$patterns = array ("%1\$d",
                   "<A href=\"edit_endpoint.php?id=%1\$d\">%2\$s</A>",
                   "%6\$s",
                   "",
                   "%3\$s",
                   "%4\$s",
                   "",
                   "",
                   "%7\$s",);
$formatters = array (NULL,
                     NULL,
                     NULL,
                     new CompanyList (),
                     NULL,
                     NULL,
                     new BooleanData (),
                     new LastContact (),
                     NULL);


RenderTable ($query, $titles, $names, $patterns, $formatters);
?>
</TABLE>
</BODY></HTML>