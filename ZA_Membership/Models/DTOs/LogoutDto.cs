using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZA_Membership.Models.DTOs
{
    public class LogoutDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}