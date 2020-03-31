using System;
using System.Globalization;
using EnvDTE;

namespace BIA.CRUDScaffolder.UI
{
    /// <summary>
    /// Wrapper for CodeType so we can use it in the UI.
    /// </summary>
    public class ModelType
    {
        public ModelType(string display)
        {
            DisplayName = display;

        }
        public ModelType(CodeType codeType)
        {
            if (codeType == null)
            {
                throw new ArgumentNullException("codeType");
            }

            CodeType = codeType;
            Namespace = new NameSpace(codeType.Namespace);
            TypeName = codeType.FullName;
            ShortTypeName = codeType.Name;
            DisplayName = (codeType.Namespace == null || String.IsNullOrWhiteSpace(codeType.Namespace.FullName))
                            ? codeType.Name
                            : String.Format(CultureInfo.InvariantCulture, "{0} ({1})", codeType.Name, codeType.Namespace.FullName);
        }

        public NameSpace Namespace { get; set; }

        public CodeType CodeType { get; set; }

        public string DisplayName { get; set; }

        public string TypeName { get; set; }

        public string ShortTypeName { get; set; }
    }
}
