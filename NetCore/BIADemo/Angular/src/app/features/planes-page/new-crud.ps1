$oldSelector = read-host "old crud name? (singular)"
$oldSelectorPlural = read-host "old crud name? (plural)"
$newSelector = read-host "new crud name? (singular)"
$newSelectorPlural = read-host "new crud name? (plural)"

$oldSelector = $oldSelector.ToLower();
$newSelector = $newSelector.ToLower();
$oldSelectorPlural = $oldSelectorPlural.ToLower();
$newSelectorPlural = $newSelectorPlural.ToLower();

$TextInfo = (Get-Culture).TextInfo
$oldClassName = $TextInfo.ToTitleCase("$oldSelector") #.replace('-', '')
$newClassName = $TextInfo.ToTitleCase("$newSelector") #.replace('-', '')

$old = " (PageMode)"
$new = ""
write-host "old " $old "new " $new
Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $old, $new) | Set-Content -Path $_.PSPath }

$old = "-mode-page"
$new = ""
write-host "old " $old "new " $new
Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $old, $new) | Set-Content -Path $_.PSPath }

$old = "examples/" + $oldSelectorPlural + "-page"
$new = $newSelectorPlural
write-host "old " $old "new " $new
Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $old, $new) | Set-Content -Path $_.PSPath }

# ----------------------------------------

$old = $oldSelectorPlural
$new = $newSelectorPlural
write-host "old " $old "new " $new
Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $old, $new) | Set-Content -Path $_.PSPath }

$old = $oldSelector
$new = $newSelector
write-host "old " $old "new " $new
Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $old, $new) | Set-Content -Path $_.PSPath }

$old = $oldClassName
$new = $newClassName
write-host "old " $old "new " $new
Get-ChildItem -File -Recurse | ForEach-Object { ((Get-Content -path $_.PSPath -Raw) -creplace $old, $new) | Set-Content -Path $_.PSPath }

# file name
write-host "replace file name plural"
Get-ChildItem -File -Recurse | ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelectorPlural, $newSelectorPlural) } 
write-host "replace file name singular"
Get-ChildItem -File -Recurse | ForEach-Object { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } 

# folder name
write-host "replace folder name plural"
Get-ChildItem -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldSelectorPlural, $newSelectorPlural)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelectorPlural, $newSelectorPlural) } }
write-host "replace folder name singular"
Get-ChildItem -Directory -Recurse | ForEach-Object { if ($_.Name -ne $_.Name.replace($oldSelector, $newSelector)) { Rename-Item -Path $_.PSPath -NewName $_.Name.replace($oldSelector, $newSelector) } }

Write-Host "Finish"
pause
