namespace ZA_Membership.Models.Entities
{
    /// <summary>
    /// Represents a permission that can be assigned to roles for access control.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// The unique identifier for the permission.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the permission.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A brief description of what the permission allows.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The category of the permission for organizational purposes.
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Indicates whether the permission is currently active and can be assigned.
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        /// <summary>
        /// The roles that have this permission assigned.
        /// </summary>
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}