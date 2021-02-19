// <copyright file="Rights.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Crosscutting.Common
{
    /// <summary>
    /// The list of all rights.
    /// </summary>
    public static class Rights
    {
        /// <summary>
        /// The home rights.
        /// </summary>
        public static class Home
        {
            /// <summary>
            /// The right to access the home.
            /// </summary>
            public const string Access = "Home_Access";
        }

        /// <summary>
        /// The logs rights.
        /// </summary>
        public static class Logs
        {
            /// <summary>
            /// The right to create logs.
            /// </summary>
            public const string Create = "Logs_Create";
        }

        /// <summary>
        /// The members rights.
        /// </summary>
        public static class Members
        {
            /// <summary>
            /// The right to access to the list of members.
            /// </summary>
            public const string ListAccess = "Member_List_Access";

            /// <summary>
            /// The right to create members.
            /// </summary>
            public const string Create = "Member_Create";

            /// <summary>
            /// The right to read members.
            /// </summary>
            public const string Read = "Member_Read";

            /// <summary>
            /// The right to update members.
            /// </summary>
            public const string Update = "Member_Update";

            /// <summary>
            /// The right to delete members.
            /// </summary>
            public const string Delete = "Member_Delete";

            /// <summary>
            /// The right to save members.
            /// </summary>
            public const string Save = "Member_Save";

            /// <summary>
            /// The right to set default site.
            /// </summary>
            public const string SetDefaultSite = "Member_Set_Default_Site";
        }

        /// <summary>
        /// The roles rights.
        /// </summary>
        public static class Roles
        {
            /// <summary>
            /// The right to get all roles.
            /// </summary>
            public const string List = "Roles_List";
        }

        /// <summary>
        /// The LDAP domains rights.
        /// </summary>
        public static class LdapDomains
        {
            /// <summary>
            /// The right to get all LDAP domains.
            /// </summary>
            public const string List = "LdapDomains_List";
        }

        /// <summary>
        /// The sites rights.
        /// </summary>
        public static class Sites
        {
            /// <summary>
            /// The right to access to all sites.
            /// </summary>
            public const string AccessAll = "Site_Access_All";

            /// <summary>
            /// The right to access to the list of sites.
            /// </summary>
            public const string ListAccess = "Site_List_Access";

            /// <summary>
            /// The right to create sites.
            /// </summary>
            public const string Create = "Site_Create";

            /// <summary>
            /// The right to read sites.
            /// </summary>
            public const string Read = "Site_Read";

            /// <summary>
            /// The right to update sites.
            /// </summary>
            public const string Update = "Site_Update";

            /// <summary>
            /// The right to delete sites.
            /// </summary>
            public const string Delete = "Site_Delete";

            /// <summary>
            /// The right to save sites.
            /// </summary>
            public const string Save = "Site_Save";
        }

        /// <summary>
        /// The users rights.
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// The right to access to the list of users.
            /// </summary>
            public const string ListAccess = "User_List_Access";

            /// <summary>
            /// The right to get the list of users.
            /// </summary>
            public const string List = "User_List";

            /// <summary>
            /// The right to get the list of AD users.
            /// </summary>
            public const string ListAD = "User_ListAD";

            /// <summary>
            /// The right to add users.
            /// </summary>
            public const string Add = "User_Add";

            /// <summary>
            /// The right to delete users.
            /// </summary>
            public const string Delete = "User_Delete";

            /// <summary>
            /// The right to synchronize users.
            /// </summary>
            public const string Sync = "User_Sync";
        }

        /// <summary>
        /// The views rights.
        /// </summary>
        public static class Views
        {
            /// <summary>
            /// The right to get all views of the current user.
            /// </summary>
            public const string List = "View_List";

            /// <summary>
            /// The right to add an user view.
            /// </summary>
            public const string AddUserView = "View_Add_UserView";

            /// <summary>
            /// The right to add an site view.
            /// </summary>
            public const string AddSiteView = "View_Add_SiteView";

            /// <summary>
            /// The right to update an user view.
            /// </summary>
            public const string UpdateUserView = "View_Update_UserView";

            /// <summary>
            /// The right to update an site view.
            /// </summary>
            public const string UpdateSiteView = "View_Update_SiteView";

            /// <summary>
            /// The right to delete an user view.
            /// </summary>
            public const string DeleteUserView = "View_Delete_UserView";

            /// <summary>
            /// The right to delete an site view.
            /// </summary>
            public const string DeleteSiteView = "View_Delete_SiteView";

            /// <summary>
            /// The right to set default user view.
            /// </summary>
            public const string SetDefaultUserView = "View_Set_Default_UserView";

            /// <summary>
            /// The right to set default site view.
            /// </summary>
            public const string SetDefaultSiteView = "View_Set_Default_SiteView";

            /// <summary>
            /// The right to assign view to a site.
            /// </summary>
            public const string AssignToSite = "View_Assign_To_Site";
        }
    }
}
