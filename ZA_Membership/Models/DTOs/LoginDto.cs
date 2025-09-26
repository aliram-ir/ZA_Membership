namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for user login.
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// The username of the user attempting to log in.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The password of the user attempting to log in.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the user wants to be remembered on the device.
        /// </summary>
        public bool RememberMe { get; set; } = false;
    }
}