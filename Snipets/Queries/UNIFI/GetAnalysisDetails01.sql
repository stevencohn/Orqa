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
Extractvalue (Workcontext, '/analyticalTaskDataContext/isReadOnly') As IsReadOnly,
Extractvalue (Workcontext, '/analyticalTaskDataContext/basedOnExistingData') As basedOnExistingData,
wa.ModVersion,
wa.Workcontext
From Con_Wamwork Wa, SEC_User
WHERE 
Wa.USERID=SEC_USER.USERID;

------------------------------------------------------------------------
-- 2. Get sub-item counts
------------------------------------------------------------------------
Select Wa.Workid As Analysisid, Wa.Name As Analysisname, Con_Itemtype.Name,
count(Con_Wamitem.WorkId) as ItemCount
From Con_Wamwork Wa, Con_Wamitem, Con_Itemtype
WHERE 
(
(WA.WORKID=Con_Wamitem.Workid)
And
(Con_Wamitem.Itemtypeid=Con_Itemtype.Itemtypeid)
)
GROUP BY Wa.Workid, Wa.Name, Con_Itemtype.Name
order by Wa.Workid;

------------------------------------------------------------------------
-- 3. Get sub-item info for each open analysis
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
)
order by wa.workId, wa.createddttm desc;


------------------------------------------------------------------------
-- 4. Get the number of injection data items for each open analysis
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
--And
--(con_itemtype.name='InjectionData')
)
--GROUP BY Wa.Workid, Wa.Name, wa.createddttm, Extractvalue (Workcontext, '/analyticalTaskDataContext/acquisitionSetId')
order by wa.createddttm desc, wa.workId;


select b.workid, b.workItemId, a.modVersion, a.lastappliedmodversion,
extractvalue(value(resultXml), 'result/@id', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "WamInstanceId",
extractvalue(value(resultXml), 'result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "ResultName",
extractvalue(value(resultXml), 'result/sample/propertyGroups/quantitationProperties/processingOptions', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "processingOptions",
extractvalue(value(resultXml), 'result/propertyGroups/common/usedNoiseReference', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "usedNoiseReference",
 a.newitemid,
extractvalue(value(resultXml2), 'result/@id', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "InstanceId",
extractvalue(value(resultXml2), 'result/propertyGroups/common/usedNoiseReference', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "usedNoiseReference2"
from con_wamitem a, con_wamitemdata b, con_item c,
       table(xmlsequence(extract(b.InstanceData, '//result', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'))) resultXml,
       table(xmlsequence(extract(c.InstanceData, '//result', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'))) resultXml2
where
a.workitemid=b.workitemid
and c.itemid=a.newitemid
order by workitemid;
       
       

select a.workid, a.workItemId, 
extractvalue(value(resultXml), 'result/cor:common/cor:name', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "ResultName",
extractvalue(value(resultXml), 'result/references/reference/@guid', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"') as "ChannelsRef",
a.worksectionId, 
extractvalue(value(channelXml), 'channel/@id', 'xmlns="urn:www.waters.com/udf"') as "ChannelId",
extractvalue(value(channelXml), 'channel/propertyGroups/common/channelName', 'xmlns="urn:www.waters.com/udf"') as "ChannelName",
extractvalue(value(channelXml), 'channel/@dataDescription', 'xmlns="urn:www.waters.com/udf"') as "DataDescription",
extractvalue(value(channelXml), 'channel/@derived', 'xmlns="urn:www.waters.com/udf"') as "Derived",
extractvalue(value(channelXml), 'channel/stream/@location', 'xmlns="urn:www.waters.com/udf"') as "StreamLocation"
from con_wamitemsection a, con_wamitemdata b,
       table(xmlsequence(extract(a.XmlData, '//channel', 'xmlns="urn:www.waters.com/udf"'))) channelXml,
       table(xmlsequence(extract(b.InstanceData, '//result', 'xmlns="urn:www.waters.com/udf" xmlns:cor="urn:www.waters.com/Core"'))) resultXml
where 
a.workitemid=b.workitemid and
UPPER(a.SectionType)='CHANNELS'
       order by workitemid, worksectionid;