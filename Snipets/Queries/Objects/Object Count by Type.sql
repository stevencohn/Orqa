-- Object Count by Type.wsql

select owner                "Owner",
       initcap(object_type) "Object Type",
       count(*)             "Object Count"
  from sys.all_objects
 where substr(object_name,1,4) != 'BIN$'
   and substr(object_name,1,3) != 'DR$'
   and owner = user
 group by owner, initcap(object_type);
 