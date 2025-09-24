using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZA_Membership.Configuration;
using ZA_Membership.Security;
using ZA_Membership.Services.Implementations;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddZAMembership(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionName = "ZAMembership")
        {
            // Bind configuration
            var membershipOptions = new MembershipOptions();
            configuration.GetSection(configSectionName).Bind(membershipOptions);
            services.AddSingleton(membershipOptions);

            // Register services
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordService, PasswordService>();

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(membershipOptions.Jwt.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = membershipOptions.Jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = membershipOptions.Jwt.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddZAMembershipWithOptions(
            this IServiceCollection services,
            Action<MembershipOptions> configureOptions)
        {
            var options = new MembershipOptions();
            configureOptions(options);
            services.AddSingleton(options);

            // Register services
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordService, PasswordService>();

            // Configure JWT Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Jwt.SecretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = options.Jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = options.Jwt.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization();

            return services;
        }
    }
}
