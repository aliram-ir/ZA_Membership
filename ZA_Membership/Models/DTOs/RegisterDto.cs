namespace ZA_Membership.Models.DTOs
{
    /// <summary>
    /// شی انتقال داده برای ثبت نام کاربر جدید.
    /// Data Transfer Object for registering a new user.
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// نام کاربری منحصر به فرد برای کاربر.
        /// A unique username for the user.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// آدرس ایمیل کاربر.
        /// The user's email address.
        /// </summary>
        public string? Email { get; set; } = string.Empty;

        /// <summary>
        /// کد ملی کاربر (اختیاری).
        /// The user's national code (optional).
        /// </summary>
        public string? NationalCode { get; set; } = string.Empty;

        /// <summary>
        /// رمز عبور کاربر.
        /// The user's password.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// نام کوچک کاربر (اختیاری).
        /// The user's first name (optional).
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// نام خانوادگی کاربر (اختیاری).
        /// The user's last name (optional).
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// شماره تلفن کاربر (اختیاری).
        /// The user's phone number (optional).
        /// </summary>
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public DateTime? Birthday { get; set; }
    }
}