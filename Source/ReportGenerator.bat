@ECHO OFF

ECHO ---[ ReportGenerator ]---
.\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe ^
	-reports:".\TestResults\Coverage.xml" ^
	-targetdir:".\TestResults" ^
	-verbosity:Info ^
	-reporttypes:"Html;Badges;TextSummary"

ECHO ---[ Display Results ]---

START .\TestResults\index.htm

PAUSE