using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace ArchT.Services.Identity.Infrastructure.Extensions
{
    public static class AuthenticationExtensions
    {
        public const string AuthorizationPolicy = "JwtPolicy";
        public const string CorsPolicy = "CorsPolicy";

        public static void ConfigureAuthentication(this IServiceCollection services, string serviceName, string serviceVersion)
        {
            services.AddAuthentication().AddJwtBearer(options =>
                options.TokenValidationParameters = GenerateParams());

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(AuthorizationPolicy, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Info { Title = serviceName, Version = serviceVersion });

                swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OAuth2Scheme
                {
                    Flow = "password",
                    TokenUrl = "/token"
                });

                swagger.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { JwtBearerDefaults.AuthenticationScheme, Enumerable.Empty<string>() }
                });
            });
        }

        public static void ConfigureAuthentication(this IApplicationBuilder app, params string[] versions)
        {
            app.UseAuthentication();
            app.UseCors(CorsPolicy);
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach(var version in versions)
                    options.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");

                options.RoutePrefix = string.Empty;
                options.OAuthClientId("client-id");
                options.OAuthClientSecret(Convert.ToBase64String(Encoding.UTF8.GetBytes("client-top-secret")));
                options.OAuthRealm("");
                options.OAuthAppName("");
            });
        }

        public static JwtSecurityToken GenerateSecurityToken(this ClaimsIdentity identity, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = "Issuer",
                Audience = "Audience",
                SigningCredentials = credentials,
                Subject = identity,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(10)
            }) as JwtSecurityToken;
            return securityToken;
        }

        public static TokenValidationParameters GenerateParams()
        {
            var secret = Convert.ToBase64String(Encoding.UTF8.GetBytes("client-top-secret"));
            var key = Encoding.ASCII.GetBytes(secret);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            return new TokenValidationParameters
            {
                IssuerSigningKey = credentials.Key,
                ValidAudience = "Audience",
                ValidIssuer = "Issuer",
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
