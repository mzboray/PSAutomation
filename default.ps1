properties {
    $binDir = "$PSScriptRoot\bin"
	$modules = "$binDir\Modules"
	$nunitConsole = "$PSScriptRoot\src\packages\NUnit.Runners.2.6.3\tools\nunit-console.exe"
	$psAutomationModule  = "$modules\PSAutomation"
}

Framework "4.5.1"

task default -depends Test

task Clean {
    rm $binDir -Recurse -Force
}

task Init -depends Clean {
    mkdir $binDir
    mkdir $modules
	mkdir $psAutomationModule
}

task Compile -depends Init {
	msbuild "src\PSAutomation.sln" /t:Rebuild /p:OutDir="$binDir"
	pushd $binDir
	cp PSAutomation.dll, PSAutomation.pdb, PSAutomation.psd1, PSAutomation.psm1, PSAutomation.Types.ps1xml $psAutomationModule	
	popd
}

task Test -depends Compile {
    $testDlls = (ls $binDir -Filter *.Test*.dll | Convert-Path) -join " "
    & $nunitConsole $testDlls /framework=net-4.5
}

task Run -depends Test {
    $oldModulePath = $env:PSModulePath
    if ($env:PSModulePath -notlike "*$modules*") {
        $env:PSModulePath += ";$modules"
    }
	Start-Process -File "powershell.exe" -ArgumentList "-NoExit", "-Command & { Import-Module PSAutomation }"
	$env:PSModulePath = $oldModulePath
}