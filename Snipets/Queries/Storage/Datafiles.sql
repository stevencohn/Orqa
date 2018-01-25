-- Datafiles.sql

select TABLESPACE_NAME "Tablspace",  
       FILE_NAME "Filename",  
       BYTES/1024/1024 "Size MB", 
       MAXBYTES/1024/1024 "Maximum Size MB", 
       AUTOEXTENSIBLE "Autoextensible"
  from SYS.DBA_DATA_FILES
 order by 1, 2;
