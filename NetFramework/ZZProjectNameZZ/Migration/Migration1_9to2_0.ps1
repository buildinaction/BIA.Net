#Warning run this script only one time. Else restart form the backup.
#Enter here the path of the repertory containning the sln to migrate
$RepProjectToMigrate = "D:\test\V2.0\ZZProjectNameZZ"

#Enter here the shortname of the project
$ProjectName = "ZZProjectNameZZ"

#Optionnal: Enter here the path of the repertory to backup the solution before migrate it
$RepBackup = "$RepProjectToMigrate`_saved"

#Optionnal: Enter here the path of the ZZProjectNameZZ extracted at label V1.9
$CurrentScriptPath = split-path -Parent $MyInvocation.MyCommand.Definition
$RepSourceZZProjectNameZZ = "$CurrentScriptPath\.."

if ((Test-Path -path $RepBackup))
{
    Rename-Item -Path "$RepBackup" -NewName "$RepBackup`_old"
}
New-Item $RepBackup -Type Directory

$exclude = @('.vs')

Get-ChildItem $RepProjectToMigrate -Recurse -Exclude $exclude | Copy-Item -Destination {Join-Path $RepBackup $_.FullName.Substring($RepProjectToMigrate.length)}  


$Utf8NoBomEncoding = New-Object System.Text.UTF8Encoding($True)
function ToRegExpFilter
{
    param([string]$str)
    return $str.Replace('\','\\').Replace('(','\(').Replace(')','\)')
}
function ReplaceInFiles
{
    param([string]$path, [string[]]$filter, [string]$old, [string]$new, [boolean] $caseSensitive = $True, [string] $excludeFolder = "")
    $oldFilter = ToRegExpFilter $old
    $CSFiles = Get-ChildItem -File -Path "$RepProjectToMigrate\$path" -Recurse -include $filter | Where-Object { Select-String "$oldFilter" $_ -Quiet }
    if ($excludeFolder -ne "")
    {
        $CSFiles = $CSFiles| 
          ? { $_.FullName -inotmatch $excludeFolder }
    }
    foreach ($file in $CSFiles)
    {
	    $filePath = $file.FullName

        if ($caseSensitive) 
        {
            $text =  [IO.File]::ReadAllText("$filePath").Replace($old, $new)
        }
        else
        {
            $text = [IO.File]::ReadAllText("$filePath") -replace "$old", "$new"
        }

	    [IO.File]::WriteAllText("$filePath", $text, $Utf8NoBomEncoding)
    }
}

function GetFromZZproject
{
    param([string]$path)
    Copy-Item -Path "$RepSourceZZProjectNameZZ\ZZProjectNameZZ\Safran.ZZProjectNameZZ$path" -Destination "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName$path"
    ReplaceInFiles "$ProjectName\Safran.$ProjectName$path" "*.cs" "ZZProjectNameZZ" "$ProjectName" 
}

function InsertAfter
{
    param([string]$path, [string] $before, [string] $toInsert)
    $beforeFilter = ToRegExpFilter $before
    (Get-Content $RepProjectToMigrate\$path) | 
    Foreach-Object {
        $_ # send the current line to output
        if ($_ -match "$beforeFilter") 
        {
            #Add Lines after the selected pattern 
            "$toInsert"
        }
    } | Set-Content $RepProjectToMigrate\$path
}
function InsertBefore
{
    param([string]$path, [string] $after, [string] $toInsert, [int] $occurs = -1000)
    $afterFilter = ToRegExpFilter $after
    (Get-Content $RepProjectToMigrate\$path) | 
    Foreach-Object {
        if ($_ -match "$afterFilter") 
        {
            if ($occurs -ge 0){ $occurs --;}
            if (($occurs -eq 0) -Or ($occurs -eq -1000) )
            {
                #Add Lines after the selected pattern 
                "$toInsert"
            }
        }
        $_ # send the current line to output
    } | Set-Content $RepProjectToMigrate\$path
}





ReplaceInFiles "$ProjectName\Safran.$ProjectName.Common\Constants.cs" "*.cs" "public const string FrameworkVersion = ""1.9.0"";" "public const string FrameworkVersion = ""2.0.0"";"
