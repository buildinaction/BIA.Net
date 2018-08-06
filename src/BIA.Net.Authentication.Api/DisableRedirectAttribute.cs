using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Filters;

namespace BIA.Net.Authentication.Api
{
    public class DisableRedirectAttribute : AuthorizationFilterAttribute
    {
        //
        // Summary:
        //     Gets or sets the user roles that are authorized to access the controller or action
        //     method.
        //
        // Returns:
        //     The user roles that are authorized to access the controller or action method.
        public string Roles { get; set; }

        public List<string> GetRoles()
        {
            return Roles.Split(',').ToList(); ;
        }
    }
}
