@ECHO OFF
REM go to repository root
cd "%~dp0%\.."

SET config=Release
SET sln=.\Source\SharpGeometry.sln
SET msbuild=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe

IF NOT EXIST "%msbuild%" ECHO Could not find %msbuild% & PAUSE & GOTO :EOF
IF NOT EXIST "%sln%" ECHO Could not find %sln% & PAUSE & GOTO :EOF

ECHO ---[ Build %sln% @ %config% ]---
"%msbuild%" /nologo /verbosity:minimal "%sln%" /p:Configuration=%config%
