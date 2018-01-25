-- Dependencies.sql

select owner                 "Owner", 
       name                  "Name",
       type                  "Type",
       referenced_owner      "Referenced_Owner",
       referenced_name       "Referenced_Name",
       referenced_type       "Referenced_Type",
       referenced_link_name  "Referenced_Link_Name",
       dependency_type       "Dependency_Type",
       owner                  sdev_link_owner,
       name                   sdev_link_name,
       type                   sdev_link_type
  from sys.all_dependencies
 where owner = user
   and substr(name,1,4) != 'BIN$'
   and substr(referenced_name,1,4) != 'BIN$'
 order by owner, name, referenced_owner, referenced_name;
