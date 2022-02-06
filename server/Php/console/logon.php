<?php 
session_start ();
if (array_key_exists ('dbserver', $_POST) &&
    array_key_exists ('database', $_POST) &&
    array_key_exists ('user', $_POST) &&
    array_key_exists ('password', $_POST))
{
    $_SESSION ['dbserver'] = $_POST ['dbserver'];
    $_SESSION ['database'] = $_POST ['database'];
    $_SESSION ['user'] = $_POST ['user'];
    $_SESSION ['password'] = $_POST ['password'];

    $GLOBALS ['dbConnection'] = mysql_connect ($_SESSION ['dbserver'], 
                                               $_SESSION ['user'], 
                                               $_SESSION ['password']);
    if (FALSE != $GLOBALS ['dbConnection']) 
    {
        if ( FALSE != mysql_select_db ($_SESSION ['database'], $GLOBALS ['dbConnection']))
        {
            //echo "Location: companies.php?" . session_name () . "=" . session_id ();
            header ("Location: companies.php?" . session_name () . "=" . session_id ());
            //include_once ('companies.php');
            return;
        }
    }   
    unset ($GLOBALS ['dbConnection']);
    session_write_close ();
}
include_once ('html_header.php');
RenderHeader ('Вход в систему');
echo '<BODY>';
?>
<H2>Вход в систему</H2>
<FORM action="" method="post" enctype="application/x-www-form-urlencoded" name="logon">
    <TABLE width="100%" border="0">
        <TR> 
            <TD><STRONG>Сервер:</STRONG></TD>
            <TD><INPUT name="dbserver" type="text" id="server" value="localhost" size="64"> 
            </TD>
        </TR>
        <TR> 
            <TD><STRONG>База данных:</STRONG></TD>
            <TD><INPUT name="database" type="text" id="dbserver" value="gps" size="64"></TD>
        </TR>
        <TR> 
            <TD><STRONG>Пользователь:</STRONG></TD>
            <TD><INPUT name="user" type="text" id="dtabase" value="Ваше имя" size="64"></TD>
        </TR>
        <TR>
            <TD><STRONG>Пароль:</STRONG></TD>
            <TD><INPUT name="password" type="password" id="password" size="64"></TD>
        </TR>
        <TR> 
            <TD>&nbsp;</TD>
            <TD><INPUT name="logon" type="submit" id="logon" value="Войти"></TD>
        </TR>
    </TABLE>
</FORM>
</BODY>
</HTML>