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