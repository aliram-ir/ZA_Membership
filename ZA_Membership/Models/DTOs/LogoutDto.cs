using System.ComponentModel.DataAnnotations;

namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for user logout.
    /// </summary>
    public class LogoutDto
    {
        /// <summary>
        /// The refresh token to be invalidated during logout.
        /// </summary>
        [Required]
        public LogoutDto(string refreshToken) 
        {
            this.RefreshToken = refreshToken;
   
        }
                public string RefreshToken { get; set; } = string.Empty;
    }
}