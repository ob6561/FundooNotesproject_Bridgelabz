using BCrypt.Net;
using BusinessLayer.Services;
using DataLayer.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTOs;
using ModelLayer.DTOs.Auth;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            IOtpRepository otpRepository,
            EmailService emailService,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _emailService = emailService;
            _configuration = configuration;
        }

        
        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                return false;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.RegisterAsync(user);
            await GenerateAndSendOtpAsync(user);

            return true;
        }

        
        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            if (!user.IsEmailVerified)
                throw new Exception("Please verify your email before logging in");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isValidPassword)
                throw new Exception("Invalid email or password");

            return GenerateJwtToken(user);
        }

        
        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public async Task GenerateAndSendOtpAsync(User user)
        {
            string generatedOtp = GenerateOtp();

            var otp = new Otp
            {
                UserId = user.UserId,
                Code = generatedOtp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            await _otpRepository.AddAsync(otp);
            await _emailService.SendOtpAsync(user.Email, generatedOtp);
        }

        public async Task<string> VerifyOtpAsync(string email, string otpCode)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new Exception("Invalid user");

            var otp = await _otpRepository.GetValidOtpAsync(user.UserId, otpCode);
            if (otp == null)
                throw new Exception("Invalid or expired OTP");

            otp.IsUsed = true;
            await _otpRepository.UpdateAsync(otp);

            user.IsEmailVerified = true;
            await _userRepository.UpdateAsync(user);

            return GenerateJwtToken(user);
        }


        public async Task<string> ForgotPasswordAsync(ForgotPasswordDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Email not registered");

            await GenerateAndSendOtpAsync(user);
            return "OTP sent to registered email";
        }


        public async Task<string> ResetPasswordWithOtpAsync(ResetPasswordWithOtpDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid user");

            var otp = await _otpRepository.GetValidOtpAsync(user.UserId, dto.Otp);
            if (otp == null)
                throw new Exception("Invalid or expired OTP");

            otp.IsUsed = true;
            await _otpRepository.UpdateAsync(otp);

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            await _userRepository.UpdateAsync(user);

            return "Password reset successful";
        }



        public async Task<string> VerifyEmailAsync(VerifyEmailDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            var otp = await _otpRepository.GetValidOtpAsync(user.UserId, dto.Otp);
            if (otp == null)
                throw new Exception("Invalid or expired OTP");

            user.IsEmailVerified = true;
            otp.IsUsed = true;

            await _userRepository.UpdateAsync(user);
            await _otpRepository.UpdateAsync(otp);

            return "Email verified successfully";
        }

        
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserId", user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration["Jwt:ExpiryMinutes"])
                ),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
