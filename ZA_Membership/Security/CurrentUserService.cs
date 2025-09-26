using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZA_Membership.Security
{
    /// <summary>
    /// اینترفیس سرویس کاربر جاری که اطلاعات کاربر احراز هویت شده را فراهم می‌کند.
    /// Interface for the current user service that provides information about the authenticated user.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// شناسه کاربر جاری یا null اگر کاربر احراز هویت نشده باشد.
        /// The current user's ID or null if the user is not authenticated.
        /// </summary>
        int? UserId { get; }

        /// <summary>
        /// نام کاربری کاربر جاری یا null اگر کاربر احراز هویت نشده باشد.
        /// The current user's username or null if the user is not authenticated.
        /// </summary>
        string? Username { get; }

        /// <summary>
        /// ایمیل کاربر جاری یا null اگر کاربر احراز هویت نشده باشد.
        /// The current user's email or null if the user is not authenticated.
        /// </summary>
        string? Email { get; }

        /// <summary>
        /// مشخص می‌کند که آیا کاربر احراز هویت شده است یا خیر.
        /// Indicates whether the user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// لیست نقش‌های کاربر جاری.
        /// List of roles for the current user.
        /// </summary>
        List<string> Roles { get; }

        /// <summary>
        /// لیست مجوزهای کاربر جاری.
        /// List of permissions for the current user.
        /// </summary>
        List<string> Permissions { get; }

        /// <summary>
        /// بررسی می‌کند که آیا کاربر جاری دارای مجوز مشخص شده است یا خیر.
        /// Checks if the current user has the specified permission.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        bool HasPermission(string permission);

        /// <summary>
        /// بررسی می‌کند که آیا کاربر جاری دارای نقش مشخص شده است یا خیر.
        /// Checks if the current user has the specified role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool IsInRole(string role);
    }

    /// <summary>
    /// پیاده‌سازی سرویس کاربر جاری که اطلاعات کاربر احراز هویت شده را از HttpContext استخراج می‌کند.
    /// Implementation of the current user service that extracts authenticated user information from HttpContext.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// ایجاد یک نمونه جدید از سرویس کاربر جاری با استفاده از IHttpContextAccessor برای دسترسی به HttpContext.
        /// Creates a new instance of the current user service using IHttpContextAccessor to access HttpContext.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc/>
        public int? UserId => GetClaimValue<int?>(ClaimTypes.NameIdentifier, value =>
            int.TryParse(value, out var id) ? id : null);
        /// <inheritdoc/>
        public string? Username => GetClaimValue<string?>(ClaimTypes.Name);
        /// <inheritdoc/>
        public string? Email => GetClaimValue<string?>(ClaimTypes.Email);
        /// <inheritdoc/>
        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        /// <inheritdoc/>
        public List<string> Roles => _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)?
            .Select(c => c.Value)
            .ToList() ?? new List<string>();
        /// <inheritdoc/>
        public List<string> Permissions => _httpContextAccessor.HttpContext?.User?
            .FindAll("permission")?
            .Select(c => c.Value)
            .ToList() ?? new List<string>();
        /// <inheritdoc/>
        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }
        /// <inheritdoc/>
        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }
        /// <inheritdoc/>
        private T GetClaimValue<T>(string claimType, Func<string, T>? converter = null)
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst(claimType);
            if (claim?.Value == null)
                return default(T)!;

            if (converter != null)
                return converter(claim.Value);

            return (T)Convert.ChangeType(claim.Value, typeof(T));
        }
    }
}


