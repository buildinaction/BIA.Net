// <copyright file="RootWizard.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.ProjectCreatorWizard
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using BIA.ProjectCreatorWizard.UI;
    using EnvDTE;
    using Microsoft.VisualStudio.TemplateWizard;

    /// <summary>
    /// Root Wizard.
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.TemplateWizard.IWizard" />
    public class RootWizard : IWizard
    {
        /// <summary>
        /// The saferootprojectname.
        /// </summary>
        public const string SAFEROOTPROJECTNAME = "$saferootprojectname$";

        /// <summary>
        /// The safeprojectname.
        /// </summary>
        public const string SAFEPROJECTNAME = "$safeprojectname$";

        /// <summary>
        /// The projectName.
        /// </summary>
        public const string PROJECTNAME = "$projectname$";

        /// <summary>
        /// The company name.
        /// </summary>
        public const string COMPANYNAME = "$companyName$";

        /// <summary>
        /// The global dictionary.
        /// </summary>
        public static readonly Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();

        /// <inheritdoc/>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {


            CompanyAndDesignOptionViewModel _viewModel = new CompanyAndDesignOptionViewModel();
            CompanyAndDesignOptionForm window = new CompanyAndDesignOptionForm(_viewModel);
            DialogResult showDialog = window.ShowDialog();

            if (showDialog != DialogResult.OK)
            {
                throw new WizardBackoutException();
            }

            string solutionName = replacementsDictionary[SAFEPROJECTNAME];
            string companyName = _viewModel.CompanyName;
            string safeProjectName = string.Join(".", companyName, solutionName);

            GlobalDictionary[SAFEPROJECTNAME] = safeProjectName;
            GlobalDictionary[SAFEROOTPROJECTNAME] = solutionName;
            GlobalDictionary[COMPANYNAME] = companyName;

            replacementsDictionary[SAFEROOTPROJECTNAME] = solutionName;
            replacementsDictionary[SAFEPROJECTNAME] = safeProjectName;
            //replacementsDictionary[PROJECTNAME] = combineName;
            replacementsDictionary[COMPANYNAME] = companyName;
        }

        /// <inheritdoc/>
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// <inheritdoc/>
        public void RunFinished()
        {
            // Method intentionally left empty.
        }

        /// <inheritdoc/>
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
            // Method intentionally left empty.
        }

        /// <inheritdoc/>
        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
            // Method intentionally left empty.
        }

        /// <inheritdoc/>
        public void ProjectFinishedGenerating(Project project)
        {
            // Method intentionally left empty.
        }
    }
}