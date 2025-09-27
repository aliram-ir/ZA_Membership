using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for managing Role entities.
    /// </summary>
    internal interface IRoleRepository
    {
        /// <summary>
        /// دریافت نقش بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Role?> GetByIdAsync(int id);

        /// <summary>
        /// دریافت نقش بر اساس نام
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Role?> GetByNameAsync(string name);

        /// <summary>
        /// دریافت همه نقش‌ها
        /// </summary>
        /// <returns></returns>
        Task<List<Role>> GetAllAsync();

        /// <summary>
        /// ایجاد نقش جدید
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<Role> CreateAsync(Role role);

        /// <summary>
        /// به‌روزرسانی اطلاعات نقش
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<Role> UpdateAsync(Role role);

        /// <summary>
        /// حذف نقش بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// بررسی وجود نقش بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(int id);

        /// <summary>
        /// بررسی وجود نقش بر اساس نام
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistsByNameAsync(string name);
    }
}