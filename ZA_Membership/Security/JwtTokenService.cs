using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ZA_Membership.Configuration;
using ZA_Membership.Models.Entities;

namespace ZA_Membership.Security
{

    /// <summary>
    /// Service for generating and validating JWT tokens.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a JWT access token for the specified user, roles, and permissions.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        string GenerateAccessToken(User user, List<string> roles, List<string> permissions);

        /// <summary>
        /// Generates a secure refresh token.
        /// </summary>
        /// <returns></returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Validates the specified JWT token and returns the associated claims principal if valid.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ClaimsPrincipal? ValidateToken(string token);

        /// <summary>
        /// Gets the expiration date and time of the specified JWT token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        DateTime GetTokenExpiration(string token);
    }

    /// <summary>
    /// Implementation of the IJwtTokenService interface for handling JWT tokens.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        /// <inheritdoc/>
        private readonly MembershipOptions _options;

        /// <summary>
        /// Initializes a new instance of the JwtTokenService class with the specified membership options.
        /// </summary>
        /// <param name="options"></param>
        public JwtTokenService(MembershipOptions options)
        {
            _options = options;
        }


        /// <inheritdoc/>
        public string GenerateAccessToken(User user, List<string> roles, List<string> permissions)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("jti", Guid.NewGuid().ToString()),
                new("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add permissions
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var token = new JwtSecurityToken(
                issuer: _options.Jwt.Issuer,
                audience: _options.Jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.Jwt.AccessTokenExpiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <inheritdoc/>
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()) +
                   Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }
        /// <inheritdoc/>
        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Jwt.SecretKey));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = _options.Jwt.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _options.Jwt.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        /// <inheritdoc/>
        public DateTime GetTokenExpiration(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            return jwt.ValidTo;
        }
    }
}