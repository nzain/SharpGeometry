@ECHO OFF
REM go to repository root
cd "%~dp0%\.."

SET config=Release

IF NOT EXIST "OpenCover" mkdir "OpenCover"

ECHO ---[ OpenCover + NUnit ]---
.\Source\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe ^
	-register:user ^
	-target:".\Source\packages\NUnit.Console.3.0.1\tools\nunit3-console.exe" ^
	-targetargs:".\Source\SharpGeometry.Tests\SharpGeometry.Tests.csproj --config=%config% -noh --work=.\OpenCover --out=TestResults.txt" ^
	-output:".\OpenCover\Coverage.xml" ^
	-filter:"+[*]* -[*Tests]*" ^
	-skipautoprops ^
	-showunvisited
