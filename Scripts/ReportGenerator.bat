@ECHO OFF
REM go to repository root
cd "%~dp0%\.."

IF NOT EXIST "OpenCover" ECHO Run OpenCover.bat first & PAUSE & GOTO :EOF

ECHO ---[ ReportGenerator ]---
.\Source\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe ^
	-reports:".\OpenCover\Coverage.xml" ^
	-targetdir:".\OpenCover" ^
	-verbosity:Info ^
	-reporttypes:"Html;Badges;TextSummary"

ECHO ---[ Display Results ]---

START .\OpenCover\index.htm
