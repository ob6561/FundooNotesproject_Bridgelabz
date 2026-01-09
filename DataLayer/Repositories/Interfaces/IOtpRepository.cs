using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Entities;

namespace DataLayer.Repositories.Interfaces
{
    public interface IOtpRepository
    {
        Task AddAsync(Otp otp);
        Task<Otp?> GetValidOtpAsync(int userId, string code);
        Task UpdateAsync(Otp otp);
    }
}
