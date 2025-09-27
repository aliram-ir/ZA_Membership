using System.ComponentModel.DataAnnotations.Schema;
using ZA_Membership.Models.Enums;

namespace ZA_Membership.Models.Entities
{
    public class UserActivity
    {
        /// <summary>
        /// The type of activity (e.g., Login, Logout, LoginFailed).
        /// </summary>
        public UserActivityType ActivityType { get; private set; }

        /// <summary>
        /// The ID of the user associated with the activity. Can be null for failed login attempts where the user is not identified.
        /// </summary>
        public int UserId { get; private set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; private set; }

        /// <summary>
        /// The username or email used in the attempt, especially useful for failed logins.
        /// </summary>
        public string UsernameAttempted { get; set; }

        public string IpAddress { get; set; }

        /// <summary>
        /// Information about the client's User-Agent string.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Details about the browser, parsed from UserAgent. e.g., "Chrome 105.0".
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// Details about the operating system, parsed from UserAgent. e.g., "Windows 11".
        /// </summary>
        public string? OperatingSystem { get; set; }

        /// <summary>
        /// Details about the device, parsed from UserAgent. e.g., "PC", "iPhone".
        /// </summary>
        public string? Device { get; set; }

        /// <summary>
        /// Indicates if the activity was successful. e.g., true for Login, false for LoginFailed.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Timestamp of the activity.
        /// </summary>
        public DateTime ActivityTime { get; set; }

        /// <summary>
        /// Optional details or reason for the activity, e.g., "Invalid password", "User logged out".
        /// </summary>
        public string? Details { get; set; }
    }
}
