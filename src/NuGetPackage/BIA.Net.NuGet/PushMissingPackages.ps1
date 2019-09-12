param([String]$files,[String]$apiKey, [String]$source)
# cd "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet"
# .\nuget.exe sources Add -Name "SEP_Repository" -Source "https://tfsdm.eu.labinal.snecma/SAFRANEP%20Collection/_packaging/SEP_Repository/nuget/v3/index.json"
#  .\PushMissingPackages.ps1 "D:\Sources\Innovation Factory\*\*\packages\*\*.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Dialog.MVC\BIA.Net.Dialog.MVC.2.0.0.1.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Design\BIA.Net.Design.2.0.2.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.MVC\BIA.Net.MVC.2.0.1.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Model\BIA.Net.Model.2.5.0.nupkg" VSTS "SEP_Repository"

$filesList = Get-ChildItem -Path $files
$filesList | ForEach-Object{
    $packageName = $_.FullName;

    .\nuget.exe push -Source $source -ApiKey $apiKey $packageName
}