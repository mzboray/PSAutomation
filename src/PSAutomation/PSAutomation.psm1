function Register-FocusChangedEvent {
	param([ScriptBlock]$Action)

	$evt = [PSAutomation.PSAutomationEvent]::Instance
	Register-ObjectEvent $evt -EventName FocusChanged -SourceIdentifier "PSAutomation.FocusChanged" -Action $Action -SupportEvent | out-null
}

Set-Alias gce Get-ChildElement
Set-Alias nc New-Condition
Export-ModuleMember -Alias * -Function * -Cmdlet *