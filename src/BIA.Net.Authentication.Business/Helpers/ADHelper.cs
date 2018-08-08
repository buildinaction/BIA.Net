namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Common;
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.Text.RegularExpressions;
    using static BIA.Net.Common.Configuration.AuthenticationElement.SourcesElement.UserPropertiesElement;
    using static BIA.Net.Common.Configuration.AuthenticationElement.SourcesElement.UserPropertiesElement.ADFieldsCollection;
    using static BIA.Net.Common.Configuration.CommonElement;

    /// <summary>
    /// AuthHelper
    /// </summary>
    public static class ADHelper
    {

        /// <summary>
        /// Set Properties from AD conformly to the parameter in web.config
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="userProperties"></param>
        /// <param name="adFieldsCollection"></param>
        public static void SetPropertiesFromAD<TUserADinDB>(string userLogin, TUserADinDB userProperties)
        {
            ADFieldsCollection adFieldsCollection = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.AD;
            MethodFunctionElement customCodeAD = BIASettingsReader.BIANetSection?.Authentication?.Sources?.UserProperties?.CustomCodeAD;

            if ((adFieldsCollection != null && adFieldsCollection.Count > 0) || (customCodeAD != null))
            {
                UserPrincipal userPrincipal = ADHelper.GetUserFromADs(userLogin);

                if (adFieldsCollection != null && adFieldsCollection.Count > 0)
                {

                    if (userPrincipal != null)
                    {
                        foreach (ADFieldElement value in adFieldsCollection)
                        {
                            PropertyInfo propertyInfo = userProperties.GetType().GetProperty(value.Key);
                            if (propertyInfo != null)
                            {
                                propertyInfo.SetValue(userProperties, Convert.ChangeType(ADHelper.GetProperty(userPrincipal, value.Adfield, value.MaxLenght, value.Default), propertyInfo.PropertyType));
                            }
                        }
                    }
                }

                if (customCodeAD != null)
                {
                    if (customCodeAD.Type != null)
                    {
                        if (!string.IsNullOrEmpty(customCodeAD.Method))
                        {
                            customCodeAD.Type.GetMethod(customCodeAD.Method).Invoke(null, new object[] { userPrincipal, userProperties });
                        }
                        else
                        {
                            customCodeAD.Type.GetProperty(customCodeAD.Property).GetValue(null, new object[] { userPrincipal, userProperties });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the login of the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>the login of the user</returns>
        public static string GetUserName(UserPrincipal user)
        {
            string userName = user.Sid.Translate(typeof(NTAccount)).ToString();

            return RemoveDomain(userName);
        }

        /// <summary>
        /// Remove Domain. Example: From EU\LT1234 To LT1234
        /// </summary>
        /// <param name="user">The login</param>
        /// <returns>the login without domain</returns>
        public static string RemoveDomain(string userName)
        {
            if (userName.Contains('\\'))
            {
                userName = userName.Split('\\')[1];
            }

            return userName;
        }

        /// <summary>
        /// Gets the user in group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="adDomains">List of AD Domain.</param>
        /// <returns>List of user in group</returns>
        public static List<UserPrincipal> GetAllUsersInGroup(string groupName)
        {
            List<string> adDomains = BIASettingsReader.ADDomains;

            GroupPrincipal group = null;

            if (adDomains != null)
            {
                foreach (string domain in adDomains)
                {
                    try
                    {
                        PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);
                        group = GroupPrincipal.FindByIdentity(ctx, groupName);
                        if (group != null)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        TraceManager.Warn("ADHelper", "CreateUserFromAD", "Could not join Domain :" + domain, e);
                    }
                }
            }

            if (group == null)
            {
                // Used on developement VM
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                group = GroupPrincipal.FindByIdentity(ctx, groupName);
            }

            List<UserPrincipal> listUsers = new List<UserPrincipal>();
            List<GroupPrincipal> listTreatedGroups = new List<GroupPrincipal>();
            // if found....
            if (group != null)
            {
                GetAllUsersFromGroupRecursivly(group, listUsers, listTreatedGroups);
            }

            return listUsers;
        }

        private static void GetAllUsersFromGroupRecursivly(GroupPrincipal group, List<UserPrincipal> listUsers, List<GroupPrincipal> listTreatedGroups)
        {
            listTreatedGroups.Add(group);
            // iterate over members
            foreach (Principal p in group.GetMembers())
            {
                Console.WriteLine("{0}: {1}", p.StructuralObjectClass, p.DisplayName);

                // do whatever you need to do to those members


                if (p is UserPrincipal)
                {
                    UserPrincipal theUser = p as UserPrincipal;
                    if (!listUsers.Select(u => u.DistinguishedName).Contains(theUser.DistinguishedName))
                    {
                        listUsers.Add(theUser);
                    }
                }
                else if (p is GroupPrincipal)
                {
                    GroupPrincipal theGroup = p as GroupPrincipal;
                    if (!listTreatedGroups.Select(g => g.DistinguishedName).Contains(theGroup.DistinguishedName))
                    {
                        GetAllUsersFromGroupRecursivly(theGroup, listUsers, listTreatedGroups);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the user from ad.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="adDomains">The ad domains.</param>
        /// <returns>the user from ad</returns>
        public static UserPrincipal GetUserFromADs(string userName)
        {
            List<string> adDomains = BIASettingsReader.ADDomains;
            UserPrincipal user = null;

            if (adDomains != null)
            {
                foreach (string domain in adDomains)
                {
                    try
                    {
                        PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);
                        user = UserPrincipal.FindByIdentity(ctx, userName);
                        if (user != null)
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        TraceManager.Warn("ADHelper", "GetUserFromADs", "Could not join Domain :" + domain, e);
                    }
                }
            }
            /*
            if (user == null)
            {
                // Used on developement VM
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                user = UserPrincipal.FindByIdentity(ctx, userName);
            }*/

            return user;
        }


        static Dictionary<string, string> CacheGroupName = new Dictionary<string, string>();

        /// <summary>
        /// Gets the groups of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>the list of the group of the user</returns>
        public static List<string> GetGroups(string userName)
        {
            TraceManager.Debug("ADHelper", "GetGroups", "Begin for : " + userName);

            userName = RemoveDomain(userName);


            List<string> adDomains = BIASettingsReader.ADDomains;

            if (adDomains != null)
            {
                foreach (string domain in adDomains)
                {
                    try
                    {
                        WindowsIdentity wi = new WindowsIdentity(userName + "@" + domain);
                        if (wi != null)
                        {
                            TraceManager.Info("ADHelper", "GetGroups", "User : " + userName + " find in Domain : " + domain);
                            return GetGroups(wi);
                        }
                    }
                    catch (Exception)
                    {
                        TraceManager.Info("ADHelper", "GetGroups", "Could not find user : " + userName + " in Domain : " + domain);
                    }
                }
            }
            
            return new List<string>();
        }

        /// <summary>
        /// Gets the groups of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>the list of the group of the user</returns>
        public static List<string> GetGroups(string userName, KeyValueCollection ADRoles, KeyValueCollection adDomains)
        {
            TraceManager.Debug("ADHelper", "GetGroups", "Begin for : " + userName);

            if (adDomains != null)
            {
                foreach (KeyValueElement domain in adDomains)
                {
                    try
                    {
                        WindowsIdentity wi = new WindowsIdentity(userName + "@" + domain.Value);
                        if (wi != null)
                        {
                            TraceManager.Info("ADHelper", "GetGroups", "User : " + userName + " find in Domain : " + domain.Value);
                            return GetGroups(wi, ADRoles);
                        }
                    }
                    catch (Exception)
                    {
                        TraceManager.Info("ADHelper", "GetGroups", "Could not find user : " + userName + " in Domain : " + domain.Value);
                    }
                }
            }

            return new List<string>();
        }

        public static List<string> GetGroups(WindowsIdentity wi)
        {
            List<string> adSimuRoles = BIASettingsReader.ADSimuRoles;
            if (adSimuRoles != null && adSimuRoles.Count() > 0)
            {
                TraceManager.Debug("ADHelper", "GetGroups", "Roles simulated");
                return adSimuRoles;
            }

            TraceManager.Debug("ADHelper", "GetGroups", "Nb group in AD : " + wi.Groups.Count);

            List<string> result = new List<string>();

            foreach (IdentityReference group in wi.Groups)
            {
                try
                {
                    string groupName = "";
                    string groupValue = group.Value;
                    if (!CacheGroupName.TryGetValue(groupValue, out groupName))
                    {
                        groupName = group.Translate(typeof(NTAccount)).ToString();
                        CacheGroupName.Add(groupValue, groupName);
                    }

                    result.Add(groupName);
                }
                catch (Exception)
                {
                    TraceManager.Debug("ADHelper", "GetGroups", "Error");
                }
            }

            result.Sort();
            TraceManager.Debug("ADHelper", "GetGroups", "End");
            return FilterGroup(result);
        }


        public static List<string> GetGroups(WindowsIdentity wi, KeyValueCollection ADRoles)
        {
            TraceManager.Debug("ADHelper", "GetGroups", "Nb group in AD : " + wi.Groups.Count);

            List<string> result = new List<string>();

            foreach (IdentityReference group in wi.Groups)
            {
                try
                {
                    string groupName = "";
                    string groupValue = group.Value;
                    if (!CacheGroupName.TryGetValue(groupValue, out groupName))
                    {
                        groupName = group.Translate(typeof(NTAccount)).ToString();
                        CacheGroupName.Add(groupValue, groupName);
                    }

                    result.Add(groupName);
                }
                catch (Exception)
                {
                    TraceManager.Debug("ADHelper", "GetGroups", "Error");
                }
            }

            result.Sort();
            TraceManager.Debug("ADHelper", "GetGroups", "End");
            return FilterGroup(result, ADRoles);
        }

        /// <summary>
        /// Filters the group.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="adRolesPrefixesToRemove">The ad Roles.</param>
        /// <param name="adRolesFilters">The ad Filter.</param>
        /// <returns>List of group filtered</returns>
        private static List<string> FilterGroup(List<string> groups)
        {
            Dictionary<string, List<string>> adRoles = BIASettingsReader.ADRoles;
            List <string> result = new List<string>();
            foreach(var roleGroup in adRoles)
            {
                foreach(string adGroup in roleGroup.Value)
                {
                    string sRole = roleGroup.Key;
                    if (sRole.Contains('$'))
                    {
                        string pattern = "^" + adGroup + "$";
                        foreach (string group in groups)
                        {
                            if (Regex.IsMatch(group, pattern))
                            {
                                string roleToAdd = Regex.Replace(group, pattern, sRole);
                                result.Add(roleToAdd);
                                TraceManager.Debug("ADHelper", "FilterGroup", "Group added :" + roleToAdd);
                            }
                        }
                    }
                    else if (adGroup.Contains('.') || adGroup.Contains('*') || adGroup.Contains('{') || adGroup.Contains('[') || adGroup.Contains("\\\\"))
                    {
                        string pattern = "^" + adGroup + "$";
                        foreach (string group in groups)
                        {
                            if (Regex.IsMatch(group, pattern))
                            {
                                result.Add(sRole);
                                TraceManager.Debug("ADHelper", "FilterGroup", "Group added :" + sRole);
                                break;
                            }
                        }
                    }
                    else
                    { 

                        if (groups.Contains(adGroup))
                        {
                            result.Add(sRole);
                            TraceManager.Debug("ADHelper", "FilterGroup", "Group added :" + sRole);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Filters the group.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="adRolesPrefixesToRemove">The ad Roles.</param>
        /// <param name="adRolesFilters">The ad Filter.</param>
        /// <returns>List of group filtered</returns>
        private static List<string> FilterGroup(List<string> groups, KeyValueCollection ADRoles)
        {
            List<string> result = new List<string>();
            foreach (KeyValueElement roleGroup in ADRoles)
            {
                foreach (string adGroup in roleGroup.Value.Split(','))
                {
                    string sRole = roleGroup.Key;
                    if (sRole.Contains('$'))
                    {
                        string pattern = "^" + adGroup + "$";
                        foreach (string group in groups)
                        {
                            if (Regex.IsMatch(group, pattern))
                            {
                                string roleToAdd = Regex.Replace(group, pattern, sRole);
                                result.Add(roleToAdd);
                                TraceManager.Debug("ADHelper", "FilterGroup", "Group added :" + roleToAdd);
                            }
                        }
                    }
                    else if (adGroup.Contains('.') || adGroup.Contains('*') || adGroup.Contains('{') || adGroup.Contains('[') || adGroup.Contains("\\\\"))
                    {
                        string pattern = "^" + adGroup + "$";
                        foreach (string group in groups)
                        {
                            if (Regex.IsMatch(group, pattern))
                            {
                                result.Add(sRole);
                                TraceManager.Debug("ADHelper", "FilterGroup", "Group added :" + sRole);
                                break;
                            }
                        }
                    }
                    else
                    {

                        if (groups.Contains(adGroup))
                        {
                            result.Add(sRole);
                            TraceManager.Debug("ADHelper", "FilterGroup", "Group added :" + sRole);
                        }
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Gets the property.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="property">The property.</param>
        /// <param name="maxLenght">The maximum lenght.</param>
        /// <param name="empty">The empty.</param>
        /// <returns>the value of the property</returns>
        public static string GetProperty(UserPrincipal principal, string property, int maxLenght, string empty = "")
        {
            if (!string.IsNullOrEmpty(property))
            {
                DirectoryEntry directoryEntry = principal.GetUnderlyingObject() as DirectoryEntry;
                if (directoryEntry.Properties.Contains(property))
                {
                    string propValue = directoryEntry.Properties[property].Value.ToString();
                    propValue = propValue.Length <= maxLenght ? propValue : propValue.Substring(0, maxLenght);
                    return propValue;
                }
                else
                {
                    return empty;
                }
            }
            else
            {
                return empty;
            }
        }
    }
}