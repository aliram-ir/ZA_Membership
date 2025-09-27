using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUserActivityRepository _userActivityRepository;

        internal UserActivityService(IUserActivityRepository userActivityRepository)
        {
            _userActivityRepository = userActivityRepository;
        }

        public Task<UserActivity?> GetByIdAsync(int id) =>
            _userActivityRepository.GetByIdAsync(id);

        public Task<List<UserActivity>> GetByUserIdAsync(int userId) =>
            _userActivityRepository.GetByUserIdAsync(userId);

        public Task<List<UserActivity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate) =>
            _userActivityRepository.GetByDateRangeAsync(startDate, endDate);

        public Task<UserActivity> CreateAsync(UserActivity activity) =>
            _userActivityRepository.CreateAsync(activity);

        public Task<UserActivity> UpdateAsync(UserActivity activity) =>
            _userActivityRepository.UpdateAsync(activity);

        public Task DeleteAsync(int id) =>
            _userActivityRepository.DeleteAsync(id);

        public Task DeleteByUserIdAsync(int userId) =>
            _userActivityRepository.DeleteByUserIdAsync(userId);

        public Task<bool> ExistsAsync(int id) =>
            _userActivityRepository.ExistsAsync(id);
    }
}
