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
            errors = [];

            if (password.Length < _options.Password.MinimumLength)
                errors.Add($"رمز عبور باید حداقل {_options.Password.MinimumLength} کاراکتر باشد");

            if (_options.Password.RequireUppercase && !password.Any(char.IsUpper))
                errors.Add("رمز عبور باید حداقل یک حرف بزرگ داشته باشد");

            if (_options.Password.RequireLowercase && !password.Any(char.IsLower))
                errors.Add("رمز عبور باید حداقل یک حرف کوچک داشته باشد");

            if (_options.Password.RequireDigit && !password.Any(char.IsDigit))
                errors.Add("رمز عبور باید حداقل یک عدد داشته باشد");

            if (_options.Password.RequireSpecialCharacter && !password.Any(ch => !char.IsLetterOrDigit(ch)))
                errors.Add("رمز عبور باید حداقل یک کاراکتر خاص داشته باشد");

            return errors.Count == 0;
        }
    }
}