using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ZA_Membership.Security
{
    public interface ICurrentUserService
    {
        int? UserId { get; }
        string? Username { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        List<string> Roles { get; }
        List<string> Permissions { get; }
        bool HasPermission(string permission);
        bool IsInRole(string role);
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId => GetClaimValue<int?>(ClaimTypes.NameIdentifier, value =>
            int.TryParse(value, out var id) ? id : null);

        public string? Username => GetClaimValue<string?>(ClaimTypes.Name);

        public string? Email => GetClaimValue<string?>(ClaimTypes.Email);

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public List<string> Roles => _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)?
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

        public List<string> Permissions => _httpContextAccessor.HttpContext?.User?
            .FindAll("permission")?
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

        public bool HasPermission(string permission)
        {
            return Permissions.Contains(permission);
        }

        public bool IsInRole(string role)
        {
            return Roles.Contains(role);
        }

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


