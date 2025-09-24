using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Configuration
{
    public class UserOptions
    {
        public bool RequireUniqueEmail { get; set; } = true;
        public bool RequireEmailConfirmation { get; set; } = false;
        public bool RequirePhoneNumberConfirmation { get; set; } = false;
        public bool AllowedUserNameCharacters { get; set; } = true;
    }
}