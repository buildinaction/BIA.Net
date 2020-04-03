// <copyright file="ChildWizard.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.ProjectCreatorWizard
{
    using System.Collections.Generic;
    using EnvDTE;
    using Microsoft.VisualStudio.TemplateWizard;

    /// <summary>
    /// Child Wizard.
    /// </summary>
    /// <seealso cref="Microsoft.VisualStudio.TemplateWizard.IWizard" />
    public class ChildWizard : IWizard
    {
        /// <inheritdoc/>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            string solutionName = RootWizard.GlobalDictionary.TryGetValue(RootWizard.SAFEROOTPROJECTNAME, out solutionName) ? solutionName : string.Empty;
            string companyName = RootWizard.GlobalDictionary.TryGetValue(RootWizard.COMPANYNAME, out companyName) ? companyName : string.Empty;

            replacementsDictionary[RootWizard.SAFEROOTPROJECTNAME] = solutionName;
            replacementsDictionary[RootWizard.COMPANYNAME] = companyName;
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