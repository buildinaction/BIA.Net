

namespace BIA.Net.Authentication.Business.Helpers
{
    using BIA.Net.Common;
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;
    using static BIA.Net.Common.Configuration.AuthenticationElement.ParametersElement;

    public class ADGroup
    {
        public ADGroup(string groupName, string role)
        {
            GroupName = groupName;
            Role = role;
            if (groupName.Contains('\\'))
            {
                string[] split = groupName.Split('\\');
                GroupShortName = split[split.Length - 1];
            }
            else GroupShortName = groupName;
            IsInit = false;
            IsValid = false;
            Domain = null;
            Group = null;
        }

        public string GroupName { get; }
        public string Role { get; }
        public string GroupShortName { get; }
        private bool IsInit { get; set; }
        private bool IsValid { get; set; }
        private string Domain { get; set; }
        private GroupPrincipal Group { get; set; }

        public bool IsUserInGroup(string Login)
        {
            List<UserPrincipal> allUser = GetAllUsersInGroup();
            return allUser.Any(u => u.Name == Login);
        }



        /// <summary>
        /// Gets the user in group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns>List of user in group</returns>

        public List<UserPrincipal> GetAllUsersInGroup()
        {
            Init();
            List<UserPrincipal> listUsers = new List<UserPrincipal>();
            List<GroupPrincipal> listTreatedGroups = new List<GroupPrincipal>();
            // if found....
            if (IsValid)
            {
                TraceManager.Debug("Group " + GroupName + " is valid");
                GetAllUsersFromGroupRecursivly(Group, listUsers, listTreatedGroups);
            }
            else
            {
                TraceManager.Debug("Group " + GroupName + " is not valid");
            }

            return listUsers;
        }

        private static void GetAllUsersFromGroupRecursivly(GroupPrincipal group, List<UserPrincipal> listUsers, List<GroupPrincipal> listTreatedGroups)
        {
            listTreatedGroups.Add(group);
            TraceManager.Debug("Treat group " + group.Name);
            try
            {
                // iterate over members
                foreach (Principal p in group.GetMembers())
                {
                    Console.WriteLine("{0}: {1}", p.StructuralObjectClass, p.DisplayName);
                    TraceManager.Debug("Treat member " + p.Name);
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
                TraceManager.Debug("Group " + group.Name + " treated.");
            }
            catch (Exception e)
            {
                TraceManager.Warn("GetAllUsersFromGroupRecursivly crash", e);
            }
        }

        private void Init()
        {
            if (!IsInit)
            {
                List<string> adDomains = BIASettingsReader.BIANetSection?.Authentication?.Parameters?.ADDomains;
                GroupPrincipal group = null;

                if (adDomains != null)
                {
                    foreach (string domain in adDomains)
                    {
                        try
                        {
                            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain);
                            group = GroupPrincipal.FindByIdentity(ctx, GroupName);
                            if (group != null)
                            {
                                Domain = domain;
                                Group = group;
                                IsValid = true;
                                TraceManager.Debug("Group " + GroupName + " not in domain " + domain);
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            TraceManager.Warn("Could not join Domain :" + domain, e);
                        }
                        TraceManager.Debug("Group " + GroupName + " not found in domain " + domain);
                    }
                }
            }

            IsInit = true;
        }
    }

}
