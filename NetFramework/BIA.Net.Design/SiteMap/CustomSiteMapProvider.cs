// <copyright file="CustomSiteMapProvider.cs" company="BIA.NET">
// Copyright (c) BIA.NET. All rights reserved.
// </copyright>

namespace BIA.Net.Design.SiteMap
{
    using System.Web;

    public class CustomSiteMapProvider : XmlSiteMapProvider
    {
        private const string TITLE_AMINISTRATION = "Administration";

        public override bool IsAccessibleToUser(HttpContext context, SiteMapNode node)
        {
            bool isAccessible = false;
            if (node.Roles == null || node.Roles.Count ==0) isAccessible = true;
            else
            { 
                foreach (var role in node.Roles)
                {
                    if (context.User.IsInRole(role.ToString()))
                    {
                        isAccessible = true;
                    }
                }
            }

            if (!isAccessible)
            {
                if (node.Roles.Contains("Anonymous"))
                {
                    if (!context.User.Identity.IsAuthenticated)
                    {
                        isAccessible = true;
                    }
                }
                if (node.Roles.Contains("Identified"))
                {
                    if (context.User.Identity.IsAuthenticated)
                    {
                        isAccessible = true;
                    }
                }
            }

            return isAccessible;
        }
    }
}