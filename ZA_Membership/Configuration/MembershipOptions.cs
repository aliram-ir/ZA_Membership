using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Configuration
{
    public class MembershipOptions
    {
        public JwtOptions Jwt { get; set; } = new();
        public PasswordOptions Password { get; set; } = new();
        public UserOptions User { get; set; } = new();
        public SecurityOptions Security { get; set; } = new();
    }
}