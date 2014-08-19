call "%VS120ComnTools%vsvars32.bat"
pushd "%~dp0"
set modules=%~dp0bin\Modules\
mkdir %modules%PSAutomation
msbuild "src\PSAutomation.sln" /p:OutDir="%modules%PSAutomation"
echo %PSModulePath% | findstr /L "%modules%" >NUL
if errorlevel 1 (
    set PSModulePath=%PSModulePath%;%modules%
)
start powershell.exe
popd