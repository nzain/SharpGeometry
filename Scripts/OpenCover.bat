@ECHO OFF
REM go to repository root
cd "%~dp0%\.."

SET config=Release
SET nunitVersion=3.2.0
SET openCoverVersion=4.6.519

IF NOT EXIST "OpenCover" mkdir "OpenCover"

ECHO ---[ OpenCover + NUnit ]---
.\Source\packages\OpenCover.%openCoverVersion%\tools\OpenCover.Console.exe ^
	-register:user ^
	-target:".\Source\packages\NUnit.Console.%nunitVersion%\bin\nunit3-console.exe" ^
	-targetargs:".\Source\SharpGeometry.Tests\SharpGeometry.Tests.csproj --config=%config% -noh --work=.\OpenCover --out=TestResults.txt" ^
	-output:".\OpenCover\Coverage.xml" ^
	-filter:"+[*]* -[*Tests]*" ^
	-skipautoprops ^
	-showunvisited
