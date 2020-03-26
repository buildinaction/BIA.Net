// <copyright file="IJwtFactory.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    /// <summary>
    /// The interface defining the JWT factory.
    /// </summary>
    public interface IJwtFactory
    {
        /// <summary>
        /// Generate an encoded JWT.
        /// </summary>
        /// <param name="identity">The identity of the current user.</param>
        /// <returns>The encoded JWT as string.</returns>
        Task<string> GenerateEncodedTokenAsync(ClaimsIdentity identity);

        /// <summary>
        /// Generate the identity for a user.
        /// </summary>
        /// <param name="userName">The user name (login).</param>
        /// <param name="id">The user identifier.</param>
        /// <param name="roles">The role list of the user.</param>
        /// <returns>The claims identity.</returns>
        ClaimsIdentity GenerateClaimsIdentity(string userName, int id, IEnumerable<string> roles);

        /// <summary>
        /// Generate a JWT.
        /// </summary>
        /// <param name="identity">The identity of the current user.</param>
        /// <param name="userInfo">The user info.</param>
        /// <param name="userProfile">The user profile.</param>
        /// <returns>The JWT as string.</returns>
        Task<object> GenerateJwtAsync(ClaimsIdentity identity, object userInfo, object userProfile);
    }
}