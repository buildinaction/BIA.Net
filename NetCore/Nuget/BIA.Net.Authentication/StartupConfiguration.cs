// <copyright file="StartupConfiguration.cs" company="BIA.Net">
//     Copyright (c) BIA.Net. All rights reserved.
// </copyright>

namespace BIA.Net.Authentication
{
    using System;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// The class containing configuration for authentication.
    /// </summary>
    public static class StartupConfiguration
    {
        /// <summary>
        /// The secret key used for sign JWT.
        /// </summary>
        private const string SecretKey = "0BA9AE2BDC6FBA1056D86EB2ACB45D53072F786FBD94C658240FAE6198F5DC64DE018FE517E68410B9D41B9BE106532345D21090549B0BC396437DCEF3AADAAC"; // todo: get this from somewhere secure

        /// <summary>
        /// The signing key for JWT.
        /// </summary>
        private static readonly SymmetricSecurityKey SigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        /// <summary>
        /// Configure the authentication.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The application configuration.</param>
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            // Get options from app settings
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = SigningKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(JwtBearerDefaults.AuthenticationScheme, policy => policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser().Build());
            });
        }
    }
}