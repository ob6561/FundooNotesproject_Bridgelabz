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
        void Add(Otp otp);
        Otp GetValidOtp(int userId, string code);
        void Update(Otp otp);
    }
}
