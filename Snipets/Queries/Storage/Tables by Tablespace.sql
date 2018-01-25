-- Tables by Tablespace.sql

select segment_name    "Table_Name", 
       tablespace_name "Tablespace", 
       bytes/1024/1024 "Megabytes"
  from user_segments
 where segment_type = 'TABLE'
   and segment_name not like 'BIN$%'
 order by segment_name, tablespace_name;
 