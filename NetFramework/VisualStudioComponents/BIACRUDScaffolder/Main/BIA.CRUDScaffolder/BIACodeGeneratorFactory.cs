using Microsoft.AspNet.Scaffolding;
using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BIA.CRUDScaffolder
{
    [Export(typeof(CodeGeneratorFactory))]
    public class BIACodeGeneratorFactory : CodeGeneratorFactory
    {
        /// <summary>
        ///  Information about the code generator goes here.
        /// </summary>
        private static CodeGeneratorInformation _info = new CodeGeneratorInformation(
            displayName: "BIA C.R.U.D. Scaffolder",
            description: "This is a scaffolder to generate Controllers and views using BIA.Net architecture.",
            author: "Gwenael Fillon",
            version: new Version(2, 1, 0, 0),
            id: typeof(BIACodeGenerator).Name,
            icon: ToImageSource(Resources.Icon_BIA),
            gestures: new[] { "Controller" },
            categories: new[] { Categories.Common, Categories.MvcController, Categories.Other });

        public BIACodeGeneratorFactory()
            : base(_info)
        {
        }
        /// <summary>
        /// This method creates the code generator instance.
        /// </summary>
        /// <param name="context">The context has details on current active project, project item selected, Nuget packages that are applicable and service provider.</param>
        /// <returns>Instance of CodeGenerator.</returns>
        public override ICodeGenerator CreateInstance(CodeGenerationContext context)
        {
            return new BIACodeGenerator(context, Information);
        }

        /// <summary>
        /// Provides a way to check if the custom scaffolder is valid under this context
        /// </summary>
        /// <param name="codeGenerationContext">The code generation context</param>
        /// <returns>True if valid, False otherwise</returns>
        public override bool IsSupported(CodeGenerationContext codeGenerationContext)
        {
            if (codeGenerationContext.ActiveProject.CodeModel.Language != EnvDTE.CodeModelLanguageConstants.vsCMLanguageCSharp)
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Helper method to convert Icon to Imagesource.
        /// </summary>
        /// <param name="icon">Icon</param>
        /// <returns>Imagesource</returns>
        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
