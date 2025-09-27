using System.ComponentModel.DataAnnotations;
using ZA_Membership.Models.Enums;

namespace ZA_Membership.Models.Entities
{
    public class AuthFailedAttempt
    {
        public int Id { get; set; }

        /// <summary>
        /// The type of action that failed (e.g., Login, OtpVerification).
        /// </summary>
        public AttemptType AttemptType { get; set; }

        /// <summary>
        /// A normalized identifier for the subject of the attempt.
        /// For Login: Normalized Username/Email.
        /// For OTP: UserID.
        /// For Registration: IP Address or Email.
        /// </summary>
        [Required]
        [StringLength(256)]
        public string SubjectIdentifier { get; set; }

        /// <summary>
        /// The IP address from which the attempt was made.
        /// </summary>
        [Required]
        [StringLength(45)] // Accommodates IPv6
        public string IpAddress { get; set; }

        /// <summary>
        /// The timestamp of the failed attempt.
        /// </summary>
        public DateTime AttemptTime { get; set; }
    }
}
