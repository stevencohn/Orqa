-- Active Sessions.sql

select inst_id,program,module,event,SQL_ID,machine,
       lpad(to_char(trunc(24*(sysdate-s.logon_time))) ||
            to_char(trunc(sysdate) + (sysdate-s.logon_time), ':MI:SS'), 10, ' ') AS UP_time
  from gv$session s
 where type != 'BACKGROUND'
   and status = 'ACTIVE'
   and sql_id is not null;