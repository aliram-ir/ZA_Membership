using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        internal PermissionService(
            IPermissionRepository permissionRepository,
            IRolePermissionRepository rolePermissionRepository)
        {
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public Task<Permission?> GetByIdAsync(int id) => _permissionRepository.GetByIdAsync(id);

        public Task<Permission?> GetByNameAsync(string name) => _permissionRepository.GetByNameAsync(name);

        public Task<List<Permission>> GetAllAsync() => _permissionRepository.GetAllAsync();

        public Task<Permission> CreateAsync(Permission permission) => _permissionRepository.CreateAsync(permission);

        public Task<Permission> UpdateAsync(Permission permission) => _permissionRepository.UpdateAsync(permission);

        public Task DeleteAsync(int id) => _permissionRepository.DeleteAsync(id);

        public async Task AssignPermissionToRoleAsync(int roleId, int permissionId)
        {
            if (!await _rolePermissionRepository.ExistsAsync(roleId, permissionId))
            {
                var rp = new RolePermission { RoleId = roleId, PermissionId = permissionId };
                await _rolePermissionRepository.CreateAsync(rp);
            }
        }

        public async Task RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var rolePermissions = await _rolePermissionRepository.GetByRoleIdAsync(roleId);
            var rp = rolePermissions.FirstOrDefault(p => p.PermissionId == permissionId);
            if (rp != null)
            {
                await _rolePermissionRepository.DeleteAsync(rp.Id);
            }
        }
    }
}
