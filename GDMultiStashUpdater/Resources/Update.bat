@echo off
for %%F in ("Update\*") do call:copy "%%~F"
rmdir /S /Q "Update"
if exist "GDMultiStashUpdated.exe" move /Y "GDMultiStashUpdated.exe" "GDMultiStash.exe">nul
start cmd /c start "" "GDMultiStash.exe"
goto:exit
:copy
if exist "%~nx1" del "%~nx1"
copy "%~dp0%~1" "%~nx1">nul
exit /b 0
:exit
echo Update finished!
goto 2>nul & del "%~f0"