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
        User? GetByEmail(string email);
        void Register(User user);
        void Update(User user);
        User? GetByResetToken(string token);


    }
}
