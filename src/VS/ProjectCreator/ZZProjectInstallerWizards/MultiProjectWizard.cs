using System;
using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using ZZProjectInstallerWizards.UI;
using System.Windows.Forms;
using System.Globalization;

namespace ZZProjectInstallerWizards
{

    public class RootWizard : IWizard
    {
        // Use to communicate $saferootprojectname$ to ChildWizard     
        public static Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();

        public const string safeRootProjectNameConst = "$saferootprojectname$";

        public const string safeProjectNameConst = "$safeprojectname$";
        public const string projectNameConst = "$projectname$";
        public const string staticAddressConst = "$staticAddress$";
        public const string companyNameConst = "$companyName$";
        public const string divisionNameConst = "$divisionName$";
        public const string supportMailConst = "$supportMail$";
        public const string packageResourcesConst = "$packageResources$";

        public const string localStaticAddressConst = "http://localhost/static";

        CompanyAndDesignOptionViewModel _viewModel = new CompanyAndDesignOptionViewModel();

        // Add global replacement parameters     
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {

            // SettingsPageGrid mySettings = automationObject.package as SettingsPageGrid;
           // System.Windows.Forms.MessageBox.Show(string.Format(CultureInfo.CurrentCulture, "OptionInteger: {0}", mySettings.CompanyName));



            string companyName = string.Empty;
            string divisionName = string.Empty;
            string solutionName = string.Empty;
            string combineName = string.Empty;
            string staticAddress = string.Empty;
            string packageResources = string.Empty;
            bool useSupportMail = false;
            string supportMail = string.Empty;

            CompanyAndDesignOptionForm window = new CompanyAndDesignOptionForm(_viewModel);
            DialogResult showDialog = window.ShowDialog();

            if (showDialog != DialogResult.OK)
            {
                throw new WizardBackoutException();
            }

            solutionName = replacementsDictionary[safeProjectNameConst];
            companyName = _viewModel.CompanyName;
            divisionName = _viewModel.DivisionName;
            combineName = string.Join(".", companyName, solutionName);
            staticAddress = _viewModel.UseRemoteDesign ? _viewModel.RemoteDesignAddress : localStaticAddressConst;
            packageResources = _viewModel.UseRemoteDesign ? string.Empty : "<package id=\"BIA.Net.Design\" version=\"2.0.0\" targetFramework=\"net452\" />";
            useSupportMail = _viewModel.UseSupportMail;
            supportMail = _viewModel.SupportMail;


            GlobalDictionary[safeProjectNameConst] = combineName;
            GlobalDictionary[safeRootProjectNameConst] = solutionName;
            GlobalDictionary[companyNameConst] = companyName;
            GlobalDictionary[divisionNameConst] = divisionName;
            GlobalDictionary[staticAddressConst] = staticAddress;
            GlobalDictionary[supportMailConst] = supportMail;
            GlobalDictionary[packageResourcesConst] = packageResources;

            replacementsDictionary[RootWizard.safeRootProjectNameConst] = solutionName;
            replacementsDictionary[RootWizard.safeProjectNameConst] = combineName;
            replacementsDictionary[RootWizard.projectNameConst] = combineName;
            replacementsDictionary[RootWizard.companyNameConst] = companyName;
            replacementsDictionary[RootWizard.divisionNameConst] = divisionName;
            replacementsDictionary[RootWizard.staticAddressConst] = staticAddress;
            replacementsDictionary[RootWizard.supportMailConst] = supportMail;
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