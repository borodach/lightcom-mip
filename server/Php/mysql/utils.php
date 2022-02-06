<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           utils.php
//
//  Facility:       Работа с MySQL.
//
//
//  Abstract:       Функции для работы MySQL.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  20/12/2005
//
///////////////////////////////////////////////////////////////////////////////

//
// Конвертирует дату/время из формата MySQL в формат PHP.
//
//      $strDateTime - дата/время в формате MySQL.
//
//      Возвращает дату/время в формате PHP.
//

function ParseMySQLDateTime ($strDateTime)
{
    //Y-m-d H:i:s
    sscanf ($strDateTime, '%d-%d-%d %d:%d:%d', 
            $nYear, $nMonth, $nDay, $nHour, $nMinute, $nSecond);
    return gmmktime ($nHour, $nMinute, $nSecond, $nMonth, $nDay, $nYear);
}
?>