using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ModelLayer.Entities;

namespace DataLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions<FundooContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        // Notes will be added later
    }
}

