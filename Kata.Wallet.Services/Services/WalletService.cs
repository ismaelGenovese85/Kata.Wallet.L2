using Kata.Wallet.Domain;
using Kata.Wallet.Domain.IRepositories;
using Kata.Wallet.Domain.IServices;
using Kata.Wallet.Dtos;
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

        public WalletService(IWalletRepository walletRepository)
        {
            this.walletRepository = walletRepository;
        }

        public async Task<Domain.Wallet?> GetById(int id)
        {
            return await walletRepository.GetById(id);
        }
        public async Task Create(Domain.Wallet wallet)
        {
            await walletRepository.Create(wallet);
        }

        public async Task<List<Domain.Wallet>> Filter(WalletFilter walletFilter)
        {
            return await walletRepository.Filter(
                userDocument: walletFilter.UserDocument,
                currency: walletFilter.Currency
            );
        }

        public async Task SaveChangesAsync()
        {
           await walletRepository.SaveChangesAsync();
        }
    }
}
