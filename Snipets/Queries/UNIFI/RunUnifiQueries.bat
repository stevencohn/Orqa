REM **** Usage: RunUnifieQueries <dbusername> <dbPassword>

md Results

SET dbName=UNIFI
SET usr=%1
SET pw=%2


for /f %%d in ('dir /b "Get*.sql" 2^>nul') do (
	  
	    call :RunSql %%~nd
	  
	)
	goto :EOF
	:RunSql
		set xnam=%1%
		exit|SQLPLUS -S -M "HTML ON TABLE 'BORDER="2"'" %usr%/%pw%@%dbName% @%xnam%.sql > .\Results\%xnam%.htm

		goto :EOF
