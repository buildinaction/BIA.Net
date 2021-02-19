```powershell
# Adapt the path to your dev environment
$serviceFullPath = "D:\Sources\Azure.DevOps.Safran\DigitalManufacturing\BIADemo\DotNet\Safran.BIADemo.WorkerService\bin\Debug\netcoreapp3.1\Safran.BIADemo.WorkerService.exe"

# Adapt the user un pass to your environment
$secpasswd = ConvertTo-SecureString "**********" -AsPlainText -Force
$cred = New-Object System.Management.Automation.PSCredential ("EU\*****", $secpasswd)

# Adapt the appli name to your application
$appli = "BIADemo"
$sCompanyName = "Safran"
$sTeamName = "DM"

$serviceName = $sCompanyName + "_" + $sTeamName + "_" + $appli
$ServiceDisplayName = $sCompanyName + " - " + $sTeamName + " - " + $appli + " Service"
$ServiceDescription = $sCompanyName + " - " + $sTeamName + " service for " + $appli + " application."
New-Service -Name $serviceName  -BinaryPathName $serviceFullPath -DisplayName $ServiceDisplayName -Credential $cred -Description $ServiceDescription
```