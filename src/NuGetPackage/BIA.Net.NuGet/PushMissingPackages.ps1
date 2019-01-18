param([String]$files,[String]$apiKey, [String]$source)

# .\nuget.exe sources Add -Name "SEP_Repository" -Source "https://tfsdm.eu.labinal.snecma/SAFRANEP%20Collection/_packaging/SEP_Repository/nuget/v3/index.json"
#  .\PushMissingPackages.ps1 "D:\Sources\Innovation Factory\*\*\packages\*\*.nupkg" VSTS "SEP_Repository"

$filesList = Get-ChildItem -Path $files
$filesList | ForEach-Object{
    $packageName = $_.FullName;

    .\nuget.exe push -Source $source -ApiKey $apiKey $packageName
}