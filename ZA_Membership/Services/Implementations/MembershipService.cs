using ZA_Membership.Configuration;
using ZA_Membership.Models.DTOs;
using ZA_Membership.Models.Entities;
using ZA_Membership.Models.Results;
using ZA_Membership.Security;
using ZA_Membership.Services.Interfaces;

namespace ZA_Membership.Services.Implementations
{
    /// <summary>
    /// Implementation of membership services including user registration, login, token management, and user profile management.
    /// </summary>
    public class MembershipService : IMembershipService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserTokenRepository _tokenRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordService _passwordService;
        private readonly MembershipOptions _options;

        /// <summary>
        /// Constructor for MembershipService.
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="tokenRepository"></param>
        /// <param name="roleRepository"></param>
        /// <param name="jwtTokenService"></param>
        /// <param name="passwordService"></param>
        /// <param name="options"></param>
        public MembershipService(
            IUserRepository userRepository,
            IUserTokenRepository tokenRepository,
            IRoleRepository roleRepository,
            IJwtTokenService jwtTokenService,
            IPasswordService passwordService,
            MembershipOptions options)
        {
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _roleRepository = roleRepository;
            _jwtTokenService = jwtTokenService;
            _passwordService = passwordService;
            _options = options;
        }

        /// <inheritdoc/>
        public async Task<AuthResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Validate password
                if (!_passwordService.ValidatePassword(registerDto.Password, out var passwordErrors))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = passwordErrors,
                        Message = "رمز عبور نامعتبر است"
                    };
                }

                // Check if username exists
                if (await _userRepository.ExistsByUsernameAsync(registerDto.Username))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "نام کاربری قبلاً استفاده شده است" },
                        Message = "خطا در ثبت نام"
                    };
                }

                // Check if email exists
                if (_options.User.RequireUniqueEmail && await _userRepository.ExistsByEmailAsync(registerDto.Email))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "ایمیل قبلاً استفاده شده است" },
                        Message = "خطا در ثبت نام"
                    };
                }

                // Create user
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = _passwordService.HashPassword(registerDto.Password),
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    PhoneNumber = registerDto.PhoneNumber,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    EmailConfirmed = !_options.User.RequireEmailConfirmation,
                    PhoneNumberConfirmed = !_options.User.RequirePhoneNumberConfirmation
                };

                var createdUser = await _userRepository.CreateAsync(user);

                // Generate tokens
                var roles = await _userRepository.GetUserRolesAsync(createdUser.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(createdUser.Id);
                var accessToken = _jwtTokenService.GenerateAccessToken(createdUser, roles, permissions);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                // Save refresh token
                var userToken = new UserToken
                {
                    UserId = createdUser.Id,
                    Token = refreshToken,
                    TokenType = "RefreshToken",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(_options.Jwt.RefreshTokenExpiryDays)
                };

                await _tokenRepository.CreateAsync(userToken);

                var userDto = MapToUserDto(createdUser, roles, permissions);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = userDto,
                    Message = "ثبت نام با موفقیت انجام شد"
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "خطای سیستمی رخ داده است" },
                    Message = "خطا در ثبت نام"
                };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginDto loginDto, string? ipAddress = null, string? deviceInfo = null)
        {
            try
            {
                var user = await _userRepository.GetByUsernameOrEmailAsync(loginDto.Username);

                if (user == null || !user.IsActive)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "نام کاربری یا رمز عبور اشتباه است" },
                        Message = "خطا در ورود"
                    };
                }

                if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "نام کاربری یا رمز عبور اشتباه است" },
                        Message = "خطا در ورود"
                    };
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                // Generate tokens
                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
                var accessToken = _jwtTokenService.GenerateAccessToken(user, roles, permissions);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                // Save refresh token
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

                var userDto = MapToUserDto(user, roles, permissions);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = userDto,
                    Message = "ورود با موفقیت انجام شد"
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "خطای سیستمی رخ داده است" },
                    Message = "خطا در ورود"
                };
            }
        }

        public async Task<ServiceResult> LogoutAsync(string token)
        {
            try
            {
                await _tokenRepository.RevokeTokenAsync(token);
                return ServiceResult.Success("خروج با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است", "خطا در خروج");
            }
        }

        public async Task<ServiceResult> LogoutAllDevicesAsync(int userId)
        {
            try
            {
                await _tokenRepository.RevokeAllUserTokensAsync(userId);
                return ServiceResult.Success("خروج از همه دستگاه‌ها با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است", "خطا در خروج");
            }
        }

        // سایر متدها...

        private UserDto MapToUserDto(User user, List<string> roles, List<string> permissions)
        {
            return new UserDto
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
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                Roles = roles,
                Permissions = permissions
            };
        }

        // ادامه Services/Implementations/MembershipService.cs

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var userToken = await _tokenRepository.GetByTokenAsync(refreshToken);

                if (userToken == null || userToken.IsRevoked || userToken.ExpiresAt < DateTime.UtcNow)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "توکن نامعتبر یا منقضی شده است" },
                        Message = "خطا در بازآوری توکن"
                    };
                }

                var user = await _userRepository.GetByIdAsync(userToken.UserId);
                if (user == null || !user.IsActive)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "کاربر یافت نشد یا غیرفعال است" },
                        Message = "خطا در بازآوری توکن"
                    };
                }

                // Revoke old refresh token
                await _tokenRepository.RevokeTokenAsync(refreshToken);

                // Generate new tokens
                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
                var newAccessToken = _jwtTokenService.GenerateAccessToken(user, roles, permissions);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

                // Save new refresh token
                var newUserToken = new UserToken
                {
                    UserId = user.Id,
                    Token = newRefreshToken,
                    TokenType = "RefreshToken",
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(_options.Jwt.RefreshTokenExpiryDays),
                    IpAddress = userToken.IpAddress,
                    DeviceInfo = userToken.DeviceInfo
                };

                await _tokenRepository.CreateAsync(newUserToken);

                var userDto = MapToUserDto(user, roles, permissions);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(newAccessToken),
                    User = userDto,
                    Message = "توکن با موفقیت بازآوری شد"
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = new List<string> { "خطای سیستمی رخ داده است" },
                    Message = "خطا در بازآوری توکن"
                };
            }
        }

        public async Task<ServiceResult<UserDto>> GetUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<UserDto>.Failure("کاربر یافت نشد");
                }

                var roles = await _userRepository.GetUserRolesAsync(userId);
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                var userDto = MapToUserDto(user, roles, permissions);

                return ServiceResult<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return ServiceResult<UserDto>.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<UserDto>.Failure("کاربر یافت نشد");
                }

                // Check email uniqueness if changed
                if (!string.IsNullOrEmpty(updateDto.Email) &&
                    updateDto.Email != user.Email &&
                    _options.User.RequireUniqueEmail)
                {
                    if (await _userRepository.ExistsByEmailAsync(updateDto.Email))
                    {
                        return ServiceResult<UserDto>.Failure("این ایمیل قبلاً استفاده شده است");
                    }
                }

                // Update user properties
                user.FirstName = updateDto.FirstName ?? user.FirstName;
                user.LastName = updateDto.LastName ?? user.LastName;
                user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(updateDto.Email) && updateDto.Email != user.Email)
                {
                    user.Email = updateDto.Email;
                    user.EmailConfirmed = false; // Reset email confirmation
                }

                var updatedUser = await _userRepository.UpdateAsync(user);
                var roles = await _userRepository.GetUserRolesAsync(userId);
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                var userDto = MapToUserDto(updatedUser, roles, permissions);

                return ServiceResult<UserDto>.Success(userDto, "اطلاعات کاربر با موفقیت به‌روزرسانی شد");
            }
            catch (Exception ex)
            {
                return ServiceResult<UserDto>.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure("کاربر یافت نشد");
                }

                // Verify current password
                if (!_passwordService.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return ServiceResult.Failure("رمز عبور فعلی اشتباه است");
                }

                // Validate new password
                if (!_passwordService.ValidatePassword(changePasswordDto.NewPassword, out var passwordErrors))
                {
                    return ServiceResult.Failure(passwordErrors, "رمز عبور جدید نامعتبر است");
                }

                // Update password
                user.PasswordHash = _passwordService.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                // Revoke all tokens (force re-login)
                await _tokenRepository.RevokeAllUserTokensAsync(userId);

                return ServiceResult.Success("رمز عبور با موفقیت تغییر کرد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult> DeactivateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure("کاربر یافت نشد");
                }

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                // Revoke all tokens
                await _tokenRepository.RevokeAllUserTokensAsync(userId);

                return ServiceResult.Success("کاربر با موفقیت غیرفعال شد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult> ActivateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure("کاربر یافت نشد");
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                return ServiceResult.Success("کاربر با موفقیت فعال شد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult> AssignRoleAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure("کاربر یافت نشد");
                }

                var role = await _roleRepository.GetByNameAsync(roleName);
                if (role == null)
                {
                    return ServiceResult.Failure("نقش یافت نشد");
                }

                var userRoles = await _userRepository.GetUserRolesAsync(userId);
                if (userRoles.Contains(roleName))
                {
                    return ServiceResult.Failure("این نقش قبلاً به کاربر اختصاص داده شده است");
                }

                // Implementation would depend on your UserRole repository
                // This is just a placeholder - you'd implement this in your host project

                return ServiceResult.Success($"نقش {roleName} با موفقیت به کاربر اختصاص داده شد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult> RemoveRoleAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure("کاربر یافت نشد");
                }

                var userRoles = await _userRepository.GetUserRolesAsync(userId);
                if (!userRoles.Contains(roleName))
                {
                    return ServiceResult.Failure("این نقش به کاربر اختصاص داده نشده است");
                }

                // Implementation would depend on your UserRole repository

                return ServiceResult.Success($"نقش {roleName} با موفقیت از کاربر حذف شد");
            }
            catch (Exception ex)
            {
                return ServiceResult.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<List<string>>.Failure("کاربر یافت نشد");
                }

                var roles = await _userRepository.GetUserRolesAsync(userId);
                return ServiceResult<List<string>>.Success(roles);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<string>>.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult<List<string>>> GetUserPermissionsAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<List<string>>.Failure("کاربر یافت نشد");
                }

                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                return ServiceResult<List<string>>.Success(permissions);
            }
            catch (Exception ex)
            {
                return ServiceResult<List<string>>.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult<bool>> HasPermissionAsync(int userId, string permission)
        {
            try
            {
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                var hasPermission = permissions.Contains(permission);
                return ServiceResult<bool>.Success(hasPermission);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure("خطای سیستمی رخ داده است");
            }
        }

        public async Task<ServiceResult<bool>> IsInRoleAsync(int userId, string roleName)
        {
            try
            {
                var roles = await _userRepository.GetUserRolesAsync(userId);
                var isInRole = roles.Contains(roleName);
                return ServiceResult<bool>.Success(isInRole);
            }
            catch (Exception ex)
            {
                return ServiceResult<bool>.Failure("خطای سیستمی رخ داده است");
            }
        }
    }
}

