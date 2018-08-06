namespace BIA.Net.Authentication.Web
{
    public static class AuthenticationConstants
    {
        /// <summary>
        /// Contain the user info
        /// </summary>
        public const string SessionUserInfo = "UserInfo";

        /// <summary>
        /// Contain the user name (for generic AD account)
        /// </summary>
        public const string SessionFineUserName = "FineUserName";

        /// <summary>
        /// Time of the last refresh of the Session variable user Info
        /// </summary>
        public const string SessionRefreshUserProfileDate = "RefreshUserProfile";

        /// <summary>
        /// Time of the last refresh of the Session variable user Info
        /// </summary>
        public const string SessionRefreshUserRolesDate = "RefreshUserRoles";

        /// <summary>
        /// Time of the last refresh of the Session variable user Info
        /// </summary>
        public const string SessionRefreshUserPropertiesDate = "RefreshUserProperties";
    }
}
