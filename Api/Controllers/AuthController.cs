using Microsoft.AspNetCore.Mvc;
using ZA_Membership.Models.DTOs;
using ZA_Membership.Services.Interfaces;

namespace ZAMembershipTestApi.Controllers
{
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
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _membershipService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _membershipService.LoginAsync(dto);
            return Ok(result);
        }
    }
}
