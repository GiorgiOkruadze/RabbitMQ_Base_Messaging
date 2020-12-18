using MicroRabbit.Banging.Domain.Interfaces;
using MicroRabbit.Banging.Domain.Models;
using MicroRabbit.Banking.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroRabbit.Banking.Data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankingDbContext _dbContext = default;

        public AccountRepository(BankingDbContext db)
        {
            _dbContext = db;
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _dbContext.Accounts.ToList();
        }
    }
}
