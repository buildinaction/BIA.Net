param($installPath, $toolsPath, $package, $project)

Write-Host "Begin script Install"

function CreateItemIfNotExist
{
	Param($ProjectItems)

	$IfNotExist = $ProjectItems | Where-Object { $_.Properties.Item("Filename").Value -eq "IfNotExist" }
	if ($IfNotExist -ne $null)
	{
		Write-Host "Folder IfNotExist found in " $IfNotExist.Properties.Item("FullPath").Value
		foreach($itemToTest in $IfNotExist.ProjectItems) 
		{
			$fileName = $itemToTest.Properties.Item("Filename").Value.ToString();
			Write-Host  "> Treate file " $fileName
			$itemToPreserve = $ProjectItems | Where-Object { $_.Properties.Item("Filename").Value -eq $fileName}

			if ($itemToPreserve -eq $null)
			{
				Write-Host "File with path $fileName doesn't exist, deploying default."
				(Get-Interface $ProjectItems "EnvDTE.ProjectItems").AddFromFileCopy($itemToTest.Properties.Item("FullPath").Value)
			}
			else
			{
				Write-Host "File $fileName already exists, preserving existing file."
			}
			(Get-Interface $itemToTest "EnvDTE.ProjectItem").Delete()
		}

		if ($IfNotExist -ne $null)
		{
			(Get-Interface $IfNotExist "EnvDTE.ProjectItem").Delete()
		}
	}
}

function CreateItemIfNotExistInProjectItems
{
	Param($ProjectItems)

    CreateItemIfNotExist -ProjectItems $ProjectItems
	foreach($item in $ProjectItems) 
	{
        CreateItemIfNotExistInProjectItems -ProjectItems $item.ProjectItems		
	}
}


function ReplaceDDLGenerationTemplate
{
	Param([string]$filePath, [string[]]$newProperties)

	# Load project XML. 
	$doc = New-Object System.Xml.XmlDocument 
	$doc.Load($filePath) 

    
	
	$namespace = 'http://schemas.microsoft.com/developer/msbuild/2003'

    $pathNode = '/*[name()="edmx:Edmx"]/*[name()="edmx:Designer"]/*[name()="edmx:Options"]/*[name()="DesignerInfoPropertySet"]'
	$node = $doc.SelectSingleNode($pathNode)
	if($node -ne $null) {
        $DDLGen = $node.SelectSingleNode('*[@Name="DDLGenerationTemplate"]')
       
        if ($DDLGen -ne $null)
        {
		    $DDLGen.SetAttribute("Value", $newProperties)
        }
        else
        {
            $newDesignerProperty = $doc.CreateElement("DesignerProperty",  $doc.DocumentElement.NamespaceURI)
            $newDesignerProperty.SetAttribute("Name", "DDLGenerationTemplate")
            $newDesignerProperty.SetAttribute("Value", $newProperties)
            $node.AppendChild($newDesignerProperty)
        }
	}
 
	 # Save changes. 
	 $doc.Save($filePath) 
} 


function ReplaceDDLGenerationTemplateInProjectItems
{
	Param($ProjectItems)

	foreach($item in $ProjectItems) 
	{
        ReplaceDDLGenerationTemplateInProjectItems -ProjectItems $item.ProjectItems		
		$fileName = $item.Properties.Item("Filename").Value.ToString();
		if ([System.IO.Path]::GetExtension($fileName) -eq ".edmx" )
		{
			Write-Host "Edmx found : " $item.Properties.Item("FullPath").Value
			ReplaceDDLGenerationTemplate -filePath $item.Properties.Item("FullPath").Value -newProperties "Template\BIA_SSDLToSQL.tt"
		}
	}
}

CreateItemIfNotExistInProjectItems -ProjectItems $project.ProjectItems

$templatefolder = $project.ProjectItems | where-object {$_.Name -eq "Template"} 
$item = $templatefolder.ProjectItems | where-object {$_.Name -eq "BIA_SSDLToSQL.tt"} 
$item.Properties.Item("BuildAction").Value = [int]0
$item.Properties.Item("CustomTool").Value = ""
$item = $templatefolder.ProjectItems | where-object {$_.Name -eq "BIA_Post_SSDLToSQL.tt"} 
$item.Properties.Item("BuildAction").Value = [int]0
$item.Properties.Item("CustomTool").Value = ""

ReplaceDDLGenerationTemplateInProjectItems -ProjectItems $project.ProjectItems
