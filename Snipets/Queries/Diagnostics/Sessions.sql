-- Sessions.sql

SELECT terminal, sid, username, osuser, program, logon_time
  FROM v$session
 ORDER BY terminal, username, program;
