using System.ComponentModel.DataAnnotations;
using ZA_Membership.Resources;

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
        [Required(ErrorMessageResourceName = "RefreshToken_Required", ErrorMessageResourceType = typeof(Messages))]
        [MinLength(10, ErrorMessageResourceName = "RefreshToken_Invalid", ErrorMessageResourceType = typeof(Messages))]
        public string RefreshToken { get; set; } = string.Empty;
    }
}