-- Historical Row Lock Contentions.sql

SELECT DISTINCT
       A.sid "waiting sid",
	   D.sql_text "waiting SQL",
	   A.ROW_WAIT_OBJ# "locked object",
	   A.BLOCKING_SESSION "blocking sid",
	   C.sql_text "SQL from blocking session"
  FROM v$session A,
       v$active_session_history B,
	   v$sql C, v$sql D
 WHERE A.event            = 'enq: TX - row lock contention'
   AND A.sql_id           = D.sql_id
   AND A.blocking_session = B.session_id
   AND C.sql_id           = B.sql_id
   AND B.CURRENT_OBJ#     = A.ROW_WAIT_OBJ#
   AND B.CURRENT_FILE#    = A.ROW_WAIT_FILE#
   AND B.CURRENT_BLOCK#   = A.ROW_WAIT_BLOCK#;
