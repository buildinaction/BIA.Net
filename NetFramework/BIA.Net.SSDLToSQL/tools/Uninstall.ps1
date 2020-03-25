param($installPath, $toolsPath, $package, $project)

Write-Host "Begin uninstall script BIA.Net.SSDLToSQL"

function RemoveDDLGenerationTemplate
{
	Param([string]$filePath)

	# Load project XML. 
	$doc = New-Object System.Xml.XmlDocument 
	$doc.Load($filePath) 

    $pathNode = '/*[name()="edmx:Edmx"]/*[name()="edmx:Designer"]/*[name()="edmx:Options"]/*[name()="DesignerInfoPropertySet"]'
	$node = $doc.SelectSingleNode($pathNode)
	if($node -ne $null) {
        $DDLGen = $node.SelectSingleNode('*[@Name="DDLGenerationTemplate"]')
       
        if ($DDLGen -ne $null)
        {
            $node.RemoveChild($DDLGen)
        }
	}
 
	 # Save changes. 
	 $doc.Save($filePath) 
} 

function RemoveDDLGenerationTemplateInProjectItems
{
	Param($ProjectItems)

	foreach($item in $ProjectItems) 
	{
        RemoveDDLGenerationTemplateInProjectItems -ProjectItems $item.ProjectItems		
		$fileName = $item.Properties.Item("Filename").Value.ToString();
		if ([System.IO.Path]::GetExtension($fileName) -eq ".edmx" )
		{
			Write-Host "Edmx found : " + $item.Properties.Item("FullPath").Value
			RemoveDDLGenerationTemplate -filePath $item.Properties.Item("FullPath").Value
		}
	}
}

RemoveDDLGenerationTemplateInProjectItems -ProjectItems $project.ProjectItems