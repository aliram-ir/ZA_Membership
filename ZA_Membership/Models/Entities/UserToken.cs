using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Models.Entities
{
    public class UserToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty; // "AccessToken", "RefreshToken", "EmailConfirmation", etc.
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public string? DeviceInfo { get; set; }
        public string? IpAddress { get; set; }

        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}