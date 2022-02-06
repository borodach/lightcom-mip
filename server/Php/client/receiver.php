<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           receiver.php
//
//  Facility:       CGI-скрипт, принимающий данные от мобильных клиентов.
//
//
//  Abstract:       Класс Receiver получает сведения от мобильного клиента,
//                  переданные методом POST и разбирает их.
//
//  Environment:    PHP 5
//
//  Author:         Зайцев С. А.
//
//  Creation Date:  06/10/2005
//
///////////////////////////////////////////////////////////////////////////////

//
// Размер буфера для чтения строк запроса POST.
//

define ('MAX_READ_BUFFER_SIZE', 4096);

//
// Класс Receiver получает сведения от мобильного клиента,
// переданные методом POST и разбирает их.
//

class Receiver
{

    //
    // Константы.
    //
    // Номер самой последней версии формата данных, передаваемых
    // мобильным клиентом.
    //

    var $m_nClientLatestVersion = 1;


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
        $this->m_nVersion = $this->m_nClientLatestVersion;
        $this->m_strKey = "";
        $this->m_strProductVersion = "";
        $this->m_strProductName = "";
        $this->m_strClientId = "";
        $this->m_strRawGPSInfo = "";
        $this->m_nFixYear = 2000;
        $this->m_nFixMonth = 1;
        $this->m_nFixDay = 1;
        $this->m_nFixHour = 0;
        $this->m_nFixMinute = 0;
        $this->m_fFixSecond = 0;
        $this->m_fLatitude = 0;
        $this->m_fLongitude = 0;
        $this->m_fSpeed = 0;
        $this->m_fCourse = 0; 
    }

    //
    // Читает зашифрованную строчку.
    //

    function Decrypt ($data, &$nIdx, $key)
    {
        if ($nIdx >= strlen ($data)) return "";
        $result = "";
        $nSize =  ord ($data [$nIdx]);
        $nIdx ++;
        if ($nIdx + $nSize > strlen ($data)) return "";

        $mask = $key;
        for ($i = 0; $i < $nSize; ++$i, ++$nIdx)
        {
            
            $val = ord ($data [$nIdx]);
            $result = $result . chr ($val ^ $mask);
            $mask = 0xff & ((($val << 4) & 0xf0) | (($val >> 4) & 0xf));


        }

        return $result;
    }

    //
    // Читает строчку.
    //

    function ReadString ($data, &$nIdx)
    {
        if ($nIdx >= strlen ($data)) return "";
        $result = "";
        $nSize =  ord ($data [$nIdx]);
        $nIdx ++;
        if ($nIdx + $nSize > strlen ($data)) return "";

        for ($i = 0; $i < $nSize; ++$i, ++$nIdx)
        {
            
            $val = ord ($data [$nIdx]);
            $result = $result . $data [$nIdx];
        }

        return $result;
    }

    //
    // Разбирает данные, отправленные мобильным клиентом. Возвращает true,
    // если разбор прошел успешно.
    //

    function ParseRequest ()
    {
        $nIdx = 0;
        $key = 0x5a;
        $hData = fopen ('php://input', 'rb');
        if (NULL == $hData) return false;
        $data = fread ($hData, MAX_READ_BUFFER_SIZE);
        fclose ($hData);
        $strData = $this->ReadString ($data, $nIdx);
        $this->m_nVersion = (int) $strData;
        if ($this->m_nVersion < 1 || 
            $this->m_nVersion > $this->m_nClientLatestVersion)
        {
            //
            // Unsupported version.
            //

            $this->Reset ();
            return false;
        }

        $this->m_strProductName = $_SERVER ['HTTP_USER_AGENT'];
        $this->m_strProductVersion = $this->Decrypt ($data, $nIdx, $key);
        $this->m_strClientId = $this->Decrypt ($data, $nIdx, $key);
        $this->m_strRawGPSInfo = $this->Decrypt ($data, $nIdx, $key);

        return $this->ParseRawGPSdata ();
  
      /*
        $hData = fopen ('php://input', 'rb');
        if (NULL == $hData) return false;

        $clientData = array ($this->m_nLineCount);
        for ($nIdx =0; $nIdx < $this->m_nLineCount && ! feof ($hData); ++$nIdx) 
        {
            $clientData [$nIdx] = rtrim (fgets ($hData, MAX_READ_BUFFER_SIZE));
        }

        fclose ($hData);


        if ($nIdx != $this->m_nLineCount) return false;
 
        $this->m_nVersion = (int) $clientData [0];

        if ($this->m_nVersion < 0 || 
            $this->m_nVersion > $this->m_nClientLatestVersion)
        {
            //
            // Unsupported version.
            //

            $this->Reset ();
            return false;
        }
        
        $this->m_strKey = $clientData [1];
        if ($this->m_strKey != $this->m_strClientKey)
        {
            //
            // Incorrect key.
            //

            $this->Reset ();
            
            return false;
        }

        $this->m_strProductName = $clientData [2];
        $this->m_strProductVersion = $clientData [3];        
        $this->m_strClientId = $clientData [4];
        $this->m_strRawGPSInfo = $clientData [5];
    */

        return true;
    }

    //
    // Разбирает raw GPS данные, отправленные мобильным клиентом. Возвращает true,
    // если разбор прошел успешно.
    //

    function ParseRawGPSdata ()
    {
        //   0     1    2    3      4   5        6  7      8    9       10  11
        //$GPRMC,180258,A,4056.4604,N,07216.9673,W,3.965,263.0,130800, 14.8,W*41
        $parsedGPS = explode (",", $this->m_strRawGPSInfo);

        if (count ($parsedGPS) < 12) return FALSE;
        if ($parsedGPS [0] != '$GPRMC') return FALSE;


        $this->m_nFixYear  = 2000 + substr ($parsedGPS [9], 4, 2);
        $this->m_nFixMonth = 0 + substr ($parsedGPS [9], 2, 2);
        $this->m_nFixDay   = 0 + substr ($parsedGPS [9], 0, 2);

        $this->m_nFixHour = 0 + substr ($parsedGPS [1], 0, 2);        
        $this->m_nFixMinute = 0 + substr ($parsedGPS [1], 2, 2);        
        $this->m_fFixSecond = 0 + substr ($parsedGPS [1], 4, 2);        

        //
        // Извлекаем широту и долготу.
        //

        $latitude = 0.0 + $parsedGPS [3];
        //$latitude = floor ($latitude / 100) + (floor ($latitude) % 100) / 60 + 
        //            (($latitude - floor ($latitude)) / 36);

        $nDeg = floor (floor ($latitude) / 100);
        $latitude = $nDeg + (($latitude - $nDeg * 100) / 60.0);        

   
        if (strcasecmp ('S', $parsedGPS [4]) == 0) $latitude = - $latitude;
        $longitude = 0.0 + $parsedGPS [5];

        //$longitude = floor ($longitude / 100) + (floor ($longitude) % 100) / 60 + 
        //             (($longitude - floor ($longitude)) / 36);
        $nDeg = floor (floor ($longitude) / 100);
        $longitude = $nDeg + (($longitude - $nDeg * 100) / 60.0);


        if (strcasecmp ('W', $parsedGPS [6]) == 0) $longitude = - $longitude;

        $this->m_fLatitude = $latitude;
        $this->m_fLongitude = $longitude;

        $this->m_fSpeed = 0 + $parsedGPS [7];
        $this->m_fCourse = 0 + $parsedGPS [8];

        return TRUE;
    }


    //
    // Данные класса.
    //
    // Номер версии формата данных, переданных мобильным клиентом.
    //

    var $m_nVersion;

      //
    // Версия продукта.
    //

    var $m_strProductVersion;

    //
    // Название продукта.
    //

    var $m_strProductName;

    //
    // Уникальный идентификатор мобильного клиента.
    //

    var $m_strClientId;

    //
    // Raw GPS data.
    //

    var $m_strRawGPSInfo;    

    //
    // Parsed GPS data.
    //

    var $m_nFixYear;
    var $m_nFixMonth;
    var $m_nFixDay;
    var $m_nFixHour;
    var $m_nFixMinute;
    var $m_fFixSecond;
    var $m_fLatitude;
    var $m_fLongitude;
    var $m_fSpeed;
    var $m_fCourse; 


};
?>