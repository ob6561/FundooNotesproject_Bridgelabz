using BCrypt.Net;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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
        private readonly IConfiguration _configuration;

        public UserService(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        #region Register

        public bool Register(RegisterDto dto)
        {
            var existingUser = _userRepository.GetByEmail(dto.Email);
            if (existingUser != null)
                return false;

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _userRepository.Register(user);
            return true;
        }

        #endregion

        #region Login

        public string Login(LoginDto dto)
        {
            var user = _userRepository.GetByEmail(dto.Email);
            if (user == null)
                return null;

            var isValidPassword = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);
            if (!isValidPassword)
                return null;

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

            // Normally emailed — returned for now
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


    }
}
#endregion