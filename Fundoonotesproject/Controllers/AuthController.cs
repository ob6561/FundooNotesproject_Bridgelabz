
using Microsoft.AspNetCore.Http;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using BusinessLayer;

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
        public IActionResult Register(RegisterDto dto)
        {
            var result = _userService.Register(dto);
            if (!result)
                return BadRequest("User already exists");

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var token = _userService.Login(dto);

            if (token == null)
                return Unauthorized("Invalid email or password");

            return Ok(new { Token = token });
        }

        [HttpPost("verify-email")]
        public IActionResult VerifyEmail(VerifyEmailDto dto)
        {
            var result = _userService.VerifyEmail(dto);
            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(ForgotPasswordDto dto)
        {
            var token = _userService.ForgotPassword(dto);
            return Ok(new { ResetToken = token });
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ResetPasswordDto dto)
        {
            var result = _userService.ResetPassword(dto);
            return Ok(result);
        }

    }
}

