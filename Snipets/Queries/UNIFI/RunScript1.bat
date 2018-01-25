md Results

SET dbName=UNIFI
SET usr=%1
SET pw=%2

ECHO Running Query 01
SQLPLUS -S -M "HTML ON TABLE 'BORDER="2"'" %usr%/%pw%@%dbName% @GetAnalysisDetails01.sql > .\Results\GetAnalysisDetails01.html
ECHO Query 01 Complete

ECHO Running Query 02
SQLPLUS -S -M "HTML ON TABLE 'BORDER="2"'" %usr%/%pw%@%dbName% @GetAnalysisMethodDetails.sql > .\Results\GetAnalysisMethodDetails.html
ECHO Query 02 Complete

ECHO Running Query 03
SQLPLUS -S -M "HTML ON TABLE 'BORDER="2"'" %usr%/%pw%@%dbName% @GetLockInfo01.sql > .\Results\GetLockInfo01.html
ECHO Query 03 Complete