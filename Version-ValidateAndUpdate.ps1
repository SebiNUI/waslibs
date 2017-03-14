[CmdletBinding()] 
Param(
	[Parameter(Mandatory=$True,Position=1)]
	[string]$MayorVersion,
	[Parameter(Mandatory=$True,Position=2)]
	[string]$MinorVersion,
	[Parameter(Mandatory=$False,Position=3)]
	[String]$Patch,
	[Parameter(Mandatory=$False,Position=4)]
	[string]$Semantic = "",
    [Parameter(Mandatory=$False,Position=5)]
	[string]$Revision = "",
	[Parameter(Mandatory=$False, Position=6)]
    [string]$TfsBuildNumber="" #Must match the pattern buildName_YYYYMMDD.r
)

if($error){
	$error.Clear()
}

if($Patch -ne "" -and $TfsBuildNumber -ne ""){
	throw "Is not allowed to specify the parameter TfsBuildNumber and Patch at the same time"
}

if($Patch -eq "" -and $TfsBuildNumber -eq ""){
	throw "You must specify the parameter TfsBuildNumber or Patch"
}
if($TfsBuildNumber -ne ""){
	if($Revision -ne "" -or $Semantic -ne "") {
		throw "Semantic and/or Revision can't be specified if you set the BuildName parameter"
	}
	else{
		Write-Host "Infering version from BuildName $TfsBuildNumber"
		$TfsBuildNumberRegEx = "_(\d\d\d\d\d\d\d\d)\.(\d+)"
		
		if($TfsBuildNumber -match $TfsBuildNumberRegEx){
			$buildDate = [DateTime]::ParseExact($matches[1], "yyyyMMdd", $null)
			$BuildMonthDay = $buildDate.DayOfYear.ToString("#000");
			$BuildRevision = ([int]$matches[2]).ToString("#00");
			Write-Host "Build Month and Day: $BuildMonthDay"
			Write-Host "Build Revision: $BuildRevision"		
			
			$Revision = $BuildRevision
			$NewVersion = $MayorVersion + "." + $MinorVersion + "." + $BuildMonthDay + $Revision
			$Semantic = "" #Ensure No Semantic Version for CI Builds
			
		}
		else{
			throw "Build format does not match the expected pattern (buildName_YYYYMMDD.r)"
		}
	}
}
else{
	$NewVersion = $MayorVersion + "." + $MinorVersion + "." + $Patch 
}

if(!$error -and $NewVersion -and $NewVersion -ne ""){
	
	$PackageVersion = $NewVersion
	if($Semantic -ne "") {
		$PackageVersion = $PackageVersion + "-" + $Semantic + $Revision
	}
	
	$PackageVersionTag = "$PackageVersion.r$Revision"  

	Write-Host "New Version: $NewVersion"
	Write-Host "Revision: $Revision"
	Write-Host "Package Version: $PackageVersion"
	Write-Host "Package Version: $PackageVersion"
	Write-Host "Invoking UpdateVersionFiles.ps1..."
    Invoke-Command -ScriptBlock  { .\UpdateVersionFiles.ps1 $NewVersion $Semantic $Revision }
	Write-Host "UpdateVersionFiles.ps1 Finished."
	
	Write-Host ("##vso[task.setvariable variable=NugetPackageVersion;]$PackageVersion")
	Write-Host "Environment Variable NugetPackageVersion set."
	
	Write-Host ("##vso[task.setvariable variable=PackageVersionTag;]$PackageVersionTag")
	Write-Host "Environment Variable PackageVersionTag set."
}
else{
	throw "New version for packages can't be determined."
}