
ALTER SYSTEM
  { archive_log_clause
  | CHECKPOINT [ GLOBAL | LOCAL ]
  | CHECK DATAFILES [ GLOBAL | LOCAL ]
  | { ENABLE | DISABLE } DISTRIBUTED RECOVERY
  | { ENABLE | DISABLE } RESTRICTED SESSION
  | FLUSH SHARED_POOL
  | end_session_clauses
  | SWITCH LOGFILE
  | { SUSPEND | RESUME }
  | { QUIESCE RESTRICTED | UNQUIESCE }
  | SHUTDOWN [ IMMEDIATE ] dispatcher_name
  | REGISTER
  | SET alter_system_set_clause
        [ alter_system_set_clause ]...
  | RESET alter_system_reset_clause
        [ alter_system_reset_clause ]...
  };
