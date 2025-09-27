using ZA_Membership.Models.Entities;

namespace ZA_Membership.Repositories.Interfaces
{
    /// <summary>
    /// رابط ریپازیتوری برای مدیریت آدرس‌های کاربران.
    /// Repository interface for managing user addresses.
    /// </summary>
    internal interface IAddressRepository
    {
        /// <summary>
        /// دریافت آدرس با شناسه یکتا
        /// Get an address by its unique ID.
        /// </summary>
        Task<Address?> GetByIdAsync(int id);

        /// <summary>
        /// دریافت همه آدرس‌های کاربر
        /// Get all addresses for a user by user ID.
        /// </summary>
        Task<List<Address>> GetByUserIdAsync(int userId);

        /// <summary>
        /// دریافت آدرس پیش‌فرض کاربر
        /// Get the default address of a user.
        /// </summary>
        Task<Address?> GetDefaultAddressAsync(int userId);

        /// <summary>
        /// ساخت آدرس جدید
        /// Create a new address.
        /// </summary>
        Task<Address> CreateAsync(Address address);

        /// <summary>
        /// بروزرسانی آدرس
        /// Update an existing address.
        /// </summary>
        Task<Address> UpdateAsync(Address address);

        /// <summary>
        /// حذف آدرس با شناسه یکتا
        /// Delete an address by ID.
        /// </summary>
        Task DeleteAsync(int id);
    }
}
