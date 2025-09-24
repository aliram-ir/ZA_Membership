# ZA_Membership

کتابخانه مدیریت عضویت، احراز هویت و نقش‌ها برای ASP.NET Core

## ویژگی‌ها

- ثبت نام و ورود کاربران
- مدیریت JWT Token و Refresh Token
- سیستم نقش‌ها و مجوزها
- امکان تغییر رمز عبور
- فعال/غیرفعال کردن کاربران
- خروج از همه دستگاه‌ها
- پیکربندی انعطاف‌پذیر

## نصب
```bash
dotnet add package ZA.Membership

## پیکربندی

### 1. appsettings.json

json
{
  "ZAMembership": {
"Jwt": {
"SecretKey": "your-super-secret-jwt-key-here-minimum-32-characters",
"Issuer": "YourApp",
"Audience": "YourApp",
"AccessTokenExpiryMinutes": 60,
"RefreshTokenExpiryDays": 30
},
"Password": {
"MinimumLength": 8,
"RequireUppercase": true,
"RequireLowercase": true,
"RequireDigit": true,
"RequireSpecialCharacter": true
},
"User": {
"RequireUniqueEmail": true,
"RequireEmailConfirmation": false,
"RequirePhoneNumberConfirmation": false
},
"Security": {
"MaxFailedAccessAttempts": 5,
"LockoutTimeSpanMinutes": 30,
"RequireTwoFactor": false
}
  }
}

### 2. Program.cs

csharp
using ZA_Membership.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add ZA Membership
builder.Services.AddZAMembership(builder.Configuration);

// یا با پیکربندی دستی
builder.Services.AddZAMembershipWithOptions(options =>
{
options.Jwt.SecretKey = "your-secret-key";
options.Jwt.Issuer = "YourApp";
options.Jwt.Audience = "YourApp";
// ...
});

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

### 3. پیاده‌سازی Repository ها

شما باید interface های repository را در پروژه خود پیاده‌سازی کنید:

csharp
public class UserRepository : IUserRepository
{
// پیاده‌سازی متدهای مربوط به User
}

public class UserTokenRepository : IUserTokenRepository
{
// پیاده‌سازی متدهای مربوط به UserToken
}

public class RoleRepository : IRoleRepository
{
// پیاده‌سازی متدهای مربوط به Role
}

## استفاده

### در Controller:

csharp
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
private readonly IMembershipService _membershipService;

public AccountController(IMembershipService membershipService)
{
_membershipService = membershipService;
}

[HttpPost("register")]
public async Task<IActionResult> Register(RegisterDto model)
{
var result = await _membershipService.RegisterAsync(model);
return result.IsSuccess ? Ok(result) : BadRequest(result);
}

[HttpPost("login")]
public async Task<IActionResult> Login(LoginDto model)
{
var result = await _membershipService.LoginAsync(model);
return result.IsSuccess ? Ok(result) : BadRequest(result);
}

[HttpPost("logout")]
[Authorize]
public async Task<IActionResult> Logout()
{
var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
var result = await _membershipService.LogoutAsync(token);
return result.IsSuccess ? Ok(result) : BadRequest(result);
}
}

### استفاده از Attribute ها:

csharp
[RequirePermission("UserManagement")]
public async Task<IActionResult> GetUsers()
{
// کد شما
}

## مجوز

MIT License
