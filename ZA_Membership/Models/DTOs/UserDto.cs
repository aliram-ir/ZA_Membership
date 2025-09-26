namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// Data Transfer Object for user information.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// The email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// The national code of the user.
        /// </summary>
        public string NationalCode { get; set; } = string.Empty;

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
        /// Indicates whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Indicates whether the user's email has been confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Indicates whether the user's phone number has been confirmed.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// The date and time when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The date and time when the user last logged in.
        /// </summary>
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// The roles assigned to the user.
        /// </summary>
        public List<string> Roles { get; set; } = [];

        /// <summary>
        /// The permissions granted to the user.
        /// </summary>
        public List<string> Permissions { get; set; } = [];
    }
}