using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Kata.Wallet.Domain.IServices;
using Kata.Wallet.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Services.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository walletRepository;
        private readonly ILogger<WalletService> logger;

        public WalletService(IWalletRepository walletRepository, ILogger<WalletService> logger)
        {
            this.walletRepository = walletRepository;
            this.logger = logger;
        }

        public async Task<Domain.Wallet?> GetById(int id)
        {
            logger.LogInformation("Service: GetById wallet {Id}", id);
            return await walletRepository.GetById(id);
        }
        public async Task Create(Domain.Wallet wallet)
        {
            logger.LogInformation("Service: Create wallet {wallet}", wallet);
            await walletRepository.Create(wallet);
        }

        public async Task<List<Domain.Wallet>> Filter(WalletFilter walletFilter)
        {
            logger.LogInformation("Service: Filter wallets {@Filter}", walletFilter);
            if (walletFilter == null)
            {
                logger.LogWarning("Service: WalletService.Filter. Params NULL");
                throw new ArgumentNullException(nameof(walletFilter));
            }

            var result = await walletRepository.Filter(
                userDocument: walletFilter.UserDocument,
                currency: walletFilter.Currency
            );

            if (!result.Any())
            {
                logger.LogInformation("Service: Not Found wallets: {@Filter}", walletFilter);
            }

            return result;
        }

        public async Task SaveChangesAsync()
        {
           await walletRepository.SaveChangesAsync();
        }
    }
}
