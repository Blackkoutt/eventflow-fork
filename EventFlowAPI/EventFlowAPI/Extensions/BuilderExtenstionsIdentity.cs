using EventFlowAPI.DB.Context;
using EventFlowAPI.DB.Entities;
using EventFlowAPI.Enums;
using EventFlowAPI.Logic.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace EventFlowAPI.Extensions
{
    public static class BuilderExtenstionsIdentity
    {
        public static void AddJwtSecurityKey(this WebApplicationBuilder builder, AuthConfiguration jwtSettingsSection)
        {
            var keyConfigurationPath = $"{jwtSettingsSection}:securityKey";
            if (!string.IsNullOrEmpty(builder.Configuration[keyConfigurationPath]))
            {
                return;
            }

            if (!builder.Environment.IsDevelopment())
            {
                throw new InvalidOperationException(
                    $"Missing configuration value '{keyConfigurationPath}'. Outside of development it has to be provided, e.g. with the 'JWTAuth__securityKey' environment variable.");
            }

            var keyFilePath = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "jwt.key");
            if (!File.Exists(keyFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(keyFilePath)!);
                File.WriteAllText(keyFilePath, Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
            }

            builder.Configuration[keyConfigurationPath] = File.ReadAllText(keyFilePath).Trim();
        }

        public static void AddAuthentication(this WebApplicationBuilder builder, AuthConfiguration jwtSettingsSection)
        {
            var jwtSettings = builder.Configuration.GetSection(jwtSettingsSection.ToString());

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetJWTTokenOptions(jwtSettings);
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Cookies[Cookie.EventFlowJWTCookie.ToString()];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        }

        private static TokenValidationParameters GetJWTTokenOptions(IConfigurationSection jwtSettingsSection)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettingsSection["validIssuer"],
                ValidAudience = jwtSettingsSection["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(jwtSettingsSection.GetSection("securityKey").Value!)),
                RoleClaimType = "userRoles"
            };
        }
    }
}
