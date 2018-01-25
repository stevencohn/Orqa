
-- ===============================================================================================
-- SEQUENCE [sequence]
--		Sequences are typically associated with a particular table used for incrementing
--		primary key columns or other unique identities.  As such, this CREATE command
--		is best positioned adjacent to the table for which it will used.  Refer to the
--		CREATE TABLE template for more information.
-- ===============================================================================================

CREATE SEQUENCE [schema .] sequence
[{ { INCREMENT BY | START WITH } integer
 | { MAXVALUE integer | NOMAXVALUE }
 | { MINVALUE integer | NOMINVALUE }
 | { CYCLE | NOCYCLE }
 | { CACHE integer | NOCACHE }
 | { ORDER | NOORDER }
 } 
 [ { INCREMENT BY | START WITH } integer
 | { MAXVALUE integer | NOMAXVALUE }
 | { MINVALUE integer | NOMINVALUE }
 | { CYCLE | NOCYCLE }
 | { CACHE integer | NOCACHE }
 | { ORDER | NOORDER } 
 ]...
]
;
