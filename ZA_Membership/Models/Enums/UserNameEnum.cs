namespace ZA_Membership.Models.Enums
{
    /// <summary>
    /// Enumeration for different types of user name generation strategies.
    /// </summary>
    public enum UserNameEnum
    {
        /// <summary>
        /// Indicates a custom user name provided by the user.
        /// </summary>
        Custom,

        /// <summary>
        /// Indicates that the user name will be generated based on the user's email address.
        /// </summary>
        Email,

        /// <summary>
        /// Indicates that the user name will be generated based on the user's phone number.
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// Indicates that the user name will be generated based on the user's national code.
        /// </summary>
        NationalCode,

        /// <summary>
        /// Indicates that all user name generation strategies are allowed.
        /// </summary>
        All,

        /// <summary>
        /// Indicates that the user name will be generated randomly.
        /// </summary>
        Random
    }
}