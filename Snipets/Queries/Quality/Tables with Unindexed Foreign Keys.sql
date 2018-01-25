-- Tables with Unindexed Foreign Keys.sql

select a.owner            "Owner",
       a.table_name       "Table_Name",
       a.constraint_name  "Constraint_Name",
       a.columns          "Foreign_Key_Column_1",
       b.columns          "Foreign_Key_Column_2",
       a.owner             sdev_link_owner,
       a.table_name        sdev_link_name,
       'TABLE'             sdev_link_type
  from (select a.owner, substr(a.table_name,1,30) table_name, 
               substr(a.constraint_name,1,30) constraint_name, 
               max(decode(position, 1,     substr(column_name,1,30),NULL)) || 
               max(decode(position, 2,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 3,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 4,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 5,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 6,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 7,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 8,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position, 9,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,10,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,11,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,12,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,13,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,14,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,15,', '||substr(column_name,1,30),NULL)) || 
               max(decode(position,16,', '||substr(column_name,1,30),NULL)) columns
          from sys.dba_cons_columns a,
               sys.dba_constraints b
         where a.constraint_name = b.constraint_name
           and a.owner = b.owner
           and a.owner = USER
           and b.constraint_type = 'R'
         group by a.owner, substr(a.table_name,1,30), substr(a.constraint_name,1,30) ) a, 
        (select table_owner, 
                substr(table_name,1,30) table_name, substr(index_name,1,30) index_name, 
                max(decode(column_position, 1,substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 2,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 3,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 4,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 5,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 6,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 7,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 8,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position, 9,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,10,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,11,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,12,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,13,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,14,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,15,', '||substr(column_name,1,30),NULL)) || 
                max(decode(column_position,16,', '||substr(column_name,1,30),NULL)) columns
          from sys.dba_ind_columns 
         group by table_owner, substr(table_name,1,30), substr(index_name,1,30) ) b
  where a.owner      = b.table_owner (+)
    and a.table_name = b.table_name (+)
    and substr(a.table_name,1,4) != 'BIN$'
    and substr(a.table_name,1,3) != 'DR$'
    and b.table_name is null
    and b.columns (+) like a.columns || '%'
  order by a.owner, a.table_name, a.constraint_name;
