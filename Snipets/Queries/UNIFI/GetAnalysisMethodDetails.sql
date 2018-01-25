------------------------------------------------------------------------
-- Get Analysis Method Details
------------------------------------------------------------------------
Select Wa.Workid As Analysisid, 
dbms_lob.getlength(xmltype.getclobval(Instancedata)) as XmlLength,
Extractvalue (Workcontext, '//defaultAnalysisMethodId') As defaultAnalysisMethodId,
Extractvalue (Workcontext, '//currentMethodVersionId') As currentMethodVersionId,
Con_Wamitemdata.Workitemid,
Con_Itemtype.Name,
con_wamitem.BasedOnItemId,
con_wamitem.ModVersion,
con_wamitem.LastAppliedModVersion,
Con_Wamitemdata.InstanceData
From Con_Wamwork Wa, Con_Wamitem, Con_Itemtype,Con_Wamitemdata
WHERE 
(
(WA.WORKID=Con_Wamitem.Workid)
And
(Con_Wamitem.Itemtypeid=Con_Itemtype.Itemtypeid)
And
(Con_Wamitem.WorkItemID(+)=Con_Wamitemdata.WorkItemId)
And
(con_itemtype.name='Analysis Method')
)
order by wa.workId, wa.createddttm desc;