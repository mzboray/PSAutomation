$ErrorActionPreference = "Stop"
Push-Location $PSScriptRoot
$modules = "$PSScriptRoot\bin\Modules"
New-Item "$modules\PSAutomation" -ItemType Directory -Force | Write-Debug
& msbuild "src\PSAutomation.sln" /t:Rebuild /p:OutDir="$modules\PSAutomation"
if ($env:PSModulePath -notlike "*$modules*") {
    $env:PSModulePath += ";$modules"
}
Start-Process -File powershell.exe -ArgumentList "-NoExit", "-Command & { Import-Module PSAutomation }"
Pop-Location