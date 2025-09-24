namespace ZA_Membership.Configuration
{
    /// <summary>
    /// Configuration options for JWT (JSON Web Token) authentication.
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// The secret key used for signing JWTs. Must be kept secure and not hard-coded in production.
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;
        /// <summary>
        /// The issuer of the JWT, typically the application or service generating the token.
        /// </summary>
        public string Issuer { get; set; } = string.Empty;
        /// <summary>
        /// The intended audience of the JWT, typically the application or service that will consume the token.
        /// </summary>
        public string Audience { get; set; } = string.Empty;
        /// <summary>
        /// The expiration time for access tokens in minutes.
        /// </summary>
        public int AccessTokenExpiryMinutes { get; set; } = 60;
        /// <summary>
        /// The expiration time for refresh tokens in days.
        /// </summary>
        public int RefreshTokenExpiryDays { get; set; } = 30;
    }
}