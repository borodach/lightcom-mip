<?php
include_once ('connect.php');
include_once ('utils.php');
if (! MakeDBConnection ())
{
    header ('Location: logon.php');
    //include_once ('logon.php');
    return;
}
include_once ('html_header.php');
include_once ('main_menu.php');
RenderHeader ('Edit account');
echo '<BODY>';
RenderMenu ('edit_account.php');

$strTable = 'LOGINS';
$strKeyColumn = "OBJECTID";
$columnData = array ('DOMAIN', 
                     'LOGIN',
                     'CAPACITY',    
                     'ENABLED',
                     'PASSWORD',
                     'COMMENTS');
$id = NULL;
if (strcasecmp ('POST', $_SERVER ['REQUEST_METHOD']) == 0)
{
    $messages = NULL;
    $id = ProcessEditCommand ($strTable, $strKeyColumn, $columnData, $messages);
    if (! is_null ($messages))
    {
        $nCount = count ($messages);
        for ($i = 0; $i < $nCount; ++ $i)
        {
            echo $messages [$i];
            echo '<BR>';
        } 
    }
}
if (strcasecmp ('GET', $_SERVER ['REQUEST_METHOD']) == 0)
{
    if (array_key_exists ('id', $_GET))
    {
        $id = $_GET ['id'];        
    }
}
ProcessRelationData ('DOMAINS', 'FKLOGINS', 'FKCOMPANY', $id);
?>
<FORM action="edit_account.php" method="post" enctype="application/x-www-form-urlencoded" name="edit_account">
<TABLE cellSpacing=0 cellPadding=0 border=0>
<?php

$strQuery = '';
if (! is_null ($id))
{
    $strQuery = 'SELECT * FROM LOGINS WHERE OBJECTID=' . $id;    
}
$strKeyColumnTitle = 'OBECTID';
$columnTitles = array ('Domain',
                       'Login',
                       'Capacity',
                       'Enabled',
                       'Password',
                       'Comments');

$valuePattern = array ("<INPUT name=\"DOMAIN\" type=\"text\" id=\"DOMAIN\" size=\"64\" maxlength=\"64\" value=\"%1\$s\">",
                       "<INPUT name=\"LOGIN\" type=\"text\" id=\"LOGIN\" size=\"64\" maxlength=\"64\" value=\"%2\$s\">",
                       "<INPUT name=\"CAPACITY\" type=\"text\" id=\"CAPACITY\" size=\"64\" maxlength=\"64\" value=\"%3\$s\">",
                       "<INPUT name=\"ENABLED\" type=\"text\" id=\"ENABLED\" size=\"1\" maxlength=\"1\" value=\"%4\$s\">",
                       "<INPUT name=\"PASSWORD\" type=\"text\" id=\"PASSWORD\" size=\"64\" maxlength=\"64\" value=\"%5\$s\">",
                       "<TEXTAREA name=\"COMMENTS\" id=\"COMMENTS\" cols=\"67\" rows=\"4\">%6\$s</TEXTAREA>");

RenderEditTable ($strQuery,
                 $strKeyColumnTitle, 
                 $id,
                 $columnData,
                 $columnTitles,
                 $valuePattern);
?>
<TR>
<TD valign="top">
    <B>Relationship with companies</B>
</TD>
<TD></TD><TD></TD>
</TR>
<TR>
<TD></TD><TD></TD>
<TD>
<TABLE cellSpacing=1 cellPadding=3 border=1 width="100%">
<?php
$titles = array ('Company', 'Related?');
$columnData = array ('OBJECTID', 'COMPANY_NAME');
RenderRelationData ($id,
                    $titles,
                    $columnData,
                    'COMPANY', 
                    "<A href=\"edit_company.php?id=%1\$d\">%2\$s</A>", 
                    'DOMAINS', 
                    'FKLOGINS',
                    'FKCOMPANY');
?>
</TABLE>
</TD></TR>
    <TR> 
        <TD>&nbsp</TD>
        <TD>&nbsp</TD>
        <TD align="right">
            <BR>
            <INPUT name="EDIT_COMMAND" type="HIDDEN" id="EDIT_COMMAND" value="INSERT">
            <INPUT name="insert" type="BUTTON" id="insert" value="Add" onClick = "EDIT_COMMAND.value='INSERT'; submit ();">
<?php if (! is_null ($id))
{
?>
            <INPUT name="update" type="BUTTON" id="update" value="Update" onClick = "EDIT_COMMAND.value='UPDATE'; submit ();">
            <INPUT name="delete" type="BUTTON" id="delete" value="Delete" onClick = "EDIT_COMMAND.value='DELETE'; submit ();">
<?php 
}
?>
            <INPUT name="cancel" type="button" id="cancel" value="Cancel" onClick="navigate ('companies.php');">
        </TD>
    </TR>
</TABLE>
</FORM>
</BODY></HTML>