namespace BIA.Net.Authentication.Business
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    public abstract class AServiceSynchronizeUser : IDisposable
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="userLogin">Login in first security level</param>
        /// <param name="appliLogin">Login id for local DB</param>
        /// <param name="userRoles">Roles of the user</param>
        /// <returns>The user properties</returns>
        public static IUserDB GetUserProperties(string userLogin, string appliLogin, List<string> userRoles)
        {
            IUserDB userProperties = null;
            AServiceSynchronizeUser serviceUser = BIAUnity.Resolve<AServiceSynchronizeUser>();
            if (userRoles.Contains("User"))
            {
                userProperties = serviceUser.GetAspNetUserByName(appliLogin);
                if (userProperties == null)
                {
                    List<string> Options = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.Service?.Options;
                    userProperties = serviceUser.CreateUserFromAD(userLogin, Options.Contains("autoCreateUserInDatabase"));
                }
                else
                {
                    if ((userProperties?.UserAdInDB != null) && (!userProperties.UserAdInDB.DAIEnable))
                    {
                        userProperties.UserAdInDB.DAIEnable = serviceUser.ResetDAIEnable(userProperties.UserAdInDB, true).DAIEnable;
                    }
                }
            }
            else if (userRoles.Contains("Generic"))
            {
                userProperties = serviceUser.CreateUserGenericFromAD(userLogin);
            }
            else
            {
                // Technical user or DisableUserGroupCheck return an user build with AD Info
                userProperties = serviceUser.CreateUserFromAD(userLogin, false);
            }

            return userProperties;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.Exception">Please overide Dispose</exception>
        public virtual void Dispose()
        {
            throw new Exception("Please overide Dispose");
        }

        /// <summary>
        /// Gets the name of the ASP net user by.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide GetAspNetUserByName</exception>
        public virtual IUserDB GetAspNetUserByName(string userName)
        {
            throw new Exception("Please overide GetAspNetUserByName");
        }

        /// <summary>
        /// Resets the dai enable.
        /// </summary>
        /// <param name="aspNetUser">The ASP net user.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide ResetDAIEnable</exception>
        public virtual IUserADinDB ResetDAIEnable(IUserADinDB aspNetUser, bool value)
        {
            throw new Exception("Please overide ResetDAIEnable");
        }

        /// <summary>
        /// Creates the user from ad.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="insertInDB">if true insert in table ASPNetUser.</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide CreateUserFromAD</exception>
        public virtual IUserDB CreateUserFromAD(string userName, bool insertInDB)
        {
            throw new Exception("Please overide CreateUserFromAD");
        }

        /// <summary>
        /// Creates the user from ad.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide CreateUserFromAD</exception>
        public virtual IUserDB CreateUserGenericFromAD(string userName)
        {
            throw new Exception("Please overide CreateUserGenericFromAD");
        }

        /// <summary>
        /// Synchronizes all user.
        /// </summary>
        /// <param name="adGroupsAsApplicationUsers">List of ad groups</param>
        /// <typeparam name="TUserADinDB">The type of the user DB table DTO.</typeparam>
        /// <returns>List of user deleted</returns>
        public List<string> SynchronizeUsers<TUserInfo, TUserDB, TUserADinDB>(List<string> adGroupsAsApplicationUsers)
               where TUserADinDB : IUserADinDB, new()
               where TUserInfo : AUserInfo<TUserDB>, new()
               where TUserDB : IUserDB, new()
        {
            List<string> listUserInGroup = new List<string>();
            List<IUserADinDB> listUserName = GetAllUsersInDB();
            foreach (string groupName in adGroupsAsApplicationUsers)
            {
                List<UserPrincipal> listUsers = ADHelper.GetAllUsersInGroup(groupName);

                foreach (UserPrincipal user in listUsers)
                {
                    string userName = ADHelper.GetUserName(user);
                    listUserInGroup.Add(userName);
                    IUserADinDB findedUser = listUserName.Where(a => a.Login == userName).FirstOrDefault();

                    if (findedUser == null)
                    {
                        TUserInfo userInfo = new TUserInfo();
                        userInfo.Login = userName;
                        // Create the missing user
                        userInfo.Properties = PropertiesHelper.PrepareProperties<TUserInfo, TUserDB>(userInfo);

                        IUserADinDB adUserCreated = Insert(userInfo.Properties.UserAdInDB);
                        listUserName.Add(new TUserADinDB { Login = userName, DAIEnable = true });
                    }
                    else if (findedUser.DAIEnable == false)
                    {
                        findedUser.DAIEnable = true;
                        IUserADinDB updatedAspNetUser = ResetDAIEnable(findedUser, true);
                    }
                }
            }

            List<string> usersDeleted = new List<string>();

            // check users to unactive
            foreach (TUserADinDB aspNetUser in listUserName)
            {
                if (!listUserInGroup.Contains(aspNetUser.Login) && aspNetUser.DAIEnable == true)
                {
                    usersDeleted.Add(aspNetUser.Login);
                    aspNetUser.DAIEnable = false;
                    IUserADinDB updatedAspNetUser = ResetDAIEnable(aspNetUser, false);
                }
            }

            return usersDeleted;
        }

        /// <summary>
        /// Inserts the specified ASP user.
        /// </summary>
        /// <param name="aspUser">The ASP user.</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide Insert</exception>
        protected virtual IUserADinDB Insert(IUserADinDB aspUser)
        {
            throw new Exception("Please overide Insert");
        }

        /// <summary>
        /// Gets all users in database.
        /// </summary>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide GetAllUsersInDB</exception>
        protected virtual List<IUserADinDB> GetAllUsersInDB()
        {
            throw new Exception("Please overide GetAllUsersInDB");
        }
    }
}