using ZA_Membership.Models.Entities;

namespace ZA_Membership.Services.Interfaces
{
    /// <summary>
    /// Repository interface for managing User entities.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// دریافت کاربر بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// دریافت کاربر بر اساس نام کاربری
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// دریافت کاربر بر اساس ایمیل
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// دریافت کاربر بر اساس نام کاربری یا ایمیل
        /// </summary>
        /// <param name="usernameOrEmail"></param>
        /// <returns></returns>
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);

        /// <summary>
        /// ایجاد کاربر جدید
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateAsync(User user);

        /// <summary>
        /// به‌روزرسانی اطلاعات کاربر
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> UpdateAsync(User user);

        /// <summary>
        /// حذف کاربر بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// بررسی وجود کاربر بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// بررسی وجود کاربر بر اساس نام کاربری
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<bool> ExistsByUsernameAsync(string username);

        /// <summary>
        /// بررسی وجود کاربر بر اساس ایمیل
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<bool> ExistsByEmailAsync(string email);

        /// <summary>
        /// دریافت نقش‌ها و مجوزهای کاربر بر اساس شناسه
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetUserRolesAsync(int userId);

        /// <summary>
        /// دریافت مجوزهای کاربر بر اساس شناسه
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetUserPermissionsAsync(int userId);
    }
}