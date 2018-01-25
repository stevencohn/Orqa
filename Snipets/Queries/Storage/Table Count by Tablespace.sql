-- Table Count by Tablespace.sql

select t.tablespace_name "Tablespace", 
       count(*) "Tables",
       sum(bytes/1024/1024) "Megabytes",
       t.max_megabytes "Max_Megabytes",
       t.quota "Quota"
  from user_segments s,
       (select tablespace_name, 
               (case when max_bytes = -1
                     then null
                     else max_bytes/1024/1024
                end) max_megabytes,
               (case when max_bytes = -1
                     then 'UNLIMITED'
                     else null
                end) quota
          from user_ts_quotas) t
 where segment_type = 'TABLE'
   and t.tablespace_name(+) = s.tablespace_name
 group by t.tablespace_name, t.max_megabytes, t.quota;
 