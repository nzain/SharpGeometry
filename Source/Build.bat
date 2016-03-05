@ECHO OFF

SET config=Release
SET sln=SharpGeometry.sln
SET msbuild=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe

IF NOT EXIST "%msbuild%" ECHO Could not find %msbuild% & PAUSE & EXIT -1
IF NOT EXIST "%sln%" ECHO Could not find %sln% & PAUSE & EXIT -1

ECHO ---[ Build %sln% @ %config% ]---
"%msbuild%" /nologo /verbosity:minimal "%sln%" /p:Configuration=%config%
