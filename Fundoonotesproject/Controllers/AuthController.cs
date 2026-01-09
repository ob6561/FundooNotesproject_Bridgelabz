using Microsoft.AspNetCore.Mvc;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Auth;
using BusinessLayer;
using System.Threading.Tasks;

namespace Fundoonotesproject.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _userService.RegisterAsync(dto);
            if (!result)
                return BadRequest("User already exists");

            return Ok("User registered successfully");
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _userService.LoginAsync(dto);
            return Ok(new { Token = token });
        }

        
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailDto dto)
        {
            var result = await _userService.VerifyEmailAsync(dto);
            return Ok(result);
        }

        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var token = await _userService.ForgotPasswordAsync(dto);
            return Ok(new { ResetToken = token });
        }

        
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto);
            return Ok(result);
        }
    }
}
