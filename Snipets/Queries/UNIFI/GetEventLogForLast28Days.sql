------------------------------------------------------------------------
-- Get the event log
------------------------------------------------------------------------

SELECT "EVENTID",
    "TYPEID",
    "DATANODEID",
    "DATANODEPATH",
    "ITEMID",
    "ITEMNAME",
    "ITEMVERSIONNO",
    "INSTRUMENTID",
    "INSTRUMENTNAME",
    "SYSTEMID",
    "SYSTEMNAME",
    "SEVERITYID",
    "VARIABLES",
    "NATIVEDATA",
    "TEXT",
    "TEXTCULTURE",
    "REASON",
    "ROLENAME",
    "COMPUTERNAME",
    "USERNAME",
    "FULLNAME",
    "CREATEDDTTM",
    "CREATORID"
  FROM NOT_Event EV
  WHERE isImported = 0
  and CreatedDttm > sysdate-28
  Order By CreatedDttm desc;
