namespace ZA_Membership.Models.Entities
{
    /// <summary>
    /// Represents a role that can be assigned to users for access control.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// The unique identifier for the role.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the role.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A brief description of the role and its purpose.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates whether the role is currently active and can be assigned to users.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// The date and time when the role was created.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        /// <summary>
        /// The users that are assigned to this role.
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; } = [];

        /// <summary>
        /// The permissions that are assigned to this role.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}