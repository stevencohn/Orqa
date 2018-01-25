-- Cursors by Session.sql

select c.sid, 
       s.username, 
       count(*) open_cursors
  from v$open_cursor c, v$session s
 where s.sid = c.sid
   and s.username is not null
 group by c.sid, s.username;
