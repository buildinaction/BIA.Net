// <copyright file="RootWizard.cs" company="BIA">
//     Copyright (c) BIA. All rights reserved.
// </copyright>

namespace BIA.ProjectCreatorWizard
{
    using System.Collections.Generic;
    using System.IO;
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
        /// The $safecompanyName$.
        /// </summary>
        public const string SAFECOMPANYNAME = "$safecompanyName$";

        /// <summary>
        /// The global dictionary.
        /// </summary>
        public static readonly Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();

        /// <summary>
        /// View model use for UI.
        /// </summary>
        private static readonly CompanyAndDesignOptionViewModel ViewModel = new CompanyAndDesignOptionViewModel();

        /// <summary>
        /// Destinaion Folder for solution.
        /// </summary>
        private DirectoryInfo destFolder;

        /// <summary>
        /// The solution name.
        /// </summary>
        private string solutionName;

        /// <inheritdoc/>
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            this.destFolder = Directory.GetParent(replacementsDictionary["$destinationdirectory$"]);

            CompanyAndDesignOptionForm window = new CompanyAndDesignOptionForm(ViewModel);
            DialogResult showDialog = window.ShowDialog();

            if (showDialog != DialogResult.OK)
            {
                throw new WizardBackoutException();
            }

            this.solutionName = replacementsDictionary[SAFEPROJECTNAME];
            string companyName = ViewModel.CompanyName;
            string combineName = string.Join(".", companyName, this.solutionName);

            GlobalDictionary[SAFEPROJECTNAME] = combineName;
            GlobalDictionary[SAFEROOTPROJECTNAME] = this.solutionName;
            GlobalDictionary[SAFECOMPANYNAME] = companyName;

            replacementsDictionary[SAFEROOTPROJECTNAME] = this.solutionName;
            replacementsDictionary[SAFEPROJECTNAME] = combineName;
        }

        /// <inheritdoc/>
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        /// <inheritdoc/>
        public void RunFinished()
        {
            this.PlaceAdditionnalFilesInSolutionFolder();
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

        /// <summary>
        /// Copy files for VSIX AdditionnalFiles folder to the root solution folder.
        /// </summary>
        /// <param name="fileName">file to copy.</param>
        private void PlaceAdditionnalFilesInSolutionFolder()
        {
            var destPath = Path.Combine(this.destFolder.FullName, this.solutionName);

            string path_codebase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            string path_dir = Path.GetDirectoryName(path_codebase).Replace("file:\\", string.Empty) + "\\AdditionalFiles\\";

            this.Copy(path_dir, destPath);
        }

        /// <summary>
        /// Copy files for VSIX AdditionnalFiles folder to the root solution folder.
        /// </summary>
        /// <param name="sourceDir">source folder.</param>
        /// <param name="targetDir">target folder.</param>
        private void Copy(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string targetFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, targetFile, true);
                this.ReplaceInFile(targetFile, "BIATemplate", this.solutionName);
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                this.Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }
        }

        private void ReplaceInFile(string filePath, string oldValue, string newValue)
        {
            string text = File.ReadAllText(filePath);
            text = text.Replace(oldValue, newValue);
            File.WriteAllText(filePath, text);
        }
    }
}