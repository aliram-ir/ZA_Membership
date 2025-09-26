using System.ComponentModel.DataAnnotations;

namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for refreshing JWT tokens.
    /// </summary>
    public class RefreshTokenDto
    {
        /// <summary>
        /// The refresh token used to obtain a new JWT.
        /// </summary>
        [Required]
        [MinLength(10, ErrorMessageResourceName = "RefreshToken_Invalid")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}