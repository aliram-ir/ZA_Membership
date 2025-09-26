namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for updating user information.
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string? Email { get; set; }
    }
}