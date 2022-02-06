<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           RequestParser.php
//
//  Facility:       Обработка запросов диспетчеров.
//
//
//  Abstract:       Набор функций для обработки запросов диспетчеров.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  13/12/2005
//
///////////////////////////////////////////////////////////////////////////////

require_once ('mysql/connections.php');
include_once ('errorcode.php');

define ('rtMinValue',               0);
define ('rtConnect',                1);
define ('rtDisconnect',             2);
define ('rtGetClientList',          3);
define ('rtGetClientsInfo',         4);
define ('rtGetEvents',              5);
define ('rtReadGPSData',            6);
define ('rtReadLatestGPSData',      7);
define ('rtUpdateMobileClientInfo', 8);
define ('rtMaxValue',               9);

define ('nInvalidConnectionId', -1);

define('nMask',          0xAA);
define('nKeySize',        32);
define('nVersion',        1);

//
// Вычисление контрольной суммы строки.
// 
//      $data    - строка, контрольную сумму которой нужно подсчитать.
//
// Возвращает контрольную сумму. 
//

function CalculateChecksum ($data)
{
    $nResult = 0;
    $nLen = strlen ($data);
    for ($nIdx = 0; $nIdx < $nLen; ++$nIdx)
    {
        $nResult += ord ($data {$nIdx});
    }

    return $nResult;
}

//
// Шифрование строки.
//
//      $data    - шифруемые данные.
//      $key     - ключ.
//
// Возвращает зашифрованные данные. В случае ошибки возыращает NULL.
//

function Encrypt ($data, $key)
{
    $result = $data;
    $nDataLen = strlen ($data);    
    $nKeyLen = strlen ($key);
    if (0 == $nKeyLen) 
    {
        return $result;
    }

    for ($nIdx = 0; $nIdx < $nDataLen; ++$nIdx)
    {
        $result [$nIdx] = chr (ord ($data {$nIdx}) ^ ord ($key {$nIdx % $nKeyLen}));
    }

    return $result;
}

//
// Дешифрование строки.
//
//      $data    - дешифруемые данные.
//      $key     - ключ.
//
// Возвращает расшифрованные данные. В случае ошибки возыращает NULL.
//

function Decrypt ($data, $key)
{
    $result = $data;
    $nDataLen = strlen ($data);    
    $nKeyLen = strlen ($key);
    if (0 == $nKeyLen) 
    {
        return $result;
    }

    for ($nIdx = 0; $nIdx < $nDataLen; ++$nIdx)
    {
        $nVal = ord ($key {$nIdx % $nKeyLen}) ^ nMask;
        $result {$nIdx} = chr (ord ($data {$nIdx}) ^ $nVal);
    }

    return $result;
}

//
// Генерирование пары ключей
//
//      $ekey    - ключ для шифрования.
//      $dkey    - ключ для расшифровки.
//
// Возвращает TRUE при успешном завершении. 
//

function GenerateKey (&$ekey, &$dkey)
{
    $ekey = str_repeat (' ', nKeySize);
    $dkey = str_repeat (' ', nKeySize);
    for ($nIdx = 0; $nIdx < nKeySize; ++$nIdx)
    {
        $nCode = mt_rand (0, 255);
        $ekey {$nIdx} = chr ($nCode);
        $dkey {$nIdx} = chr ($nCode ^ nMask);        
    }

    return TRUE;
}

//
// Создание запроса.
//
//      $type     - тип запроса. 
//      $key      - ключ для шифрования.
//      $rnd      - маркер запроса.
//      $data     - передаваемые данные.
//      $request  - сформированный запрос.
//
// Возвращает ecOK или код ошибки. 
//

function CreateRequest ($type, $key, $rnd, $data, &$request)
{

    $flatData = '' . pack ('V', $rnd) . $data;
    $nCRC = CalculateChecksum ($flatData);
    $encryptedData = Encrypt ($flatData, $key);
    if (NULL == $encryptedData) return ecCryptoError;
    $nSize = strlen ($encryptedData);
    $request = pack ('VVVV', nVersion, $type, $nCRC, $nSize) . $encryptedData;

    return ecOK;
        
}

//
// Разбор запроса.
//
//      $request        - запрос.
//      $dbConnection   - дескриптор соединения с базой даных.
//      $type           - тип запроса. 
//      $id             - идентификатор соединения. 
//      $rnd            - маркер запроса.
//      $data           - полученные данные.
//      $eKey           - ключ для шифрования ответа.
//      $loginID        - идентификатор логина пользователя.
//      $version        - версия запроса.
//            
//
// Возвращает код ошибки.
//

function ParseRequest ($request, $dbConnection, &$type, &$id, &$rnd, &$data, 
                       &$eKey, &$loginID, &$version)
{
    $nLength = strlen ($request);
    if ($nLength < 4) return ecBadFormat;

    $version = unpack ('Vversion', $request);        
    //echo $version ['version'] . ' ' . bin2hex ($request);
    $ver = $version ['version'];
    if ($ver <= 0 || $ver > nVersion) 
    {
        return ecUnsupportedVersion;
    }

    if ($nLength < 8) return ecBadFormat;
    $header = unpack ('Vver/Vtype', $request);        
    $type = $header ['type'];
    if ($type <= rtMinValue || $type >= rtMaxValue)
    {
        return ecIvalidType;
    }

    $encryptedData = '';
    $crc = 0;
    $size = 0;
    $tmpKey = '';

    if (rtConnect == $type)
    {
        if ($nLength < 16) return ecBadFormat;
        $header = unpack ('Vver/Vtype/Vcrc/Vsize', $request);        
        $id = 0;
        $crc = $header ['crc'];
        $size = $header ['size'];
        $encryptedData = substr ($request, 16);

        //
        // Read connection key.
        //

        $hData = fopen ('dispatcher/dKey.dat', 'rb');
        if (NULL == $hData) return ecCryptoError;
        $tmpKey = fread ($hData, nKeySize);
        fclose ($hData);
    }
    else
    {
        if ($nLength < 20) return ecBadFormat;
        $header = unpack ('Vver/Vtype/Vid/Vcrc/Vsize', $request);        
        $id = $header ['id'];
        $crc = $header ['crc'];
        $size = $header ['size'];
        $encryptedData = substr ($request, 20);

        $nResult = GetConnectionDetails ($dbConnection, $id, 
                                         $eKey, $tmpKey, $loginID);
        if (ecOK != $nResult)
        {
            return $nResult;
        }

        $nResult = UpdateLastAccessTime ($dbConnection, $id);
        if (ecOK != $nResult)
        {
            return $nResult;    
        }

    }

    if ($size != strlen ($encryptedData)) return ecBadFormat;

    $decryptedData = Decrypt ($encryptedData, $tmpKey);
    if (NULL == $decryptedData) return ecCryptoError;

    $nCRC = CalculateChecksum ($decryptedData);
    if ($nCRC != $crc) return ecCheckSum;

    $tmp = unpack ('Vrnd', $decryptedData);
    $rnd = $tmp ['rnd'];
    $data = substr ($decryptedData, 4);

    return ecOK;
}

//
// Debug method.
//

function test_requestparser ()
{
?>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<META http-equiv=Content-Type content="text/html; charset=windows-1251">
<HEAD><TITLE>Test page</TITLE>
<body>
<?php

$strText = "Тестовая строчка.";
echo "<A>Checksum = " . CalculateChecksum ($strText);
$ekey = "";
$dkey = "";
GenerateKey (&$ekey, &$dkey);

echo "<BR>Original text = " . htmlentities ($strText, ENT_QUOTES, "cp1251");
echo "<BR>eKey = " . bin2hex ($ekey);
echo "<BR>dKey = " . bin2hex ($dkey);
$strEncrypted = Encrypt ($strText, $ekey);
echo "<BR>Encrypted text = " . htmlentities ($strEncrypted, ENT_QUOTES, "cp1251");
$strDecrypted = Decrypt ($strEncrypted, $dkey);
echo "<BR>Decrypted text = " . htmlentities ($strDecrypted, ENT_QUOTES, "cp1251");

$request = "";
CreateRequest (rtConnect, $ekey, 1001, $strText, &$request);
echo "<BR>Resuest = " . bin2hex ($request);

$type = 0;
$id = 5;
$rnd = 0;
$data = "";
$result = ParseRequest ($request, $dkey, $type, $id, $rnd, $data);
echo "<BR>Resposnse: result = {$result}, type = {$type}, id = {$id}, rnd = {$rnd}, data = {$data}.";

?>
</body>
</html>
<?php
}

?>