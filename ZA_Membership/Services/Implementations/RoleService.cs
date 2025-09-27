using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IUserRoleRepository _userRoleRepository;

        internal RoleService(
            IRoleRepository roleRepository,
            IRolePermissionRepository rolePermissionRepository,
            IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _userRoleRepository = userRoleRepository;
        }

        public Task<Role?> GetByIdAsync(int id) => _roleRepository.GetByIdAsync(id);

        public Task<Role?> GetByNameAsync(string name) => _roleRepository.GetByNameAsync(name);

        public Task<List<Role>> GetAllAsync() => _roleRepository.GetAllAsync();

        public Task<Role> CreateAsync(Role role) => _roleRepository.CreateAsync(role);

        public Task<Role> UpdateAsync(Role role) => _roleRepository.UpdateAsync(role);

        public Task DeleteAsync(int id) => _roleRepository.DeleteAsync(id);

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
            var entries = await _rolePermissionRepository.GetByRoleIdAsync(roleId);
            var entry = entries.FirstOrDefault(e => e.PermissionId == permissionId);
            if (entry != null)
            {
                await _rolePermissionRepository.DeleteAsync(entry.Id);
            }
        }

        public async Task AssignRoleToUserAsync(int userId, int roleId)
        {
            if (!await _userRoleRepository.ExistsAsync(userId, roleId))
            {
                var ur = new UserRole { UserId = userId, RoleId = roleId };
                await _userRoleRepository.CreateAsync(ur);
            }
        }

        public async Task RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var entries = await _userRoleRepository.GetByUserIdAsync(userId);
            var entry = entries.FirstOrDefault(e => e.RoleId == roleId);
            if (entry != null)
            {
                await _userRoleRepository.DeleteAsync(entry.Id);
            }
        }
    }
}
