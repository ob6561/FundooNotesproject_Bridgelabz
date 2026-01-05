using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataLayer.Context;
using DataLayer.Repositories.Interfaces;
using ModelLayer.Entities;
using System.Linq;

namespace DataLayer.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly FundooContext _context;

        public UserRepository(FundooContext context)
        {
            _context = context;
        }

        public User? GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public User? GetByResetToken(string token)
        {
            return _context.Users.FirstOrDefault(u =>
                u.ResetToken == token &&
                u.ResetTokenExpiry > DateTime.UtcNow);
        }

    }
}


