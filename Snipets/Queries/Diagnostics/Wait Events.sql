-- Wait Events.sql

 SELECT s.sid,
       s.serial#,
       p.pid,
       p.spid,
       p.program,
       s.username,
       p.username os_user,
       sw.event,
       sw.p2text,
       sw.p2,
       sw.seconds_in_wait sec
  FROM gv$process p, gv$session s, gv$session_wait sw
 WHERE     (p.inst_id = s.inst_id AND p.addr = s.paddr)
       AND s.username IN ('EVEREST', 'WATERSUNIFI1')
       AND (s.inst_id = sw.inst_id AND s.sid = sw.sid)
ORDER BY p.inst_id, s.sid;