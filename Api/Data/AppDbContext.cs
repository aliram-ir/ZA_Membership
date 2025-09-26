using Microsoft.EntityFrameworkCore;
using ZA_Membership.Models.Entities;

namespace Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    // This will only be used for design-time operations like migrations
            //    optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ZA_Membership;Trusted_Connection=true;TrustServerCertificate=true;");
            //}
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Permission> Permission { get; set; }
    }
}


