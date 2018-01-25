-- Non-default Parameters.sql

select name "Paramater_Name", 
       decode(type, 1, 'Boolean',
                    2, 'String',
                    3, 'Integer',
                    4, 'Parameter file',
                    5, 'Reserved',
                    6, 'Big Integer') type,
       value "Value",
       isses_modifiable "Session_Modifiable",
       issys_modifiable "System_Modifiable",
       description "Description" 
  from v$parameter 
 where isdefault = 'FALSE'  
   and substr(name,1,2) != '__' 
 order by name;
