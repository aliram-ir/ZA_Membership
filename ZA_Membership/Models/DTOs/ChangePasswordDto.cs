namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for changing a user's password.
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// The user's current password.
        /// </summary>
        public string CurrentPassword { get; set; } = string.Empty;

        /// <summary>
        /// The new password to set for the user.
        /// </summary>
        public string NewPassword { get; set; } = string.Empty;
    }
}