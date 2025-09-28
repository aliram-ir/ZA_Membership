using Microsoft.EntityFrameworkCore;
using ZA_Membership.Configuration;
using ZA_Membership.Models.Entities;
using ZA_Membership.Security;

namespace ZA_Membership.Data
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // استفاده از سیاست پیش‌فرض پسورد
            var passwordService = new PasswordService(new MembershipOptions
            {
                Password = new PasswordOptions
                {
                    MinimumLength = 6,
                    RequireUppercase = true,
                    RequireLowercase = true,
                    RequireDigit = true,
                    RequireSpecialCharacter = true
                }
            });

            var adminPassword = "Admin@123";
            var fixedDate = new DateTime(2025, 09, 28);

            // کاربر پیش‌فرض
            var adminUser = new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@example.com",
                PasswordHash = passwordService.HashPassword(adminPassword),
                FirstName = "System",
                LastName = "Administrator",
                IsActive = true,
                IsVerify = true,
                CreatedAt = fixedDate
            };

            // نقش‌ها
            var adminRole = new Role { Id = 1, Name = "Admin" };
            var userRole = new Role { Id = 2, Name = "User" };

            // تعریف Permissionها
            var permissions = new List<Permission>
            {
                new Permission { Id = 1, Name = "ManageUsers" },
                new Permission { Id = 2, Name = "ViewReports" },
                new Permission { Id = 3, Name = "EditSettings" },
                new Permission { Id = 4, Name = "ManageRoles" },
                new Permission { Id = 5, Name = "ManagePermissions" }
            };

            // نقش Admin → همه‌ی Permissionها
            var rolePermissions = new List<RolePermission>();
            int rpId = 1;
            foreach (var perm in permissions)
            {
                rolePermissions.Add(new RolePermission
                {
                    Id = rpId++,
                    RoleId = 1,
                    PermissionId = perm.Id
                });
            }

            // ارتباط کاربر admin با نقش Admin
            var adminUserRole = new UserRole { Id = 1, UserId = 1, RoleId = 1 };

            // ثبت داده‌ها
            modelBuilder.Entity<User>().HasData(adminUser);
            modelBuilder.Entity<Role>().HasData(adminRole, userRole);
            modelBuilder.Entity<Permission>().HasData(permissions);
            modelBuilder.Entity<RolePermission>().HasData(rolePermissions);
            modelBuilder.Entity<UserRole>().HasData(adminUserRole);
        }
    }
}
