using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace BIA.Net.Common.Helpers
{

    public static class ListHelper
    {
        public static object SortList<T>(string sort, ref List<T> result,  object viewstate)
        {
            if (viewstate == null || viewstate.ToString() == "Desc")
            {
                result = result.AsQueryable().OrderBy("r => r." +sort).ToList();
                return "Asc";
                }
            else
            {
                result = result.AsQueryable().OrderBy(sort + " descending").ToList();
                return "Desc";
            }
        }
    }
}
