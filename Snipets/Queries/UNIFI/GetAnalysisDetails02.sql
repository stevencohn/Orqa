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
-- 1. Get context info for each open analysis
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

exit;