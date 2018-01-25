@echo off

:: Post Build Event:
::
::    cd $(ProjectDir)
::    $(ProjectDir)generate.bat $(SolutionDir) $(ConfigurationName) $(OutDir) $(TargetPath) $(ProjectDir)Bin\Merged $(ProjectDir)Bin\Merged\$(TargetFileName)
::


set solutionDir=%1
set configuration=%2
set sourceDir=%3
set sourceFilename=%4
set targetDir=%5
set targetFilename=%6

echo solutionDir..... %solutionDir%
echo configuration... %configuration%
echo sourceDir....... %sourceDir%
echo sourceFilename.. %sourceFilename%
echo targetDir....... %targetDir%
echo targetFilename.. %targetFilename%

::goto %configuration%

:: ===========================================================================
:Debug
call :PublishSnipets %sourceDir%
exit

:: ===========================================================================
:Release

if not exist %targetDir% md %targetDir%
if exist %targetFilename% del %targetFilename%

set filelist=River.Orqa.exe River.Orqa.Editor.dll

ilmerge /lib:%sourceDir% /AllowDup /t:winexe /out:%targetFilename% %filelist%
xcopy /q /y %sourceDir%Oracle.ManagedDataAccess.dll %targetDir%\
xcopy /q /y %sourceDir%River.Orqa.exe.config %targetDir%\

call :PublishSnipets %targetDir%

if exist %targetFileName% echo SUCCESS: File generated - %targetFileName%
if not exist %targetFilename% echo ERROR: Failed to generate file

exit

:: ===========================================================================
:PublishSnipets
set snipets=%1%\Snipets
if not exist %snipets% mkdir %snipets%
if not exist %snipets%\Queries mkdir %snipets%\Queries
if not exist %snipets%\Reports mkdir %snipets%\Reports
if not exist %snipets%\Templates mkdir %snipets%\Templates
xcopy /q /y /e %solutionDir%\Snipets\Queries %snipets%\Queries\
xcopy /q /y /e %solutionDir%\Snipets\Reports %snipets%\Reports\
xcopy /q /y /e %solutionDir%\Snipets\Templates %snipets%\Templates\
goto :EOF


::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
Debug
  solutionDir..... C:\River\Orqa\
  configuration... Debug
  sourceDir....... bin\Debug64\
  sourceFilename.. C:\River\Orqa\River.Orqa\bin\Debug64\River.Orqa.exe
  targetDir....... C:\River\Orqa\River.Orqa\Bin\Merged
  targetFilename.. C:\River\Orqa\River.Orqa\Bin\Merged\River.Orqa.exe

Release
  solutionDir..... C:\River\Orqa\
  configuration... Release
  sourceDir....... bin\Release64\
  sourceFilename.. C:\River\Orqa\River.Orqa\bin\Release64\River.Orqa.exe
  targetDir....... C:\River\Orqa\River.Orqa\Bin\Merged
  targetFilename.. C:\River\Orqa\River.Orqa\Bin\Merged\River.Orqa.exe
::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
