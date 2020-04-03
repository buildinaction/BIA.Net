Add-Type -AssemblyName System.IO.Compression.FileSystem

$projectTemplateName = "BIATemplate"
$projectsNameSpace = "MyCompany.BIATemplate."
$curentUserName = $env:UserName
$RepSource="C:\Users\$curentUserName\Documents\Visual Studio 2019\My Exported Templates"
$pathZZProjectInstaller = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath(".")

# $pathZZProjectKit= $pathZZProjectInstaller + "\ZZProjectKit"
$RepTarget= $pathZZProjectInstaller + "\BIA.ProjectKit\Temp"
# $RepTargetProjectTemplates= $pathZZProjectInstaller + "\BIA.ProjectCreator\ProjectTemplates"

Remove-Item -Recurse -Force -path "$RepTarget\*"

Write-Host "Copy template $projectsNameSpace*.zip from : $RepSource"
Copy-Item "$RepSource\$projectsNameSpace*.zip" "$RepTarget\"

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
	$dirname = (Get-Item "$RepTarget\$file").Basename.substring($projectsNameSpace.length);
	# $dirname = (Get-Item "$RepTarget\$file").Basename
    New-Item -Force -ItemType directory -Path "$RepTarget\$dirname"
	Unzip "$RepTarget\$file" "$RepTarget\$dirname\"
}

Remove-Item "$RepTarget\*.zip"

Write-Host "Treate file : "
$allFiles = Get-ChildItem -File -Path "$RepTarget" -Exclude "*.vstemplate" -rec | Where-Object { Select-String $projectTemplateName $_ -Quiet }
foreach ($file in $allFiles)
{
	$filePath = $file.FullName
	$shortFilePath = $filePath.Replace($RepTarget,"")
	Write-Host " - $shortFilePath"
	$text = [IO.File]::ReadAllText("$filePath") -replace $projectTemplateName, "`$saferootprojectname`$"
	$text = $text -replace "MyCompany", "`$companyName`$"
	# $text = $text -replace "ZZDivisionNameZZ", "`$divisionName`$"
	# $text = $text -replace "ZZSupportMailZZ", "`$supportMail`$"
	[IO.File]::WriteAllText("$filePath", $text)
}

# $file = "$RepTarget/MVC/Web.config"
# $text = [IO.File]::ReadAllText("$file") -replace "http://localhost/static", "`$staticAddress`$"
# [IO.File]::WriteAllText("$file", $text)

# $file = "$RepTarget/MVC/packages.config"
# $text = [IO.File]::ReadAllText("$file") -replace "</packages>", "`$packageResources`
# </packages>"
# [IO.File]::WriteAllText("$file", $text)
Write-Host "Treate vstemplate file :"
$templateFiles = Get-ChildItem -File -Path "$RepTarget" *.vstemplate -rec | Where-Object { Select-String "</VSTemplate>" $_ -Quiet }
foreach ($file in $templateFiles)
{
	$filePath = $file.FullName
	$shortFilePath = $filePath.Replace($RepTarget,"")
	Write-Host " - $shortFilePath"
	$text = [IO.File]::ReadAllText("$filePath") -replace "</VSTemplate>", "  <WizardExtension>
    <Assembly>BIA.ProjectCreatorWizard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null</Assembly>
    <FullClassName>BIA.ProjectCreatorWizard.ChildWizard</FullClassName>
  </WizardExtension>
</VSTemplate>"
	[IO.File]::WriteAllText("$filePath", $text)
}

# $file = "$RepTarget/SyncDatabase/MyTemplate.vstemplate"
# $text = [IO.File]::ReadAllText("$file") -replace 'ReplaceParameters="false" (TargetFileName="[A-Za-z0-9\-_]*.scmp")', 'ReplaceParameters="true" $1'
# [IO.File]::WriteAllText("$file", $text)

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
