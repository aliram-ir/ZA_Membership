using ZA_Membership.Models.DTOs;
using ZA_Membership.Models.Entities;
using ZA_Membership.Models.Results;

namespace ZA_Membership.Services.Interfaces
{
    /// <summary>
    /// فارسی: اینترفیس سرویس عضویت و احراز هویت
    /// English: Membership and Authentication Service Interface
    /// </summary>
    public interface IMembershipService
    {
        /// <summary>
        /// ثبت نام کاربر جدید با اطلاعات داده شده.
        /// Registers a new user with the given information.
        /// </summary>
        /// <param name="registerDto">The user data transfer object containing registration info.</param>
        /// <returns>Returns true if registration was successful, false otherwise.</returns>
        Task<AuthResult> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// فارسی: احراز هویت کاربر و بازگشت نتیجه احراز هویت شامل توکن‌ها و اطلاعات کاربر.
        /// Authenticates a user and returns an authentication result containing tokens and user info.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <param name="ipAddress"></param>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        Task<AuthResult> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? deviceInfo = null);

        /// <summary>
        /// فارسی: خروج کاربر با نامعتبر کردن توکن ارائه شده.
        /// Logs out a user by invalidating the provided token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ServiceResult> LogoutAsync(string token);

        /// <summary>
        /// خروج کاربر از همه دستگاه‌ها با نامعتبر کردن تمام توکن‌های فعال کاربر.
        /// Logs out a user from all devices by invalidating all active tokens for the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult> LogoutAllDevicesAsync(int userId);

        /// <summary>
        /// تازه‌سازی توکن با استفاده از توکن تازه‌سازی ارائه شده.
        /// Refreshes the authentication token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        Task<AuthResult> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// دریافت اطلاعات کاربر با شناسه داده شده.
        /// Retrieves user information by the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<UserDto>> GetUserAsync(int userId);

        /// <summary>
        /// به‌روزرسانی اطلاعات کاربر با شناسه داده شده.
        /// Updates user information for the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateDto"></param>
        /// <returns></returns>
        Task<ServiceResult<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto);

        /// <summary>
        /// تغییر رمز عبور کاربر با شناسه داده شده.
        /// Changes the password for the user with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="changePasswordDto"></param>
        /// <returns></returns>
        Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);

        /// <summary>
        /// غیرفعال‌سازی حساب کاربری با شناسه داده شده.
        /// Deactivates the user account with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult> DeactivateUserAsync(int userId);

        /// <summary>
        /// فعال‌سازی حساب کاربری با شناسه داده شده.
        /// Activates the user account with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult> ActivateUserAsync(int userId);

        /// <summary>
        /// اختصاص نقش به کاربر با شناسه داده شده.
        /// Assigns a role to the user with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<ServiceResult> AssignRoleAsync(int userId, string roleName);

        /// <summary>
        /// حذف نقش از کاربر با شناسه داده شده.
        /// Removes a role from the user with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<ServiceResult> RemoveRoleAsync(int userId, string roleName);

        /// <summary>
        /// دریافت نقش‌های کاربر با شناسه داده شده.
        /// Retrieves the roles of the user with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId);

        /// <summary>
        /// دریافت مجوزهای کاربر با شناسه داده شده.
        /// Retrieves the permissions of the user with the given user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult<List<string>>> GetUserPermissionsAsync(int userId);

        /// <summary>
        /// بررسی اینکه آیا کاربر با شناسه داده شده دارای مجوز مشخص شده است یا خیر.
        /// Checks if the user with the given user ID has the specified permission.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> HasPermissionAsync(int userId, string permission);

        /// <summary>
        /// بررسی اینکه آیا کاربر با شناسه داده شده دارای نقش مشخص شده است یا خیر.
        /// Checks if the user with the given user ID has the specified role.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<ServiceResult<bool>> IsInRoleAsync(int userId, string roleName);

        Task<Address> AddUserAddressAsync(int userId, Address address);
        Task<List<Address>> GetUserAddressesAsync(int userId);


    }
}