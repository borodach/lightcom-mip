<?php
function RenderHeader ($page)
{
    $pages = array ('admin_home.php' => 'GPS home page',
                    'clients.php' => 'Clients table',
                    'company.php' => 'Companies',
                    'gps_data.php' => 'Parsed GPS data table',
                    'raw_gps_data.php' => 'Raw GPS data table',
                    'last_publish_time.php' => 'Last publish time',
                    'company_members.php' => 'Company members',
                    'logins.php' => 'Logins',
                    'domains.php' => 'Domains',
                    'connections.php' => 'Connections'
                    );
//<META http-equiv=Content-Type content="text/html; charset=windows-1251">                    
?><!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">
<html>
<META http-equiv=Content-Type content="text/html; charset=utf-8">
<HEAD><TITLE><?php echo ($pages [$page]);?></TITLE>
<body>
<H2><?php echo ($pages [$page]);?></H2>
<table border = 0>
    <tr>
<?php
    while ($page_name = current ($pages)) 
    {
        echo ("        <td>\n");
        if ($page == key ($pages)) 
        {
            echo ('        <B>' . $pages [$page]) . '</B>';   
        }
        else
        {
            echo ('        ' . '<A href ="' . key ($pages) . '">' . current ($pages) . '</A>');
        }
        echo ("\n        </td><td></td>\n");
        next ($pages);
    }
?>
    </tr>
</table>
<?php
}
?>