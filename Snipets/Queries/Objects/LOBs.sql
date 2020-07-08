select col.table_name, 
       col.column_name, 
       col.data_type
  from sys.all_tab_columns col
 inner join sys.all_tables t
    on col.owner = t.owner 
   and col.table_name = t.table_name
 where col.data_type in ('BLOB', 'CLOB', 'NCLOB', 'BFILE')
   and col.owner = 'W_DEMO_000'
 order by col.table_name, col.column_name;
