using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Common.Helpers
{
    /// <summary>
    /// Memorize the content constructors
    /// </summary>
    public static class BIAUnityContentCreator
    {
        /// <summary>
        /// all constructors of the contents
        /// </summary>
        public static Dictionary<Type, Func<object>> ContentsCreator = new Dictionary<Type, Func<object>>();
    }

    /// <summary>
    /// Class to use as containner of a class when you cannot directly use the class, the constructor are referenced in BIAContentCreator
    /// </summary>
    /// <typeparam name="Content"></typeparam>
    public class BIAUnityContainer<Content>
    {
        /// <summary>
        /// Content of this container class
        /// </summary>
        public Content content;

        /// <summary>
        /// Create the 
        /// </summary>
        public BIAUnityContainer()
        {
            content = (Content)BIAUnityContentCreator.ContentsCreator[typeof(Content)]();
        }
    }
}
