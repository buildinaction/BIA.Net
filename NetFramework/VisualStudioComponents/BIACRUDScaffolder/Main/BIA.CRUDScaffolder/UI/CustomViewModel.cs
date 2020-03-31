using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BIA.CRUDScaffolder.UI
{
    /// <summary>
    /// View model for code types so that it can be displayed on the UI.
    /// </summary>
    public class CustomViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The code generation context</param>
        public CustomViewModel(CodeGenerationContext context)
        {
            Versions = new string [3] { "V1.9", "V2.0", "V2.1" };
            Context = context;
            GenerateViews = true;
            TranslateType = true;
            TranslateFields = true;
            GenerateAdvancedFilter = true;

            ICodeTypeService codeTypeService = (ICodeTypeService)Context
                   .ServiceProvider.GetService(typeof(ICodeTypeService));

            _AllModelTypes = codeTypeService
            .GetAllCodeTypes(Context.ActiveProject).Where(codeType => codeType.IsValidWebProjectEntityType() && codeType.Namespace != null).ToList();
            Namespaces = _AllModelTypes
            .Where(codeType => codeType.IsValidWebProjectEntityType())
                    .GroupBy(p => p.Namespace.FullName)
                    .Select(g => g.First())
                    .Select(codeType => new NameSpace(codeType.Namespace));

            DBContexts = new List<ModelType>() { _emptyDBContext }.Concat(_AllModelTypes.Where(codeType => codeType.IsValidDbContextType())
                    .Select(codeType => new ModelType(codeType))).ToList();
        }

        List<CodeType> _AllModelTypes = null;

        /// <summary>
        /// This gets all the Model types from the active project.
        /// </summary>
        public IEnumerable<ModelType> ModelTypes
        {
            get
            {
                return _AllModelTypes.Where(modelType => modelType.IsValidWebProjectEntityType() && (selectedNamespace == null || modelType.Namespace.Name == selectedNamespace.NamespaceName)).Select(codeType => new ModelType(codeType));
            }
        }

        public IEnumerable<string> Versions
        {
            get;
            set;
        }

        public IEnumerable<NameSpace> Namespaces
        {
            get;
            set;
        }

        public IEnumerable<ModelType> DBContexts
        {
            get; set;
        }


        private ModelType selectedModelType;
        public ModelType SelectedModelType
        {
            get { return selectedModelType; }
            set
            {
                if ((selectedModelType == null && value != null) 
                    || (selectedModelType != null && value == null) 
                    || (selectedModelType != null && value != null && value.DisplayName != selectedModelType.DisplayName))
                {
                    selectedModelType = value;
                    if (selectedModelType != null)
                    {
                        string ShortName = selectedModelType.ShortTypeName;
                        if (ShortName.Length>3  && ShortName.ToLower().IndexOf("dto") == ShortName.Length - 3)
                        {
                            ShortName = ShortName.Substring(0, ShortName.Length - 3);
                        }
                        ControllerName = ShortName + "Controller";
                        OnNotifyPropertyChanged("ControllerName");
                    }
                }
            }
        }

        private string selectedVersion;
        public string SelectedVersion
        {
            get { return selectedVersion; }
            set
            {
                if ((selectedVersion == null && value != null)
                    || (selectedVersion != null && value == null)
                    || (selectedVersion != null && value != null && value != selectedVersion))
                {
                    selectedVersion = value;
                    OnNotifyPropertyChanged("Version");
                }
            }
        }

        private NameSpace selectedNamespace;
        public NameSpace SelectedNamespace {
            get { return selectedNamespace; }
            set
            {
                if ((selectedNamespace == null && value != null)
                    || (selectedNamespace != null && value == null)
                    || (selectedNamespace != null && value != null && value.DisplayName != selectedNamespace.DisplayName))
                {
                    selectedNamespace = value;
                    OnNotifyPropertyChanged("ModelTypes");
                }
            }
        }

        public static ModelType _emptyDBContext = new ModelType("---- DO NOT USE DB CONTEXT ----");

        public ModelType _selectedDBContext = _emptyDBContext;

        public ModelType SelectedDBContext { get { return _selectedDBContext; } set { _selectedDBContext = value; } }

        public string ControllerName { get; set; }

        public bool GenerateViews { get; set; }

        public bool TranslateType { get; set; }

        public bool TranslateFields { get; set; }

        public bool UseAsync { get; set; }

        public bool GenerateListAjax { get; set; }

        public bool GenerateListFullAjax { get; set; }

        public bool GenerateAdvancedFilter { get; set; }

        public bool CollapseAction { get; set; }

        public bool IconifiedInput { get; set; }

        public CodeGenerationContext Context { get; private set; }

        // simple INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
