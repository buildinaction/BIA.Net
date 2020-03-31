using System.Collections.Generic;
using BIA.Net.Authentication.Business.Helpers;
using BIA.Net.Authentication.Business.Synchronize;
using BIA.Net.Common.Helpers;
using Hangfire;
using ZZCompanyNameZZ.ZZProjectNameZZ.Business.DTO;
using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Helpers;
using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Services.Authentication;
using ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService.Job.Shared;

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WindowsService.Job
{
    public class UpdateUsersFromAd : BaseJob
    {
        private IServiceSynchronizeUser serviceUserDB;

        public UpdateUsersFromAd()
        {
            serviceUserDB = BIAUnity.Resolve<ServiceSynchronizeUser>();
        }

        [Queue("zzprojectnamezz")]
        [AutomaticRetry(Attempts = 100)]
        public void Run()
        {
            List<string> userDeleted = new List<string>();
            if (ADHelper.GetADGroupsForRole("User") != null)
            {
                userDeleted = serviceUserDB.SynchronizeUsers(ADHelper.GetADGroupsForRole("User"));
            }

            //foreach (string userName in userDeleted)
            //{
            //    BIAAuthorizationFilterMVC<UserInfo, UserDTO>.RefreshUserRoles(userName);
            //}
        }
    }
}