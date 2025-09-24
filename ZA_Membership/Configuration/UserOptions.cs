using ZA_Membership.Models.Enums;

namespace ZA_Membership.Configuration
{
    /// <summary>
    /// Configuration options for user-related settings.
    /// تنظیمات مربوط به کاربران مانند الزامات ایمیل و نام کاربری.
    /// اگر این گزینه‌ها به درستی تنظیم شوند، می‌توانند به بهبود امنیت و مدیریت کاربران کمک کنند.
    /// برای مثال، الزام به یکتایی ایمیل می‌تواند از ایجاد چندین حساب با یک ایمیل جلوگیری کند.
    /// همچنین، اگر تأیید ایمیل یا شماره تلفن فعال باشد، کاربران باید قبل از ورود به سیستم، ایمیل یا شماره تلفن خود را تأیید کنند که این امر می‌تواند امنیت حساب را افزایش دهد.
    /// اگر محدودیت‌های کاراکترهای نام کاربری فعال باشد، می‌توان از استفاده از کاراکترهای غیرمجاز جلوگیری کرد که این نیز می‌تواند به امنیت و یکپارچگی سیستم کمک کند.
    /// </summary>
    public class UserOptions
    {
        /// <summary>
        /// Specifies the strategy for generating or validating user names.
        /// استراتژی تولید یا اعتبارسنجی نام‌های کاربری را مشخص می‌کند
        /// </summary>
        public UserNameEnum UserNameStrategy { get; set; } = UserNameEnum.All;

        /// <summary>
        /// Indicates whether each user must have a unique email address.
        /// </summary>
        public bool RequireUniqueEmail { get; set; } = true;

        /// <summary>
        /// Indicates whether email confirmation is required before a user can sign in.
        /// </summary>
        public bool RequireEmailConfirmation { get; set; } = false;

        /// <summary>
        /// Indicates whether phone number confirmation is required before a user can sign in.
        /// </summary>
        public bool RequirePhoneNumberConfirmation { get; set; } = false;

        /// <summary>
        /// Indicates whether to enforce restrictions on allowed characters in usernames.
        /// اگر این گزینه فعال باشد، نام‌های کاربری باید فقط شامل حروف الفبا، اعداد و برخی کاراکترهای خاص باشند.
        /// </summary>
        public bool AllowedUserNameCharacters { get; set; } = true;
    }
}