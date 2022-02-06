<?php
    include_once ('client/receiver.php');
    include_once ('mysql/publisher.php');
    include_once ('mysql/mysqlconnectiondetails.php');
?>
<?php
    $receiver = new Receiver ();
    //2005-12-15 16:18:27   1   2005-12-15 01:48:29 55.0427783056   82.94435    2.2 312
    $receiver->m_strRawGPSInfo = '$GPRMC,014829.18,A,5501.940019,N,08255.996600,E,002.2,312.0,151205,,*30';
    $receiver->ParseRawGPSdata ();
    echo 'Raw   : ' . $receiver->m_strRawGPSInfo . '<BR>';
    echo 'Year  : ' . $receiver->m_nFixYear . '<BR>';
    echo 'Month : ' . $receiver->m_nFixMonth . '<BR>';
    echo 'Day   : ' . $receiver->m_nFixDay . '<BR>';
    echo 'Hour  : ' . $receiver->m_nFixHour . '<BR>';
    echo 'Minute: ' . $receiver->m_nFixMinute . '<BR>';
    echo 'Second: ' . $receiver->m_fFixSecond . '<BR>';
    echo 'Lat   : ' . $receiver->m_fLatitude . '<BR>';
    echo 'Long  : ' . $receiver->m_fLongitude . '<BR>';
    echo 'Speed : ' . $receiver->m_fSpeed . '<BR>';
    echo 'Course: ' . $receiver->m_fCourse . '<BR>';

    $receiver->m_strClientId = 'test';
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
