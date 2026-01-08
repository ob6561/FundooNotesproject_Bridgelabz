using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Context;
using DataLayer.Repositories.Interfaces;
using ModelLayer.Entities;

namespace DataLayer.Repositories.Implementations
{
    public class OtpRepository : IOtpRepository
    {
        private readonly FundooContext _context;

        public OtpRepository(FundooContext context)
        {
            _context = context;
        }

        public void Add(Otp otp)
        {
            _context.Otps.Add(otp);
            _context.SaveChanges();
        }

        public Otp GetValidOtp(int userId, string code)
        {
            return _context.Otps.FirstOrDefault(o =>
                o.UserId == userId &&
                o.Code == code &&
                !o.IsUsed &&
                o.ExpiresAt > DateTime.UtcNow
            );
        }

        public void Update(Otp otp)
        {
            _context.Otps.Update(otp);
            _context.SaveChanges();
        }
    }
}
