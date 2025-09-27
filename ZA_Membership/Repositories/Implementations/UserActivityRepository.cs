using Microsoft.EntityFrameworkCore;
using ZA_Membership.Data;
using ZA_Membership.Models.Entities;
using ZA_Membership.Repositories.Interfaces;

namespace ZA_Membership.Repositories.Implementations
{
    internal class UserActivityRepository : IUserActivityRepository
    {
        private readonly MembershipDbContext _context;

        public UserActivityRepository(MembershipDbContext context)
        {
            _context = context;
        }

        public async Task<UserActivity?> GetByIdAsync(int id)
        {
            return await _context.UserActivities.FindAsync(id);
        }

        public async Task<List<UserActivity>> GetByUserIdAsync(int userId)
        {
            return await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.ActivityTime)
                .ToListAsync();
        }

        public async Task<List<UserActivity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.UserActivities
                .Where(ua => ua.ActivityTime >= startDate && ua.ActivityTime <= endDate)
                .OrderByDescending(ua => ua.ActivityTime)
                .ToListAsync();
        }

        public async Task<UserActivity> CreateAsync(UserActivity activity)
        {
            _context.UserActivities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<UserActivity> UpdateAsync(UserActivity activity)
        {
            _context.UserActivities.Update(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.UserActivities.FindAsync(id);
            if (entity != null)
            {
                _context.UserActivities.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByUserIdAsync(int userId)
        {
            var entities = await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .ToListAsync();

            if (entities.Any())
            {
                _context.UserActivities.RemoveRange(entities);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.UserActivities.AnyAsync(ua => ua.UserId == id);
        }
    }
}
