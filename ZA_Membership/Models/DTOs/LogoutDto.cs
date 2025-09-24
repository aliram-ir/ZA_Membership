using System.ComponentModel.DataAnnotations;

namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for user logout.
    /// </summary>
    public class LogoutDto
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogoutDto"/> class.
        /// </summary>
        /// <param name="refreshToken"></param>
        public LogoutDto(string refreshToken)
        {
            this.RefreshToken = refreshToken;
        }

        /// <summary>
        /// The refresh token used to invalidate the user's session.
        /// </summary>
        [Required(ErrorMessageResourceName = "RefreshToken_Required", ErrorMessageResourceType = typeof(Resources.Messages))]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
