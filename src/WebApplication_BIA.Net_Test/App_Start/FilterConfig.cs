using System.Web;
using System.Web.Mvc;

namespace WebApplication_BIA.Net_Test
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
