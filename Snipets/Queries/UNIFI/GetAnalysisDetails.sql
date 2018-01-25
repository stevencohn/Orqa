
  
  
  execute GetItemParentInfo ('FAC2581FF1104BD6861F949EF99BEF61');

  select LISTAGG(everest.con_item.itemid, ',')
        SELECT everest.con_item.itemid || '::' || everest.con_item.CommonName || '::' || everest.Con_ItemType.Name || '::' || everest.con_item.VersionNo || '::' || everest.con_item.IsLastVersion as X 
        FROM everest.CON_ItemType, everest.Con_item, EVEREST.con_itemrelation
        WHERE 
        EVEREST.con_itemrelation.relateditemid=everest.Con_Item.ItemID
        and everest.Con_ItemType.ItemTypeId=Con_item.ItemTypeId 
        and EVEREST.con_itemrelation.relationtype='PARTOF'
        AND everest.Con_Item.ItemID= p_itemId;
        
        
        SELECT wm_concat(everest.con_item.itemid)
        FROM everest.CON_ItemType, everest.Con_item, EVEREST.con_itemrelation
        WHERE 
        EVEREST.con_itemrelation.relateditemid=everest.Con_Item.ItemID
        and everest.Con_ItemType.ItemTypeId=Con_item.ItemTypeId 
        and EVEREST.con_itemrelation.relationtype='PARTOF'
        AND everest.Con_Item.ItemID= p_itemId
        group by con_item.itemid;
  
  show errors
  SELECT everest.con_item.itemid, everest.con_item.itemid || '::' || everest.con_item.CommonName || '::' || Con_ItemType.Name || '::' || everest.con_item.VersionNo || '::' || everest.con_item.IsLastVersion as X 
        FROM everest.CON_ItemType, everest.Con_item, EVEREST.con_itemrelation
        WHERE 
        EVEREST.con_itemrelation.relateditemid=everest.Con_Item.ItemID
        and everest.Con_ItemType.ItemTypeId=Con_item.ItemTypeId 
        and EVEREST.con_itemrelation.relationtype='PARTOF';
        

desc everest.GetItemTypeName;

select itemId, GetItemTypeName(itemId) from CON_ITEM;

select * from con_item where itemtypeid='1EE29011F12C475B97892E2DCF67903F'
order by statemodifieddttm desc;

select * from con_item where itemtypeid='1EE29011F12C475B97892E2DCF67903F'
order by statemodifieddttm desc;

select WORKITEMID, MODVERSION, itemmodversion, APPLYTYPE,workversionno from con_wamitem;

select * from con_item where itemid='63ECE735717F425D80E3D6FBB318B654';

select * from con_itemstream where con_itemstream.streamid not in (Select streamid from con_itemstreamblob);

select con_item.commonname , con_itemstream.*
from con_itemstream, con_item
where 
con_itemstream.itemid = con_item.itemid and
con_itemstream.refitemid != '00000000000000000000000000000000' and
con_itemstream.refitemid not in (Select itemid from con_item);

UPDATE CON_ITEM SET STATE=8000 WHERE ITEMID IN ('3C608D87B3774A2B92AEDE30A103DE5E')
------------------------------------------------------------------------
-- 1. Get context info for each open analysis
------------------------------------------------------------------------
Select Wa.Workid As Analysisid, SEC_User.Username, Wa.Name As Analysisname, Wa.Createddttm, 
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid,
Extractvalue (Workcontext, '//analysisResultId') As analysisResultId,
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionId') As Acquisitionid,
Extractvalue (Workcontext, '/analyticalTaskDataContext/jobId') As Workflowid,
Extractvalue (Workcontext, '/analyticalTaskDataContext/AnalyticalTaskStatus/AnalyticalTaskExecutionStatus/AnalyticalTaskStatus') As AnalyticalTaskStatus,
Extractvalue (Workcontext, '//AcquisitionListExecutionState') As Liststate,
Extractvalue (Workcontext, '//AcquisitionExecutionStatusState') As Acqstate,
Extractvalue (Workcontext, '//ProcessingExecutionStatus/State') As Procstate,
Extractvalue (Workcontext, '//InteractiveProcessingExecution') As InteractiveProcstate,
Extractvalue (Workcontext, '//ProcessedDataConsistency') As ProcessedDataConsistency,
Extractvalue (Workcontext, '//ConsolidatedProcessingStatusElementName') As ConProcstate,
Extractvalue (Workcontext, '//AnalyticalDataModification') As AnalyticalDataModification,
Extractvalue (Workcontext, '//defaultAnalysisMethodId') As defaultAnalysisMethodId,
Extractvalue (Workcontext, '//currentMethodVersionId') As currentMethodVersionId,
Extractvalue (Workcontext, '//lastSavedAnalysisResultId') As LastSavedAnalysisResultId,
Extractvalue (Workcontext, '/analyticalTaskDataContext/modVersion') As ModVersionXml,
wa.ModVersion,
wa.Workcontext
From Con_Wamwork Wa, SEC_User
WHERE Wa.USERID=SEC_USER.USERID;

------------------------------------------------------------------------
-- 2. Get open analysis by injection data with limited stream info
------------------------------------------------------------------------
Select Wa.Workid As Analysisid, Wa.Name As Analysisname, Wa.Createddttm, Con_Item.Itemid, Con_Itemtype.Name As Itemtypename, 
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionId') As Acquisitionid,
Extractvalue (Workcontext, '/analyticalTaskDataContext/jobId') As Workflowid,
Extractvalue (Workcontext, '//AcquisitionListExecutionState') As Liststate,
Extractvalue (Workcontext, '//AcquisitionExecutionStatusState') As Acqstate,
Extractvalue (Workcontext, '//ProcessingExecutionStatus/State') As Procstate,
Count(Con_Itemstream.Streamid) As Streamcount, Min(CON_StreamInfo.Streamsize) As Minstreamsize, Max(CON_StreamInfo.Streamsize) As Maxstreamsize, Min(CON_StreamInfo.Createddttm) As Mincreatedtime, Max(CON_StreamInfo.Modifieddttm) As Maxcreatedtime,
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid,
Extractvalue (Con_Item.Instancedata, '//result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As Itemname,
Extractvalue (Con_item.Instancedata, '//result/status/propertyGroups/common/acqState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As ItemAcqStatus,
dbms_lob.getlength(xmltype.getclobval(Instancedata)) as XmlLength,
Con_Item.Createddttm As Resultcreated,
Con_Item.Statemodifieddttm As ResultCompleted,
con_item.state,
Con_Item.Statemodifieddttm-Con_Item.Createddttm As Resulttime
From Con_Item, Con_Itemrelation,Con_Itemtype,Con_Itemstream, CON_Streaminfo, Con_Wamwork Wa 
Where 
(
(Con_Item.Itemid(+)=Con_Itemrelation.Itemid) And
(Con_Item.Itemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId') Or Con_Itemrelation.Relateditemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId'))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
And
(con_itemtype.name='InjectionData')
And
(Con_Item.Itemid = Con_Itemstream.Itemid)
And
(Con_Itemstream.StorageId = CON_StreamInfo.StorageId)
and (con_item.itemid='A9BD9F26F9F849E493039626BA5D7D93')
)
GROUP BY Wa.Workid, Wa.Name, Wa.Createddttm, Con_Item.Itemid, Con_Itemtype.Name, Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionId'), Extractvalue (Workcontext, '/analyticalTaskDataContext/jobId'), Extractvalue (Workcontext, '//AcquisitionListExecutionState'), Extractvalue (Workcontext, '//AcquisitionExecutionStatusState'), Extractvalue (Workcontext, '//ProcessingExecutionStatus/State'), Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId'), Extractvalue (Con_Item.Instancedata, '//result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'), Extractvalue (Con_item.Instancedata, '//result/status/propertyGroups/common/acqState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'), dbms_lob.getlength(xmltype.getclobval(Instancedata)), Con_Item.Createddttm, Con_Item.Statemodifieddttm, con_item.state, Con_Item.Statemodifieddttm-Con_Item.Createddttm
Order By 
Wa.Createddttm Desc, 
wa.workId;


Select Wa.Workid As Analysisid, Wa.Name As Analysisname, Wa.Createddttm, Con_Item.Itemid, Con_Itemtype.Name As Itemtypename, 
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionId') As Acquisitionid,
Extractvalue (Workcontext, '/analyticalTaskDataContext/jobId') As Workflowid,
Extractvalue (Workcontext, '//AcquisitionListExecutionState') As Liststate,
Extractvalue (Workcontext, '//AcquisitionExecutionStatusState') As Acqstate,
Extractvalue (Workcontext, '//ProcessingExecutionStatus/State') As Procstate,
Count(Con_Itemstream.Streamid) As Streamcount, Min(Con_Itemstream.Streamsize) As Minstreamsize, Max(Con_Itemstream.Streamsize) As Maxstreamsize, Min(Con_Itemstream.Createddttm) As Mincreatedtime, Max(Con_Itemstream.Modifieddttm) As Maxcreatedtime,
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid,
Extractvalue (Con_Item.Instancedata, '//result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As Itemname,
Extractvalue (Con_item.Instancedata, '//result/status/propertyGroups/common/acqState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As ItemAcqStatus,
dbms_lob.getlength(xmltype.getclobval(Instancedata)) as XmlLength,
Con_Item.Createddttm As Resultcreated,
Con_Item.Statemodifieddttm As ResultCompleted,
con_item.state,
Con_Item.Statemodifieddttm-Con_Item.Createddttm As Resulttime
From Con_Item, Con_Itemrelation,Con_Itemtype,Con_Itemstream, Con_Wamwork Wa 
Where 
(
(Con_Item.Itemid(+)=Con_Itemrelation.Itemid) And
(Con_Item.Itemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId') Or Con_Itemrelation.Relateditemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId'))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
And
(con_itemtype.name='Acquisition List')
And
(Con_Item.Itemid = Con_Itemstream.Itemid)
)
GROUP BY Wa.Workid, Wa.Name, Wa.Createddttm, Con_Item.Itemid, Con_Itemtype.Name, Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionId'), Extractvalue (Workcontext, '/analyticalTaskDataContext/jobId'), Extractvalue (Workcontext, '//AcquisitionListExecutionState'), Extractvalue (Workcontext, '//AcquisitionExecutionStatusState'), Extractvalue (Workcontext, '//ProcessingExecutionStatus/State'), Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId'), Extractvalue (Con_Item.Instancedata, '//result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'), Extractvalue (Con_item.Instancedata, '//result/status/propertyGroups/common/acqState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'), dbms_lob.getlength(xmltype.getclobval(Instancedata)), Con_Item.Createddttm, Con_Item.Statemodifieddttm, con_item.state, Con_Item.Statemodifieddttm-Con_Item.Createddttm
Order By 
Wa.Createddttm Desc, 
Wa.Workid;

A9BD9F26F9F849E493039626BA5D7D93
------------------------------------------------------------------------
-- 3. Get the number of injection data items for each open analysis
------------------------------------------------------------------------
Select Wa.Workid as AnalysisId, Wa.Name as AnalysisName, wa.createddttm, 
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid, 
Con_Itemtype.Name as ItemTypeName,
count(Con_Item.Itemid), con_item.state, con_item.VersionNo, min(Con_Item.Createddttm), max(CON_ITEM.STATEMODIFIEDDTTM)
FROM CON_ITEM, CON_ITEMRELATION,Con_Itemtype, CON_WAMWORK WA
WHERE 
(
((CON_ITEM.ITEMID(+)=CON_ITEMRELATION.ITEMID) AND
(Con_Item.Itemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId') Or Con_Itemrelation.Relateditemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId')))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
--And
--(con_itemtype.name='InjectionData')
)
group by Wa.Workid, Wa.Name, wa.createddttm, Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId'), Con_Itemtype.Name, con_item.state, con_item.VersionNo
order by wa.createddttm desc, wa.workId;

------------------------------------------------------------------------
-- 4. Get basic info about the open analysis
------------------------------------------------------------------------
Select Wa.Workid As Analysisid, Wa.Name As Analysisname, Wa.Createddttm, 
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid,
Extractvalue (Workcontext, '/analyticalTaskDataContext/analysisResults/analysisResultId') As Analysisresultid,
Extractvalue (Workcontext, '/analyticalTaskDataContext/AnalyticalTaskStatus/AnalyticalTaskExecutionStatus/AcquisitionListExecutionState') As AcquisitionListExecutionState
FROM CON_WAMWORK WA
Order By Wa.Createddttm Desc, Wa.Workid;

------------------------------------------------------------------------
-- Get info about the analysis result for an open analysis with injection result information
------------------------------------------------------------------------
Select Wa.Workid As Analysisid, Wa.Name As Analysisname, Wa.Createddttm, 
Extractvalue (Con_Wamitemdata.Instancedata, '//result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As Itemname,
Extractvalue (Con_Wamitemdata.Instancedata, '//result/status/propertyGroups/common/acqState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As Itemacqstatus,
Extractvalue (Con_Wamitemdata.Instancedata, '//result/status/propertyGroups/common/procState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As ItemProcStatus,
dbms_lob.getlength(xmltype.getclobval(Instancedata)) as XmlLength,
Extractvalue (Workcontext, '/analyticalTaskDataContext/analysisResults/analysisResultId') As Analysisresultid,
Con_Wamitemdata.Workitemid,
Con_Itemtype.Name,
con_wamitem.BasedOnItemId,
con_wamitem.ModVersion,
con_wamitem.LastAppliedModVersion
From Con_Wamwork Wa, Con_Wamitem, Con_Itemtype,Con_Wamitemdata
WHERE 
(
(WA.WORKID=Con_Wamitem.Workid)
And
(Con_Wamitem.Itemtypeid=Con_Itemtype.Itemtypeid)
And
(Con_Wamitem.WorkItemID(+)=Con_Wamitemdata.WorkItemId)
--And
--(con_itemtype.name='InjectionResult')
)
order by wa.workId, wa.createddttm desc;

------------------------------------------------------------------------
-- 5. Get details of injecion data with stream info
------------------------------------------------------------------------
Select Wa.Workid as AnalysisId, Wa.Name as AnalysisName, wa.createddttm, Con_Item.Itemid, Con_Itemtype.Name As Itemtypename, 
CON_ITEMSTREAM.STREAMID, con_itemstream.streamsize,con_itemstream.status,
Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId') As Acquisitionsetid,
Con_Item.Createddttm As Resultcreated,
Con_Item.Statemodifieddttm As ResultCompleted,
Con_Item.State,
Con_Item.Statemodifieddttm-Con_Item.Createddttm As Resulttime,
Con_Itemstream.Createddttm As Streamcreated,
Con_Itemstream.Modifieddttm As Streamcompleted,
Con_Itemstream.Status As Streamstate,
Con_Itemstream.Modifieddttm-Con_Itemstream.Createddttm As Streamtime,
Con_Itemstream.Createddttm-Con_Item.Createddttm As ResultStreamDiff
FROM CON_ITEM, CON_ITEMRELATION,Con_Itemtype,CON_ITEMSTREAM, CON_WAMWORK WA
WHERE 
(
((CON_ITEM.ITEMID(+)=CON_ITEMRELATION.ITEMID) AND
(Con_Item.Itemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId') Or Con_Itemrelation.Relateditemid=Extractvalue(Wa.Workcontext, '/analyticalTaskDataContext/acquisitionSetId')))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
And
(con_itemtype.name='InjectionData')
And
(Con_Item.Itemid = Con_Itemstream.Itemid)
)
order by wa.createddttm desc,wa.workId,Con_Item.Createddttm;

Select Con_Wamitemdata.Workitemid,
Extractvalue (Con_Wamitemdata.Instancedata, '//result/@id', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As Udfid,
Con_Wamitem.Basedonitemid, 
Extractvalue (Con_Wamitemdata.Instancedata, '//result/@parentId', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As UdfParentId
From "EVEREST"."CON_WAMITEM", "EVEREST"."CON_WAMITEMDATA" 
WHERE ( "EVEREST"."CON_WAMITEMDATA"."WORKITEMID" = "EVEREST"."CON_WAMITEM"."WORKITEMID" )


SELECT * FROM CON_ITEMRELATION WHERE RELATIONTYPE='BASEDON';

SELECT * FROM CON_ITEM WHERE CommonName like '%QC_3013%'


------------------------------------------------------------------------
-- Get details of all content relations
------------------------------------------------------------------------
Select DISTINCT Con_Item.Itemid, 
Extractvalue (con_item.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') As InstanceDataId,
COn_Item.CommonName, Con_Item.VersionNo, con_item.issubitem, Con_Itemtype.Name As Itemtypename, 
CON_ITEMRELATION.RelationType, NVL(CON_ITEMRELATION.RELATEDITEMID,Con_Item.Itemid) as RELATEDITEMID,
GetItemTypeName(CON_ITEMRELATION.RELATEDITEMID) as RelatedItemType,
Extractvalue (con_item.instancedata, '/result/@parentId', 'xmlns="urn:www.waters.com/udf"') As ParentId,
GetItemInfo(CON_ITEMRELATION.RELATEDITEMID) as RelatedItemInfo,
GetItemParentInfo(relatedItemId) as ParentItemInfo,
con_item.exclusiveeditlock,
Con_Item.Createddttm,
Con_Item.Statemodifieddttm,
Con_Item.State,
con_item.datanodeid
FROM CON_ITEM, CON_ITEMRELATION,Con_Itemtype
WHERE 
(
(CON_ITEM.ITEMID=CON_ITEMRELATION.ITEMID (+))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
and
(Con_Item.Createddttm > sysdate-1)
and 
(CON_ITEMRELATION.RELATIONTYPE IS NOT NULL OR CON_ITEM.ITEMID in (SELECT RELATEDITEMID FROM CON_ITEMRELATION))
and
((con_item.CommonName like '%Test1_%') OR (con_item.CommonName like '%Test1_%') OR (GetItemInfo(CON_ITEMRELATION.RELATEDITEMID) like '%Test1_%' OR GetItemInfo(CON_ITEMRELATION.RELATEDITEMID) like '%Test1_%'))
--and 
--(con_item.islastversion=1)
--and
--(Con_Itemtype.Name='Analysis Method' OR GetItemTypeName(CON_ITEMRELATION.RELATEDITEMID)='Analysis Method')
)
order by con_item.Createddttm, datanodeid, RELATEDITEMID, ItemID;



Select Con_wamItemData.workid, Con_wamItemData.WorkItemid, 
Extractvalue (Con_wamItemData.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') As InstanceDataId, 
Con_wamItemData.instancedata
from Con_wamItemData;
--where Extractvalue (Con_wamItemData.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') != Con_wamItemData.WorkItemid;

Select DISTINCT Con_Item.Itemid, 
Extractvalue (Con_Item.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') As InstanceDataId,
con_item.*
from Con_Item
where 
Extractvalue (Con_Item.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') is not null
and Extractvalue (Con_Item.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') != itemid;


Select DISTINCT Con_Item.Itemid, 
Extractvalue (Con_Item.instancedata, '/result/@id', 'xmlns="urn:www.waters.com/udf"') As InstanceDataId,
Extractvalue (Con_Item.instancedata, '//sampleId', 'xmlns="urn:www.waters.com/udf"') As SampleId,
con_item.commonname
from Con_Item
where itemtypeid='50E20098FC5A4A998BA7FC211CE82848'
order by statemodifieddttm desc;


Select DISTINCT Con_wamItem.WorkItemid, Con_Item.CommonName, Con_Itemtype.Name As Itemtypename, CON_ITEMRELATION.RelationType, NVL(CON_ITEMRELATION.RELATEDITEMID,Con_Item.Itemid) as RELATEDITEMID,
GetItemTypeName(itemId) AS ItemTypeName,GetItemTypeName(CON_ITEMRELATION.RELATEDITEMID) as RelatedItemInfo,
GetItemParentInfo(relatedItemid) as ParentInfo,
con_item.exclusiveeditlock,
Con_Item.Createddttm,
Con_Item.Statemodifieddttm,
Con_Item.State
FROM CON_ITEM, CON_ITEMRELATION,Con_Itemtype
WHERE 
(
(CON_ITEM.ITEMID=CON_ITEMRELATION.ITEMID (+))
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
--and
--(Con_Item.Createddttm > sysdate-2)
and 
(CON_ITEMRELATION.RELATIONTYPE IS NOT NULL OR CON_ITEM.ITEMID in (SELECT RELATEDITEMID FROM CON_ITEMRELATION))
)
order by RELATEDITEMID, Createddttm;

select * from con_itemrelation where itemid='DC16EC2D09DA46258ECB196D140DBDA0' OR RELATEDITEMID='063CFDC71E81470B90F5190196746930' OR ITEMID='063CFDC71E81470B90F5190196746930';
select * from con_itemrelation where relationtype='COPYOF';
select * from con_item where itemid='04E9FFC5F3A648F59359D567A674A9BC';
select con_item.itemid, con_itemstream.streamid, con_itemtype.name from con_itemstream, con_item, con_itemtype
where con_itemstream.itemid=con_item.itemid and
con_item.itemtypeid=con_itemtype.itemtypeid;


Select con_item.itemid, con_item.commonname, Con_Item.Createddttm,
dbms_lob.getlength(xmltype.getclobval(Instancedata)) as XmlLength
From Con_Item, Con_Itemrelation,Con_Itemtype
Where 
(
(Con_Item.Itemid(+)=Con_Itemrelation.Itemid) 
And
(Con_Item.Itemtypeid=Con_Itemtype.Itemtypeid)
And
(con_itemtype.name='InjectionResult')
AND
(con_item.commonname like '%Alpraz%')
)
order by
Con_Item.Createddttm desc

-- get all sub-items for a Sample Set
Select con_item.itemid, con_item.commonname, Con_Item.Createddttm, versionno
from con_item where commonname like 'Kermit%30%'
and itemtypeid='B89E182F836745DA865C12D4EF60E97B';

select con_item.itemid, con_item.commonname, Con_Item.Createddttm, Con_Item.versionno,con_itemtype.Name,
Extractvalue (Con_item.Instancedata, '//result/status/propertyGroups/common/acqState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As Itemacqstatus,
Extractvalue (Con_item.Instancedata, '//result/status/propertyGroups/common/procState', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') As ItemProcStatus,
dbms_lob.getlength(xmltype.getclobval(Con_item.Instancedata)) as XmlLength
from con_item, con_itemrelation,con_itemtype
where con_item.itemid=con_itemrelation.itemid
and con_item.itemtypeid=con_itemtype.itemtypeid
and con_itemrelation.relateditemid='34ABA5DDAE5A4884BE00E00018F1C0D8'
order by Con_Item.Createddttm asc;




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



----------------------


