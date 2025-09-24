using ZA_Membership.Models.DTOs;

namespace ZA_Membership.Models.Results
{
    /// <summary>
    /// Result of an authentication operation, including tokens and user info.
    /// </summary>
    public class AuthResult
    {
        /// <summary>
        /// Indicates whether the authentication was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// The access token issued upon successful authentication.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// The refresh token issued upon successful authentication.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// The expiration time of the access token.
        /// </summary>
        public DateTime? ExpiresAt { get; set; }

        /// <summary>
        /// The authenticated user's information.
        /// </summary>
        public UserDto? User { get; set; }

        /// <summary>
        /// A list of error messages if the authentication failed.
        /// </summary>
        public List<string> Errors { get; set; } = [];

        /// <summary>
        /// An optional message providing additional context about the authentication result.
        /// </summary>
        public string? Message { get; set; }
    }
}