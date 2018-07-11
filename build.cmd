@echo off
setlocal

set _config=%1
if not defined _config (
  set _config=Debug
)

echo --------------------------
echo !!! Restoring packages !!!
echo --------------------------
dotnet restore

echo -------------------------
echo !!! Cleaning solution !!!
echo -------------------------
dotnet clean -c %_config%

echo -------------------------
echo !!! Building solution !!!
echo -------------------------
dotnet build -c %_config%

echo ---------------------
echo !!! Running tests !!!
echo ---------------------
dotnet test --no-build -c %_config% test\CommandLine.tests.csproj
dotnet test --no-build -c %_config% analyzer\CommandLine.Analyzer.Test\CommandLine.Analyzer.Test.csproj

if not "%_config%" == "Release" (
	echo =======================================================
	echo Skipping over package creation as not building Release
	echo =======================================================
	goto :eof
)

echo ------------------------------
echo !!! Creating NuGet package !!!
echo ------------------------------
dotnet pack -c %_config%

endlocal
@echo on
