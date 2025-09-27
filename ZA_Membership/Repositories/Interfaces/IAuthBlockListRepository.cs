using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    internal interface IAuthBlockListRepository
    {
        Task<AuthBlockList?> GetByIdAsync(int id);
        Task<AuthBlockList?> GetByUserIdAsync(int userId);
        Task<AuthBlockList> CreateAsync(AuthBlockList blockEntry);
        Task<AuthBlockList> UpdateAsync(AuthBlockList blockEntry);
        Task DeleteAsync(int id);
        Task DeleteByUserIdAsync(int userId);
    }
}
