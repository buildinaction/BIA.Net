# Package a new version of the Framework:

## Refine the BIADemo project
- In the .Net Part: put comments "// BIADemo only" at the beginning of each file which must not appear in the template
- Put behind comments "// Begin BIADemo" and "// End BIADemo" the parts of the files to make disappear in the template
- Remove all warnings.
- Change the framework version in **..\BIADemo\DotNet\Safran.BIADemo.Crosscutting.Common\Constants.cs**
- Verify project version in
  - **..\BIADemo\DotNet\Safran.BIADemo.Crosscutting.Common\Constants.cs**
  - **..\BIADemo\Angular\src\environments\environment.ts**
  - **..\BIADemo\Angular\src\environments\environment.prod.ts**
- COMMIT BIADemo

## Compile the BIA packages:
- Change the version number of all BIA.Net.Core packages to match the version to be released.
- Compile the whole solution in release
- Publish all the packages (right click on each project, publish, "Copy to NuGetPackage folder", Publish)

## Switch the BIADemo project to nuget
- Start the script **...\BIADemo\DotNet\Switch-To-Project.ps1**
- Check that the solution compiles (need to have configured a local source nuget to ...\BIADemo\BIAPackage\NuGetPackage)
- test the BIADemo project.
- DO NOT COMMIT BIADemo here (this will block the build 24H because the packages are not published on nuget.org)

## Prepare BIATemplate:
- Launch **...\BIATemplate\DotNet-BIADemo-BIATemplate.ps1**
- Launch **...\BIATemplate\Angular-BIADemo-BIATemplate.ps1** (if some files are to exclude modify the script)
- Compile the solution BIATemplate, Test and verify the absence of warning.
- DO NOT COMMIT BIATemplate here (this will block the build 24H because the packages are not published on nuget.org)

## Publish BIAPackage
- If everything is ok Publish the packages on nuget.org
- Wait the confirmation by mail of all packages
- COMMIT BIADemo and BIATemplate

## Prepare the VSExtension
- In BIATemplate project extract the tempate for all projects:
  - use Ctrl+E, Ctrl+X to dislay wizard
  - Click next
  - Uncheck the 2 combo
  - Click Finish
- Check than the path ($RepSource) is correct 
- Launch **...\BIAVSExtension\BIAProjectCreator\RefreshKit.ps1** (it refresh the files in BIAVSExtension\BIAProjectCreator\BIA.ProjectCreatorTemplateV3 from the template extrated)
- Open the solution **..\BIAVSExtension\BIAVSExtension.sln**
- If project have been added in the BIATemplate update **...\BIAVSExtension\BIAProjectCreator\BIA.ProjectCreatorTemplateV3\BIA.vstemplate**
- Change the version number in the name tag of the file **...\BIAVSExtension\BIAProjectCreator\BIA.ProjectCreatorTemplateV3\BIA.vstemplate**
- Change the version number (add a .0 to be on 4 digits) in **...\BIAVSExtension\BIAProjectCreator\BIA.ProjectCreator\source.extension.vsixmanifest**
- Check that all the Additional Files are added to the project and have the properties: Build action = Content, Include In VSIX = true- You can test the project BIAProjectCreator in debug ... when it start, create a new project and select BIA V... Template.
- Rebuild solution in release.
- In project BIAVSExtension create a new folder VX.Y.Z with (X.Y.Z = version) 
- Zip the generated file **...\BIAVSExtension\BIAVSExtension\BIA.ProjectCreator.vsix** to  **...\BIAVSExtension\BIAVSExtension\VX.Y.Z\BIA.ProjectCreator.X.Y.Z.zip**
- Zip the **...\BIATemplate\Angular** folder to  **...\BIAVSExtension\BIAVSExtension\VX.Y.Z\BIA.AngularTemplate.X.Y.Z.zip**
- COMMIT the BIAVSExtension solution.

## Deliver the version
- Place the 2 zip in https://inshare.collab.group.safran/bao/DevOps/BIAV3/Shared%20Documents/Forms/AllItems.aspx
  - AngularTemplates pour  BIA.AngularTemplate.X.Y.Z.zip
  - VSExtensions pour BIA.ProjectCreator.X.Y.Z.zip