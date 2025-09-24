# ZA Membership

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/ZA.Membership.svg)](https://www.nuget.org/packages/ZA.Membership/)

A comprehensive membership management library for .NET applications with clean architecture support.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

Membership, Authentication, and Roles Management Library for ASP.NET Core

## Features

- User registration and login
- JWT Token and Refresh Token management
- Roles and permissions system
- Ability to change password
- Enable/disable users
- Log out from all devices
- Flexible configuration

# How to install
Nuget:
https://www.nuget.org/packages/ZA.Membership/
### Terminal
```bash
> dotnet add package ZA.Membership
```
### Package Manager
```bash
PM> NuGet\Install-Package ZA.Membershi
```
## Files in the host project:
### appSettings:
```bash
"ZAMembership": {
  "Jwt": {
    "SecretKey": "YourSuperSecretKeyForJWTTokensWhichMustBeAtLeast32Characters!",
    "Issuer": "YourApp",
    "Audience": "YourAppUsers",
    "AccessTokenExpiryMinutes": 15,
    "RefreshTokenExpiryDays": 7
  },
  "Password": {
    "RequiredLength": 6,
    "RequireDigit": true,
    "RequireLowercase": true,
    "RequireUppercase": true,
    "RequireNonAlphanumeric": true
  },
  "User": {
    "RequireUniqueEmail": true,
    "RequireEmailConfirmation": false,
    "RequirePhoneNumberConfirmation": false
  }
},
```
### Program.cs:
```bash
using ZA_Membership.Extensions;
.
.
.
builder.Services.AddZAMembership(builder.Configuration);
.
.
var app = builder.Build();
```
### AppDbContext.cs:
```bash
using Microsoft.EntityFrameworkCore;
using ZA_Membership.Models.Entities;

namespace TestApi
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public AppDbContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Permission> Permission { get; set; }
    }
}
```
#### Migration
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### AuthController:
```bash
using Microsoft.AspNetCore.Mvc;
using ZA_Membership.Models.DTOs;
using ZA_Membership.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMembershipService _membershipService;

    public AuthController(IMembershipService membershipService)
    {
        _membershipService = membershipService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _membershipService.RegisterAsync(model);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = result.Message,
                    user = result.User,
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken,
                    expiresAt = result.ExpiresAt
                });
            }

            return BadRequest(new
            {
                message = result.Message,
                errors = result.Errors
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            var result = await _membershipService.LoginAsync(model, ipAddress, userAgent);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = result.Message,
                    user = result.User,
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken,
                    expiresAt = result.ExpiresAt
                });
            }

            return Unauthorized(new
            {
                message = result.Message,
                errors = result.Errors
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutDto model)
    {
        try
        {
            var result = await _membershipService.LogoutAsync(model.RefreshToken);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto model)
    {
        try
        {
            var result = await _membershipService.RefreshTokenAsync(model.RefreshToken);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = result.Message,
                    user = result.User,
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken,
                    expiresAt = result.ExpiresAt
                });
            }

            return BadRequest(new
            {
                message = result.Message,
                errors = result.Errors
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
```
### ProfileController:

```bash
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZA_Membership.Models.DTOs;
using ZA_Membership.Security;
using ZA_Membership.Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMembershipService _membershipService;
    private readonly ICurrentUserService _currentUserService;

    public ProfileController(
        IMembershipService membershipService,
        ICurrentUserService currentUserService)
    {
        _membershipService = membershipService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                return Unauthorized();

            var result = await _membershipService.GetUserAsync(userId.Value);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    user = result.Data
                });
            }

            return NotFound(new { message = result.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                return Unauthorized();

            var result = await _membershipService.UpdateUserAsync(userId.Value, model);

            if (result.IsSuccess)
            {
                return Ok(new
                {
                    message = result.Message,
                    user = result.Data
                });
            }

            return BadRequest(new
            {
                message = result.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                return Unauthorized();

            var result = await _membershipService.ChangePasswordAsync(userId.Value, model);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAllDevices()
    {
        try
        {
            var userId = _currentUserService.UserId;

            if (userId == null)
                return Unauthorized();

            var result = await _membershipService.LogoutAllDevicesAsync(userId.Value);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}

```
##
Packages:
```bash
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.9">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.9" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.0-rc.1.25451.107">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="9.0.4" />
    <PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="9.0.4" />
<PackageReference Include="ZA.Membership" Version="1.0.0" />

```
# Api Tests:
## Register:
```bash
POST: /api/auth/register
Content-Type: application/json

{
    "firstName": "Ali",
    "lastName": "Ramezani",
    "email": "ali@example.com",
    "phoneNumber": "09123456789",
    "password": "Test123!",
    "confirmPassword": "Test123!"
}
```
## Login:
```bash
POST: /api/auth/login
Content-Type: application/json

{
    "email": "ali@example.com",
    "password": "Test123!"
}
```
## Get Profile:
GET: /api/profile
Authorization: Bearer YOUR_JWT_TOKEN_HERE
```bash
PUT: /api/profile
Authorization: Bearer YOUR_JWT_TOKEN_HERE
Content-Type: application/json

{
    "firstName": Ali",
    "lastName": "Ramezani",
    "phoneNumber": "09987654321"
}
```
## Change Password:
```bash
POST: /api/profile/change-password
Authorization: Bearer YOUR_JWT_TOKEN_HERE
Content-Type: application/json

{
    "currentPassword": "Test123!",
    "newPassword": "NewTest123!",
    "confirmNewPassword": "NewTest123!"
}
```


