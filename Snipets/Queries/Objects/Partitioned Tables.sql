-- Partitioned Tables.sql

select OWNER, 
       TABLE_NAME, 
       TABLESPACE_NAME, 
       logging, 
       partitioned,
       owner       sdev_link_owner,
       table_name  sdev_link_name,
       'TABLE'     sdev_link_type
  from sys.dba_tables
 where owner = user
   and table_name not like 'BIN$%'
   and partitioned = 'YES'
 order by 1, 2;
