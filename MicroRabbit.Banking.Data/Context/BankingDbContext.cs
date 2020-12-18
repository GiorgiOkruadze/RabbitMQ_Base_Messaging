using MicroRabbit.Banging.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Data.Context
{
    public class BankingDbContext:DbContext
    {
        public BankingDbContext() { }
        public BankingDbContext(DbContextOptions options) : base(options) { }


        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer("Server=GIORGIOKRUADZE;Database=BankingDB;Trusted_Connection=true;MultipleActiveResultSets=true;");
        }

    }
}
