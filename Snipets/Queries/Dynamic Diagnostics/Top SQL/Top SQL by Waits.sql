-- Top SQL by Waits.sql
-- Replace "(30)" with alternate minutes...

select INST_ID,
       (cpu_time/1000000) "CPU_Seconds",
       disk_reads "Disk_Reads",
       buffer_gets "Buffer_Gets",
       executions "Executions",
       case when rows_processed = 0 then null
            else round((buffer_gets/nvl(replace(rows_processed,0,1),1))) 
            end "Buffer_gets/rows_proc",
       round((buffer_gets/nvl(replace(executions,0,1),1))) "Buffer_gets/executions",
       (elapsed_time/1000000) "Elapsed_Seconds", 
       --round((elapsed_time/1000000)/nvl(replace(executions,0,1),1)) "Elapsed/Execution",
       substr(sql_text,1,500) "SQL",
       module "Module",SQL_ID
  from gv$sql s
 where sql_id in (
          select distinct sql_id from (
          WITH sql_class AS
          (select sql_id, state, count(*) occur from
            (select sql_id,
                    CASE WHEN session_state = 'ON CPU' THEN 'CPU'
                         WHEN session_state = 'WAITING' AND wait_class IN ('User I/O') THEN 'IO'
                    ELSE 'WAIT' END state
               from gv$active_session_history
              where session_type IN ('FOREGROUND')
              and sample_time  between trunc(sysdate,'MI') - (30)/24/60 and trunc(sysdate,'MI') )
              group by sql_id, state),
               ranked_sqls AS
            (select sql_id,  sum(occur) sql_occur  , rank () over (order by sum(occur)desc) xrank
               from sql_class
              group by sql_id)
          select sc.sql_id, state, occur from sql_class sc, ranked_sqls rs
          where rs.sql_id = sc.sql_id 
          --and rs.xrank <= :top_n 
          order by xrank, sql_id, state ))
 order by elapsed_time desc nulls last;
