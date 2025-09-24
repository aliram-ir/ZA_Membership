namespace ZA_Membership.Models.Entities
{
    /// <summary>
    /// Entity representing tokens associated with users for various purposes (e.g., authentication, email confirmation).
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// The unique identifier for the UserToken entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user associated with the token.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The token string used for authentication or other purposes.
        /// </summary>
        public string Token { get; set; } = string.Empty;

       /// <summary>
       /// The type of the token (e.g., "AccessToken", "RefreshToken", "EmailConfirmation").
       /// </summary>
        public string TokenType { get; set; } = string.Empty; // "AccessToken", "RefreshToken", "EmailConfirmation", etc.

        /// <summary>
        /// The date and time when the token was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when the token expires.
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Indicates whether the token has been revoked.
        /// </summary>
        public bool IsRevoked { get; set; } = false;

        /// <summary>
        /// Optional information about the device from which the token was issued.
        /// </summary>
        public string? DeviceInfo { get; set; }

        /// <summary>
        /// Optional IP address from which the token was issued.
        /// </summary>
        public string? IpAddress { get; set; }

        // Navigation Properties
        /// <summary>
        /// The user associated with this token.
        /// </summary>
        public virtual User User { get; set; } = null!;
    }
}