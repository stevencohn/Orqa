-- ***********************************************************************************************
--
-- Use the CREATE DATABASE statement to create a database, making it available for general use.
--
-- This statement erases all data in any specified datafiles that already exist in order to
-- prepare them for initial database use. If you use the statement on an existing database,
-- then all data in the datafiles is lost.
--
-- After creating the database, this statement mounts it in either exclusive or parallel mode,
-- depending on the value of the CLUSTER_DATABASE initialization parameter and opens it, making
-- it available for normal use. You can then create tablespaces for the database.
--
-- ***********************************************************************************************

CREATE DATABASE [database]
{ USER SYS IDENTIFIED BY password
| USER SYSTEM IDENTIFIED BY password
| CONTROLFILE REUSE
| LOGFILE [GROUP integer] redo_log_file_spec
       [, [GROUP integer] redo_log_file_spec]...
| MAXLOGFILES integer
| MAXLOGMEMBERS integer
| MAXLOGHISTORY integer
| MAXDATAFILES integer
| MAXINSTANCES integer
| { ARCHIVELOG | NOARCHIVELOG }
| FORCE LOGGING
| CHARACTER SET charset
| NATIONAL CHARACTER SET charset
| DATAFILE datafile_tempfile_spec [, datafile_tempfile_spec]...
| EXTENT MANAGEMENT LOCAL
| default_temp_tablespace
| undo_tablespace_clause
| set_time_zone_clause
}...
;

-- Example

CREATE DATABASE sample
   CONTROLFILE REUSE 
   LOGFILE
      GROUP 1 ('diskx:log1.log', 'disky:log1.log') SIZE 50K, 
      GROUP 2 ('diskx:log2.log', 'disky:log2.log') SIZE 50K 
   MAXLOGFILES 5 
   MAXLOGHISTORY 100 
   MAXDATAFILES 10 
   MAXINSTANCES 2 
   ARCHIVELOG 
   CHARACTER SET AL32UTF8
   NATIONAL CHARACTER SET AL16UTF16
   DATAFILE  
      'disk1:df1.dbf' AUTOEXTEND ON,
      'disk2:df2.dbf' AUTOEXTEND ON NEXT 10M MAXSIZE UNLIMITED
   DEFAULT TEMPORARY TABLESPACE temp_ts
   UNDO TABLESPACE undo_ts 
   SET TIME_ZONE = '+02:00';
