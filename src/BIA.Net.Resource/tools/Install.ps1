param($installPath, $toolsPath, $package, $project)

Write-Host "Begin script Install"

$resourceFolder = $project.ProjectItems | where-object {$_.Name -eq "Resources"}
$biaFolder = $resourceFolder.ProjectItems | where-object {$_.name -eq "BIA.Net"}
$item = $biaFolder.ProjectItems | where-object {$_.Name -eq "TextResources.resx"} 
$item.Properties.Item("CustomTool").Value = "PublicResXFileCodeGenerator"

Write-Host "Finish script Install"