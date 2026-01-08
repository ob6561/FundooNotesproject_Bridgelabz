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

        

        public bool Register(RegisterDto dto)
        {
            var existingUser = _userRepository.GetByEmail(dto.Email);
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

            _userRepository.Register(user);

            
            GenerateAndSendOtp(user);

            return true;
        }

        

        public string Login(LoginDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password");

            
            if (!user.IsEmailVerified)
                throw new Exception("Please verify your email before logging in");

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isValidPassword)
                throw new Exception("Invalid email or password");

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


        

        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public void GenerateAndSendOtp(User user)
        {
            string generatedOtp = GenerateOtp();

            var otp = new Otp
            {
                UserId = user.UserId,
                Code = generatedOtp,
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            };

            _otpRepository.Add(otp);
            _emailService.SendOtp(user.Email, generatedOtp);
        }

        public string VerifyOtp(string email, string otpCode)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
                throw new Exception("Invalid user");

            var otp = _otpRepository.GetValidOtp(user.UserId, otpCode);
            if (otp == null)
                throw new Exception("Invalid or expired OTP");

            otp.IsUsed = true;
            _otpRepository.Update(otp);

            user.IsEmailVerified = true;
            _userRepository.Update(user);

            
            return GenerateJwtToken(user);
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

        

        public string ForgotPassword(ForgotPasswordDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null)
                throw new Exception("Email not registered");

            var token = Guid.NewGuid().ToString();

            user.ResetToken = token;
            user.ResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            _userRepository.Update(user);

            return token; 
        }

        public string ResetPassword(ResetPasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var user = _userRepository.GetByResetToken(dto.Token);
            if (user == null)
                throw new Exception("Invalid or expired token");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            _userRepository.Update(user);

            return "Password reset successful";
        }

        public string VerifyEmail(VerifyEmailDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null)
                throw new Exception("User not found");

            var otp = _otpRepository.GetValidOtp(user.UserId, dto.Otp);
            if (otp == null)
                throw new Exception("Invalid or expired OTP");

            user.IsEmailVerified = true;
            otp.IsUsed = true;

            _userRepository.Update(user);
            _otpRepository.Update(otp);

            return "Email verified successfully";
        }

    }
}
