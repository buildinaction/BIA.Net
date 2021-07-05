#Before launch the script:
#		- Extract the project templateFiles of solution BIATemplate (use Ctrl+E Ctrl+X in visual studio)
#		- Verify path $RepSource
#		- Verify that in path $RepSource there is only the BIATemplate projects (9 items)
#		- Veriy the path $RepAdditionalFiles


Add-Type -AssemblyName System.IO.Compression.FileSystem

$projectTemplateName = "BIATemplate"
$companyateTemplateName = "TheBIADevCompany"
$RepSource="C:\Users\L025308\Documents\Visual Studio 2019\My Exported Templates"
$RepAdditionalFiles="..\..\BIATemplate\DotNet"
$RepTarget= Resolve-Path -Path ".\BIA.ProjectCreatorTemplateV3\Temp"
$RepTargetAdditionalFiles= Resolve-Path -Path ".\BIA.ProjectCreator\AdditionalFiles"


if (!(Test-Path -path $RepTarget)) {New-Item $RepTarget -Type Directory}

Remove-Item -Recurse -Force -path "$RepTarget\*"

Write-Host "Copy $RepSource\*.zip to $RepTarget\" 
Copy-Item "$RepSource\*.zip" "$RepTarget\" -Recurse -Force


Add-Type -AssemblyName System.IO.Compression.FileSystem
function Unzip
{
    param([string]$zipfile, [string]$outpath)

    [System.IO.Compression.ZipFile]::ExtractToDirectory($zipfile, $outpath)
}
function Zip ([string]$zipfile, [string]$outpath) {
   [System.IO.Compression.ZipFile]::CreateFromDirectory($outpath, $zipfile)
}

$zipFiles = Get-ChildItem "$RepTarget" *.zip
foreach ($file in $zipFiles)
{
	$dirname = (Get-Item "$RepTarget\$file").Basename.Replace("$companyateTemplateName.$projectTemplateName.","")
    New-Item -Force -ItemType directory -Path "$RepTarget\$dirname"
	Unzip "$RepTarget\$file" "$RepTarget\$dirname\"
}

Remove-Item "$RepTarget\*.zip"

# Remove company config files
$allFiles = Get-ChildItem -File -Path "$RepTarget" -Include "*.*.json" -Exclude "*.Example*.json" -rec | Where-Object {$_.Name -like "appsettings.*.json" -or $_.Name -like "bianetconfig.*.json"}
foreach ($file in $allFiles)
{
	$filePath = $file.FullName
	Write-Host "Remove file : $filePath"
	Remove-Item "$filePath"
}

$allFiles = Get-ChildItem -File -Path "$RepTarget" -Include "*.vstemplate" -rec
foreach ($file in $allFiles)
{
	$filePath = $file.FullName
	Write-Host "Clean template file : $filePath"
	Set-Content -Path $filePath -Value (get-content -Path $filePath | Where-Object { 
	($_ -NotMatch '(appsettings|bianetconfig).*.json*' -or $_ -like '*appsettings.Example*.json*' -or $_ -like '*bianetconfig.Example*.json*')
	})
}




$allFiles = Get-ChildItem -File -Path "$RepTarget" -Exclude "*.vstemplate" -rec | Where-Object { Select-String $projectTemplateName $_ -Quiet }
foreach ($file in $allFiles)
{
	$filePath = $file.FullName
	# Write-Host "Treate file : $filePath"
	$text = [IO.File]::ReadAllText("$filePath") -replace $projectTemplateName, "`$saferootprojectname`$"
	$text = [IO.File]::ReadAllText("$filePath") -replace $companyateTemplateName, "`$safecompanyName`$"
	[IO.File]::WriteAllText("$filePath", $text)
}

$templateFiles = Get-ChildItem -File -Path "$RepTarget" *.vstemplate -rec | Where-Object { Select-String "</VSTemplate>" $_ -Quiet }
foreach ($file in $templateFiles)
{
	$filePath = $file.FullName
	# Write-Host "Treate file : $filePath"
	$text = [IO.File]::ReadAllText("$filePath") -replace "</VSTemplate>", "  <WizardExtension>
    <Assembly>BIA.ProjectCreatorWizard, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
    <FullClassName>BIA.ProjectCreatorWizard.ChildWizard</FullClassName>
  </WizardExtension>
</VSTemplate>"
	[IO.File]::WriteAllText("$filePath", $text)
}

Copy-Item "$RepAdditionalFiles\BIA.ruleset" "$RepTargetAdditionalFiles\" -Force
Copy-Item "$RepAdditionalFiles\Directory.Build.props" "$RepTargetAdditionalFiles\" -Force
Copy-Item "$RepAdditionalFiles\*.ps1" "$RepTargetAdditionalFiles\" -Force
#Copy-Item "$RepAdditionalFiles\Docs" "$RepTargetAdditionalFiles" -Force –Recurse

# $file = "$RepTarget/SyncDatabase/MyTemplate.vstemplate"
# $text = [IO.File]::ReadAllText("$file") -replace 'ReplaceParameters="false" (TargetFileName="[A-Za-z0-9\-_]*.scmp")', 'ReplaceParameters="true" $1'
# [IO.File]::WriteAllText("$file", $text)

# $ScmpFiles = Get-ChildItem -File -Path "$RepTarget\SyncDatabase\Scmp" * | Where-Object { Select-String "VM-ISDT-DEV" $_ -Quiet }
# foreach ($file in $ScmpFiles)
# {
# 	$filePath = $file.FullName
# 	Write-Host "Treate file : $filePath"
# 	$text = [IO.File]::ReadAllText("$filePath") -replace "VM-ISDT-DEV", "."
# 	[IO.File]::WriteAllText("$filePath", $text)
# }

# $file = "Model/MyTemplate.vstemplate"
# Write-Host "Treate file : $file"
# $text = [IO.File]::ReadAllText("$RepTarget/$file") -replace 'ReplaceParameters="false" TargetFileName="ProjectDB.edmx"', 'ReplaceParameters="true" TargetFileName="ProjectDB.edmx"'
# [IO.File]::WriteAllText("$RepTarget/$file", $text)

# Project ZZCompanyNameZZ Template
# function ManageSingleProject ($projectName,[int] $SortOrder)
# {
# 	Write-Host "Project $projectName"
# 	$displayProjectName = $projectName

# 	if($projectName -eq "MVC")
# 	{
# 		$displayProjectName = "MVC web application"
# 	}


# 	$dirname =  $projectName
# 	$file = "$RepTarget\$dirname\MyTemplate.vstemplate"
# 	$zipFiles = "$projectName.zip"

# 	Copy-Item -Path $pathZZProjectKit\"__TemplateIcon.jpg" -Destination "$RepTarget\$dirname\"

# 	Write-Host "Get XML"

# 	$xml = [xml](Get-Content $pathZZProjectKit/"BIA.vstemplate")

# 	$xmlName = $xml.VSTemplate.TemplateData.Name
# 	$xmlDesc = $xml.VSTemplate.TemplateData.Description
# 	$xmlSortOrder = [int] $xml.VSTemplate.TemplateData.SortOrder
# 	$xmlIcon = $xml.VSTemplate.TemplateData.Icon
# 	$defaultName = "<DefaultName>" + $xml.VSTemplate.TemplateData.DefaultName + "</DefaultName>"

# 	Write-Host "Get XML Target"

# 	$xmlTarget = [xml](Get-Content $file)
# 	$xmlTarget.VSTemplate.TemplateData.Name = $xmlName + " > " + $displayProjectName 
# 	$xmlTarget.VSTemplate.TemplateData.Description = $xmlDesc + " > " + $displayProjectName
# 	$xmlTarget.VSTemplate.TemplateData.SortOrder = [string] ($xmlSortOrder + $SortOrder)
# 	$xmlTarget.VSTemplate.TemplateData.Icon = $xmlIcon 

# 	Write-Host "Save XML Target"
# 	$xmlTarget.Save($file)

# 	Write-Host "Remove zip"
# 	if (Test-Path "$RepTargetProjectTemplates\$zipFiles") {
# 	  Remove-Item "$RepTargetProjectTemplates\$zipFiles"
# 	}

# 	Write-Host "Zip"

# 	Zip "$RepTargetProjectTemplates\$zipFiles" "$RepTarget\$dirname"
# }


# ManageSingleProject -projectName "MVC" -SortOrder 1
# ManageSingleProject -projectName "WebApi" -SortOrder 2
# ManageSingleProject -projectName "Business" -SortOrder 3
# ManageSingleProject -projectName "Common" -SortOrder 4
# ManageSingleProject -projectName "Model" -SortOrder 5
# ManageSingleProject -projectName "SyncDatabase" -SortOrder 6
# ManageSingleProject -projectName "Template" -SortOrder 7
# ManageSingleProject -projectName "WindowsService" -SortOrder 8
# ManageSingleProject -projectName "Test" -SortOrder 9
# ManageSingleProject -projectName "Connector" -SortOrder 10
