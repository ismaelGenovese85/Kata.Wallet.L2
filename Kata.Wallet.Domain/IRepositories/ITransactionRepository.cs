using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kata.Wallet.Domain.IRepositories
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetById(int id);
        Task Create(Transaction transaction);
        Task<List<Transaction>> GetAllByWalletId(int walletId);
    }
}
