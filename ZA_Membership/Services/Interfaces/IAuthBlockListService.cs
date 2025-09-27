using ZA_Membership.Models.Entities;

namespace ZA_Membership.Services.Interfaces
{
    public interface IAuthBlockListService
    {
        /// <summary>
        /// دریافت بر اساس شناسه
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AuthBlockList?> GetByIdAsync(int id);

        /// <summary>
        /// دریافت بر اساس شناسه کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AuthBlockList?> GetByUserIdAsync(int userId);

        /// <summary>
        /// افزودن مورد جدید
        /// </summary>
        /// <param name="blockEntry"></param>
        /// <returns></returns>
        Task<AuthBlockList> CreateAsync(AuthBlockList blockEntry);

        /// <summary>
        /// ویرایش ردیف
        /// </summary>
        /// <param name="blockEntry"></param>
        /// <returns></returns>
        Task<AuthBlockList> UpdateAsync(AuthBlockList blockEntry);

        /// <summary>
        /// حذف ردیف
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// حذف بر اساس شناسه کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteByUserIdAsync(int userId);
    }
}
