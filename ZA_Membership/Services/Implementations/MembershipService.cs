using System.Globalization;
using System.Resources;
using ZA_Membership.Configuration;
using ZA_Membership.Models.DTOs;
using ZA_Membership.Models.Entities;
using ZA_Membership.Models.Results;
using ZA_Membership.Properties;
using ZA_Membership.Repositories.Interfaces;
using ZA_Membership.Security;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    public class MembershipService : IMembershipService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _tokenRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordService _passwordService;
        private readonly MembershipOptions _options;
        private readonly ResourceManager _rm = new ResourceManager(typeof(Messages));

        internal MembershipService(
            IUserRepository userRepository,
            IUserTokenRepository tokenRepository,
            IJwtTokenService jwtTokenService,
            IPasswordService passwordService,
            MembershipOptions options)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _jwtTokenService = jwtTokenService;
            _passwordService = passwordService;
            _options = options;
        }

        private string Msg(string key, string fallback) =>
            _rm.GetString(key, CultureInfo.CurrentUICulture) ?? fallback;

        public async Task<AuthResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                if (!_passwordService.ValidatePassword(registerDto.Password, out var passErrors))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = passErrors,
                        Message = Msg("Password_Invalid", "Invalid password")
                    };
                }

                if (await _userRepository.ExistsByUsernameAsync(registerDto.Username))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = [Msg("Username_AlreadyExists", "Username already exists")],
                        Message = Msg("Register_Failed", "Registration failed")
                    };
                }

                if (_options.User.RequireUniqueEmail &&
                    await _userRepository.ExistsByEmailAsync(registerDto.Email ?? string.Empty))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = [Msg("Email_AlreadyExists", "Email already exists")],
                        Message = Msg("Register_Failed", "Registration failed")
                    };
                }

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email ?? string.Empty,
                    NationalCode = registerDto.NationalCode ?? string.Empty,
                    PasswordHash = _passwordService.HashPassword(registerDto.Password),
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Birthday = registerDto.Birthday,
                    ProfilePictureUrl = registerDto.ProfilePictureUrl ?? string.Empty,
                    IsVerify = false,
                    IsActive = true
                };

                var createdUser = await _userRepository.CreateAsync(user);

                var accessToken = _jwtTokenService.GenerateAccessToken(createdUser, new List<string>(), new List<string>());
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                var userToken = new UserToken
                {
                    UserId = createdUser.Id,
                    Token = refreshToken,
                    TokenType = "RefreshToken",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(_options.Jwt.RefreshTokenExpiryDays)
                };

                await _tokenRepository.CreateAsync(userToken);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = MapToUserDto(createdUser),
                    Message = Msg("Register_Success", "Registration successful")
                };
            }
            catch
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = [Msg("System_Error", "System error occurred")],
                    Message = Msg("Register_Failed", "Registration failed")
                };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? deviceInfo = null)
        {
            try
            {
                var user = await _userRepository.GetByUsernameOrEmailAsync(loginDto.Username);
                if (user is null || !user.IsActive || !_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = [Msg("Login_InvalidCredentials", "Invalid username or password")],
                        Message = Msg("Login_Failed", "Login failed")
                    };
                }

                var accessToken = _jwtTokenService.GenerateAccessToken(user, new List<string>(), new List<string>());
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                var userToken = new UserToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    TokenType = "RefreshToken",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(_options.Jwt.RefreshTokenExpiryDays),
                    IpAddress = ipAddress,
                    DeviceInfo = deviceInfo
                };

                await _tokenRepository.CreateAsync(userToken);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = MapToUserDto(user),
                    Message = Msg("Login_Success", "Login successful")
                };
            }
            catch
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = [Msg("System_Error", "System error occurred")],
                    Message = Msg("Login_Failed", "Login failed")
                };
            }
        }

        // ... سایر متدها با همین الگو null-coalescing

        public async Task<ServiceResult<UserDto>> GetUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                    return ServiceResult<UserDto>.Failure(Msg("User_NotFound", "User not found"));

                return ServiceResult<UserDto>.Success(MapToUserDto(user));
            }
            catch
            {
                return ServiceResult<UserDto>.Failure(Msg("System_Error", "System error occurred"));
            }
        }

        public async Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                    return ServiceResult.Failure(Msg("User_NotFound", "User not found"));

                if (!_passwordService.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
                    return ServiceResult.Failure(Msg("Password_CurrentInvalid", "Current password invalid"));

                if (!_passwordService.ValidatePassword(changePasswordDto.NewPassword, out var errors))
                    return ServiceResult.Failure(errors, Msg("Password_NewInvalid", "New password invalid"));

                user.PasswordHash = _passwordService.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
                await _tokenRepository.RevokeAllUserTokensAsync(userId);

                return ServiceResult.Success(Msg("Password_ChangeSuccess", "Password changed successfully"));
            }
            catch
            {
                return ServiceResult.Failure(Msg("System_Error", "System error occurred"));
            }
        }

        private UserDto MapToUserDto(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            IsActive = user.IsActive,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
            CreatedAt = user.CreatedAt
        };

        public async Task<ServiceResult> LogoutAsync(string token)
        {
            try
            {
                await _tokenRepository.RevokeTokenAsync(token);
                return ServiceResult.Success(Msg("Logout_Success", "Logout successful"));
            }
            catch
            {
                return ServiceResult.Failure(Msg("System_Error", "System error occurred"),
                                             Msg("Logout_Failed", "Logout failed"));
            }
        }

        public async Task<ServiceResult> LogoutAllDevicesAsync(int userId)
        {
            try
            {
                await _tokenRepository.RevokeAllUserTokensAsync(userId);
                return ServiceResult.Success(Msg("LogoutAllDevices_Success", "Logout from all devices successful"));
            }
            catch
            {
                return ServiceResult.Failure(Msg("System_Error", "System error occurred"),
                                             Msg("Logout_Failed", "Logout failed"));
            }
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var token = await _tokenRepository.GetByTokenAsync(refreshToken);
                if (token is null || token.IsRevoked || token.ExpiresAt < DateTime.UtcNow)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = [Msg("RefreshToken_Invalid", "Invalid or expired refresh token")],
                        Message = Msg("RefreshToken_Failed", "Refresh token failed")
                    };
                }

                var user = await _userRepository.GetByIdAsync(token.UserId);
                if (user is null || !user.IsActive)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = [Msg("User_NotFoundOrInactive", "User not found or inactive")],
                        Message = Msg("RefreshToken_Failed", "Refresh token failed")
                    };
                }

                await _tokenRepository.RevokeTokenAsync(refreshToken);

                var newAccessToken = _jwtTokenService.GenerateAccessToken(user, new List<string>(), new List<string>());
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

                var newTokenEntity = new UserToken
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    TokenType = "RefreshToken",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(_options.Jwt.RefreshTokenExpiryDays),
                    IpAddress = token.IpAddress,
                    DeviceInfo = token.DeviceInfo
                };

                await _tokenRepository.CreateAsync(newTokenEntity);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(newAccessToken),
                    User = MapToUserDto(user),
                    Message = Msg("RefreshToken_Success", "Token refreshed successfully")
                };
            }
            catch
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = [Msg("System_Error", "System error occurred")],
                    Message = Msg("RefreshToken_Failed", "Refresh token failed")
                };
            }
        }

        public async Task<ServiceResult<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                    return ServiceResult<UserDto>.Failure(Msg("User_NotFound", "User not found"));

                if (!string.IsNullOrEmpty(updateDto.Email) &&
                    updateDto.Email != user.Email &&
                    _options.User.RequireUniqueEmail &&
                    await _userRepository.ExistsByEmailAsync(updateDto.Email))
                {
                    return ServiceResult<UserDto>.Failure(Msg("Email_AlreadyExists", "Email already exists"));
                }

                user.FirstName = updateDto.FirstName ?? user.FirstName;
                user.LastName = updateDto.LastName ?? user.LastName;
                user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
                user.Email = updateDto.Email ?? user.Email;
                user.UpdatedAt = DateTime.UtcNow;

                var updatedUser = await _userRepository.UpdateAsync(user);
                return ServiceResult<UserDto>.Success(MapToUserDto(updatedUser),
                    Msg("User_UpdateSuccess", "User updated successfully"));
            }
            catch
            {
                return ServiceResult<UserDto>.Failure(Msg("System_Error", "System error occurred"));
            }
        }

        public async Task<ServiceResult> DeactivateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                    return ServiceResult.Failure(Msg("User_NotFound", "User not found"));

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
                await _tokenRepository.RevokeAllUserTokensAsync(userId);

                return ServiceResult.Success(Msg("User_DeactivateSuccess", "User deactivated successfully"));
            }
            catch
            {
                return ServiceResult.Failure(Msg("System_Error", "System error occurred"));
            }
        }

        public async Task<ServiceResult> ActivateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user is null)
                    return ServiceResult.Failure(Msg("User_NotFound", "User not found"));

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                return ServiceResult.Success(Msg("User_ActivateSuccess", "User activated successfully"));
            }
            catch
            {
                return ServiceResult.Failure(Msg("System_Error", "System error occurred"));
            }
        }

    }
}
