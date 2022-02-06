<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           gate.php
//
//  Facility:       CGI-скрипт, принимающий данные от мобильных клиентов.
//
//
//  Abstract:       Управление обработой запросов мобильных клиентов.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  06/10/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('client/receiver.php');
include_once ('mysql/publisher.php');
include_once ('mysql/mysqlconnectiondetails.php');

$receiver = new Receiver ();

if (TRUE == $receiver->ParseRequest ())
{
    $publisher = new Publisher ();    
    $publisher->Publish ($mysqlHost, 
                         $mysqlUser, 
                         $mysqlPassword, 
                         $mysqlDB,
                         $receiver);
}

//   echo "execute\n"; 
//   echo "download\\tst.exe\n";
//   readfile  ("test.exe");
?>
