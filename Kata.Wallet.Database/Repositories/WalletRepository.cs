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
    public class WalletRepository: IWalletRepository
    {
        private readonly DataContext dataContext;
        private readonly ILogger<WalletRepository> logger;

        public WalletRepository(DataContext dataContext, ILogger<WalletRepository> logger)
        {
            this.dataContext = dataContext;
            this.logger = logger;
        }

        public async Task Create(Domain.Wallet wallet)
        {
            logger.LogInformation("Repository: Create wallet {wallet}", wallet);
            dataContext.Add(wallet);
            await dataContext.SaveChangesAsync();
        }

        public async Task<Domain.Wallet?> GetById(int id)
        {
            logger.LogInformation("Repo: GetById wallet {Id}", id);
            return await dataContext.Wallets.FindAsync(id);
        }

        public async Task<List<Domain.Wallet>> Filter(string? userDocument, Currency? currency)
        {
            logger.LogInformation("Repo: filter wallets. userDocument: {Doc}, Currency: {Currency}", userDocument, currency);
            var query = dataContext.Wallets.AsQueryable();

            if (!string.IsNullOrEmpty(userDocument))
                query = query.Where(w => w.UserDocument != null && w.UserDocument.Contains(userDocument));

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
