using ZA_Membership.Models.DTOs;
using ZA_Membership.Models.Results;

namespace ZA_Membership.Services.Interfaces
{
    public interface IMembershipService
    {
        /// <summary>
        /// ثبت نام کاربر جدید
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        Task<AuthResult> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// لاگین کاربران
        /// </summary>
        /// <param name="loginDto"></param>
        /// <param name="ipAddress"></param>
        /// <param name="deviceInfo"></param>
        /// <returns></returns>
        Task<AuthResult> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? deviceInfo = null);
        
        /// <summary>
        /// خروج کاربر
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<ServiceResult> LogoutAsync(string token);
        
        /// <summary>
        /// خروج از همه دستگاه‌ها
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ServiceResult> LogoutAllDevicesAsync(int userId);
        
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task<ServiceResult<UserDto>> GetUserAsync(int userId);
        Task<ServiceResult<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto);
        Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<ServiceResult> DeactivateUserAsync(int userId);
        Task<ServiceResult> ActivateUserAsync(int userId);
    }
}
