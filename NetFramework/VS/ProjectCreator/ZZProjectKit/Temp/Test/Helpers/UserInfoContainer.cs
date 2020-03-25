namespace $safeprojectname$.Helpers
{
    using BIA.Net.Common.Helpers;
    using BIA.Net.Authentication.Business.Helpers;
    using Business.DTO;
    using Business.Helpers;
    using System.Collections.Generic;

    /// <summary>
    /// UserInfo Container
    /// </summary>
    public static class UserInfoContainer
    {
        static UserInfo userInfo;

        public static void SetUserInfo(int userId, string userLogin, List<string> roles)
        {
            userInfo = new UserInfo
            {
                Properties = new UserDTO() {
                    Id = userId,
                    Login = userLogin
                },
                Roles = roles
            };
        }
        public static UserInfo GetUserInfo()
        {
            return userInfo;
        }
    }
}