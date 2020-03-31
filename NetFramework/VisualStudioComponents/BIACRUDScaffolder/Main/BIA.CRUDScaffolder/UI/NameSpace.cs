using System;
using System.Globalization;
using EnvDTE;

namespace BIA.CRUDScaffolder.UI
{
    /// <summary>
    /// Wrapper for CodeType so we can use it in the UI.
    /// </summary>
    public class NameSpace
    {
        public NameSpace(CodeNamespace codeNamespace)
        {
            if (codeNamespace == null)
            {
                return; // throw new ArgumentNullException("codeNamespace");
            }

            CodeNamespace = codeNamespace;
            NamespaceName = codeNamespace.FullName;
            ShortNamespaceName = codeNamespace.Name;
            DisplayName = NamespaceName;
        }

        public CodeNamespace CodeNamespace { get; set; }

        public string DisplayName { get; set; }

        public string NamespaceName { get; set; }

        public string ShortNamespaceName { get; set; }
    }
}
