namespace ZA_Membership.Models.Enums
{
    public enum UserActivityType
    {
        /// <summary>
        /// A successful login event.
        /// </summary>
        Login,

        /// <summary>
        /// A logout event.
        /// </summary>
        Logout,

        /// <summary>
        /// A failed login attempt.
        /// </summary>
        LoginFailed,

        /// <summary>
        /// An issued refresh token event.
        /// </summary>
        RefreshTokenIssued,

        /// <summary>
        /// A used refresh token event.
        /// </summary>
        RefreshTokenUsed
    }
}
