using Microsoft.EntityFrameworkCore;
using ZA_Membership.Data;
using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;

namespace ZA_Membership.Repositories.Implementations
{
    internal class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly MembershipDbContext _context;

        public RolePermissionRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<RolePermission?> GetByIdAsync(int id)
        {
            return await _context.RolePermissions.FindAsync(id);
        }

        public async Task<List<RolePermission>> GetByRoleIdAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();
        }

        public async Task<List<RolePermission>> GetByPermissionIdAsync(int permissionId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .ToListAsync();
        }

        public async Task<RolePermission> CreateAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
            return rolePermission;
        }

        public async Task<RolePermission> UpdateAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Update(rolePermission);
            await _context.SaveChangesAsync();
            return rolePermission;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.RolePermissions.FindAsync(id);
            if (entity != null)
            {
                _context.RolePermissions.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByRoleIdAsync(int roleId)
        {
            var entities = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.RolePermissions.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByPermissionIdAsync(int permissionId)
        {
            var entities = await _context.RolePermissions
                .Where(rp => rp.PermissionId == permissionId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.RolePermissions.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RolePermissions.AnyAsync(rp => rp.Id == id);
        }

        public async Task<bool> ExistsAsync(int roleId, int permissionId)
        {
            return await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        }
    }
}
