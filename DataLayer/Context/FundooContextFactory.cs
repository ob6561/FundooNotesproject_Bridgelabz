using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataLayer.Context
{
    public class FundooContextFactory : IDesignTimeDbContextFactory<FundooContext>
    {
        public FundooContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FundooContext>();

            optionsBuilder.UseSqlServer(
                "Server=ADIYOGI;Database=FundooNotesDB;Trusted_Connection=True;TrustServerCertificate=True"
            );

            return new FundooContext(optionsBuilder.Options);
        }
    }
}

