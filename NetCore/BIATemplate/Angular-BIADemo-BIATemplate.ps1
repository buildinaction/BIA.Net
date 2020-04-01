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
function RemoveCodeExample ($sourcePath, $excludeFolder, $excludeExtension ) {

  # Call this function again, using the child folders of the current source folder.
  Get-ChildItem -Path $sourcePath -Directory -Exclude $excludeFolder | % {RemoveCodeExample $_.FullName $excludeFolder $excludeExtension}

  Get-ChildItem -Path $sourcePath -File | Where-Object { (($excludeExtension) -notcontains $_.Extension) } | Where-Object { Select-String "Begin BIADemo" $_ -Quiet } | ForEach-Object { 
    $lineBegin = @()
    $file = $_.FullName
	Write-Host "RemoveCodeExample in " $file
  
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

function RemoveFolder {
  param (
    [string]$path
  )
  if (Test-Path $path) {
    Write-Host "delete " $path " folder"
    Remove-Item $path -Recurse -Force -Confirm:$false
  }
}

function ReplaceProjectName ($oldName,$newName,$sourcePath, $excludeFolder, $excludeExtension) 
{
  # Write-Host "ReplaceProjectName ($oldName) $sourcePath exclude $excludeFolder"
  # Call this function again, using the child folders of the current source folder.
  Get-ChildItem -Path $sourcePath -Directory -Exclude $excludeFolder | % {ReplaceProjectName $oldName $newName $_.FullName $excludeFolder $excludeExtension}

  Get-ChildItem -Path $sourcePath -File | Where-Object { (($excludeExtension) -notcontains $_.Extension) } | Where-Object { Select-String $oldName $_ -Quiet } | ForEach-Object { 
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

# -------------------- Begin Remove folder Angular and replace it ----------------------------
Write-Host "Remove .\Angular"
Remove-Item -LiteralPath "Angular" -exclude "node_modules" -Force -Recurse

$from = "..\$oldName\Angular"
$to = ".\Angular"
Write-Host "Copy from $from"
Write-Host "     to ..$to"

# param ex: excludeExtension = @(".md")
function Copy-WithFilter ($sourcePath, $destPath, $excludeFolder, $excludeExtension )
{
	# Write-Host "Copy-WithFilter $sourcePath, $destPath"
    # Call this function again, using the child folders of the current source folder.
    Get-ChildItem -Path $sourcePath -Directory -Exclude $excludeFolder | % {Copy-WithFilter $_.FullName (Join-Path -Path $destPath -ChildPath $_.Name) $excludeFolder $excludeExtension}

    # Create the destination directory, if it does not already exist.
    if (!(Test-Path $destPath)) { New-Item -Path $destPath -ItemType Directory | Out-Null }

    # Copy the child files from source to destination.
    Get-ChildItem -Path $sourcePath -File | Where-Object { (($excludeExtension) -notcontains $_.Extension) } | Copy-Item -Force -Destination $destPath
}
Copy-WithFilter $from $to @('node_modules')
# -------------------- End Remove folder Angular and replace it ----------------------------

Set-Location -Path ./Angular

Write-Host "RemoveFolder dist"
RemoveFolder -path 'dist'
# Write-Host "RemoveFolder node_modules"
# RemoveFolder -path 'node_modules'
Write-Host "RemoveFolder src\app\features\planes"
RemoveFolder -path 'src\app\features\planes'
Write-Host "RemoveFolder src\app\features\planes-page"
RemoveFolder -path 'src\app\features\planes-page'

Write-Host "Remove code example"
RemoveCodeExample "." @('node_modules','dist','scss','docs','assets') @('.ps1', '.md')

Write-Host "replace project name"
ReplaceProjectName $oldName $newName "." @('node_modules','dist','scss','docs','assets') @('.ps1')
Write-Host "replace project name lower"
ReplaceProjectName $oldName.ToLower() $newName.ToLower() "." @('node_modules','dist','scss','docs','assets') @('.ps1')

Write-Host "npm install"
npm install
# Write-Host "ng build --aot"
# ng build --aot
Write-Host "Finish"

Set-Location -Path ".."

pause
