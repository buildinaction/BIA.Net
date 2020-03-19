param([String]$files,[String]$apiKey, [String]$source)
# cd "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet"
# .\nuget.exe sources Add -Name "SEP_Repository" -Source "https://tfsdm.eu.labinal.snecma/SAFRANEP%20Collection/_packaging/SEP_Repository/nuget/v3/index.json"
#  .\PushMissingPackages.ps1 "D:\Sources\Innovation Factory\*\*\packages\*\*.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Dialog.MVC\BIA.Net.Dialog.MVC.2.0.0.1.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Design\BIA.Net.Design.2.0.2.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.MVC\BIA.Net.MVC.2.0.1.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Model\BIA.Net.Model.2.5.0.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Authentication.Web\BIA.Net.Authentication.Web.2.1.0.nupkg" VSTS "SEP_Repository"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Common\BIA.Net.Common.2.1.1.nupkg" VSTS "SEP_Repository"

# .\nuget.exe sources Disable -Name "DM_Feed" 
# .\nuget.exe sources Enable -Name "DM_Feed" 
# .\nuget.exe sources Remove -Name "DM_Feed" 
# .\nuget.exe sources Add -Name "DM_Feed" -Source "https://azure.devops.safran/SafranElectricalAndPower/_packaging/DM_Feed/nuget/v3/index.json"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Dialog.MVC\BIA.Net.Dialog.MVC.2.0.0.1.nupkg" VSTS "DM_Feed"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\*\*.nupkg" VSTS "DM_Feed"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Business\BIA.Net.Business.1.9.1.nupkg" VSTS "DM_Feed"
#  .\PushMissingPackages.ps1 "D:\Sources\GitHub\BIA.Net\src\NuGetPackage\BIA.Net.NuGet\BIA.Net.Business\BIA.Net.Business.2.1.1.nupkg" VSTS "DM_Feed"
#  .\PushMissingPackages.ps1 "D:\Sources\azure.devops.safran\SafranEP\Digital Manuacturing\ZZProjectNameZZ\NetFramework\packages\*\*.nupkg" VSTS "DM_Feed"
#  .\PushMissingPackages.ps1 "D:\Sources\Azure.DevOps.Safran\Digital Manufacturing\ZZProjectNameZZ\NetFramework\packages\*\*.nupkg" VSTS "DM_Feed"
#  .\PushMissingPackages.ps1 "D:\Sources\Azure.DevOps.Safran\Digital Manufacturing\DMIndex\NetFramework\packages\*\*.nupkg" VSTS "DM_Feed"
$filesList = Get-ChildItem -Path $files
$filesList | ForEach-Object{
    $packageName = $_.FullName;

    .\nuget.exe push -Source $source -ApiKey $apiKey $packageName
}