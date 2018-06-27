using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIA.Net.Common.Helpers
{
    public static class BIAContentCreator
    {
        public static Dictionary<Type, Func<object>> ContentsCreator = new Dictionary<Type, Func<object>>();
    }

    public class BIAContainer<Contents>
    {
        public Contents contents;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoContainer"/> class.
        /// </summary>
        public BIAContainer()
        {
            contents = (Contents)BIAContentCreator.ContentsCreator[typeof(Contents)]();
        }
    }
}
