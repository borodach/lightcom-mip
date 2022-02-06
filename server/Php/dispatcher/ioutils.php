<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           ioutils.php
//
//  Facility:       �������� �������������� � ������������.
//
//
//  Abstract:       ������� ������� �����/������.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  16/12/2005
//
///////////////////////////////////////////////////////////////////////////////

//
//��� ������.
//

define ('rtOK', 1);
define ('rtError', 2);

//
//����� ������, ���������� ����/�����.
//

define ('nDateTimeLength', 23);

//
// ���������� ����� �������.
//
//      $response   - �����.
//
// ������ �� ����������.
//
//

function WriteResponse ($response)
{
    $hOut = fopen ('php://output', 'wb');
    if (NULL == $hOut) 
    {
        return;
    }
    fwrite ($hOut, bin2hex ($response));    
}

//
// ��������� � ����� ������ ����� �����.
//
//      $strBuffer - ������, � ������� ������������ ��������.
//      $value     - ������������ ��������.
//
// ������ �� ����������.
//
//

function WriteInt (&$strBuffer, $value)
{
    $strBuffer .= pack ('V', $value);
}

//
// ��������� �� ������ ������ ����� �����. ����������� ����� ��������� ��
// ������.
//
//      $strBuffer - ������, �� ������� ����������� ��������.
//
// ����������� ��������.
//
//

function ReadInt (&$strBuffer)
{
    
    $val = unpack ('Vval', $strBuffer);
    $strBuffer = substr ($strBuffer, 4);
    return $val ['val'];
}

//
// ��������� � ����� ������ �������.
//
//      $strBuffer - ������, � ������� ������������ ��������.
//      $value     - ������������ ��������. ������ ���� � ��������� UTF8.
//
// ������ �� ����������.
//
//

function WriteString (&$strBuffer, $value)
{
    $nLen = strlen ($value);
    WriteInt ($strBuffer, $nLen);
    $strBuffer .= $value;
}

//
// ��������� �� ������ ������ �������. ����������� ������� ��������� ��
// ������.
//
//      $strBuffer - ������, �� ������� ����������� ��������.
//
// ����������� �������� (��������� �� ��������).
//
//

function ReadString (&$strBuffer)
{
    $nLen = ReadInt ($strBuffer);
    if ($nLen <= 0) return '';
    $strResult = substr ($strBuffer, 0, $nLen);
    $strBuffer = substr ($strBuffer, $nLen);

    return $strResult;

}

//
// ��������� � ����� ������ ����/�����.
//
//      $strBuffer - ������, � ������� ������������ ��������.
//      $value     - ������������ ��������.
//
// ������ �� ����������.
//
//

function WriteDateTime (&$strBuffer, $value)
{
    $strVal = utf8_encode (gmdate ('d.m.Y H:i:s.000', $value));    
    WriteString ($strBuffer, $strVal);
}

//
// ��������� �� ������ ������ ����/�����. ����������� ����/����� ��������� ��
// ������.
//
//      $strBuffer - ������, �� ������� ����������� ��������.
//
// ����������� ��������.
//
//

function ReadDateTime (&$strBuffer)
{
    $strVal = utf8_decode (ReadString ($strBuffer));
    if (strlen ($strVal) != nDateTimeLength) return 0;

    //("dd.MM.yyyy HH:mm:ss.fff");
    $nDay = 0 + substr ($strVal, 0, 2);
    $nMonth = 0 + substr ($strVal, 3, 2);
    $nYear = 0 + substr ($strVal, 6, 4);
    $nHour = 0 + substr ($strVal, 11, 2);
    $nMinute = 0 + substr ($strVal, 14, 2);
    $nSecond = 0 + substr ($strVal, 17, 2);
    $nMillisecond = 0 + substr ($strVal, 20, 3);

    return gmmktime ($nHour, $nMinute, $nSecond, $nMonth, $nDay, $nYear);

}

function test_ioutils ()
{
?>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<META http-equiv=Content-Type content="text/html; charset=windows-1251">
<HEAD><TITLE>Test page</TITLE>
<body>
<?php

$val = time ();
$val1 = $val + 600;
echo '<BR>Current time: ' .  gmdate  ('d.m.Y H:i:s', $val);
echo '<BR>In 10 minutes: ' .  gmdate  ('d.m.Y H:i:s', $val1);
$strVal = '';
WriteDateTime ($strVal, $val);
echo '<BR>Encoded1: ' .  bin2hex ($strVal);
WriteDateTime ($strVal, $val1);
echo '<BR>Encoded2: ' .  bin2hex ($strVal);

$v = ReadDateTime ($strVal);
echo '<BR>Decoded1: ' .  bin2hex ($strVal);
$v1 = ReadDateTime ($strVal);
echo '<BR>gmdate2: ' .  bin2hex ($strVal);
echo '<BR>Current time: ' .  gmdate  ('d.m.Y H:i:s', $v);
echo '<BR>In 10 minutes: ' .  gmdate  ('d.m.Y H:i:s', $v1);

?>
</body>
</html>
<?php
}
?>
