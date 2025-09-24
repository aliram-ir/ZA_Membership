using ZA_Membership.Models.DTOs;
using ZA_Membership.Models.Results;

namespace ZA_Membership.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<AuthResult> RegisterAsync(RegisterDto registerDto);
        Task<AuthResult> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? deviceInfo = null);
        Task<ServiceResult> LogoutAsync(string token);
        Task<ServiceResult> LogoutAllDevicesAsync(int userId);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task<ServiceResult<UserDto>> GetUserAsync(int userId);
        Task<ServiceResult<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto);
        Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<ServiceResult> DeactivateUserAsync(int userId);
        Task<ServiceResult> ActivateUserAsync(int userId);
        Task<ServiceResult> AssignRoleAsync(int userId, string roleName);
        Task<ServiceResult> RemoveRoleAsync(int userId, string roleName);
        Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId);
        Task<ServiceResult<List<string>>> GetUserPermissionsAsync(int userId);
        Task<ServiceResult<bool>> HasPermissionAsync(int userId, string permission);
        Task<ServiceResult<bool>> IsInRoleAsync(int userId, string roleName);
    }
}