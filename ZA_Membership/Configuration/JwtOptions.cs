using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Configuration
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpiryMinutes { get; set; } = 60;
        public int RefreshTokenExpiryDays { get; set; } = 30;
    }
}