<?php
function RenderMenu ($page, $bThisPageLink = false)
{
    //
    // Main commands
    //

    $companyPageInfo    = array ('name'  => 'Companies',  
                                 'file'  => 'companies.php',
                                 'title' => 'Companies');
    $endPointPageInfo   = array ('name'  => 'Mobile clients',  
                                 'file'  => 'endpoint.php',
                                 'title' => 'Mobile clients');
    $accountPageInfo    = array ('name'  => 'Accounts', 
                                 'file'  => 'accounts.php',
                                 'title' => 'Accounts');
    $connectionPageInfo = array ('name'  => 'Connections', 
                                 'file'  => 'connections.php',
                                 'title' => 'Connections');
    $tableMagementInfo  = array ('name'  => 'Table management', 
                                 'file'  => 'table_management.php',
                                 'title' => 'Table management');
    $consoleInfo        = array ('name'  => 'Console', 
                                 'file'  => 'console.php',
                                 'title' => 'SQL console');

    $logoffPageInfo     = array ('name'  => 'Logoff', 
                                 'file'  => 'logoff.php',
                                 'title' => 'Logoff');

    //
    // Company operations
    //

    $addCompanyInfo = array ('name'   => 'Add',
                             'file'   => 'edit_company.php',
                             'parent' => &$companyPageInfo,
                             'title'  => 'Add company');
    $remCompanyInfo = array ('name'   => 'Delete',
                             'file'   => 'delete_companies.php',
                             'parent' => &$companyPageInfo,
                             'title'  => 'Delete companies');

    //
    // Account operations
    //

    $addAccountInfo  = array ('name'   => 'Add',
                              'file'   => 'edit_account.php',
                              'parent' => &$accountPageInfo,
                              'title'  =>  'Add account');
    $remAccountInfo  = array ('name'   => 'Delete',
                              'file'   => 'delete_accounts.php',
                              'parent' => &$accountPageInfo,
                              'title'  =>  'Delete accounts');
    $lockAccountInfo = array ('name'   => 'Lock',
                              'file'   => 'lock_account.php',
                              'parent' => &$accountPageInfo,
                              'title'  =>  'Lock accounts');
    //
    // Mobile client operations
    //

    $addEndPointInfo  = array ('name'   => 'Add',
                               'file'   => 'edit_endpoint.php',
                               'parent' => &$endPointPageInfo,
                               'title'  => 'Add mobile client');
    $remEndPointInfo  = array ('name'   => 'Delete',
                               'file'   => 'delete_endpoint.php',
                               'parent' => &$endPointPageInfo,
                               'title'  => 'Delete mobile clients');
    $lockEndPointInfo = array ('name'   => 'Lock',
                               'file'   => 'lock_endpoint.php',
                               'parent' => &$endPointPageInfo,
                               'title'  => 'Lock mobile clients');

    //
    // Connections operations
    //

    
    //
    // Table management
    //

    $rawCompanyInfo    = array ('name'   => 'COMPANY',
                                'file'   => 'company_table.php',
                                'parent' => &$tableMagementInfo,
                                'title'  => 'COMPANY table');
    $domainInfo         = array ('name'   => 'DOMAINS',
                                 'file'   => 'domains_table.php',
                                 'parent' => &$tableMagementInfo,
                                 'title'  => 'DOMAINS table');
    $rawAccountInfo     = array ('name'    => 'LOGINS',
                                 'file'    => 'logins_table.php',
                                 'parent'  => &$tableMagementInfo,
                                 'title'   => 'LOGINS table');
    $rawEndPointInfo    = array ('name'   => 'CLIENTS',
                                 'file'   => 'clients_table.php',
                                 'parent' => &$tableMagementInfo,
                                 'title'  => 'CLIENTS table');
    $companyMembersInfo = array ('name'   => 'COMPANY_MEMBERS',
                                 'file'   => 'company_members_table.php',
                                 'parent' => &$tableMagementInfo,
                                 'title'  => 'COMPANY_MEMBERS table');
    $gpsDataInfo        = array ('name'   => 'GPS_DATA',
                                 'file'   => 'gps_data_table.php',
                                 'parent' => &$tableMagementInfo,
                                 'title'  => 'GPS_DATA');
    $rawGpsDataInfo     = array ('name'   => 'RAW_GPS_DATA table',
                                 'file'   => 'raw_gps_data_table.php',
                                 'parent' => &$tableMagementInfo,
                                 'title'  => 'RAW_GPS_DATA table');
    $rawConnectionInfo  = array ('name'   => 'CONNECTIONS',
                                 'file'   => 'connections_table.php',
                                 'parent' => &$tableMagementInfo,
                                 'title'  => 'CONNECTIONS table');                 

    $mainMenu = array (&$companyPageInfo, 
                       &$endPointPageInfo,
                       &$accountPageInfo,
                       &$connectionPageInfo,
                       &$tableMagementInfo,
                       &$consoleInfo,
                       &$logoffPageInfo);

    $subMenu = array (&$addCompanyInfo, 
                      &$remCompanyInfo,
                      &$addAccountInfo,
                      &$remAccountInfo,
                      &$lockAccountInfo,
                      &$addEndPointInfo,
                      &$remEndPointInfo,
                      &$lockEndPointInfo,
                     
                      &$rawCompanyInfo,
                      &$rawEndPointInfo,
                      &$companyMembersInfo,
                      &$gpsDataInfo,
                      &$rawGpsDataInfo,
                      &$rawAccountInfo,
                      &$domainInfo,
                      &$rawConnectionInfo
                      );

    $strMenuBody = '<TABLE border=0><TR>';
    $strTitle = '';
    $currentMainPage = NULL ;

    while ($currentPage = & current ($mainMenu)) 
    {
        $strMenuBody .= "<TD>\n";
        if ($page == $currentPage ['file']) 
        {
            if (! $bThisPageLink)
            {
                $strMenuBody .= '<B>' . $currentPage ['name'] . '</B>';   
            }
            else
            {
                $strMenuBody .= '<A href="' . $currentPage ['file'] . '">' . $currentPage ['name'] . '</A>';
            }
            $strTitle = $currentPage ['title'];
            $currentMainPage = &$currentPage;
        }
        else
        {
            $strMenuBody .= '<A href="' . $currentPage ['file'] . '">' . $currentPage ['name'] . '</A>';
        }
        $strMenuBody .= "\n </TD><TD></TD>\n";
        next ($mainMenu);
    }
    $strMenuBody .= '</TR></TABLE>';

    
    $bSubMenuPresents = false;
    while ($currentPage = & current ($subMenu)) 
    {

        if ($page == $currentPage ['file']) 
        {
            $strTitle = $currentPage ['title'];
            $currentMainPage = & $currentPage ['parent'];
        }
        next ($subMenu);
    }
    reset ($subMenu);
    while ($currentPage = & current ($subMenu)) 
    {

        if ($page == $currentPage ['file']) 
        {
            if (! $bSubMenuPresents)
            {
                $strMenuBody .= '<TABLE border=0><TR>';
            }

            if (! $bThisPageLink)
            {
                $strMenuBody .= '<TD><B>' . $currentPage ['name'] . '</B></TD>';   
            }
            else
            {
                $strMenuBody .= '<TD><A href="' . $currentPage ['file'] . '">' . $currentPage ['name'] . '</A></TD><TD></TD>';    
            }

            $bSubMenuPresents = true;
        }

        elseif ($currentPage ['parent'] == $currentMainPage &&
                ! is_null ($currentMainPage))
        {
            if (! $bSubMenuPresents)
            {
                $strMenuBody .= '<TABLE border=0><TR>';
            }

            $strMenuBody .= '<TD><A href="' . $currentPage ['file'] . '">' . $currentPage ['name'] . '</A></TD><TD></TD>';

            $bSubMenuPresents = true;
        }
        next ($subMenu);
    }
    if ($bSubMenuPresents)
    {
        $strMenuBody .= '</TR></TABLE>';
    }

?>
<H2><?php echo $strTitle;?></H2>
<?php echo $strMenuBody;?>
<?php
}?>
