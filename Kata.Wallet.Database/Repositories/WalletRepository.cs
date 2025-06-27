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
    public class WalletRepository: IWalletRepository
    {
        private readonly DataContext dataContext;

        public WalletRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(Domain.Wallet wallet)
        {
            dataContext.Add(wallet);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Domain.Wallet?> GetById(int id)
        {
            return await dataContext.Wallets.FindAsync(id);
        }

        public async Task<List<Domain.Wallet>> Filter(string? userDocument, Currency? currency)
        {
            var query = dataContext.Wallets.AsQueryable();

            if (!string.IsNullOrEmpty(userDocument))
                query = query.Where(w => w.UserDocument!.Contains(userDocument));

            if (currency.HasValue)
                query = query.Where(w => w.Currency == currency.Value);

            return await query.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await dataContext.SaveChangesAsync();
        }

    }
}
