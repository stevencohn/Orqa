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
  and CreatedDttm > sysdate-3
  Order By CreatedDttm desc;
  
  
  SELECT CON_ITEM.ITEMID,
  CON_ITEM.ITEMTYPEID,
  Con_Itemtype.name,
  CON_ITEM.COMMONNAME,
  CON_ITEM.STATE,
  CON_ITEM.STATEMODIFIEDDTTM,
  CON_ITEM.STATEMODIFIERID,
  CON_ITEM.INSTANCEDATA,
  CON_ITEM.INSTANCEDATAMODIFIEDDTTM,
  CON_ITEM.CREATEDDTTM,
  CON_ITEM.CREATORID,
  CON_ITEM.CREATORNAME,
  CON_ITEM.MODIFIEDDTTM,
  CON_ITEM.MODVERSION
FROM EVEREST.CON_ITEM, Con_Itemtype
WHERE 
(Con_Item.Itemtypeid='0FF2678DA281455BBCE77A2C1F064E2A')
AND (Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
ORDER BY CREATEDDTTM DESC;
  
exit;
