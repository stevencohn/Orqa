-- Open Cursors.sql

SELECT count(*), sql_text
  FROM v$open_cursor
 GROUP BY sql_text
 ORDER BY 1 DESC;
 