using Microsoft.EntityFrameworkCore;
using ZA_Membership.Data.Configurations;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Data
{
    public class MembershipDbContext : DbContext
    {
        public MembershipDbContext(DbContextOptions<MembershipDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<AuthBlockList> AuthBlockLists { get; set; }
        public DbSet<AuthFailedAttempt> AuthFailedAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(MembershipDbContext).Assembly);
            #region Manual Configs
            //builder.ApplyConfiguration(new UserConfiguration());
            //builder.ApplyConfiguration(new RoleConfiguration());
            //builder.ApplyConfiguration(new UserRoleConfiguration());
            //builder.ApplyConfiguration(new UserTokenConfiguration());
            //builder.ApplyConfiguration(new AddressConfiguration());
            //builder.ApplyConfiguration(new AuthBlockListConfiguration());
            //builder.ApplyConfiguration(new AuthFailedAttemptConfiguration());
            //builder.ApplyConfiguration(new PermissionConfiguration());
            //builder.ApplyConfiguration(new RolePermissionConfiguration());
            //builder.ApplyConfiguration(new UserActivityConfiguration());
            #endregion
        }
    }
}
