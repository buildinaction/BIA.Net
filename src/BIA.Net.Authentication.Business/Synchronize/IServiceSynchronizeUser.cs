using BIA.Net.Authentication.Business.Helpers;
namespace BIA.Net.Authentication.Business.Synchronize
{
    using System.Collections.Generic;
    public interface IServiceSynchronizeUser
    {
        List<string> SynchronizeUsers(List<ADGroup> adGroupsAsApplicationUsers);
    }
}
