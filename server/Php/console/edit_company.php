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
RenderHeader ('Компании');
echo '<BODY>';
RenderMenu ('edit_company.php');

$strTable = 'COMPANY';
$strKeyColumn = "OBJECTID";
$columnData = array ('COMPANY_NAME',
                     'PHONE',
                     'EMAIL',
                     'CONTACT',
                     'ADDRESS',
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
?>
<FORM action="edit_company.php" method="post" enctype="application/x-www-form-urlencoded" name="edit_company">
<TABLE cellSpacing=0 cellPadding=0 border=0>
<?php

$strQuery = '';
if (! is_null ($id))
{
    $strQuery = 'SELECT * FROM COMPANY WHERE OBJECTID=' . $id;    
}
$strKeyColumnTitle = 'OBECTID';
$columnTitles = array ('Company name',
                       'Phone',
                       'eMail',
                       'Contact person',
                       'Address',
                       'Comments');

$valuePattern = array ("<INPUT name=\"COMPANY_NAME\" type=\"text\" id=\"COMPANY_NAME\" size=\"64\" maxlength=\"255\" value=\"%1\$s\">",
                       "<INPUT name=\"PHONE\" type=\"text\" id=\"PHONE\" size=\"64\" maxlength=\"64\" value=\"%2\$s\">",
                       "<INPUT name=\"EMAIL\" type=\"text\" id=\"EMAIL\" size=\"64\" maxlength=\"128\" value=\"%3\$s\">",
                       "<INPUT name=\"CONTACT\" type=\"text\" id=\"CONTACT\" size=\"64\" maxlength=\"128\" value=\"%4\$s\">",
                       "<INPUT name=\"ADDRESS\" type=\"text\" id=\"ADDRESS\" size=\"64\" maxlength=\"255\" value=\"%5\$s\">",
                       "<TEXTAREA name=\"COMMENTS\" id=\"COMMENTS\" cols=\"67\" rows=\"4\">%6\$s</TEXTAREA>");

RenderEditTable ($strQuery,
                 $strKeyColumnTitle, 
                 $id,
                 $columnData,
                 $columnTitles,
                 $valuePattern);
?>
    <TR> 
        <TD>&nbsp</TD>
        <TD>&nbsp</TD>
        <TD align="right">
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