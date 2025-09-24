using ZA_Membership.Models.Entities;

namespace ZA_Membership.Services.Interfaces
{
    public interface IUserTokenRepository
    {
        Task<UserToken?> GetByTokenAsync(string token);
        Task<UserToken> CreateAsync(UserToken userToken);
        Task<UserToken> UpdateAsync(UserToken userToken);
        Task DeleteAsync(int id);
        Task DeleteByUserIdAsync(int userId);
        Task RevokeTokenAsync(string token);
        Task RevokeAllUserTokensAsync(int userId);
        Task<List<UserToken>> GetUserActiveTokensAsync(int userId);
    }
}