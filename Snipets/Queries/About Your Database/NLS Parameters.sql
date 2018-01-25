-- NLS Parameters.sql

select parameter "Parameter",
       value     "Value"
  from v$nls_parameters 
 order by 1;
