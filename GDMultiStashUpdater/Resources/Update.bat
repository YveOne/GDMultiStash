@echo off

rem let updater close first
timeout /T 1 >nul

echo Copying files... 
timeout /T 1 >nul
for %%F in ("Update\*") do call:copy "%%~F"
rmdir /S /Q "Update"

if exist "GDMultiStashUpdated.exe" move /Y "GDMultiStashUpdated.exe" "GDMultiStash.exe">nul
start cmd /c start "" "GDMultiStash.exe"
goto:exit

:copy
if exist "%~nx1" del "%~nx1"
copy "%~dp0%~1" "%~nx1" 1>nul
exit /b 0

:exit
echo Update finished! Exiting in 3 seconds...
timeout /T 3 >nul
goto 2>nul & del "%~f0"