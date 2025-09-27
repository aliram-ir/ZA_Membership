using System.ComponentModel.DataAnnotations.Schema;

namespace ZA_Membership.Models.Entities
{
    /// <summary>
    /// Represents a user in the system with authentication and profile details.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username chosen by the user for login purposes.
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
        /// The hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// The first name of the user.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Gets the full name of the user by combining first and last names.
        /// </summary>
        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName}  {LastName}"; }
        }

        /// <summary>
        /// The phone number of the user.
        /// </summary>
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Indicates whether the user account is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
        public bool IsVerify { get; set; } = false;
        public bool IsDelete { get; set; } = false;

        //TODO: Add IsLocked Field And Implement Lockout Mechanism
        //TODO: Add AccessFailedCount Field
        //TODO: Add LockoutEnd Field
        //TODO: Add TwoFactorEnabled Field
        //TODO: Add SecurityStamp Field
        //TODO: Add ConcurrencyStamp Field
        //TODO: Add Address Field

        /// <summary>
        /// Indicates whether the user's email has been confirmed.
        /// </summary>
        public bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// Indicates whether the user's phone number has been confirmed.
        /// </summary>
        public bool PhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// The date and time when the user account was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date and time when the user account was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// The date and time when the user last logged in.
        /// </summary>
        

        // Navigation Properties
        /// <summary>
        /// The roles associated with the user.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

        /// <summary>
        /// The refresh tokens associated with the user.
        /// </summary>
        public virtual ICollection<UserToken> UserTokens { get; set; } = [];
        public virtual ICollection<Address> Addresses { get; set; } = [];
        public virtual ICollection<UserActivity> UserActivity { get; set; } = [];
        public virtual ICollection<AuthBlockList> AuthBlockList { get; set; } = [];
    }
}