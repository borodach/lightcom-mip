<?php
///////////////////////////////////////////////////////////////////////////////
//                                                                            
//  File:           publisher.php
//
//  Facility:       CGI-������, ����������� ������ �� ��������� �������� � 
//                  ���� ������.
//
//
//  Abstract:       ����� Publisher ��������� ���������� �� ���������� �������
//                  �������� � ���� ������.
//
//  Environment:    PHP 5
//
//  Author:         ������ �. �.
//
//  Creation Date:  07/10/2005
//
///////////////////////////////////////////////////////////////////////////////

//
// ����� Publisher ��������� ���������� �� ���������� �������
// �������� � ���� ������.
//

class Publisher
{
    //
    // ��������� GPS ������ � ���� ������.
    //
    //      $strServer    - mySQL server name.
    //      $strUsername  - mySQL user name.
    //      $strPassword  - mySQL password.
    //      $receiver     - ������ ������ Receiver, ���������� ��������,
    //                            ���������� �� ���������� �������. 
    //
    // ��� �������� ���������� ���������� TRUE. 
    //

    function Publish ($strServer, 
                      $strUsername, 
                      $strPassword, 
                      $strDatabase, 
                      $receiver)
    {
        $dbConnection = mysql_connect ($strServer, 
                                       $strUsername, 
                                       $strPassword);
        if (FALSE == $dbConnection) 
        {
            return FALSE;
        }
        

        if ( FALSE == mysql_select_db ($strDatabase))
        {
            return FALSE;    
        }

        $nClientId = $this->ProcessClientInfo ($dbConnection, $receiver);
        if ($nClientId < 0) 
        {
            return FALSE;
        }
        
        $publishTime = localtime (time (), true);

        //
        // MySQL ����� ����/����� � ������� 'YYYY-MM-DD HH:MM:SS'                       
        //

        $dateTime = sprintf ('%d-%02d-%02d %02d:%02d:%02d',
                             $publishTime ["tm_year"] + 1900,
                             $publishTime ["tm_mon"] + 1,
                             $publishTime ["tm_mday"],
                             $publishTime ["tm_hour"],
                             $publishTime ["tm_min"],
                             $publishTime ["tm_sec"]);

        if ( !$this->PublishRawGPS ($dbConnection, 
                                    $nClientId, 
                                    $receiver->m_strRawGPSInfo,
                                    $dateTime))
        {
            return FALSE;
        }

        if ( !$this->PublishGPS ($dbConnection, 
                                 $nClientId, 
                                 $receiver->m_nFixYear,
                                 $receiver->m_nFixMonth,
                                 $receiver->m_nFixDay,
                                 $receiver->m_nFixHour,
                                 $receiver->m_nFixMinute,
                                 $receiver->m_fFixSecond,
                                 $receiver->m_fLatitude,
                                 $receiver->m_fLongitude,
                                 $receiver->m_fSpeed,
                                 $receiver->m_fCourse, 
                                 $dateTime))
        {
            return FALSE;
        }

        $query = "DELETE FROM LAST_PUBLISH_TIME WHERE FKCLIENTS = {$nClientId}"; 
        $result = mysql_query ($query, $dbConnection);
        if (FALSE == $result) return FALSE;
        //mysql_free_result ($result);    
        
        $query = "INSERT INTO LAST_PUBLISH_TIME (PUBLISH_TIME, FKCLIENTS) VALUES (\"{$dateTime}\", {$nClientId})"; 
        $result = mysql_query ($query, $dbConnection);
        if (FALSE == $result) return FALSE;
        //mysql_free_result ($result);    

        return TRUE;
    }

    //
    // ���������� � ���� ������ raw GPS info.
    //
    //      $dbConnection - ���������� � mySQL.
    //      $nClientId    - mobile client primary key.
    //      $strRawGPS    - raw GPS info.
    //      $nTimeStamp   - publish timestamp.
    //
    // ��� �������� ���������� ���������� TRUE. 
    //
    
    function PublishRawGPS ($dbConnection, 
                            $nClientId, 
                            $strRawGPS, 
                            $dateTime)
    {   
        $query = "INSERT INTO RAW_GPS_DATA (PUBLISH_TIME, FKCLIENTS, GPS_DATA) VALUES (\"{$dateTime}\", {$nClientId}, \"{$strRawGPS}\")"; 
        $result = mysql_query ($query, $dbConnection);
        if (FALSE == $result) return FALSE;
        //mysql_free_result ($result);    

        return TRUE;
    }


    //
    // ���������� � ���� ������ raw GPS info.
    //
    //      $dbConnection - ���������� � mySQL.
    //      $nClientId    - mobile client primary key.
    //      $strRawGPS    - raw GPS info.
    //      $nTimeStamp   - publish timestamp.
    //      $nFixYear     - GPS year.
    //      $nFixMonth    - GPS month.
    //      $nFixDay      - GPS day.
    //      $nFixHour     - GPS hour.
    //      $nFixMinute   - GPS minute.
    //      $fFixSecond   - GPS second.
    //      $fLatitude    - GPS latitude.
    //      $fLongitude   - GPS longitude.
    //
    // ��� �������� ���������� ���������� TRUE. 
    //
    
    function PublishGPS ($dbConnection, 
                         $nClientId, 
                         $nFixYear,
                         $nFixMonth,
                         $nFixDay,
                         $nFixHour,
                         $nFixMinute,
                         $fFixSecond,
                         $fLatitude,
                         $fLongitude,
                         $fSpeed,
                         $fCourse, 
                         $dateTime)
    {
        $fixDateTime  = $nFixYear . '-';
        $fixDateTime .= $nFixMonth . '-';
        $fixDateTime .= $nFixDay . ' ';

        $fixDateTime .= $nFixHour . ':';        
        $fixDateTime .= $nFixMinute . ':';        
        $fixDateTime .= $fFixSecond;        

        $query = 'INSERT INTO GPS_DATA (PUBLISH_TIME, FKCLIENTS, FIX_TIME, LATITUDE, LONGITUDE, GROUND_SPEED, COURSE)  VALUES (' .
                "\"{$dateTime}\", {$nClientId}, \"{$fixDateTime}\", {$fLatitude}, {$fLongitude}, {$fSpeed}, {$fCourse})"; 
        
        $result = mysql_query ($query, $dbConnection);
        if (FALSE == $result) 
        {
            return FALSE;
        }
        //mysql_free_result ($result);    
               
        return TRUE;
    }

    //
    // ������������ ���������� � �������. 
    //
    //      $dbConnection - ���������� � mySQL
    //      $receiver     - ������ ������ Receiver, ���������� ��������,
    //                      ���������� �� ���������� �������. 
    //
    // ���������� �������������� ����� - ��������� ���� ������� ��� ������������� 
    // ��� ������: -1  DB reading error
    //             -2  unknown �lient
    //             -3  client is disabled
    //             -4  DB writing error 
    //

    function ProcessClientInfo ($dbConnection, $receiver)
    {
        $query = "SELECT OBJECTID, PRODUCT_NAME, PRODUCT_VERSION, ENABLED FROM CLIENTS WHERE ID =".
                 '"' . $receiver->m_strClientId . '"'; 
        $result = mysql_query ($query, $dbConnection);
        if (FALSE == $result) return -1;

        $row = mysql_fetch_array ($result, MYSQL_NUM);  
        if (FALSE == $row) 
        {
            mysql_free_result ($result);

            return -2;    
        }

        //
        // Client is disabled.
        //

        if (0 == $row [3]) return -3;
        $nObjectId = 0 + $row [0];

        //
        // Update product name or version.
        //

        if ($row [1] != $receiver->m_strProductName ||
            $row [2] != $receiver->m_strProductVersion)
        {
            mysql_free_result ($result);    
            $query = "UPDATE CLIENTS SET PRODUCT_NAME = \"{$receiver->m_strProductName}\", PRODUCT_VERSION = \"{$receiver->m_strProductVersion}\" WHERE ID =".
                 '"' . $receiver->m_strClientId . '"'; 
            $result = mysql_query ($query, $dbConnection);
            if (FALSE == $result) return -2;
            //mysql_free_result ($result);

        }
        else
        {
            mysql_free_result ($result);
        }

        return $nObjectId;
    }
};
?>