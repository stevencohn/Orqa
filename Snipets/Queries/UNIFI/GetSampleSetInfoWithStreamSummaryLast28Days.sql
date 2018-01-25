Select mainItems.Itemid as MainItemID, mainItems.Commonname as MainItemName, mainItems.state as MainItemState, mainItemType.Name as MainItemType, mainItems.statemodifieddttm as MainItemStateModiedDttm,
subItems.Itemid as SubItemId, subItemType.Name as SubItemType, subItems.State as SubItemState, count(*) as StreamCount,  min(con_itemstream.streamsize) as MinStreamSize, max(con_itemstream.streamsize) as MaxStreamSize,
max(subItems.statemodifieddttm) as MaxSubItemStateModiedDttm
From Con_Item mainItems, Con_Itemrelation, con_item subItems, con_itemtype mainItemType, con_itemtype subItemType, con_itemstream
Where mainItems.Itemtypeid IN ('B89E182F836745DA865C12D4EF60E97B', '1EE29011F12C475B97892E2DCF67903F')
And mainItems.Itemid = Con_Itemrelation.Relateditemid
and subItems.Itemid = Con_Itemrelation.itemid
and subItems.ItemId = con_itemstream.itemid
And subItems.ItemTypeid = subItemType.ItemTypeId
And mainItems.ItemTypeid = mainItemType.ItemTypeId
AND (mainItems.MODIFIEDDTTM     > SysDate - 28)
group by mainItems.Itemid, mainItems.Commonname, mainItems.state, subItems.Itemid, subItemType.Name, subItems.State, mainItemType.Name, mainItems.statemodifieddttm, con_itemstream.itemid
order by MainItemId;