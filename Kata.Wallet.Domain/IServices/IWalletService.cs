using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain.IServices
{
    public interface IWalletService
    {
        Task SaveChangesAsync();
        Task Create(Wallet wallet);
        Task<Domain.Wallet?> GetById(int id);
        Task<List<Domain.Wallet>> Filter(WalletFilter walletFilter);

    }
}
