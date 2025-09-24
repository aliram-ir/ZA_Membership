namespace ZA_Membership.Configuration
{
    /// <summary>
    /// Configuration options for password policies and settings.
    /// </summary>
    public class PasswordOptions
    {
        /// <summary>
        /// The minimum length required for passwords.
        /// </summary>
        public int MinimumLength { get; set; } = 6;

        /// <summary>
        /// Indicates whether passwords must contain at least one uppercase letter.
        /// </summary>
        public bool RequireUppercase { get; set; } = true;

        /// <summary>
        /// Indicates whether passwords must contain at least one lowercase letter.
        /// </summary>
        public bool RequireLowercase { get; set; } = true;

        /// <summary>
        /// Indicates whether passwords must contain at least one digit.
        /// </summary>
        public bool RequireDigit { get; set; } = true;

        /// <summary>
        /// Indicates whether passwords must contain at least one special character (e.g., !, @, #, $, etc.).
        /// </summary>
        public bool RequireSpecialCharacter { get; set; } = true;
    }
}