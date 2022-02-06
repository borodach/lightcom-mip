<?php
$_SESSION ['dbserver'] = '';
$_SESSION ['database'] = '';
$_SESSION ['user'] ='';
$_SESSION ['password'] = '';
$GLOBALS ['dbConnection'] = FALSE;
header ('Location: logon.php');
//include ('logon.php');
?>