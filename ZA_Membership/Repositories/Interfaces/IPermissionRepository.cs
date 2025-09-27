using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    internal interface IPermissionRepository
    {
        Task<Permission?> GetByIdAsync(int id);
        Task<Permission?> GetByNameAsync(string name);
        Task<List<Permission>> GetAllAsync();
        Task<Permission> CreateAsync(Permission permission);
        Task<Permission> UpdateAsync(Permission permission);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByNameAsync(string name);
    }
}
