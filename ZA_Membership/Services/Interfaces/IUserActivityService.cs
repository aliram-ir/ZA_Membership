using ZA_Membership.Models.Entities;

namespace ZA_Membership.Services.Interfaces
{
    public interface IUserActivityService
    {
        Task<UserActivity?> GetByIdAsync(int id);
        Task<List<UserActivity>> GetByUserIdAsync(int userId);
        Task<List<UserActivity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<UserActivity> CreateAsync(UserActivity activity);
        Task<UserActivity> UpdateAsync(UserActivity activity);
        Task DeleteAsync(int id);
        Task DeleteByUserIdAsync(int userId);
        Task<bool> ExistsAsync(int id);
    }
}
