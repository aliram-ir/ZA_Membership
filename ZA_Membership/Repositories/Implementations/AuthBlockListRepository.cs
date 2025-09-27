using Microsoft.EntityFrameworkCore;
using ZA_Membership.Data;
using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;

namespace ZA_Membership.Repositories.Implementations
{
    internal class AuthBlockListRepository : IAuthBlockListRepository
    {
        private readonly MembershipDbContext _context;

        internal AuthBlockListRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<AuthBlockList?> GetByIdAsync(int id)
        {
            return await _context.AuthBlockLists.FindAsync(id);
        }


        public async Task<AuthBlockList> CreateAsync(AuthBlockList blockEntry)
        {
            _context.AuthBlockLists.Add(blockEntry);
            await _context.SaveChangesAsync();
            return blockEntry;
        }

        public async Task<AuthBlockList> UpdateAsync(AuthBlockList blockEntry)
        {
            _context.AuthBlockLists.Update(blockEntry);
            await _context.SaveChangesAsync();
            return blockEntry;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.AuthBlockLists.FindAsync(id);
            if (entity != null)
            {
                _context.AuthBlockLists.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var entities = await _context.AuthBlockLists
                .Where(bl => bl.UserId == userId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.AuthBlockLists.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<AuthBlockList?> GetByUserIdAsync(int userId)
        {
            return await _context.AuthBlockLists.SingleOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
