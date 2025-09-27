using Microsoft.EntityFrameworkCore;
using ZA_Membership.Data;
using ZA_Membership.Models.Entities;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly MembershipDbContext _context;

        public UserTokenRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<UserToken> CreateAsync(UserToken token)
        {
            _context.UserTokens.Add(token);
            await _context.SaveChangesAsync();
            return token;
        }

        public async Task RevokeTokenAsync(string token)
        {
            var entity = await _context.UserTokens.FirstOrDefaultAsync(t => t.Token == token);
            if (entity != null)
            {
                _context.UserTokens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RevokeAllUserTokensAsync(int userId)
        {
            var tokens = await _context.UserTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();
            _context.UserTokens.RemoveRange(tokens);
            await _context.SaveChangesAsync();
        }

        public async Task<UserToken?> GetByTokenAsync(string token)
        {
            return await _context.UserTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<UserToken> UpdateAsync(UserToken userToken)
        {
            _context.UserTokens.Update(userToken);
            await _context.SaveChangesAsync();
            return userToken;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.UserTokens.FindAsync(id);
            if (entity != null)
            {
                _context.UserTokens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var tokens = await _context.UserTokens
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if (tokens.Any())
            {
                _context.UserTokens.RemoveRange(tokens);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<UserToken>> GetUserActiveTokensAsync(int userId)
        {
            return await _context.UserTokens
                .Where(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();
        }
    }
}
