using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    public class AuthBlockListService : IAuthBlockListService
    {
        private readonly IAuthBlockListRepository _authBlockListRepository;

        internal AuthBlockListService(IAuthBlockListRepository authBlockListRepository)
        {
            _authBlockListRepository = authBlockListRepository;
        }

        public Task<AuthBlockList?> GetByIdAsync(int id) =>
            _authBlockListRepository.GetByIdAsync(id);

        public Task<AuthBlockList?> GetByUserIdAsync(int userId) =>
            _authBlockListRepository.GetByUserIdAsync(userId);

        public Task<AuthBlockList> CreateAsync(AuthBlockList blockEntry) =>
            _authBlockListRepository.CreateAsync(blockEntry);

        public Task<AuthBlockList> UpdateAsync(AuthBlockList blockEntry) =>
            _authBlockListRepository.UpdateAsync(blockEntry);

        public Task DeleteAsync(int id) =>
            _authBlockListRepository.DeleteAsync(id);

        public Task DeleteByUserIdAsync(int userId) =>
            _authBlockListRepository.DeleteByUserIdAsync(userId);

    }
}
