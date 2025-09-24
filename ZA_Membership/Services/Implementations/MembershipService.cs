using ZA_Membership.Configuration;
using ZA_Membership.Models.DTOs;
using ZA_Membership.Models.Entities;
using ZA_Membership.Models.Results;
using ZA_Membership.Security;
using ZA_Membership.Services.Interfaces;
using ZA_Membership.Resources;
using System.Globalization;
using System.Resources;

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
        private readonly ResourceManager _rm = new ResourceManager(typeof(Messages));

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
                        Message = _rm.GetString("Password_Invalid", CultureInfo.CurrentUICulture) ?? "Invalid password"
                    };
                }

                // Check if username exists
                if (await _userRepository.ExistsByUsernameAsync(registerDto.Username))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = [_rm.GetString("Username_AlreadyExists", CultureInfo.CurrentUICulture) ?? "Username already exists"],
                        Message = _rm.GetString("Register_Failed", CultureInfo.CurrentUICulture) ?? "Registration failed"
                    };
                }

                // Check if email exists
                if (_options.User.RequireUniqueEmail && await _userRepository.ExistsByEmailAsync(registerDto.Email ?? ""))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string>
                        { _rm.GetString("Email_AlreadyExists", CultureInfo.CurrentUICulture) ?? "Email already exists" },
                        Message = _rm.GetString("Register_Failed", CultureInfo.CurrentUICulture) ?? "Registration failed"
                    };
                }

                // Create user
                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email ?? string.Empty,
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

                var roles = await _userRepository.GetUserRolesAsync(createdUser.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(createdUser.Id);
                var accessToken = _jwtTokenService.GenerateAccessToken(createdUser, roles, permissions);
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

                var userDto = MapToUserDto(createdUser, roles, permissions);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = userDto,
                    Message = _rm.GetString("Register_Success", CultureInfo.CurrentUICulture) ?? "Registration successful"
                };
            }
            catch
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = new List<string>
                    { _rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred" },
                    Message = _rm.GetString("Register_Failed", CultureInfo.CurrentUICulture) ?? "Registration failed"
                };
            }
        }
        /// <inheritdoc/>
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
                        Errors = new List<string>
                        { _rm.GetString("Login_InvalidCredentials", CultureInfo.CurrentUICulture) ?? "Invalid username or password" },
                        Message = _rm.GetString("Login_Failed", CultureInfo.CurrentUICulture) ?? "Login failed"
                    };
                }

                if (!_passwordService.VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string>
                        { _rm.GetString("Login_InvalidCredentials", CultureInfo.CurrentUICulture) ?? "Invalid username or password" },
                        Message = _rm.GetString("Login_Failed", CultureInfo.CurrentUICulture) ?? "Login failed"
                    };
                }

                user.LastLoginAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
                var accessToken = _jwtTokenService.GenerateAccessToken(user, roles, permissions);
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

                var userDto = MapToUserDto(user, roles, permissions);

                return new AuthResult
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = _jwtTokenService.GetTokenExpiration(accessToken),
                    User = userDto,
                    Message = _rm.GetString("Login_Success", CultureInfo.CurrentUICulture) ?? "Login successful"
                };
            }
            catch
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = new List<string>
                    { _rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred" },
                    Message = _rm.GetString("Login_Failed", CultureInfo.CurrentUICulture) ?? "Login failed"
                };
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> LogoutAsync(string token)
        {
            try
            {
                await _tokenRepository.RevokeTokenAsync(token);
                return ServiceResult.Success(_rm.GetString("Logout_Success", CultureInfo.CurrentUICulture) ?? "Logout successful");
            }
            catch
            {
                return ServiceResult.Failure(
                    _rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred",
                    _rm.GetString("Logout_Failed", CultureInfo.CurrentUICulture) ?? "Logout failed"
                );
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> LogoutAllDevicesAsync(int userId)
        {
            try
            {
                await _tokenRepository.RevokeAllUserTokensAsync(userId);
                return ServiceResult.Success(_rm.GetString("LogoutAllDevices_Success", CultureInfo.CurrentUICulture) ?? "Logged out from all devices successfully");
            }
            catch
            {
                return ServiceResult.Failure(
                    _rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred",
                    _rm.GetString("Logout_Failed", CultureInfo.CurrentUICulture) ?? "Logout failed"
                );
            }
        }

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
        /// <inheritdoc/>
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
                        Errors = new List<string>
                        { _rm.GetString("RefreshToken_Invalid", CultureInfo.CurrentUICulture) ?? "Invalid or expired token" },
                        Message = _rm.GetString("RefreshToken_Failed", CultureInfo.CurrentUICulture) ?? "Refresh token failed"
                    };
                }

                var user = await _userRepository.GetByIdAsync(userToken.UserId);
                if (user == null || !user.IsActive)
                {
                    return new AuthResult
                    {
                        IsSuccess = false,
                        Errors = new List<string>
                        { _rm.GetString("User_NotFoundOrInactive", CultureInfo.CurrentUICulture) ?? "User not found or inactive" },
                        Message = _rm.GetString("RefreshToken_Failed", CultureInfo.CurrentUICulture) ?? "Refresh token failed"
                    };
                }

                await _tokenRepository.RevokeTokenAsync(refreshToken);

                var roles = await _userRepository.GetUserRolesAsync(user.Id);
                var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
                var newAccessToken = _jwtTokenService.GenerateAccessToken(user, roles, permissions);
                var newRefreshToken = _jwtTokenService.GenerateRefreshToken();

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
                    Message = _rm.GetString("RefreshToken_Success", CultureInfo.CurrentUICulture) ?? "Token refreshed successfully"
                };
            }
            catch
            {
                return new AuthResult
                {
                    IsSuccess = false,
                    Errors = new List<string>
                    { _rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred" },
                    Message = _rm.GetString("RefreshToken_Failed", CultureInfo.CurrentUICulture) ?? "Refresh token failed"
                };
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult<UserDto>> GetUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<UserDto>.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                var roles = await _userRepository.GetUserRolesAsync(userId);
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                var userDto = MapToUserDto(user, roles, permissions);

                return ServiceResult<UserDto>.Success(userDto);
            }
            catch
            {
                return ServiceResult<UserDto>.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult<UserDto>> UpdateUserAsync(int userId, UpdateUserDto updateDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<UserDto>.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                if (!string.IsNullOrEmpty(updateDto.Email) &&
                    updateDto.Email != user.Email &&
                    _options.User.RequireUniqueEmail)
                {
                    if (await _userRepository.ExistsByEmailAsync(updateDto.Email))
                    {
                        return ServiceResult<UserDto>.Failure(_rm.GetString("Email_AlreadyExists", CultureInfo.CurrentUICulture) ?? "Email already exists");
                    }
                }

                user.FirstName = updateDto.FirstName ?? user.FirstName;
                user.LastName = updateDto.LastName ?? user.LastName;
                user.PhoneNumber = updateDto.PhoneNumber ?? user.PhoneNumber;
                user.UpdatedAt = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(updateDto.Email) && updateDto.Email != user.Email)
                {
                    user.Email = updateDto.Email;
                    user.EmailConfirmed = false;
                }

                var updatedUser = await _userRepository.UpdateAsync(user);
                var roles = await _userRepository.GetUserRolesAsync(userId);
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                var userDto = MapToUserDto(updatedUser, roles, permissions);

                return ServiceResult<UserDto>.Success(userDto,
                    _rm.GetString("User_UpdateSuccess", CultureInfo.CurrentUICulture) ?? "User updated successfully");
            }
            catch
            {
                return ServiceResult<UserDto>.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                if (!_passwordService.VerifyPassword(changePasswordDto.CurrentPassword, user.PasswordHash))
                {
                    return ServiceResult.Failure(_rm.GetString("Password_CurrentInvalid", CultureInfo.CurrentUICulture) ?? "Current password is incorrect");
                }

                if (!_passwordService.ValidatePassword(changePasswordDto.NewPassword, out var passwordErrors))
                {
                    return ServiceResult.Failure(passwordErrors, _rm.GetString("Password_NewInvalid", CultureInfo.CurrentUICulture) ?? "New password is invalid");
                }

                user.PasswordHash = _passwordService.HashPassword(changePasswordDto.NewPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                await _tokenRepository.RevokeAllUserTokensAsync(userId);

                return ServiceResult.Success(_rm.GetString("Password_ChangeSuccess", CultureInfo.CurrentUICulture) ?? "Password changed successfully");
            }
            catch
            {
                return ServiceResult.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> DeactivateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                await _tokenRepository.RevokeAllUserTokensAsync(userId);

                return ServiceResult.Success(_rm.GetString("User_DeactivateSuccess", CultureInfo.CurrentUICulture) ?? "User deactivated successfully");
            }
            catch
            {
                return ServiceResult.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> ActivateUserAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                user.IsActive = true;
                user.UpdatedAt = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);

                return ServiceResult.Success(_rm.GetString("User_ActivateSuccess", CultureInfo.CurrentUICulture) ?? "User activated successfully");
            }
            catch
            {
                return ServiceResult.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> AssignRoleAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                var role = await _roleRepository.GetByNameAsync(roleName);
                if (role == null)
                {
                    return ServiceResult.Failure(_rm.GetString("Role_NotFound", CultureInfo.CurrentUICulture) ?? "Role not found");
                }

                var userRoles = await _userRepository.GetUserRolesAsync(userId);
                if (userRoles.Contains(roleName))
                {
                    return ServiceResult.Failure(_rm.GetString("Role_AlreadyAssigned", CultureInfo.CurrentUICulture) ?? "Role already assigned to user");
                }

                // Implementation in host project

                return ServiceResult.Success(string.Format(
                    _rm.GetString("Role_AssignSuccess", CultureInfo.CurrentUICulture) ?? "Role {0} assigned successfully to user",
                    roleName
                ));
            }
            catch
            {
                return ServiceResult.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult> RemoveRoleAsync(int userId, string roleName)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                var userRoles = await _userRepository.GetUserRolesAsync(userId);
                if (!userRoles.Contains(roleName))
                {
                    return ServiceResult.Failure(_rm.GetString("Role_NotAssigned", CultureInfo.CurrentUICulture) ?? "Role not assigned to user");
                }

                // Implementation in host project

                return ServiceResult.Success(string.Format(
                    _rm.GetString("Role_RemoveSuccess", CultureInfo.CurrentUICulture) ?? "Role {0} removed successfully from user",
                    roleName
                ));
            }
            catch
            {
                return ServiceResult.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult<List<string>>> GetUserRolesAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<List<string>>.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                var roles = await _userRepository.GetUserRolesAsync(userId);
                return ServiceResult<List<string>>.Success(roles);
            }
            catch
            {
                return ServiceResult<List<string>>.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult<List<string>>> GetUserPermissionsAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ServiceResult<List<string>>.Failure(_rm.GetString("User_NotFound", CultureInfo.CurrentUICulture) ?? "User not found");
                }

                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                return ServiceResult<List<string>>.Success(permissions);
            }
            catch
            {
                return ServiceResult<List<string>>.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> HasPermissionAsync(int userId, string permission)
        {
            try
            {
                var permissions = await _userRepository.GetUserPermissionsAsync(userId);
                var hasPermission = permissions.Contains(permission);
                return ServiceResult<bool>.Success(hasPermission);
            }
            catch
            {
                return ServiceResult<bool>.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> IsInRoleAsync(int userId, string roleName)
        {
            try
            {
                var roles = await _userRepository.GetUserRolesAsync(userId);
                var isInRole = roles.Contains(roleName);
                return ServiceResult<bool>.Success(isInRole);
            }
            catch
            {
                return ServiceResult<bool>.Failure(_rm.GetString("System_Error", CultureInfo.CurrentUICulture) ?? "A system error occurred");
            }
        }
    }
}
