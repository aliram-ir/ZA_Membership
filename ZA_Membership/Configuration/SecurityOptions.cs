using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Configuration
{
    public class SecurityOptions
    {
        public int MaxFailedAccessAttempts { get; set; } = 5;
        public int LockoutTimeSpanMinutes { get; set; } = 30;
        public bool RequireTwoFactor { get; set; } = false;
    }
}