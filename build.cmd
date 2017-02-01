@echo off
setlocal

set _config=%1
if not defined _config (
  set _config=Debug
)

echo Building Config '%_config%'

echo Restoring packages
dotnet restore

echo Cleaning projects
dotnet clean CommandLine.sln

echo Building solution
dotnet build CommandLine.sln -c %_config%

echo Running tests
dotnet test --no-build -c %_config% test\CommandLine.Tests\CommandLine.Tests.csproj

echo Creating NuGet package
dotnet pack --no-build -c %_config% src\CommandLine.csproj

endlocal

@echo on