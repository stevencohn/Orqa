-- Locks by User.sql

SELECT p.username username,
       p.pid      pid,
       s.sid      sid,
       s.serial#  serial,
       p.spid     spid,
       s.username ora,
       DECODE(l2.type,
              'TX','TRANSACTION ROW-LEVEL',
              'RT','REDO-LOG',
              'TS','TEMPORARY SEGMENT ',
              'TD','TABLE LOCK',
              'TM','ROW LOCK',
                   l2.type) vlock,
       DECODE(l2.type,
              'TX','DML LOCK',
              'RT','REDO LOG',
              'TS','TEMPORARY SEGMENT',
              'TD',DECODE(l2.lmode+l2.request,
                          4,'PARSE ' || u.name || '.' || o.name,
                          6,'DDL',
                          l2.lmode+l2.request),
              'TM','DML ' || u.name || '.' || o.name,
                   l2.type) type,
       DECODE(l2.lmode+l2.request,
              2, 'RS',
              3, 'RX',
              4, 'S',
              5, 'SRX',
              6, 'X',
              l2.lmode+l2.request) lmode,
       DECODE(l2.request, 0,NULL, 'WAIT') wait
  FROM v$process p,
       v$_lock l1,
       v$lock l2,
       v$resource r,
       sys.obj$ o,
       sys.user$ u,
       v$session s
 WHERE s.paddr  = p.addr
   AND s.saddr  = l1.saddr
   AND l1.raddr = r.addr
   AND l2.addr  = l1.laddr
   AND l2.type  <> 'MR'
   AND r.id1    = o.obj# (+)
   AND o.owner# = u.user# (+)
 --AND  u.name = 'GME'
 ORDER BY p.username, p.pid, p.spid, ora,
       DECODE(l2.type,
              'TX','TRANSACTION ROW-LEVEL',
              'RT','REDO-LOG',
              'TS','TEMPORARY SEGMENT ',
              'TD','TABLE LOCK',
              'TM','ROW LOCK',
              l2.type);
                   