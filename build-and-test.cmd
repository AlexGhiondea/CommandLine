@echo off
setlocal

set _projectName=CommandLine
set _testProject=test\%_projectName%.tests.csproj
set _analyzerTestProject=analyzer\%_projectName%.Analyzer.Test\%_projectName%.Analyzer.Test.csproj

set _config=%1
if not defined _config (
  set _config=Debug
)

echo --------------------------
echo !!! Restoring packages !!!
echo --------------------------
dotnet restore

call build.cmd

echo ---------------------
echo !!! Running tests !!!
echo ---------------------
dotnet test --no-build -c %_config% %_testProject%
dotnet test --no-build -c %_config% %_analyzerTestProject%

endlocal
@echo on