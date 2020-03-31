using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.ProjectCreatorWizard.UI
{
    public class CompanyAndDesignOptionViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor for <see cref="CompanyAndDesignOptionViewModel" />
        /// </summary>
        public CompanyAndDesignOptionViewModel()
        {

        }

        /// <summary>
        /// Compagny Name
        /// </summary>
        public string CompanyName { get; set; }
        public string DivisionName { get; set; }

        private bool _useRemoteDesign;
        /// <summary> 
        /// Use a common design, if <c>true</c> user can specify the address of ressources, if <c>false</c> the site use local content and script folder.
        /// </summary>
        public bool UseRemoteDesign
        {
            get { return _useRemoteDesign; }
            set { _useRemoteDesign = value; OnNotifyPropertyChanged("UseRemoteDesign"); }
        }

        /// <summary>
        /// Address of ressources
        /// </summary>
        public string RemoteDesignAddress { get; set; }

        private bool _useSupportMail;
        /// <summary> 
        /// Use a common design, if <c>true</c> user can specify the address of ressources, if <c>false</c> the site use local content and script folder.
        /// </summary>
        public bool UseSupportMail
        {
            get { return _useSupportMail; }
            set { _useSupportMail = value; OnNotifyPropertyChanged("UseSupportMail"); }
        }

        /// <summary>
        /// Mail Address of support
        /// </summary>
        public string SupportMail { get; set; }

        // simple INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
