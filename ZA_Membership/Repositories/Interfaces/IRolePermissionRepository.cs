using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    internal interface IRolePermissionRepository
    {
        Task<RolePermission?> GetByIdAsync(int id);
        Task<List<RolePermission>> GetByRoleIdAsync(int roleId);
        Task<List<RolePermission>> GetByPermissionIdAsync(int permissionId);
        Task<RolePermission> CreateAsync(RolePermission rolePermission);
        Task<RolePermission> UpdateAsync(RolePermission rolePermission);
        Task DeleteAsync(int id);
        Task DeleteByRoleIdAsync(int roleId);
        Task DeleteByPermissionIdAsync(int permissionId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(int roleId, int permissionId);
    }
}
