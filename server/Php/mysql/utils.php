<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           utils.php
//
//  Facility:       ������ � MySQL.
//
//
//  Abstract:       ������� ��� ������ MySQL.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  20/12/2005
//
///////////////////////////////////////////////////////////////////////////////

//
// ������������ ����/����� �� ������� MySQL � ������ PHP.
//
//      $strDateTime - ����/����� � ������� MySQL.
//
//      ���������� ����/����� � ������� PHP.
//

function ParseMySQLDateTime ($strDateTime)
{
    //Y-m-d H:i:s
    sscanf ($strDateTime, '%d-%d-%d %d:%d:%d', 
            $nYear, $nMonth, $nDay, $nHour, $nMinute, $nSecond);
    return gmmktime ($nHour, $nMinute, $nSecond, $nMonth, $nDay, $nYear);
}
?>