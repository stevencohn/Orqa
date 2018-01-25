-- Dynamic Memory.sql

select distinct to_char(a.end_interval_time,'MM-DD-YY') change_date,
       c.name, 
	   (c.value)/1024/1024 MB
  from dba_hist_snapshot a,
       (select *
	      from (select snap_id,instance_number,name,value, 
		               lag(value,1) over (partition by instance_number,name  order by snap_id) previous
                  from DBA_HIST_SGA) 
         where previous !=value and instance_number= 1)b, DBA_HIST_SGA c
 where a.snap_id=b.snap_id
   and b.snap_id= c.snap_id
 order by change_date;
