@ECHO OFF

SET config=Release
SET msbuild=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
SET sln=SharpGeometry.sln

IF NOT EXIST "%msbuild%" ECHO Could not find %msbuild% & PAUSE & EXIT -1
IF NOT EXIST "%sln%" ECHO Could not find %sln% & PAUSE & EXIT -1
IF NOT EXIST "TestResults" mkdir TestResults

ECHO ---[ Build %sln% @ %config% ]---
"%msbuild%" /nologo /verbosity:minimal "%sln%" /p:Configuration=%config%

ECHO ---[ OpenCover + NUnit ]---
.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe ^
	-register:user ^
	-target:".\packages\NUnit.Console.3.0.1\tools\nunit3-console.exe" ^
	-targetargs:".\SharpGeometry.Tests\SharpGeometry.Tests.csproj --config=%config% -noh --work=.\TestResults --out=TestResults.txt" ^
	-output:".\TestResults\Coverage.xml" ^
	-filter:"+[*]* -[*Tests]*" ^
	-skipautoprops ^
	-showunvisited

ECHO ---[ ReportGenerator ]---
.\packages\ReportGenerator.2.4.4.0\tools\ReportGenerator.exe ^
	-reports:".\TestResults\Coverage.xml" ^
	-targetdir:".\TestResults" ^
	-verbosity:Info ^
	-reporttypes:"Html;Badges"

ECHO ---[ Display Results ]---

START .\TestResults\index.htm

PAUSE