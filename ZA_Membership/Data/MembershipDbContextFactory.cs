using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ZA_Membership.Configuration;

namespace ZA_Membership.Data
{
    public class MembershipDbContextFactory : IDesignTimeDbContextFactory<MembershipDbContext>
    {
        public MembershipDbContext CreateDbContext(string[] args)
        {
            IConfiguration configuration = BuildConfiguration();

            // خواندن بخش ZAMembership
            var membershipOptions = new MembershipOptions();
            configuration.GetSection("ZAMembership").Bind(membershipOptions);

            // اگر کانکشن‌استرینگ نبود، یک مقدار پیش‌فرض بده
            if (string.IsNullOrWhiteSpace(membershipOptions.ConnectionString))
            {
                membershipOptions.ConnectionString =
                    "Server=(localdb)\\MSSQLLocalDB;Database=ZAMembershipDb;Trusted_Connection=True;TrustServerCertificate=True";
            }

            var optionsBuilder = new DbContextOptionsBuilder<MembershipDbContext>();
            optionsBuilder.UseSqlServer(
                membershipOptions.ConnectionString,
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(MembershipDbContext).Assembly.FullName)
            );

            return new MembershipDbContext(optionsBuilder.Options);
        }

        private IConfiguration BuildConfiguration()
        {
            // مسیر جاری (پروژه Startup یا کتابخانه)
            var basePath = Directory.GetCurrentDirectory();
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile("appsettings.Development.json", optional: true);

            // اگر فایل پیدا نشد، برو سراغ مسیر کتابخانه ZA_Membership
            var libPath = Path.Combine(basePath, "..", "ZA_Membership");
            if (Directory.Exists(libPath))
            {
                configBuilder
                    .AddJsonFile(Path.Combine(libPath, "appsettings.json"), optional: true)
                    .AddJsonFile(Path.Combine(libPath, "appsettings.Development.json"), optional: true);
            }

            return configBuilder.Build();
        }
    }
}
