-- Tablespace Quotas.sql

select tablespace_name "Tablespace_Name", 
       username "Username",
       bytes/1024/1024 "Megabytes", 
       (case when max_bytes = -1 then null 
             else max_bytes/1024/1024 end) "Max_Megabytes",
       (case when max_bytes = -1 then 'UNLIMITED' 
             else null end) "Quota"
  from sys.dba_ts_quotas
 order by tablespace_name, username;
