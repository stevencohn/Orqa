
TRUNCATE 
{ TABLE [schema .] table [{ PRESERVE | PURGE } MATERIALIZED VIEW LOG]
| CLUSTER [schema .] cluster
}
[{ DROP | REUSE } STORAGE];
