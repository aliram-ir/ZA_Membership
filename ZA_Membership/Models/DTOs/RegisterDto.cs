using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Models.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? NationalCode { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}