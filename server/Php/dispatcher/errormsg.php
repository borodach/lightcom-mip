<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           errormsg.php
//
//  Facility:       Обработка ошибок.
//
//
//  Abstract:       Текстовые описания ошибок.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  17/12/2005
//
///////////////////////////////////////////////////////////////////////////////

$errorMessages = array (ecOK => 'OK',
                        ecUnsupportedVersion => 'Unsupported request version',
                        ecBadFormat => 'Incorrect format',
                        ecCheckSum => 'Incorrect checksum',
                        ecCryptoError => 'Cryptography error',
                        ecIvalidType => 'Unsuported request type',
                        ecFileIOError => 'File IO error',
                        ecNoConnection => 'No connection',
                        ecDBConnectionFailed => 'Failed to connect to database',
                        ecLogoutFailed => 'Logout failed',
                        ecDBError => 'DB error',
                        ecLoginDisabled => 'Login is disabled',
                        ecInvalidCredentials => 'Invalid credentials',
                        ecNoFreeLicenses => 'No free licenses');
?>