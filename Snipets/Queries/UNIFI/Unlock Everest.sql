--sqlplus sys/<pwd> as sysdba

grant dba to everest;
alter user everest account unlock;
alter user everest identified by <pwd>;
