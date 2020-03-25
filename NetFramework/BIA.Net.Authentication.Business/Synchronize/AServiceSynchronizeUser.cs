namespace BIA.Net.Authentication.Business.Synchronize
{
    using BIA.Net.Authentication.Business.Helpers;
    using BIA.Net.Common;
    using BIA.Net.Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    public abstract class AServiceSynchronizeUser<TLinkedUserInfo, TLinkedProperties> : IServiceSynchronizeUser, IDisposable
               where TLinkedProperties : ILinkedProperties, new()
               where TLinkedUserInfo : ALinkedUserInfo<TLinkedProperties>, new()
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.Exception">Please overide Dispose</exception>
        public virtual void Dispose()
        {
            throw new Exception("Please overide Dispose");
        }

        /// <summary>
        /// Resets the dai enable.
        /// </summary>
        /// <param name="userProperties">The ASP net user.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide SetUserValidity</exception>
        public virtual ILinkedProperties SetUserValidity(ILinkedProperties userProperties, bool value)
        {
            throw new Exception("Please overide SetUserValidity");
        }

        /// <summary>
        /// Synchronizes all user.
        /// </summary>
        /// <param name="adGroupsAsApplicationUsers">List of ad groups</param>
        /// <typeparam name="TLinkedUserProperties">The type of the user DB table DTO.</typeparam>
        /// <returns>List of user deleted</returns>
        public virtual List<string> SynchronizeUsers(List<ADGroup> adGroupsAsApplicationUsers)
        {
            List<string> listUserInGroup = new List<string>();
            List<ILinkedProperties> listUserName = GetAllUsersInDB();
            foreach (ADGroup group in adGroupsAsApplicationUsers)
            {
                List<UserPrincipal> listUsers = group.GetAllUsersInGroup();

                foreach (UserPrincipal user in listUsers)
                {
                    string userName = ADHelper.GetUserName(user);
                    listUserInGroup.Add(userName);
                    ILinkedProperties findedUser = listUserName.Where(a => a.Login == userName).FirstOrDefault();

                    if (findedUser == null)
                    {
                        TLinkedUserInfo userInfo = new TLinkedUserInfo();
                        userInfo.Login = userName ;
                        userInfo.Roles = new List<string> { group.Role };
                        // Create the missing user

                        if (IsUserToSyncFromAd(userInfo))
                        {
                            ILinkedProperties adUserCreated = Insert(userInfo.LinkedProperties);
                            listUserName.Add(new TLinkedProperties { Login = userName, IsValid = true });
                        }
                    }
                    else if (findedUser.IsValid == false)
                    {
                        findedUser.IsValid = true;
                        ILinkedProperties updatedUserProperties = SetUserValidity(findedUser, true);
                    }
                }
            }

            List<string> usersDeleted = new List<string>();

            // check users to unactive
            foreach (TLinkedProperties userProperties in listUserName)
            {
                if (!listUserInGroup.Contains(userProperties.Login) && userProperties.IsValid == true)
                {
                    usersDeleted.Add(userProperties.Login);
                    userProperties.IsValid = false;
                    ILinkedProperties updatedUserProperties = SetUserValidity(userProperties, false);
                }
            }

            return usersDeleted;
        }

        /// <summary>
        /// Check if the user is to insert during sync from AD.
        /// </summary>
        /// <param name="userInfo">The ASP net user.</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please override SetUserValidity</exception>
        protected virtual bool IsUserToSyncFromAd(TLinkedUserInfo userInfo)
        {
            if (!userInfo.IsInAd()) return false;
            return true;
        }

        /// <summary>
        /// Inserts the specified ASP user.
        /// </summary>
        /// <param name="aspUser">The ASP user.</param>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please override Insert</exception>
        protected virtual ILinkedProperties Insert(ILinkedProperties aspUser)
        {
            throw new Exception("Please overide Insert");
        }

        /// <summary>
        /// Gets all users in database.
        /// </summary>
        /// <returns>Nothing: Function to override</returns>
        /// <exception cref="System.Exception">Please overide GetAllUsersInDB</exception>
        protected virtual List<ILinkedProperties> GetAllUsersInDB()
        {
            throw new Exception("Please overide GetAllUsersInDB");
        }
    }
}