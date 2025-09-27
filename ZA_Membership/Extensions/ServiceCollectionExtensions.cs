using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZA_Membership.Configuration;
using ZA_Membership.Data;
using ZA_Membership.Security;
using ZA_Membership.Services.Implementations;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// ثبت سرویس‌های ZA_Membership همراه با DbContext، ریپازیتوری‌ها و اجرای خودکار مایگریشن‌ها
        /// </summary>
        public static IServiceCollection AddZAMembership(
            this IServiceCollection services,
            IConfiguration configuration,
            string configSectionName = "ZAMembership")
        {
            // Bind configuration to MembershipOptions
            var membershipOptions = new MembershipOptions();
            configuration.GetSection(configSectionName).Bind(membershipOptions);
            services.AddSingleton(membershipOptions);

            // Register DbContext for ZA_Membership
            services.AddDbContext<MembershipDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(MembershipDbContext).Assembly.FullName)
                )
            );

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();

            // Register core services
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IPasswordService, PasswordService>();

            // Configure JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(optionsJwt =>
            {
                optionsJwt.TokenValidationParameters = new TokenValidationParameters
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

            // ⚡ Safe automatic migration execution at startup
            services.AddHostedService<MembershipMigrationHostedService>();

            return services;
        }
    }
}


#region OldCode
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using ZA_Membership.Configuration;
//using ZA_Membership.Data;
//using ZA_Membership.Security;
//using ZA_Membership.Services.Implementations;
//using ZA_Membership.Services.Interfaces;

//namespace ZA_Membership.Extensions
//{
//    public static class ServiceCollectionExtensions
//    {
//        /// <summary>
//        /// ثبت سرویس‌های ZA_Membership همراه با DbContext، ریپازیتوری‌ها و اجرای خودکار مایگریشن‌ها
//        /// </summary>
//        public static IServiceCollection AddZAMembership(
//            this IServiceCollection services,
//            IConfiguration configuration,
//            string configSectionName = "ZAMembership")
//        {
//            // Bind configuration to MembershipOptions
//            var membershipOptions = new MembershipOptions();
//            configuration.GetSection(configSectionName).Bind(membershipOptions);
//            services.AddSingleton(membershipOptions);

//            // Register DbContext for ZA_Membership
//            services.AddDbContext<MembershipDbContext>(options =>
//                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
//                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(MembershipDbContext).Assembly.FullName)
//                )
//            );

//            // Register repositories
//            services.AddScoped<IUserRepository, UserRepository>();
//            services.AddScoped<IRoleRepository, RoleRepository>();
//            services.AddScoped<IUserTokenRepository, UserTokenRepository>();

//            // Register core services
//            services.AddScoped<IMembershipService, MembershipService>();
//            services.AddScoped<IAddressRepository, AddressRepository>();
//            services.AddScoped<IJwtTokenService, JwtTokenService>();
//            services.AddScoped<IPasswordService, PasswordService>();

//            // Configure JWT authentication
//            services.AddAuthentication(options =>
//            {
//                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//            })
//            .AddJwtBearer(optionsJwt =>
//            {
//                optionsJwt.TokenValidationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(membershipOptions.Jwt.SecretKey)),
//                    ValidateIssuer = true,
//                    ValidIssuer = membershipOptions.Jwt.Issuer,
//                    ValidateAudience = true,
//                    ValidAudience = membershipOptions.Jwt.Audience,
//                    ValidateLifetime = true,
//                    ClockSkew = TimeSpan.Zero
//                };
//            });

//            services.AddAuthorization();

//            // Run migrations automatically at startup
//            using (var scope = services.BuildServiceProvider().CreateScope())
//            {
//                var db = scope.ServiceProvider.GetRequiredService<MembershipDbContext>();
//                db.Database.Migrate();
//            }

//            return services;
//        }
//    }
//}

#endregion