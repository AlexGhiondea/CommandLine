@echo off
setlocal

set INIT_TOOLS_LOG=%~dp0init-tools.log
set TOOLRUNTIME_DIR=%~dp0Tools
set DOTNET_PATH=%TOOLRUNTIME_DIR%\dotnetcli\
set DOTNET_CMD=%DOTNET_PATH%dotnet.exe

:: if force option is specified then clean the tool runtime and build tools package directory to force it to get recreated
if [%1]==[force] (
  if exist "%TOOLRUNTIME_DIR%" rmdir /S /Q "%TOOLRUNTIME_DIR%"
)

:: Download nuget
if NOT exist "%TOOLRUNTIME_DIR%\NuGet.exe" (
  if NOT exist "TOOLRUNTIME_DIR" mkdir "%TOOLRUNTIME_DIR%"
  powershell -NoProfile -ExecutionPolicy unrestricted -Command "(New-Object Net.WebClient).DownloadFile('https://www.nuget.org/nuget.exe', '%TOOLRUNTIME_DIR%\NuGet.exe')
)

if exist "%DOTNET_CMD%" goto :afterdotnetrestore

echo Installing dotnet cli...
if NOT exist "%DOTNET_PATH%" mkdir "%DOTNET_PATH%"
set DOTNET_VERSION=1.0.0-preview1-002700
set DOTNET_ZIP_NAME=dotnet-dev-win-x64.%DOTNET_VERSION%.zip
set DOTNET_REMOTE_PATH=https://dotnetcli.blob.core.windows.net/dotnet/beta/Binaries/%DOTNET_VERSION%/%DOTNET_ZIP_NAME%
set DOTNET_LOCAL_PATH=%DOTNET_PATH%%DOTNET_ZIP_NAME%
echo Installing '%DOTNET_REMOTE_PATH%' to '%DOTNET_LOCAL_PATH%' >> %INIT_TOOLS_LOG%
powershell -NoProfile -ExecutionPolicy unrestricted -Command "(New-Object Net.WebClient).DownloadFile('%DOTNET_REMOTE_PATH%', '%DOTNET_LOCAL_PATH%'); Add-Type -Assembly 'System.IO.Compression.FileSystem' -ErrorVariable AddTypeErrors; if ($AddTypeErrors.Count -eq 0) { [System.IO.Compression.ZipFile]::ExtractToDirectory('%DOTNET_LOCAL_PATH%', '%DOTNET_PATH%') } else { (New-Object -com shell.application).namespace('%DOTNET_PATH%').CopyHere((new-object -com shell.application).namespace('%DOTNET_LOCAL_PATH%').Items(),16) }" >> %INIT_TOOLS_LOG%
if NOT exist "%DOTNET_LOCAL_PATH%" (
  echo ERROR: Could not install dotnet cli correctly. See '%INIT_TOOLS_LOG%' for more details.
  goto :EOF
)

:afterdotnetrestore

echo Done initializing tools.

endlocal