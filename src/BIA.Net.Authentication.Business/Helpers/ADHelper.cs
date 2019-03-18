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
    using static BIA.Net.Common.Configuration.AuthenticationElement.ParametersElement;
    using static BIA.Net.Common.Configuration.CommonElement;


    /// <summary>
    /// AuthHelper
    /// </summary>
    public static class ADHelper
    {

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

        private static String isRoleInit = "";

        private static Dictionary<string, List<ADGroup>> adRoles = null;
        public static Dictionary<string, List<ADGroup>> ADRoles
        {
            get
            {
                lock (isRoleInit)
                {
                    if (adRoles == null)
                    {
                        List<string> adDomains = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.ADDomains;

                        adRoles = new Dictionary<string, List<ADGroup>>();
                        HeterogeneousCollection rolesValues = BIASettingsReader.BIANetSection?.Authentication?.Roles;
                        if (rolesValues != null && rolesValues.Count > 0)
                        {
                            foreach (IHeterogeneousConfigurationElement heterogeneousElem in rolesValues)
                            {
                                if (heterogeneousElem.TagName == "ADRole")
                                {
                                    ValueElement ADRole = (ValueElement)heterogeneousElem;
                                    List<string> values = new List<string>(ADRole.Value.Split(',')).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
                                    if (values != null && values.Any())
                                    {
                                        List<ADGroup> groups = new List<ADGroup>();

                                        foreach (string value in values)
                                        {
                                            ADGroup group = new ADGroup(value, ADRole.Key);
                                            groups.Add(group);
                                        }
                                        adRoles.Add(ADRole.Key, groups);
                                    }
                                }
                            }
                        }
                        isRoleInit = "init";
                    }
                }
                return adRoles;
            }
        }

        /// <summary>
        /// Gets AD Groups As Application Users
        /// </summary>
        public static List<ADGroup> GetADGroupsForRole(string role)
        {
            List<ADGroup> value = null;
            if (ADRoles.TryGetValue(role, out value))
            {
                return value;
            }
            return null;
        }

        /// <summary>
        /// Gets AD Groups As Application Users
        /// </summary>
        public static List<ADGroup> GetADGroupsForRoleOrCreate(string role,List<string> adGroupsName)
        {
            List<ADGroup> value = null;
            if (ADRoles.TryGetValue(role, out value))
            {
                return value;
            }
            List<ADGroup> groups = new List<ADGroup>();
            foreach (string groupName in adGroupsName)
            {
                ADGroup group = new ADGroup(groupName, role);
                groups.Add(group);
            }
            adRoles.Add(role, groups);
            return groups;
        }


        /// <summary>
        /// Gets the user from ad.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="adDomains">The ad domains.</param>
        /// <returns>the user from ad</returns>
        public static UserPrincipal GetUserFromADs(string userName)
        {
            List<string> adDomains = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.ADDomains;
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

        /// <summary>
        /// Gets the groups of the user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns>the list of the group of the user</returns>
        public static List<string> GetGroups(string userName)
        {
            List<string> adDomains = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.ADDomains;
            TraceManager.Debug("ADHelper", "GetGroups", "Begin for : " + userName);

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




        public static List<string> GetGroups(WindowsIdentity wi)
        {
            List<string> result = new List<string>();
            if (wi != null)
            {
                TraceManager.Debug("ADHelper", "GetGroups", "Nb group in AD : " + wi.Groups.Count);

                foreach (IdentityReference group in wi.Groups)
                {
                    try
                    {
                        string groupName = "";
                        string groupValue = group.Value;
                        if (!CacheGroupName.TryGetValue(groupValue, out groupName))
                        {
                            TraceManager.Debug("Try resolve name : " + groupValue);
                            groupName = group.Translate(typeof(NTAccount)).ToString();
                            CacheGroupName.Add(groupValue, groupName);
                            TraceManager.Debug("Name resolve : " + groupName);
                        }

                        result.Add(groupName);
                    }
                    catch (Exception e)
                    {
                        TraceManager.Debug("Error");
                        TraceManager.Warn("Error when treat " + group.Value, e);
                    }
                }

                result.Sort();
                TraceManager.Debug("ADHelper", "GetGroups", "End");
            }

            return result;
        }
        /*
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
            List<string> result = new List<string>();
            foreach (var roleGroup in adRoles)
            {
                foreach (string adGroup in roleGroup.Value)
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
        */
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