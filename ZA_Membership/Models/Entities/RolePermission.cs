namespace ZA_Membership.Models.Entities
{
    /// <summary>
    /// Represents the association between a Role and a Permission.
    /// </summary>
    public class RolePermission
    {
        /// <summary>
        /// The unique identifier for the RolePermission entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the associated Role.
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// The ID of the associated Permission.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Indicates whether the RolePermission entry is currently active.
        /// </summary>
        // Navigation Properties
        public virtual Role Role { get; set; } = null!;

        /// <summary>
        /// The associated Permission entity.
        /// </summary>
        public virtual Permission Permission { get; set; } = null!;
    }
}