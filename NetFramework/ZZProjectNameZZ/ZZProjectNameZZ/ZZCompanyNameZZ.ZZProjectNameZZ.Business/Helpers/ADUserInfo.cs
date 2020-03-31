namespace ZZCompanyNameZZ.ZZProjectNameZZ.Business.Helpers
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Authentication.Business.Synchronize;
    using Business.DTO;

    /// <summary>
    /// Class used to synchronize AD with Database (table User)
    /// </summary>
    public class ADUserInfo : ALinkedUserInfo<UserDTO>
    {
        /// <summary>
        /// Set custom complexe properties from AD.
        /// </summary>
        /// <param name="userProperties">Properties to set by code</param>
        public override void CustomCodeLinkedProperties(UserDTO userProperties)
        {
            userProperties.IsEmployee = true;
            string title = ADHelper.GetProperty(UserPrincipal, "title", 50);
            if (!string.IsNullOrEmpty(title))
            {
                if (title.IndexOf(':') > 0)
                {
                    string[] extInfo = title.Split(':');
                    if (extInfo[0] == "EXT" && extInfo.Length == 2)
                    {
                        userProperties.IsEmployee = false;
                        userProperties.IsExternal = true;
                        userProperties.ExternalCompany = extInfo[1];
                    }
                }
            }

            string fullDepartment = ADHelper.GetProperty(UserPrincipal, "department", 100, "Dummy");
            string department = fullDepartment;
            string subDepartment = string.Empty;
            if (fullDepartment.IndexOf('-') > 0)
            {
                department = fullDepartment.Substring(0, fullDepartment.IndexOf('-') - 1);
                if (fullDepartment.Length > fullDepartment.IndexOf('-') + 2)
                {
                    subDepartment = fullDepartment.Substring(fullDepartment.IndexOf('-') + 3);
                }
            }

            userProperties.Department = !string.IsNullOrWhiteSpace(department) ? department : "Dummy";
            userProperties.SubDepartment = subDepartment;
        }
    }
}