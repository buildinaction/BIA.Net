using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System.IO;
using ZZProjectInstallerWizards.UI;

namespace ZZProjectInstallerWizards
{
    public class ChildWizard : IWizard
    {

        // Add global replacement parameters     
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            string projectName = string.Empty;
            string combineName = RootWizard.GlobalDictionary.TryGetValue(RootWizard.safeProjectNameConst, out combineName) ? combineName : string.Empty;
            string companyName = RootWizard.GlobalDictionary.TryGetValue(RootWizard.companyNameConst, out companyName) ? companyName : string.Empty;
            string divisionName = RootWizard.GlobalDictionary.TryGetValue(RootWizard.divisionNameConst, out divisionName) ? divisionName : string.Empty;
            string supportMail = RootWizard.GlobalDictionary.TryGetValue(RootWizard.supportMailConst, out supportMail) ? supportMail : string.Empty;
            string solutionName = RootWizard.GlobalDictionary.TryGetValue(RootWizard.safeRootProjectNameConst, out solutionName) ? solutionName : string.Empty;
            string staticAddress = RootWizard.GlobalDictionary.TryGetValue(RootWizard.staticAddressConst, out staticAddress) ? staticAddress : string.Empty;
            string packageResources = RootWizard.GlobalDictionary.TryGetValue(RootWizard.packageResourcesConst, out packageResources) ? packageResources : string.Empty;

            projectName = replacementsDictionary[RootWizard.safeProjectNameConst];
            if (projectName.Contains("."))
            {
                string[] splittedName = projectName.Split('.');
                projectName = splittedName.Length > 2 ? string.Join(".", combineName, splittedName[splittedName.Length - 1]) : combineName;
            }

            replacementsDictionary[RootWizard.safeRootProjectNameConst] = solutionName;
            replacementsDictionary[RootWizard.safeProjectNameConst] = projectName;
            replacementsDictionary[RootWizard.projectNameConst] = projectName;
            replacementsDictionary[RootWizard.companyNameConst] = companyName;
            replacementsDictionary[RootWizard.divisionNameConst] = divisionName;
            replacementsDictionary[RootWizard.supportMailConst] = supportMail;
            replacementsDictionary[RootWizard.staticAddressConst] = staticAddress;
            replacementsDictionary[RootWizard.packageResourcesConst] = packageResources;
        }


        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void RunFinished()
        {
        }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {

        }

        public void ProjectFinishedGenerating(Project project)
        {

        }
    }
}