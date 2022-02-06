<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           clientinfo.php
//
//  Facility:       Класс - описание мобильного клиента.
//
//
//  Abstract:       Класс - описание мобильного клиента.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  20/12/2005
//
///////////////////////////////////////////////////////////////////////////////

include_once ('ioutils.php');
include_once ('errorcode.php');

//
// Класс - описание мобильного клиента.
//

class ClientInfo
{
    //
    // Константы.
    //
    // Номер самой последней версии формата данных.
    //

    var $m_LatestVersion = 1;

    // 
    // Уникальный идентификатор мобильного клиента.
    //

    //
    // Конструктор.
    //

    function __construct () 
    {
        $this->Reset ();
    }

    //
    // Очищает данные, хранимые в объекте.
    //

    function Reset ()
    {
        $this->m_ClientId = '';
        $this->m_FriendlyName = '';
        $this->m_Company = '';
        $this->m_LastEvent = '';
        $this->m_SoftwareName = '';
        $this->m_SoftwareVersion = '';
        $this->m_Comments = '';        
    }

    //
    // Извлекает из начала строки целое число. Извлеченное число удаляется из
    // строки.
    //
    //      $strBuffer - строка, в которую записывается значение.
    //
    // Ничего не возвращает.
    //
    //

    function SaveGuts (&$strBuffer)
    {
        WriteInt      ($strBuffer, $this->m_LatestVersion);
        WriteString   ($strBuffer, $this->m_ClientId);
        WriteString   ($strBuffer, $this->m_FriendlyName);
        WriteString   ($strBuffer, $this->m_Company);
        WriteDateTime ($strBuffer, $this->m_LastEvent);    
        WriteString   ($strBuffer, $this->m_SoftwareName);
        WriteString   ($strBuffer, $this->m_SoftwareVersion);
        WriteString   ($strBuffer, $this->m_Comments);
    }

    //
    // Извлекает из начала строки объект. Извлеченный объект удаляется из
    // строки.
    //
    //      $strBuffer - строка, из которой извлекается значение.
    //
    // Возвращает код ошибки.
    //
    //

    function RestoreGuts (&$strBuffer)
    {
        $this->Reset ();
        $nVersion = ReadInt ($strBuffer);
        if ($nVersion > $this->m_LatestVersion ||
            $nVersion <= 0) return ecBadFormat;

        $this->m_ClientId        = ReadString   ($strBuffer);
        $this->m_FriendlyName    = ReadString   ($strBuffer);
        $this->m_Company         = ReadString   ($strBuffer);
        $this->m_LastEvent       = ReadDateTime ($strBuffer);    
        $this->m_SoftwareName    = ReadString   ($strBuffer);
        $this->m_SoftwareVersion = ReadString   ($strBuffer);
        $this->m_Comments        = ReadString   ($strBuffer);

        return ecOK;
    }
     
    var $m_ClientId;

    // 
    // Дружественное название мобильного клиента.
    //

    var $m_FriendlyName;

    // 
    // Название компании - владельца мобильного клиента.
    //
    
    var $m_Company;

    // 
    // Время получения последнего сообщения от мобильного клиента.
    //

    var $m_LastEvent;

    // 
    // Название клиентского ПО.
    //

    var $m_SoftwareName;

    // 
    // Версия клиентского ПО.
    //


    var $m_SoftwareVersion;

    // 
    // Строка с примечаниями.
    //

    var $m_Comments;
}
?>