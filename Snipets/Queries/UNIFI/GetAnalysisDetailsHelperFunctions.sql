------------------------------------------------------------------------
-- Helper function
------------------------------------------------------------------------
CREATE OR REPLACE FUNCTION GetItemTypeName(p_itemId IN CHAR)
    RETURN CHAR IS
    
    dbItemTypeName varchar2(255);
    
  BEGIN
    BEGIN
      SELECT Name
        INTO dbItemTypeName
        FROM (SELECT everest.con_itemtype.name FROM everest.CON_ItemType, everest.Con_item WHERE everest.Con_ItemType.ItemTypeId=Con_item.ItemTypeId AND everest.Con_Item.ItemID= p_itemId);
    END;

    RETURN dbItemTypeName;
  END GetItemTypeName;

/

------------------------------------------------------------------------
-- Helper function
------------------------------------------------------------------------
CREATE OR REPLACE FUNCTION GetItemParentInfo(p_itemId IN CHAR)
    RETURN CHAR IS
    
    dbItemTypeName varchar2(1000);

  BEGIN
    BEGIN
    
    SELECT X
        INTO dbItemTypeName
        FROM
      (
        SELECT DISTINCT wm_concat(everest.con_item.itemid || '::' || everest.con_item.CommonName || '::' || everest.Con_ItemType.Name || '::' || everest.con_item.VersionNo || '::' || everest.con_item.IsLastVersion) as X 
        FROM everest.CON_ItemType, everest.Con_item, EVEREST.con_itemrelation
        WHERE 
        EVEREST.con_itemrelation.relateditemid=everest.Con_Item.ItemID
        and everest.Con_ItemType.ItemTypeId=Con_item.ItemTypeId 
        and EVEREST.con_itemrelation.relationtype='PARTOF'
        AND everest.con_itemrelation.ItemID= p_itemId
        group by everest.Con_Item.ItemID);
        
        exception
          when no_data_found
          then
            dbItemTypeName:='NONE';
          when too_many_rows
          then
             dbItemTypeName:='TOO_MANY';
    END;

    RETURN dbItemTypeName;
  END GetItemParentInfo;
  
  /
  
  ------------------------------------------------------------------------
-- Helper function
------------------------------------------------------------------------
CREATE OR REPLACE FUNCTION GetItemInfo(p_itemId IN CHAR)
    RETURN CHAR IS
    
    dbItemInfo varchar2(255);
    
  BEGIN
    BEGIN
      SELECT X
        INTO dbItemInfo
        FROM (SELECT everest.con_item.CommonName || '::' || everest.con_item.VersionNo || '::' || everest.con_item.IsLastVersion as X  FROM everest.Con_item WHERE everest.Con_Item.ItemID= p_itemId);
    END;

    RETURN dbItemInfo;
  END GetItemInfo;

/

show errors;