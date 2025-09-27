using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for managing UserToken entities.
    /// </summary>
    internal interface IUserTokenRepository
    {
        /// <summary>
        /// دریافت توکن کاربر بر اساس شناسه
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserToken?> GetByTokenAsync(string token);

        /// <summary>
        /// ایجاد توکن جدید برای کاربر
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        Task<UserToken> CreateAsync(UserToken userToken);

        /// <summary>
        /// به‌روزرسانی اطلاعات توکن کاربر
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        Task<UserToken> UpdateAsync(UserToken userToken);

        /// <summary>
        /// حذف توکن کاربر بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// حذف همه توکن‌های یک کاربر بر اساس شناسه کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteByUserIdAsync(int userId);

        /// <summary>
        /// بررسی وجود توکن کاربر بر اساس شناسه
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task RevokeTokenAsync(string token);

        /// <summary>
        /// ابطلی همه توکن‌های فعال یک کاربر بر اساس شناسه کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RevokeAllUserTokensAsync(int userId);

        /// <summary>
        /// دریافت همه توکن‌های فعال یک کاربر بر اساس شناسه کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserToken>> GetUserActiveTokensAsync(int userId);
    }
}