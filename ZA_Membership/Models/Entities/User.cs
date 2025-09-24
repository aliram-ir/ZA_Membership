using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZA_Membership.Models.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Username { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(10)]
        public string NationalCode { get; set; } = string.Empty;

        [MaxLength(50)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return $"{FirstName}  {LastName}"; }
        }
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public bool EmailConfirmed { get; set; } = false;
        public bool PhoneNumberConfirmed { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    }
}