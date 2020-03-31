namespace ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Jwt
{
    using BIA.Net.Authentication.Web;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Claims;
    using System.Text;
    using System.Web;
    using ZZCompanyNameZZ.ZZProjectNameZZ.WebApi.Helpers;

    /// <summary>
    /// Bia Jwt Manager
    /// </summary>
    public abstract class BiaJwtManager
    {
        /// <summary>
        /// The allow role get token
        /// </summary>
        public const string ALLOW_GET_TOKEN = "AllowGetToken";

        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public abstract string Secret { get; }

        /// <summary>
        /// Gets the expires of the token.
        /// </summary>
        /// <value>
        /// The expires.
        /// </value>
        public abstract int Expires { get; }

        /// <summary>
        /// The header authorization
        /// </summary>
        private const string HeaderAuthorization = "Authorization";

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateToken()
        {
            string tokenString = null;

            UserInfoWebApi userInfo = this.CreateUserInfo();
            if (userInfo != null)
            {
                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secret));
                SigningCredentials signinCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                List<Claim> claims = FillClaims(userInfo);

                if (userInfo.Roles != null)
                {
                    foreach (string role in userInfo.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(this.Expires),
                    signingCredentials: signinCredentials
                );

                tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            }

            return tokenString;
        }

        /// <summary>
        /// Fills the claims.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        /// <returns></returns>
        public virtual List<Claim> FillClaims(UserInfoWebApi userInfo)
        {
            List<Claim> claims = null;

            if (userInfo != null)
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.WindowsAccountName, userInfo.Login),
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Properties?.Id.ToString()),
                    new Claim("Language", userInfo.Language)
                };
            }

            return claims;
        }

        /// <summary>
        /// Fills the user information.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        public virtual UserInfoWebApi FillUserInfo(IEnumerable<Claim> claims)
        {
            UserInfoWebApi userInfo = null;

            if (claims != null)
            {
                userInfo = new UserInfoWebApi();

                int.TryParse(userInfo.Properties?.Id.ToString(), out int id);
                userInfo.Properties = new Business.DTO.UserDTO() { Id = id };
                userInfo.Login = claims.FirstOrDefault(x => x.Type == ClaimTypes.WindowsAccountName)?.Value;
                userInfo.Language = claims.FirstOrDefault(x => x.Type == "Language")?.Value;
                userInfo.Roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).Distinct().ToList();
            }

            return userInfo;
        }

        /// <summary>
        /// Retrieves the user information.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual UserInfoWebApi RetrieveUserInfo(string token)
        {
            JwtSecurityToken securityToken = GetSecurityToken(token);
            return this.RetrieveUserInfo(securityToken);
        }

        /// <summary>
        /// Retrieves the user information.
        /// </summary>
        /// <param name="securityToken">The security token.</param>
        /// <returns></returns>
        public virtual UserInfoWebApi RetrieveUserInfo(JwtSecurityToken securityToken)
        {
            UserInfoWebApi userInfo = FillUserInfo(securityToken?.Claims);
            return userInfo;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public virtual string GetToken(HttpRequest request)
        {
            string jwtToken = null;

            NameValueCollection headers = request?.Headers;

            if (headers != null && headers[HeaderAuthorization] != null)
            {
                AuthenticationHeaderValue authorization = AuthenticationHeaderValue.Parse(headers[HeaderAuthorization].ToString());

                if (authorization.Scheme?.Equals("Bearer", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    jwtToken = authorization?.Parameter;
                }
            }

            return jwtToken;
        }

        /// <summary>
        /// Gets the security token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public virtual JwtSecurityToken GetSecurityToken(string token)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Secret));
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false
            };

            handler.ValidateToken(token, validations, out SecurityToken securityToken);
            return securityToken as JwtSecurityToken;
        }

        /// <summary>
        /// Gets the token.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public virtual string GetToken(HttpRequestMessage request)
        {
            string jwtToken = null;

            HttpRequestHeaders headers = request?.Headers;

            if (headers != null)
            {
                jwtToken = headers?.FirstOrDefault(x => x.Key.Equals(HeaderAuthorization, StringComparison.CurrentCultureIgnoreCase)).Value?.FirstOrDefault();

                if (headers?.Authorization?.Scheme?.Equals("Bearer", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    if (string.IsNullOrWhiteSpace(jwtToken))
                    {
                        jwtToken = headers?.Authorization?.Parameter;
                    }
                }
            }

            return jwtToken;
        }

        /// <summary>
        /// Creates the user information.
        /// </summary>
        /// <returns></returns>
        public virtual UserInfoWebApi CreateUserInfo()
        {
            UserInfoWebApi userInfo = BaseAuthorizationFilter<UserInfoWebApi, Business.DTO.UserDTO>.PrepareUserInfo();

            return userInfo;
        }
    }
}