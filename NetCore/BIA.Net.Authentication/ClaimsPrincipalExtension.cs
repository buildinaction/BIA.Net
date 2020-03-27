// <copyright file="ClaimsPrincipalExtension.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// A class extension helping with the claims principal to get claims values.
    /// </summary>
    public static class ClaimsPrincipalExtension
    {
        /// <summary>
        /// Get the user identifier in the claims.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The user identifier.</returns>
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.HasClaim(x => x.Type == ClaimTypes.Sid))
            {
                return 0;
            }

            var userId = claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
        }

        /// <summary>
        /// Get the user login in the claims.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The user login.</returns>
        public static string GetUserLogin(this ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.HasClaim(x => x.Type == ClaimTypes.Name))
            {
                return string.Empty;
            }

            return claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.Name).Value;
        }

        /// <summary>
        /// Get the user rights in the claims.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>The user rights.</returns>
        public static IEnumerable<string> GetUserRights(this ClaimsPrincipal claimsPrincipal)
        {
            if (!claimsPrincipal.HasClaim(x => x.Type == ClaimTypes.Role))
            {
                return new List<string>();
            }

            return claimsPrincipal.FindAll(x => x.Type == ClaimTypes.Role).Select(s => s.Value).ToList();
        }
    }
}