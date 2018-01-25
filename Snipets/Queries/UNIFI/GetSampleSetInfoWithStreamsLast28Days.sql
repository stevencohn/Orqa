-- Sample Sets and Analysis Result with expanded streams
Select mainItems.Itemid as MainItemID, mainItems.Commonname as MainItemName, mainItems.state as MainItemState, mainItemType.Name as MainItemType, mainItems.statemodifieddttm as MainItemStateModiedDttm,
subItems.Itemid as SubItemId, subItemType.Name as SubItemType, subItems.State as SubItemState, subItems.statemodifieddttm as SubItemStateModified,
con_itemstream.streamsize, con_itemstream.status, con_itemstream.modifieddttm as StreamModified
From Con_Item mainItems, Con_Itemrelation, con_item subItems, con_itemtype mainItemType, con_itemtype subItemType, con_itemstream
Where mainItems.Itemtypeid IN ('B89E182F836745DA865C12D4EF60E97B', '1EE29011F12C475B97892E2DCF67903F')
And mainItems.Itemid = Con_Itemrelation.Relateditemid
and subItems.Itemid = Con_Itemrelation.itemid
and subItems.ItemId = con_itemstream.itemid
And subItems.ItemTypeid = subItemType.ItemTypeId
And mainItems.ItemTypeid = mainItemType.ItemTypeId
AND (mainItems.MODIFIEDDTTM     > SysDate - 28)
order by MainItemId;