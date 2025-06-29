using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Database.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly DataContext dataContext;
        private readonly ILogger<TransactionRepository> logger;

        public TransactionRepository(DataContext dataContext, ILogger<TransactionRepository> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public async Task<Domain.Transaction?> GetById(int id)
        {
            logger.LogInformation("Repo: GetById Transaction: {Id}", id);
            return await dataContext.Transactions.FindAsync(id);
        }

        public async Task Create(Transaction transaction)
        {
            logger.LogInformation("Repository: Create Transaction: {Transaction}", transaction);
            dataContext.Add(transaction);
            await dataContext.SaveChangesAsync();
        }
        public async Task<List<Transaction>> GetAllByWalletId(int walletId)
        {
            logger.LogInformation("Repository: GetAllByWalletId WalletId: {walletId}", walletId);
            return await dataContext.Transactions
                .Include(t => t.WalletIncoming)
                .Include(t => t.WalletOutgoing)
                .Where(t => t.WalletIncoming.Id == walletId || t.WalletOutgoing.Id == walletId)
                .ToListAsync();
        }
    }
}
