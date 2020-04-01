# Returns all line numbers containing the value passed as a parameter.
function GetLineNumber($pattern, $file) {
  $LineNumber = Select-String -Path $file -Pattern $pattern | Select-Object -ExpandProperty LineNumber
  return $LineNumber
}

# Deletes a set of lines whose number is between $start and $end.
function DeleteLine($start, $end, $file) {
  $i = 0
  $start--
  $end--
  Write-Host "start " $start "end " $end "file " $file
  (Get-Content $file) | Where-Object {
    ($i -lt $start -or $i -gt $end)
    $i++
  } | set-content $file -Encoding utf8
}

# Deletes lines between // Begin BIADemo and // End BIADemo 
function RemoveCodeExample {
  Get-ChildItem -File -Recurse -include *.csproj, *.cs, *.sln, *.json | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
    $lineBegin = @()
    $file = $_.FullName
  
    $searchWord = '// Begin BIADemo'
    $starts = GetLineNumber -pattern $searchWord -file $file
    $lineBegin += $starts
  
    $searchWord = '// End BIADemo'
    $ends = GetLineNumber -pattern $searchWord -file $file
    $lineBegin += $ends
  
    if ($lineBegin -and $lineBegin.Length -gt 0) {
      $lineBegin = $lineBegin | Sort-Object
      for ($i = $lineBegin.Length - 1; $i -gt 0; $i = $i - 2) {
        $start = [int]$lineBegin[$i - 1]
        $end = [int]$lineBegin[$i]
        DeleteLine -start $start -end $end -file $file
      }
    }
  }
}

function RenameFile {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -File -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldName, $newName)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldName, $newName) } }
}

function RenameFolder {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldName, $newName)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldName, $newName) } }
}

function RemoveItemFolder {
  param (
    [string]$path
  )
  if (Test-Path $path) {
    Write-Host "delete " $path " folder"
    Remove-Item $path -Recurse -Force -Confirm:$false
  }
}

function ReplaceProjectName {
  param (
    [string]$oldName,
    [string]$newName
  )
  Get-ChildItem -File -Recurse -include *.csproj, *.cs, *.sln, *.json, *.config | Where-Object { $_.FullName -NotLike "*/bin/*" -and $_.FullName -NotLike "*/obj/*" } | ForEach-Object { 
    $oldContent = [System.IO.File]::ReadAllText($_.FullName);
    $newContent = $oldContent.Replace($oldName, $newName);
    if ($oldContent -ne $newContent) {
      Write-Host $_.FullName
      [System.IO.File]::WriteAllText($_.FullName, $newContent)
    }
  }
}

# $oldName = Read-Host "old project name ?"
$oldName = 'BIADemo'
# $newName = Read-Host "new project name ?"
$newName = 'BIATemplate'

Write-Host "old name: " $oldName
Write-Host "new name: " $newName

# -------------------- Begin Remove folder DotNet and replace it ----------------------------
Write-Host "Remove .\DotNet"
Remove-Item -LiteralPath "DotNet" -Force -Recurse

$from = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath("..\$oldName\DotNet")
$to = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath(".\DotNet")
Write-Host "Copy from $from"
Write-Host "     to ..$to"

# param ex: excludeExtension = @(".md")
function Copy-WithFilter ($sourcePath, $destPath, $excludeFolder, $excludeExtension )
{
	Write-Host "Copy-WithFilter $sourcePath, $destPath"
    # Call this function again, using the child folders of the current source folder.
    Get-ChildItem -Path $sourcePath -Directory -Exclude $excludeFolder | % {Copy-WithFilter $_.FullName (Join-Path -Path $destPath -ChildPath $_.Name) $excludeFolder $excludeExtension}

    # Create the destination directory, if it does not already exist.
    if (!(Test-Path $destPath)) { New-Item -Path $destPath -ItemType Directory | Out-Null }

    # Copy the child files from source to destination.
    Get-ChildItem -Path $sourcePath -File | Where-Object { (($excludeExtension) -notcontains $_.Extension) } | Copy-Item -Force -Destination $destPath
}
Copy-WithFilter $from $to @('.vs', 'bin', 'obj')
# -------------------- End Remove folder DotNet and replace it ----------------------------


Set-Location -Path ".\DotNet"

Write-Host "Remove PlanesController.cs"
RemoveItemFolder -path 'MyCompany.BIADemo.Presentation.Api\Controllers\PlanesController.cs'
Write-Host "Remove PlaneModelBuilder.cs"
RemoveItemFolder -path 'MyCompany.BIADemo.Infrastructure.Data\ModelBuilders\PlaneModelBuilder.cs'
Write-Host "Remove Migrations folder"
RemoveItemFolder -path 'MyCompany.BIADemo.Infrastructure.Data\Migrations'
Write-Host "Remove MyCompany.BIADemo.Application\Plane"
RemoveItemFolder -path 'MyCompany.BIADemo.Application\Plane'
Write-Host "Remove MyCompany.BIADemo.Domain\PlaneModule"
RemoveItemFolder -path 'MyCompany.BIADemo.Domain\PlaneModule'
Write-Host "Remove MyCompany.BIADemo.Domain.Dto\Plane"
RemoveItemFolder -path 'MyCompany.BIADemo.Domain.Dto\Plane'

Write-Host "Remove code example"
RemoveCodeExample

Write-Host "replace project name"
ReplaceProjectName -oldName $oldName -newName $newName
ReplaceProjectName -oldName $oldName.ToLower() -newName $newName.ToLower()

Write-Host "Rename File"
RenameFile -oldName $oldName -newName $newName
RenameFile -oldName $oldName.ToLower() -newName $newName.ToLower()

Write-Host "Rename Folder"
RenameFolder -oldName $oldName -newName $newName
RenameFolder -oldName $oldName.ToLower() -newName $newName.ToLower()

Write-Host "Finish"

Set-Location -Path ".."

pause
