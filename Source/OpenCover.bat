@ECHO OFF

SET config=Release

IF NOT EXIST "TestResults" mkdir TestResults

ECHO ---[ OpenCover + NUnit ]---
.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe ^
	-register:user ^
	-target:".\packages\NUnit.Console.3.0.1\tools\nunit3-console.exe" ^
	-targetargs:".\SharpGeometry.Tests\SharpGeometry.Tests.csproj --config=%config% -noh --work=.\TestResults --out=TestResults.txt" ^
	-output:".\TestResults\Coverage.xml" ^
	-filter:"+[*]* -[*Tests]*" ^
	-skipautoprops ^
	-showunvisited
