
-- ===============================================================================================
-- TABLE <TableName>
-- ===============================================================================================

CREATE TABLE <Schema>.<TableName>
(
    <column0>   <datatype>  NULL,
    <column1>   <datatype>  NOT NULL,
    <column2>   <datatype>  DEFAULT 0 NOT NULL
);

ALTER TABLE <Schema>.<TableName> ADD
(
	CONSTRAINT PK_<TableName>          PRIMARY KEY (<column0>),
	CONSTRAINT IX_<TableName>_<XName>  UNIQUE (<column1>,<column2>)
);

CREATE SEQUENCE <Schema>.<TableName>Seq;
