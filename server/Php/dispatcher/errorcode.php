<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           errorcode.php
//
//  Facility:       Обработка ошибок.
//
//
//  Abstract:       Коды ошибок.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  17/12/2005
//
///////////////////////////////////////////////////////////////////////////////

define ('ecOK',                 0);
define ('ecUnsupportedVersion', 1);
define ('ecBadFormat',          2);
define ('ecCheckSum',           3);
define ('ecCryptoError',        4);
define ('ecIvalidType',         5);
define ('ecFileIOError',        6);
define ('ecNoConnection',       7);
define ('ecDBConnectionFailed', 8);
define ('ecLogoutFailed',       9);
define ('ecDBError',            10);
define ('ecLoginDisabled',      11);
define ('ecInvalidCredentials', 12);
define ('ecNoFreeLicenses',     13);
?>