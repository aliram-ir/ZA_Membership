using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    internal interface IUserRoleRepository
    {
        Task<UserRole?> GetByIdAsync(int id);
        Task<List<UserRole>> GetByUserIdAsync(int userId);
        Task<List<UserRole>> GetByRoleIdAsync(int roleId);
        Task<UserRole> CreateAsync(UserRole userRole);
        Task<UserRole> UpdateAsync(UserRole userRole);
        Task DeleteAsync(int id);
        Task DeleteByUserIdAsync(int userId);
        Task DeleteByRoleIdAsync(int roleId);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsAsync(int userId, int roleId);
    }
}
