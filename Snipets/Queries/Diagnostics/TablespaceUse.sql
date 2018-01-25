-- TablespaceUse.sql

SELECT a.tablespace_name,
       round(((c.bytes - nvl(b.bytes,0)) / c.bytes) * 100, 2) pct_used,
       c.bytes / 1024 / 1024 allocated,
       round(c.bytes / 1024 / 1024 - nvl(b.bytes,0) / 1024 / 1024, 2) used,
       round(nvl(b.bytes,0) / 1024 / 1024, 2) free,
       c.datafiles
  FROM dba_tablespaces a,
       (select tablespace_name, sum(bytes) bytes
	      from dba_free_space
		 group by tablespace_name) B,
       (select count(1) datafiles, sum(bytes) bytes, tablespace_name
	      from dba_data_files
		 group by tablespace_name) C
 WHERE B.tablespace_name (+) = A.tablespace_name
   AND C.tablespace_name (+) = A.tablespace_name
 ORDER BY nvl(((C.bytes - nvl(B.bytes,0)) / c.bytes), 0) DESC;

