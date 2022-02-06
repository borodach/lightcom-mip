<?php
session_start();
function MakeDBConnection ()
{
    if (array_key_exists ('dbConnection', $GLOBALS)) 
    {
        if (FALSE != $GLOBALS ['dbConnection']) 
        {
            return TRUE;
        }
    }

    if (isset ($_SESSION  ['dbserver']) &&
        isset ($_SESSION  ['database']) &&
        isset ($_SESSION ['user']) &&
        isset ($_SESSION ['password']))
    {

        $GLOBALS ['dbConnection'] = mysql_connect ($_SESSION ['dbserver'], 
                                                   $_SESSION ['user'], 
                                                   $_SESSION ['password']);
        if (FALSE != $GLOBALS ['dbConnection']) 
        {
            if ( FALSE != mysql_select_db ($_SESSION ['database'], 
                                           $GLOBALS ['dbConnection']))
            {
                return TRUE;
            }
        }
    }

    unset ($GLOBALS ['dbConnection']);

    return FALSE;
}
?>