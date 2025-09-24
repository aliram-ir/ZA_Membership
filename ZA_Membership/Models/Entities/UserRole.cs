namespace ZA_Membership.Models.Entities
{
    /// <summary>
    /// Entity representing the association between users and roles.
    /// </summary>
    public class UserRole
    {
        /// <summary>
        /// The unique identifier for the UserRole entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the user associated with the role.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// The ID of the role assigned to the user.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The date and time when the role was assigned to the user.
        /// </summary>
        public DateTime AssignedAt { get; set; }

        // Navigation Properties
        /// <summary>
        /// The user associated with this UserRole entry.
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// The role associated with this UserRole entry.
        /// </summary>
        public virtual Role Role { get; set; } = null!;
    }
}