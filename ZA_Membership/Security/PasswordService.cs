using BCrypt.Net;
using ZA_Membership.Configuration;

namespace ZA_Membership.Security
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        bool ValidatePassword(string password, out List<string> errors);
    }

    public class PasswordService : IPasswordService
    {
        private readonly MembershipOptions _options;

        public PasswordService(MembershipOptions options)
        {
            _options = options;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

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

        public bool ValidatePassword(string password, out List<string> errors)
        {
            errors = new List<string>();

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