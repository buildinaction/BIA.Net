using System.Collections.Generic;
using ZZCompanyNameZZ.ZZProjectNameZZ.Business.Helpers;
using ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.DTO;

namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Extensions
{
    public static class UserInfoExtension
    {
        public static UserInfoDTO ToDTO(this UserInfo userInfo)
        {
            UserInfoDTO userInfoDTO = new UserInfoDTO();

            if (userInfo != null)
            {
                userInfoDTO.Language = userInfo.Language;
                userInfoDTO.Login = userInfo.Login;
                userInfoDTO.Roles = userInfo.Roles;
                userInfoDTO.User = userInfo.Properties;
                userInfoDTO.UserProfile = new Dictionary<string, string>();

                if (userInfo.UserProfile != null)
                {
                    foreach (var item in userInfo.UserProfile)
                    {
                        // a dictionary is transformed into an object, client side.
                        // On the client side, the properties of an object are miniscule.
                        userInfoDTO.UserProfile.Add(item.Key.ToLower(), item.Value);
                    }
                }
            }

            return userInfoDTO;
        }
    }
}