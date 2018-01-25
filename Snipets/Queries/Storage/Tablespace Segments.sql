-- Tablespace Segments.sql
-- change tablespace_name filter...

select owner "Owner", 
       tablespace_name "Tablespace", 
       segment_name "Segment", 
       extents "Extents", 
       bytes/1024/1024 "Megabytes"
  from sys.dba_segments 
 where upper(substr(segment_name,1,4)) != 'BIN$'
   and upper(substr(segment_name,1,3)) != 'DR$'
   and tablespace_name = 'WATDATA'
 order by owner, extents desc, tablespace_name, segment_name;
