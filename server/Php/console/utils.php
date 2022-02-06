<?php

//
// Извлекает из запроса POST идентификаторы выделенных объектов.
// Объекты выделяются с помощью CHECK. Названия CHECKs имеют формат
// $pattern<id>. Функция возвращает массив идентификаторов.
//

function ExtractIdFromPostRequest ($pattern)
{
    $result = NULL ;
    $nPatternLength = strlen ($pattern);
    reset ($_POST);
    while ($var = current ($_POST))
    {
//        echo $var . ' ' . key ($_POST) . '<BR>';
        $strVarName = key ($_POST);
        if (0 == strcmp ($var, 'on') && 
            0 == strncmp ($strVarName, $pattern, $nPatternLength))
        {
            $id = substr ($strVarName, $nPatternLength) + 0;
            $result [] = $id;
        }        
        next ($_POST);
    }
    reset ($_POST);

    return $result;
}

//
// Для каждого идентификатора из массива $ids выполняет каждый запрос
// из массива $queries. Запросы в массиве $queries параметризованы и
// разворачиваются с помощью команды вида
// $query = sprintf ($queries [$nQuery], $ids [$nId]);
// Возвращае массив с трок с сообщениями об ошибках или NULL, если 
// ошибок не было.
//

function BatchExecuteSQL ($ids, $queries)
{
    $result = NULL ;
    $nIdCount = 0;
    if (NULL  != $ids) $nIdCount = count ($ids);
    $nQueryCount = 0;
    if (NULL  != $queries) $nQueryCount = count ($queries);

    for ($nId = 0; $nId < $nIdCount; ++ $nId)
    {
        $id = $ids [$nId];
        for ($nQuery = 0; $nQuery < $nQueryCount; ++ $nQuery)
        {
            $query = sprintf ($queries [$nQuery], $id);
//            echo $query . '<BR>';
            $result = mysql_query ($query, $GLOBALS ['dbConnection']);
            if (FALSE == $result)
            {
                $result [] = mysql_error ();
            }
        }
    }

    return $result;
}

//
// Удаляет из таблицы $strTable записи с идентификаторами $ids.
//

function DeleteRecords ($strTable, $ids)
{
    $queries = array (
    'LOGINS'      => array ("DELETE FROM LOGINS WHERE OBJECTID=%d",
                            "DELETE FROM DOMAINS WHERE FKLOGINS=%d",
                            "DELETE FROM CONNECTIONS WHERE FKLOGINS=%d"),
    'COMPANY'     => array ("DELETE FROM COMPANY WHERE OBJECTID=%d",
                            "DELETE FROM COMPANY_MEMBERS WHERE FKCOMPANY=%d",
                            "DELETE FROM DOMAINS WHERE FKCOMPANY=%d"),
    'CLIENTS'     => array ("DELETE FROM CLIENTS WHERE OBJECTID=%d",
                            "DELETE FROM COMPANY_MEMBERS WHERE FKCLIENTS=%d",
                            "DELETE FROM LAST_PUBLISH_TIME WHERE FKCLIENTS=%d",
                            "DELETE FROM RAW_GPS_DATA WHERE FKCLIENTS=%d",
                            "DELETE FROM GPS_DATA WHERE FKCLIENTS=%d"),
    'CONNECTIONS' => array ("DELETE FROM CONNECTIONS WHERE OBJECTID=%d"));
     if (! array_key_exists ($strTable, $queries)) return NULL;
     return BatchExecuteSQL ($ids, $queries [$strTable]);
}

//
// Отрисовывает результат выполненичя SQL запроса SELECT - $strQuery.
// $strQuery      - SQL запрос.
// $columnTitles  - Заголовкм отрисовываемой таблицы.
// $dbColumnNames - Названия столбцлв таблицы базы данных, которые будут
//                  использоваться при отрисовке.
// $patterns      - Шаблоны форматирования для обработки выводимых данных.
//                  Для каждого элемента из $columnTitles должен быть
//                  шаблон. Шаблон испольуется вот так:
//                  $val = vsprintf ($patterns [$i], $argv);
//                  где $argv - массив значений, соответствующих столбцам
//                  из $dbColumnNames.
// $formatters    - Объекты, выполняющие форматирование отображаемых значений.
//                  Эти объекты опциональны и являются альтернативой  
//                  шаблонам $patterns. $formatters позволяют выполнить 
//                  более сложное функциональное форматирование.
//  Функция ничего не возвращает.
//

function RenderTable ($strQuery, 
                      $columnTitles, 
                      $dbColumnNames, 
                      $patterns,
                      $formatters = NULL)
{
    if (NULL   == $columnTitles ||
        NULL  == $dbColumnNames ||
        NULL  == $patterns)
    {
        return;
    }

    $nCount = count ($columnTitles);

    //
    // Render table header
    //

    echo '<TR>';
    for ($nIdx = 0; $nIdx < $nCount; ++ $nIdx)
    {
        echo '<TD ALIGN="CENTER"><B>';
        echo htmlspecialchars ($columnTitles [$nIdx]);
        echo '</B></TD>';
    }
    echo "</TR>\n";
    
    $result = mysql_query ($strQuery, $GLOBALS ['dbConnection']);
    if (FALSE == $result) 
    {
        echo "DB error: " . mysql_error () . '<BR>';
        return;
    }
    while ($row = mysql_fetch_array ($result, MYSQL_ASSOC))
    {
        echo "<TR>";
        $argv = NULL ;
        for ($i = 0; $i < count ($dbColumnNames); ++$i)
        {
            $data = $row [$dbColumnNames [$i]];
            if ('' == $data) $data = 'empty';
            $argv [] = htmlspecialchars ($data);             
        }

        for ($i = 0; $i < $nCount; ++$i)
        {
            if (is_null ($formatters) || is_null ($formatters [$i]))
            {
                $val = vsprintf ($patterns [$i], $argv);
            }
            else
            {
                $val = $formatters [$i]->Format ($patterns [$i], $argv);
            }

            echo "<TD>{$val}</TD>";
        }
        echo "</TR>\n";
    }
    
    mysql_free_result ($result);
}

//
// Обработка команд (INSERT, UPDATE, DELETE) редактирования одной строки
// таблицы $strTable. 
// $strKeyColumn - Название столбца первичного ключа.
// $columnData   - Массив названий обрабатываемых столбцов таблицы.
// $messages     - Массив, в который функция записывает сгенерированные 
//                 сообщения. Если сообщений ет, то $messages == NULL.
//
// Функция возвращает значение первичного ключа inserted or updated
// записи или NULL, если выполнялась операция DELETE или не выполнялось 
// ничего.
// Функция извлекает детали о команде из запроса POST. Вот какие переменные
// используются:
// 'EDIT_COMMAND' - Тип команды ('INSERT', 'UPDATE' или 'DELETE')
// 'OBJECTID'     - Значение первичного ключа удаляемой или изменяемой 
//                  записи.
//  В запросе также ищутся значения с названиями столбцов $columnData. 
//  Найденные значений используются для выполнения запросов INSERT и UPDATE.
//

function ProcessEditCommand ($strTable, $strKeyColumn, $columnData, &$messages)
{
    $messages = NULL;
    if (! array_key_exists ('EDIT_COMMAND', $_POST))
    {
        return NULL;    
    }
    $command = $_POST ['EDIT_COMMAND'];

    //
    // DELETE command processing.
    //

    if ('DELETE' == $command)
    {
        if (array_key_exists ('OBJECTID', $_POST))
        {
            $id = $_POST ['OBJECTID'];
            $result = DeleteRecords ($strTable, array ($id));
            if (! is_null ($result))
            {
                $nCount = count ($result);
                for ($i = 0; $i < $nCount; ++ $i)
                {
                    $messages [] = $result [$i];
                } 
            }
            else
            {
                $messages [] = "Row is deleted.";    
            }
        }
        else
        {
            $messages [] = "Error: OBJECTID is not specified.";
        }

        return NULL;
    }

    //
    // INSERT command processing.
    //

    if ('INSERT' == $command)
    {
        $strColumns = '';
        $strValues = '';
        $nCount = count ($columnData);
        for ($i = 0; $i < $nCount; ++ $i)
        {
            if (array_key_exists ($columnData [$i], $_POST))
            {
                if (0 != $i) 
                {
                    $strColumns .= ',' . $columnData [$i];
                    $strValues  .= ',"' . $_POST [$columnData [$i]] . '"';
                }
                else 
                {
                    $strColumns .= $columnData [$i];
                    $strValues  .= '"' . $_POST [$columnData [$i]] . '"';
                }
            }
        }    
        $strQuery = "INSERT INTO {$strTable} ({$strColumns}) VALUES ({$strValues})";
        $result = mysql_query ($strQuery, 
                               $GLOBALS ['dbConnection']);
        if (FALSE == $result) 
        {
            $messages [] = "DB error: " . mysql_error ();
            return NULL;
        }
        else
        {
            $messages [] = "Row is inserted.";    
            $query = 'SELECT LAST_INSERT_ID()'; 
            $result = mysql_query ($query, $GLOBALS ['dbConnection']);
            if (FALSE == $result) 
            {
                $messages [] = "Failed to get primary key. ";
                $messages [] = 'DB error: ' . mysql_error ();
                return NULL;
            }           
            $row = mysql_fetch_array ($result, MYSQL_NUM);  
            return $row [0];
        }
    }

    //
    // UPDATE command processing.
    //

    if ('UPDATE' == $command)
    {
        if (! array_key_exists ('OBJECTID', $_POST))
        {
            $messages [] = "Error: OBJECTID is not specified.";
            return NULL;
        }

        $id = $_POST ['OBJECTID'];
        $strQuery = "UPDATE {$strTable} SET ";

        $nCount = count ($columnData);
        for ($i = 0; $i < $nCount; ++ $i)
        {
            if (array_key_exists ($columnData [$i], $_POST))
            {
                if (0 != $i) 
                {
                    $val = $_POST [$columnData [$i]];
                    $strQuery .= ",{$columnData [$i]}=\"$val\"";
                }
                else 
                {
                    $val = $_POST [$columnData [$i]];
                    $strQuery .= "{$columnData [$i]}=\"$val\"";
                }
            }
        }
        $strQuery .= " WHERE {$strKeyColumn}={$id}";    
        
        $result = mysql_query ($strQuery, 
                               $GLOBALS ['dbConnection']);
        if (FALSE == $result) 
        {
            $messages [] = "DB error: " . mysql_error ();
        }
        else
        {
            $messages [] = "Row is updated.";    
            return $id;
        }
        return NULL;
    }

    return NULL;
}

//
// Отриисовка тела формы редактирования одной записи таблицы.
// Поддерживаются три команды редактирования (INSERT, UPDATE, DELETE).
// $strQuery          - SQL запрос для получения данных для режима UPDATE.
//                      Этот запрос должен прочитать редактируемую запись.
// $strKeyColumnTitle - Заголовок для вывода значения первичного ключа.
//                      В режиме INSERT не используется.
// $objectId          - Значение первичного ключа. Для режима INSERT должен
//                      быть равен NULL.
// $columnData        - Массив названий обрабатываемых столбцов таблицы.
// $columnTitles      - Массив заголовков обрабатываемых столбцов таблицы.
// $valuePattern      - Шаблоны для форматирования выводимых в форма значений (для 
//                      режима INSERT не используется).
//                      Шаблоны разворачиваются с помощью команды
//                      $value = vsprintf ($valuePattern [$i], $argv);
//                      где $argv - значения, соответствующие столбцам из 
//                      $columnData.
//

function RenderEditTable ($strQuery, 
                          $strKeyColumnTitle, 
                          $objectId,
                          $columnData,
                          $columnTitles,
                          $valuePattern)
{
    $argv = NULL;
    if (! is_null ($objectId))
    {
        echo "<INPUT type=\"HIDDEN\" id=\"OBJECTID\" name=\"OBJECTID\" value=\"{$objectId}\">";
        $result = mysql_query ($strQuery, $GLOBALS ['dbConnection']);
        if (FALSE == $result) 
        {
            echo "DB error: " . mysql_error () . '<BR>';
            return;
        }

        if ($row = mysql_fetch_array ($result, MYSQL_ASSOC))
        {
            $nCount = count ($columnData);
            for ($i = 0; $i < $nCount; ++ $i)
            {
                $data = $row [$columnData [$i]];
                $argv [] = htmlspecialchars ($data);             
            }

            mysql_free_result ($result);
        }
        else
        {
            echo "Error: no data.<BR>";
            return;
        }
    }
    else
    {
        $nCount = count ($columnData);
        for ($i = 0; $i < $nCount; ++ $i)
        {
            $argv [] = '';    
        }
    }

    if (! is_null ($objectId))
    {
        echo "<TR><TD><B>{$strKeyColumnTitle}</B></TD><TD>&nbsp;</TD><TD>{$objectId}</TD></TR>";     
    }

    $nCount = count ($columnTitles);
    for ($i = 0; $i < $nCount; ++ $i)
    {
        $value = vsprintf ($valuePattern [$i], $argv);
        echo "<TR><TD><B>{$columnTitles[$i]}</B></TD><TD>&nbsp;</TD><TD>{$value}</TD></TR>";         
    }
}

class RelationFormatter
{
    function Init ($strTable, $objectId, $strObjectKey, $strEntityKey) 
    {
        $this->m_strTable = $strTable;
        $this->m_ObjectId = $objectId;
        $this->m_strObjectKey = $strObjectKey;
        $this->m_strEntityKey = $strEntityKey;
    }

    function Format ($format, $argv)
    {
        $checked = '';    
        $query = "SELECT COUNT(*) FROM {$this->m_strTable} WHERE {$this->m_strObjectKey}={$this->m_ObjectId} AND {$this->m_strEntityKey}={$argv[0]}";
        $result = mysql_query ($query, $GLOBALS ['dbConnection']);
        if (FALSE != $result)
        {
            $data = mysql_fetch_array ($result, MYSQL_NUM);  
            $nCount = $data [0];
            mysql_free_result ($result);
            if ($nCount > 0) $checked = " CHECKED";
        }
        else
        {
            echo mysql_error ();
        }

        return "<INPUT type=\"checkbox\" id=\"rel_{$argv[0]}\" name=\"rel_{$argv[0]}\"{$checked}>";
    }

    var $m_strTable;
    var $m_ObjectId;
    var $m_strObjectKey;
    var $m_strEntityKey;
}

//
// Отрисовка дополнительных элементов формы редактирования записи.
// Эти элементы содержат сведения об отношениях редактируемой записи
// с таблицой $strRelatedTable.
// $objectId           - Первичный ключ редактируемой записи или NULL.
// $titles             - Заголовки таблицы отрисовываемой таблицы.
// $columnData         - Массив названий обрабатываемых столбцов таблицы.
//                       Первым столбцом должен быть первичный ключ.
// $strRelatedTable    - Имя таблицы, в которой хранятся сущности, связанные
//                       с редактируемой записью. 
// $strKeyColumn       - Имя столбца-первичного ключа таблицы
//                       $strRelatedTable.
// $strTitleColumn     - Имя столбца, содержащего название сущности из таблицы
//                       $strRelatedTable.
// $strPattern         - Шаблон для отображения сведений о связанной
//                       сущности. 
// $strRelationTable   - Имя таблицы, хранящей отношения.
// $strFirstKeyColumn  - Имя столбца таблицы $strRelationTable, ссылающегося
//                       на редактируемую запись.
// $strSecondKeyColumn - Имя столбца таблицы $strRelationTable, ссылающегося
//                       на связанную сущность.
//

function RenderRelationData ($objectId,
                             $titles,
                             $columnData,
                             $strRelatedTable, 
                             $strPattern, 
                             $strRelationTable, 
                             $strFirstKeyColumn,
                             $strSecondKeyColumn)
{
    $strQuery = "SELECT * FROM {$strRelatedTable}";
    if (is_null ($objectId))
    {
        $patterns = array ($strPattern,
                           "<INPUT type=\"checkbox\" name=\"rel_%1\$d\">");
        RenderTable ($strQuery, $titles, $columnData, $patterns);
    }
    else
    {
        $patterns = array ($strPattern,
                           '');
        $formatter = new RelationFormatter ();
        $formatter->Init ($strRelationTable, 
                          $objectId,
                          $strFirstKeyColumn,
                          $strSecondKeyColumn);

        $formatters = array (NULL, $formatter);
        RenderTable ($strQuery, $titles, $columnData, $patterns, $formatters);        
    }
}

//
// Обработка команд, изменяющих отношения редактируемой записи с сущностями.
//

function ProcessRelationData ($strTable, $strObjectKey, $strEntityKey, $id)
{
    if (strcasecmp ('POST', $_SERVER ['REQUEST_METHOD']) != 0)
    {
        return;
    }
    if (! array_key_exists ('EDIT_COMMAND', $_POST))
    {
        return;    
    }
    $command = $_POST ['EDIT_COMMAND'];
    if ('INSERT' != $command && 'UPDATE' != $command)
    {
        return;
    }

    $query = "DELETE FROM {$strTable} WHERE {$strObjectKey}={$id}";
    $result = mysql_query ($query, $GLOBALS ['dbConnection']);
    if (FALSE == $result)
    {
        echo mysql_error () . '<BR>';
    }


    $ids = ExtractIdFromPostRequest ('rel_');

    $queries = array ("INSERT INTO {$strTable} ({$strObjectKey},$strEntityKey) VALUES ({$id},%d)");
    $result = BatchExecuteSQL ($ids, $queries);
    if (! is_null ($result))
    {
        $nCount = count ($result);
        for ($i = 0; $i < $nCount; ++ $i)
        {
            echo $result [$i];
            echo '<BR>';
        } 
    }
}
?>
