using DataLayer.Context;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories.Implementations
{
    public class OtpRepository : IOtpRepository
    {
        private readonly FundooContext _context;

        public OtpRepository(FundooContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Otp otp)
        {
            await _context.Otps.AddAsync(otp);
            await _context.SaveChangesAsync();
        }

        public async Task<Otp?> GetValidOtpAsync(int userId, string code)
        {
            return await _context.Otps.FirstOrDefaultAsync(o =>
                o.UserId == userId &&
                o.Code == code &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow
            );
        }

        public async Task UpdateAsync(Otp otp)
        {
            _context.Otps.Update(otp);
            await _context.SaveChangesAsync();
        }
    }
}