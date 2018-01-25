-- Tablespaces.sql

select TABLESPACE_NAME "Tablespace",
       BLOCK_SIZE "Block_Size",
       INITIAL_EXTENT "Initial_Extent",
       NEXT_EXTENT "Next_Extent",
       MIN_EXTENTS "Minimum_Extents",
       MAX_EXTENTS "Maximum_Extents",
       PCT_INCREASE "Percent_Increase",
       MIN_EXTLEN "Minimum_Extent_Length",
       STATUS "Status",
       CONTENTS "Contents",
       LOGGING "Logging",
       FORCE_LOGGING "Force_Logging",
       EXTENT_MANAGEMENT "Extent_Management",
       ALLOCATION_TYPE "Allocation_Type",
       PLUGGED_IN "Plugged_In",
       SEGMENT_SPACE_MANAGEMENT "Segment_Space_Management",
       DEF_TAB_COMPRESSION "Default_Tab_Compression",
       RETENTION "Retention",
       BIGFILE "Big File"
 from  sys.dba_tablespaces
 order by 1;
