-- Invalid Objects.sql

select owner                "Owner",
	   initcap(object_type) "Object_Type",
	   object_name          "Object_Name",
	   owner                 sdev_link_owner,
	   object_name           sdev_link_name,
	   object_type           sdev_link_type
  from sys.all_objects
 where status = 'INVALID'
   and substr(object_name,1,4) != 'BIN$'
   and substr(object_name,1,3) != 'DR$'
   and owner = user
 order by owner, object_type, object_name;
