namespace ZA_Membership.Configuration
{
    /// <summary>
    /// Configuration options for security-related settings.
    /// </summary>
    public class SecurityOptions
    {
        /// <summary>
        /// The maximum number of failed access attempts before a user account is locked out.
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; } = 5;

        /// <summary>
        /// The time span (in minutes) for which a user account remains locked out after reaching the maximum failed access attempts.
        /// </summary>
        public int LockoutTimeSpanMinutes { get; set; } = 30;

        /// <summary>
        /// Indicates whether two-factor authentication is required for user accounts.
        /// </summary>
        public bool RequireTwoFactor { get; set; } = false;
    }
}