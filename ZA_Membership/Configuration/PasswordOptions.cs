using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Configuration
{
    public class PasswordOptions
    {
        public int MinimumLength { get; set; } = 8;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireSpecialCharacter { get; set; } = true;
    }
}