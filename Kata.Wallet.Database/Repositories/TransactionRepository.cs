using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
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

        public TransactionRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
        public async Task Create(Transaction transaction)
        {
            dataContext.Add(transaction);
            await dataContext.SaveChangesAsync();
        }
        public async Task<List<Transaction>> GetAllByWalletId(int walletId)
        {
            return await dataContext.Transactions
                .Include(t => t.WalletIncoming)
                .Include(t => t.WalletOutgoing)
                .Where(t => t.WalletIncoming.Id == walletId || t.WalletOutgoing.Id == walletId)
                .ToListAsync();
        }
    }
}
