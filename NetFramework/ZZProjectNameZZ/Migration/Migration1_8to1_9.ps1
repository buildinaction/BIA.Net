#Warning run this script only one time. Else restart form the backup.
#Enter here the path of the repertory containning the sln to migrate
$RepProjectToMigrate = "D:\test\V1.9\ZZProjectNameZZ"

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

#Rename project MVC
Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.MVC"
Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.csproj" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.MVC.csproj"
Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.csproj.user" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.MVC.csproj.user"

ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" "namespace Safran.$ProjectName" "namespace Safran.$ProjectName.MVC" 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" "@model Safran.ZZProjectNameZZ.ViewModel" "@model Safran.ZZProjectNameZZ.MVC.ViewModel" 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\App_Start\UnityMvcActivator.cs" "*.cs" "Safran.$ProjectName.UnityMvcActivator" "Safran.$ProjectName.MVC.UnityMvcActivator"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\Controllers" "*.cs" "Safran.$ProjectName.Controllers" "Safran.$ProjectName.MVC.Controllers"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\Global.asax" "*" "Safran.$ProjectName.MvcApplication" "Safran.$ProjectName.MVC.MvcApplication"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.MVC.csproj" "*" "<DocumentationFile>bin\Safran.$ProjectName.XML</DocumentationFile>" "<DocumentationFile>bin\Safran.$ProjectName.MVC.xml</DocumentationFile>"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.MVC.csproj" "*" "<RootNamespace>Safran.$ProjectName</RootNamespace>" "<RootNamespace>Safran.$ProjectName.MVC</RootNamespace>"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.MVC.csproj" "*" "<AssemblyName>Safran.$ProjectName</AssemblyName>" "<AssemblyName>Safran.$ProjectName.MVC</AssemblyName>"
   
    
#Rule set in common
ReplaceInFiles "$ProjectName\" "*.csproj" "<CodeAnalysisRuleSet>Safran.ruleset</CodeAnalysisRuleSet>" "<CodeAnalysisRuleSet>..\Safran.$ProjectName.Common\Safran.ruleset</CodeAnalysisRuleSet>"
ReplaceInFiles "$ProjectName\" "*.csproj" "<CodeAnalysisRuleSet>..\Safran.$ProjectName\Safran.ruleset</CodeAnalysisRuleSet>" "<CodeAnalysisRuleSet>..\Safran.$ProjectName.Common\Safran.ruleset</CodeAnalysisRuleSet>"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Common\Safran.$ProjectName.Common.csproj" "*" "<CodeAnalysisRuleSet>..\Safran.$ProjectName.Common\Safran.ruleset</CodeAnalysisRuleSet>" "<CodeAnalysisRuleSet>Safran.ruleset</CodeAnalysisRuleSet>"
Remove-Item "$RepProjectToMigrate\$ProjectName\*\Safran.ruleset"
ReplaceInFiles "$ProjectName\" "*.csproj" "    <Content Include=""Safran.ruleset"" />" ""
GetFromZZproject ".Common\Safran.ruleset"
InsertAfter "$ProjectName\Safran.$ProjectName.Common\Safran.$ProjectName.Common.csproj" "<AdditionalFiles Include=""stylecop.json"" />" "    <Content Include=""Safran.ruleset"" />"

#Use BIA.Net.Authentication
ReplaceInFiles "$ProjectName" "*.cs" "using Net.Authentication" "using BIA.Net.Authentication" $false
ReplaceInFiles "$ProjectName" "*.cs" "using Net.Authetication.Controllers" "using BIA.Net.Authentication.Controllers" $false
ReplaceInFiles "$ProjectName" "*.cs" "using Safran.Net.Authentication" "using BIA.Net.Authentication" $false
ReplaceInFiles "$ProjectName" "*.cs" "using BIA.Net.Authentication;" "using BIA.Net.Authentication.MVC;" $false
ReplaceInFiles "$ProjectName" "*.cs" "using BIA.Net.Authentication.Controllers;" "using BIA.Net.Authentication.MVC.Controllers;" $false
ReplaceInFiles "$ProjectName" "*.cs" "using BIA.Net.Authentication.api" "using BIA.Net.Authentication.WebAPI" $false
ReplaceInFiles "$ProjectName" "*.cs" "using Safran.Net.Authentication.Api" "using BIA.Net.Authentication.WebAPI" $false

#Change for BIA.Net.Authentication
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" "SafranAuthorizationFilterApi" "BIAAuthorizationFilterWebAPI" 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" "SafranAuthorizationFilter" "BIAAuthorizationFilterMVC" 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" "<UserInfo," "<UserInfoMVC," 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" " UserInfo " " UserInfoMVC " 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\App_Start\UnityConfig.cs" "*.cs" "PrepareUserInfo(HttpContext.Current.User, HttpContext.Current.Session)" "PrepareUserInfo()"
InsertBefore "$ProjectName\Safran.$ProjectName.MVC\Global.asax.cs" "CultureHelper.SetCurrentLangageCode();" "            UserInfoMVC userInfo = (UserInfoMVC)UserInfo.GetCurrentUserInfo();"
InsertBefore "$ProjectName\Safran.$ProjectName.MVC\Global.asax.cs" "CultureHelper.SetCurrentLangageCode();" "            CultureHelper.SetLangageCode(userInfo.Language);"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC" "*.cs" "CultureHelper.SetCurrentLangageCode();" "" 
ReplaceInFiles "$ProjectName" "*.cs" "UserADinDB" "LinkedProperties" $false
ReplaceInFiles "$ProjectName" "*.cs" "UserAdInDB" "LinkedProperties" $false
ReplaceInFiles "$ProjectName" "*.cs" "UserDB" "UserProperties"
InsertBefore "$ProjectName\Safran.$ProjectName.MVC\App_Start\FilterConfig.cs" "using System.Web.Mvc;" "    using Safran.$ProjectName.MVC.Helpers;"
InsertBefore "$ProjectName\Safran.$ProjectName.MVC\App_Start\UnityConfig.cs" "using System.Web;" "    using Safran.$ProjectName.MVC.Helpers;"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\App_Start\UnityConfig.cs" "*.cs" "    using Safran.$ProjectName.Business.Services.Authentication;" ""
ReplaceInFiles "$ProjectName\Safran.$ProjectName.MVC\App_Start\UnityConfig.cs" "*.cs" "    using Safran.$ProjectName.Helpers;" ""
InsertBefore "$ProjectName\Safran.$ProjectName.MVC\Controllers\MemberController.cs" "using System.Collections.Generic;" "    using Safran.$ProjectName.MVC.Helpers;"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Services\Authentication\ServiceSynchronizeUser.cs" "*.cs" "AServiceSynchronizeUser" "AServiceSynchronizeUser<ADUserInfo, UserDTO>"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Helpers\UnityConfigBusiness.cs" "*.cs" "List<Type> listService = AllClasses.FromAssemblies(System.Reflection.Assembly.GetAssembly(typeof(UnityConfigBusiness))).ToList();" "List<Type> listService = typeof(UnityConfigBusiness).Assembly.GetTypes().ToList();"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Helpers\UnityConfigBusiness.cs" "*.cs" "AServiceSynchronizeUser" "IServiceSynchronizeUser"


GetFromZZproject ".MVC\Helpers\UserInfoMVC.cs"
GetFromZZproject ".MVC\Helpers\VarSession.cs"
InsertBefore "$ProjectName\Safran.$ProjectName.MVC\Safran.$ProjectName.MVC.csproj" "<Compile Include=""Helpers\VarSession.cs"" />" "    <Compile Include=""Helpers\UserInfoMVC.cs"" />"

$path = "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.MVC\Web.config"
[Xml]$xml = (Get-Content $path)
$nodes = $xml.SelectNodes("//configuration/configSections")
$child_node = $xml.SelectSingleNode('//configuration/configSections/section[@name="ADRoles"]')
$nodes.RemoveChild($child_node)
$child_node = $xml.SelectSingleNode('//configuration/configSections/section[@name="BiaNet"]')
$child_node.SetAttribute(“type”,”BIA.Net.Common.Configuration.BIANetSection”);


$nodes = $xml.SelectNodes("//configuration/appSettings")
$child_node = $xml.SelectSingleNode('//configuration/appSettings/add[@key="ADDomains"]')
if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}
$child_node = $xml.SelectSingleNode('//configuration/appSettings/add[@key="ADRolesPrefixesToRemove"]')
if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}
$child_node = $xml.SelectSingleNode('//configuration/appSettings/add[@key="ADRolesFilters"]')
if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}
$child_node = $xml.SelectSingleNode('//configuration/appSettings/add[@key="ADSimuRoles"]')
if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}
$child_node = $xml.SelectSingleNode('//configuration/appSettings/add[@key="ADSimuUser"]')
if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}
$child_node = $xml.SelectSingleNode('//configuration/appSettings/add[@key="UrlRefreshProfile"]')
if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}

#Remove ADRoles manualy to report customization in Roles

#$nodes = $xml.SelectNodes("//configuration")
#$child_node = $xml.SelectSingleNode('//configuration/ADRoles')
#if ($child_node -ne $null) {$nodes.RemoveChild($child_node)}

[xml]$languageXml = @"
    <Language>
      <ApplicationLanguages>
        <add key="fr-FR" name="Français" shortName="FR" />
        <add key="es-ES" name="Español" shortName="ES" />
        <add key="en-GB" name="English GB" shortName="EN GB" />
        <add key="en-US" name="English US" shortName="EN US" />
      </ApplicationLanguages>
    </Language>
"@
$xml.configuration.BiaNet.InsertBefore($xml.ImportNode($languageXml.Language, $true),$xml.configuration.BiaNet.FirstChild)

[xml]$authenticationXml = @"
    <Authentication>
      <Parameters>
        <Values>
          <add key="ADDomains" value="eu.labinal.snecma,na.labinal.snecma" />
          <add key="ADRolesMode" value="IISGroup" />
          <!--ADRolesMode can be IISGroup, ADUserFirst, ADGroupFirst-->
          <add key="Caching" value="Session" />
        </Values>    
      </Parameters>
      <Identities>
        <!--
        <Value key="Login" value="LT28400" />
        <Value key="LocalUserID" value="LT28400" />
        -->
        <WindowsIdentity key="Login" identityField="Name" removeDomain="true" />
      </Identities>
      <Roles>
        <!--
          <Value key="User" value="User"/>
          <Value key="Admin" value="Admin"/>
          <Value key="Internal" value="Internal"/>
        -->
        <ADRole key="User" value="EU\P_LPS_DM_EU_$ProjectName`_User,NA\P_LPS_DM_NA_$ProjectName`_User" /> 
        <ADRole key="Admin" value="EU\P_LPS_DM_EU_$ProjectName`_Admin,NA\P_LPS_DM_NA_$ProjectName`_Admin" />
        <ADRole key="Service" value="EU\P_LPS_DM_EU_Proj_Service" />
        <ADRole key="Internal" value="EU\P_LPS_DM_EU_Proj_Internal" />
        <CustomCode key="CustomCodeRoles" />
      </Roles>
      <Properties>
        <!-- Properties of the user are generaly gets from database (by overriding the function UserInfo > RefreshProperties . But you can force then here
        <Value key="Language" value="EN" />
        <Value key="FirstName" value="MyFirstName" />
        <Value key="LastName" value="MyLastName" />
        -->
        <!-- If user is not in database we retrieve the infos (Country and FirstName) from AD-->
        <ADField key="Country" adfield="c" maxLenght="10" default="EN" />
        <ADField key="FirstName" adfield="givenName" maxLenght="50" default="      " />
      </Properties>
      <LinkedProperties>
        <!-- Linked properties are use when you synchronize the database from active directory or other webservice
        <Value key="Language" value="EN" />
        <Value key="FirstName" value="MyFirstName" />
        <Value key="LastName" value="MyLastName" />
        -->
        <ObjectField key="Login" source="Login" />
        <Function key="DAIDate" type="System.DateTime" property="Now" />            
        <ADField key="Country" adfield="c" maxLenght="10" default="EN" />
        <ADField key="FirstName" adfield="givenName" maxLenght="50" default="      " />
        <ADField key="LastName" adfield="sn" maxLenght="50" default="      " />
        <ADField key="Email" adfield="mail" maxLenght="256" default="" />
        <ADField key="Site" adfield="description" maxLenght="50" default="Dummy" />
        <ADField key="Company" adfield="company" maxLenght="50" default="Dummy" />
        <ADField key="Office" adfield="physicalDeliveryOfficeName" maxLenght="20" default="Dummy" />
        <ADField key="Department" adfield="department" maxLenght="50" default="Dummy" />
        <ADField key="DistinguishedName" adfield="distinguishedName" maxLenght="50" default="Dummy" />
        <ADField key="Manager" adfield="manager" maxLenght="50" default="Dummy" />
        <ADField key="DAIEnable" default="true" />
        <CustomCode key="CustomCodeProperties" />
      </LinkedProperties>
      <Language>
        <Mapping key="Properties.Country">
          <add key="FR" value="fr-FR" />
          <add key="MA" value="fr-FR" />
          <add key="ES" value="es-ES" />
          <add key="MX" value="es-ES" />
          <add key="GB" value="en-GB" />
          <add key="US" value="en-US" />
        </Mapping>
        <CustomCode key="CustomCodeLanguage" />
        <Value key="default" value="en-US" />
      </Language>
      <UserProfile>
        <!--<Value key="Theme" value="Light" />-->
        <WebService key="DMIndexProfile" URL="`$(UrlDMIndex)/UserProfile/GetUserProfile">
            <Parameter key="login" source="Login" />    
        </WebService>
      </UserProfile>
    </Authentication>
"@ 
$xml.configuration.BiaNet.InsertBefore($xml.ImportNode($authenticationXml.Authentication, $true),$xml.configuration.BiaNet.Dialog)

$xml.Save($path)


#Rename AspNetUser in User and Language in Country
ReplaceInFiles "$ProjectName" "*.cs" "GetAspNetUserByName" "GetUserPropertiesByName"
ReplaceInFiles "$ProjectName" ('*') "AspNetUsers" "User" $True "SyncDatabase"
ReplaceInFiles "$ProjectName" ('*') "AspNetUser" "User" $True "SyncDatabase"
ReplaceInFiles "$ProjectName" "*.cs" "aspNetUser" "user" $false "SyncDatabase"
#GetFromZZproject ".SyncDatabase\SyncDatabaseV1.9.refactorlog"
Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Business\DTO\AspNetUsersDTO.cs" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Business\DTO\UserDTO.cs"
Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Business\Services\ServiceAspNetUsers.cs" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Business\Services\ServiceUser.cs"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\DTO\UserDTO.cs" "*.cs" "Language" "Country"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\DTO\MemberDTO.cs" "*.cs" "Language" "Country"
InsertBefore "$ProjectName\Safran.$ProjectName.Business\DTO\UserDTO.cs" "////ECC/ END CUSTOM CODE SECTION" "        public bool IsValid { get { return DAIEnable; } set { DAIEnable = value; } }" 1
InsertBefore "$ProjectName\Safran.$ProjectName.Business\DTO\UserDTO.cs" "////ECC/ END CUSTOM CODE SECTION" "" 1
Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Model\AspNetUsers.cs" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Model\User.cs"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Model\User.cs" "*.cs" "Language" "Country"
#Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.SyncDatabase\dbo\Tables\AspNetUsers.sql" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.SyncDatabase\dbo\Tables\User.sql"
#ReplaceInFiles "$ProjectName\Safran.$ProjectName.SyncDatabase\dbo\Tables\User.sql" "*.sql" "Language" "Country"
#InsertBefore "$ProjectName\Safran.$ProjectName.SyncDatabase\Safran.$ProjectName.SyncDatabase.sqlproj" "</Project>" "  <ItemGroup>"
#InsertBefore "$ProjectName\Safran.$ProjectName.SyncDatabase\Safran.$ProjectName.SyncDatabase.sqlproj" "</Project>" "    <RefactorLog Include=""SyncDatabaseV1.9.refactorlog"" />" 
#InsertBefore "$ProjectName\Safran.$ProjectName.SyncDatabase\Safran.$ProjectName.SyncDatabase.sqlproj" "</Project>" "  </ItemGroup>" 
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Model\ProjectDB.edmx" "*" "Language" "Country"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Model\ProjectDB.edmx.sql" "*" "Language" "Country"


Rename-Item -Path "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Business\Helpers\UserInfo.cs" -NewName "$RepProjectToMigrate\$ProjectName\Safran.$ProjectName.Business\Helpers\UserInfo_toRemove.cs"
GetFromZZproject ".Business\Helpers\UserInfo.cs"
GetFromZZproject ".Business\Helpers\ADUserInfo.cs"
InsertAfter "$ProjectName\Safran.$ProjectName.Business\Safran.$ProjectName.Business.csproj" "<Compile Include=""Helpers\UserInfo.cs"" />" "    <Compile Include=""Helpers\UserInfo_toRemove.cs"" />"
InsertAfter "$ProjectName\Safran.$ProjectName.Business\Safran.$ProjectName.Business.csproj" "<Compile Include=""DTO\SiteAdminsDTO.cs"" />" "    <Compile Include=""Helpers\ADUserInfo.cs"" />"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Services\Authentication\ServiceSynchronizeUser.cs" "*.cs" "ResetDAIEnable" "SetUserValidity"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Services\ServiceUser.cs" "*.cs" "ResetDAIEnable" "SetUserValidity"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Services\Authentication\ServiceSynchronizeUser.cs" "*.cs" "user.DAIEnable = value;" "((UserDTO)user).DAIEnable = value;"
ReplaceInFiles "$ProjectName\Safran.$ProjectName.Business\Services\Authentication\ServiceSynchronizeUser.cs" "*.cs" "return serviceUser.GetAll(AllServicesDTO.ServiceAccessMode.All).Select(a => (ILinkedProperties)a).ToList();" "return serviceUser.GetAll(AllServicesDTO.ServiceAccessMode.All).Select(a => (ILinkedProperties)a).ToList();"




ReplaceInFiles "$ProjectName\Safran.$ProjectName.Common\Constants.cs" "*.cs" "public const string FrameworkVersion = ""1.8.0"";" "public const string FrameworkVersion = ""1.9.0"";"
