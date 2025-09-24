using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZA_Membership.Models.DTOs;

namespace ZA_Membership.Models.Results
{
    public class AuthResult
    {
        public bool IsSuccess { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public UserDto? User { get; set; }
        public List<string> Errors { get; set; } = new();
        public string? Message { get; set; }
    }
}