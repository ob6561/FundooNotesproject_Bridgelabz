using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelLayer.Entities;

namespace DataLayer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task RegisterAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> GetByResetTokenAsync(string token);
    }
}
