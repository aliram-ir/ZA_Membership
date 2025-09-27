namespace ZA_Membership.Models.Enums
{
    public enum AttemptType
    {
        /// <summary>
        /// A login attempt using username/password.
        /// </summary>
        Login,

        /// <summary>
        /// An attempt to verify an OTP code.
        /// </summary>
        OtpVerification,

        /// <summary>
        /// A password reset request.
        /// </summary>
        PasswordResetRequest,

        /// <summary>
        /// A user registration attempt.
        /// </summary>
        Registration
    }
}
