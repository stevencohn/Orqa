
-- EnqueueStatWT.sql

SELECT vs.inst_id,
       vs.sid || ',' || vs.serial# sidser,
       vs.sql_address,
       vs.sql_hash_value,
       vs.last_call_et,
       vsw.seconds_in_wait,
       vsw.event,
       vsw.state
  FROM gv$session_wait vsw, gv$session vs
 WHERE     vsw.sid = vs.sid
   AND vsw.inst_id = vs.inst_id
   AND vs.TYPE <> 'BACKGROUND'
   AND vsw.event NOT IN
       ('rdbms ipc message',
        'smon timer',
        'pmon timer',
        'SQL*Net message from client',
        'lock manager wait for remote message',
        'ges remote message',
        'gcs remote message',
        'gcs for action',
        'client message',
        'pipe get',
        'Null event',
        'PX Idle Wait',
        'single-task message',
        'PX Deq: Execution Msg',
        'KXFQ: kxfqdeq - normal deqeue',
        'listen endpoint status',
        'slave wait',
        'wakeup time manager');

-- ActiveSessions_wsql_wait
	
SELECT V.event, V.state, V.seconds_in_wait, V.last_call_et, G.sql_text
  FROM gv$sqltext G
  JOIN (SELECT S.sql_address, S.sql_hash_value, S.inst_id, S.sid,
               S.serial# sidser, S.last_call_et, W.seconds_in_wait,
			   W.event, W.state
          FROM gv$session S
		  JOIN gv$session_wait W
		    ON W.sid = S.sid
		   AND W.inst_id = S.inst_id
		   AND S.type <> 'BACKGROUND'
		   AND W.event NOT IN
		      ('rdbms ipc message',
        'smon timer',
        'pmon timer',
        'SQL*Net message from client',
        'lock manager wait for remote message',
        'ges remote message',
        'gcs remote message',
        'gcs for action',
        'client message',
        'pipe get',
        'Null event',
        'PX Idle Wait',
        'single-task message',
        'PX Deq: Execution Msg',
        'KXFQ: kxfqdeq - normal deqeue',
        'listen endpoint status',
        'slave wait',
        'wakeup time manager')
       ) V
	ON V.sql_address = G.address
   AND V.sql_hash_value = G.hash_value
   AND V.inst_id = G.inst_id
 ORDER BY G.piece;

