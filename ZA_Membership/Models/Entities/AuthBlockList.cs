using System.ComponentModel.DataAnnotations.Schema;

namespace ZA_Membership.Models.Entities
{
    public class AuthBlockList
    {
        /// <summary>
        /// The ID of the user to block. This is the most specific block.
        /// Can be null if the block is based on other identifiers.
        /// </summary>
        public int? UserId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; private set; }

        /// <summary>
        /// The IP address to block. Effective against automated attacks from a single source.
        /// Can be null if the block targets a user or other identifiers.
        /// </summary>
        public string? IpAddress { get; private set; }

        /// <summary>
        /// The email address to block. Useful for preventing registrations or password resets for a specific email.
        /// Should be stored in a normalized (e.g., uppercase) format.
        /// </summary>
        public string? Email { get; private set; }

        /// <summary>
        /// The phone number to block. Useful for preventing registrations or OTP requests for a specific number.
        /// </summary>
        public string? PhoneNumber { get; private set; }

        /// <summary>
        /// A clear, mandatory reason for the block (e.g., "Too many failed registration attempts from this IP").
        /// Essential for auditing and diagnostics.
        /// </summary>
        public string Reason { get; private set; }

        /// <summary>
        /// The UTC date and time when the block automatically expires.
        /// If null, the block is permanent until manually removed.
        /// </summary>
        public DateTime? BlockDate { get; private set; }
        public DateTime? BlockExpiresAt { get; private set; }
    }
}
