using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain.IRepositories
{
    public interface IWalletRepository
    {
        Task SaveChangesAsync();
        Task Create (Wallet wallet);
        Task<Wallet?> GetById(int id);
        Task<List<Wallet>> Filter(string? userDocument, Currency? currency);

    }
}
