// <copyright file="CompanyAndDesignOptionViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace BIA.ProjectCreatorWizard.UI
{
    using System.ComponentModel;

    /// <summary>
    /// CompanyAndDesignOption ViewModel.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class CompanyAndDesignOptionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The use remote design.
        /// </summary>
        private bool useRemoteDesign;

        /// <summary>
        /// The use support mail.
        /// </summary>
        private bool useSupportMail;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyAndDesignOptionViewModel"/> class.
        /// </summary>
        public CompanyAndDesignOptionViewModel()
        {
        }

        /// <summary>
        /// Occurs when a property value changes. simple INotifyPropertyChanged implementation.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the division.
        /// </summary>
        /// <value>
        /// The name of the division.
        /// </value>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use a common design, if <c>true</c> user can specify the address of ressources, if <c>false</c> the site use local content and script folder.
        /// </summary>
        public bool UseRemoteDesign
        {
            get
            {
                return this.useRemoteDesign;
            }

            set
            {
                this.useRemoteDesign = value;
                this.OnNotifyPropertyChanged("UseRemoteDesign");
            }
        }

        /// <summary>
        /// Gets or sets address of ressources.
        /// </summary>
        public string RemoteDesignAddress { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether use a common design, if <c>true</c> user can specify the address of ressources, if <c>false</c> the site use local content and script folder.
        /// </summary>
        public bool UseSupportMail
        {
            get
            {
                return this.useSupportMail;
            }

            set
            {
                this.useSupportMail = value;
                this.OnNotifyPropertyChanged("UseSupportMail");
            }
        }

        /// <summary>
        /// Gets or sets Mail Address of support.
        /// </summary>
        public string SupportMail { get; set; }

        private void OnNotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
