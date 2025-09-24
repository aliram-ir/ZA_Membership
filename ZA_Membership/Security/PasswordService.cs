using System.Globalization;
using System.Resources;
using BCrypt.Net;
using ZA_Membership.Configuration;

namespace ZA_Membership.Security
{
    /// <summary>
    /// Service for handling password hashing, verification, and validation.
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Hashes the specified password using a secure hashing algorithm.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies the specified password against the given hashed password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="hashedPassword"></param>
        /// <returns></returns>
        bool VerifyPassword(string password, string hashedPassword);

        /// <summary>
        /// Validates the specified password against the configured password policies.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        bool ValidatePassword(string password, out List<string> errors);
    }

    /// <summary>
    /// Implementation of the IPasswordService interface for handling password operations.
    /// </summary>
    public class PasswordService : IPasswordService
    {
        private readonly MembershipOptions _options;

        /// <summary>
        /// Initializes a new instance of the PasswordService class with the specified membership options.
        /// </summary>
        /// <param name="options"></param>
        public PasswordService(MembershipOptions options)
        {
            _options = options;
        }

        /// <inheritdoc/>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        /// <inheritdoc/>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                return false;
            }
        }
        /// <inheritdoc/>
        public bool ValidatePassword(string password, out List<string> errors)
        {
            errors = new List<string>();

            var rm = new ResourceManager("ZA_Membership.Resources.Messages",
                                         typeof(PasswordService).Assembly);
            var culture = CultureInfo.CurrentUICulture;

            string? msg;

            // حداقل طول
            msg = rm.GetString("Password_MinimumLength", culture);
            if (password.Length < _options.Password.MinimumLength && !string.IsNullOrEmpty(msg))
                errors.Add(string.Format(msg, _options.Password.MinimumLength));

            // حرف بزرگ
            msg = rm.GetString("Password_RequireUppercase", culture);
            if (_options.Password.RequireUppercase && !password.Any(char.IsUpper) && !string.IsNullOrEmpty(msg))
                errors.Add(msg);

            // حرف کوچک
            msg = rm.GetString("Password_RequireLowercase", culture);
            if (_options.Password.RequireLowercase && !password.Any(char.IsLower) && !string.IsNullOrEmpty(msg))
                errors.Add(msg);

            // عدد
            msg = rm.GetString("Password_RequireDigit", culture);
            if (_options.Password.RequireDigit && !password.Any(char.IsDigit) && !string.IsNullOrEmpty(msg))
                errors.Add(msg);

            // کاراکتر خاص
            msg = rm.GetString("Password_RequireSpecialChar", culture);
            if (_options.Password.RequireSpecialCharacter && !password.Any(ch => !char.IsLetterOrDigit(ch)) && !string.IsNullOrEmpty(msg))
                errors.Add(msg);

            return errors.Count == 0;
        }

    }
}