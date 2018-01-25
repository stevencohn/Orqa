-- Datatype Occurences.sql

select owner       "Owner",
       initcap(data_type) ||
          decode(data_type,  
                 'CHAR',      '('|| char_length ||')',
                 'VARCHAR',   '('|| char_length ||')',
                 'VARCHAR2',  '('|| char_length ||')',
                 'NCHAR',     '('|| char_length ||')',         
                 'NVARCHAR',  '('|| char_length ||')',  
                 'NVARCHAR2', '('|| char_length ||')', 
                 'NUMBER',    '('||
                  nvl(data_precision,data_length)||
                      decode(data_scale,null,null,','||data_scale)||')',
                      null) "Type",
                  count(*) "Occurrences"
  from sys.dba_tab_columns
 where owner = user
   and substr(table_name,1,4) != 'BIN$'
   and substr(table_name,1,3) != 'DR$'
   group by owner, initcap(data_type) ||
          decode(data_type,  
                 'CHAR',      '('|| char_length ||')',
                 'VARCHAR',   '('|| char_length ||')',
                 'VARCHAR2',  '('|| char_length ||')',
                 'NCHAR',     '('|| char_length ||')',         
                 'NVARCHAR',  '('|| char_length ||')',  
                 'NVARCHAR2', '('|| char_length ||')', 
                 'NUMBER',    '('||
                  nvl(data_precision,data_length)||
                      decode(data_scale,null,null,','||data_scale)||')',
                      null) 
 order by 1, 2;
 