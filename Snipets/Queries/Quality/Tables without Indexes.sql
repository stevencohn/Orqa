-- Tables without Indexes.sql

SELECT *
  FROM (SELECT owner owner,
               TABLE_NAME TABLE_NAME,
               owner sdev_link_owner,
               TABLE_NAME sdev_link_name,
               'TABLE' sdev_link_type
          FROM sys.dba_tables
         WHERE owner = USER
           AND TEMPORARY = 'N'
           AND SUBSTR(TABLE_NAME,    1,    4) != 'BIN$'
           AND SUBSTR(TABLE_NAME,    1,    3) != 'DR$'
         minus
        SELECT owner,
               TABLE_NAME,
               owner sdev_link_owner,
               TABLE_NAME sdev_link_name,
               'TABLE' sdev_link_type
          FROM sys.dba_indexes
         WHERE owner = USER)
ORDER BY owner, table_name;
