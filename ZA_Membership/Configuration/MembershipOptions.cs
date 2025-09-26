namespace ZA_Membership.Configuration
{
    /// <summary>
    /// Configuration options for the ZA_Membership system, including JWT, password, user, and security settings.
    /// تنظیمات مربوط به سیستم ZA_Membership، شامل JWT، رمز عبور، کاربر و تنظیمات امنیتی.
    /// این کلاس به عنوان یک نقطه مرکزی برای مدیریت تنظیمات مختلف مربوط به عضویت کاربران عمل می‌کند.
    /// اگر این تنظیمات به درستی پیکربندی شوند، می‌توانند به بهبود امنیت، تجربه کاربری و مدیریت کاربران کمک کنند.
    /// </summary>
    public class MembershipOptions
    {
        /// <summary>
        /// Configuration options for JWT (JSON Web Token) authentication.
        /// </summary>
        public JwtOptions Jwt { get; set; } = new();

        /// <summary>
        /// Configuration options for password policies and settings.
        /// </summary>
        public PasswordOptions Password { get; set; } = new();

        /// <summary>
        /// Configuration options for user-related settings.
        /// </summary>
        public UserOptions User { get; set; } = new();

        /// <summary>
        /// Configuration options for security-related settings.
        /// </summary>
        public SecurityOptions Security { get; set; } = new();
    }
}