using BIA.CRUDScaffolder.MetaData;
using BIA.CRUDScaffolder.UI;
using EnvDTE;
using Microsoft.AspNet.Scaffolding;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using Microsoft.AspNet.Scaffolding.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BIA.CRUDScaffolder
{
    internal static class IServiceProviderExtensions
    {
        public static TService GetService<TService>(this IServiceProvider provider) where TService : class
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            return (TService)provider.GetService(typeof(TService));
        }

        public static bool IsServiceAvailable<TService>(this IServiceProvider provider) where TService : class
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }

            return GetService<TService>(provider) != null;
        }
    }

    public enum ClassType
    {
        Controller,
        Views,
        ViewModel,
        CTO,
        SpecBuilder
    }
    public static class ClassTypeExtensions
    {
        public static string ToFriendlyString(this ClassType me)
        {
            switch (me)
            {
                case ClassType.Controller:
                    return "Controller";
                case ClassType.Views:
                    return "Views";
                case ClassType.ViewModel:
                    return "ViewModel";
                case ClassType.CTO:
                    return "CTO";
                case ClassType.SpecBuilder:
                    return "SpecBuilder";
                default:
                    return "You should define the Friendly Name in code";
            }
        }
    }


    public class Class2Generate
    {
        public string ClassName { get; }
        public ClassType Type { get; }
        public Class2Generate(string className, ClassType type)
        {
            ClassName = className;
            Type = type;
        }

        public string GetOutPath(string controllerRootName)
        {
            switch (Type) {
                case ClassType.Controller:
                    return "Controllers\\" + controllerRootName + "Controller";
                case ClassType.Views:
                    return "Views\\" + controllerRootName + "\\" + ClassName;
                case ClassType.ViewModel:
                    return "ViewModel\\" + controllerRootName + "\\" + controllerRootName + ClassName;
                case ClassType.CTO:
                    return "TO_MOVE_IN_BUSINESS\\CTO\\" + controllerRootName + ClassName;
                case ClassType.SpecBuilder:
                    return "TO_MOVE_IN_BUSINESS\\SpecBuilder\\" + controllerRootName + ClassName;

            }


            return "CodeGenerated\\" + controllerRootName + "\\" + Type.ToString() + "\\" + ClassName;
        }

        public string GetTemplate(string templateFolder)
        {
            switch (Type)
            {
                case ClassType.Controller:
                    return templateFolder + "/MVC/Controllers/Controller";
                case ClassType.Views:
                case ClassType.ViewModel:
                    return templateFolder + "/Mvc/" + Type.ToFriendlyString() + "/" + ((ClassName == "Index") ? "List" : ClassName);
                case ClassType.CTO:
                case ClassType.SpecBuilder:
                    return templateFolder + "/Business/" + Type.ToFriendlyString() + "/" +  ClassName;
            }

            return null;
        }

    }

    public class BIACodeGenerator : CodeGenerator
    {
        CustomViewModel _viewModel;

        /// <summary>
        /// Constructor for the custom code generator
        /// </summary>
        /// <param name="context">Context of the current code generation operation based on how scaffolder was invoked(such as selected project/folder) </param>
        /// <param name="information">Code generation information that is defined in the factory class.</param>
        public BIACodeGenerator(
            CodeGenerationContext context,
            CodeGeneratorInformation information)
            : base(context, information)
        {
            _viewModel = new CustomViewModel(Context);
        }


        /// <summary>
        /// Any UI to be displayed after the scaffolder has been selected from the Add Scaffold dialog.
        /// Any validation on the input for values in the UI should be completed before returning from this method.
        /// </summary>
        /// <returns></returns>
        public override bool ShowUIAndValidate()
        {
            // Bring up the selection dialog and allow user to select a model type
            SelectModelWindow window = new SelectModelWindow(_viewModel);
            bool? showDialog = window.ShowModal();
            return showDialog ?? false;
        }


        internal static class AsyncHelper
        {
            private static readonly TaskFactory _myTaskFactory = new
              TaskFactory(CancellationToken.None,
                          TaskCreationOptions.None,
                          TaskContinuationOptions.None,
                          TaskScheduler.Default);

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            {
                return AsyncHelper._myTaskFactory
                  .StartNew<Task<TResult>>(func)
                  .Unwrap<TResult>()
                  .GetAwaiter()
                  .GetResult();
            }

            public static void RunSync(Func<System.Threading.Tasks.Task> func)
            {
                AsyncHelper._myTaskFactory
                  .StartNew<System.Threading.Tasks.Task>(func)
                  .Unwrap()
                  .GetAwaiter()
                  .GetResult();
            }
        }

        /// <summary>
        /// This method is executed after the ShowUIAndValidate method, and this is where the actual code generation should occur.
        /// In this example, we are generating a new file from t4 template based on the ModelType selected in our UI.
        /// </summary>
        public override void GenerateCode()
        {
            // Get the selected code type
            /*var selectionRelativePath = GetSelectionRelativePath();*/
            string controllerName = _viewModel.ControllerName;
            string controllerRootName = controllerName.Replace("Controller", "");

            //ModelMetadata modelMetadata = new ModelMetadata();

            // Get the Entity Framework Meta Data
            ModelMetadata modelMetadata = null;
            Dictionary<string, string> dictFormat = new Dictionary<string, string>();
            //ModelMetadata modelMetadata2 = null;

            if (_viewModel.SelectedDBContext != null && !string.IsNullOrEmpty(_viewModel.SelectedDBContext.TypeName))
            {
                IEntityFrameworkService efService = Context.ServiceProvider.GetService<IEntityFrameworkService>();
                modelMetadata = efService.AddRequiredEntity(Context, _viewModel.SelectedDBContext.TypeName, _viewModel.SelectedModelType.CodeType.FullName);

                /*//TO REMOVE just for test
                CodeType code = _viewModel.SelectedModelType.CodeType;
                ICodeTypeService codeTypeService = (ICodeTypeService)Context.ServiceProvider.GetService(typeof(ICodeTypeService));
                modelMetadata2 = MetaDataBuilder.PrepareMetaDataWithoutEntity(code, codeTypeService.GetAllCodeTypes(Context.ActiveProject));
                //End TO REMOVE*/
            }
            else
            {
                CodeType code = _viewModel.SelectedModelType.CodeType;
                ICodeTypeService codeTypeService = (ICodeTypeService)Context.ServiceProvider.GetService(typeof(ICodeTypeService));

                modelMetadata = MetaDataBuilder.PrepareMetaDataWithoutEntity(code, codeTypeService.GetAllCodeTypes(Context.ActiveProject), out dictFormat);
            }

            // Setup the scaffolding item creation parameters to be passed into the T4 template.
           

            string templateFolder = _viewModel.SelectedVersion;

            List<Class2Generate> listClassToGenerate = new List<Class2Generate>()
            {
                new Class2Generate("Controller", ClassType.Controller),
            };
            switch (templateFolder)
            {
                case "V1.9":
                    if (_viewModel.GenerateViews)
                    {
                        listClassToGenerate.AddRange(new List<Class2Generate>{
                            new Class2Generate("Details", ClassType.Views),
                            new Class2Generate("Create", ClassType.Views),
                            new Class2Generate("Edit", ClassType.Views),
                            new Class2Generate("Delete", ClassType.Views),
                            new Class2Generate("Index", ClassType.Views),
                            new Class2Generate("_List", ClassType.Views),
                        });
                    }
                    break;
                case "V2.0":
                    if (_viewModel.GenerateViews)
                    {
                        listClassToGenerate.AddRange(new List<Class2Generate>{
                            new Class2Generate("Details", ClassType.Views),
                            new Class2Generate("Create", ClassType.Views),
                            new Class2Generate("Edit", ClassType.Views),
                            new Class2Generate("Delete", ClassType.Views),
                            new Class2Generate("Index", ClassType.Views),
                            new Class2Generate("_List", ClassType.Views),
                            new Class2Generate("_Form", ClassType.Views),
                        });
                        if (_viewModel.GenerateAdvancedFilter)
                        {
                            listClassToGenerate.AddRange(new List<Class2Generate>{
                            new Class2Generate("Filter", ClassType.Views),
                            new Class2Generate("FilterVM", ClassType.ViewModel),
                        });
                        }
                    }
                    break;
                case "V2.1":
                    if (_viewModel.GenerateViews)
                    {
                        listClassToGenerate.AddRange(new List<Class2Generate>{
                            new Class2Generate("Details", ClassType.Views),
                            new Class2Generate("Create", ClassType.Views),
                            new Class2Generate("Edit", ClassType.Views),
                            new Class2Generate("Delete", ClassType.Views),
                            new Class2Generate("Index", ClassType.Views),
                            new Class2Generate("_List", ClassType.Views),
                            new Class2Generate("_Form", ClassType.Views),
                        });
                        if (_viewModel.GenerateAdvancedFilter)
                        {
                            listClassToGenerate.AddRange(new List<Class2Generate>{
                            new Class2Generate("AdvancedFilter", ClassType.Views),
                            new Class2Generate("AdvancedFilterVM", ClassType.ViewModel),
                            new Class2Generate("AdvancedFilterCTO", ClassType.CTO),
                            new Class2Generate("AdvancedFilterSpecBuilder", ClassType.SpecBuilder),
                        });
                        }
                    }
                    break;
            }

            string nameWithoutDTO = _viewModel.SelectedModelType.CodeType.Name;
            if ((nameWithoutDTO.IndexOf("DTO") >0 ) && (nameWithoutDTO.IndexOf("DTO") == nameWithoutDTO.Length -3))
            {
                nameWithoutDTO = nameWithoutDTO.Substring(0, nameWithoutDTO.Length - 3);
            }

            foreach (Class2Generate classToGenerate in listClassToGenerate)
            {
                Dictionary<string, object> viewParams = PrepareParams(controllerRootName, modelMetadata, nameWithoutDTO, classToGenerate, dictFormat);
                string outpath = classToGenerate.GetOutPath(controllerRootName);
                string template = classToGenerate.GetTemplate(templateFolder);

                // Add the custom scaffolding item from T4 template.
                this.AddFileFromTemplate(Context.ActiveProject,
                    outpath,
                    template,
                    viewParams,
                    skipIfExists: false);
            }
        }

        private Dictionary<string, object> PrepareParams(string controllerRootName, ModelMetadata modelMetadata, string nameWithoutDTO, Class2Generate classToGenerate, Dictionary<string, string> dictFormat)
        {
            switch (classToGenerate.Type)
            {
                case ClassType.Controller:
                    return new Dictionary<string, object>()
                    {
                        { "ControllerName", controllerRootName + "Controller" },
                        { "ControllerRootName", controllerRootName},
                        { "Namespace",   GetDefaultNamespace()},
                        { "AreaName", "" },
                        { "ContextTypeName", "" },
                        { "ModelTypeName",  _viewModel.SelectedModelType.CodeType.Name },
                        { "ModelVariable", GetTypeVariable(_viewModel.SelectedModelType.CodeType.Name)},
                        { "ModelMetadata", modelMetadata },
                        { "EntitySetVariable", "" },
                        { "UseAsync", _viewModel.UseAsync },
                        { "GenerateListAjax", _viewModel.GenerateListAjax },
                        { "GenerateListFullAjax", _viewModel.GenerateListFullAjax },
                        { "GenerateFilter", _viewModel.GenerateAdvancedFilter },
                        { "CollapseAction", _viewModel.CollapseAction },
                        { "IsOverpostingProtectionRequired", true },
                        { "BindAttributeIncludeText",  GetBindAttributeIncludeText(modelMetadata.Properties.ToList())},
                        { "UpdateAttribute",  GetUpdateAttribute(modelMetadata.Properties.ToList())},
                        { "OverpostingWarningMessage",  "To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598." },
                        { "RequiredNamespaces", new HashSet<String>() { _viewModel.SelectedModelType.CodeType.Namespace.FullName } },
                        //{ "DictFormat", dictFormat }
                        //You can pass more parameters after they are defined in the template
                    };

            }

            return new Dictionary<string, object>(){
                        { "ViewDataTypeName", _viewModel.SelectedModelType.CodeType.FullName }
                        , { "ViewDataTypeShortName", nameWithoutDTO }
                        , { "IsPartialView" ,  classToGenerate.ClassName.Substring(0,1) == "_" }
                        , { "IsLayoutPageSelected", true }
                        , { "IsBundleConfigPresent", true }
                        , { "ReferenceScriptLibraries", true }
                        , { "ControllerRootName", controllerRootName }
                        , { "ViewName",  classToGenerate.ClassName }
                        , { "LayoutPageFile", "" }
                        , { "JQueryVersion","2.1.0"}
                        , { "ModelMetadata", modelMetadata }
                        , { "MvcVersion", new Version("5.1.2.0")}
                        , { "TranslateFields", _viewModel.TranslateFields }
                        , { "TranslateType", _viewModel.TranslateType }
                        , { "GenerateListAjax", _viewModel.GenerateListAjax }
                        , { "GenerateListFullAjax", _viewModel.GenerateListFullAjax }
                        , { "GenerateFilter", _viewModel.GenerateAdvancedFilter }
                        , { "CollapseAction", _viewModel.CollapseAction }
                        , { "IconifiedInput", _viewModel.IconifiedInput }
                        , { "ViewModelNamespace", GetViewModelNamespace() }
                        , { "BusinessNamespace", GetBusinessNamespace() }
                        //, { "DictFormat", dictFormat }
                        };
        }

        /*protected string GetSelectionRelativePath()
        {
            return Context.ActiveProjectItem == null ? String.Empty : ProjectItemUtils.GetProjectRelativePath(Context.ActiveProjectItem);
        }*/
        protected string GetDefaultNamespace()
        {
            return Context.ActiveProjectItem == null
                ? Context.ActiveProject.GetDefaultNamespace()
                : Context.ActiveProjectItem.GetDefaultNamespace();
        }
        protected string GetViewModelNamespace()
        {
            return Context.ActiveProject.GetDefaultNamespace()  + ".ViewModel";
        }
        protected string GetBusinessNamespace()
        {
            string activeProjectNameSpace = Context.ActiveProject.GetDefaultNamespace();
            if (activeProjectNameSpace.EndsWith(".MVC"))
            {
                activeProjectNameSpace = activeProjectNameSpace.Substring(0, activeProjectNameSpace.Length - 4);
            }
            return activeProjectNameSpace + ".Business";
        }

        private string GetTypeVariable(string typeName)
        {
            return typeName.Substring(0, 1).ToLower() + typeName.Substring(1, typeName.Length - 1);
        }
        private string GetBindAttributeIncludeText(List<PropertyMetadata> modelProperties)
        {
            string result = "";
            foreach (PropertyMetadata m in modelProperties)
            {
                if (!m.IsAssociation)
                {
                    result += "," + m.PropertyName;
                }
            }
            return result.Substring(1);
        }
        private string GetUpdateAttribute(List<PropertyMetadata> modelProperties)
        {
            string result = "";
            foreach (PropertyMetadata m in modelProperties)
            {
                string prop = m.PropertyName;
                if (! m.IsForeignKey && ! m.IsPrimaryKey)
                {
                    result += "," + m.PropertyName;
                }
            }
            return result.Substring(1);
        }


    }
}
