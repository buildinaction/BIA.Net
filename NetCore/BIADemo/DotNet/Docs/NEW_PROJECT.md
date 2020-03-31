# New Project
This document explains how to create a new .NET Core project based on the BIA framework.<br/>

## Prerequisite

### Knowledge to have:
* [C# 8](https://docs.microsoft.com/fr-fr/dotnet/csharp/)
* [.NET Core 3.1](https://docs.microsoft.com/fr-fr/dotnet/core/)

### Configuration for the MyCompany proxy
[Add the following environment variables :](https://www.architectryan.com/2018/08/31/how-to-change-environment-variables-on-windows-10/)  
* HTTP_PROXY: http://10.179.8.30:3128/
* HTTPS_PROXY: http://10.179.8.30:3128/
* NO_PROXY: https://tfsdm.eu.labinal.snecma

### Visual Studio 2019 & components
Install [Visual Studio 2019](https://visualstudio.microsoft.com/fr/vs/) and be sure to add the latest SDK of .NET Core from the components list.
Add those components during installation :
- [Git for Visual Studio](https://subscription.packtpub.com/book/programming/9781789530094/9/ch09lvl1sec71/installing-git-for-visual-studio-2019)
- [Development Time IIS Support](https://devblogs.microsoft.com/aspnet/development-time-iis-support-for-asp-net-core-applications/)

If Visual Studio 2019 is already install, you can add thoses component by launching the VS Installer.

### Git config
To find the path to the **.gitconfig** file, type the following command:  
`git config --list --show-origin`  

Open your **.gitconfig** file (usualy located in your user folder) and add this configuration:
```
[http "https://site1..../"]
                sslVerify = false
                proxy = ""
[http "https://azure.devops.my-company/"]
                sslVerify = false
                proxy = ""
```


## Create a new project
Install the latest version of the **[BIA Project Creator]** extension for Visual Studio 2019.  
You can now go to File > New > Project in Visual Studio.  
Select the BIA template in the type of projects and click on Next button.  
Fill the required fields (project name and location) and **check "Place solution and project in the same directory"**.  
Copy the following files in your DotNet folder : `Directory.Build.props`, `MyCompany.ruleset` and `.gitignore` from the DotNet folder in the BIADemo project.

## Setup project
Once your solution is created, you need to update some files.  
1. Create the corresponding database on your computer. 
2. Open the appsettings.json file and update the connection string to the database.
3. Launch the Package Manager Console in VS 2019 (Tools > Nuget Package Manager > Package Manager Console).
4. Be sure to have the project **MyCompany.[ProjectName].Infrastructure.Data** selected as the Default Project in the console and the project **MyCompany.[ProjectName].Presentation.Api** as the Startup Project of your solution.
5. Run the **Add-Migration** command to initialize the migrations for the database project. `Add-Migration [nameOfYourMigration]`
6. Run the **Update-Database** command to update you database schema (you can check if everything is fine in SQL Server Management Studio).
7. Update the Roles section in the bianetconfig.json file to use the correct AD groups or the Fakes roles.

## Launch project

To launch your project, you have to use **only IIS** (not compatible with IIS Express).
Your IIS pool must be in "No Managed Code".
For the user launching the pool, you have two options :
- Use your own account but your will not be able to manage Users in the associated CRUD.
- Use a Service account which have sufficient rights on AD. If you choose this option, don't forget to allow this user to access your database in SQL Server Management Studio.