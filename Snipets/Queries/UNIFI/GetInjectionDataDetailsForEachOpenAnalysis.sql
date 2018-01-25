------------------------------------------------------------------------
-- Get the number of injection data items for each open analysis
------------------------------------------------------------------------
Select Wa.Workid as AnalysisId, Wa.Name as AnalysisName, wa.createddttm, 
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid, 
Con_Itemtype.Name as ItemTypeName, Con_Item.Itemid, Con_Item.CommonName, con_item.state, con_item.VersionNo, Con_Item.Createddttm, CON_ITEM.STATEMODIFIEDDTTM
FROM CON_ITEM, CON_ITEMRELATION,Con_Itemtype, CON_WAMWORK WA
WHERE 
(
((CON_ITEM.ITEMID(+)=CON_ITEMRELATION.ITEMID) AND
(Con_Item.Itemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId') Or Con_Itemrelation.Relateditemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId')))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
)
order by wa.createddttm desc, wa.workId;