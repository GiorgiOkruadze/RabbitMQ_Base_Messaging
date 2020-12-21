using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Data.Context
{
    public class TransferDbContext:DbContext
    {

        public TransferDbContext() { }
        public TransferDbContext(DbContextOptions options):base(options) { }

        public DbSet<TransferLog> TransferLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=GIORGIOKRUADZE;Database=TransferDB;Trusted_Connection=true;MultipleActiveResultSets=true;");
        }
    }
}
