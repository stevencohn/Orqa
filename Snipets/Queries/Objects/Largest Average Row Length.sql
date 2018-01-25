-- Largest Average Row Length.sql

select owner         "Owner",
       table_name    "Table_Name",
       last_analyzed "Last_Analyzed",
       num_rows      "Rows",
       avg_row_len   "Average_Row_Length",
       decode(iot_type,'YES','IOT','HEAP')||
              decode(temporary,'N',null,'-Temporary')||
              decode(trim(cache),'N',null,'-Cached')||
              decode(partitioned,'NO',null,'-Partitioned')||
              decode(compression,'DISABLED',null,'-Compressed') "Table_Type",
       owner       sdev_link_owner,
       table_name  sdev_link_name,
       'TABLE'     sdev_link_type
  from sys.dba_tables
 where owner = USER
   and substr(table_name,1,4) != 'BIN$' 
   and substr(table_name,1,3) != 'DR$'
 order by avg_row_len desc nulls last, owner, table_name;
