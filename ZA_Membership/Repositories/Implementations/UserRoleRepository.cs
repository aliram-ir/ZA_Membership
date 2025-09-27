using Microsoft.EntityFrameworkCore;
using ZA_Membership.Data;
using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;

namespace ZA_Membership.Repositories.Implementations
{
    internal class UserRoleRepository : IUserRoleRepository
    {
        private readonly MembershipDbContext _context;

        public UserRoleRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<UserRole?> GetByIdAsync(int id)
        {
            return await _context.UserRoles.FindAsync(id);
        }

        public async Task<List<UserRole>> GetByUserIdAsync(int userId)
        {
            return await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<UserRole>> GetByRoleIdAsync(int roleId)
        {
            return await _context.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<UserRole> CreateAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }

        public async Task<UserRole> UpdateAsync(UserRole userRole)
        {
            _context.UserRoles.Update(userRole);
            await _context.SaveChangesAsync();
            return userRole;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.UserRoles.FindAsync(id);
            if (entity != null)
            {
                _context.UserRoles.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var entities = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.UserRoles.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByRoleIdAsync(int roleId)
        {
            var entities = await _context.UserRoles
                .Where(ur => ur.RoleId == roleId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.UserRoles.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.UserRoles.AnyAsync(ur => ur.Id == id);
        }

        public async Task<bool> ExistsAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }
    }
}
