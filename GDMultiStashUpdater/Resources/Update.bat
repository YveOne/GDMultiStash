@echo off
for %%F in ("Update\*") do call:copy "%%~F"
rmdir /S /Q "Update"
if exist "GDMultiStash_DontStartMe.exe" del "GDMultiStash.exe" & move "GDMultiStash_DontStartMe.exe" "GDMultiStash.exe">nul
start cmd /c start "" "GDMultiStash.exe"
goto:exit
:copy
if exist "%~nx1" del "%~nx1"
copy "%~dp0%~1" "%~nx1">nul
exit /b 0
:exit
echo Update finished!
goto 2>nul & del "%~f0"